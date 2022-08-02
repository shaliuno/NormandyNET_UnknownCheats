using NormandyNET.Core;

namespace NormandyNET.Modules.RUST
{
    internal class Pointers
    {
        internal static ulong UnityModule;
        internal static ulong GameAssembly;
        internal static ulong GameObjectManager;
        internal static ulong BaseNetworkable;
        internal static ulong BasePlayer;
        internal static ulong SkyDome;
        internal static bool canReadData;
        internal static ulong EntityListAll;
        internal static ulong EntityListPlayersOnly;
        internal static int EntityCountAll;
        internal static int EntityCountPlayersOnly;
        internal static int PlayersCount;
        internal static ulong ClientEntitiesAll;
        internal static ulong ClientEntitiesPlayersOnly;

        internal static void CleanUp()
        {
            UnityModule = 0;
            GameAssembly = 0;
            GameObjectManager = 0;
            BaseNetworkable = 0;
            BasePlayer = 0;
            Camera.MainCamera = 0;
            canReadData = false;
            EntityListAll = 0;
            EntityListPlayersOnly = 0;
            EntityCountAll = 0;
            EntityCountPlayersOnly = 0;
        }

        internal static void GetModules(bool usermode)
        {
            
            if (usermode)
            {
                Pointers.UnityModule = SynchronousSocketClient.ProcessOpen(ModuleRUST.radarForm.settingsRadar.Network.lastGameProcessID, "UnityPlayer.dll");
                Pointers.GameAssembly = SynchronousSocketClient.ProcessOpen(ModuleRUST.radarForm.settingsRadar.Network.lastGameProcessID, "GameAssembly.dll");
            }
            else
            {
                Pointers.UnityModule = SynchronousSocketDriverClient.GetModuleBase(ModuleRUST.radarForm.settingsRadar.Network.lastGameProcessID, "UnityPlayer.dll");
                Pointers.GameAssembly = SynchronousSocketDriverClient.GetModuleBase(ModuleRUST.radarForm.settingsRadar.Network.lastGameProcessID, "GameAssembly.dll");
            }

                                }

        internal static void GetPointers()
        {
            
            Pointers.GetInstance();
            Pointers.GetNet();
            Pointers.GetBasePlayer();
            Pointers.GetSkyDome();

            Memory.DetectShortPtr(Pointers.GameObjectManager);

                                    
            UpdateClientEntities();

                    }

        internal static void UpdateClientEntities()
        {
            UpdateClientEntitiesAll();
            UpdateClientEntitiesPlayersOnly();
        }

        internal static void UpdateClientEntitiesAll()
        {
            
            Pointers.ClientEntitiesAll = Memory.ReadChain<ulong>(Pointers.BaseNetworkable, ModuleRUST.offsetsRUST.ClientEntitiesAll, false);
            
            Pointers.EntityListAll = Memory.Read<ulong>(ClientEntitiesAll + 0x18, false);
            
            GetEntityAllCount();

                    }

        internal static void UpdateClientEntitiesPlayersOnly()
        {
            
            Pointers.ClientEntitiesPlayersOnly = Memory.ReadChain<ulong>(Pointers.BasePlayer, ModuleRUST.offsetsRUST.ClientEntitiesPlayersOnlyDict, false);
            
            Pointers.EntityListPlayersOnly = Memory.Read<ulong>(ClientEntitiesPlayersOnly + 0x18, false);
            
            GetEntityPlayersOnlyCount();

                    }

        internal static void GetEntityAllCount()
        {
            
            if (Pointers.ClientEntitiesAll != 0)
            {
                Pointers.EntityCountAll = Memory.Read<int>(ClientEntitiesAll + 0x10, false);
                                                            }
            else
            {
                Pointers.EntityCountAll = 0;
                            }
        }

        internal static void GetEntityPlayersOnlyCount()
        {
            
            if (Pointers.ClientEntitiesPlayersOnly != 0)
            {
                Pointers.EntityCountPlayersOnly = Memory.Read<int>(ClientEntitiesPlayersOnly + 0x10, false);
                                                            }
            else
            {
                Pointers.EntityCountPlayersOnly = 0;
                            }
        }

        internal static bool GameWorldValid()
        {
            return (GameObjectManager != 0 && BaseNetworkable != 0 && BasePlayer != 0);
        }

        internal static void GetInstance()
        {
            GameObjectManager = Memory.Read<ulong>(Pointers.UnityModule + ModuleRUST.offsetsRUST.GameOjectManager, false, false);
        }

        internal static void GetNet()
        {
            BaseNetworkable = Memory.Read<ulong>(Pointers.GameAssembly + ModuleRUST.offsetsRUST.BaseNetworkable, false, false);
        }

        internal static void GetBasePlayer()
        {
            BasePlayer = Memory.Read<ulong>(Pointers.GameAssembly + ModuleRUST.offsetsRUST.BasePlayer, false, false);
        }

        internal static void GetSkyDome()
        {
            var skyDomeObject = GetTaggedObjects();
            skyDomeObject.GetNext();
            skyDomeObject.GetThis();

            SkyDome = Memory.ReadChain<ulong>(skyDomeObject.address, new uint[] { 0x30, 0x18, 0x28 });
        }

        internal static BaseObject GetTaggedObjects()
        {
            var baseObject = new BaseObject();
            baseObject.address = Memory.Read<ulong>(GameObjectManager + 0x8, false, false);
            return baseObject;
        }
    }
}