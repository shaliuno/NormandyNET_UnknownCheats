using NormandyNET.Core;

namespace NormandyNET.Modules.EFT.Objects.Components
{
    internal abstract class GameObjectComponent
    {
        internal ulong component;
        internal ulong componentFields;
        internal ulong componentStaticFields;
        internal ulong classDetails;
        internal ulong pName;
        internal string className;
        private bool debugLog = false;

        public GameObjectComponent(object[] args)
        {
            component = (ulong)args[0];
            componentFields = (ulong)args[1];
            componentStaticFields = (ulong)args[2];
            classDetails = (ulong)args[3];
            pName = (ulong)args[4];
            className = (string)args[5];

                                                                                            }

        public GameObjectComponent(ulong address)
        {
            this.component = address;
        }

        internal void GetComponentData()
        {
            if (Memory.IsValidPointer(component) == false)
            {
                                return;
            }

            componentFields = Memory.Read<ulong>(component + ModuleEFT.offsetsEFT.GameObject_ComponentsFields);
            
            if (!Memory.IsValidPointer(componentFields))
            {
                return;
            }

            var componentStaticFields = Memory.Read<ulong>(componentFields + 0x0);
            
            var classDetails = Memory.Read<ulong>(componentStaticFields + 0x0);
            
            var pName = Memory.Read<ulong>(classDetails + 0x48);
            
            var className = CommonHelpers.GetStringFromMemory(pName, false, 128);

                    }
    }
}