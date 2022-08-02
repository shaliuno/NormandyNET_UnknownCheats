using System;
using System.Collections.Generic;
using System.Drawing;

namespace NormandyNET.Settings
{
    [Serializable]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class SettingsRUSTJson
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

        [NonSerialized]
        public MemoryWritingSetting MemoryWriting = new MemoryWritingSetting();

        public SettingsRUSTJson()
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
            Colors.LootColors.lootPriority0 = ColorTranslator.FromHtml("#804000");
            Colors.LootColors.lootPriority1 = ColorTranslator.FromHtml("Lime");
            Colors.LootColors.lootPriority2 = ColorTranslator.FromHtml("Aqua");
            Colors.LootColors.lootPriority3 = ColorTranslator.FromHtml("#8000FF");
            Colors.LootColors.lootPriority4 = ColorTranslator.FromHtml("Yellow");
            Colors.LootColors.lootPriority5 = ColorTranslator.FromHtml("Red");
            Colors.LootColors.lootPriorityQuest = ColorTranslator.FromHtml("Yellow");

            Colors.EntityColors.ColorsChanged = false;
        }

        public class MemoryWritingSetting
        {
            [NonSerialized]
            public bool FakeAdmin = false;

            [NonSerialized]
            public bool FakeAdminShowWarning = true;

            [NonSerialized]
            public bool FakeAdminDo = false;

            [NonSerialized]
            public bool BrightDay = false;

            [NonSerialized]
            public bool SpiderMan = false;

            [NonSerialized]
            public bool SpiderManDo = false;

            [NonSerialized]
            public bool BrightDayDo = false;
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class ColorSetting
        {
            [NonSerialized]
            public readonly Color ColorCorpse = Color.FromArgb(0, 0, 0);

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

                public Dictionary<string, Color> EntityTypeColors = new Dictionary<string, Color>
                  {
                      { "LocalPlayer" , ColorTranslator.FromHtml("#80FF00") },
                      { "Player" , ColorTranslator.FromHtml("DodgerBlue") },
                      { "PlayerNPC" , ColorTranslator.FromHtml("#FF8000") },
                      { "VehicleNPC",Color.FromArgb(255, 128, 0)},
                      { "Vehicle",ColorTranslator.FromHtml("Maroon")},
                      { "Unknown" , ColorTranslator.FromHtml("White") },
                      { "Animal" , ColorTranslator.FromHtml("#964B00") },
                      { "House" , ColorTranslator.FromHtml("Lime") },
                      { "Turret" , ColorTranslator.FromHtml("Fuchsia") },
                      { "Teammate" , ColorTranslator.FromHtml("#00FFFF") },
            };
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class LootColorSetting
            {
                [NonSerialized]
                public bool ColorsChanged = true;

                public Dictionary<string, Color> LootCategoryColors = new Dictionary<string, Color>
                {
                    {"Ammo",ColorTranslator.FromHtml("#915A67")},
                    {"Barrel",ColorTranslator.FromHtml("#FF8000")},
                    {"Berries",ColorTranslator.FromHtml("#1e90ff")},
                    {"Clothing",ColorTranslator.FromHtml("#a5db6f")},
                    {"Collectable",ColorTranslator.FromHtml("#baadff")},
                    {"Components",ColorTranslator.FromHtml("#ba8735")},
                    {"Construction",ColorTranslator.FromHtml("#468400")},
                    {"Corpse",ColorTranslator.FromHtml("#FF0000")},
                    {"Crate",ColorTranslator.FromHtml("#015959")},
                    {"Electronics",ColorTranslator.FromHtml("#43a3a8")},
                    {"Food",ColorTranslator.FromHtml("#301fa3")},
                    {"Instruments",ColorTranslator.FromHtml("#a9d1e8")},
                    {"Items",ColorTranslator.FromHtml("#efaef2")},
                    {"Medicals",ColorTranslator.FromHtml("#ce742f")},
                    {"Minecart",ColorTranslator.FromHtml("#a4bda9")},
                    {"Misc",ColorTranslator.FromHtml("#FF8000")},
                    {"Ore",ColorTranslator.FromHtml("#52870d")},
                    {"Potions",ColorTranslator.FromHtml("#ff84b7")},
                    {"Prefabs Deployable",ColorTranslator.FromHtml("#a1f4df")},
                    {"Prefabs Misc",ColorTranslator.FromHtml("#7bedb2")},
                    {"Prefabs NPC",ColorTranslator.FromHtml("#47ffbb")},
                    {"Prefabs Resource",ColorTranslator.FromHtml("#fcc8c4")},
                    {"Prefabs Tools",ColorTranslator.FromHtml("#3397a0")},
                    {"Prefabs Weapons",ColorTranslator.FromHtml("#aa2e55")},
                    {"Prefabs Weapons Mods",ColorTranslator.FromHtml("#f25e3a")},
                    {"Resources",ColorTranslator.FromHtml("#0faf22")},
                    {"Road Sign",ColorTranslator.FromHtml("#2d8989")},
                    {"Seeds",ColorTranslator.FromHtml("#baf713")},
                    {"Tools",ColorTranslator.FromHtml("#c66ad8")},
                    {"Traps",ColorTranslator.FromHtml("#988def")},
                    {"Vehicle Parts",ColorTranslator.FromHtml("#390f8c")},
                    {"Weapons",ColorTranslator.FromHtml("#b00000")}
                };

                public Color lootPriority0;
                public Color lootPriority1;
                public Color lootPriority2;
                public Color lootPriority3;
                public Color lootPriority4;
                public Color lootPriority5;
                public Color lootPriorityQuest;
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class DebugSetting
        {
            [NonSerialized]
            public bool DebugStuff = false;
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class EntitySetting
        {
            public ElevationSetting Elevation = new ElevationSetting();
            public LineOfSightSetting LineOfSight = new LineOfSightSetting();

            public List<string> EntityTypesSuppressed = new List<string>();

            [NonSerialized]
            public bool ShowStatusChanged = true;

            public bool Bodies = true;
            public bool Distance = true;
            public bool LOS = true;
            public bool Name = true;
            public bool Weapon = true;
            public bool Animals = true;

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
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class LootSetting
        {
            public bool Show = true;
            public bool LiveLoot = true;
            public int LiveLootPerCycle = 40;

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
            public int Value = 5;
            internal bool Aggregate = false;
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class MapSetting
        {
            public bool CenterMap = true;
            public bool DrawText = true;
            public bool NetBubbleCircles = false;
            public bool HouseOwners = true;
            public int MapResolution = 2;

            public float IconSizePlayers = 0.5f;
            public float IconSizeLoot = 0.5f;
            public float IconSizeInfected = 0.5f;
            public float TextScale = 1f;
            public bool ProximityAlert = true;

            [NonSerialized]
            public bool BaseCanvasOverride = true;

            public int MapSize = 0;
            public int MapSeed = 0;

            [NonSerialized]
            public int MapCoeff = 2;
        }

        public class OverlaySetting
        {
            public int DrawDistance = 150;
            public int DrawDistanceLoot = 150;
            public WindowStyleEnum WindowStyle = WindowStyleEnum.FullScreen;
            public Size GameResolution = new Size(1920, 1080);
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