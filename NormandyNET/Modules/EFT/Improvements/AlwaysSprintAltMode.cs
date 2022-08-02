using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class AlwaysSprintAltMode
    {
        private EntityPlayer entityPlayer;

        private DateTime TimeLast;
        private int TimeLastRateMs = 200;

        internal AlwaysSprintAltMode(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Enabled)
            {
                return;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprintAltMode.Enabled)
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
            entityPlayer.improvementsController.alwaysSprint.ResetPhysicalCondition();
        }
    }
}