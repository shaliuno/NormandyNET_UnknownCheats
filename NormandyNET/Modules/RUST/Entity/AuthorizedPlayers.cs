using NormandyNET.Core;
using System;
using System.Collections.Generic;

namespace NormandyNET.Modules.RUST.Entity
{
    internal class AuthorizedPlayers
    {
        private ulong address;
        internal List<PlayerNameID> authorizedPlayers;

        internal DateTime refreshTimeLast;
        internal int refreshTimeLastMinutes = 5;
        internal string protectedMinutes;

        internal AuthorizedPlayers(ulong _address)
        {
            address = _address;
            authorizedPlayers = new List<PlayerNameID>();

            var authorizedPlayersList = Memory.Read<ulong>(address + 0x10);
            var authorizedPlayersListCount = Memory.Read<int>(address + 0x18);

            for (uint i = 0; i < authorizedPlayersListCount; i++)
            {
                var playerNameIdAddress = Memory.Read<ulong>(authorizedPlayersList + 0x20 + (i * 0x8));

                if (Memory.IsValidPointer(playerNameIdAddress))
                {
                    var playerNameId = new PlayerNameID(playerNameIdAddress);
                    authorizedPlayers.Add(playerNameId);
                }
            }

            refreshTimeLast = CommonHelpers.dateTimeHolder.AddMinutes(refreshTimeLastMinutes);
        }

        internal string GetProtectedMinutes(ulong address)
        {
            var timeProtected = Memory.Read<float>(address + ModuleRUST.offsetsRUST.BuildingPrivlidgeCachedProtectedMinutes);

            TimeSpan t = TimeSpan.FromMinutes(timeProtected);

            string answer = string.Format("{0:D2}d {1:D2}h {2:D2}m",
                            t.Days,
                            t.Hours,
                            t.Minutes);
            return answer;
        }

        internal bool CanRefresh()
        {
            if (CommonHelpers.dateTimeHolder > refreshTimeLast)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}