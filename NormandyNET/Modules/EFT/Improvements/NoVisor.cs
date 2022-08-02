using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class NoVisor
    {
        private EntityPlayer entityPlayer;

        private DateTime TimeLast;
        private int TimeLastRateMs = 5000;

        internal NoVisor(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check()
        {
            if (CommonHelpers.dateTimeHolder > TimeLast)
            {
                TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                Enable();
            }
        }

        private static void Enable()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.NoVisor.Enabled)
            {
                ModuleEFT.readerEFT.fpsCamera?.DisableVisorEffects();
            }
        }
    }
}