using NormandyNET.Core;
using NormandyNET.Helpers;
using NormandyNET.Settings;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace NormandyNET.Modules.ARMA
{
    public class EntityArma : IEntityPlayer, IEquatable<EntityArma>
    {
        internal readonly ulong baseAddrPointer;
        internal readonly ulong playerAddress;
        private float distanceToLocalPlayer;

        public EntityArma(ulong addrPointer, ulong addr)
        {
            baseAddrPointer = addrPointer;
            playerAddress = addr;
        }

        public bool Equals(EntityArma other)
        {
            if (playerAddress == other.playerAddress)
            {
                return true;
            }

            return false;
        }

        internal void GetPlayerValues()
        {
            
            canReadData = true;

            if (CommonHelpers.dateTimeHolder > DelayedChecksTimeLast)
            {
                DelayedChecksAllowed = true;
                DelayedChecksTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(DelayedChecksUpdateMSeconds + ModuleARMA.radarForm.fastRandom.Next(3000, 5000));
            }
            else
            {
                DelayedChecksAllowed = false;
            }

            if (CommonHelpers.dateTimeHolder > ExtraInfoUpdateTimeLast)
            {
                ExtraInfoUpdateAllowed = true;
                ExtraInfoUpdateTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(ExtraInfoUpdateMSec + ModuleARMA.radarForm.fastRandom.Next(3000, 6000));
            }
            else
            {
                ExtraInfoUpdateAllowed = false;
            }

            if (tableType == TableType.Bullet)
            {
            }

            GetBase();
            GetTypeName();

            GetCSVData();

            GetEntityPosition();
            GetModelName();
            GetPlayerRotationLocalPlayer();

            GetConfigName();
            GetCleanName();
            GetSimulationName();
            GetWeapons();
            GetIsDead();
            GetNetworkId();
            GetTeamIdAkaSide();

            GetPassengers();

            GetPlayerIsLocal();
            GetPlayerNickName();
            GetColor();
            GetIcon();
            GetCanRender();
            GetIsImportant();

                    }

        private void GetPassengers()
        {
            if (passengerEntity)
            {
                return;
            }

            if (EntityType != null && EntityType.Equals("Vehicle"))
            {
                passengers.Clear();
                turrets.Clear();

                var driverPtr = Memory.Read<ulong>(playerAddress + ModuleARMA.offsetsARMA.Entity_Driver, false, true);
                
                var passengersPtr = Memory.Read<ulong>(playerAddress + ModuleARMA.offsetsARMA.Entity_Passengers, false, true);
                
                var passengersSeatCount = Memory.Read<int>(playerAddress + ModuleARMA.offsetsARMA.Entity_Passengers + 0x8);
                
                var turretPtr = Memory.Read<ulong>(playerAddress + ModuleARMA.offsetsARMA.Entity_Turret, false, true);
                
                var turretsSeatCount = Memory.Read<int>(playerAddress + ModuleARMA.offsetsARMA.Entity_Turret + 0x8);
                
                if (driverPtr != 0)
                {
                    var driverEntity = new EntityArma(playerAddress + ModuleARMA.offsetsARMA.Entity_Driver, driverPtr);
                    driverEntity.passengerEntity = true;
                    driver = driverEntity;
                    driver.GetPlayerValues();
                }

                for (uint ipassenger = 0; ipassenger < passengersSeatCount; ipassenger++)
                {
                    var passengerEntityPtr = Memory.Read<ulong>(passengersPtr + ipassenger * 0x8, false, true);

                    if (passengerEntityPtr == 0)
                    {
                        continue;
                    }

                    var passengerEntity = new EntityArma(passengersPtr + ipassenger, passengerEntityPtr);
                    passengerEntity.passengerEntity = true;
                    passengerEntity.GetPlayerValues();
                    passengers.Add(passengerEntity);
                }

                for (uint igunner = 0; igunner < turretsSeatCount; igunner++)
                {
                    var turrentEntityPtr = Memory.Read<ulong>(turretPtr + igunner * 0x8, false, true);

                    if (turrentEntityPtr == 0)
                    {
                        continue;
                    }

                    var turretMannerPtr = Memory.Read<ulong>(turrentEntityPtr + ModuleARMA.offsetsARMA.Entity_TurretManner, false, true);

                    if (turretMannerPtr == 0)
                    {
                        continue;
                    }

                    var turretMannerEntity = new EntityArma(turrentEntityPtr + ModuleARMA.offsetsARMA.Entity_TurretManner, turretMannerPtr);
                    turretMannerEntity.passengerEntity = true;
                    turretMannerEntity.GetPlayerValues();
                    turrets.Add(turretMannerEntity);
                }

                if (driver != null)
                {
                    PassengersNames = $"D: {driver.Nickname}";
                    PassengersCount = 1;
                }

                foreach (var ent in passengers)
                {
                    PassengersNames += $"\nP: {ent.Nickname}";
                    PassengersCount++;
                }

                foreach (var ent in turrets)
                {
                    PassengersNames += $"\nT: {ent.Nickname}";
                    PassengersCount++;
                }

                                                                                            }
        }

        private void GetTeamIdAkaSide()
        {
            if (canReadData == false)
            {
                return;
            }

            
            if (EntityType.Equals("Soldier"))
            {
                var sideByte = Memory.Read<byte>(playerAddress + ModuleARMA.offsetsARMA.EntityTeamId);

                switch (sideByte)
                {
                    case 1:
                        side = Side.BLUEFOR;

                        break;

                    case 0:
                        side = Side.OPFOR;
                        break;

                    case 2:
                        side = Side.INDI;
                        break;

                    case 3:
                        side = Side.CIV;
                        break;

                    default:
                        break;
                }
            }

                    }

        private void GetIcon()
        {
            if (Icon == IconPositionTexture.none || updateColors)
            {
                                
                updateColors = false;

                if (EntityType == null)
                {
                    return;
                }

                if (EntityType.Equals("Loot") == false)
                {
                    ignoreDirection = false;

                    switch (EntityType)
                    {
                        case string a when a.Contains("Animal"):
                            Icon = IconPositionTexture.animal;
                            ignoreDirection = true;
                            break;

                        case string a when a.Contains("Loot"):
                            Icon = IconPositionTexture.loot;
                            renderLayer = RenderLayers.LootPriority1;
                            ignoreDirection = true;
                            break;

                        case string a when a.Contains("Soldier"):
                            if (isDead)
                            {
                                Icon = IconPositionTexture.statictech;
                                ignoreDirection = true;
                            }
                            else
                            {
                                Icon = IconPositionTexture.player;
                            }

                            renderLayer = RenderLayers.PlayersPriorityLow;
                            break;

                        case string a when a.Contains("Vehicle"):
                            Icon = IconPositionTexture.vehicle;
                            break;

                        case string a when a.Contains("You"):
                            Icon = IconPositionTexture.player;
                            renderLayer = RenderLayers.You;
                            break;

                        default:
                            Icon = IconPositionTexture.unknown;
                            break;
                    }
                }

                            }
        }

        internal void GetCanRender()
        {
            if (updateRenderStatus)
            {
                if (EntityType != null)
                {
                    canRender = false;

                    if (EntityType.Equals("Loot") == false)
                    {
                        if (ModuleARMA.settingsForm.settingsJson.Entity.EntityTypesSuppressed.Contains(EntityType))
                        {
                            canRender = false;
                            updateRenderStatus = false;
                            return;
                        }
                        else
                        {
                            canRender = true;
                        }
                    }

                    if (EntityType.Equals("Loot") == true)
                    {
                        if (LootItemHelper.LootFriendlyNamesToShow.Count > 0)
                        {
                            if (FriendlyName != null && LootItemHelper.LootFriendlyNamesToShow.Contains(FriendlyName))
                            {
                                canRender = true;
                            }
                        }
                        else
                        {
                            if (Category != null && !ModuleARMA.settingsForm.settingsJson.Loot.LootCategorySuppressed.Contains(Category))
                            {
                                canRender = true;
                            }
                        }
                    }

                    updateRenderStatus = false;
                }
            }
        }

        internal void GetColor()
        {
            if (Color.IsEmpty || ColorSide.IsEmpty || updateColors)
            {
                                
                updateColors = false;

                Color = System.Drawing.Color.White;
                ColorSide = System.Drawing.Color.White;

                if (EntityType == null)
                {
                    return;
                }

                if (EntityType.Equals("Loot") == false)
                {
                    if (ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.TryGetValue(EntityType, out System.Drawing.Color entityColor))
                    {
                                                Color = entityColor;
                    }
                    else
                    {
                                            }
                }

                if (EntityType.Equals("Loot") == true)
                {
                    if (ModuleARMA.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(Category, out System.Drawing.Color entityColor))
                    {
                        
                        Color = entityColor;
                    }
                    else
                    {
                                            }
                }

                if (EntityType.Equals("Soldier") == true || EntityType.Equals("Vehicle") == true)
                {
                    showSide = true;
                    switch (side)
                    {
                        case Side.BLUEFOR:
                            ColorSide = ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.OtherColors["Side - BLUEFOR"];
                            break;

                        case Side.OPFOR:
                            ColorSide = ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.OtherColors["Side - OPFOR"];
                            break;

                        case Side.CIV:
                            ColorSide = ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.OtherColors["Side - CIV"];
                            break;

                        case Side.INDI:
                            ColorSide = ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.OtherColors["Side - INDI"];
                            break;
                    }

                    if (isLocalPlayer)
                    {
                        showSide = false;
                    }
                }

                            }
        }

        private void GetBase()
        {
            
            if (objectBase == null)
            {
                                objectBase = Memory.Read<ulong>(playerAddress + ModuleARMA.offsetsARMA.Entity_Type);
                
                if (objectBase == 0)
                {
                    canReadData = false;
                                    }
            }

                    }

        private void GetSimulationName()
        {
            if (canReadData == false)
            {
                return;
            }

            if (SimulationName == null)
            {
                                var address = Memory.Read<ulong>((ulong)objectBase + ModuleARMA.offsetsARMA.Entity_SimulationName);
                                if (Memory.IsValidPointer(address))
                {
                    SimulationName = ArmaString.GetString(address);
                }
                else
                {
                    SimulationName = "NULL";
                }

                                            }
        }

        private void GetCleanName()
        {
            CleanName = "PLACEHOLDER";
        }

        private void GetConfigName()
        {
            ConfigName = "PLACEHOLDER";
        }

        private void GetCSVData()
        {
            if (canReadData == false)
            {
                return;
            }

            if (FriendlyName == null)
            {
                                var lootCSV = LootItemHelper.GetLootFromCSV(TypeName);
                FriendlyName = lootCSV.FriendlyName;
                EntityType = lootCSV.EntityType;
                Category = lootCSV.Category;

                                                            }
        }

        internal VisualState GetFutureVisualState()
        {
            
            if (canReadData == false)
            {
                return new VisualState(0);
            }

            ulong futureVisualState = Memory.Read<ulong>(playerAddress + ModuleARMA.offsetsARMA.Entity_FutureVisualState);
            
            if (futureVisualState != 0)
            {
                return new VisualState(futureVisualState);
            }

            return new VisualState(0);

                    }

        internal VisualState GetRenderVisualState()
        {
            
            if (canReadData == false)
            {
                return new VisualState(0);
            }

            ulong renderVisualState = Memory.Read<ulong>(playerAddress + ModuleARMA.offsetsARMA.Renderer_VisualState);
            
            if (renderVisualState != 0)
            {
                return new VisualState(renderVisualState);
            }

            return new VisualState(0);

                    }

        private void GetEntityPosition()
        {
            if (canReadData == false)
            {
                return;
            }

            
            if (CommonHelpers.dateTimeHolder < PositionUpdateTime && wtsRender == false)
            {
                
                return;
            }

            ulong renderVisualState = Memory.Read<ulong>(playerAddress + ModuleARMA.offsetsARMA.Renderer_VisualState);
            
            if (renderVisualState != 0)
            {
                if (isLocalPlayer == false)
                {
                    var databuffer = Memory.ReadBytes((ulong)renderVisualState + ModuleARMA.offsetsARMA.VisualState_Direction, sizeof(float) * 6);

                    Vector3 PositionNew = new Vector3(
                           BitConverter.ToSingle(databuffer, 0x0 + 0xC),
                           BitConverter.ToSingle(databuffer, 0x4 + 0xC),
                           BitConverter.ToSingle(databuffer, 0x8 + 0xC));

                    Position = PositionNew;

                    Vector3 DirectionNew = new Vector3(
                        BitConverter.ToSingle(databuffer, 0x0),
                        BitConverter.ToSingle(databuffer, 0x4),
                        BitConverter.ToSingle(databuffer, 0x8));
                    Direction = DirectionNew;
                }
            }
            else
            {
                canReadData = false;
                return;
            }

            if (Position.X == 0 && Position.Y == 0 && Position.Z == 0)
            {
                ulong manVisualState = Memory.Read<ulong>(playerAddress + ModuleARMA.offsetsARMA.Entity_FutureVisualState);
                
                if (manVisualState != 0)
                {
                    if (isLocalPlayer == false)
                    {
                        var databuffer = Memory.ReadBytes((ulong)manVisualState + ModuleARMA.offsetsARMA.VisualState_Direction, sizeof(float) * 6);

                        Vector3 PositionNew = new Vector3(
                               BitConverter.ToSingle(databuffer, 0x0 + 0xC),
                               BitConverter.ToSingle(databuffer, 0x4 + 0xC),
                               BitConverter.ToSingle(databuffer, 0x8 + 0xC));

                        Position = PositionNew;

                        Vector3 DirectionNew = new Vector3(
                            BitConverter.ToSingle(databuffer, 0x0),
                            BitConverter.ToSingle(databuffer, 0x4),
                            BitConverter.ToSingle(databuffer, 0x8));
                        Direction = DirectionNew;
                    }
                }
                else
                {
                    canReadData = false;
                    return;
                }
            }

            PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0]);

            if (isLocalPlayer == false)
            {
                try
                {
                    var localPlayerPosition = Vector3.Zero;
                    var localPlayer = ModuleARMA.readerARMA.GetLocalPlayer();

                    if (localPlayer != null)
                    {
                        localPlayerPosition = localPlayer.Position;
                    }

                    if (localPlayerPosition.X != 0 && localPlayerPosition.Z != 0)
                    {
                        distanceToLocalPlayer = Vector3.Distance(new Vector3(Position.X, 0, Position.Z), new Vector3(localPlayerPosition.X, 0, localPlayerPosition.Z));
                    }
                    else
                    {
                        distanceToLocalPlayer = 0;
                    }

                    if (EntityType.Equals("Loot"))
                    {
                        PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(ModuleARMA.radarForm.fastRandom.Next(PositionUpdateRate[2], PositionUpdateRate[3]) + (int)Math.Round(Math.Ceiling(distanceToLocalPlayer / 100) / 2 * distanceToLocalPlayer, 0));
                    }
                    else
                    {
                        PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0] + (int)Math.Round(Math.Ceiling(distanceToLocalPlayer / 100) / 2 * distanceToLocalPlayer, 0));
                    }
                }
                catch
                {
                    
                    PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[2] + (int)Math.Round(Math.Ceiling(distanceToLocalPlayer / 100) / 2 * distanceToLocalPlayer, 0));
                }
            }

                                }

        private void GetIsImportant()
        {
            if (canReadData == false)
            {
                return;
            }
            if (isImportant == null)
            {
                
                if (EntityType.Equals("Soldier") || EntityType.Equals("You"))
                {
                    isImportant = true;
                }
                else
                {
                    isImportant = false;
                }
            }
        }

        private void GetWeapons()
        {
            if (canReadData == false)
            {
                return;
            }

            if (DelayedChecksAllowed == false)
            {
                                return;
            }

            
            if (EntityType.Equals("Soldier"))
            {
                
                                var weaponSlotsManager = Memory.Read<ulong>(playerAddress + ModuleARMA.offsetsARMA.Entity_WeaponSlotsManager, false, true);
                
                                var weaponInHands = Memory.Read<ulong>(weaponSlotsManager + ModuleARMA.offsetsARMA.WeaponInHands, false, true);
                
                                var weaponId = Memory.Read<int>(weaponInHands + ModuleARMA.offsetsARMA.WeaponId, false, true);
                
                                var weaponIdToWeaponType = Memory.Read<ulong>(weaponInHands + ModuleARMA.offsetsARMA.WeaponIdToWeaponType, false, true);
                
                                var currentWeaponType = Memory.Read<ulong>(weaponIdToWeaponType + (ulong)(weaponId * 40 + 8));
                
                                var weaponNamePtr = Memory.Read<ulong>(currentWeaponType + ModuleARMA.offsetsARMA.WeaponNamePtr, false, true);
                
                Weapons = ArmaString.GetString(weaponNamePtr).CleanInput();
                            }

                    }

        private void GetIsDead()
        {
            if (canReadData == false)
            {
                return;
            }

            
            var isDeadByte = Memory.Read<byte>(playerAddress + ModuleARMA.offsetsARMA.Entity_IsDead);

            if (isDeadByte == 0x1)
            {
                isDead = true;
            }
            else
            {
                isDead = false;
            }

                    }

        private void GetModelName()
        {
            ModelName = "PLACEHOLDER";
        }

        private void GetNetworkId()
        {
            if (canReadData == false)
            {
                return;
            }

            if (networkId == null && EntityType.Equals("Soldier"))
            {
                                networkId = Memory.Read<uint>(playerAddress + ModuleARMA.offsetsARMA.Entity_NetworkId);
                                            }
        }

        private void GetPlayerIsLocal()
        {
            if (canReadData == false)
            {
                return;
            }

            
            if (passengerEntity == false && EntityType.Equals("Soldier") && (ModuleARMA.readerARMA.localPlayerFound == false || DelayedChecksAllowed))
            {
                                
                World.GetLocalEntity();

                var distance = Vector3.Distance(World.GetLocalEntityPosition(), Position);

                if (distance < 3)
                {
                    ModuleARMA.readerARMA.localPlayerFound = true;
                    isLocalPlayer = true;
                    updateColors = true;
                    EntityType = "You";
                }

                                            }
        }

        private void GetPlayerNickName()
        {
            if (canReadData == false)
            {
                return;
            }

            if (Nickname == null)
            {
                if (EntityType.Equals("Soldier"))
                {
                    if (NetworkClient.networkClient != 0 && networkId != null)
                    {
                        if (NetworkClient.playersList.TryGetValue((uint)networkId, out ulong playerIdentityAddress))
                        {
                            var player_name_ptr = Memory.Read<ulong>(playerIdentityAddress + ModuleARMA.offsetsARMA.PlayerIdentitySizePlayerName);
                                                        if (Memory.IsValidPointer(player_name_ptr))
                            {
                                var playerName = ArmaString.GetString(player_name_ptr);
                                                                Nickname = playerName;
                            }
                            else
                            {
                                Nickname = "!read error";
                            }
                            return;
                        }
                        else
                        {
                            Nickname = $"{networkId} - {TypeName}";
                            return;
                        }
                    }
                    else
                    {
                        Nickname = $"{FriendlyName} - {TypeName}"; ;
                    }
                }

                if (EntityType.Equals("Unknown"))
                {
                    Nickname = TypeName;
                    return;
                }

                Nickname = $"{FriendlyName}";
                return;
            }
        }

        private void GetPlayerRotationLocalPlayer()
        {
            if (isLocalPlayer == false || !canReadData || EntityType.Equals("You") == false)
            {
                return;
            }

            if (Camera.camera != 0)
            {
                
                var databuffer = Memory.ReadBytes(Camera.camera + 0x20, sizeof(float) * 6);

                Vector3 PositionNew = new Vector3(
                       BitConverter.ToSingle(databuffer, 0x0 + 0xC),
                       BitConverter.ToSingle(databuffer, 0x4 + 0xC),
                       BitConverter.ToSingle(databuffer, 0x8 + 0xC));

                Position = PositionNew;

                Vector3 DirectionNew = new Vector3(
                    BitConverter.ToSingle(databuffer, 0x0),
                    BitConverter.ToSingle(databuffer, 0x4),
                    BitConverter.ToSingle(databuffer, 0x8));
                Direction = DirectionNew;

                            }
            else
            {
            }
        }

        private void GetTypeName()
        {
            if (canReadData == false)
            {
                return;
            }

            if (TypeName == null)
            {
                                var address = Memory.Read<ulong>((ulong)objectBase + ModuleARMA.offsetsARMA.Entity_Type_TypeName);
                                if (Memory.IsValidPointer(address))
                {
                    TypeName = ArmaString.GetString(address);
                }
                else
                {
                    TypeName = "NULL";
                }

                if (TypeName.Equals("Particle"))
                {
                    canReadData = false;
                    canRender = false;
                    ModelName = "NULL";
                    ConfigName = "NULL";
                    CleanName = "NULL";
                }

                                            }
        }

        internal void GenerateRenderItem()
        {
            if (ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged)
            {
                updateColors = true;
                GetColor();
            }

            if (ModuleARMA.settingsForm.settingsJson.Entity.ShowStatusChanged)
            {
                updateRenderStatus = true;
                GetCanRender();
            }

            if (EntityType == null || EntityType.Equals("Loot") || EntityType.Equals("NULL") || !canRender)
            {
                return;
            }

            if (!ModuleARMA.settingsForm.settingsJson.Entity.Bodies && isDead)
            {
                return;
            }

            if (isLocalPlayer == false)
            {
            }

            if (Position == Vector3.Zero)
            {
                return;
            }

            var entityPosXingame = Position.X;
            var entityPosYingame = Position.Y;
            var entityPosZingame = Position.Z;

            var entityPosXmap = (entityPosXingame * OpenGL.CanvasDiffCoeff);
            var entityPosZmap = (entityPosZingame * OpenGL.CanvasDiffCoeff);
            var entityPosYmap = entityPosYingame;

            Vector3 vectorTarget = new Vector3(entityPosXingame, 0, entityPosZingame);
            Vector3 vectorMe = new Vector3(CommonHelpers.myIngamePositionX, 0, CommonHelpers.myIngamePositionZ);
            var distance = (int)Math.Round(Vector3.Distance(vectorTarget, vectorMe));

            if (!ModuleARMA.radarForm.IsVisibleOnControl(entityPosXmap, entityPosZmap, 0))
            {
                if (ModuleARMA.settingsForm.settingsJson.Map.ProximityAlert && EntityType.Equals("Soldier") && distance < 200)
                {
                    /*
                      * Вообще всё проще.
                         Пусть A = (x1,y1) - начало отрезка, B = (x2,y2) - конец, тогда они образуют вектор a = {x2-x1, y2 - y1}.
                         Пусть так же есть точка C удалённая на расстояние d, по данному отрезку от точки A, тогда чтобы получить координаты точки C надо "сдвинуть" точку A на расстояние d по напровлению вектора a. Формально это записывается вот так:
                         OC = OA + d * a/||a||, ||a|| - норма вектора a(длина отрезка AB). Можно расписать это в числах и по координатно:
                         x[3] = x1 + d * (x2-x1) / sqrt( (x2-x1)^2 + (y2 - y1)^2 )
                         y[3] = y1 + d * (y2-y1) / sqrt( (x2-x1)^2 + (y2 - y1)^2 )
                      * */

                    var myIngamePositionXmap = CommonHelpers.myIngamePositionX * OpenGL.CanvasDiffCoeff;
                    var myIngamePositionYmap = CommonHelpers.myIngamePositionY * OpenGL.CanvasDiffCoeff;
                    var myIngamePositionZmap = CommonHelpers.myIngamePositionZ * OpenGL.CanvasDiffCoeff;

                    var pnt = ModuleARMA.radarForm.GetControlEdgeIntersectionPoint(entityPosXmap, entityPosZmap);

                    renderItem = new RenderItem();
                    renderItem.Text = "line";
                    renderItem.MapPosX = entityPosXmap;
                    renderItem.MapPosZ = entityPosZmap;
                    renderItem.MapPosXend = pnt.X;
                    renderItem.MapPosZend = pnt.Y;
                    renderItem.DrawColor = Color;
                    renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                    renderItem.Size = 2;
                    OpenGL.MapGeometry.Add(renderItem);

                    renderItem = new RenderItem();
                    renderItem.IconPositionTexture = IconPositionTexture.statictech;
                    renderItem.MapPosX = pnt.X;
                    renderItem.MapPosZ = pnt.Y;
                    renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                    renderItem.DrawColor = Color;
                    renderItem.Rotation = 0;
                    renderItem.IconSize = ModuleARMA.settingsForm.settingsJson.Map.IconSizePlayers;
                    OpenGL.MapIcons.Add(renderItem);

                    renderItem = new RenderItem();
                    renderItem.MapPosX = pnt.X + OpenGL.CanvasDiffCoeff;
                    renderItem.MapPosZ = pnt.Y;
                    renderItem.DrawColor = ModuleARMA.settingsForm.settingsJson.Colors.ColorText;
                    renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                    renderItem.TextOverlayOutline = true;
                    renderItem.Size = (int)FontSizes.misc;
                    renderItem.Text = $"{distance}";
                    OpenGL.MapText.Add(renderItem);
                }
                else
                {
                    return;
                }
            }


            var viewAngle = 0f;

            if (ignoreDirection == false)
            {
                viewAngle = Geometry.CalculateDirectionToDegress(Direction);
            }

            if (ModuleARMA.settingsForm.settingsJson.Entity.LOS && isImportant == true && !isDead)
            {
                DrawFOVLines(viewAngle, entityPosXmap, entityPosZmap);
            }

            renderItem = new RenderItem();

            RenderLayers renderLayer;
            renderLayer = RenderLayers.PlayersPriorityLow;

            renderItem.IconSize = ModuleARMA.settingsForm.settingsJson.Map.IconSizePlayers;

            renderItem.IconPositionTexture = Icon;
            renderItem.renderLayer = renderLayer;
            renderItem.Rotation = viewAngle;
            renderItem.MapPosX = entityPosXmap;
            renderItem.MapPosZ = entityPosZmap;
            renderItem.DrawColor = Color;

            if (isDead == true)
            {
                renderItem.IconPositionTexture = IconPositionTexture.npc_dead;
            }

            OpenGL.MapIcons.Add(renderItem);

            renderItem = new RenderItem();
            renderItem.MapPosX = entityPosXmap + 20 + OpenGL.CanvasDiffCoeff;
            renderItem.MapPosZ = entityPosZmap + 15;
            renderItem.DrawColor = ModuleARMA.settingsForm.settingsJson.Colors.ColorText;
            renderItem.renderLayer = renderLayer;
            renderItem.TextOverlayOutline = true;
            renderItem.Size = (int)FontSizes.misc;

            renderItem.Text = $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.ColorText)}";

            var newLine = false;

            if (ModuleARMA.settingsForm.settingsJson.Entity.Distance && !isLocalPlayer)
            {
                renderItem.Text += CommonHelpers.GetDistanceUnit(distance);
                newLine = true;
            }

            var elevation = Math.Round((Position.Y - CommonHelpers.myIngamePositionY), 0);

            var offsetElevationType = 0;

            if (ModuleARMA.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Absolute))
            {
                renderItem.Text += $"({(int)Position.Y}) ";
                newLine = true;
            }

            if (ModuleARMA.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Relative))
            {
                renderItem.Text += $"({elevation}) ";
                newLine = true;
            }

            if (ModuleARMA.settingsForm.settingsJson.Entity.Elevation.Arrows)
            {
                renderItem.Text += CommonHelpers.GetElevationData(elevation);
            }

            if (newLine)
            {
                renderItem.Text += $"\n";
            }

            if (ModuleARMA.settingsForm.settingsJson.Entity.Name && !isLocalPlayer)
            {
                if (isDead == false)
                {
                    renderItem.Text += $"©{CommonHelpers.ColorHexConverter(Color)}";
                    renderItem.Text += $"{Nickname}\n";

                    if (EntityType.Equals("Vehicle"))
                    {
                        if (ModuleARMA.settingsForm.settingsJson.Entity.VehiclePassengers)
                        {
                            renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.ColorText)}P: {PassengersCount}\n";
                            if (ModuleARMA.settingsForm.settingsJson.Entity.VehiclePassengersDetailed == false)
                            {
                                renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.ColorText)}P: {PassengersCount}\n";
                            }
                            else
                            {
                                renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.ColorText)}{PassengersNames}\n";
                            }
                        }
                    }
                }
                else
                {
                    switch (EntityType)
                    {
                        case "Soldier":

                            break;
                    }
                }
            }

            if (ModuleARMA.settingsForm.settingsJson.Entity.Weapon && Weapons.Length > 0 && !isLocalPlayer)
            {
                renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.ColorText)}{Weapons.Truncate(22)}\n";
            }

            if (DebugClass.Debug)
            {
                renderItem.Text += $"{tableType}\n";
            }

            OpenGL.MapText.Add(renderItem);

            if (true && showSide)
            {
                renderItem = new RenderItem();
                renderItem.IconSize = Math.Max(0.4f, ModuleARMA.settingsForm.settingsJson.Map.IconSizePlayers - 0.4f);

                renderItem.IconPositionTexture = IconPositionTexture.statictech;
                renderItem.renderLayer = renderLayer + 1;
                renderItem.Rotation = viewAngle + 45;
                renderItem.MapPosX = entityPosXmap;
                renderItem.MapPosZ = entityPosZmap;
                renderItem.DrawColor = ColorSide;

                OpenGL.MapIcons.Add(renderItem);
            }
        }

        private void DrawFOVLines(float degressFOVcenter, float entityPosXmap, float entityPosZmap)
        {
            var lengthFOV = ModuleARMA.settingsForm.settingsJson.Entity.LineOfSight.Enemy * OpenGL.CanvasDiffCoeff;

            if (EntityType.Equals("You"))
            {
                lengthFOV = ModuleARMA.settingsForm.settingsJson.Entity.LineOfSight.You * OpenGL.CanvasDiffCoeff;
            }

            var fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOVcenter * Math.PI) / 180) + (Math.PI / 2)) * lengthFOV));
            var fov_line_Z = (float)(entityPosZmap + (Math.Sin(((degressFOVcenter * Math.PI) / 180) + (Math.PI / 2)) * lengthFOV));

            renderItem = new RenderItem();
            renderItem.Text = "linestripple";
            renderItem.MapPosX = entityPosXmap;
            renderItem.MapPosZ = entityPosZmap;
            renderItem.MapPosXend = fov_line_X;
            renderItem.MapPosZend = fov_line_Z;
            renderItem.DrawColor = Color;
            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
            renderItem.Size = 2;
            OpenGL.MapGeometry.Add(renderItem);

            renderItem = new RenderItem();
            renderItem.Text = "linestripple_invert";
            renderItem.MapPosX = entityPosXmap;
            renderItem.MapPosZ = entityPosZmap;
            renderItem.MapPosXend = fov_line_X;
            renderItem.MapPosZend = fov_line_Z;
            renderItem.DrawColor = System.Windows.Forms.ControlPaint.Dark(Color);
            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
            renderItem.Size = 2;
            OpenGL.MapGeometry.Add(renderItem);
        }
    }

    public class IEntityPlayer
    {
        public bool canDelete = false;
        public bool canReadData = true;

        public bool canRender = false;

        public string Category;
        public string CleanName;
        public string SimulationName;
        public System.Drawing.Color Color;
        public System.Drawing.Color ColorSide;
        public bool showSide;
        public IconPositionTexture Icon = IconPositionTexture.none;
        public string ConfigName;
        public Vector3 Direction;
        public bool ignoreDirection;
        public string EntityType;

        public string FriendlyName;

        public bool isDead;
        public bool? isImportant;
        public bool isLocalPlayer;
        public string ModelName;
        public uint? networkId;
        public string Nickname;

        public ulong? objectBase;

        public Vector3 Position;
        public int[] PositionUpdateRate = new int[] { 1, 500, 5000, 15000 };

        public DateTime PositionUpdateTime;

        public TableType tableType;
        public Side side;

        public string TypeName;

        public bool updateColors = true;
        public bool updateRenderStatus = true;
        public string Weapons = "";

        internal bool wtsRender;

        public RenderLayers renderLayer = RenderLayers.Default;

        internal int ExtraInfoUpdateMSec = 25000;
        internal DateTime ExtraInfoUpdateTimeLast;
        internal bool ExtraInfoUpdateAllowed;

        internal DateTime DelayedChecksTimeLast;
        internal int DelayedChecksUpdateMSeconds = 3000;
        internal bool DelayedChecksAllowed;

        internal RenderItem renderItem = new RenderItem();
        internal RenderItem renderItemOverlay = new RenderItem();

        internal bool passengerEntity = false;
        internal List<EntityArma> passengers = new List<EntityArma>();
        internal List<EntityArma> turrets = new List<EntityArma>();
        internal EntityArma driver;
        internal string PassengersNames;
        internal int PassengersCount;
    }
}