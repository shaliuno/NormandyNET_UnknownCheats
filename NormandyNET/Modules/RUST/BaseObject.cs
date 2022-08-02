using NormandyNET.Core;

namespace NormandyNET.Modules.RUST
{
    public class BaseObject
    {
        internal ulong address;

        internal void GetNext()
        {
            address = Memory.Read<ulong>(address + 0x8, false, false);
        }

        internal void GetThis()
        {
            address = Memory.Read<ulong>(address + 0x10, false, false);
        }
    }
}