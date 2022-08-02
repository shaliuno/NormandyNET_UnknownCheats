using NormandyNET.Core;

namespace NormandyNET.Modules.RUST
{
    internal class HeldItem
    {
        private ulong ent;
        private ulong bp;
        private ulong recoil_properties;
        private ulong primary_mag;
        internal string name;

        internal HeldItem(ulong _ent)
        {
            ent = _ent;
            name = GetItemName();
        }

        private string GetItemName()
        {
                        
            var info = Memory.Read<ulong>(ent + 0x20);
            
            var display_name = Memory.Read<ulong>(info + 0x20);
            

            var wide_name = CommonHelpers.GetStringFromMemory(display_name + 0x14, true);
            
            return wide_name;
        }
    }
}