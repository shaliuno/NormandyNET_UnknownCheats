using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NormandyNET.Core;
using System.IO;

namespace NormandyNET.Modules.RUST
{
    internal class OffsetsRUST : Offsets
    {
        public override string offsetsJsonFile
        {
            get { return "offsetsRUST.json"; }
        }

        #region OffsetsFields

        public uint GameOjectManager;
        public uint BaseNetworkable;
        public uint BasePlayer;

        public uint[] ClientEntitiesAll;
        public uint[] ClientEntitiesPlayersOnlyDict;

        public uint GameObjectTransFormPositionOffset;
        public uint[] GameObjectChain;
        public uint[] TransformChain;

        public uint BaseEntityPrefabID;
        public uint BaseEntityIsDestroyed;

        public uint BasePlayerFlags;
        public uint BasePlayerSteamID;
        public uint BasePlayerTeamID;

        public uint BasePlayerActiveItem;
        public uint BasePlayerDisplayName;
        public uint BasePlayerPlayerInput;
        public uint BasePlayerBaseMovement;

        public uint BasePlayerPlayerInputBodyAngles;

        public uint BasePlayerPlayerModel;
        public uint BasePlayerPlayerModelIsLocalPlayer;
        public uint BasePlayerPlayerModelPosition;
        public uint BasePlayerPlayerModelRotation;

        public uint BaseCombatEntityHealth;
        public uint BaseCombatDeathTime;

        public uint BuildingPrivlidgeAuthorizedPlayers;
        public uint BuildingPrivlidgeCachedProtectedMinutes;
        public uint AuthorizedPlayersPlayerNameID;
        public uint PlayerNameIDUsername;
        public uint PlayerNameIDSteamID;

        public uint BasePlayerPlayerInventory;

        public uint ItemContainerContainerBelt;

        public uint ItemContainerListContents;

        public uint TodCycle;
        public uint TodNight;
        public uint TodAmbient;

        #endregion OffsetsFields

        public OffsetsRUST(bool cleanOffsets = false)
        {
            if (cleanOffsets == false)
            {
                FillDefaultData();
            }
        }

        public void FillDefaultData()
        {
            GameOjectManager = 0x17C1F18;

            BaseNetworkable = 0x334A268;
            BasePlayer = 0x334ADA8;

            BaseEntityPrefabID = 0x48;
            BaseEntityIsDestroyed = 0x58;
            BasePlayerPlayerModel = 0x4C0;
            BasePlayerPlayerInput = 0x4E0;
            BasePlayerBaseMovement = 0x4E8;
            BasePlayerTeamID = 0x598;
            BasePlayerActiveItem = 0x5D0;
            BasePlayerFlags = 0x680;
            BasePlayerPlayerInventory = 0x690;
            BasePlayerSteamID = 0x6C8;
            BasePlayerDisplayName = 0x6E0;
            BasePlayerPlayerInputBodyAngles = 0x3C;
            BasePlayerPlayerModelIsLocalPlayer = 0x289;
            BasePlayerPlayerModelPosition = 0x208;
            BasePlayerPlayerModelRotation = 0x238;
            ItemContainerContainerBelt = 0x28;
            ItemContainerListContents = 0x38;
            BaseCombatEntityHealth = 0x224;
            BaseCombatDeathTime = 0x230;
            BuildingPrivlidgeAuthorizedPlayers = 0x588;
            BuildingPrivlidgeCachedProtectedMinutes = 0x578;
            PlayerNameIDUsername = 0x18;
            PlayerNameIDSteamID = 0x20;
            TodCycle = 0x38;
            TodNight = 0x58;
            TodAmbient = 0x90;

            ClientEntitiesAll = new uint[] { 0xB8, 0x0, 0x10, 0x28 };

            ClientEntitiesPlayersOnlyDict = new uint[] { 0xB8, 0x10, 0x28 };

            GameObjectTransFormPositionOffset = 0x90;
            GameObjectChain = new uint[] { 0x10, 0x30 };
            TransformChain = new uint[] { 0x30, 0x8, 0x38 };
        }

