using NormandyNET.Core;
using NormandyNET.Helpers;
using NormandyNET.Modules.EFT.Improvements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static NormandyNET.Core.MapManager;

namespace NormandyNET.Modules.EFT
{
    public class IEntityPlayer
    {
        internal Profile profile;
        internal Info info;
        internal Settings settings;
        internal PlayerBody playerBody;
        internal MovementContext movementContext;
        internal HealthController healthController;
        internal InventoryController inventoryController;
        internal ProceduralWeaponAnimation proceduralWeaponAnimation;
        internal Physical physical;
        internal CharacterController characterController;
        internal HandsController handsController;

        internal EntityPlayer localEntity;

        internal ImprovementsController improvementsController;

        internal ulong transformAddress;
        internal int TransformUpdateRate = 5;
        internal DateTime TransformUpdateRateTime;

        public PlayerTypeEFT playerType;

        public System.Drawing.Color Color;

        public UnityEngine.Vector3 Position;
        public UnityEngine.Vector2 ViewAngle;
        internal float? angleHorizontalDegrees;
        internal float? angleVerticalDegrees;

        public DateTime ResetTime;
        public DateTime PositionUpdateTime;
        public int[] PositionUpdateRate = new int[] { 1, 125, 500 };

        public bool isLocalPlayer = false;
        public bool isFollowThisPlayer = false;
        public bool? isTeammate;
        public bool IsDeadAlready;

        public int TeamIdx;
        public bool DumpComplete;
        public string Pose;
        public bool Cheater;
        public int HealthPercent;
        public bool isBoss;

        public bool isAtExfilPoint;
        public string ExfilPointName;
        internal DateTime ExfilPointLast;
        internal int ExfilPointTimeMax = 4;

        public bool canRender = false;
        public bool canDelete = false;
        public bool notPresentTest = false;
        public bool notPresent = false;
        public bool canReadData = true;

        public DateTime DoStalePlayerActionTimeLast;
        public int DoStalePlayerActionTimeLastRateMs = 500;

        internal int ExtraInfoUpdateMSec = 25000;
        internal DateTime ExtraInfoUpdateTimeLast;
        internal bool ExtraInfoUpdateAllowed;

        internal int FastInfoUpdateRate = 250;
        internal DateTime FastInfoTimeLast;

        internal bool updateColors;
        internal bool followPlayerUpdateRequested;

        internal DateTime WriteMemoryPeriodicChecks;
        internal bool WriteMemoryPeriodicChecksAllowed;

        internal bool wtsRender;

        internal DateTime entityFirstTimeInArc;
        internal DateTime entityLatestTimeInArc;
        internal bool entityInSightArc;
        internal float entityInSightArcAngleXZ;
        internal float entityInSightArcAngleXY;
        internal bool entityInSightFirstTime;

        internal Dictionary<WildSpawnType, string> bossEnumToName = new Dictionary<WildSpawnType, string>
        {
            { WildSpawnType.bossTest , "TestBoss" },
            { WildSpawnType.bossBully , "Reshala" },
            { WildSpawnType.bossKilla , "Killa" },
            { WildSpawnType.bossKojaniy , "Shturman" },
            { WildSpawnType.bossGluhar , "Glukhar" },
            { WildSpawnType.bossSanitar , "Sanitar" },
            { WildSpawnType.sectantPriest , "Priest" },
            { WildSpawnType.bossTagilla , "Tagilla" },
            { WildSpawnType.bossKnight , "Knight" },
            { WildSpawnType.followerBigPipe , "BigPipe" },
            { WildSpawnType.followerBirdEye , "BirdEye" }
        };

        internal bool debug = true;
        internal Regex weaponRegex = new Regex("(n.a)|weapon_|(izhmash_|izhmeh_|toz_|tochmash_|lobaev_|molot_)|_[0-9]+([0-9]|[a-zA-Z])+");
    }

    public class EntityPlayer : IEntityPlayer, IEquatable<EntityPlayer>
    {
        internal float distanceToFollowedPlayer;
        internal readonly ulong baseAddrPointer;
        internal readonly ulong playerAddress;

