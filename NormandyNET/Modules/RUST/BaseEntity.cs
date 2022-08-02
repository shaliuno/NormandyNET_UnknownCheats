using NormandyNET.Core;
using NormandyNET.Helpers;
using NormandyNET.Modules.RUST.Entity;
using System;
using System.Numerics;
using static NormandyNET.Modules.RUST.Misc;

namespace NormandyNET.Modules.RUST
{
    public class IBaseEntity
    {
        public string ClassName;

        public int PrefabID;
        public ulong? GameObject;
        public string GameObjectName;

        public Vector3 Position;

        public Vector2 Direction;
        public Vector3 ViewAngle;
        public DateTime PositionUpdateTime;
        public DateTime DestroyedUpdateTime;
        public int[] PositionUpdateRate = new int[] { 1, 500, 5000, 15000 };

        public string DisplayName;
        public ulong? PlayerInput;
        public ulong? PlayerModel;
        public ulong? BaseMovement;
        public int? PlayerFlags;
        public ulong? SteamID;
        public ulong? TeamID;

        public string Weapons = "";
        public bool isLocalPlayer;
        public bool isConnected;
        public bool isDestroyed;
        public bool isDead;
        public bool isTeammate;

        public string FriendlyName;
        public string Category;
        public EntityTypeRUST EntityType;

        public bool canRender = false;
        public bool canDelete = false;
        public bool canReadData = true;
        public bool blacklist = false;
        public bool updateRenderStatus = true;

        public System.Drawing.Color ColorEntity;
        internal bool updateColors;

        internal int ExtraInfoUpdateMSec = 25000;

        internal DateTime ExtraInfoUpdateTimeLast;
        internal bool ExtraInfoUpdateAllowed;

        internal DateTime DelayedChecksTimeLast;
        internal int DelayedChecksUpdateMSeconds = 3000;
        internal bool DelayedChecksAllowed;

        internal DateTime WriteMemoryPeriodicChecks;
        internal bool WriteMemoryPeriodicChecksAllowed;

        internal bool wtsRender;

        internal AuthorizedPlayers authorizedPlayers;
    }

    public class BaseEntity : IBaseEntity, IEquatable<BaseEntity>
    {
        internal readonly ulong address;
        private float distanceToLocalPlayer;

        public BaseEntity(ulong addr)
        {
            address = addr;
        }

        public bool Equals(BaseEntity other)
        {
            if (address == other.address)
            {
                return true;
            }

            return false;
        }

        public void GetEntityValues(bool forceUpdate = false)
        {
            if (blacklist)
            {
                return;
            }

            if (CommonHelpers.dateTimeHolder > ExtraInfoUpdateTimeLast)
            {
                ExtraInfoUpdateAllowed = true;
                ExtraInfoUpdateTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(ExtraInfoUpdateMSec + ModuleRUST.radarForm.fastRandom.Next(3000, 6000));
            }
            else
            {
                ExtraInfoUpdateAllowed = false;
            }

            if (CommonHelpers.dateTimeHolder > DelayedChecksTimeLast)
            {
                DelayedChecksAllowed = true;
                DelayedChecksTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(DelayedChecksUpdateMSeconds + ModuleRUST.radarForm.fastRandom.Next(1000, 2000));
            }
            else
            {
                DelayedChecksAllowed = false;
            }

            if (isDestroyed)
            {
            }

            GetGameObject();

            GetGameObjectName();

            GetCSVData();

            if (EntityType == EntityTypeRUST.Blacklist)
            {
                canReadData = false;
                blacklist = true;
                return;
            }
            else
            {
                updateRenderStatus = true;
            }

            GetClassName();
            GetEntityPosition();

            if (EntityType == EntityTypeRUST.Player || EntityType == EntityTypeRUST.PlayerNPC || EntityType == EntityTypeRUST.LocalPlayer)
            {
                GetPlayerFlags();
                GetPlayerAI();
                GetPlayerConnected();
                GetPlayerSteamID();
                GetPlayerModel();
                GetPlayerDisplayName();
                GetPlayerInput();
                GetPlayerRotation();
                GetPlayerIsLocal();
                GetPlayerHeldItem();
                GetPlayerTeamID();
                GetIsDead();
                DoLocalPlayerStuff();
            }

            if (EntityType == EntityTypeRUST.House)
            {
                if (authorizedPlayers == null || authorizedPlayers.CanRefresh())
                {
                    authorizedPlayers = GetAuthorizedPlayers();
                    authorizedPlayers.protectedMinutes = authorizedPlayers.GetProtectedMinutes(address);
                }
            }

            if (EntityType == EntityTypeRUST.ESP)
            {
                canReadData = false;
                blacklist = false;
            }
            else
            {
                updateRenderStatus = true;
            }

            GetColor();
            GetCanRender();
        }

