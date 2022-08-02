using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class FastRunning
    {
        private EntityPlayer entityPlayer;
        internal static bool isSprinting;

        private DateTime TimeLast;
        private int TimeLastRateMs = 100;

        public FastRunning(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check()
        {
            if (entityPlayer.physical == null)
            {
                return;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastRunning.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);

                    if (IsSprinting())
                    {
                        isSprinting = true;
                    }
                    else
                    {
                        isSprinting = false;
                    }
                }
            }
        }

        internal bool IsSprinting()
        {
            if (entityPlayer.physical == null)
            {
                return false;
            }

            return entityPlayer.physical.IsSprinting();
        }
    }
}