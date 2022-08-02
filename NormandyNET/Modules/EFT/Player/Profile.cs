using NormandyNET.Core;
using System;

namespace NormandyNET.Modules.EFT
{
    internal class Profile
    {
        internal ulong address;
        internal string AccountID;
        internal float KillDeathRatio;
        private EntityPlayer entityPlayer;

        public Profile(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_Profile);
            GetStaticData();
        }

        private void GetStaticData()
        {
            AccountID = GetAccountID();
            KillDeathRatio = GetPlayerKillDeathRatio();
        }

        private string GetAccountID()
        {
            return CommonHelpers.GetStringFromMemory_Unity(Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_Profile_AccountID), true);
        }

        private float GetPlayerKillDeathRatio()
        {
            var stats = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_Profile_Stats);
            var overallCounters = Memory.Read<ulong>(stats + ModuleEFT.offsetsEFT.Player_Profile_Stats_OverallCounters);
            var counters = Memory.Read<ulong>(overallCounters + 0x10);
            var count = Memory.Read<uint>(counters + 0x40);

            if (count < 0 || count > 1000)
            {
                return 0f;
            }

            var entries = Memory.Read<ulong>(counters + 0x18);
            var keys = Memory.Read<ulong>(counters + 0x28);

            if (!Memory.IsValidPointer(entries))
            {
                return 0f;
            }

            ulong kills = 0, deaths = 0;

            var arrayBase = entries + 0x28;

            for (uint i = 0; i < count; i++)
            {
                var key = Memory.Read<ulong>(arrayBase + i * 0x18 + 0x18);

                var value = Memory.Read<ulong>(arrayBase + i * 0x18 + 0x20);

                var keySet = Memory.Read<ulong>(key + 0x10);
                var keySetSlots = Memory.Read<ulong>(keySet + 0x18);
                var setArrayBase = keySetSlots + 0x28;
                var first = Memory.Read<ulong>(setArrayBase);

                var statName = CommonHelpers.GetStringFromMemory_Unity(first, true);

                if (statName == ("Kills"))
                {
                    kills = value;
                }
                else if (statName == ("Deaths"))
                {
                    deaths = value;
                }
            }

            var result = (deaths == 0) ? (float)(kills) : (float)(kills) / (float)(deaths);

            return (float)Math.Round(result, 2);
        }
    }
}