        internal override void LoadThis()
        {
            if (File.Exists(offsetsJsonFile))
            {
                try
                {
                    string s = File.ReadAllText(offsetsJsonFile);
                    var settingsJsonTmp = JsonConvert.DeserializeObject<OffsetsRUST>(s, new HexToJsonAndBack
                    {
                    });
                    CopyThis(settingsJsonTmp);
                    return;
                }
                catch (JsonSerializationException ex)
                {
                }
            }
        }

        internal override void LoadThis(JObject offsetsPart)
        {
        }

        public void CopyThis(OffsetsRUST tmp)
        {
            this.GameOjectManager = tmp.GameOjectManager;
            this.BaseNetworkable = tmp.BaseNetworkable;
            this.BasePlayer = tmp.BasePlayer;

            this.BaseEntityPrefabID = tmp.BaseEntityPrefabID;
            this.BaseEntityIsDestroyed = tmp.BaseEntityIsDestroyed;
            this.BasePlayerPlayerModel = tmp.BasePlayerPlayerModel;
            this.BasePlayerPlayerInput = tmp.BasePlayerPlayerInput;
            this.BasePlayerBaseMovement = tmp.BasePlayerBaseMovement;
            this.BasePlayerTeamID = tmp.BasePlayerTeamID;
            this.BasePlayerActiveItem = tmp.BasePlayerActiveItem;
            this.BasePlayerFlags = tmp.BasePlayerFlags;
            this.BasePlayerPlayerInventory = tmp.BasePlayerPlayerInventory;
            this.BasePlayerSteamID = tmp.BasePlayerSteamID;
            this.BasePlayerDisplayName = tmp.BasePlayerDisplayName;
            this.BasePlayerPlayerInputBodyAngles = tmp.BasePlayerPlayerInputBodyAngles;
            this.BasePlayerPlayerModelIsLocalPlayer = tmp.BasePlayerPlayerModelIsLocalPlayer;
            this.BasePlayerPlayerModelPosition = tmp.BasePlayerPlayerModelPosition;
            this.BasePlayerPlayerModelRotation = tmp.BasePlayerPlayerModelRotation;
            this.ItemContainerContainerBelt = tmp.ItemContainerContainerBelt;
            this.ItemContainerListContents = tmp.ItemContainerListContents;
            this.BaseCombatEntityHealth = tmp.BaseCombatEntityHealth;
            this.BaseCombatDeathTime = tmp.BaseCombatDeathTime;
            this.BuildingPrivlidgeAuthorizedPlayers = tmp.BuildingPrivlidgeAuthorizedPlayers;
            this.BuildingPrivlidgeCachedProtectedMinutes = tmp.BuildingPrivlidgeCachedProtectedMinutes;
            this.PlayerNameIDUsername = tmp.PlayerNameIDUsername;
            this.PlayerNameIDSteamID = tmp.PlayerNameIDSteamID;
            this.TodCycle = tmp.TodCycle;
            this.TodNight = tmp.TodNight;
            this.TodAmbient = tmp.TodAmbient;
            this.ClientEntitiesAll = tmp.ClientEntitiesAll;
            this.ClientEntitiesPlayersOnlyDict = tmp.ClientEntitiesPlayersOnlyDict;
            this.GameObjectTransFormPositionOffset = tmp.GameObjectTransFormPositionOffset;
            this.GameObjectChain = tmp.GameObjectChain;
            this.TransformChain = tmp.TransformChain;
        }

        internal OffsetsRUST Load()
        {
            if (File.Exists(offsetsJsonFile))
            {
                try
                {
                    string s = File.ReadAllText(offsetsJsonFile);
                    var settingsJson = JsonConvert.DeserializeObject<OffsetsRUST>(s, new HexToJsonAndBack
                    {
                    });

                    return settingsJson;
                }
                catch (JsonSerializationException ex)
                {
                    return new OffsetsRUST(true);
                }
            }
            else
            {
                return new OffsetsRUST(true);
            }
        }
    }
}