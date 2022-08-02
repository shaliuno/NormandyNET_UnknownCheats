using NormandyNET.Core;

namespace NormandyNET.Modules.ARMA
{
    internal class NetworkManager
    {
        internal static NetworkClient networkClient;
        internal static ulong networkManager;

        public NetworkManager()
        {
        }

        internal static void GetInstance()
        {
                        networkManager = Memory.Read<ulong>(Memory.moduleBaseAddress + ModuleARMA.offsetsARMA.NetworkManager, false, false);
                    }

        internal static void GetNetworkClient()
        {
                        NetworkClient.networkClient = Memory.Read<ulong>(networkManager + ModuleARMA.offsetsARMA.NetworkClient, false, false);

                    }
    }
}