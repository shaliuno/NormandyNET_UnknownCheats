using NormandyNET.Core;
using NormandyNET.Helpers;
using NormandyNET.Modules.EFT.Objects;
using NormandyNET.Settings;
using NormandyNET.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace NormandyNET.Modules.EFT
{
    internal class ReaderEFT
    {
        internal static readonly List<EntityLoot> lootList = new List<EntityLoot>();
        internal static readonly List<EntityPlayer> playersList = new List<EntityPlayer>();
        internal static readonly List<Grenade> grenadesList = new List<Grenade>();
        internal FPSCamera fpsCamera;
        internal LocalGameWorld localGameWorld;
        internal MainApplicationObject mainApplication;

        private bool IterateLootComplete = true;
        private bool IteratePlayersComplete = true;
        private System.Timers.Timer networkReaderTimer;

        internal PointerStruct pointer;
        internal bool UpdateBasePointers;
        internal int UpdateRate = 0;
        internal static bool localPlayerFound = false;
        internal static bool followPlayerUpdateRequested = false;
        internal static string followPlayerUpdateNewGuid = "";

        internal static bool swapInventoryRequested = false;
        internal static ulong swapInventoryAddress;

        internal static bool teleportEntityRequested = false;
        internal static ulong teleporEntityAddress;

        internal bool staticLootDone = true;

        internal DateTime LootTime = CommonHelpers.dateTimeHolder;
        private static int LootTimeUpdateRate = 1000;

        private static DateTime PlayersExtraInfoTime = CommonHelpers.dateTimeHolder;
        private DateTime PlayersInfoTime = CommonHelpers.dateTimeHolder;
        private readonly int CameraUpdateRateSec = 5;
        private readonly int PointersUpdateRateSec = 10;
        internal static DateTime PointersTime = DateTime.UtcNow;
        internal static DateTime CameraTime = DateTime.UtcNow;
        private bool stopRequested = false;

        internal bool breakReadings = false;
        internal bool searchRequested = false;

        internal ReaderEFT()
        {
            ModuleEFT.radarForm.StartStopButtonClickEvent += new RadarForm.StartStopButtonClickHandler(this.StartOrStop);
            ModuleEFT.radarForm.OnMapMouseButtonClick += new RadarForm.MapMouseButtonClickHandler(MapMouseButtonClick);
            ModuleEFT.radarForm.OnUpdateLootButtonClick += UpdateLoot;
            Task.Run(() => AwaitInitializations());
        }

        private void MapMouseButtonClick(int mousePositionStartX, int mousePositionStartY, Point cursorPos)
        {
            
            if (!ModuleEFT.radarForm.Started || ModuleEFT.radarForm.mapManager.CurrentMap == "")
            {
                return;
            }

            var invertMap = ModuleEFT.radarForm.mapManager.GetInvertMap();

            var ingamePointCoordinateX =

                ((mousePositionStartX - ModuleEFT.radarForm.GetOpenGlControlSize.Width / 2) -
                RadarForm.mapDragOffsetX -
                ModuleEFT.radarForm.mapManager.mapObjects[ModuleEFT.radarForm.mapManager.CurrentMap].offsetForObjectsX *
                OpenGL.CanvasDiffCoeff) * invertMap / OpenGL.CanvasDiffCoeff;

            var ingamePointCoordinateZ =
                (-(mousePositionStartY - ModuleEFT.radarForm.GetOpenGlControlSize.Height / 2) -
                RadarForm.mapDragOffsetZ -
                ModuleEFT.radarForm.mapManager.mapObjects[ModuleEFT.radarForm.mapManager.CurrentMap].offsetForObjectsY *
                OpenGL.CanvasDiffCoeff) * invertMap / OpenGL.CanvasDiffCoeff;

            RendererEFT.testCoordX = ingamePointCoordinateX;
            RendererEFT.testCoordZ = ingamePointCoordinateZ;

            
                                                            
                                                            
            
            ModuleEFT.radarForm.metroContextMenuMap.Items.Clear();
            var distanceThreshold = 30;

            
            var ingamePointCoordinate = new UnityEngine.Vector3(ingamePointCoordinateX, 0, ingamePointCoordinateZ);

            var menuCandidates = playersList.FindAll(r => r.canRender == true && r.canReadData == true);

            ToolStripMenuItem followEntity = new ToolStripMenuItem();
            followEntity.Name = $"FollowEntity";
            followEntity.Text = $"Follow";

            ToolStripMenuItem swapInventoryfollowEntity = new ToolStripMenuItem();
            swapInventoryfollowEntity.Name = $"SwapInventoryEntity";
            swapInventoryfollowEntity.Text = $"SwapInventory";

            ToolStripMenuItem teleportEntity = new ToolStripMenuItem();
            teleportEntity.Name = $"TeleportEntity";
            teleportEntity.Text = $"teleport";

            if (menuCandidates.Count > 0)
            {
                for (int i = 0; i < menuCandidates.Count; i++)

                {
                    var distance = (int)Math.Round(UnityEngine.Vector3.Distance(menuCandidates[i].Position, ingamePointCoordinate));

                    if (distance < distanceThreshold)
                    {
                        ToolStripMenuItem followEntityMenuEntry = new ToolStripMenuItem();
                        followEntityMenuEntry.Name = $"{menuCandidates[i].playerAddress.ToString()}";
                        followEntityMenuEntry.Text = $"{menuCandidates[i].info.Nickname} [{menuCandidates[i].info.Side}] distance: {distance}";
                        followEntity.DropDownItems.Add(followEntityMenuEntry);

                        followEntityMenuEntry.MouseDown += new System.Windows.Forms.MouseEventHandler(this.followEntityMenuEntry_MouseDown);

                        if (DebugClass.Debug)
                        {
                            ToolStripMenuItem swapInventoryEntityMenuEntry = new ToolStripMenuItem();
                            swapInventoryEntityMenuEntry.Name = $"{menuCandidates[i].playerAddress.ToString()}";
                            swapInventoryEntityMenuEntry.Text = $"{menuCandidates[i].playerAddress.ToString("x2")} - {menuCandidates[i].info.Nickname} [{menuCandidates[i].info.Side}] distance: {distance}";
                            swapInventoryfollowEntity.DropDownItems.Add(swapInventoryEntityMenuEntry);
                            swapInventoryEntityMenuEntry.MouseDown += new System.Windows.Forms.MouseEventHandler(this.swapInventoryEntityMenuEntry_MouseDown);

                            ToolStripMenuItem teleportEntityMenuEntry = new ToolStripMenuItem();
                            teleportEntityMenuEntry.Name = $"{menuCandidates[i].playerAddress.ToString()}";
                            teleportEntityMenuEntry.Text = $"{menuCandidates[i].playerAddress.ToString("x2")} - {menuCandidates[i].info.Nickname} [{menuCandidates[i].info.Side}] distance: {distance}";
                            teleportEntity.DropDownItems.Add(teleportEntityMenuEntry);

                            teleportEntityMenuEntry.MouseDown += new System.Windows.Forms.MouseEventHandler(this.teleportEntityMenuEntry_MouseDown);
                        }
                    }
                }
            }

            ModuleEFT.radarForm.metroContextMenuMap.Items.Add(followEntity);

            if (DebugClass.Debug)
            {
                ModuleEFT.radarForm.metroContextMenuMap.Items.Add(swapInventoryfollowEntity);
                ModuleEFT.radarForm.metroContextMenuMap.Items.Add(teleportEntity);
            }

            ModuleEFT.radarForm.metroContextMenuMap.Name = "metroContextMenuMap";
            ModuleEFT.radarForm.metroContextMenuMap.Size = new System.Drawing.Size(165, 48);
            ModuleEFT.radarForm.metroContextMenuMap.Show(mousePositionStartX, mousePositionStartY);
        }

        private void swapInventoryEntityMenuEntry_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var targetSwapInventory = ((ToolStripMenuItem)sender).Name;
                if (UInt64.TryParse(targetSwapInventory, out ulong _swapInventoryAddress))
                {
                    swapInventoryAddress = _swapInventoryAddress;
                    swapInventoryRequested = true;
                }
            }
        }

        private void teleportEntityMenuEntry_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var teleportEntityInventory = ((ToolStripMenuItem)sender).Name;
                if (UInt64.TryParse(teleportEntityInventory, out ulong _teleportEntityAddress))
                {
                    teleporEntityAddress = _teleportEntityAddress;
                    teleportEntityRequested = true;
                }
            }
        }

        private void followEntityMenuEntry_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var newLocalPlayer = ((ToolStripMenuItem)sender).Name;

                
                followPlayerUpdateNewGuid = newLocalPlayer;
                followPlayerUpdateRequested = true;

                
                Console.WriteLine(((ToolStripMenuItem)sender).Name);
            }
        }

        private async void AwaitInitializations()
        {
            while (ModuleEFT.settingsForm == null)
            {
                Thread.Sleep(100);
            }

            ModuleEFT.radarForm.OnAdjustMapZoomCoeff += ModuleEFT.settingsForm.AdjustMapZoomCoeff;

            await Task.Delay(TimeSpan.FromSeconds(2));
        }

        internal void StartOrStop(bool str)
        {
            if (ModuleEFT.radarForm.Started)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        internal void Start()
        {
            try
            {
                ModuleEFT.radarForm.mapManager.CurrentMap = string.Empty;

                Init();
                ModuleEFT.radarForm.Started = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExceptionWithExtraText(RadarForm.ActiveForm, ex);
                ModuleEFT.radarForm.Started = false;
                Stop();
            }
            finally
            {
                ModuleEFT.radarForm.ApplyButtonImagesFromModule();
            }
        }

        internal void Stop()
        {
            try
            {
                CleanUpViaButton();
                ModuleEFT.radarForm.Started = false;
                ModuleEFT.radarForm.mapManager.CurrentMap = string.Empty;
            }
            catch (Exception ex)
            {
                            }
            finally
            {
                ModuleEFT.radarForm.ApplyButtonImagesFromModule();
                HelperLogger.SaveLogToFile(false);
            }
        }

        internal void Init()
        {
            
            if (DebugClass.UseUserModeServer)
            {
                SynchronousSocketClient.InitClient(ModuleEFT.radarForm.settingsRadar.Network.ServerAddress);
                Memory.moduleBaseAddress = SynchronousSocketClient.ProcessOpen(ModuleEFT.radarForm.settingsRadar.Network.lastGameProcessID, "UnityPlayer.dll");
            }
            else
            {
                                SynchronousSocketDriverClient.InitClient(ModuleEFT.radarForm.settingsRadar.Network.ServerAddress, ModuleEFT.radarForm.settingsRadar.Network.ServerPort);

                                Memory.moduleBaseAddress = SynchronousSocketDriverClient.GetModuleBase(ModuleEFT.radarForm.settingsRadar.Network.lastGameProcessID, "UnityPlayer.dll");
            }

            GameObjectManager.GetGameObjectManager();
            TimeScale.GetTimeScale();

            Memory.DetectShortPtr(GameObjectManager.gameObjectManagerAddress);

            LootTime = CommonHelpers.dateTimeHolder.AddSeconds(-300);
            localPlayerFound = false;
            stopRequested = false;
            UpdateBasePointers = true;
            Memory.ResetFields();

            ModuleEFT.radarForm.mapManager.CurrentMap = string.Empty;
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

        private void UpdateLoot(object sender, EventArgs e)
        {
            staticLootDone = false;
            LootTime = CommonHelpers.dateTimeHolder;
        }

        private void CleanupGeneral()
        {
            if (lootList.Count > 0)
            {
                lock (lootList)
                {
                                        lootList.Clear();
                                    }
            }

            if (playersList.Count > 0)
            {
                lock (playersList)
                {
                                        playersList.Clear();
                                    }
            }

            if (grenadesList.Count > 0)
            {
                lock (grenadesList)
                {
                                        grenadesList.Clear();
                                    }
            }

            localGameWorld?.Cleanup();

            PlayersInfoTime = CommonHelpers.dateTimeHolder;

            fpsCamera = null;

            GameWorld.address = 0;

            pointer.LootEntityArray = 0;
            pointer.LootListPointer = 0;
            pointer.OpticCameraMatrixPointer = 0;
            pointer.OpticCameraPointer = 0;
            pointer.PlayerCount = 0;
            pointer.LootCount = 0;
            pointer.PlayerCountBOT = 0;
            pointer.PlayerCountPMC = 0;
            pointer.PlayerCountSCAV = 0;
            pointer.PlayersEntityArray = 0;
            pointer.PlayersListPointer = 0;
            pointer.ExfilPmcPointPointer = 0;
            pointer.ExfilScavPointPointer = 0;
            pointer.GrenadeEntityArray = 0;
            pointer.GrenadeListPointer = 0;

            IterateLootComplete = true;
            IteratePlayersComplete = true;
            localPlayerFound = false;
            followPlayerUpdateRequested = false;
            followPlayerUpdateNewGuid = "";
            staticLootDone = true;
            stopRequested = false;

            ModuleEFT.settingsForm.settingsJson.MemoryWriting.ThermalVision.Enabled = false;
        }

        internal void CleanUpViaEmpyGameworld()
        {
            
            CleanupGeneral();

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

        internal static EntityPlayer GetLocalFollowedPlayer()
        {
            var result = playersList.Find(x => x.isFollowThisPlayer == true);
            return result;
        }

        internal static EntityPlayer GetLocalPlayer()
        {
            var result = playersList.Find(x => x.isLocalPlayer == true);
            return result;
        }

        private void BuildFreshLootablesList(ulong entityArray)
        {
            if (staticLootDone && !ModuleEFT.settingsForm.settingsJson.Loot.LiveLoot)
            {
                return;
            }

                        
            if (pointer.LootEntityArray == 0)
            {
                
                return;
            }
            
            var lootListNew = new List<EntityLoot>();

            if (pointer.LootCount == 0)
            {
                return;
            }

            int chunkCurrent = 0;
            int chunkAmountStatic = SynchronousSocketDriverClient.MTU / sizeof(ulong);
            int chunkAmount = chunkAmountStatic;

            Memory.SlowMode = true;

            for (int i = 0; i < lootList.Count; i++)
            {
                lootList[i].notPresentTest = true;
            }

            while (chunkCurrent < pointer.LootCount)
            {
                var databuffer = Memory.ReadBytes(entityArray + 0x20 + (uint)chunkCurrent * 0x8, sizeof(ulong) * chunkAmount);
                
                for (uint i = 0; i < chunkAmount; i++)
                {
                    var lootObjectAdressPointer = entityArray + 0x20 + ((uint)chunkCurrent * 0x8);

                    var lootObjectAddress = BitConverter.ToUInt64(databuffer, (int)(i * sizeof(ulong)));
                    
                    if (Memory.IsValidPointer(lootObjectAddress) == false)
                    {
                                                chunkCurrent++;
                        continue;
                    }

                    var lootEntity = new EntityLoot(lootObjectAddress);

                    lootListNew.Add(lootEntity);

                    var indexCache = lootList.IndexOf(lootEntity);

                    if (indexCache < 0)
                    {
                        lootList.Add(lootEntity);
                    }
                    else
                    {
                        lootList[indexCache].notPresentTest = false;
                    }
                    chunkCurrent++;
                }

                if ((pointer.LootCount - chunkCurrent) < chunkAmountStatic)
                {
                    chunkAmount = pointer.LootCount - chunkCurrent;
                }
            }

            var entryCurrent = 0;
            var entryMax = ModuleEFT.settingsForm.settingsJson.Loot.LiveLootPerCycle;

            var varUnknownLoot = lootList.FindAll(r => r.ItemBasicName == null);

            if (varUnknownLoot.Count > 0)
            {
            }
            else
            {
                lootList.Sort((x, y) => DateTime.Compare(x.ExtraInfoUpdateTimeLast, y.ExtraInfoUpdateTimeLast));
            }

            for (int i = 0; i < lootList.Count; i++)
            {
                if (lootList[i].notPresentTest)
                {
                    lootList[i].notPresent = true;
                }

                if (lootList[i].notPresent)
                {
                    continue;
                }

                var indexTemp = lootListNew.IndexOf(lootList[i]);

                if (indexTemp >= 0)
                {
                    if (staticLootDone == false)
                    {
                        if (lootList[i].canReadData)
                        {
                            lootList[i].GetData();
                        }
                    }
                    else
                    {
                        if (ModuleEFT.settingsForm.settingsJson.Loot.LiveLoot)
                        {
                            if (entryCurrent <= entryMax)
                            {
                                if (lootList[i].ItemBasicName == null)
                                {
                                    if (lootList[i].canReadData)
                                    {
                                        entryCurrent++;
                                        lootList[i].GetData();
                                    }
                                }
                                else
                                {
                                    if (varUnknownLoot.Count == 0 && lootList[i].blacklist == false && lootList[i].CanUpdateLootOverTime() && lootList[i].containedItem != null)
                                    {
                                        if (lootList[i].canReadData)
                                        {
                                            entryCurrent++;
                                            lootList[i].GetData();
                                        }
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
            }

            Memory.SlowMode = false;

            if (staticLootDone == false)
            {
                staticLootDone = true;
            }
        }

        private void BuildFreshPlayerList(ulong entityArray)
        {
            if (pointer.PlayersEntityArray == 0)
            {
                return;
            }

            if (pointer.PlayerCount == 0)
            {
                return;
            }

            int chunkCurrent = 0;
            int chunkAmountStatic = SynchronousSocketDriverClient.MTU / sizeof(ulong);
            int chunkAmount = chunkAmountStatic;

            if (chunkAmount > pointer.PlayerCount)
            {
                chunkAmount = pointer.PlayerCount;
            }

            
            for (int i = 0; i < playersList.Count; i++)
            {
                playersList[i].notPresentTest = true;
            }

            while (chunkCurrent < pointer.PlayerCount)
            {
                var databuffer = Memory.ReadBytes(entityArray + 0x20 + (uint)chunkCurrent * 0x8, sizeof(ulong) * chunkAmount);
                
                for (uint i = 0; i < chunkAmount; i++)
                {
                    var playerObjectAddressPointer = entityArray + 0x20 + ((uint)chunkCurrent * 0x8);

                    var playerObjectAddress = BitConverter.ToUInt64(databuffer, (int)(i * sizeof(ulong)));
                    
                    if (Memory.IsValidPointer(playerObjectAddress) == false)
                    {
                                                chunkCurrent++;
                        continue;
                    }

                    var indexCache = playersList.FindIndex(t => t.playerAddress == playerObjectAddress);

                    if (indexCache < 0)
                    {
                        playersList.Add(new EntityPlayer(playerObjectAddressPointer, playerObjectAddress));
                    }
                    else
                    {
                        playersList[indexCache].notPresentTest = false;
                    }

                    chunkCurrent++;
                }

                if ((pointer.PlayerCount - chunkCurrent) < chunkAmountStatic)
                {
                    chunkAmount = pointer.PlayerCount - chunkCurrent;
                }
            }

            for (int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i].notPresentTest)
                {
                    playersList[i].notPresent = true;
                    playersList[i].DoStalePlayerAction();
                }
                else
                {
                    playersList[i].GetPlayerValues();
                }
            }

                    }

        private void FindLootCount()
        {
            if (GameWorldValid() == false)
            {
                
                return;
            }

            
            if (Memory.IsValidPointer((ulong)pointer.LootListPointer))
            {
                pointer.LootCount = Memory.Read<int>(pointer.LootListPointer + ModuleEFT.offsetsEFT.ListClassEntryCount);
                            }
            else
            {
                pointer.LootCount = 0;
                            }
        }

        private void GetPlayerCount()
        {
            if (GameWorldValid() == false)
            {
                
                return;
            }

            
            if (Memory.IsValidPointer((ulong)pointer.PlayersListPointer))
            {
                pointer.PlayerCount = Memory.Read<int>(pointer.PlayersListPointer + ModuleEFT.offsetsEFT.ListClassEntryCount);
                            }
            else
            {
                pointer.PlayerCount = 0;
                            }
        }

        private void FindPlayerListPointer()
        {
            if (GameWorldValid() == false)
            {
                                return;
            }

                        pointer.PlayersListPointer = Memory.Read<ulong>(localGameWorld.address + ModuleEFT.offsetsEFT.GameWorld_RegisteredPlayers);

            
            if (pointer.PlayersListPointer == 0)
            {
                                UpdateBasePointers = true;
            }
        }

        private void FindLootListPointer()
        {
            if (GameWorldValid() == false)
            {
                                return;
            }

            
            pointer.LootListPointer = Memory.Read<ulong>(localGameWorld.address + ModuleEFT.offsetsEFT.GameWorld_LootList);
            
            if (pointer.LootListPointer == 0)
            {
                                UpdateBasePointers = true;
            }
        }

        private void FindGrenadeListPointer()
        {
            if (GameWorldValid() == false)
            {
                return;
            }

            pointer.GrenadeListPointer = Memory.Read<ulong>(localGameWorld.address + 0xF8);

            if (pointer.GrenadeListPointer == 0)
            {
                                UpdateBasePointers = true;
            }
        }

        private void GetPointersBase(bool force = false)
        {
            
            if (force)
            {
                GameWorld.address = 0;
                pointer.ExfilPmcPointPointer = 0;
                pointer.ExfilScavPointPointer = 0;
                            }

            GameWorld.address = GameObjectManager.GetGameWorld();

            if (Memory.IsValidPointer(GameWorld.address))
            {
                GetPointers(force);
            }

                    }

        private void GetPointers(bool force = false)
        {
            
            if (force)
            {
                fpsCamera = null;

                pointer.PlayersListPointer = 0;
                pointer.PlayersEntityArray = 0;
                pointer.LootListPointer = 0;
                pointer.LootEntityArray = 0;
                pointer.GrenadeEntityArray = 0;
                pointer.GrenadeListPointer = 0;

                            }

            localGameWorld = GameWorld.GetLocalGameWorld();

            fpsCamera = null;

            FindPlayerListPointer();
            FindLootListPointer();
            FindGrenadeListPointer();
            GetPlayerCount();
            FindLootCount();

                    }

        internal bool GameWorldValid()
        {
            return (Memory.IsValidPointer(localGameWorld.address) && Memory.IsValidPointer(GameWorld.address));
        }

        private void IterateLoot()
        {
            

            if (GameWorldValid())
            {
                                pointer.LootEntityArray = Memory.Read<ulong>(pointer.LootListPointer + ModuleEFT.offsetsEFT.ListClass);
                            }

            if (Memory.IsValidPointer((ulong)pointer.LootEntityArray))
            {
                                BuildFreshLootablesList(pointer.LootEntityArray);
            }

            IterateLootComplete = true;
        }

        private void IteratePlayers()
        {
          
            if (pointer.PlayersEntityArray == 0 && GameWorldValid())
            {
                                pointer.PlayersEntityArray = Memory.Read<ulong>(pointer.PlayersListPointer + ModuleEFT.offsetsEFT.ListClass);
                            }

            if (Memory.IsValidPointer((ulong)pointer.PlayersEntityArray))
            {
                BuildFreshPlayerList(pointer.PlayersEntityArray);
            }

            IteratePlayersComplete = true;
        }

        private void NetworkReaderTimerTick(object source, ElapsedEventArgs e)
        {
            if (ModuleEFT.radarForm.Started)
            {
                if (Memory.stopOnError && Memory.InvalidAddressCount > 0)
                {
                    stopRequested = true;
                }
                

                if (CommonHelpers.dateTimeHolder > PointersTime && pointer.PlayerCount == 0 && stopRequested == false)
                {
                    
                    ModuleEFT.settingsForm.WriteMemoryFlyHackDisable();

                    TimeScale.DisableTimeScale();

                    ModuleEFT.radarForm.mapManager.CurrentMap = string.Empty;
                    CleanUpViaEmpyGameworld();
                    GetPointersBase(true);

                    PointersTime = CommonHelpers.dateTimeHolder.AddSeconds(PointersUpdateRateSec);
                                    }

                if (mainApplication == null)
                {
                    mainApplication = GameObjectManager.GetMainApplication();
                }
                else
                {
                    if (Memory.IsValidPointer(mainApplication.address))
                    {
                        if (mainApplication.noInertia == null)
                        {
                            mainApplication.noInertia = mainApplication.GetInertia();
                        }
                        else
                        {
                            mainApplication.noInertia.Check();
                        }
                    }
                }

                if (pointer.PlayerCount > 0 && stopRequested == false)
                {
                    
                    var localEntity = GetLocalPlayer();

                    
                    if ((CommonHelpers.dateTimeHolder > PlayersInfoTime) && IteratePlayersComplete && pointer.PlayerCount > 0)
                    {
                        GetPlayerCount();

                        FindLootCount();

                        localGameWorld.GetExfiltrationController();
                        localGameWorld.exfiltrationController.FindExfilPoints();

                        pointer.PlayerCountPMC = 0;
                        pointer.PlayerCountSCAV = 0;
                        pointer.PlayerCountBOT = 0;

                        IteratePlayersComplete = false;
                        IteratePlayers();
                        IterateGrenades();

                        PlayersInfoTime = CommonHelpers.dateTimeHolder.AddMilliseconds(UpdateRate);
                    }

                    if ((CommonHelpers.dateTimeHolder > LootTime) && IterateLootComplete && pointer.LootCount > 0 && pointer.PlayerCount > 0)
                    {
                        IterateLootComplete = false;

                        IterateLoot();

                        LootTime = CommonHelpers.dateTimeHolder.AddMilliseconds(LootTimeUpdateRate);
                    }

                    if (localPlayerFound)
                    {
                        if (CommonHelpers.dateTimeHolder > CameraTime)
                        {
                            if (fpsCamera == null || ModuleEFT.readerEFT.fpsCamera?.address == 0)
                            {
                                fpsCamera = GameObjectManager.GetFPSCamera();

                                if (fpsCamera != null)
                                {
                                    fpsCamera.transform = fpsCamera.GetCameraTransform();
                                    fpsCamera.cameraComponent = fpsCamera.GetCameraComponent();
                                }
                            }

                            CameraTime = CommonHelpers.dateTimeHolder.AddSeconds(CameraUpdateRateSec);
                        }

                        if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.ThermalVision.Enabled)
                        {
                            ModuleEFT.readerEFT.fpsCamera?.EnableThermalVision();
                        }
                        else
                        {
                            ModuleEFT.readerEFT.fpsCamera?.DisableThermalVision();
                        }

                        if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.NightVision.Enabled)
                        {
                            ModuleEFT.readerEFT.fpsCamera?.EnableNightVision();
                        }
                        else
                        {
                            ModuleEFT.readerEFT.fpsCamera?.DisableNightVision();
                        }

                        if (ModuleEFT.radarForm.overlay != null && ModuleEFT.radarForm.overlay.canRun && fpsCamera != null)
                        {
                                                        fpsCamera?.cameraComponent?.GetViewMatrix();
                            fpsCamera?.cameraComponent?.GetViewFov();
                                                    }
                    }

                                    }
            }
            else
            {
                pointer.PlayerCount = 0;
                pointer.LootCount = 0;
            }

            if (stopRequested == false && ModuleEFT.radarForm.Started == true && networkReaderTimer != null)
            {
                networkReaderTimer.Start();
            }
        }

        private void IterateGrenades()
        {
            if (pointer.GrenadeEntityArray == 0 && GameWorldValid())
            {
                pointer.GrenadeEntityArray = Memory.Read<ulong>(pointer.GrenadeListPointer + 0x18);
            }

            if (Memory.IsValidPointer((ulong)pointer.GrenadeEntityArray))
            {
                BuildFreshGrenadeList(pointer.GrenadeEntityArray);
            }
        }

        private void BuildFreshGrenadeList(ulong grenadeEntityArray)
        {
            if (pointer.GrenadeEntityArray == 0)
            {
                return;
            }

            var dictionaryEntry = Memory.Read<ulong>(pointer.GrenadeEntityArray + ModuleEFT.offsetsEFT.ListClass);
            var dictionaryEntriesCount = Memory.Read<uint>(pointer.GrenadeEntityArray + ModuleEFT.offsetsEFT.ListClassEntryCount);

            for (int i = 0; i < grenadesList.Count; i++)
            {
                grenadesList[i].notPresentTest = true;
            }

            for (uint i = 0; i < dictionaryEntriesCount; i++)
            {
                var nadeAddress = Memory.Read<ulong>(dictionaryEntry + 0x20 + (i * 0x8));

                var indexCache = grenadesList.FindIndex(t => t.address == nadeAddress);

                if (indexCache < 0)
                {
                    grenadesList.Add(new Grenade(nadeAddress));
                }
                else
                {
                    grenadesList[indexCache].notPresentTest = false;
                }
            }

            for (int i = 0; i < grenadesList.Count; i++)
            {
                if (grenadesList[i].notPresentTest)
                {
                    if (grenadesList[i].notPresent == false)
                    {
                        grenadesList[i].notPresent = true;
                        grenadesList[i].timeNotPresent = CommonHelpers.dateTimeHolder;
                    }

                    grenadesList[i].StaleObjectAction();
                }
                else
                {
                    grenadesList[i].GetData();
                }
            }
        }

        internal struct PointerStruct
        {
            internal ulong GrenadeListPointer;
            internal ulong GrenadeEntityArray;
            internal ulong LootEntityArray;
            internal ulong LootListPointer;
            internal ulong OpticCameraMatrixPointer;
            internal ulong OpticCameraPointer;

            internal int PlayerCount;
            internal int LootCount;

            internal int PlayerCountBOT;

            internal int PlayerCountPMC;
            internal int PlayerCountSCAV;
            internal ulong PlayersEntityArray;
            internal ulong PlayersListPointer;

            internal ulong ExfilPmcPointPointer;
            internal ulong ExfilScavPointPointer;
            private ulong CommonUIPointer;

            private ulong MainAppPointer;
        }
    }
}