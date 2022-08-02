using NormandyNET.Core;

namespace NormandyNET.Modules.EFT.Objects.Components
{
    internal class ChromaticAberration
    {
        private bool debugLog = false;

        internal ulong address;

        internal ChromaticAberration(ulong baseComponent)
        {
                                    address = Memory.Read<ulong>(baseComponent + ModuleEFT.offsetsEFT.Component_ThermalVision_ChromaticAberration);

                    }

        internal void RemoveAbberation()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                                return;
            }

            Memory.Write<float>(address + ModuleEFT.offsetsEFT.ChromaticAberration_Shift, 0);
        }
    }
}