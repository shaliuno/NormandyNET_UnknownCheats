using NormandyNET.Core;
using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class LootThroughWalls
    {
        private EntityPlayer entityPlayer;
        private bool done;

        private DateTime TimeLast;
        private int TimeLastRateMs = 75;

        internal LootThroughWalls(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.LootThroughWalls.Enabled)
            {
                if (done == false)
                {
                    Enable();
                    done = true;
                }
            }
            else
            {
                if (done == true)
                {
                    Disable();
                    done = false;
                }
            }
        }

        internal void Enable()
        {
            entityPlayer.proceduralWeaponAnimation.WeaponLnDo(true);
            Memory.Write<float>(entityPlayer.proceduralWeaponAnimation.address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_FovCompensatoryDistance, ModuleEFT.settingsForm.settingsJson.MemoryWriting.LootThroughWalls.Distance);
        }

        internal void Disable()
        {
            entityPlayer.proceduralWeaponAnimation.WeaponLnDo(false);
            Memory.Write<float>(entityPlayer.proceduralWeaponAnimation.address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_FovCompensatoryDistance, 0f);
        }
    }
}