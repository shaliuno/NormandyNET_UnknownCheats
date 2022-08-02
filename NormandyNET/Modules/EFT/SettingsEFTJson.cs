using NormandyNET.Modules.EFT;
using NormandyNET.Modules.EFT.Improvements;
using System;
using System.Collections.Generic;
using System.Drawing;
using static NormandyNET.Modules.EFT.UI.AimBotSettings;

namespace NormandyNET.Settings
{
    [Serializable]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class SettingsEFTJson
    {
        public ColorSetting Colors = new ColorSetting();

        public DebugSetting Debug = new DebugSetting();

        public EntitySetting Entity = new EntitySetting();

        public LootSetting Loot = new LootSetting();

        public MapSetting Map = new MapSetting();

        public OverlaySetting Overlay = new OverlaySetting();

        [NonSerialized]
        public int[] MapZoomLevelsMinMaxRange = { 1, 10 };

        public OnScreenDisplaySetting OnScreenDisplay = new OnScreenDisplaySetting();

        public MemoryWritingSetting MemoryWriting = new MemoryWritingSetting();

        public SettingsEFTJson()
        {
            FillDefaults();
        }

        private void FillDefaults()
        {
            Entity.LineOfSight.Enemy = 50;
            Entity.LineOfSight.You = 50;

            Entity.Elevation.Arrows = true;
            Entity.Elevation.Type = ElevationType.None;

            Colors.LootColors.ColorsChanged = false;
            Colors.EntityColors.ColorsChanged = false;

            Colors.EntityColors.ListTeamColors = new List<Color>(
                new Color[]
                {
                       Color.MediumOrchid,
                       Color.FromArgb(255, 96, 255),
                       Color.FromArgb(96, 196, 255),
                       Color.Gold,

                       Color.MediumOrchid,
                       Color.FromArgb(255, 96, 255),
                       Color.FromArgb(96, 196, 255),
                });
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class ColorSetting
        {
            [NonSerialized]
            public readonly Color ColorOSD = Color.Yellow;

            [NonSerialized]
            public readonly Color ColorText = Color.FromArgb(255, 255, 255);

            public EntityColorSetting EntityColors = new EntityColorSetting();
            public LootColorSetting LootColors = new LootColorSetting();

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class EntityColorSetting
            {
                [NonSerialized]
                public bool ColorsChanged;

                public Color Bot = System.Drawing.ColorTranslator.FromHtml("#FF8000");
                public Color BotElite = System.Drawing.ColorTranslator.FromHtml("Yellow");
                public Color BotHuman = System.Drawing.ColorTranslator.FromHtml("#9933FF");
                public Color IconPlayerCheater = System.Drawing.ColorTranslator.FromHtml("#FF3030");
                public Color PMC = System.Drawing.ColorTranslator.FromHtml("DodgerBlue");
                public Color Special = System.Drawing.ColorTranslator.FromHtml("#FF3030");
                public Color Boss = System.Drawing.ColorTranslator.FromHtml("#FF3030");
                public Color Teammate = System.Drawing.ColorTranslator.FromHtml("#00FFFF");
                public Color You = System.Drawing.ColorTranslator.FromHtml("#80FF00");
                public Color Corpse = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                public Color Grenade = System.Drawing.ColorTranslator.FromHtml("#FF0000");

                public List<Color> ListTeamColors;
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class LootColorSetting
            {
                [NonSerialized]
                public bool ColorsChanged;

                public Dictionary<string, Color> LootCategoryColors = new Dictionary<string, Color>
                {
                    { "4.6x30" , ColorTranslator.FromHtml("#a5db6f") },
                    { "5.7x28" , ColorTranslator.FromHtml("#a5db6f") },
                    { "5.45x39" , ColorTranslator.FromHtml("#a5db6f") },
                    { "5.56x45" , ColorTranslator.FromHtml("#a5db6f") },
                    { "7.62x25" , ColorTranslator.FromHtml("#a5db6f") },
                    { "7.62x39" , ColorTranslator.FromHtml("#a5db6f") },
                    { "7.62x51" , ColorTranslator.FromHtml("#a5db6f") },
                    { "7.62x54R" , ColorTranslator.FromHtml("#a5db6f") },
                    { "9x18" , ColorTranslator.FromHtml("#a5db6f") },
                    { "9x19" , ColorTranslator.FromHtml("#a5db6f") },
                    { "9x21" , ColorTranslator.FromHtml("#a5db6f") },
                    { "9x39" , ColorTranslator.FromHtml("#a5db6f") },
                    { "12.7x55" , ColorTranslator.FromHtml("#a5db6f") },
                    { "12.7x108" , ColorTranslator.FromHtml("#a5db6f") },
                    { "12ga" , ColorTranslator.FromHtml("#a5db6f") },
                    { "20ga" , ColorTranslator.FromHtml("#a5db6f") },
                    { "23ga" , ColorTranslator.FromHtml("#a5db6f") },
                    { "40mm" , ColorTranslator.FromHtml("#a5db6f") },
                    { "45" , ColorTranslator.FromHtml("#a5db6f") },
                    { "300BLK" , ColorTranslator.FromHtml("#a5db6f") },
                    { "338L" , ColorTranslator.FromHtml("#a5db6f") },
                    { "366TKM" , ColorTranslator.FromHtml("#a5db6f") },
                    { "Additional Armors" , ColorTranslator.FromHtml("#baadff") },
                    { "Armor Vests" , ColorTranslator.FromHtml("#468400") },
                    { "Armored Helmets" , ColorTranslator.FromHtml("#3a64b7") },
                    { "Armored Rigs" , ColorTranslator.FromHtml("#044268") },
                    { "Assault Carbines" , ColorTranslator.FromHtml("#3856aa") },
                    { "Assault Rifles" , ColorTranslator.FromHtml("#edabf2") },
                    { "Attachments" , ColorTranslator.FromHtml("#015959") },
                    { "Auxillary Parts" , ColorTranslator.FromHtml("#43a3a8") },
                    { "Backpacks" , ColorTranslator.FromHtml("#301fa3") },
                    { "Barrels" , ColorTranslator.FromHtml("#bfe4ff") },
                    { "Barter Items" , ColorTranslator.FromHtml("#a9d1e8") },
                    { "Bipods" , ColorTranslator.FromHtml("#efaef2") },
                    { "Bolt-Action Rifles" , ColorTranslator.FromHtml("#a3d86e") },
                    { "Charging Handles" , ColorTranslator.FromHtml("#ce742f") },
                    { "Container" , ColorTranslator.FromHtml("#a4bda9") },
                    { "Currency" , ColorTranslator.FromHtml("#FF8000") },
                    { "Foregrips" , ColorTranslator.FromHtml("#9db9dd") },
                    { "Gas Blocks" , ColorTranslator.FromHtml("#f9bdcf") },
                    { "Handguards" , ColorTranslator.FromHtml("#52870d") },
                    { "Headsets" , ColorTranslator.FromHtml("#ff84b7") },
                    { "Keys" , ColorTranslator.FromHtml("#0e8e4c") },
                    { "Lights & Lasers" , ColorTranslator.FromHtml("#2d18a3") },
                    { "Lootable" , ColorTranslator.FromHtml("#e5a275") },
                    { "Machine Guns" , ColorTranslator.FromHtml("#a1f4df") },
                    { "Magazines" , ColorTranslator.FromHtml("#4672c4") },
                    { "Marksman Rifles" , ColorTranslator.FromHtml("#a1c438") },
                    { "Medical" , ColorTranslator.FromHtml("#1e1d7f") },
                    { "Melee" , ColorTranslator.FromHtml("#38f7c4") },
                    { "Mounts" , ColorTranslator.FromHtml("#7bedb2") },
                    { "Muzzle Devices" , ColorTranslator.FromHtml("#c6fc9c") },
                    { "Night Vision" , ColorTranslator.FromHtml("#47ffbb") },
                    { "Parent Element" , ColorTranslator.FromHtml("#fcc8c4") },
                    { "Pistol Grips" , ColorTranslator.FromHtml("#3397a0") },
                    { "Pistols" , ColorTranslator.FromHtml("#9d68e8") },
                    { "Port. Container" , ColorTranslator.FromHtml("#aa2e55") },
                    { "Provisions" , ColorTranslator.FromHtml("#f25e3a") },
                    { "Quest" , ColorTranslator.FromHtml("#9af9bc") },
                    { "Receivers & Slides" , ColorTranslator.FromHtml("#FF8000") },
                    { "Shotguns" , ColorTranslator.FromHtml("#0faf22") },
                    { "Sights" , ColorTranslator.FromHtml("#2d8989") },
                    { "SMGs" , ColorTranslator.FromHtml("#baf713") },
                    { "Special" , ColorTranslator.FromHtml("#c66ad8") },
                    { "Stash" , ColorTranslator.FromHtml("#988def") },
                    { "Stocks & Chassis" , ColorTranslator.FromHtml("#390f8c") },
                    { "Tactical Rigs" , ColorTranslator.FromHtml("#b00000") },
                    { "Throwables" , ColorTranslator.FromHtml("#efd25d") },
                    { "Tools" , ColorTranslator.FromHtml("#ba8735") },
                    { "Unknown" , ColorTranslator.FromHtml("#A7E199") },
                    { "Vanity" , ColorTranslator.FromHtml("#605EE9") },
                    { "Visors" , ColorTranslator.FromHtml("#5C05DF") },
                };
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class DebugSetting
        {
            [NonSerialized]
            public bool DebugStuff = false;
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class MemoryWritingSetting
        {
            public bool DisclaimerAgreed;

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class MemoryWritingSettingBase
            {
                public bool Enabled = false;
            }

            [NonSerialized]
            public bool NoRecoilAdsConfirmed = false;

            public NoRecoilSetting NoRecoil = new NoRecoilSetting();
            public MemoryWritingSettingBase InstantADS = new MemoryWritingSettingBase();
            public MemoryWritingSettingBase UtilityHack = new MemoryWritingSettingBase();
            public MemoryWritingSettingBase NightVision = new MemoryWritingSettingBase();
            public MemoryWritingSettingBase PinkDudes = new MemoryWritingSettingBase();
            public MemoryWritingSettingBase NoVisor = new MemoryWritingSettingBase();
            public MemoryWritingSettingBase ThermalVision = new MemoryWritingSettingBase();
            public MemoryWritingSettingBase UnlimitedStamina = new MemoryWritingSettingBase();
            public MemoryWritingSettingBase AlwaysSprintAltMode = new MemoryWritingSettingBase();
            public MemoryWritingSettingBase FastRPM = new MemoryWritingSettingBase();
            public LootThroughWallsSetting LootThroughWalls = new LootThroughWallsSetting();
            public SkillHackSetting SkillHack = new SkillHackSetting();
            public AimBotSetting AimBot = new AimBotSetting();
            public FlyHackSetting FlyHack = new FlyHackSetting();
            public SignatureSettingBase AlwaysSprint = new SignatureSettingBase();
            public LeanHackSetting LeanHack = new LeanHackSetting();
            public FastReloadSetting FastReload = new FastReloadSetting();
            public MemoryWritingSettingBase FastRunning = new MemoryWritingSettingBase();
            public MemoryWritingSettingBase NoInertia = new MemoryWritingSettingBase();

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class NoRecoilSetting : MemoryWritingSettingBase
            {
                public float Intensity = 0f;
                public bool StreamerSafe = false;
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class LootThroughWallsSetting : MemoryWritingSettingBase
            {
                public float Distance = 2f;
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class LeanHackSetting : MemoryWritingSettingBase
            {
                public float Distance = 0.12f;
            }

            [NonSerialized]
            internal bool MoveDo = false;

            [NonSerialized]
            public float FallDamageOriginal = 0;

            [NonSerialized]
            internal Movements MoveDirection;

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class SkillHackSetting : MemoryWritingSettingBase
            {
                public bool MagDrills = false;
                public bool SuperJump = false;
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class AimBotSetting : MemoryWritingSettingBase
            {
                public bool Randomizer = false;
                public float RandomizerStrength = 1f;
                public float PredictionForce = 2f;
                public bool Prediction = false;
                public bool TargetTeammates = false;
                public bool TriggerByAiming = false;
                public bool TriggerByTilt = false;
                public bool Highlight = false;
                public bool Prioritizing = false;
                public bool ClosestToCrosshair = false;
                public bool ClosestByDistance = false;
                public uint Distance = 75;
                public uint CycleBonesDelay = 200;

                public SelectedBones SelectedBones = SelectedBones.OnlyHead;

                [NonSerialized]
                public bool SelectedBonesUpdate = false;

                public BoneReadable[,] CustomPatternBones = new BoneReadable[3, 9] {
                        {
                            BoneReadable.Head,
                            BoneReadable.LForearm1,
                            BoneReadable.Neck,
                            BoneReadable.Pelvis,
                            BoneReadable.LCalf,
                            BoneReadable.RCalf,
                            BoneReadable.None,
                            BoneReadable.None,
                            BoneReadable.None,
                        },

                        {
                            BoneReadable.Head,
                            BoneReadable.Neck,
                            BoneReadable.LUpperarm,
                            BoneReadable.RUpperarm,
                            BoneReadable.LCollarbone,
                            BoneReadable.RCollarbone,
                            BoneReadable.None,
                            BoneReadable.None,
                            BoneReadable.None,
                        },

                        {
                            BoneReadable.Pelvis,
                            BoneReadable.LCalf,
                            BoneReadable.RCalf,
                            BoneReadable.None,
                            BoneReadable.None,
                            BoneReadable.None,
                            BoneReadable.None,
                            BoneReadable.None,
                            BoneReadable.None,
                        },
                    };

                public uint CustomPatternBoneIndex = 0;

                public float AngleX = 2f;
                public float AngleY = 2f;
                public bool ClosestTargetOverride;
                public float DistanceOverride = 75;
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class SignatureSettingBase
            {
                public List<SigscanSignature> Signatures = new List<SigscanSignature>();

                [NonSerialized]
                public bool Enabled = false;

                public uint ProcessID = 0;
                public ulong ImageBase = 0;
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class FastReloadSetting : SignatureSettingBase
            {
                public string ClassName = string.Empty;
                public bool Enabled = false;
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class FlyHackSetting : SignatureSettingBase
            {
                [NonSerialized]
                public bool MoveUp = false;

                public float Intensity = -0.15f;
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class EntitySetting
        {
            public ElevationSetting Elevation = new ElevationSetting();
            public LineOfSightSetting LineOfSight = new LineOfSightSetting();

            public bool Bodies = true;
            public bool Distance = true;
            public bool Health = true;
            public bool Side = true;
            public bool KDRatio = true;
            public float KDRatioThreshold = 10f;
            public bool InventoryValue = true;
            public bool InventoryValueUseLootFilters = false;
            public bool Level = true;
            public bool LOS = true;
            public bool Name = true;
            public bool Weapon = true;
            public bool ArmorClass = true;

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class ElevationSetting
            {
                public bool Arrows = true;
                public ElevationType Type = ElevationType.Relative;
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public struct LineOfSightSetting
            {
                public int Enemy;
                public int You;
                public bool Solid;
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class LootSetting
        {
            public bool Show = true;
            public bool LiveLoot = false;
            public bool ShortNames = false;
            public int LiveLootPerCycle = 20;
            public int NameLengthLimit = 50;

            public List<string> LootListToLookFor = new List<string>();
            public List<string> LootCategorySuppressed = new List<string>();

            [NonSerialized]
            public bool ShowStatusChanged = true;

            public List<bool> AlwaysShowPriority = new List<bool>(
             new bool[]
             {
                    false,
                    false,
                    false,
                    true,
                    true,
                    true,
             });

            public bool ShowByValue = false;
            public bool ShowQuestItems = false;
            public bool ShowPricesAlways = false;
            public bool ForceShow = false;
            public bool ValuePerSlot = false;
            public int Value = 5;
            public bool Aggregate = false;
            public bool ReadLootInContainers = true;
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class MapSetting
        {
            public bool CenterMap = true;
            public bool DrawText = true;
            public bool AutoHeight = true;
            public bool ExfiltrationPoint = false;
            public int MapResolution = 2;

            public float IconSizePlayers = 0.5f;
            public float IconSizeLoot = 0.5f;
            public float TextScale = 1f;
            public bool HideTextAroundPlayer = false;
            public int HideTextAroundPlayerDistance = 75;
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class OverlaySetting
        {
            public int DrawDistance = 100;
            public int DrawDistanceLoot = 25;
            public WindowStyleEnum WindowStyle = WindowStyleEnum.FullScreen;
            public Size GameResolution = new Size(1920, 1080);
            public BoneSettings Bones = new BoneSettings();

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class BoneSettings
            {
                public bool Humans = true;
                public bool AI = false;
                public bool HighDetail = false;
                public int DrawDistance = 25;
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class OnScreenDisplaySetting
        {
            public bool DateTime = true;
            public bool FPS = true;
            public bool NetworkStats = true;
            public bool Stats = true;
        }
    }
}