        private AuthorizedPlayers GetAuthorizedPlayers()
        {
            var authorizedPlayers = Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.BuildingPrivlidgeAuthorizedPlayers);

            if (Memory.IsValidPointer(authorizedPlayers))
            {
                return new AuthorizedPlayers(authorizedPlayers);
            }

            return null;
        }

        private void GetIsDead()
        {
            if (DelayedChecksAllowed == false)
            {
                                return;
            }

            var deathTime = Memory.Read<float>(address + ModuleRUST.offsetsRUST.BaseCombatDeathTime);

            if (deathTime == 0)
            {
                isDead = false;
            }
            else
            {
                isDead = true;
            }
        }

        private void GetIsDestroyed()
        {
            if (DelayedChecksAllowed == false)
            {
                                return;
            }

            var isDestroyedByte = Memory.Read<byte>(address + ModuleRUST.offsetsRUST.BaseEntityIsDestroyed);

            if (CommonHelpers.dateTimeHolder < DestroyedUpdateTime && wtsRender == false)
            {
                
                return;
            }
            DestroyedUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(3000);

            if (isDestroyedByte == (byte)1)
            {
                isDestroyed = true;
            }
            else
            {
                isDestroyed = false;
            }
        }

        private void GetGameObject()
        {
            if (canReadData == false)
            {
                return;
            }

            if (GameObject == null)
            {
                                GameObject = Memory.ReadChain<ulong>(address, ModuleRUST.offsetsRUST.GameObjectChain, false);
                                            }
        }

        private void GetClassName()
        {
            if (canReadData == false)
            {
                return;
            }

            if (ClassName == null)
            {
                                var classNamePtr = Memory.ReadChain<ulong>(address, new uint[] { 0x0, 0x10 }, false);

                if (!Memory.IsValidPointer(classNamePtr))
                {
                                        ClassName = "";
                    canReadData = false;
                    blacklist = true;
                }
                else
                {
                    ClassName = CommonHelpers.GetStringFromMemory(classNamePtr, false);
                                                                            }
            }
        }

        private void GetGameObjectName()
        {
            if (canReadData == false)
            {
                return;
            }

            if (GameObject != null && GameObject != 0)
            {
                if (GameObjectName == null)
                {
                                        var gameObjectNamePtr = Memory.Read<ulong>((ulong)GameObject + 0x60);
                    
                    if (!Memory.IsValidPointer(gameObjectNamePtr))
                    {
                                                GameObjectName = "";
                        canReadData = false;
                        blacklist = true;
                    }
                    else
                    {
                        GameObjectName = CommonHelpers.GetStringFromMemory(gameObjectNamePtr, false, 128);
                        if (GameObjectName.Length < 20)
                        {
                        }
                                                                    }
                }
            }
        }

        private void GetCSVData()
        {
            if (canReadData == false)
            {
                return;
            }

            if (FriendlyName == null)
            {
                                var lootCSV = LootItemHelper.GetLootFromCSV(GameObjectName);
                FriendlyName = lootCSV.FriendlyName;

                if (Enum.TryParse<EntityTypeRUST>(lootCSV.EntityType, true, out EntityTypeRUST result))
                {
                    EntityType = result;
                }
                else
                {
                    EntityType = EntityTypeRUST.Unknown;
                }

                Category = lootCSV.Category;

                                            }
        }

