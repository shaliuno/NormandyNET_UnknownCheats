using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace NormandyNET.Core
{
    public class OffsetsDAYZ : Offsets
    {
        public override string offsetsJsonFile
        {
            get { return "offsetsDAYZ.json"; }
        }

        #region OffsetsFields

        public uint World;
        public uint NetworkManager;
        public ulong NetworkClient;
        public ulong NetworkClient_ScoreBoard;
        public ulong NetworkClient_ScoreBoard_PlayerIdentitySize;
        public ulong NetworkClient_ServerName;
        public ulong PlayerIdentity_NetworkId;
        public ulong PlayerIdentity_PlayerName;
        public ulong PlayerIdentity_SteamId;
        public ulong World_Camera;
        public ulong World_Table_NearAnimal;
        public ulong World_Table_FarAranimal;
        public ulong World_Table_SlowAnimal;
        public ulong World_Table_Items;
        public ulong World_Table_FastAnimal;
        public ulong World_Table_Comms;
        public ulong World_PlayerOn;
        public ulong World_Time;
        public ulong World_AmbientLight;
        public ulong World_NoGrass;
        public ulong World_NoGrass_Online;
        public ulong Camera_ViewRight;
        public ulong Camera_ViewUp;
        public ulong Camera_ViewForward;
        public ulong Camera_ViewTranslation;
        public ulong Camera_ViewportSize;
        public ulong Camera_Projection_D1;
        public ulong Camera_Projection_D2;
        public ulong EntitySortedObject;
        public ulong FutureVisualState;
        public ulong RendererVisualState;
        public ulong RenderEntityType;
        public ulong Entity_Networkid;
        public ulong Entity_Inventory;
        public ulong Inventory_ItemInHands;
        public ulong EntityType_TypeName;
        public ulong EntityType_ModelName;
        public ulong EntityType_ConfigName;
        public ulong EntityType_CleanName;
        public ulong VisualState_Position;
        public ulong VisualState_Velocity;
        public ulong VisualState_HeadPosition;
        public ulong VisualState_Direction;
        public uint OffsetVerify;

        #endregion OffsetsFields

        internal override void LoadThis(JObject offsetsPart)
        {
            try
            {
                var settingsJsonTmp = JsonConvert.DeserializeObject<OffsetsDAYZ>(offsetsPart.ToString(), new HexToJsonAndBack
                {
                });

                CopyThis(settingsJsonTmp);
                applied = (OffsetVerify == 0x10);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        internal override void LoadThis()
        {
        }

        public void CopyThis(OffsetsDAYZ tmp)
        {
            this.World = tmp.World;
            this.NetworkManager = tmp.NetworkManager;
            this.NetworkClient = tmp.NetworkClient;
            this.NetworkClient_ScoreBoard = tmp.NetworkClient_ScoreBoard;
            this.NetworkClient_ScoreBoard_PlayerIdentitySize = tmp.NetworkClient_ScoreBoard_PlayerIdentitySize;
            this.NetworkClient_ServerName = tmp.NetworkClient_ServerName;
            this.PlayerIdentity_NetworkId = tmp.PlayerIdentity_NetworkId;
            this.PlayerIdentity_PlayerName = tmp.PlayerIdentity_PlayerName;
            this.PlayerIdentity_SteamId = tmp.PlayerIdentity_SteamId;
            this.World_Camera = tmp.World_Camera;
            this.World_Table_NearAnimal = tmp.World_Table_NearAnimal;
            this.World_Table_FarAranimal = tmp.World_Table_FarAranimal;
            this.World_Table_SlowAnimal = tmp.World_Table_SlowAnimal;
            this.World_Table_Items = tmp.World_Table_Items;
            this.World_Table_FastAnimal = tmp.World_Table_FastAnimal;
            this.World_Table_Comms = tmp.World_Table_Comms;
            this.World_PlayerOn = tmp.World_PlayerOn;
            this.World_Time = tmp.World_Time;
            this.World_AmbientLight = tmp.World_AmbientLight;
            this.World_NoGrass = tmp.World_NoGrass;
            this.World_NoGrass_Online = tmp.World_NoGrass_Online;
            this.Camera_ViewRight = tmp.Camera_ViewRight;
            this.Camera_ViewUp = tmp.Camera_ViewUp;
            this.Camera_ViewForward = tmp.Camera_ViewForward;
            this.Camera_ViewTranslation = tmp.Camera_ViewTranslation;
            this.Camera_ViewportSize = tmp.Camera_ViewportSize;
            this.Camera_Projection_D1 = tmp.Camera_Projection_D1;
            this.Camera_Projection_D2 = tmp.Camera_Projection_D2;
            this.EntitySortedObject = tmp.EntitySortedObject;
            this.FutureVisualState = tmp.FutureVisualState;
            this.RendererVisualState = tmp.RendererVisualState;
            this.RenderEntityType = tmp.RenderEntityType;
            this.Entity_Networkid = tmp.Entity_Networkid;
            this.Entity_Inventory = tmp.Entity_Inventory;
            this.Inventory_ItemInHands = tmp.Inventory_ItemInHands;
            this.EntityType_TypeName = tmp.EntityType_TypeName;
            this.EntityType_ModelName = tmp.EntityType_ModelName;
            this.EntityType_ConfigName = tmp.EntityType_ConfigName;
            this.EntityType_CleanName = tmp.EntityType_CleanName;
            this.VisualState_Position = tmp.VisualState_Position;
            this.VisualState_Velocity = tmp.VisualState_Velocity;
            this.VisualState_HeadPosition = tmp.VisualState_HeadPosition;
            this.VisualState_Direction = tmp.VisualState_Direction;
            this.OffsetVerify = tmp.OffsetVerify;
        }

        public OffsetsDAYZ(bool cleanOffsets = false)
        {
            if (cleanOffsets == false)
            {
            }
        }
    }
}