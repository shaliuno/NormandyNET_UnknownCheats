using NormandyNET.Core;
using NormandyNET.Modules.EFT.Player;
using System;
using System.Collections.Generic;

namespace NormandyNET.Modules.EFT
{
    internal class PlayerBody
    {
        internal ulong address;

        private EntityPlayer entityPlayer;
        public Dictionary<Bone, BoneClass> bonesClassDict = new Dictionary<Bone, BoneClass>();

        internal PlayerBody(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_PlayerBody);
            GetPlayerBones();
        }

        internal void GetPlayerBonesOverlay()
        {
            if (entityPlayer.isLocalPlayer == true)
            {
                return;
            }

            if (ModuleEFT.radarForm.overlay.canRun)
            {
                if ((entityPlayer.playerType == PlayerTypeEFT.Player || entityPlayer.playerType == PlayerTypeEFT.BotHuman) && ModuleEFT.settingsForm.settingsJson.Overlay.Bones.Humans == false)
                {
                    return;
                }

                if ((entityPlayer.playerType == PlayerTypeEFT.Bot || entityPlayer.playerType == PlayerTypeEFT.BotElite) && ModuleEFT.settingsForm.settingsJson.Overlay.Bones.AI == false)
                {
                    return;
                }

                if (entityPlayer.wtsRender == false)
                {
                    return;
                }

                if (entityPlayer.distanceToFollowedPlayer > ModuleEFT.settingsForm.settingsJson.Overlay.Bones.DrawDistance)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            if (ModuleEFT.settingsForm.settingsJson.Overlay.Bones.HighDetail)
            {
                for (int i = 0; i < Sketelon.bonesESPhighDetail.Count; i++)
                {
                    if (bonesClassDict.TryGetValue(Sketelon.bonesESPhighDetail[i], out BoneClass boneClass))
                    {
                        boneClass.GetPosition();
                    }
                }
            }
            else
            {
                for (int i = 0; i < Sketelon.bonesESPlowDetail.Count; i++)
                {
                    if (bonesClassDict.TryGetValue(Sketelon.bonesESPlowDetail[i], out BoneClass boneClass))
                    {
                        boneClass.GetPosition();
                    }
                }
            }
        }

        internal void GetPlayerBones()
        {
            if (entityPlayer.isLocalPlayer == true)
            {
                return;
            }

            var skinnedMesh = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_PlayerBody_SkinnedMesh);
            var boneDict = Memory.Read<ulong>(skinnedMesh + ModuleEFT.offsetsEFT.SkinnedMesh_Bone_dict);
            var boneList = Memory.Read<ulong>(boneDict + ModuleEFT.offsetsEFT.SkinnedMesh_Bone_list);

            var boneMembers = Enum.GetValues(typeof(Bone));

            foreach (var bone in Enum.GetValues(typeof(Bone)))
            {
                if (bonesClassDict.TryGetValue((Bone)bone, out BoneClass boneClass) == false)
                {
                    var boneClassTmp = new BoneClass(
                        Memory.Read<ulong>(boneList + 0x20 + ((uint)(Bone)bone * 0x8))
                        );
                    bonesClassDict.Add((Bone)bone, boneClassTmp);
                }
            }

                    }
    }
}