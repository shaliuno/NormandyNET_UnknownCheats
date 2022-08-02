using NormandyNET.Core;

namespace NormandyNET.Modules.ARMA
{
    internal static class ArmaString
    {
        internal static string GetString(ulong cleanNamePtr)
        {
            int size = Memory.Read<int>(cleanNamePtr + 0x8);

            if (size <= 2 || size > 60)
            {
                return "";
            }

            var nameId = cleanNamePtr + 0x10;
            var text = CommonHelpers.GetStringFromMemory(nameId, false, size);

            return text;
        }
    }
}