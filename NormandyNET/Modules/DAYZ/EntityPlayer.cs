using NormandyNET.Core;
using NormandyNET.Helpers;
using System;
using System.Numerics;

namespace NormandyNET.Modules.DAYZ
{
    public struct Pointers
    {
    }

    public class EntityPlayer : IEntityPlayer, IEquatable<EntityPlayer>
    {
        public Pointers pointers;

        internal readonly ulong baseAddrPointer;
        internal readonly ulong playerAddress;
        private float distanceToLocalPlayer;

        public EntityPlayer(ulong addrPointer, ulong addr)
        {
            baseAddrPointer = addrPointer;
            playerAddress = addr;

            pointers = new Pointers
            {
            };
        }

        public bool Equals(EntityPlayer other)
        {
            if (playerAddress == other.playerAddress && tableType == other.tableType)
            {
                return true;
            }

            return false;
        }

        internal void GetCanRender()
        {
            if (updateRenderStatus)
            {
                string entityTypeShowStatus;

                if (EntityType != null)
                {
                    canRender = false;

                    if (EntityType.Equals("Loot") == false)
                    {
                        if (ModuleDAYZ.settingsForm.settingsJson.Entity.EntityTypesSuppressed.Contains(EntityType))
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
                            if (Category != null && !ModuleDAYZ.settingsForm.settingsJson.Loot.LootCategorySuppressed.Contains(Category))
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
            if (Color.IsEmpty || updateColors)
            {
                                
                updateColors = false;
                Color = System.Drawing.Color.White;
                if (EntityType == null)
                {
                    return;
                }

                if (EntityType.Equals("Loot") == false)
                {
                    if (ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.TryGetValue(EntityType, out System.Drawing.Color entityColor))
                    {
                                                Color = entityColor;
                    }
                    else
                    {
                                            }
                }

                if (EntityType.Equals("Loot") == true)
                {
                    if (ModuleDAYZ.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(Category, out System.Drawing.Color entityColor))
                    {
                        
                        Color = entityColor;
                    }
                    else
                    {
                                            }
                }

                            }
        }

        internal void GetPlayerValues()
        {
            if (blacklist)
            {
                return;
            }

            if (canReadData == false)
            {
                return;
            }

            if (CommonHelpers.dateTimeHolder > DelayedChecksTimeLast)
            {
                DelayedChecksAllowed = true;
                DelayedChecksTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(DelayedChecksUpdateMSeconds + ModuleDAYZ.radarForm.fastRandom.Next(3000, 5000));
            }
            else
            {
                DelayedChecksAllowed = false;
            }

            GetBase();
            GetTypeName();

            GetCSVData();

            if (EntityType != null && EntityType.Equals("Blacklist"))
            {
                canReadData = false;
                blacklist = true;
                return;
            }
            else
            {
                updateRenderStatus = true;
            }

            GetEntityPosition();
            GetModelName();
            GetPlayerRotationLocalPlayer();

            GetConfigName();
            GetCleanName();
            GetItemInHands();
            GetNetworkId();

            GetPlayerNickName();
            GetColor();
            GetPlayerIsLocal();
            GetCanRender();
            GetIsImportant();
            GetIsDead();

            DoLocalPlayerStuff();

            if (EntityType != null && EntityType.Equals("Unknown"))
            {
                canReadData = false;
            }
        }

        private void GetCleanName()
        {
            CleanName = "PLACEHOLDER";

            return;
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

        private void GetIsDead()
        {
            if (EntityType == null)
            {
                return;
            }

            if (EntityType.Equals("Player") || EntityType.Equals("Infected"))
            {
                
                if (tableType == TableType.Slow)
                {
                    isDead = true;
                }
                else
                {
                    isDead = false;
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
                
                if (EntityType.Equals("Player") || EntityType.Equals("You"))
                {
                    isImportant = true;
                }
                else
                {
                    isImportant = false;
                }
            }
        }

        private void GetItemInHands()
        {
            if (canReadData == false)
            {
                return;
            }

            
            if (EntityType.Equals("Player") && !isLocalPlayer)
            {
                
                                var inventory = Memory.Read<ulong>(playerAddress + ModuleDAYZ.offsetsDAYZ.Entity_Inventory, false, true);
                
                                var itemInHands = Memory.Read<ulong>(inventory + ModuleDAYZ.offsetsDAYZ.Inventory_ItemInHands, false, true);
                
                                var itemData = Memory.Read<ulong>(itemInHands + 0x8, false, true);
                
                                var cleanName = Memory.Read<ulong>(itemData + 0x10, false, true);
                
                if (Memory.IsValidPointer(itemInHands))
                {
                                        Weapons = CommonHelpers.GetStringFromMemory(cleanName, false, 64);
                                    }
            }
            else
            {
                Weapons = "";
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

            if (networkId == null && EntityType.Equals("Player"))
            {
                                networkId = Memory.Read<uint>(playerAddress + ModuleDAYZ.offsetsDAYZ.Entity_Networkid);
                                            }
        }

        private void GetPlayerIsLocal()
        {
            if (canReadData == false)
            {
                return;
            }

            
            if (ModuleDAYZ.readerDAYZ.localPlayerFound == false)
            {
                                
                World.GetLocalEntity();

                var distance = Vector3.Distance(World.GetLocalEntityPosition(), Position);

                if (distance < 2)
                {
                    ModuleDAYZ.readerDAYZ.localPlayerFound = true;
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

            if (Nickname == null || DelayedChecksAllowed == true)
            {
                if (EntityType.Equals("Player"))
                {
                    if (NetworkClient.networkClient != 0 && networkId != null)
                    {
                        if (NetworkClient.playersList.TryGetValue((uint)networkId, out ulong playerIdentityAddress))
                        {
                            SteamGUID = Memory.Read<ulong>(playerIdentityAddress + ModuleDAYZ.offsetsDAYZ.PlayerIdentity_SteamId, false, false);

                            var player_name_ptr = Memory.Read<ulong>(playerIdentityAddress + ModuleDAYZ.offsetsDAYZ.PlayerIdentity_PlayerName);
                            
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
                            Nickname = $"{networkId}";
                        }
                    }
                }

                if (EntityType.Equals("Unknown"))
                {
                    Nickname = TypeName;
                    return;
                }

                Nickname = FriendlyName;
                return;
            }
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

            ulong renderVisualState = Memory.Read<ulong>(playerAddress + ModuleDAYZ.offsetsDAYZ.RendererVisualState);
            
            if (renderVisualState != 0)
            {
                if (isLocalPlayer == false)
                {
                    var databuffer = Memory.ReadBytes((ulong)renderVisualState + ModuleDAYZ.offsetsDAYZ.VisualState_Direction, sizeof(float) * 6);

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

            PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0]);

            if (isLocalPlayer == false)
            {
                try
                {
                    var localPlayerPosition = Vector3.Zero;
                    var localPlayer = ReaderDAYZ.GetLocalPlayer();

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

                    if (EntityType.Equals("Player") || EntityType.Equals("Infected"))
                    {
                        PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0] + (int)Math.Round(Math.Ceiling(distanceToLocalPlayer / 100) / 2 * distanceToLocalPlayer, 0));
                    }
                    else
                    {
                        PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(ModuleDAYZ.radarForm.fastRandom.Next(PositionUpdateRate[2], PositionUpdateRate[3]) + (int)Math.Round(Math.Ceiling(distanceToLocalPlayer / 100) / 2 * distanceToLocalPlayer, 0));
                    }
                }
                catch
                {
                    
                    PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[2] + (int)Math.Round(Math.Ceiling(distanceToLocalPlayer / 100) / 2 * distanceToLocalPlayer, 0));
                }
            }

                                }

        private void GetPlayerRotationLocalPlayer()
        {
            if (isLocalPlayer == false || !canReadData)
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

        private void GetBase()
        {
            
            if (objectBase == null)
            {
                
                var tempAddr = Memory.Read<ulong>(baseAddrPointer);

                if (tempAddr != playerAddress)
                {
                                                                                canReadData = false;
                    canDelete = true;
                    return;
                }

                objectBase = Memory.Read<ulong>(playerAddress + ModuleDAYZ.offsetsDAYZ.RenderEntityType);
                
                if (objectBase == 0)
                {
                    canReadData = false;
                                    }
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
                                var address = Memory.Read<ulong>((ulong)objectBase + ModuleDAYZ.offsetsDAYZ.EntityType_TypeName);
                                if (Memory.IsValidPointer(address))
                {
                    TypeName = ArmaString.GetString(address);
                }
                else
                {
                    TypeName = "NULL";
                }

                                            }
        }

        private void DoLocalPlayerStuff()
        {
            if (isLocalPlayer)
            {
                if (ModuleDAYZ.settingsForm.settingsJson.MemoryWriting.NoGrass)
                {
                                        
                    Memory.Write<float>(World.world + ModuleDAYZ.offsetsDAYZ.World_NoGrass, 0f);
                    Memory.Write<float>(World.world + ModuleDAYZ.offsetsDAYZ.World_NoGrass_Online, 0f);
                    ModuleDAYZ.settingsForm.settingsJson.MemoryWriting.NoGrass = false;
                                    }
            }
        }

        private void DrawFOVLines(float degressFOVcenter, float entityPosXmap, float entityPosZmap)
        {
            var lengthFOV = ModuleDAYZ.settingsForm.settingsJson.Entity.LineOfSight.Enemy * OpenGL.CanvasDiffCoeff;

            if (EntityType.Equals("You"))
            {
                lengthFOV = ModuleDAYZ.settingsForm.settingsJson.Entity.LineOfSight.You * OpenGL.CanvasDiffCoeff;
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

        internal void GenerateRenderItem()
        {
            if (ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged)
            {
                updateColors = true;
                GetColor();
            }

            if (ModuleDAYZ.settingsForm.settingsJson.Entity.ShowStatusChanged)
            {
                updateRenderStatus = true;
                GetCanRender();
            }

            if (ModuleDAYZ.settingsForm.settingsJson.Loot.ShowStatusChanged)
            {
                updateRenderStatus = true;
                GetCanRender();
            }

            if (EntityType == null || EntityType.Equals("NULL") || !canRender)
            {
                return;
            }

            if (!ModuleDAYZ.settingsForm.settingsJson.Entity.Bodies && isDead)
            {
                return;
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

            if (isLocalPlayer == true)
            {
                CommonHelpers.myIngamePositionX = entityPosXingame;
                CommonHelpers.myIngamePositionY = entityPosYingame;
                CommonHelpers.myIngamePositionZ = entityPosZingame;
            }

            Vector3 vectorTarget = new Vector3(entityPosXingame, 0, entityPosZingame);
            Vector3 vectorMe = new Vector3(CommonHelpers.myIngamePositionX, 0, CommonHelpers.myIngamePositionZ);
            var distance = (int)Math.Round(Vector3.Distance(vectorTarget, vectorMe));

            if ((EntityType.Equals("Animal") == true) ||
                EntityType.Equals("Car_Wreck") == true ||
                EntityType.Equals("Helicrash") == true ||
                EntityType.Equals("Infected") == true ||
                EntityType.Equals("Vehicle") == true ||
                EntityType.Equals("Blacklist") == true ||
                EntityType.Equals("You") == true ||
                EntityType.Equals("Unknown") == true ||
                EntityType.Equals("Player") == true)
            {
                if (!ModuleDAYZ.radarForm.IsVisibleOnControl(entityPosXmap, entityPosZmap, 0))
                {
                    if (ModuleDAYZ.settingsForm.settingsJson.Map.ProximityAlert && EntityType.Equals("Player") && distance < 200)
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

                        var pnt = ModuleDAYZ.radarForm.GetControlEdgeIntersectionPoint(entityPosXmap, entityPosZmap);

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
                        renderItem.IconSize = ModuleDAYZ.settingsForm.settingsJson.Map.IconSizePlayers;
                        OpenGL.MapIcons.Add(renderItem);

                        renderItem = new RenderItem();
                        renderItem.MapPosX = pnt.X + OpenGL.CanvasDiffCoeff;
                        renderItem.MapPosZ = pnt.Y;
                        renderItem.DrawColor = ModuleDAYZ.settingsForm.settingsJson.Colors.ColorText;
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


                var viewAngle = Geometry.CalculateDirectionToDegress(Direction);

                if (ModuleDAYZ.settingsForm.settingsJson.Entity.LOS && isImportant == true && !isDead)
                {
                    DrawFOVLines(viewAngle, entityPosXmap, entityPosZmap);
                }

                renderItem = new RenderItem();

                RenderLayers renderLayer;
                renderLayer = RenderLayers.PlayersPriorityLow;

                renderItem.IconSize = ModuleDAYZ.settingsForm.settingsJson.Map.IconSizePlayers;

                switch (EntityType)
                {
                    case "Animal":
                        renderItem.IconPositionTexture = IconPositionTexture.animal;
                        renderItem.Rotation = 0;
                        break;

                    case "Loot":
                        renderItem.IconPositionTexture = IconPositionTexture.loot;
                        renderItem.renderLayer = RenderLayers.LootPriority1;
                        renderItem.Rotation = 0;
                        break;

                    case "Infected":
                        if (isDead)
                        {
                            renderItem.IconPositionTexture = IconPositionTexture.statictech;
                        }
                        else
                        {
                            renderItem.IconPositionTexture = IconPositionTexture.player;
                        }
                        renderItem.Rotation = (float)viewAngle;
                        renderItem.IconSize = ModuleDAYZ.settingsForm.settingsJson.Map.IconSizeInfected;

                        break;

                    case "Player":
                        if (isDead)
                        {
                            renderItem.IconPositionTexture = IconPositionTexture.statictech;
                        }
                        else
                        {
                            renderItem.IconPositionTexture = IconPositionTexture.player;
                        }
                        renderItem.Rotation = (float)viewAngle;
                        renderItem.renderLayer = RenderLayers.PlayersPriorityMedium;
                        break;

                    case "Vehicle":
                        renderItem.IconPositionTexture = IconPositionTexture.vehicle;
                        renderItem.Rotation = 0;
                        break;

                    case "House":
                        renderItem.IconPositionTexture = IconPositionTexture.house;
                        renderItem.Rotation = 0;
                        break;

                    case "Helicrash":
                        renderItem.IconPositionTexture = IconPositionTexture.helicrash;
                        renderItem.Rotation = 0;
                        break;

                    case "You":
                        renderItem.IconPositionTexture = IconPositionTexture.player;
                        renderItem.Rotation = (float)viewAngle;
                        renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                        break;

                    default:
                        renderItem.IconPositionTexture = IconPositionTexture.unknown;
                        renderItem.Rotation = 0;
                        break;
                }

                renderItem.MapPosX = entityPosXmap;
                renderItem.MapPosZ = entityPosZmap;
                renderItem.DrawColor = Color;

                OpenGL.MapIcons.Add(renderItem);

                renderItem = new RenderItem();
                renderItem.MapPosX = entityPosXmap + 20 + OpenGL.CanvasDiffCoeff;
                renderItem.MapPosZ = entityPosZmap + 15;
                renderItem.DrawColor = ModuleDAYZ.settingsForm.settingsJson.Colors.ColorText;
                renderItem.renderLayer = renderLayer;
                renderItem.TextOverlayOutline = true;
                renderItem.Size = (int)FontSizes.misc;

                renderItem.Text = $"©{CommonHelpers.ColorHexConverter(ModuleDAYZ.settingsForm.settingsJson.Colors.ColorText)}";

                var newLine = false;

                if (ModuleDAYZ.settingsForm.settingsJson.Entity.Distance && !isLocalPlayer && (EntityType.Equals("Player") || EntityType.Equals("Vehicle")))
                {
                    renderItem.Text += CommonHelpers.GetDistanceUnit(distance);
                    newLine = true;
                }

                var elevation = Math.Round((Position.Y - CommonHelpers.myIngamePositionY), 0);

                var offsetElevationType = 0;

                if (ModuleDAYZ.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Absolute))
                {
                    renderItem.Text += $"({(int)Position.Y}) ";
                    newLine = true;
                }

                if (ModuleDAYZ.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Relative))
                {
                    renderItem.Text += $"({elevation}) ";
                    newLine = true;
                }

                if (ModuleDAYZ.settingsForm.settingsJson.Entity.Elevation.Arrows)
                {
                    renderItem.Text += CommonHelpers.GetElevationData(elevation);
                }

                if (newLine)
                {
                    renderItem.Text += $"\n";
                }

                if (ModuleDAYZ.settingsForm.settingsJson.Entity.Name && !isLocalPlayer)
                {
                    if (!isDead)
                    {
                        renderItem.Text += $"©{CommonHelpers.ColorHexConverter(Color)}";
                        renderItem.Text += $"{Nickname}\n";
                    }
                    else
                    {
                        renderItem.Text += $"©{CommonHelpers.ColorHexConverter(Color)}";
                        renderItem.Text += $"Dead Body\n";
                    }
                }

                if (ModuleDAYZ.settingsForm.settingsJson.Entity.Weapon && Weapons.Length > 0 && !isLocalPlayer)
                {
                    renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleDAYZ.settingsForm.settingsJson.Colors.ColorText)}{Weapons.Truncate(22)}\n";
                }

                OpenGL.MapText.Add(renderItem);
            }

            if (EntityType.Equals("Loot") == true)
            {
                renderItem = new RenderItem();
                renderItem.IconPositionTexture = IconPositionTexture.loot;
                renderItem.renderLayer = RenderLayers.LootPriority1;
                renderItem.Rotation = 0;
                renderItem.IconSize = ModuleDAYZ.settingsForm.settingsJson.Map.IconSizeLoot;

                renderItem.MapPosX = entityPosXmap;
                renderItem.MapPosZ = entityPosZmap;
                renderItem.DrawColor = Color;
                OpenGL.MapIcons.Add(renderItem);

                renderItem = new RenderItem();
                renderItem.MapPosX = entityPosXmap + 20 + OpenGL.CanvasDiffCoeff;
                renderItem.MapPosZ = entityPosZmap + 15;
                renderItem.DrawColor = Color;
                renderItem.renderLayer = RenderLayers.LootPriority1;
                renderItem.TextOverlayOutline = true;
                renderItem.Size = (int)FontSizes.misc;

                renderItem.Text = $"©{CommonHelpers.ColorHexConverter(ModuleDAYZ.settingsForm.settingsJson.Colors.ColorText)}";

                var newLine = false;
                var elevation = Math.Round((Position.Y - CommonHelpers.myIngamePositionY), 0);

                if (ModuleDAYZ.settingsForm.settingsJson.Entity.Elevation.Arrows)
                {
                    renderItem.Text += CommonHelpers.GetElevationData(elevation);
                    newLine = true;
                }

                if (newLine)
                {
                    renderItem.Text += $"\n";
                }

                if (ModuleDAYZ.settingsForm.settingsJson.Entity.Name)
                {
                    renderItem.Text += $"©{CommonHelpers.ColorHexConverter(Color)}";
                    renderItem.Text += $"{Nickname}\n";
                }
                OpenGL.MapText.Add(renderItem);
            }

            if ((EntityType.Equals("AreaEffect") == true))
            {
                renderItem = new RenderItem();

                RenderLayers renderLayer;
                renderLayer = RenderLayers.PlayersPriorityLow;

                renderItem.MapPosX = entityPosXmap + 20 + OpenGL.CanvasDiffCoeff;
                renderItem.MapPosZ = entityPosZmap + 15;
                renderItem.DrawColor = ModuleDAYZ.settingsForm.settingsJson.Colors.ColorText;
                renderItem.renderLayer = renderLayer;
                renderItem.TextOverlayOutline = true;
                renderItem.Size = (int)FontSizes.misc;

                renderItem.Text = $"©{CommonHelpers.ColorHexConverter(ModuleDAYZ.settingsForm.settingsJson.Colors.ColorText)}";
                renderItem.Text += $"©{CommonHelpers.ColorHexConverter(Color)}";
                renderItem.Text += $"{FriendlyName}\n";
                OpenGL.MapText.Add(renderItem);

                renderItem = new RenderItem();
                renderItem.Text = "circlefill";
                renderItem.MapPosX = entityPosXmap;
                renderItem.MapPosZ = entityPosZmap;
                renderItem.DrawColor = System.Drawing.Color.FromArgb(64, Color);
                renderItem.Size = (int)Math.Round(100 * OpenGL.CanvasDiffCoeff, 0);
                OpenGL.MapGeometry.Add(renderItem);
            }
        }
    }

    public class IEntityPlayer
    {
        public bool canDelete = false;
        public bool canReadData = true;
        public bool blacklist = false;

        public bool canRender = false;

        public string Category;
        public string CleanName;
        public System.Drawing.Color Color;
        public string ConfigName;
        public Vector3 Direction;
        public string EntityType;

        public string FriendlyName;

        public bool isDead;
        public bool? isImportant;
        public bool isLocalPlayer;
        public string ModelName;
        public uint? networkId;
        public string Nickname;
        public ulong SteamGUID;
        public string SteamNickname;

        public ulong? objectBase;

        public Vector3 Position;
        public int[] PositionUpdateRate = new int[] { 1, 500, 5000, 15000 };

        public DateTime PositionUpdateTime;

        internal int ExtraInfoUpdateMSec = 25000;
        internal DateTime ExtraInfoUpdateTimeLast;
        internal bool ExtraInfoUpdateAllowed;

        internal DateTime DelayedChecksTimeLast;
        internal int DelayedChecksUpdateMSeconds = 3000;
        internal bool DelayedChecksAllowed;

        public TableType tableType;

        public string TypeName;

        public bool updateColors = true;
        public bool updateRenderStatus = true;
        public string Weapons = "";

        internal bool wtsRender;

        private RenderLayers renderLayer = RenderLayers.Default;

        internal RenderItem renderItem = new RenderItem();
        internal RenderItem renderItemOverlay = new RenderItem();
    }
}