using NormandyNET.Core;
using NormandyNET.Modules.EFT.Objects.Components;
using System;

namespace NormandyNET.Modules.EFT
{
    internal class GameObject
    {
        internal ulong address;
        internal ulong componentsList;
        internal string name;
        private bool debugLog = false;
        internal uint foundInCurObject = 0;

        public GameObject(ulong x_address = 0)
        {
            address = x_address;
            componentsList = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.GameObject_ComponentsList);
        }

        internal string GetName()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                return "n/a";
            }

            var pName = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.GameObjectBasicName);
            
            if (Memory.IsValidPointer(pName) == false)
            {
                return "n/a";
            }

            var objectName = CommonHelpers.GetStringFromMemory(pName, false, 128);
            name = objectName;
            
            return objectName;
        }

        public UnknownComponent GetComponentByName(string name)
        {
            return GetComponentByName<UnknownComponent>(name);
        }

        public T GetComponentByName<T>(string name)
        {
            
            for (uint i = 0x8; i < 0x600; i += 0x10)
            {
                var component = Memory.Read<ulong>(componentsList + i);
                
                if (Memory.IsValidPointer(component) == false)
                {
                    
                    continue;
                }

                var componentFields = Memory.Read<ulong>(component + ModuleEFT.offsetsEFT.GameObject_ComponentsFields);
                
                if (Memory.IsValidPointer(componentFields) == false)
                {
                                        continue;
                }

                var componentStaticFields = Memory.Read<ulong>(componentFields + 0x0);
                
                var classDetails = Memory.Read<ulong>(componentStaticFields + 0x0);
                
                var pName = Memory.Read<ulong>(classDetails + 0x48);
                
                var className = CommonHelpers.GetStringFromMemory(pName, false, 128);

                
                if (string.Compare(className, name, true) == 0)
                {
                    
                    return (T)Activator.CreateInstance(typeof(T), new object[] { new object[] { component, componentFields, componentStaticFields, classDetails, pName, className } });
                }

                if (className.Equals("n/a") || Memory.IsValidPointer(component) == false)
                {
                    return default;
                }
            }
            return default;
        }
    }
}