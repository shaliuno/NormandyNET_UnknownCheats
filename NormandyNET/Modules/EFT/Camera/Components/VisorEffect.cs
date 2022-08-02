using NormandyNET.Core;

namespace NormandyNET.Modules.EFT.Objects.Components
{
    internal class VisorEffect : GameObjectComponent
    {
        public VisorEffect(object[] args = null) : base(args)
        {
                                }

        internal void DisableVisorEffects()
        {
            if (Memory.IsValidPointer(componentFields) == false)
            {
                                return;
            }

            
            Memory.Write<float>(componentFields + ModuleEFT.offsetsEFT.Component_VisorEffect_Intensity, 0f);

                    }
    }
}