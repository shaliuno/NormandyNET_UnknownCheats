using NormandyNET.Core;
using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class UnlimitedStamina
    {
        private EntityPlayer entityPlayer;

        private DateTime TimeLast;
        private int TimeLastRateMs = 2000;

        internal UnlimitedStamina(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.UnlimitedStamina.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                    Enable();
                }
            }
        }

        private void Enable()
        {
            var Current = Memory.Read<float>(entityPlayer.physical.stamina + ModuleEFT.offsetsEFT.Player_Physical_Stamina_Current);

            if (Current < 40)
            {
                Memory.Write<float>(entityPlayer.physical.stamina + ModuleEFT.offsetsEFT.Player_Physical_Stamina_Current, ModuleEFT.radarForm.fastRandom.Next(75, 90));
            }
        }
    }
}