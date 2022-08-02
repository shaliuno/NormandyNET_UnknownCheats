using NormandyNET.Core;

namespace NormandyNET.Modules.RUST.Entity
{
    internal class PlayerNameID
    {
        internal string nickname;
        internal ulong steamID;
        private ulong address;

        internal PlayerNameID(ulong _address)
        {
            address = _address;
            nickname = GetNickname();
            steamID = GetSteamID();
        }

        internal string GetNickname()
        {
            var stringPtr = Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.PlayerNameIDUsername);
            return CommonHelpers.GetStringFromMemory(stringPtr + 0x14, true, Memory.Read<int>(stringPtr + 0x10));
        }

        internal ulong GetSteamID()
        {
            return Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.PlayerNameIDSteamID);
        }
    }
}