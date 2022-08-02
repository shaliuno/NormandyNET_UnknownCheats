using NormandyNET.Core;
using NormandyNET.Helpers;
using NormandyNET.Settings;
using NormandyNET.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static NormandyNET.Core.MapManager;

namespace NormandyNET.Modules.RUST
{
    internal class ReaderRUST
    {
        #region Internal Fields

        internal static readonly List<BaseEntity> entityList = new List<BaseEntity>();
        internal bool localPlayerFound;
        internal int UpdateRate = 75;

        private static DateTime DelayedChecks = CommonHelpers.dateTimeHolder;
        private static int DelayedChecksUpdateSeconds = 5;
        private readonly int PointersUpdateRateSec = 3;
        internal static DateTime PointersTime = DateTime.UtcNow;
        internal bool staticLootDone = true;
        internal bool lootUpdateRequested = false;

        #endregion Internal Fields

        #region Private Fields

        private bool IterateEntitiesComplete = true;
        private System.Timers.Timer networkReaderTimer;
        private DateTime PlayersInfoTime = CommonHelpers.dateTimeHolder;
        private bool stopRequested = false;

        #endregion Private Fields

        #region Internal Constructors

        internal ReaderRUST()
        {
            ModuleRUST.radarForm.StartStopButtonClickEvent += new RadarForm.StartStopButtonClickHandler(this.StartOrStop);
            ModuleRUST.radarForm.OnUpdateLootButtonClick += UpdateLoot;
            Task.Run(() => AwaitInitializations());
        }

        private void UpdateLoot(object sender, EventArgs e)
        {
            lootUpdateRequested = true;
        }

        private async void AwaitInitializations()
        {
            while (ModuleRUST.settingsForm == null)
            {
                Thread.Sleep(100);
            }

            ModuleRUST.radarForm.OnAdjustMapZoomCoeff += ModuleRUST.settingsForm.AdjustMapZoomCoeff;

            ModuleRUST.offsetsRUST.StartTimer();

            ModuleRUST.radarForm.mapManager.CurrentMap = "Default";
        }

        #endregion Internal Constructors

        #region Internal Methods

        internal void CleanupGeneral()
        {
            
            localPlayerFound = false;
            IterateEntitiesComplete = true;
            stopRequested = false;
            Pointers.CleanUp();

            lock (entityList)
            {
                                entityList.Clear();
                            }

                    }

        internal void CleanUpViaButton()
        {
            
            CleanupGeneral();

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

                    }

        internal void Init()
        {
            
            if (DebugClass.UseUserModeServer)
            {
                                SynchronousSocketClient.InitClient(ModuleRUST.radarForm.settingsRadar.Network.ServerAddress);
                Pointers.GetModules(DebugClass.UseUserModeServer);
            }
            else
            {
                                SynchronousSocketDriverClient.InitClient(ModuleRUST.radarForm.settingsRadar.Network.ServerAddress, ModuleRUST.radarForm.settingsRadar.Network.ServerPort);
                Pointers.GetModules(DebugClass.UseUserModeServer);
            }

            Memory.ResetFields();

            ModuleRUST.radarForm.mapManager.CurrentMap = "Default";

            if (ModuleRUST.settingsForm.settingsJson.Map.BaseCanvasOverride)
            {
                MapObject mapObjectSwitchTo;
                mapObjectSwitchTo = ModuleRUST.radarForm.mapManager.mapObjects[ModuleRUST.radarForm.mapManager.CurrentMap];
                mapObjectSwitchTo.CanvasSizeBase = ModuleRUST.settingsForm.settingsJson.Map.MapSize / (float)ModuleRUST.settingsForm.settingsJson.Map.MapCoeff;
            }

            ModuleRUST.radarForm.reloadMap = true;

            Pointers.GetPointers();

            stopRequested = false;

            HelperLogger.SaveLogToFile(false);

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
                ModuleRUST.radarForm.Started = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExceptionWithExtraText(RadarForm.ActiveForm, ex);
                ModuleRUST.radarForm.Started = false;
                Stop();
            }
            finally
            {
                ModuleRUST.radarForm.ApplyButtonImagesFromModule();
            }
        }

