using NormandyNET.Core;
using System.Runtime.InteropServices;

namespace NormandyNET.Modules.EFT
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct BaseObject
    {
        internal ulong baseObjectAddress;

        internal BaseObject GetNext()
        {
            if (Memory.IsValidPointer(baseObjectAddress) == false)
            {
                return default;
            }

            return Memory.Read<BaseObject>(baseObjectAddress + 0x8);
        }

        internal GameObject GetThis()
        {
            if (Memory.IsValidPointer(baseObjectAddress) == false)
            {
                return default;
            }

            return new GameObject(Memory.Read<ulong>(baseObjectAddress + 0x10));
        }
    }
}