using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace NormandyNET.Core
{
    public class OffsetsARMA : Offsets
    {
        public override string offsetsJsonFile
        {
            get { return "offsetsARMA.json"; }
        }

        #region OffsetsFields

        public ulong World;
        public ulong NetworkManager;
        public ulong NetworkClient;

        public ulong ScoreBoard;
        public ulong PlayerIdentitySize;
        public ulong ServerName;

        public ulong WorldCamera;
        public ulong WorldPlayerOn;

        public ulong Entity_SortedObject;
        public ulong Entity_FutureVisualState;
        public ulong Renderer_VisualState;
        public ulong Entity_NetworkId;

        public ulong VisualState_Direction;
        public ulong VisualState_Position;
        public ulong VisualState_HeadPos;
        public ulong VisualState_ChestPos;

        public ulong PlayerIdentitySizeNetworkId = 0x8;

        public ulong PlayerIdentitySizePlayerName = 0x188;

        public ulong EntityTable_Near;
        public ulong EntityTable_Far;
        public ulong EntityTable_FarFar;
        public ulong EntityTable_FarFarFar;

        public ulong EntityTable_Bullet;

        public ulong EntityTable_BuildingObjectsAndLoot;
        public ulong EntityTable_BuildingObjectsAndLoot_2;

        public ulong Entity_Type_TypeName;
        public ulong Entity_SimulationName;

        public ulong EntityTeamId;
        public ulong Entity_IsDead;
        public ulong Entity_Type;

        public ulong Entity_WeaponSlotsManager;
        public ulong WeaponInHands;
        public ulong WeaponId;
        public ulong WeaponNamePtr;
        public ulong WeaponIdToWeaponType;

        public ulong camera_viewright;
        public ulong camera_viewup;
        public ulong camera_viewforward;
        public ulong camera_viewtranslation;
        public ulong camera_viewportsize;
        public ulong camera_projection_d1;
        public ulong camera_projection_d2;

        public ulong Entity_Passengers;
        public ulong Entity_Turret;
        public ulong Entity_TurretManner;
        public ulong Entity_Driver;

        #endregion OffsetsFields

        public OffsetsARMA(bool cleanOffsets = false)
        {
            if (cleanOffsets == false)
            {
                FillDefaultData();
            }
        }

        public void FillDefaultData()
        {
            World = 0x26166D8;
            NetworkManager = 0x25D3B48;
            NetworkClient = 0x48;

            ScoreBoard = 0x38;
            PlayerIdentitySize = 0x2F8;
            ServerName = 0x8C0;

            WorldCamera = 0xB50;
            WorldPlayerOn = 0x2A60;

            Entity_SortedObject = 0x8;
            Entity_FutureVisualState = 0xD0;
            Renderer_VisualState = 0x190;
            Entity_NetworkId = 0xB9C;

            VisualState_Direction = 0x20;
            VisualState_Position = VisualState_Direction + 0x2C;
            VisualState_HeadPos = 0x168;
            VisualState_ChestPos = 0x174;

            PlayerIdentitySizeNetworkId = 0x8;

            EntityTable_Near = 0x1A40;
            EntityTable_Far = EntityTable_Near + 0xC8;
            EntityTable_FarFar = EntityTable_Far + 0xC8;
            EntityTable_FarFarFar = EntityTable_FarFar + 0xC8;
            EntityTable_BuildingObjectsAndLoot = 0x2090;
            EntityTable_BuildingObjectsAndLoot_2 = 0x2090 + 0xC8;

            EntityTable_Bullet = 0x170;

            Entity_Type_TypeName = 0x68;
            Entity_SimulationName = 0xD0;

            EntityTeamId = 0x340;
            Entity_IsDead = 0x504;

            Entity_Type = 0x150;

            Entity_WeaponSlotsManager = 0xCD8;

            WeaponInHands = 0x38;
            WeaponId = 0x2C;
            WeaponNamePtr = 0x38;
            WeaponIdToWeaponType = 0x138;

            camera_viewright = 0x8;
            camera_viewup = 0x14;
            camera_viewforward = 0x20;
            camera_viewtranslation = 0x2C;
            camera_viewportsize = 0x58;
            camera_projection_d1 = 0xD0;
            camera_projection_d2 = 0xDC;

            Entity_Passengers = 0xE88 + 0x8;
            Entity_Turret = 0xF48 + 0x8;
            Entity_TurretManner = 0x1D8;
            Entity_Driver = 0xDD8 + 0x8;
        }

        internal override void LoadThis()
        {
            if (File.Exists(offsetsJsonFile))
            {
                try
                {
                    string s = File.ReadAllText(offsetsJsonFile);
                    var settingsJsonTmp = JsonConvert.DeserializeObject<OffsetsARMA>(s, new HexToJsonAndBack
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

        public void CopyThis(OffsetsARMA tmp)
        {
        }

        internal OffsetsARMA Load()
        {
            if (File.Exists(offsetsJsonFile))
            {
                try
                {
                    string s = File.ReadAllText(offsetsJsonFile);
                    var settingsJson = JsonConvert.DeserializeObject<OffsetsARMA>(s, new HexToJsonAndBack
                    {
                    });

                    return settingsJson;
                }
                catch (JsonSerializationException ex)
                {
                    return new OffsetsARMA(true);
                }
            }
            else
            {
                return new OffsetsARMA(true);
            }
        }
    }
}