using NormandyNET.Core;

namespace NormandyNET.Modules.EFT.Objects.Components
{
    internal class NightVision : GameObjectComponent
    {
        private bool debugLog = false;
        private bool enabled;

        public NightVision(object[] args = null) : base(args)
        {
                        
                    }

        internal void Enable()
        {
            if (enabled)
            {
                return;
            }

            if (Memory.IsValidPointer(componentFields) == false)
            {
                                return;
            }

            Memory.Write<byte>(componentFields + ModuleEFT.offsetsEFT.Component_NightVision_On, (byte)1);
            Memory.Write<float>(componentFields + ModuleEFT.offsetsEFT.Component_NightVision_Intensity, 0);

            Memory.Write<float>(componentFields + ModuleEFT.offsetsEFT.Component_NightVision_NoiseIntensity, 0);

                        enabled = true;
        }

        internal void Disable()
        {
            if (enabled == false)
            {
                return;
            }

            Memory.Write<byte>(componentFields + ModuleEFT.offsetsEFT.Component_NightVision_On, (byte)0);
            enabled = false;
        }
    }
}