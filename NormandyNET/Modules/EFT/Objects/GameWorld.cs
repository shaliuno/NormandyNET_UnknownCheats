using NormandyNET.Core;

namespace NormandyNET.Modules.EFT
{
    internal static class GameWorld
    {
        internal static ulong address;

        internal static LocalGameWorld GetLocalGameWorld()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                return default;
            }

            var q = Memory.ReadChain<ulong>(address, ModuleEFT.offsetsEFT.LocalGameWorldOffsets);
            
            return new LocalGameWorld(q);
        }
    }
}