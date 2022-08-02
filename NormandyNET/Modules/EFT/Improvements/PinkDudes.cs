using NormandyNET.Core;
using NormandyNET.Modules.EFT.Objects;
using System;
using System.Collections.Generic;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class PinkDudes
    {
        private EntityPlayer entityPlayer;
        internal ulong address;
        private bool cached;
        internal bool chamsReapply;
        internal bool isPink;
        internal bool onScreen;
        internal int state = 0;

        private List<RendererEntry> rendererEntries;
        private DateTime TimeLast;
        private int TimeLastRateMs = 1500;

        internal static bool emergencyDepink = false;

        internal PinkDudes(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_PlayerBody);
            rendererEntries = new List<RendererEntry>();
        }

        private enum Chams
        {
            Skins, Gear
        }

        internal void Check()
        {
            if (PinkDudes.emergencyDepink)
            {
                return;
            }

            if (TimeScale.IsTimeScaling())
            {
                return;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.PinkDudes.Enabled)
            {
                if (float.IsInfinity(entityPlayer.distanceToFollowedPlayer) || float.IsNaN(entityPlayer.distanceToFollowedPlayer))
                {
                    return;
                }

                if (entityPlayer.distanceToFollowedPlayer <= 1 || entityPlayer.distanceToFollowedPlayer >= 2000)
                {
                    return;
                }

                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs + entityPlayer.distanceToFollowedPlayer * 2);
                    Enable();
                }
            }
        }

        private void Enable()
        {
            if (entityPlayer.isTeammate == false)
            {
                if (entityPlayer.entityInSightArc && entityPlayer.distanceToFollowedPlayer < 300)
                {
                    chamsReapply = true;
                }

                if (cached)
                {
                    if (entityPlayer.localEntity.isAtExfilPoint)
                    {
                        if (isPink)
                        {
                            NullRendererCachedRevert();
                        }

                        return;
                    }

                    if (!isPink || chamsReapply)

                    {
                        NullRendererCached();

                        chamsReapply = false;
                        return;
                    }

                    return;
                }

                rendererEntries.Clear();

                WriteSkinChams();
                cached = true;
            }
        }

        private class RendererEntry
        {
            internal int materialCount;
            internal ulong address;
            internal byte[] originalMaterialData;

            internal RendererEntry(ulong address, int materialCount)
            {
                this.address = address;
                this.materialCount = materialCount;
                originalMaterialData = new byte[sizeof(uint) * materialCount];
                originalMaterialData = Memory.ReadBytes(address, originalMaterialData.Length);
            }

            internal bool Wipe(bool doWipe)
            {
                var test = Memory.ReadBytes(address, sizeof(uint) * materialCount);
                uint[] decoded = new uint[test.Length / 4];
                Buffer.BlockCopy(test, 0, decoded, 0, test.Length);

                bool doWipeForReal = false;

                for (int i = 0; i < decoded.Length; i++)
                {
                    if (test[i] != 0)
                    {
                        doWipeForReal = true;
                    }
                }

                if (doWipe)
                {
                    if (doWipeForReal)
                    {
                        var nullVec = new byte[sizeof(uint) * materialCount];
                        Memory.WriteBytes(address, ref nullVec);
                        return doWipe;
                    }
                    return doWipe;
                }
                else
                {
                    Memory.WriteBytes(address, ref originalMaterialData);
                    return doWipe;
                }
            }
        }

        internal void ForceDePink()
        {
            if (cached)
            {
                NullRendererCachedRevert();
            }
        }

        private void NullRendererCached()
        {
            isPink = true;

            for (int i = 0; i < rendererEntries.Count; i++)
            {
                if (rendererEntries[i].Wipe(true))
                {
                }
            }
        }

        private void NullRendererCachedRevert()
        {
            isPink = false;

            for (int i = 0; i < rendererEntries.Count; i++)
            {
                if (rendererEntries[i].Wipe(false))
                {
                }
            }
        }

        private bool NullRenderer(ulong pMaterialDictionary, Chams chams)
        {
            if (!Memory.IsValidPointer(pMaterialDictionary))
            {
                return false;
            }

            var materialCount = Memory.Read<int>(pMaterialDictionary + 0x158);

            if (materialCount != 1)
            {
                return false;
            }

            if (chams == Chams.Gear)
            {
                Memory.Write<int>(pMaterialDictionary + 0x158, 0);
                return true;
            }

            var rendererEntry = Memory.Read<ulong>(pMaterialDictionary + 0x148);

            if (!Memory.IsValidPointer(rendererEntry))
            {
                return false;
            }

            var rendererEntryClass = new RendererEntry(rendererEntry, materialCount);

            if (!rendererEntries.Contains(rendererEntryClass))
            {
                rendererEntries.Add(rendererEntryClass);
            }

            rendererEntryClass.Wipe(true);
            isPink = true;

            return true;
        }

        internal void WriteGearChams()
        {
            var slotViews = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_PlayerBody_SlotsView);

            if (!Memory.IsValidPointer(slotViews))
            {
                return;
            }

            var slotViewsList = Memory.Read<ulong>(slotViews + 0x18);

            if (!Memory.IsValidPointer(slotViewsList))
            {
                return;
            }

            var pList = Memory.Read<ulong>(slotViewsList + 0x10);

            var slotViewsListSize = Memory.Read<uint>(slotViewsList + 0x18);

            for (uint i = 0; i < slotViewsListSize; i++)
            {
                var pEntry = Memory.Read<ulong>(pList + 0x20 + (0x8 * i));

                if (!Memory.IsValidPointer(pEntry))
                {
                    continue;
                }

                var dressesArray = Memory.Read<ulong>(pEntry + 0x40);

                if (!Memory.IsValidPointer(dressesArray))
                {
                    continue;
                }

                var dressesArraySize = Memory.Read<uint>(dressesArray + 0x18);

                for (uint j = 0; j < dressesArraySize; j++)
                {
                    var dressesEntry = Memory.Read<ulong>(dressesArray + 0x20 + (0x8 * j));

                    if (!Memory.IsValidPointer(dressesEntry))
                    {
                        continue;
                    }

                    var rendererArray = Memory.Read<ulong>(dressesEntry + 0x28);

                    if (!Memory.IsValidPointer(rendererArray))
                    {
                        continue;
                    }

                    var rendererArraySize = Memory.Read<uint>(rendererArray + 0x18);

                    for (uint k = 0; k < rendererArraySize; k++)
                    {
                        var rendererEntry = Memory.Read<ulong>(rendererArray + 0x20 + (0x8 * k));

                        if (!Memory.IsValidPointer(rendererEntry))
                        {
                            continue;
                        }

                        var pMaterialDict = Memory.Read<ulong>(rendererEntry + 0x10);

                        NullRenderer(pMaterialDict, Chams.Gear);
                    }
                }
            }
        }

        internal void WriteSkinChams()
        {
            var pSkinsDict = Memory.Read<ulong>(address + 0x38);

            if (!Memory.IsValidPointer(pSkinsDict))
            {
                return;
            }

            var skinsCount = Memory.Read<uint>(pSkinsDict + 0x40);

            if (skinsCount == 0 || skinsCount > 10000)
            {
                return;
            }

            var skinEntries = Memory.Read<ulong>(pSkinsDict + 0x18);

            for (uint i = 0; i < skinsCount; i++)
            {
                var pBodySkins = Memory.Read<ulong>(skinEntries + 0x30 + (0x18 * i));

                if (!Memory.IsValidPointer(pBodySkins))
                {
                    continue;
                }

                var pLodsArray = Memory.Read<ulong>(pBodySkins + 0x18);

                if (!Memory.IsValidPointer(pLodsArray))
                {
                    continue;
                }

                var lodsCount = Memory.Read<uint>(pLodsArray + 0x18);

                if (lodsCount > 10)
                {
                    continue;
                }

                for (uint j = 0; j < lodsCount; j++)
                {
                    var pLodEntry = Memory.Read<ulong>(pLodsArray + 0x20 + (j * 0x8));

                    var skinnedMeshRenderer = Memory.Read<ulong>(pLodEntry + 0x20);

                    if (!Memory.IsValidPointer(skinnedMeshRenderer))
                    {
                        continue;
                    }

                    var pMaterialDictionary = Memory.Read<ulong>(skinnedMeshRenderer + 0x10);

                    if (!Memory.IsValidPointer(pMaterialDictionary) || !NullRenderer(pMaterialDictionary, Chams.Skins))
                    {
                        skinnedMeshRenderer = Memory.Read<ulong>(Memory.Read<ulong>(pLodEntry + 0x20) + 0x20);
                        pMaterialDictionary = Memory.Read<ulong>(skinnedMeshRenderer + 0x10);
                        NullRenderer(pMaterialDictionary, Chams.Skins);
                    }
                }
            }
        }
    }
}