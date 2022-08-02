using NormandyNET.Core;
using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class FastReload
    {
        private EntityPlayer entityPlayer;
        private string reloadClassName = string.Empty;
        private string notReloadClassName = string.Empty;
        private bool reloadClassNameConfirmed;
        internal static bool isReloading;

        private DateTime TimeLast;
        private int TimeLastRateMs = 125;
        private bool disabled = false;

        internal FastReload(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
            LoadSettings();
            isReloading = false;
        }

        internal void LoadSettings()
        {
            if (IsSameProcess())
            {
                reloadClassName = ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastReload.ClassName;
                reloadClassNameConfirmed = true;
            }
        }

        internal void StoreSettings()
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastReload.ProcessID = ModuleEFT.radarForm.settingsRadar.Network.lastGameProcessID;
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastReload.ImageBase = Memory.moduleBaseAddress;
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastReload.ClassName = reloadClassName;
        }

        internal void Check()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastReload.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                    Enable();
                    disabled = false;
                }
            }
            else
            {
                if (disabled == false)
                {
                    Disable();
                    disabled = true;
                }
            }
        }

        internal static bool IsSameProcess()
        {
            if (
              ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastReload.ProcessID == ModuleEFT.radarForm.settingsRadar.Network.lastGameProcessID &&
              ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastReload.ImageBase == Memory.moduleBaseAddress)
            {
                return true;
            }

            return false;
        }

        internal void Reset()
        {
            reloadClassName = string.Empty;
            notReloadClassName = string.Empty;
            reloadClassNameConfirmed = false;
        }

        internal bool Enable()
        {
            var handsController = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_HandsController);

            var currentOperation = Memory.Read<ulong>(handsController + ModuleEFT.offsetsEFT.Player_HandsController_CurrentOperation);

            var currentOperationStr = CommonHelpers.ByteArrayToString(Memory.ReadBytes(currentOperation, 5));

            if (notReloadClassName == string.Empty)
            {
                notReloadClassName = currentOperationStr;
            }

            if (currentOperationStr.Contains(notReloadClassName))
            {
                isReloading = false;
                return false;
            }

            if (reloadClassName == string.Empty && !currentOperationStr.Contains(notReloadClassName))
            {
                reloadClassName = currentOperationStr;
                reloadClassNameConfirmed = true;
            }

            if (reloadClassNameConfirmed && currentOperationStr.Contains(reloadClassName))
            {
                isReloading = true;
                StoreSettings();
                return true;
            }

            return false;
        }

        internal void Disable()
        {
            isReloading = false;
            Reset();
        }

        internal bool ShowOSDWarning(out string text)
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastReload.Enabled)
            {
                if (!reloadClassNameConfirmed)
                {
                    text = "FastReload (TimeScale)\nPlease reload once.";
                    return true;
                }
                else
                {
                    text = "";
                    return false;
                }
            }

            text = "";
            return false;
        }
    }
}