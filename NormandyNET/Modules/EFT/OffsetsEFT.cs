using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NormandyNET.Core;
using System.IO;
using System.Reflection;

namespace NormandyNET.Modules.EFT
{
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class OffsetsEFT : Offsets
    {
        public override string offsetsJsonFile
        {
            get { return "offsetsEFT.json"; }
        }

        #region OffsetsFields

        public uint GOM_Offset;
        public uint TimeScale;
        public uint TimeScale_Value;

        public uint GameWorld_RegisteredPlayers;
        public uint GameWorld_LootList;

        public uint LocalGameWorld_ExfiltrationController;
        public uint ExfiltrationController_ExfiltrationPoint;
        public uint ExfiltrationController_ScavExfiltrationPoint;
        public uint ExfiltrationPoint_ExitTriggerSettings;
        public uint ExfiltrationPoint_ExfiltrationStatus;
        public uint ExfiltrationPoint_Name;
        public uint ExfiltrationPoint_EligibleEntryPoints;
        public uint ExfiltrationPoint_Requirements;
        public uint ExfiltrationRequirement_Requirement;

        public uint Player_IsDeadAlready;
        public uint Player_IsLocalPlayer;
        public uint Player_PlayerBody;
        public uint Player_PlayerBody_SkinnedMesh;
        public uint Player_PlayerBody_SlotsView;

        public uint Player_HealthController;

        public uint Player_Physical;
        public uint Player_Physical_IsSprinting;
        public uint Player_Physical_Stamina;
        public uint Player_Physical_Stamina_Current;

        public uint Player_Physical_SprintAcceleration;
        public uint Player_Physical_PreSprintAcceleration;

        public uint Player_MovementContext;
        public uint Player_MovementContext_AzimuthAndPosition;
        public uint Player_MovementContext_Tilt;
        public uint Player_MovementContext_FreeFallTime;
        public uint Player_MovementContext_EPhysicalCondition;
        public uint Player_MovementContext_MovementState_Current;

        public uint Player_MovementState_RotationSpeedClamp;
        public uint Player_MovementState_StateSensitivity;

        public uint Player_ProceduralWeaponAnimation;
        public uint Player_ProceduralWeaponAnimation_Mask;
        public uint Player_ProceduralWeaponAnimation_FovCompensatoryDistance;
        public uint Player_ProceduralWeaponAnimation_HandsContainer;
        public uint Player_ProceduralWeaponAnimation_FirearmContoller;
        public uint Player_ProceduralWeaponAnimation_BreathEffector;
        public uint Player_ProceduralWeaponAnimation_WalkEffector;
        public uint Player_ProceduralWeaponAnimation_MotionEffector;
        public uint Player_ProceduralWeaponAnimation_ForceEffector;
        public uint Player_ProceduralWeaponAnimation_ShotEffector;
        public uint Player_ProceduralWeaponAnimation_AimSwayUnk0;

        public uint Player_ProceduralWeaponAnimation_HandsContainer_FireportTransformBifacial;
        public uint Player_ProceduralWeaponAnimation_FastADS;

        public uint Player_HandsController;
        public uint Player_HandsController_CurrentOperation;

        public uint Player_FirearmController_IsAiming;
        public uint Player_FirearmController_WeaponLn;

        public uint Player_BreathEffector_NoSway;
        public uint Player_BreathEffector_Intensity;
        public uint Player_WalkEffector_Intensity;
        public uint Player_MotionEffector_Intensity;
        public uint Player_ForceEffector_Intensity;
        public uint Player_ShotEffector_Intensity;

        public uint Player_Profile;
        public uint Player_Profile_AccountID;
        public uint Player_Profile_Info;
        public uint Player_Profile_Info_Nickname;
        public uint Player_Profile_Info_GroupID;
        public uint Player_Profile_Info_MemberCategory;
        public uint Player_Profile_Info_RegistrationDate;
        public uint Player_Profile_Info_EntryPoint;

        public uint Player_Profile_Info_Side;
        public uint Player_Profile_Info_Experience;
        public uint Player_Profile_Info_Settings;
        public uint Player_Profile_Info_Settings_Role;
        public uint Player_Profile_Stats;
        public uint Player_Profile_Stats_OverallCounters;
        public uint Player_Profile_Skills;
        public uint Skills_MagDrillsLoadSpeed;
        public uint Skills_MagDrillsUnloadSpeed;
        public uint Skills_StrengthBuffJumpHeightInc;
        public uint Skills_BotReloadSpeed;
        public uint Skills_AttentionExamine;

        public uint Player_InventoryController;
        public uint Player_InventoryController_Inventory;
        public uint Player_InventoryController_Inventory_Equipment;
        public uint Player_InventoryController_Inventory_Equipment_EquipmentSlotsArray;

        public uint InventoryLogic_Slot_ContainedItem;

        public uint InventoryLogic_Item_ItemTemplate;
        public uint InventoryLogic_Item_SpawnedInSession;
        public uint InventoryLogic_Item_StackedObjectCount;
        public uint InventoryLogic_Item_ItemGrids;
        public uint InventoryLogic_Item_ItemSlots;
        public uint InventoryLogic_Item_Components;

        public uint InventoryLogic_ItemTemplate_Id;
        public uint InventoryLogic_ItemTemplate_Name;
        public uint InventoryLogic_ItemTemplate_BackgroundColor;

        public uint Interactive_LootItem_ContainedItem;

        public uint Interactive_LootableContainer_ItemOwner;
        public uint Interactive_LootableContainer_ItemOwner_ContainedItem;

        public uint Interactive_Corpse_PlayerBody;
        public uint Interactive_Corpse_PlayerSide;

        public uint InventoryLogic_Weapon_Chambers;
        public uint InventoryLogic_AmmoTemplate_InitialSpeed;
        public uint InventoryLogic_WeaponTemplate_BoltAction;
        public uint InventoryLogic_WeaponTemplate_WeapFireType;
        public uint InventoryLogic_WeaponTemplate_SingleFireRate;
        public uint InventoryLogic_WeaponTemplate_FireRate;
        public uint InventoryLogic_WeaponTemplate_AllowJam;
        public uint InventoryLogic_WeaponTemplate_AllowFeed;
        public uint InventoryLogic_WeaponTemplate_AllowMisfire;
        public uint InventoryLogic_WeaponTemplate_AllowSlide;
        public uint InventoryLogic_WeaponTemplate_AllowOverheat;
        public uint InventoryLogic_ArmorComponent_Template;
        public uint InventoryLogic_ArmorComponent_Template_SpeedPenaltyPercent;
        public uint InventoryLogic_ArmorComponent_Template_MousePenalty;
        public uint InventoryLogic_MagazineTemplate_LoadUnloadModifier;

        public uint Component_VisorEffect_Intensity;

        public uint Component_NightVision_On;
        public uint Component_NightVision_Intensity;
        public uint Component_NightVision_NoiseIntensity;
        public uint Component_NightVision_DiffuseIntensity;

        public uint Component_ThermalVision_On;
        public uint Component_ThermalVision_IsNoisy;
        public uint Component_ThermalVision_IsFpsStuck;
        public uint Component_ThermalVision_IsMotionBlurred;
        public uint Component_ThermalVision_IsGlitch;
        public uint Component_ThermalVision_IsPixelated;
        public uint Component_ThermalVision_ThermalVisionUtilities;
        public uint ThermalVisionUtilities_CurrentRampPalette;
        public uint ThermalVisionUtilities_DepthFade;

        public uint Component_ThermalVision_ChromaticAberrationThermalShift;
        public uint Component_ThermalVision_UnsharpRadiusBlur;
        public uint Component_ThermalVision_UnsharpBias;
        public uint Component_ThermalVision_ChromaticAberration;

        public uint ChromaticAberration_Shift;

        public uint MainApplication_Backend_Interface;
        public uint MainApplication_Backend_Class;
        public uint MainApplication_Backend_Class_BackEndConfig;
        public uint MainApplication_Backend_Class_BackEndConfig_Config;
        public uint MainApplication_Backend_Class_BackEndConfig_Config_Inertia;

        public uint Inertia_ExitMovementStateSpeedThreshold;
        public uint Inertia_SpeedLimitAfterFallMin;
        public uint Inertia_SpeedLimitAfterFallMax;
        public uint Inertia_SpeedLimitDurationMin;
        public uint Inertia_SpeedLimitDurationMax;
        public uint Inertia_WalkInertia;
        public uint Inertia_SideTime;
        public uint Inertia_SpeedInertiaAfterJump;
        public uint Inertia_PenaltyPower;
        public uint Inertia_MoveTime;
        public uint Inertia_MinDirectionBlendTime;
        public uint Inertia_FallThreshold;
        public uint Inertia_InertiaLimitsStep;
        public uint Inertia_BaseJumpPenalty;
        public uint Inertia_BaseJumpPenaltyDuration;
        public uint Inertia_DurationPower;
        public uint Inertia_SuddenChangesSmoothness;

        public uint ListClass;
        public uint ListClassEntryCount;
        public uint ArrayFirstEntry;

        public uint UnityStringSize;
        public uint UnityString;

        public uint SkinnedMesh_Bone_dict;
        public uint SkinnedMesh_Bone_list;

        public uint[] LocalGameWorldOffsets;

        public uint GameObjectEntry;
        public uint GameObject;
        public uint GameObjectBasicName;
        public uint GameObject_ComponentsFields;
        public uint GameObject_ComponentsList;
        public uint GameObject_Component;
        public uint Transform_TransformAccess;
        public uint TransformPosition;
        public uint TransformRotation;

        public uint CameraMatrix;
        public uint CameraFOV;
        public uint CameraAspect;
        public uint[] CameraComponentTransform;
        public uint[] CameraEntity;
        public uint[] OpticCameraMatrixPointer;

        #endregion OffsetsFields

        internal override void LoadThis(JObject offsetsPart)
        {
            try
            {
                var settingsJsonTmp = JsonConvert.DeserializeObject<OffsetsEFT>(offsetsPart.ToString(), new HexToJsonAndBack
                {
                });

                CopyThis(settingsJsonTmp);
                applied = (ListClass == 0x10);
            }
            catch (JsonSerializationException ex)
            {
            }
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        internal override void LoadThis()
        {
            if (File.Exists(offsetsJsonFile))
            {
                try
                {
                    string s = File.ReadAllText(offsetsJsonFile);
                    var settingsJsonTmp = JsonConvert.DeserializeObject<OffsetsEFT>(s, new HexToJsonAndBack
                    {
                    });

                    CopyThis(settingsJsonTmp);
                }
                catch (JsonSerializationException ex)
                {
                }
            }
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        public void CopyThis(OffsetsEFT tmp)
        {
            FieldInfo[] fields = typeof(OffsetsEFT).GetFields();

            foreach (FieldInfo field in fields)
            {
                if (!object.Equals(field.GetValue(this), field.GetValue(tmp)))
                {
                    field.SetValue(this, field.GetValue(tmp));
                }
            }
        }

        public OffsetsEFT(bool cleanOffsets = false)
        {
            if (cleanOffsets == false)
            {
                FillDefaults();
            }
        }

        internal void FillDefaults()
        {
            ListClass = 0x10;
            ListClassEntryCount = 0x18;
            ArrayFirstEntry = 0x20;

            UnityStringSize = 0x10;
            UnityString = 0x14;

            SkinnedMesh_Bone_dict = 0x28;
            SkinnedMesh_Bone_list = 0x10;

            LocalGameWorldOffsets = new uint[] { 0x30, 0x18, 0x28 };

            GameObjectEntry = 0x10;
            GameObject = 0x30;
            GameObjectBasicName = 0x60;
            GameObject_ComponentsFields = 0x28;
            GameObject_ComponentsList = 0x30;
            GameObject_Component = 0x8;
            Transform_TransformAccess = 0x38;
            TransformPosition = 0x90;
            TransformRotation = 0xC0;

            CameraMatrix = 0xDC;
            CameraFOV = 0x4C8;
            CameraAspect = 0x158;
            CameraComponentTransform = new uint[] { GameObject_ComponentsList, 0x8 };
            CameraEntity = new uint[] { GameObject_ComponentsList, 0x18 };
            OpticCameraMatrixPointer = new uint[] { 0x30, 0x18, 0xC0 };

            GOM_Offset = 0x17FFD28;
            TimeScale = 0x17FFAE0;
            TimeScale_Value = 0xFC;

            GameWorld_RegisteredPlayers = 0x88;
            GameWorld_LootList = 0x68;
            LocalGameWorld_ExfiltrationController = 0x18;
            ExfiltrationController_ExfiltrationPoint = 0x20;
            ExfiltrationController_ScavExfiltrationPoint = 0x28;
            ExfiltrationPoint_ExitTriggerSettings = 0x58;
            ExfiltrationPoint_ExfiltrationStatus = 0xA8;
            ExfiltrationPoint_Name = 0x28;
            ExfiltrationPoint_EligibleEntryPoints = 0x80;
            ExfiltrationPoint_Requirements = 0x60;
            ExfiltrationRequirement_Requirement = 0x20;
            Player_IsDeadAlready = 0x6E0;
            Player_IsLocalPlayer = 0x807;
            Player_PlayerBody = 0xA8;
            Player_PlayerBody_SkinnedMesh = 0x28;
            Player_PlayerBody_SlotsView = 0x50;
            Player_HealthController = 0x528;
            Player_Physical = 0x500;
            Player_Physical_IsSprinting = 0x14C;
            Player_Physical_Stamina = 0x38;
            Player_Physical_Stamina_Current = 0x48;
            Player_Physical_SprintAcceleration = 0x114;
            Player_Physical_PreSprintAcceleration = 0x118;
            Player_MovementContext = 0x40;
            Player_MovementContext_AzimuthAndPosition = 0x22C;
            Player_MovementContext_Tilt = 0x218;
            Player_MovementContext_FreeFallTime = 0x1F8;
            Player_MovementContext_EPhysicalCondition = 0x2B0;
            Player_MovementContext_MovementState_Current = 0xB8;
            Player_MovementState_RotationSpeedClamp = 0x38;
            Player_MovementState_StateSensitivity = 0x3C;
            Player_ProceduralWeaponAnimation = 0x198;
            Player_ProceduralWeaponAnimation_Mask = 0x100;
            Player_ProceduralWeaponAnimation_FovCompensatoryDistance = 0x1AC;
            Player_ProceduralWeaponAnimation_HandsContainer = 0x18;
            Player_ProceduralWeaponAnimation_FirearmContoller = 0x80;
            Player_ProceduralWeaponAnimation_BreathEffector = 0x28;
            Player_ProceduralWeaponAnimation_WalkEffector = 0x30;
            Player_ProceduralWeaponAnimation_MotionEffector = 0x38;
            Player_ProceduralWeaponAnimation_ForceEffector = 0x40;
            Player_ProceduralWeaponAnimation_ShotEffector = 0x48;
            Player_ProceduralWeaponAnimation_AimSwayUnk0 = 0x230;
            Player_ProceduralWeaponAnimation_HandsContainer_FireportTransformBifacial = 0x88;
            Player_ProceduralWeaponAnimation_FastADS = 0x198;
            Player_HandsController = 0x540;
            Player_HandsController_CurrentOperation = 0x88;
            Player_FirearmController_IsAiming = 0x149;
            Player_FirearmController_WeaponLn = 0x144;
            Player_BreathEffector_Intensity = 0xA4;
            Player_WalkEffector_Intensity = 0x44;
            Player_MotionEffector_Intensity = 0xD0;
            Player_ForceEffector_Intensity = 0x28;
            Player_ShotEffector_Intensity = 0x70;
            Player_Profile = 0x4F0;
            Player_Profile_AccountID = 0x18;
            Player_Profile_Info = 0x28;
            Player_Profile_Info_Nickname = 0x10;
            Player_Profile_Info_GroupID = 0x20;
            Player_Profile_Info_MemberCategory = 0x84;
            Player_Profile_Info_RegistrationDate = 0x6C;
            Player_Profile_Info_EntryPoint = 0x28;
            Player_Profile_Info_Side = 0x68;
            Player_Profile_Info_Experience = 0x88;
            Player_Profile_Info_Settings = 0x48;
            Player_Profile_Info_Settings_Role = 0x10;
            Player_Profile_Stats = 0xE8;
            Player_Profile_Stats_OverallCounters = 0x18;
            Player_Profile_Skills = 0x60;
            Skills_MagDrillsLoadSpeed = 0x180;
            Skills_MagDrillsUnloadSpeed = 0x188;
            Skills_StrengthBuffJumpHeightInc = 0x60;
            Skills_BotReloadSpeed = 0x548;
            Skills_AttentionExamine = 0x168;
            Player_InventoryController = 0x538;
            Player_InventoryController_Inventory = 0x128;
            Player_InventoryController_Inventory_Equipment = 0x10;
            Player_InventoryController_Inventory_Equipment_EquipmentSlotsArray = 0x80;
            InventoryLogic_Slot_ContainedItem = 0x38;
            InventoryLogic_Item_ItemTemplate = 0x40;
            InventoryLogic_Item_SpawnedInSession = 0x6C;
            InventoryLogic_Item_StackedObjectCount = 0x64;
            InventoryLogic_Item_ItemGrids = 0x70;
            InventoryLogic_Item_ItemSlots = 0x78;
            InventoryLogic_Item_Components = 0x20;
            InventoryLogic_ItemTemplate_Id = 0x50;
            InventoryLogic_ItemTemplate_Name = 0x58;
            InventoryLogic_ItemTemplate_BackgroundColor = 0xA0;
            Interactive_LootItem_ContainedItem = 0xB0;
            Interactive_LootableContainer_ItemOwner = 0x108;
            Interactive_LootableContainer_ItemOwner_ContainedItem = 0xB8;
            Interactive_Corpse_PlayerBody = 0x128;
            Interactive_Corpse_PlayerSide = 0x140;
            InventoryLogic_Weapon_Chambers = 0x98;
            InventoryLogic_AmmoTemplate_InitialSpeed = 0x18C;
            InventoryLogic_WeaponTemplate_BoltAction = 0x1FC;
            InventoryLogic_WeaponTemplate_WeapFireType = 0x178;
            InventoryLogic_WeaponTemplate_SingleFireRate = 0x1CC;
            InventoryLogic_WeaponTemplate_FireRate = 0x1C8;
            InventoryLogic_WeaponTemplate_AllowJam = 0x24C;
            InventoryLogic_WeaponTemplate_AllowFeed = 0x24D;
            InventoryLogic_WeaponTemplate_AllowMisfire = 0x24E;
            InventoryLogic_WeaponTemplate_AllowSlide = 0x24F;
            InventoryLogic_WeaponTemplate_AllowOverheat = 0x264;
            InventoryLogic_ArmorComponent_Template = 0x18;
            InventoryLogic_ArmorComponent_Template_SpeedPenaltyPercent = 0x158;
            InventoryLogic_ArmorComponent_Template_MousePenalty = 0x15C;
            InventoryLogic_MagazineTemplate_LoadUnloadModifier = 0x174;
            Component_VisorEffect_Intensity = 0xB8;
            Component_NightVision_On = 0xE4;
            Component_NightVision_Intensity = 0xC0;
            Component_NightVision_NoiseIntensity = 0xC8;
            Component_ThermalVision_On = 0xE0;
            Component_ThermalVision_IsNoisy = 0xE1;
            Component_ThermalVision_IsFpsStuck = 0xE2;
            Component_ThermalVision_IsMotionBlurred = 0xE3;
            Component_ThermalVision_IsGlitch = 0xE4;
            Component_ThermalVision_IsPixelated = 0xE5;
            Component_ThermalVision_ThermalVisionUtilities = 0x18;
            ThermalVisionUtilities_CurrentRampPalette = 0x30;
            ThermalVisionUtilities_DepthFade = 0x34;
            Component_ThermalVision_ChromaticAberrationThermalShift = 0xE8;
            Component_ThermalVision_UnsharpRadiusBlur = 0xEC;
            Component_ThermalVision_UnsharpBias = 0xF0;
            Component_ThermalVision_ChromaticAberration = 0xD8;
            ChromaticAberration_Shift = 0x28;
            MainApplication_Backend_Interface = 0x28;
            MainApplication_Backend_Class = 0x58;
            MainApplication_Backend_Class_BackEndConfig = 0x110;
            MainApplication_Backend_Class_BackEndConfig_Config = 0x10;
            MainApplication_Backend_Class_BackEndConfig_Config_Inertia = 0xC8;
            Inertia_ExitMovementStateSpeedThreshold = 0x10;
            Inertia_SuddenChangesSmoothness = 0x10C;
            ListClass = 0x10;
            ListClassEntryCount = 0x18;
            ArrayFirstEntry = 0x20;
            UnityStringSize = 0x10;
            UnityString = 0x14;
            SkinnedMesh_Bone_dict = 0x28;
            SkinnedMesh_Bone_list = 0x10;

            GameObjectEntry = 0x10;
            GameObject = 0x30;
            GameObjectBasicName = 0x60;
            GameObject_ComponentsFields = 0x28;
            GameObject_ComponentsList = 0x30;
            GameObject_Component = 0x8;
            Transform_TransformAccess = 0x38;
            TransformPosition = 0x90;
            TransformRotation = 0xC0;
            CameraMatrix = 0xDC;
            CameraFOV = 0x4C8;
            CameraAspect = 0x158;
        }
    }
}