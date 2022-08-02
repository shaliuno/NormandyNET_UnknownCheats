using NormandyNET.Core;

namespace NormandyNET.Modules.EFT.Objects.Components
{
    internal class ThermalVision : GameObjectComponent
    {
        internal ThermalVisionUtilities ThermalVisionUtilities;
        internal ChromaticAberration ChromaticAberration;

        private bool? cache_On;
        private bool? cache_IsNoisy;
        private bool? cache_IsFpsStuck;
        private bool? cache_IsMotionBlurred;
        private bool? cache_IsGlitch;
        private bool? cache_IsPixelated;
        private bool enabled;

        private bool debugLog = false;

        public ThermalVision(object[] args = null) : base(args)
        {
                        
            ThermalVisionUtilities = new ThermalVisionUtilities(componentFields);
            ChromaticAberration = new ChromaticAberration(componentFields);
                    }

        internal void RemoveVisualArtifacts()
        {
            if (Memory.IsValidPointer(componentFields) == false)
            {
                                return;
            }

            Memory.Write<float>(componentFields + ModuleEFT.offsetsEFT.Component_ThermalVision_UnsharpRadiusBlur, 0);
            Memory.Write<float>(componentFields + ModuleEFT.offsetsEFT.Component_ThermalVision_UnsharpBias, 0);

            ChromaticAberration?.RemoveAbberation();
        }

        internal void EnableThermal()
        {
            if (enabled)
            {
                return;
            }

            if (Memory.IsValidPointer(componentFields) == false)
            {
                                return;
            }

            ThermalVisionUtilities?.Enable();

            var data = new byte[6] { 0x1, 0x0, 0x0, 0x0, 0x0, 0x0 };
            Memory.WriteBytes(componentFields + ModuleEFT.offsetsEFT.Component_ThermalVision_On, ref data);

            RemoveVisualArtifacts();
                        enabled = true;
        }

        internal void DisableThermal()
        {
            if (enabled == false)
            {
                return;
            }

            if (Memory.IsValidPointer(componentFields) == false)
            {
                                return;
            }

            Memory.Write<byte>(componentFields + ModuleEFT.offsetsEFT.Component_ThermalVision_On, (byte)0);

                        enabled = false;
        }
    }
}