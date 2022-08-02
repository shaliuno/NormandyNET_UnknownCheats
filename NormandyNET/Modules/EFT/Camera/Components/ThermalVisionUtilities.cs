using NormandyNET.Core;

namespace NormandyNET.Modules.EFT.Objects.Components
{
    internal class ThermalVisionUtilities
    {
        private bool debugLog = false;

        internal ulong address;
        private SelectablePalette? cache_CurrentRampPalette;
        private float? cache_DepthFade;

        public enum SelectablePalette
        {
            Fusion,
            Rainbow,
            WhiteHot,
            BlackHot
        }

        internal ThermalVisionUtilities(ulong baseComponent)
        {
                                    address = Memory.Read<ulong>(baseComponent + ModuleEFT.offsetsEFT.Component_ThermalVision_ThermalVisionUtilities);

            
            cache_CurrentRampPalette = (SelectablePalette)Memory.Read<int>(address + ModuleEFT.offsetsEFT.ThermalVisionUtilities_CurrentRampPalette);
            cache_DepthFade = Memory.Read<float>(address + ModuleEFT.offsetsEFT.ThermalVisionUtilities_DepthFade);

                                }

        internal void SetPalette(SelectablePalette selectedPalette)
        {
            if (Memory.IsValidPointer(address) == false)
            {
                                return;
            }

            Memory.Write<int>(address + ModuleEFT.offsetsEFT.ThermalVisionUtilities_CurrentRampPalette, (int)selectedPalette);
        }

        internal void Enable()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                                return;
            }

            Memory.Write<float>(address + ModuleEFT.offsetsEFT.ThermalVisionUtilities_DepthFade, 0);
        }

        internal void Disable()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                                return;
            }
        }
    }
}