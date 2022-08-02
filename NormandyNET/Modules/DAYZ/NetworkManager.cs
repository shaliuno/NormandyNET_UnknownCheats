using NormandyNET.Core;

namespace NormandyNET.Modules.DAYZ
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
                        networkManager = Memory.moduleBaseAddress + ModuleDAYZ.offsetsDAYZ.NetworkManager;
                    }

        internal static void GetNetworkClient()
        {
                        NetworkClient.networkClient = Memory.Read<ulong>(networkManager + ModuleDAYZ.offsetsDAYZ.NetworkClient, false, false);
                    }
    }
}