        internal void StartOrStop(bool str)
        {
            if (ModuleRUST.radarForm.Started)
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
                ModuleRUST.radarForm.Started = false;
            }
            catch (Exception ex)
            {
                            }
            finally
            {
                ModuleRUST.radarForm.ApplyButtonImagesFromModule();
                HelperLogger.SaveLogToFile(false);
            }
        }

        #endregion Internal Methods

        #region Private Methods

        private void IterateEntities()
        {
            
            if (Pointers.EntityListAll == 0)
            {
                IterateEntitiesComplete = true;
                return;
            }

            if (Pointers.EntityListAll != 0)
            {
                BuildFreshEntityList();
            }

            IterateEntitiesComplete = true;
                    }

        private List<BaseEntity> BuildPlayersOnlyList()
        {
            
            var entityListPlayersOnlyTemp = new List<BaseEntity>();

            var actorPre = Pointers.EntityListPlayersOnly + 0x20;

            int offset = 0;
            int maxEntiresToRead = 112;
            int entiresToRead = maxEntiresToRead;

            if (entiresToRead > Pointers.EntityCountPlayersOnly)
            {
                entiresToRead = Pointers.EntityCountPlayersOnly;
            }

            while (offset < Pointers.EntityCountPlayersOnly)
            {
                var databuffer = Memory.ReadBytes(actorPre + (uint)offset * 0x8, sizeof(ulong) * entiresToRead);
                
                for (uint i = 0; i < entiresToRead; i++)
                {
                    var entityAddressPointer = actorPre + ((uint)offset * 0x8);

                    var entityAddress = BitConverter.ToUInt64(databuffer, (int)(i * sizeof(ulong)));
                    
                    if (Memory.IsValidPointer(entityAddress) == false)
                    {
                                                offset++;
                        continue;
                    }

                    var playerEntity = new BaseEntity(entityAddress);
                    entityListPlayersOnlyTemp.Add(playerEntity);

                    offset++;
                }

                if ((Pointers.EntityCountPlayersOnly - offset) < entiresToRead)
                {
                    entiresToRead = Pointers.EntityCountPlayersOnly - offset;
                }
            }

            return entityListPlayersOnlyTemp;

                    }

        private void BuildFreshEntityList()
        {
            if (Pointers.EntityListAll == 0)
            {
                return;
            }

            if (lootUpdateRequested)
            {
                lootUpdateRequested = false;
                staticLootDone = false;
            }

            var entityListTemp = new List<BaseEntity>();
            var entityListPlayersOnly = BuildPlayersOnlyList();

            var actorPre = Pointers.EntityListAll + 0x20;

            int offset = 0;
            int maxEntiresToRead = 112;
            int entiresToRead = maxEntiresToRead;

            if (entiresToRead > Pointers.EntityCountAll)
            {
                entiresToRead = Pointers.EntityCountAll;
            }

            var entAdded = 0;
            var entRemoved = 0;

            while (offset < Pointers.EntityCountAll)
            {
                var databuffer = Memory.ReadBytes(actorPre + (uint)offset * 0x8, sizeof(ulong) * entiresToRead);
                
                for (uint i = 0; i < entiresToRead; i++)
                {
                    var entityAddressPointer = actorPre + ((uint)offset * 0x8);

                    var entityAddress = BitConverter.ToUInt64(databuffer, (int)(i * sizeof(ulong)));
                    
                    if (Memory.IsValidPointer(entityAddress) == false)
                    {
                                                offset++;
                        continue;
                    }

                    var playerEntity = new BaseEntity(entityAddress);

                    entityListTemp.Add(playerEntity);

                    var indexCache = entityList.IndexOf(playerEntity);

                    if (indexCache < 0)
                    {
                        entityList.Add(playerEntity);
                        entAdded++;
                    }

                    offset++;
                }

                if ((Pointers.EntityCountAll - offset) < entiresToRead)
                {
                    entiresToRead = Pointers.EntityCountAll - offset;
                }
            }

            var entryCurrent = 0;
            var entryMax = ModuleRUST.settingsForm.settingsJson.Loot.LiveLootPerCycle;

            for (int i = 0; i < entityList.Count; i++)
            {
                if (entityListPlayersOnly.Contains(entityList[i]))
                {
                    continue;
                }

                var indexTemp = entityListTemp.IndexOf(entityList[i]);

                if (indexTemp >= 0)
                {
                    if (entityList.Count == 0)
                    {
                        break;
                    }

                    
                    if (staticLootDone == false)
                    {
                        entityList[i].GetEntityValues();
                    }
                    else
                    {
                        if (entityList[i].EntityType == EntityTypeRUST.Animal)
                        {
                            entityList[i].GetEntityValues();
                        }

                        if (ModuleRUST.settingsForm.settingsJson.Loot.LiveLoot)
                        {
                            if (entryCurrent < entryMax)
                            {
                                if (entityList[i].EntityType == EntityTypeRUST.NULL)
                                {
                                    entryCurrent++;
                                    entityList[i].GetEntityValues();
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
                    entRemoved++;
                    entityList[i].canDelete = true;
                }
            }

            lock (entityList)
            {
                entityList.RemoveAll(r => r.canDelete == true);
            }

            {
                for (int u = 0; u < entityListPlayersOnly.Count; u++)
                {
                    var indexCache = entityList.IndexOf(entityListPlayersOnly[u]);

                    if (indexCache >= 0)
                    {
                        entityList[indexCache].GetEntityValues(true);
                    }

                    if (indexCache < 0)
                    {
                        entityList.Add(entityListPlayersOnly[u]);
                        entityListPlayersOnly.Last().GetEntityValues();
                    }
                }
            }

            if (staticLootDone == false)
            {
                staticLootDone = true;
            }
        }

        internal BaseEntity GetLocalPlayer()
        {
            var result = entityList.Find(x => x.isLocalPlayer == true);
            return result;
        }

        private void NetworkReaderTimerTick(object source, ElapsedEventArgs e)
        {
            if (ModuleRUST.radarForm.Started)
            {
                if (Memory.stopOnError && Memory.InvalidAddressCount > 0)
                {
                    stopRequested = true;
                }

                if (CommonHelpers.dateTimeHolder > PointersTime && Pointers.EntityCountAll == 0 && stopRequested == false)
                {
                    
                    CleanupGeneral();

                    PointersTime = CommonHelpers.dateTimeHolder.AddSeconds(PointersUpdateRateSec);
                                    }

                if (Pointers.EntityCountAll > 0 && stopRequested == false)
                {
                    if (CommonHelpers.dateTimeHolder > PointersTime)
                    {
                        PointersTime = CommonHelpers.dateTimeHolder.AddSeconds(PointersUpdateRateSec);
                        Pointers.UpdateClientEntities();
                    }

                    
                    Pointers.GetEntityPlayersOnlyCount();
                    Pointers.GetEntityAllCount();

                    if ((CommonHelpers.dateTimeHolder > PlayersInfoTime) && IterateEntitiesComplete && Pointers.EntityCountAll > 0)
                    {
                        Pointers.PlayersCount = 0;

                        IterateEntitiesComplete = false;
                        IterateEntities();

                        PlayersInfoTime = CommonHelpers.dateTimeHolder.AddMilliseconds(UpdateRate);
                    }

                    if (ModuleRUST.radarForm.overlay != null && ModuleRUST.radarForm.overlay.canRun)
                    {
                        if (localPlayerFound)
                        {
                            Camera.GetCameraMatrix();
                        }
                    }

                                    }
            }
            else
            {
                Pointers.EntityCountAll = 0;
            }

            if (stopRequested == false && ModuleRUST.radarForm.Started == true && networkReaderTimer != null)
            {
                networkReaderTimer.Start();
            }
        }

        #endregion Private Methods
    }
}