        public EntityPlayer(ulong addrPointer, ulong addr)
        {
            baseAddrPointer = addrPointer;
            playerAddress = addr;

            ExtraInfoUpdateTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(ModuleEFT.radarForm.fastRandom.Next(3000, 6000));
            PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0]);
        }

        public void CheckMapByElevation()
        {
            if (ModuleEFT.settingsForm.settingsJson.Map.AutoHeight && (isFollowThisPlayer == true) && !ModuleEFT.radarForm.mapManager.CurrentMap.Equals(string.Empty))
            {
                var desiredMapLevel = ModuleEFT.radarForm.mapManager.mapObjects[ModuleEFT.radarForm.mapManager.CurrentMap].GetMapLevelByElevation(Position.y);

                if (desiredMapLevel != MapLevels.Nothing)
                {
                    ModuleEFT.radarForm.mapManager.DesiredMapLevel = desiredMapLevel;
                }
            }
        }

        public bool Equals(EntityPlayer other)
        {
            if (playerAddress == other.playerAddress)
            {
                return true;
            }

            return false;
        }

        public void DoStalePlayerAction(bool asap = false)
        {
            return;
            if (CommonHelpers.dateTimeHolder > DoStalePlayerActionTimeLast || asap)
            {
                DoStalePlayerActionTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(DoStalePlayerActionTimeLastRateMs);

                if (localEntity.improvementsController.adsConfirmed)
                {
                    improvementsController.pinkDudes.ForceDePink();
                }
            }
        }

        public void GetPlayerValues()
        {
            if (notPresent)
            {
                return;
            }

                        
            if (CommonHelpers.dateTimeHolder > ExtraInfoUpdateTimeLast)
            {
                ExtraInfoUpdateAllowed = true;
                ExtraInfoUpdateTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(ExtraInfoUpdateMSec + ModuleEFT.radarForm.fastRandom.Next(3000, 6000));
            }
            else
            {
                ExtraInfoUpdateAllowed = false;
            }

            if (localEntity == null)
            {
                localEntity = ReaderEFT.GetLocalPlayer();
            }

            if (ModuleEFT.radarForm.overlay == null)
            {
                wtsRender = false;
            }

            GetPointers();

            if (canReadData)
            {
                GetCachedData();
                GetIsDeadAlready();
                CheckEmergecyLocalPlayerDeath();

                if (base.IsDeadAlready)
                {
                    notPresent = true;
                    return;
                }

                GetPlayerIsLocal();
                CheckIsTeammate();
                GetPlayerColor();
                CheckMapByElevation();

                GetPlayerPositionAndViewAngle();

                playerBody.GetPlayerBonesOverlay();
                inventoryController.GetPlayerInventory();
                inventoryController.GetFullPlayerInventory();
                DoOtherPlayerStuff();

                DetectIfPointIsInArc();
                DoLocalPlayerStuff();

                canRender = true;
            }

            if (debug)
            {
            }

            debug = false;
                    }

        private void CheckEmergecyLocalPlayerDeath()
        {
            if (localEntity != null && !localEntity.healthController.IsAlive())
            {
                PinkDudes.emergencyDepink = true;

                for (int i = 0; i < ReaderEFT.playersList.Count; i++)
                {
                    ReaderEFT.playersList[i].DoStalePlayerAction(true);
                }
            }
        }

        private void CheckIfPlayerAtExfilPoint()
        {
            if (ModuleEFT.readerEFT.localGameWorld != null && ModuleEFT.readerEFT.localGameWorld.exfiltrationController != null)
            {
                var atExfilPoint = ModuleEFT.readerEFT.localGameWorld.exfiltrationController.PlayerInExfilCircle(Position, info.Side, out string exfilName);

                if (atExfilPoint)
                {
                    if (ExfilPointName == exfilName)
                    {
                        if ((CommonHelpers.dateTimeHolder - ExfilPointLast).TotalSeconds > ExfilPointTimeMax)
                        {
                            isAtExfilPoint = true;
                        }
                        else
                        {
                            isAtExfilPoint = false;
                        }
                    }
                    else
                    {
                        ExfilPointName = exfilName;
                        ExfilPointLast = CommonHelpers.dateTimeHolder;
                    }
                }
                else
                {
                    isAtExfilPoint = false;
                }
            }
        }

        private void GetCachedData()
        {
            if (playerBody == null) { playerBody = new PlayerBody(this); }
            if (healthController == null) { healthController = new HealthController(this); }
            if (movementContext == null) { movementContext = new MovementContext(this); }

            if (inventoryController == null) { inventoryController = new InventoryController(this); }
            if (improvementsController == null) { improvementsController = new ImprovementsController(this); }
            if (characterController == null) { characterController = new CharacterController(this); }

            if (isLocalPlayer)
            {
                if (physical == null) { physical = new Physical(this); }
                if (handsController == null) { handsController = new HandsController(this); }
                if (proceduralWeaponAnimation == null) { proceduralWeaponAnimation = new ProceduralWeaponAnimation(this); }
            }
        }

        private void DetectIfPointIsInArc()
        {
            if (isLocalPlayer)
            {
                return;
            }

            if (localEntity == null)
            {
                return;
            }

            var length = Math.Max(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Distance, 2000U);

            var angleThreshold = 10;

            var localVectorXZ = new System.Numerics.Vector2(localEntity.Position.x, localEntity.Position.z);

            var projectedVectorXZ = new System.Numerics.Vector2(
              localEntity.Position.x +
                  (float)(Math.Cos(((localEntity.ViewAngle.x * Math.PI) / -180) + (Math.PI / 2)) * (int)length),
              localEntity.Position.z +
                  (float)(Math.Sin(((localEntity.ViewAngle.x * Math.PI) / -180) + (Math.PI / 2)) * (int)length));

            projectedVectorXZ -= localVectorXZ;

            var targetVectorXZ = new System.Numerics.Vector2(Position.x, Position.z) - localVectorXZ;
            var angleResultH = (float)Math.Abs(Geometry.AngleBetween(projectedVectorXZ, targetVectorXZ));

            var localVectorXY = new System.Numerics.Vector3(localEntity.Position.x, localEntity.Position.y, localEntity.Position.z);
            var targetVectorXY = new System.Numerics.Vector3(Position.x, Position.y, Position.z);
            var angleResultV = Math.Abs(Math.Abs(Geometry.CalcAngle(localVectorXY, targetVectorXY).Y) - Math.Abs(localEntity.ViewAngle.y));

            entityInSightArcAngleXZ = angleResultH;
            entityInSightArcAngleXY = angleResultV;

            if (angleResultH <= angleThreshold)
            {
                entityLatestTimeInArc = CommonHelpers.dateTimeHolder;

                if (entityInSightFirstTime)
                {
                    entityFirstTimeInArc = CommonHelpers.dateTimeHolder;
                    entityInSightFirstTime = false;
                }

                if (System.Math.Abs((entityFirstTimeInArc - entityLatestTimeInArc).TotalMilliseconds) > 500)
                {
                    entityInSightArc = true;
                    PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(-10000);
                }
                else
                {
                    entityInSightArc = false;
                }
            }
            else
            {
                entityInSightArc = false;
                entityInSightFirstTime = true;
            }
        }

        private void DoLocalPlayerStuff()
        {
            if (isLocalPlayer)
            {
                CheckIfPlayerAtExfilPoint();

                if (CommonHelpers.dateTimeHolder > WriteMemoryPeriodicChecks)
                {
                    WriteMemoryPeriodicChecksAllowed = true;
                    WriteMemoryPeriodicChecks = CommonHelpers.dateTimeHolder.AddMilliseconds(2000);
                }
                else
                {
                    WriteMemoryPeriodicChecksAllowed = false;
                }

                improvementsController.Check();

                if (ReaderEFT.swapInventoryRequested)
                {
                    ReaderEFT.swapInventoryRequested = false;

                    var newGear = ReaderEFT.playersList.Find(x => x.playerAddress == ReaderEFT.swapInventoryAddress);
                    if (newGear != null)
                    {
                        inventoryController.SwapGear(newGear);
                    }
                }

                if (ReaderEFT.teleportEntityRequested)
                {
                    ReaderEFT.teleportEntityRequested = false;

                    var smth = ReaderEFT.playersList.Find(x => x.playerAddress == ReaderEFT.teleporEntityAddress);
                    if (smth != null)
                    {
                        var tempPtr = Memory.ReadChain<ulong>(localEntity.playerAddress, new uint[] {
                            ModuleEFT.offsetsEFT.GameObjectEntry,
                            ModuleEFT.offsetsEFT.GameObject,
                            ModuleEFT.offsetsEFT.GameObject_ComponentsList,
                            ModuleEFT.offsetsEFT.GameObject_Component,
                            ModuleEFT.offsetsEFT.Transform_TransformAccess});

                        var databuffer = Memory.ReadBytes(tempPtr + ModuleEFT.offsetsEFT.TransformPosition, sizeof(float) * 3);
                        var Position = new UnityEngine.Vector3(
                           BitConverter.ToSingle(databuffer, 0x0),
                           BitConverter.ToSingle(databuffer, 0x4),
                           BitConverter.ToSingle(databuffer, 0x8));

                        var tempPtr2 = Memory.ReadChain<ulong>(smth.playerAddress, new uint[] {
                            ModuleEFT.offsetsEFT.GameObjectEntry,
                            ModuleEFT.offsetsEFT.GameObject,
                            ModuleEFT.offsetsEFT.GameObject_ComponentsList,
                            ModuleEFT.offsetsEFT.GameObject_Component,
                            ModuleEFT.offsetsEFT.Transform_TransformAccess});

                        var degressFOVcenter = ViewAngle.x;

                        var newX = (float)(Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * 2f);
                        var newZ = (float)(Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * 2f);

                        var newVectorToWrite = new System.Numerics.Vector3(Position.x + newX, Position.y, Position.z + newZ);

                        Memory.Write(tempPtr2 + ModuleEFT.offsetsEFT.TransformPosition + 0x0, newVectorToWrite);
                    }
                }

                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.MoveDo)
                {
                    ModuleEFT.settingsForm.settingsJson.MemoryWriting.MoveDo = false;

                    var positionX = Position.x;
                    var positionY = Position.y;
                    var positionZ = Position.z;

                    var degressFOVcenter = ViewAngle.x;
                    float newX, newZ;

                    var movestep = 1f;
                    System.Numerics.Vector3 newVectorToWrite;
                    switch (ModuleEFT.settingsForm.settingsJson.MemoryWriting.MoveDirection)
                    {
                        case Movements.Forward:
                            if (degressFOVcenter < 0)
                            {
                                degressFOVcenter = 360 + degressFOVcenter;
                            }

                            newX = (float)(Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                            newZ = (float)(Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);

                            newVectorToWrite = new System.Numerics.Vector3(positionX + newX, positionY, positionZ + newZ);
                            Memory.Write((ulong)transformAddress + ModuleEFT.offsetsEFT.TransformPosition, newVectorToWrite);
                            break;

                        case Movements.Backward:
                            if (degressFOVcenter < 0)
                            {
                                degressFOVcenter = 360 + degressFOVcenter;
                            }

                            newX = (float)(Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                            newZ = (float)(Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);

                            newVectorToWrite = new System.Numerics.Vector3(positionX - newX, positionY, positionZ - newZ);
                            Memory.Write((ulong)transformAddress + ModuleEFT.offsetsEFT.TransformPosition, newVectorToWrite);

                            break;

                        case Movements.Left:
                            degressFOVcenter = degressFOVcenter - 90;
                            if (degressFOVcenter < 0)
                            {
                                degressFOVcenter = 360 + degressFOVcenter;
                            }

                            newX = (float)(Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                            newZ = (float)(Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);

                            newVectorToWrite = new System.Numerics.Vector3(positionX + newX, positionY, positionZ + newZ);
                            Memory.Write((ulong)transformAddress + ModuleEFT.offsetsEFT.TransformPosition, newVectorToWrite);

                            break;

                        case Movements.Right:
                            degressFOVcenter = degressFOVcenter - 90;
                            if (degressFOVcenter < 0)
                            {
                                degressFOVcenter = 360 + degressFOVcenter;
                            }

                            newX = (float)(Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                            newZ = (float)(Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                            newVectorToWrite = new System.Numerics.Vector3(positionX - newX, positionY, positionZ - newZ);

                            Memory.Write((ulong)transformAddress + ModuleEFT.offsetsEFT.TransformPosition, newVectorToWrite);

                            break;

                        case Movements.Up:

                            break;

                        case Movements.Down:

                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private void DoOtherPlayerStuff()
        {
            if (!isLocalPlayer)
            {
                improvementsController.Check();

                if (CommonHelpers.dateTimeHolder > WriteMemoryPeriodicChecks)
                {
                    WriteMemoryPeriodicChecksAllowed = true;
                    WriteMemoryPeriodicChecks = CommonHelpers.dateTimeHolder.AddMilliseconds(2000 + ModuleEFT.radarForm.fastRandom.Next(2000, 4000));
                }
                else
                {
                    WriteMemoryPeriodicChecksAllowed = false;
                }

                if (WriteMemoryPeriodicChecksAllowed && distanceToFollowedPlayer < 300 && ModuleEFT.settingsForm.settingsJson.Entity.Health)
                {
                    if (entityInSightArc)
                    {
                        HealthPercent = healthController.GetHealthPecent();
                    }
                }
            }
        }

        private void GetPointers()
        {
                        if (profile == null) { profile = new Profile(this); }
            if (info == null) { info = new Info(this); }
            if (settings == null) { settings = new Settings(this); }

            if (!Memory.IsValidPointer(profile.address) || !Memory.IsValidPointer(info.address) || !Memory.IsValidPointer(settings.address))
            {
                canReadData = false;
                canRender = false;
            }

                    }

        private void GetPlayerIsLocal()
        {
            if (ReaderEFT.localPlayerFound == false)
            {
                                
                var value = Memory.Read<byte>(playerAddress + ModuleEFT.offsetsEFT.Player_IsLocalPlayer, false);
                
                if (value == (byte)1)
                {
                    isLocalPlayer = true;
                    isFollowThisPlayer = true;
                    localEntity = this;
                    ReaderEFT.localPlayerFound = true;
                    updateColors = true;
                }
                                                            }
        }

        internal bool GetIsDeadAlready(bool forceUpdate = false)
        {
            if (forceUpdate || CommonHelpers.dateTimeHolder > FastInfoTimeLast)
            {
                FastInfoTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(FastInfoUpdateRate);
                var value = Memory.Read<byte>(playerAddress + ModuleEFT.offsetsEFT.Player_IsDeadAlready, false);
                if (value == (byte)1)
                {
                    IsDeadAlready = true;
                }
            }

            return IsDeadAlready;
        }

        internal void GetPlayerIsFollowThisOne()
        {
            if (followPlayerUpdateRequested == true)
            {
                if (ReaderEFT.followPlayerUpdateNewGuid.Length > 0)
                {
                    if (playerAddress.ToString() == ReaderEFT.followPlayerUpdateNewGuid)
                    {
                        isFollowThisPlayer = true;
                    }
                    else
                    {
                        isFollowThisPlayer = false;
                    }

                    updateColors = true;
                }

                followPlayerUpdateRequested = false;
            }
        }

        private void CheckIsTeammate()
        {
            if (ReaderEFT.localPlayerFound == true && isTeammate == null)
            {
                if (info.GroupID == "n/a")
                {
                    TeamIdx = -1;
                }

                if (info.GroupID != "n/a" && ReaderEFT.localPlayerFound == true && isTeammate == null)
                {
                    if (info.GroupID == localEntity.info.GroupID)
                    {
                        isTeammate = true;
                        updateColors = true;
                    }
                    else
                    {
                        isTeammate = false;
                    }
                }
                else
                {
                    isTeammate = false;
                }
            }
        }

        internal void GetPlayerColor()
        {
            if (Color.IsEmpty || updateColors)
            {
                updateColors = false;

                Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.PMC;
                playerType = PlayerTypeEFT.Player;

                if (info != null && info.Side == EPlayerSide.Savage)
                {
                    if (info.RegistrationDate == 0)
                    {
                        if (settings.Role == WildSpawnType.assault)
                        {
                            Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Bot;
                            playerType = PlayerTypeEFT.Bot;
                        }
                        else if (settings.Role == WildSpawnType.gifter)
                        {
                            Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Special;
                            info.Nickname += $"SANTA";
                            playerType = PlayerTypeEFT.BotElite;
                        }
                        else if (bossEnumToName.TryGetValue(settings.Role, out string humanReadableName))
                        {
                            isBoss = true;
                            Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Boss;
                            playerType = PlayerTypeEFT.BotElite;
                            info.Nickname = $"{humanReadableName}";
                        }
                        else
                        {
                            Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotElite;
                            playerType = PlayerTypeEFT.BotElite;
                        }
                    }
                    else
                    {
                        Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotHuman;
                        playerType = PlayerTypeEFT.BotHuman;
                    }
                }

                if (info.MemberCategory != EMemberCategory.Default && info.MemberCategory != EMemberCategory.UniqueId)
                {
                    Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Special;
                    info.Nickname += $" {info.MemberCategory.ToString()}";
                }

                if (info.IsStreamerModeAvailable)
                {
                    Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Special;
                    info.Nickname += $" (Streamer)";
                }

                if (isTeammate == true && info.RegistrationDate != 0)
                {
                    Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Teammate;
                }

                if (isFollowThisPlayer == true)
                {
                    Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.You;
                }
            }
        }

        internal Vector2 GetPlayerViewAngle()
        {
            var databuffer = Memory.ReadBytes(movementContext.address + ModuleEFT.offsetsEFT.Player_MovementContext_AzimuthAndPosition, sizeof(float) * 2);

            return new UnityEngine.Vector2(BitConverter.ToSingle(databuffer, 0x0), BitConverter.ToSingle(databuffer, 0x4));
        }

        private void GetPlayerPositionAndViewAngle()
        {
            
            if (CommonHelpers.dateTimeHolder < PositionUpdateTime)
            {
                                return;
            }

            ViewAngle = GetPlayerViewAngle();

            
            if (CommonHelpers.dateTimeHolder > TransformUpdateRateTime)
            {
                TransformUpdateRateTime = CommonHelpers.dateTimeHolder.AddSeconds(TransformUpdateRate);

                var tempPtr = Memory.ReadChain<ulong>(playerAddress, new uint[] {
                ModuleEFT.offsetsEFT.GameObjectEntry,
                ModuleEFT.offsetsEFT.GameObject,
                ModuleEFT.offsetsEFT.GameObject_ComponentsList,
                ModuleEFT.offsetsEFT.GameObject_Component,
                ModuleEFT.offsetsEFT.Transform_TransformAccess});

                if (transformAddress == 0 || transformAddress != tempPtr)
                {
                    transformAddress = tempPtr;
                }
            }

            
            if (Memory.IsValidPointer((ulong)transformAddress))
            {
                var databuffer = Memory.ReadBytes(transformAddress + ModuleEFT.offsetsEFT.TransformPosition, sizeof(float) * 3);
                var PositionTemp = new UnityEngine.Vector3(
                   BitConverter.ToSingle(databuffer, 0x0),
                   BitConverter.ToSingle(databuffer, 0x4),
                   BitConverter.ToSingle(databuffer, 0x8));

                if (PositionTemp != Vector3.zero)
                {
                    Position = PositionTemp;
                }
            }

            PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0]);

            try
            {
                if (localEntity != null && localEntity.Position.x != 0 && localEntity.Position.z != 0)
                {
                    distanceToFollowedPlayer = Vector3.Distance(new Vector3(Position.x, 0, Position.z), new Vector3(localEntity.Position.x, 0, localEntity.Position.z));
                }
                else
                {
                    distanceToFollowedPlayer = 0;
                }

                if (entityInSightArc || isLocalPlayer || isFollowThisPlayer)
                {
                    PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0]);
                }
                else
                {
                    if (playerType == PlayerTypeEFT.Player || playerType == PlayerTypeEFT.BotHuman)
                    {
                        
                        PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0] + (int)Math.Round(Math.Ceiling(distanceToFollowedPlayer / 100) / 2 * distanceToFollowedPlayer, 0));
                    }
                    else
                    {
                        
                        PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[1] + (int)Math.Round(Math.Ceiling(distanceToFollowedPlayer / 100) / 2 * distanceToFollowedPlayer, 0));
                    }
                }
            }
            catch
            {
                
                PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[2]);
            }

                        return;
        }

        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }
    }
}