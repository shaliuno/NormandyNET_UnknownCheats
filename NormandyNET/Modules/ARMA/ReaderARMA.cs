using NormandyNET.Core;
using NormandyNET.Helpers;
using NormandyNET.Settings;
using NormandyNET.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NormandyNET.Modules.ARMA
{
    internal class ReaderARMA
    {
        internal List<EntityArma> mainEntityList = new List<EntityArma>();
        internal List<EntityArma> bulletEntityList = new List<EntityArma>();
        internal bool localPlayerFound;
        internal DateTime LootTime = CommonHelpers.dateTimeHolder;
        internal bool staticLootDone = true;
        internal int UpdateRate = 75;

        private static DateTime DelayedChecks = CommonHelpers.dateTimeHolder;
        private static int DelayedChecksUpdateSeconds = 5;

        private static int LootTimeUpdateRate = 10000;
        private bool IterateLootComplete = true;
        private bool IteratePlayersComplete = true;
        private System.Timers.Timer networkReaderTimer;
        private DateTime PlayersInfoTime = CommonHelpers.dateTimeHolder;
        private bool stopRequested = false;

        internal ReaderARMA()
        {
            ModuleARMA.radarForm.StartStopButtonClickEvent += new RadarForm.StartStopButtonClickHandler(this.StartOrStop);
            ModuleARMA.radarForm.OnUpdateLootButtonClick += UpdateLoot;
            Task.Run(() => AwaitInitializations());
        }

        private async void AwaitInitializations()
        {
            while (ModuleARMA.settingsForm == null)
            {
                Thread.Sleep(100);
            }

            ModuleARMA.radarForm.OnAdjustMapZoomCoeff += ModuleARMA.settingsForm.AdjustMapZoomCoeff;

            ModuleARMA.offsetsARMA.StartTimer();
        }

        internal EntityArma GetLocalPlayer()
        {
            var result = mainEntityList.Find(x => x.isLocalPlayer == true);
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

            World.CleanTables();

            lock (mainEntityList)
            {
                                mainEntityList.Clear();
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

            lock (mainEntityList)
            {
                                mainEntityList.Clear();
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

                    }

        internal void Init()
        {
            
            if (DebugClass.UseUserModeServer)
            {
                SynchronousSocketClient.InitClient(ModuleARMA.radarForm.settingsRadar.Network.ServerAddress);
                Memory.moduleBaseAddress = SynchronousSocketClient.ProcessOpen(ModuleARMA.radarForm.settingsRadar.Network.lastGameProcessID, "arma3_x64.exe");
            }
            else
            {
                                SynchronousSocketDriverClient.InitClient(ModuleARMA.radarForm.settingsRadar.Network.ServerAddress, ModuleARMA.radarForm.settingsRadar.Network.ServerPort);

                                Memory.moduleBaseAddress = SynchronousSocketDriverClient.GetModuleBase(ModuleARMA.radarForm.settingsRadar.Network.lastGameProcessID, "arma3_x64.exe");
            }

            Memory.ResetFields();

            FindPointers();

            stopRequested = false;

            HelperLogger.SaveLogToFile(false);
            ModuleARMA.radarForm.reloadMap = true;

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
                ModuleARMA.radarForm.Started = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExceptionWithExtraText(RadarForm.ActiveForm, ex);
                ModuleARMA.radarForm.Started = false;
                Stop();
            }
            finally
            {
                ModuleARMA.radarForm.ApplyButtonImagesFromModule();
            }
        }

        internal void StartOrStop(bool str)
        {
            if (ModuleARMA.radarForm.Started)
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
                ModuleARMA.radarForm.Started = false;
            }
            catch (Exception ex)
            {
                            }
            finally
            {
                ModuleARMA.radarForm.ApplyButtonImagesFromModule();
                HelperLogger.SaveLogToFile(false);
            }
        }

        private void FindPointers()
        {
            
            World.GetInstance();

            if (World.world != 0)
            {
            }

            World.GetCamera();
            NetworkManager.GetInstance();
            NetworkManager.GetNetworkClient();
            NetworkClient.GetPlayersCount();
            NetworkClient.GetServerName();
            NetworkClient.GetScoreBoard();
            NetworkClient.GetScoreboardList();

                    }

        private void IterateLoot()
        {
            
            var entriesBuildingsAndLoot = World.EntityTable_BuildingObjectsAndLoot.GetTableEntries(TableType.BuildingLoot);
            var entriesBuildingsAndLoot_2 = World.EntityTable_BuildingObjectsAndLoot_2.GetTableEntries(TableType.BuildingLoot);

            var entityListTemporary = new List<EntityArma>();

            AddEntitiesToDestEntityList(entriesBuildingsAndLoot, World.EntityTable_BuildingObjectsAndLoot_2.tableAddress, TableType.BuildingLoot, ref mainEntityList, ref entityListTemporary);
            AddEntitiesToDestEntityList(entriesBuildingsAndLoot_2, World.EntityTable_BuildingObjectsAndLoot_2.tableAddress, TableType.BuildingLoot, ref mainEntityList, ref entityListTemporary);

            ParseentityListDest(entityListTemporary, ref mainEntityList, TableType.BuildingLoot);

            if (staticLootDone == false)
            {
                staticLootDone = true;
            }

            IterateLootComplete = true;
                    }

        private void IteratePlayers()
        {
            
            World.GetEntityTables();

            var entriesNear = World.EntityTable_Near.GetTableEntries(TableType.Near);
            var entriesFar = World.EntityTable_Far.GetTableEntries(TableType.Far);
            var entriesFarFar = World.EntityTable_FarFar.GetTableEntries(TableType.FarFar);
            var entriesFarFarFar = World.EntityTable_FarFarFar.GetTableEntries(TableType.FarFarFar);

            var entityListTemporary = new List<EntityArma>();

            AddEntitiesToDestEntityList(entriesNear, World.EntityTable_Near.tableAddress, TableType.Near, ref mainEntityList, ref entityListTemporary);
            AddEntitiesToDestEntityList(entriesFar, World.EntityTable_Far.tableAddress, TableType.Far, ref mainEntityList, ref entityListTemporary);
            AddEntitiesToDestEntityList(entriesFarFar, World.EntityTable_FarFar.tableAddress, TableType.FarFar, ref mainEntityList, ref entityListTemporary);
            AddEntitiesToDestEntityList(entriesFarFarFar, World.EntityTable_FarFarFar.tableAddress, TableType.FarFarFar, ref mainEntityList, ref entityListTemporary);

            ParseentityListDest(entityListTemporary, ref mainEntityList, TableType.Near);
            ParseentityListDest(entityListTemporary, ref mainEntityList, TableType.Far);
            ParseentityListDest(entityListTemporary, ref mainEntityList, TableType.FarFar);
            ParseentityListDest(entityListTemporary, ref mainEntityList, TableType.FarFarFar);

            mainEntityList.RemoveAll(r => r.canDelete == true);

            IteratePlayersComplete = true;
                    }

        private void ParseentityListDest(List<EntityArma> entityListTemporary, ref List<EntityArma> entityListDest, TableType tableType)
        {
            var entryCurrent = 0;
            var entryMax = ModuleARMA.settingsForm.settingsJson.Loot.LiveLootPerCycle;

            for (int i = 0; i < entityListDest.Count; i++)
            {
                if (entityListDest[i].tableType != tableType)
                {
                    continue;
                }

                var indexTemp = entityListTemporary.IndexOf(entityListDest[i]);

                if (indexTemp >= 0)
                {
                    if (tableType == TableType.BuildingLoot)
                    {
                        if (staticLootDone == false)
                        {
                            entityListDest[i].GetPlayerValues();
                        }
                        else
                        {
                            if (ModuleARMA.settingsForm.settingsJson.Loot.LiveLoot)
                            {
                                if (entryCurrent < entryMax)
                                {
                                    if (entityListDest[i].EntityType == null)
                                    {
                                        entryCurrent++;
                                        if (entityListDest[i].canReadData)
                                        {
                                            entryCurrent++;
                                            entityListDest[i].GetPlayerValues();
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
                    else
                    {
                                                entityListDest[i].GetPlayerValues();
                    }
                }

                if (indexTemp < 0)
                {
                    entityListDest[i].canDelete = true;
                }
            }
        }

        internal void AddEntitiesToDestEntityList(List<ulong> entitiesAddresses, ulong entityTableAddress, TableType tableType, ref List<EntityArma> entityListDest, ref List<EntityArma> entityListTemporary)
        {
            for (int i = 0; i < entitiesAddresses.Count; i++)
            {
                if (Memory.IsValidPointer(entitiesAddresses[i]) == false)
                {
                                        continue;
                }

                var entity = new EntityArma(entityTableAddress + 0x8 * (uint)i, entitiesAddresses[i]);
                entity.tableType = tableType;

                entityListTemporary.Add(entity);

                var indexCache = entityListDest.IndexOf(entity);

                if (indexCache < 0)
                {
                    entityListDest.Add(entity);
                }
            }
        }

        private void NetworkReaderTimerTick(object source, ElapsedEventArgs e)
        {
            if (ModuleARMA.radarForm.Started)
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
                        
                        World.GetLocalEntity();

                        var myPos = World.GetLocalEntityPosition();
                        CommonHelpers.myIngamePositionX = myPos.X;
                        CommonHelpers.myIngamePositionY = myPos.Y;
                        CommonHelpers.myIngamePositionZ = myPos.Z;

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

                    DoWriteMemoryStuff();

                                    }
            }

            if (stopRequested == false && ModuleARMA.radarForm.Started == true && networkReaderTimer != null)
            {
                networkReaderTimer.Start();
            }
        }

        private void DoWriteMemoryStuff()
        {
            var localPlayer = ModuleARMA.readerARMA.GetLocalPlayer();

            if (localPlayer != null)
            {
                if (ModuleARMA.settingsForm.settingsJson.MemoryWriting.TestShit && mainEntityList.Count > 0)
                {
                    World.GetBullets();

                    var entriesBullets = World.EntityTable_Bullet.GetTableEntries(TableType.Bullet);
                    var entityListTemporary = new List<EntityArma>();

                    ModuleARMA.readerARMA.AddEntitiesToDestEntityList(entriesBullets, World.EntityTable_Bullet.tableAddress, TableType.Bullet, ref bulletEntityList, ref entityListTemporary);

                    ParseentityListDest(entityListTemporary, ref bulletEntityList, TableType.Bullet);

                    bulletEntityList.RemoveAll(r => r.canDelete == true);

                    var entTarget = mainEntityList.Find(r => r.EntityType != null && r.EntityType.Equals("Soldier") && !r.isLocalPlayer && !r.isDead);

                    foreach (EntityArma bullet in bulletEntityList)
                    {
                        if (entTarget == null)
                        {
                            break;
                        }
                        bullet.GetFutureVisualState().SetPosition(
                        entTarget

                        .GetRenderVisualState().GetHeadPosition());
                        entTarget.DelayedChecksAllowed = true;
                    }
                }
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
    }
}