        private void GetPlayerTeamID()
        {
            if (canReadData == false)
            {
                return;
            }

            if (DelayedChecksAllowed == false)
            {
                                return;
            }

            {
                                TeamID = Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.BasePlayerTeamID, false, false);
                                
                var localPlayer = ModuleRUST.readerRUST.GetLocalPlayer();

                if (localPlayer != null)
                {
                    if (TeamID != 0 && localPlayer.TeamID != 0 && TeamID == localPlayer.TeamID)
                    {
                        isTeammate = true;
                        updateColors = true;
                    }
                    else
                    {
                        isTeammate = false;
                        updateColors = true;
                    }
                }
            }
        }

        private void GetPlayerSteamID()
        {
            if (canReadData == false)
            {
                return;
            }

            if (SteamID == null)
            {
                                SteamID = Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.BasePlayerSteamID, false, false);
                                            }
        }

        private void GetPlayerFlags(bool overrideChecks = false)
        {
            if (canReadData == false)
            {
                return;
            }

            if (DelayedChecksAllowed == false && overrideChecks == false)
            {
                                return;
            }

            {
                                PlayerFlags = Memory.Read<int>(address + ModuleRUST.offsetsRUST.BasePlayerFlags, false);

                            }
        }

        private void GetPlayerAI()
        {
        }

        private void GetPlayerConnected()
        {
            if (canReadData == false)
            {
                return;
            }

            if (DelayedChecksAllowed == false)
            {
                                return;
            }

            if (PlayerFlags != null)
            {
                
                var connected = ((int)Misc.PlayerFlags.Connected & PlayerFlags) != 0;
                isConnected = connected;

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

            
            var transformChain = Memory.ReadChain<ulong>((ulong)GameObject, ModuleRUST.offsetsRUST.TransformChain);

            
            if (transformChain != 0 && Memory.IsValidPointer(transformChain))
            {
                var databuffer = Memory.ReadBytes(transformChain + ModuleRUST.offsetsRUST.GameObjectTransFormPositionOffset, sizeof(float) * (3));

                Position = new Vector3(
                       BitConverter.ToSingle(databuffer, 0x0),
                       BitConverter.ToSingle(databuffer, 0x4),
                       BitConverter.ToSingle(databuffer, 0x8));
            }
            else
            {
                return;
            }

            PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0]);

            if (isLocalPlayer == false)
            {
                try
                {
                    var localPlayerPosition = Vector3.Zero;
                    var localPlayer = ModuleRUST.readerRUST.GetLocalPlayer();

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

                    if ((EntityType != EntityTypeRUST.Player && EntityType != EntityTypeRUST.PlayerNPC && EntityType != EntityTypeRUST.LocalPlayer))
                    {
                        PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(ModuleRUST.radarForm.fastRandom.Next(PositionUpdateRate[2], PositionUpdateRate[3]) + (int)Math.Round(Math.Ceiling(distanceToLocalPlayer / 100) / 2 * distanceToLocalPlayer, 0));
                    }

                    if (EntityType == EntityTypeRUST.Player || EntityType == EntityTypeRUST.PlayerNPC || EntityType == EntityTypeRUST.LocalPlayer)
                    {
                        PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[0] + (int)Math.Round(Math.Ceiling(distanceToLocalPlayer / 100) / 2 * distanceToLocalPlayer, 0));
                    }
                }
                catch (Exception ex)
                {
                                        PositionUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(PositionUpdateRate[2] + (int)Math.Round(Math.Ceiling(distanceToLocalPlayer / 100) / 2 * distanceToLocalPlayer, 0));
                }

                                            }
        }

        internal void GetCanRender()
        {
            if (updateRenderStatus)
            {
                canRender = false;

                if ((EntityType == EntityTypeRUST.Loot) == false)
                {
                    if (ModuleRUST.settingsForm.settingsJson.Entity.EntityTypesSuppressed.Contains(EntityType.ToString()))
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

                if ((EntityType == EntityTypeRUST.Loot) == true)
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
                        if (Category != null && !ModuleRUST.settingsForm.settingsJson.Loot.LootCategorySuppressed.Contains(Category))
                        {
                            canRender = true;
                        }
                    }
                }

                updateRenderStatus = false;
            }
        }

        private void GetPrefabID()
        {
            if (canReadData == false)
            {
                return;
            }

            if (PrefabID == 0)
            {
                PrefabID = Memory.Read<int>(address + ModuleRUST.offsetsRUST.BaseEntityPrefabID);
            }
        }

        private void GetPlayerDisplayName()
        {
            if (canReadData == false)
            {
                return;
            }

            if (DisplayName == null)
            {
                
                var displayNameClassPtr = Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.BasePlayerDisplayName, true);

                if (!Memory.IsValidPointer(displayNameClassPtr))
                {
                                        DisplayName = "n/a";
                }
                else
                {
                    var displayNameStrSize = Memory.Read<int>(displayNameClassPtr + 0x10, false);

                    if (displayNameStrSize > 0)
                    {
                        DisplayName = CommonHelpers.GetStringFromMemory(displayNameClassPtr + 0x14, true, displayNameStrSize).CleanInput();
                    }
                }

                                            }
        }

        private void GetPlayerInput()
        {
            if (canReadData == false)
            {
                return;
            }

            if (PlayerInput == null)
            {
                
                PlayerInput = Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.BasePlayerPlayerInput, false);
                                            }
        }

        private void GetPlayerModel()
        {
            if (canReadData == false)
            {
                return;
            }

            if (PlayerModel == null)
            {
                
                PlayerModel = Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.BasePlayerPlayerModel, false);

                                            }
        }

        private void GetPlayerIsLocal()
        {
            if (canReadData == false)
            {
                return;
            }

            if (ModuleRUST.readerRUST.localPlayerFound == false)
            {
                
                if (EntityType == EntityTypeRUST.LocalPlayer)
                {
                    ModuleRUST.readerRUST.localPlayerFound = true;
                    isLocalPlayer = true;
                    updateColors = true;

                                    }

                            }
        }

        private void GetPlayerRotation()
        {
            if (canReadData == false)
            {
                return;
            }

            if (PlayerModel != null && PlayerModel != 0)
            {
                
                var databuffer = Memory.ReadBytes((ulong)PlayerModel + ModuleRUST.offsetsRUST.BasePlayerPlayerModelRotation, sizeof(float) * 4);

                var directionQuaterion = new Quaternion(
                    BitConverter.ToSingle(databuffer, 0x0),
                    BitConverter.ToSingle(databuffer, 0x4),
                    BitConverter.ToSingle(databuffer, 0x8),
                    BitConverter.ToSingle(databuffer, 0xC)
                    );

                var sin = directionQuaterion.Y;
                var cos = directionQuaterion.W;
                Direction = new Vector2(Geometry.CosSinToDegree(sin, cos), 0) * -1;

                                            }
        }

        private void GetPlayerHeldItem()
        {

            if (canReadData == false)
            {
                return;
            }

            if (ExtraInfoUpdateAllowed == false)
            {
                                return;
            }

            
            var active_weapon_id = Memory.Read<int>(address + ModuleRUST.offsetsRUST.BasePlayerActiveItem);
            

            var playerInventory = Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.BasePlayerPlayerInventory);
            
            if (playerInventory == 0)
            {
                return;
            }

            var containerBelt = Memory.Read<ulong>(playerInventory + ModuleRUST.offsetsRUST.ItemContainerContainerBelt);
            
            if (containerBelt == 0)
            {
                return;
            }

            var beltContentsList = Memory.Read<ulong>(containerBelt + ModuleRUST.offsetsRUST.ItemContainerListContents);
            
            if (beltContentsList == 0)
            {
                return;
            }

            var beltItemsArray = Memory.Read<ulong>(beltContentsList + 0x10);
            
            if (beltItemsArray == 0)
            {
                return;
            }

            for (uint items_on_belt = 0; items_on_belt <= 6; items_on_belt++)
            {
                var item = Memory.Read<ulong>(beltItemsArray + 0x20 + (items_on_belt * 0x8));
                
                if (item == 0)
                {
                    continue;
                }

                var active_weapon = Memory.Read<uint>(item + 0x28);
                
                if (active_weapon_id == active_weapon && active_weapon_id != 0)
                {
                    
                    HeldItem item_obj = new HeldItem(item);


                    Weapons = item_obj.name;
                }
            }
        }

        internal void GetColor()
        {
            if (ColorEntity.IsEmpty || updateColors)
            {
                                
                updateColors = false;
                ColorEntity = System.Drawing.Color.White;

                if (EntityType == EntityTypeRUST.Animal && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.Animal.ToString()))
                {
                    ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.Animal.ToString()];
                    return;
                }

                if (EntityType == EntityTypeRUST.LocalPlayer && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.LocalPlayer.ToString()))
                {
                    ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.LocalPlayer.ToString()];
                    return;
                }

                if (EntityType == EntityTypeRUST.Player && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.Player.ToString()))
                {
                    ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.Player.ToString()];

                    if (isTeammate && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.Teammate.ToString()))
                    {
                        ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.Teammate.ToString()];
                    }

                    return;
                }

                if (EntityType == EntityTypeRUST.PlayerNPC && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.PlayerNPC.ToString()))
                {
                    ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.PlayerNPC.ToString()];
                    return;
                }

                if (EntityType == EntityTypeRUST.Unknown && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.Unknown.ToString()))
                {
                    ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.Unknown.ToString()];
                    return;
                }

                if (EntityType == EntityTypeRUST.House && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.House.ToString()))
                {
                    ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.House.ToString()];
                    return;
                }

                if (EntityType == EntityTypeRUST.Vehicle && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.Vehicle.ToString()))
                {
                    ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.Vehicle.ToString()];
                    return;
                }

                if (EntityType == EntityTypeRUST.VehicleNPC && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.VehicleNPC.ToString()))
                {
                    ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.VehicleNPC.ToString()];
                    return;
                }

                if (EntityType == EntityTypeRUST.Turret && ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.ContainsKey(EntityTypeRUST.Turret.ToString()))
                {
                    ColorEntity = ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[EntityTypeRUST.Turret.ToString()];
                    return;
                }

                if (EntityType == EntityTypeRUST.Loot)
                {
                    if (ModuleRUST.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(Category, out System.Drawing.Color entityColor))
                    {
                                                ColorEntity = entityColor;
                    }
                    else
                    {
                                            }
                }

                            }
        }

        private void DoLocalPlayerStuff()
        {
            if (EntityType == EntityTypeRUST.LocalPlayer)
            {
                if (CommonHelpers.dateTimeHolder > WriteMemoryPeriodicChecks)
                {
                    WriteMemoryPeriodicChecksAllowed = true;
                    WriteMemoryPeriodicChecks = CommonHelpers.dateTimeHolder.AddMilliseconds(1000);
                }
                else
                {
                    WriteMemoryPeriodicChecksAllowed = false;
                }

                if (ModuleRUST.settingsForm.settingsJson.MemoryWriting.FakeAdminDo)
                {
                                        
                    SetPlayerFlag(Misc.PlayerFlags.IsAdmin, ModuleRUST.settingsForm.settingsJson.MemoryWriting.FakeAdmin);
                    ModuleRUST.settingsForm.settingsJson.MemoryWriting.FakeAdminDo = false;
                }
                else
                {
                    if (WriteMemoryPeriodicChecksAllowed && ModuleRUST.settingsForm.settingsJson.MemoryWriting.FakeAdmin)
                    {
                        SetPlayerFlag(Misc.PlayerFlags.IsAdmin, ModuleRUST.settingsForm.settingsJson.MemoryWriting.FakeAdmin);
                    }
                }

                if (ModuleRUST.settingsForm.settingsJson.MemoryWriting.SpiderManDo)
                {
                    SpiderMan(ModuleRUST.settingsForm.settingsJson.MemoryWriting.SpiderMan);
                    ModuleRUST.settingsForm.settingsJson.MemoryWriting.SpiderManDo = false;
                }

                if (ModuleRUST.settingsForm.settingsJson.MemoryWriting.BrightDayDo)
                {
                    ModuleRUST.settingsForm.settingsJson.MemoryWriting.BrightDayDo = false;

                    var TOD_CycleParameters = Memory.Read<ulong>(Pointers.SkyDome + ModuleRUST.offsetsRUST.TodCycle);
                    var TOD_NightParameters = Memory.Read<ulong>(Pointers.SkyDome + ModuleRUST.offsetsRUST.TodNight);
                    var TOD_AmbientParameters = Memory.Read<ulong>(Pointers.SkyDome + ModuleRUST.offsetsRUST.TodAmbient);

                    if (ModuleRUST.settingsForm.settingsJson.MemoryWriting.BrightDay)
                    {
                        Memory.Write<float>(TOD_CycleParameters + 0x10, 12f);

                        Memory.Write<float>(TOD_AmbientParameters + 0x18, 999999f);
                        Memory.Write<float>(TOD_CycleParameters + 0x10, 12f);
                    }
                    else
                    {
                        Memory.Write<float>(TOD_AmbientParameters + 0x18, 0f);
                    }
                }
            }
        }

        private void SpiderMan(bool enable)
        {
            if (canReadData == false)
            {
                return;
            }

            if (BaseMovement == null)
            {
                                BaseMovement = Memory.Read<ulong>(address + ModuleRUST.offsetsRUST.BasePlayerBaseMovement, false);
                                            }

            if (BaseMovement != null)
            {
                if (enable)
                {
                    
                    Memory.Write<float>((ulong)BaseMovement + 0x7C, float.MaxValue);
                    Memory.Write<float>((ulong)BaseMovement + 0x80, float.MaxValue);
                    Memory.Write<float>((ulong)BaseMovement + 0x84, float.MaxValue);
                                    }
                else
                {
                    
                    Memory.Write<float>((ulong)BaseMovement + 0x7C, 50f);
                    Memory.Write<float>((ulong)BaseMovement + 0x80, 60f);
                    Memory.Write<float>((ulong)BaseMovement + 0x84, 90f);
                                    }
            }
        }

        private void SetPlayerFlag(PlayerFlags flag, bool setFlag)
        {
            if (PlayerFlags == null)
            {
                GetPlayerFlags(true);
            }

            if (PlayerFlags != null)
            {
                int flagToWrite;

                if (setFlag)
                {
                    flagToWrite = (int)PlayerFlags | (int)flag;
                }
                else
                {
                    flagToWrite = (int)PlayerFlags & ~(int)flag;
                }

                Memory.Write(address + ModuleRUST.offsetsRUST.BasePlayerFlags, flagToWrite);
            }
        }
    }
}