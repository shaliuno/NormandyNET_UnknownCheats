using NormandyNET.Core;
using System.Collections.Generic;

namespace NormandyNET.Modules.DAYZ
{
    internal class NetworkClient
    {
        internal static ulong networkClient;
        internal static ulong scoreBoard;
        internal static uint playersCount;
        internal static string serverName = "N/A";
        internal static Dictionary<uint, ulong> playersList = new Dictionary<uint, ulong>();

        public NetworkClient()
        {
        }

        internal static void GetServerName()
        {
        }

        internal static void GetPlayersCount()
        {
                        playersCount = Memory.Read<uint>(networkClient + ModuleDAYZ.offsetsDAYZ.NetworkClient_ScoreBoard + 0x8);
                    }

        internal static void GetScoreBoard()
        {
                        scoreBoard = Memory.Read<ulong>(networkClient + ModuleDAYZ.offsetsDAYZ.NetworkClient_ScoreBoard);
                    }

        internal static void GetScoreboardList()
        {
            
            NetworkClient.playersList.Clear();
            for (uint i = 0; i < playersCount; i++)
            {
                
                var pIdentity = scoreBoard + i * ModuleDAYZ.offsetsDAYZ.NetworkClient_ScoreBoard_PlayerIdentitySize;
                
                var player_network_id = Memory.Read<uint>(pIdentity + ModuleDAYZ.offsetsDAYZ.PlayerIdentity_NetworkId);
                
                if (player_network_id <= 1)
                {
                    
                    continue;
                }

                if (playersList.ContainsKey(player_network_id) == false)
                {
                    playersList.Add(player_network_id, pIdentity);
                                    }
            }

                    }
    }

    internal struct PlayerIdentity
    {
        public ulong networkIdPtr;
        public ulong namePtr;
    }
}