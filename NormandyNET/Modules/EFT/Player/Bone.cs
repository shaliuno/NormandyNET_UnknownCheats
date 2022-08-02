using NormandyNET.Core;
using NormandyNET.Modules.EFT.Objects.Components;

namespace NormandyNET.Modules.EFT.Player
{
    internal class BoneClass
    {
        private ulong address;
        internal Transform transform;
        internal UnityEngine.Vector3 position;

        public BoneClass(ulong address)
        {
            this.address = address;
            transform = new Transform(Memory.Read<ulong>(address + 0x10));
        }

        internal UnityEngine.Vector3 GetPosition()
        {
            position = transform.GetPositionViaIndices();
            return position;
        }
    }
}