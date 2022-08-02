using NormandyNET.Core;
using NormandyNET.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NormandyNET.Modules.EFT.Exfiltration
{
    internal class ExfiltrationController
    {
        internal ulong address;

        internal ulong exfiltrationPointArrayAddressPMC;
        internal ulong exfiltrationPointArrayAddressSCAV;

        internal List<ExfiltrationPoint> exfiltrationPointPMC;
        internal List<ExfiltrationPoint> exfiltrationPointSCAV;
        private LocalGameWorld localGameWorld;

        private static DateTime ExfilTime = CommonHelpers.dateTimeHolder;
        private readonly int ExfilTimeRateSec = 15;

        public enum EType
        {
            PMC,
            SCAV
        }

        public ExfiltrationController(LocalGameWorld localGameWorld)
        {
            this.localGameWorld = localGameWorld;
            address = Memory.Read<ulong>(localGameWorld.address + ModuleEFT.offsetsEFT.LocalGameWorld_ExfiltrationController);
            exfiltrationPointSCAV = new List<ExfiltrationPoint>();
            exfiltrationPointPMC = new List<ExfiltrationPoint>();
        }

        internal void Cleanup()
        {
            exfiltrationPointArrayAddressPMC = 0;
            exfiltrationPointArrayAddressSCAV = 0;
            exfiltrationPointPMC.Clear();
            exfiltrationPointSCAV.Clear();
        }

        internal void FindExfilPoints()
        {
            if (ModuleEFT.readerEFT.GameWorldValid() == false)
            {
                                                return;
            }

            if (CommonHelpers.dateTimeHolder > ExfilTime)
            {
                ExfilTime = CommonHelpers.dateTimeHolder.AddSeconds(ExfilTimeRateSec);
            }
            else
            {
                if (ModuleEFT.radarForm.mapManager.CurrentMap.Length != 0)
                {
                    return;
                }
            }

            address = Memory.Read<ulong>(localGameWorld.address + ModuleEFT.offsetsEFT.LocalGameWorld_ExfiltrationController);

            if (address == 0)
            {
                ModuleEFT.readerEFT.UpdateBasePointers = true;
                return;
            }

            GetExfiltrationPoints();
        }

        internal bool PlayerInExfilCircle(Vector3 playersPosition, EPlayerSide side, out string exfilName)
        {
            if (side != EPlayerSide.Savage)
            {
                for (int i = 0; i < exfiltrationPointPMC.Count; i++)
                {
                    if (Geometry.GetDistance(playersPosition, exfiltrationPointPMC[i].position) < 10)
                    {
                        exfilName = exfiltrationPointPMC[i].name;
                        return true;
                    }
                }

                exfilName = string.Empty;
                return false;
            }
            else
            {
                for (int i = 0; i < exfiltrationPointPMC.Count; i++)
                {
                    if (Geometry.GetDistance(playersPosition, exfiltrationPointPMC[i].position) < 10)
                    {
                        exfilName = exfiltrationPointPMC[i].name;
                        return true;
                    }
                }

                for (int i = 0; i < exfiltrationPointSCAV.Count; i++)
                {
                    if (Geometry.GetDistance(playersPosition, exfiltrationPointSCAV[i].position) < 10)
                    {
                        exfilName = exfiltrationPointSCAV[i].name;

                        return true;
                    }
                }

                exfilName = string.Empty;
                return false;
            }
        }

        private void GetExfiltrationPoints()
        {
            bool success = false;
            string mapName = string.Empty;

            for (uint i = 0; i < GetExtrationPointsCountPMC(); i++)
            {
                var exfilPointAddr = Memory.Read<ulong>(exfiltrationPointArrayAddressPMC + 0x20 + (i * 0x8));
                
                var exfilPointSettingsPtr = Memory.Read<ulong>(exfilPointAddr + ModuleEFT.offsetsEFT.ExfiltrationPoint_ExitTriggerSettings);
                
                var exfilPointNamePtr = Memory.Read<ulong>(exfilPointSettingsPtr + 0x10);
                
                var exfilPointName = CommonHelpers.GetStringFromMemory_Unity(exfilPointNamePtr, true);

                var indexCache = exfiltrationPointPMC.FindIndex(t => t.address == exfilPointAddr);

                if (indexCache < 0)
                {
                    exfiltrationPointPMC.Add(new ExfiltrationPoint(exfilPointAddr, exfilPointName));
                }
                else
                {
                    exfiltrationPointPMC[indexCache].Update();
                }

                if (MapManager.exfilToMap.TryGetValue(exfilPointName, out string mapNameOut))
                {
                    mapName = mapNameOut;
                                        success = true;
                }
            }

            for (uint i = 0; i < GetExtrationPointsCountSCAV(); i++)
            {
                var exfilPointAddr = Memory.Read<ulong>(exfiltrationPointArrayAddressSCAV + 0x20 + (i * 0x8));
                
                var exfilPointSettingsPtr = Memory.Read<ulong>(exfilPointAddr + ModuleEFT.offsetsEFT.ExfiltrationPoint_ExitTriggerSettings);
                
                var exfilPointNamePtr = Memory.Read<ulong>(exfilPointSettingsPtr + 0x10);
                
                var exfilPointName = CommonHelpers.GetStringFromMemory_Unity(exfilPointNamePtr, true);

                var indexCache = exfiltrationPointSCAV.FindIndex(t => t.address == exfilPointAddr);

                if (indexCache < 0)
                {
                    exfiltrationPointSCAV.Add(new ExfiltrationPoint(exfilPointAddr, exfilPointName));
                }
                else
                {
                    exfiltrationPointSCAV[indexCache].Update();
                }
            }

            if (success && (ModuleEFT.radarForm.mapManager.CurrentMap.Length == 0 || ModuleEFT.radarForm.mapManager.CurrentMap != mapName))
            {
                ModuleEFT.radarForm.mapManager.CurrentMap = mapName;
                ModuleEFT.radarForm.reloadMap = true;
            }
        }

        internal uint GetExtrationPointsCountPMC()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                return 0;
            }

            exfiltrationPointArrayAddressPMC = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.ExfiltrationController_ExfiltrationPoint);

            if (Memory.IsValidPointer(exfiltrationPointArrayAddressPMC) == false)
            {
                return 0;
            }

            return Memory.Read<uint>(exfiltrationPointArrayAddressPMC + ModuleEFT.offsetsEFT.ListClassEntryCount);
        }

        internal uint GetExtrationPointsCountSCAV()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                return 0;
            }

            exfiltrationPointArrayAddressSCAV = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.ExfiltrationController_ScavExfiltrationPoint);

            if (Memory.IsValidPointer(exfiltrationPointArrayAddressSCAV) == false)
            {
                return 0;
            }

            return Memory.Read<uint>(exfiltrationPointArrayAddressSCAV + ModuleEFT.offsetsEFT.ListClassEntryCount);
        }

        internal static ExfiltrationPoint GetExfiltrationPoint()
        {
            return default;
        }
    }
}