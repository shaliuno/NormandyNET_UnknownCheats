using NormandyNET.Core;
using NormandyNET.Helpers;
using NormandyNET.Settings;
using NormandyNET.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NormandyNET.Modules.DAYZ
{
    internal class ReaderDAYZ
    {
        #region Internal Fields

        internal static readonly List<EntityPlayer> playersList = new List<EntityPlayer>();
        internal bool localPlayerFound;
        internal DateTime LootTime = CommonHelpers.dateTimeHolder;
        internal bool staticLootDone = true;
        internal int UpdateRate = 1;

        private static DateTime DelayedChecks = CommonHelpers.dateTimeHolder;
        private static int DelayedChecksUpdateSeconds = 5;

        #endregion Internal Fields

        #region Private Fields

        private static int LootTimeUpdateRate = 1000;
        private bool IterateLootComplete = true;
        private bool IteratePlayersComplete = true;
        private System.Timers.Timer networkReaderTimer;
        private DateTime PlayersInfoTime = CommonHelpers.dateTimeHolder;
        private PointerStruct pointer;
        private bool stopRequested = false;

        #endregion Private Fields

        #region Internal Constructors

        internal ReaderDAYZ()
        {
            ModuleDAYZ.radarForm.StartStopButtonClickEvent += new RadarForm.StartStopButtonClickHandler(this.StartOrStop);
            ModuleDAYZ.radarForm.OnUpdateLootButtonClick += UpdateLoot;
            Task.Run(() => AwaitInitializations());
        }

        private async void AwaitInitializations()
        {
            while (ModuleDAYZ.settingsForm == null)
            {
                Thread.Sleep(100);
            }

            ModuleDAYZ.radarForm.OnAdjustMapZoomCoeff += ModuleDAYZ.settingsForm.AdjustMapZoomCoeff;
            await Task.Delay(TimeSpan.FromSeconds(2));
        }

        #endregion Internal Constructors

        #region Internal Methods

        internal static EntityPlayer GetLocalPlayer()
        {
            var result = playersList.Find(x => x.isLocalPlayer == true);
            return result;
        }

        internal void CleanUpViaAuto()
        {
            
            localPlayerFound = false;
            staticLootDone = true;
            IterateLootComplete = true;
            IteratePlayersComplete = true;
            stopRequested = false;

            World.world = 0;
            World.playerOn = 0;
            Camera.camera = 0;
            NetworkManager.networkManager = 0;
            NetworkClient.networkClient = 0;
            NetworkClient.playersCount = 0;
            NetworkClient.scoreBoard = 0;
            NetworkClient.serverName = "N/A";
            NetworkClient.playersList.Clear();

            pointer.world.FarAranimalTable = 0;
            pointer.world.FastAnimalTable = 0;
            pointer.world.NearAnimalTable = 0;
            pointer.world.SlowAnimalTable = 0;
            pointer.world.world_itemtable = 0;

            lock (playersList)
            {
                                playersList.Clear();
                            }

                    }

        internal void CleanUpViaButton()
        {
            
            stopRequested = true;

            if (networkReaderTimer != null)
            {
                networkReaderTimer.Stop();
                while (networkReaderTimer.Enabled)
                {
                                    }

                networkReaderTimer.Dispose();
            }

            if (DebugClass.UseUserModeServer)
            {
                SynchronousSocketClient.ShutdownClient();
            }
            else
            {
                SynchronousSocketDriverClient.ShutdownClient();
            }

            lock (playersList)
            {
                                playersList.Clear();
                            }

            localPlayerFound = false;
            staticLootDone = true;
            IterateLootComplete = true;
            IteratePlayersComplete = true;
            stopRequested = false;

            World.world = 0;
            World.playerOn = 0;
            Camera.camera = 0;
            NetworkManager.networkManager = 0;
            NetworkClient.networkClient = 0;
            NetworkClient.playersCount = 0;
            NetworkClient.scoreBoard = 0;
            NetworkClient.serverName = "N/A";
            NetworkClient.playersList.Clear();

            pointer.world.FarAranimalTable = 0;
            pointer.world.FastAnimalTable = 0;
            pointer.world.NearAnimalTable = 0;
            pointer.world.SlowAnimalTable = 0;
            pointer.world.world_itemtable = 0;

                    }

        internal void Init()
        {
            
            if (DebugClass.UseUserModeServer)
            {
                SynchronousSocketClient.InitClient(ModuleDAYZ.radarForm.settingsRadar.Network.ServerAddress);
                Memory.moduleBaseAddress = SynchronousSocketClient.ProcessOpen(ModuleDAYZ.radarForm.settingsRadar.Network.lastGameProcessID, "DayZ_x64.exe");
            }
            else
            {
                                SynchronousSocketDriverClient.InitClient(ModuleDAYZ.radarForm.settingsRadar.Network.ServerAddress, ModuleDAYZ.radarForm.settingsRadar.Network.ServerPort);

                                Memory.moduleBaseAddress = SynchronousSocketDriverClient.GetModuleBase(ModuleDAYZ.radarForm.settingsRadar.Network.lastGameProcessID, "DayZ_x64.exe");
            }

            Memory.ResetFields();

            FindPointers();

            stopRequested = false;

            HelperLogger.SaveLogToFile(false);
            ModuleDAYZ.radarForm.reloadMap = true;

            networkReaderTimer = new System.Timers.Timer
            {
                Interval = 1
            };

            networkReaderTimer.Elapsed += NetworkReaderTimerTick;
            networkReaderTimer.AutoReset = false;
            networkReaderTimer.Enabled = true;
            networkReaderTimer.Stop();
            networkReaderTimer.Start();
                    }

        internal void Start()
        {
            try
            {
                Init();
                ModuleDAYZ.radarForm.Started = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExceptionWithExtraText(RadarForm.ActiveForm, ex);
                ModuleDAYZ.radarForm.Started = false;
                Stop();
            }
            finally
            {
                ModuleDAYZ.radarForm.ApplyButtonImagesFromModule();
            }
        }

        internal void StartOrStop(bool str)
        {
            if (ModuleDAYZ.radarForm.Started)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        internal void Stop()
        {
            try
            {
                CleanUpViaButton();
                ModuleDAYZ.radarForm.Started = false;
            }
            catch (Exception ex)
            {
                            }
            finally
            {
                ModuleDAYZ.radarForm.ApplyButtonImagesFromModule();
                HelperLogger.SaveLogToFile(false);
            }
        }

        #endregion Internal Methods

        #region Private Methods

        private void FindPointers()
        {
            
            World.GetInstance();

            if (World.world != 0)
            {
            }
            World.GetCamera();
            NetworkManager.GetInstance();
            NetworkManager.GetNetworkClient();
            NetworkClient.GetServerName();
            NetworkClient.GetScoreBoard();
            NetworkClient.GetScoreboardList();
            NetworkClient.GetPlayersCount();

                    }

        private void GetTables()
        {
            
            var databuffer = Memory.ReadBytes(World.world + ModuleDAYZ.offsetsDAYZ.World_Table_FarAranimal, sizeof(ulong) + sizeof(int));
            pointer.world.FarAranimalTable = BitConverter.ToUInt64(databuffer, 0);
            pointer.world.FarAranimalTableSz = BitConverter.ToInt32(databuffer, 8);

            databuffer = Memory.ReadBytes(World.world + ModuleDAYZ.offsetsDAYZ.World_Table_FastAnimal, sizeof(ulong) + sizeof(int));
            pointer.world.FastAnimalTable = BitConverter.ToUInt64(databuffer, 0);
            pointer.world.FastAnimalTableSz = BitConverter.ToInt32(databuffer, 8);

            databuffer = Memory.ReadBytes(World.world + ModuleDAYZ.offsetsDAYZ.World_Table_NearAnimal, sizeof(ulong) + sizeof(int));
            pointer.world.NearAnimalTable = BitConverter.ToUInt64(databuffer, 0);
            pointer.world.NearAnimalTableSz = BitConverter.ToInt32(databuffer, 8);

            databuffer = Memory.ReadBytes(World.world + ModuleDAYZ.offsetsDAYZ.World_Table_SlowAnimal, sizeof(ulong) + sizeof(int));
            pointer.world.SlowAnimalTable = BitConverter.ToUInt64(databuffer, 0);
            pointer.world.SlowAnimalTableSz = BitConverter.ToInt32(databuffer, 8);

            databuffer = Memory.ReadBytes(World.world + ModuleDAYZ.offsetsDAYZ.World_Table_Items, sizeof(ulong) + sizeof(int));
            pointer.world.world_itemtable = BitConverter.ToUInt64(databuffer, 0);
            pointer.world.world_itemtableSize = BitConverter.ToInt32(databuffer, 8);

                                                                                }

        private void IterateEntityTable(ulong tableaddress, int tableSize, TableType tableType)
        {
            
            if (tableaddress == 0)
            {
                return;
            }

            var playersListNew = new List<EntityPlayer>();

            int offset = 0;
            int maxEntiresToRead = 48;
            int entiresToRead = maxEntiresToRead;

            if (entiresToRead > tableSize)
            {
                entiresToRead = tableSize;
            }

            while (offset < tableSize)
            {
                var databuffer = Memory.ReadBytes(tableaddress + (uint)offset * 0x8, sizeof(ulong) * entiresToRead);
                
                for (uint i = 0; i < entiresToRead; i++)
                {
                    var playerObjectAddressPointer = tableaddress + ((uint)offset * 0x8);

                    var playerObjectAddress = BitConverter.ToUInt64(databuffer, (int)(i * sizeof(ulong)));
                    
                    if (Memory.IsValidPointer(playerObjectAddress) == false)
                    {
                                                offset++;
                        continue;
                    }

                    var playerEntity = new EntityPlayer(playerObjectAddressPointer, playerObjectAddress);
                    playerEntity.tableType = tableType;

                    playersListNew.Add(playerEntity);

                    var indexCache = playersList.IndexOf(playerEntity);

                    if (indexCache < 0)
                    {
                        playersList.Add(playerEntity);
                    }

                    if (indexCache >= 0)
                    {
                        playersList[indexCache].tableType = tableType;
                    }

                    offset++;
                }

                if ((tableSize - offset) < entiresToRead)
                {
                    entiresToRead = tableSize - offset;
                }
            }

            for (int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i].tableType != tableType)
                {
                    continue;
                }

                var indexTemp = playersListNew.IndexOf(playersList[i]);

                if (indexTemp >= 0)
                {
                                        playersList[i].GetPlayerValues();
                }

                if (indexTemp < 0)
                {
                    playersList[i].canDelete = true;
                }
            }

            playersList.RemoveAll(r => r.canDelete == true && r.tableType == tableType);

                    }

        private void IterateEntityTableSlowAndItems(ulong tableAddress, int tableSize, TableType tableType)
        {
            
            if (tableAddress == 0)
            {
                return;
            }

            var playersListNew = new List<EntityPlayer>();

            int offset = 0;
            int maxEntiresToRead = 38;
            int entiresToRead = maxEntiresToRead;
            int multiplicator = 3;

            if (entiresToRead > tableSize)
            {
                entiresToRead = tableSize;
            }

            while (offset < tableSize)
            {
                var databuffer = Memory.ReadBytes(tableAddress + (uint)offset * 0x18, sizeof(ulong) * entiresToRead * multiplicator);

                
                for (uint i = 0; i < entiresToRead; i++)
                {
                    var playerObjectAddressPointer = tableAddress + ((uint)offset * 0x18) + 0x8;

                    var playerObjectAddress = BitConverter.ToUInt64(databuffer, (int)(i * 0x18) + 0x8);

                    
                    var pickedUp = BitConverter.ToUInt32(databuffer, (int)(i * 0x18));

                    if (pickedUp == (int)ItemPickState.NotItem)
                    {
                                                offset++;
                        continue;
                    }

                    if (Memory.IsValidPointer(playerObjectAddress) == false)
                    {
                                                offset++;
                        continue;
                    }

                    var playerEntity = new EntityPlayer(playerObjectAddressPointer, playerObjectAddress);
                    playerEntity.tableType = tableType;

                    playersListNew.Add(playerEntity);

                    var indexCache = playersList.IndexOf(playerEntity);

                    if (indexCache < 0 && pickedUp == (int)ItemPickState.OnGround)
                    {
                        playersList.Add(playerEntity);
                    }

                    if (indexCache >= 0)
                    {
                        if (pickedUp > 1)
                        {
                            playersList[indexCache].canDelete = true;
                        }
                        else
                        {
                            playersList[indexCache].tableType = tableType;
                        }
                    }

                    offset++;
                }

                if ((tableSize - offset) < entiresToRead)
                {
                    entiresToRead = tableSize - offset;

                    if (entiresToRead < 0)
                    {
                                            }
                }
            }

            var entryCurrent = 0;
            var entryMax = ModuleDAYZ.settingsForm.settingsJson.Loot.LiveLootPerCycle;

            for (int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i].tableType != tableType)
                {
                    continue;
                }

                var indexTemp = playersListNew.IndexOf(playersList[i]);

                if (indexTemp >= 0)
                {
                    if (staticLootDone == false)
                    {
                        playersList[i].GetPlayerValues();
                    }
                    else
                    {
                        if (playersList[i].EntityType == "Animal" || playersList[i].EntityType == "Player" || playersList[i].EntityType == "Infected")
                        {
                            playersList[i].GetPlayerValues();
                        }

                        if (ModuleDAYZ.settingsForm.settingsJson.Loot.LiveLoot)
                        {
                            if (entryCurrent < entryMax)
                            {
                                if (playersList[i].EntityType == null)
                                {
                                    entryCurrent++;
                                    if (playersList[i].canReadData)
                                    {
                                        entryCurrent++;
                                        playersList[i].GetPlayerValues();
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                                    }

                if (indexTemp < 0)
                {
                    playersList[i].canDelete = true;
                }
            }

            playersList.RemoveAll(r => r.canDelete == true && r.tableType == tableType);

                    }

        private void IterateLoot()
        {
            
            if (staticLootDone && ModuleDAYZ.settingsForm.settingsJson.Loot.LiveLoot == false)
            {
                                IterateLootComplete = true;
                return;
            }

            IterateEntityTableSlowAndItems(pointer.world.world_itemtable, pointer.world.world_itemtableSize, TableType.Items);
            IterateEntityTableSlowAndItems(pointer.world.SlowAnimalTable, pointer.world.SlowAnimalTableSz, TableType.Slow);

            if (staticLootDone == false)
            {
                staticLootDone = true;
            }

            IterateLootComplete = true;
                    }

        private void IteratePlayers()
        {
                        GetTables();
            IterateEntityTable(pointer.world.NearAnimalTable, pointer.world.NearAnimalTableSz, TableType.Near);
            IterateEntityTable(pointer.world.FarAranimalTable, pointer.world.FarAranimalTableSz, TableType.Far);
            IterateEntityTable(pointer.world.FastAnimalTable, pointer.world.FastAnimalTableSz, TableType.Fast);

            IteratePlayersComplete = true;
                    }

        private void NetworkReaderTimerTick(object source, ElapsedEventArgs e)
        {
            if (ModuleDAYZ.radarForm.Started)
            {
                if (CommonHelpers.dateTimeHolder > DelayedChecks)
                {
                    
                    var localEntity = World.playerOn;

                    World.GetLocalEntity();

                    if (localEntity != World.playerOn && localEntity != 0)
                    {
                        CleanUpViaAuto();
                    }

                    if (World.world == 0)
                    {
                        FindPointers();
                    }

                    NetworkClient.GetPlayersCount();
                    NetworkClient.GetScoreboardList();

                    DelayedChecks = CommonHelpers.dateTimeHolder.AddSeconds(DelayedChecksUpdateSeconds);
                                    }

                if (World.world != 0 && World.playerOn != 0)
                {
                    
                    if ((CommonHelpers.dateTimeHolder > PlayersInfoTime) && IteratePlayersComplete)
                    {
                        
                        IteratePlayersComplete = false;
                        IteratePlayers();

                        PlayersInfoTime = CommonHelpers.dateTimeHolder.AddMilliseconds(UpdateRate);
                        Camera.GetCameraRelated();
                    }

                    if ((CommonHelpers.dateTimeHolder > LootTime) && IterateLootComplete)
                    {
                        
                        IterateLootComplete = false;

                        IterateLoot();

                        LootTime = CommonHelpers.dateTimeHolder.AddMilliseconds(LootTimeUpdateRate);
                    }

                                    }
            }

            if (stopRequested == false && ModuleDAYZ.radarForm.Started == true && networkReaderTimer != null)
            {
                networkReaderTimer.Start();
            }
        }

        private void UpdateLoot(object sender, EventArgs e)
        {
            UpdateLoot();
        }

        internal void UpdateLoot()
        {
            staticLootDone = false;
            LootTime = CommonHelpers.dateTimeHolder;
        }

        #endregion Private Methods

        #region Private Structs

        private struct PointerStruct
        {
            #region Internal Fields

            internal worldStruct world;

            #endregion Internal Fields

            #region Internal Structs

            internal struct worldStruct
            {
                #region Internal Fields

                internal ulong world_ambient_light_pointer;
                internal ulong world_cameraon_pointer;
                internal ulong FarAranimalTable;
                internal int FarAranimalTableSz;
                internal ulong FastAnimalTable;
                internal int FastAnimalTableSz;
                internal ulong world_itemtable;
                internal int world_itemtableSize;
                internal ulong NearAnimalTable;
                internal int NearAnimalTableSz;
                internal ulong world_playeron_pointer;
                internal ulong SlowAnimalTable;
                internal int SlowAnimalTableSz;
                internal ulong world_terraingrid_pointer;
                internal ulong world_time_pointer;

                #endregion Internal Fields
            }

            #endregion Internal Structs
        }

        #endregion Private Structs
    }
}