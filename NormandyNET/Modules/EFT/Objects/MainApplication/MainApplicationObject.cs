using NormandyNET.Core;
using NormandyNET.Modules.EFT.Improvements;
using NormandyNET.Modules.EFT.Objects.Components;

namespace NormandyNET.Modules.EFT
{
    internal class MainApplicationObject : GameObject
    {
        private bool debugLog = true;
        internal MainApplicationComponent mainApplicationComponent;
        internal NoInertia noInertia;

        internal MainApplicationObject(GameObject gameObject)
        {
            address = gameObject.address;
            componentsList = gameObject.componentsList;
            mainApplicationComponent = GetMainApplicationComponent();
        }

        

        internal NoInertia GetInertia()
        {
            if (!Memory.IsValidPointer(mainApplicationComponent.componentFields))
            {
                return default;
            }

            var chainToInertia = Memory.ReadChain<ulong>(mainApplicationComponent.componentFields, new uint[] {
                ModuleEFT.offsetsEFT.MainApplication_Backend_Interface,
                ModuleEFT.offsetsEFT.MainApplication_Backend_Class,
                ModuleEFT.offsetsEFT.MainApplication_Backend_Class_BackEndConfig,
                ModuleEFT.offsetsEFT.MainApplication_Backend_Class_BackEndConfig_Config,
                ModuleEFT.offsetsEFT.MainApplication_Backend_Class_BackEndConfig_Config_Inertia });

            if (Memory.IsValidPointer(chainToInertia))
            {
                return new NoInertia(chainToInertia);
            }
            else
            {
                return default;
            }
        }

        internal MainApplicationComponent GetMainApplicationComponent()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                return default;
            }

            var result = GetComponentByName<MainApplicationComponent>("MainApplication");

            if (result != null && Memory.IsValidPointer(result.component) == false)
            {
                return default;
            }

            return result;
        }
    }
}