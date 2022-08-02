using NormandyNET.Core;
using NormandyNET.Modules.EFT.Improvements;

namespace NormandyNET.Modules.EFT.Objects
{
    internal static class TimeScale
    {
        internal static ulong address;
        internal static bool working;

       
        private static float safeValue = 1.8f;
        private static float valueReload = 4f;
        private static float defaultValue = 1f;

        internal static void Check()
        {
            if (FastRunning.isSprinting == true)
            {
                SetTimeScale(safeValue);
                return;
            }

            if (FastReload.isReloading == true && FastRunning.isSprinting == false)
            {
                SetTimeScale(safeValue);
                return;
            }

            ResetTimeScale();
        }

        internal static ulong GetTimeScale()
        {
            var _address = Memory.Read<ulong>(Memory.moduleBaseAddress + ModuleEFT.offsetsEFT.TimeScale + 7 * 8, false, false);
            address = _address;
            return _address;
        }

        internal static void SetTimeScale(float value)
        {
            Memory.Write<float>(address + ModuleEFT.offsetsEFT.TimeScale_Value, value);
            working = true;
        }

        internal static void ResetTimeScale()
        {
            var value = Memory.Read<float>(address + ModuleEFT.offsetsEFT.TimeScale_Value);

            if (value != defaultValue)
            {
                Memory.Write<float>(address + ModuleEFT.offsetsEFT.TimeScale_Value, defaultValue);
            }

            working = false;
        }

        internal static bool IsTimeScaling()
        {
            if (FastReload.isReloading == true || FastRunning.isSprinting == true)
            {
                return true;
            }

            return false;
        }

        internal static void DisableTimeScale()
        {
            FastReload.isReloading = false;
            FastRunning.isSprinting = false;
            ResetTimeScale();
        }
    }
}