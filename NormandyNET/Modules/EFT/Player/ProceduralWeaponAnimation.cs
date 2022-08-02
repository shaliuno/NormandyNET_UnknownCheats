using NormandyNET.Core;
using System;
using Transform = NormandyNET.Modules.EFT.Objects.Components.Transform;

namespace NormandyNET.Modules.EFT
{
    internal class ProceduralWeaponAnimation
    {
        internal ulong address;
        private ulong blindfireBlender;
        private EntityPlayer entityPlayer;
        internal ulong handsContainer;
        internal ulong firearmContoller;
        internal ulong fireportTransformBifacial;
        internal Transform fireportTransform;
        internal ulong breathEffector;
        internal ulong walkEffector;
        internal ulong motionEffector;
        internal ulong forceEffector;
        internal ulong shotEffector;
        internal ulong turnwayEffector;

        internal int ExtraInfoUpdateMSec = 5000;

        internal DateTime ExtraInfoUpdateTimeLast;
        private float weaponLnCached;

        public ProceduralWeaponAnimation(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation);

            blindfireBlender = Memory.Read<ulong>(address + 0xC8);

            handsContainer = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_HandsContainer);
            firearmContoller = GetFirearmContoller();
            breathEffector = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_BreathEffector);
            walkEffector = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_WalkEffector);
            motionEffector = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_MotionEffector);
            forceEffector = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_ForceEffector);
            shotEffector = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_ShotEffector);
            turnwayEffector = Memory.Read<ulong>(address + 0x50);

            fireportTransformBifacial = Memory.Read<ulong>(handsContainer + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_HandsContainer_FireportTransformBifacial);

            fireportTransform = new Transform(Memory.Read<ulong>(fireportTransformBifacial + 0x10));
        }

        internal ulong GetHandsContainer()
        {
            if (CommonHelpers.dateTimeHolder > ExtraInfoUpdateTimeLast)
            {
                ExtraInfoUpdateTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(1500);
                handsContainer = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_HandsContainer);
            }

            return handsContainer;
        }

        internal ulong GetFirearmContoller()
        {
            if (CommonHelpers.dateTimeHolder > ExtraInfoUpdateTimeLast)
            {
                ExtraInfoUpdateTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(5000);
                firearmContoller = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_FirearmContoller);
            }

            return firearmContoller;
        }

        internal bool IsAiming()
        {
            var isAimingValue = Memory.Read<byte>(GetFirearmContoller() + ModuleEFT.offsetsEFT.Player_FirearmController_IsAiming, false);

            if (isAimingValue == (byte)1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool IsSideFire()
        {
            var sideFire = Memory.Read<float>(blindfireBlender + 0x10);

            return sideFire == -1;
        }

        internal bool IsBlindFire()
        {
            var blindFire = Memory.Read<float>(blindfireBlender + 0x10);

            return blindFire == 1;
        }

        internal void WeaponLnDo(bool set)
        {
            if (set)
            {
                weaponLnCached = Memory.Read<float>(entityPlayer.proceduralWeaponAnimation.GetFirearmContoller() + ModuleEFT.offsetsEFT.Player_FirearmController_WeaponLn);

                if (weaponLnCached != 0.001f)
                {
                    Memory.Write(entityPlayer.proceduralWeaponAnimation.GetFirearmContoller() + ModuleEFT.offsetsEFT.Player_FirearmController_WeaponLn, 0.001f);
                }
            }
            else
            {
                Memory.Write(entityPlayer.proceduralWeaponAnimation.GetFirearmContoller() + ModuleEFT.offsetsEFT.Player_FirearmController_WeaponLn, weaponLnCached);
            }
        }
    }
}