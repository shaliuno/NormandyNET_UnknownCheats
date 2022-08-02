using NormandyNET.Core;
using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class InstantADS
    {
        private EntityPlayer entityPlayer;
        private DateTime TimeLast;
        private DateTime ExtrasTimeLast;
        private int TimeLastRateMs = 300;

        public InstantADS(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.InstantADS.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                    Enable();
                }
            }
        }

        internal void Enable()
        {
            if (CommonHelpers.dateTimeHolder > ExtrasTimeLast)
            {
                ExtrasTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);

                InstantADSDo();
            }
        }

        internal void InstantADSDo()
        {
            var value = Memory.Read<float>(entityPlayer.proceduralWeaponAnimation.address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_FastADS);

            if (value != 10f)
            {
                Memory.Write(entityPlayer.proceduralWeaponAnimation.address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_FastADS, 10f);
            }

            var AimSwayUnk0 = Memory.Read<float>(entityPlayer.proceduralWeaponAnimation.address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_AimSwayUnk0);

            if (AimSwayUnk0 != 0f)
            {
                Memory.Write<float>(entityPlayer.proceduralWeaponAnimation.address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_AimSwayUnk0, 0f);
            }
        }

        internal void InstantADSUndo()
        {
            Memory.Write(entityPlayer.proceduralWeaponAnimation.address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_FastADS, 1.032445f);
        }
    }
}