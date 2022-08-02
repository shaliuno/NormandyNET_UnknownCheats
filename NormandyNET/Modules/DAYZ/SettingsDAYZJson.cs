using System;
using System.Collections.Generic;
using System.Drawing;

namespace NormandyNET.Settings
{
    [Serializable]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class SettingsDAYZJson
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

        public SettingsDAYZJson()
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
            public bool NoGrass = false;
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
                      { "Animal" , ColorTranslator.FromHtml("Green") },
                      { "Infected" , ColorTranslator.FromHtml("#FF8000") },
                      { "Player" , ColorTranslator.FromHtml("DodgerBlue") },
                      { "You" , ColorTranslator.FromHtml("#80FF00") },
                      { "Vehicle" , ColorTranslator.FromHtml("#b00000") },
                      { "Car_Wreck" , Color.FromArgb(196,196,196) },
                      { "Helicrash" , ColorTranslator.FromHtml("Maroon") },
                      { "Unknown" , ColorTranslator.FromHtml("White") },
                  };
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public class LootColorSetting
            {
                [NonSerialized]
                public bool ColorsChanged = true;

                public Dictionary<string, Color> LootCategoryColors = new Dictionary<string, Color>
                {
                    { "Ammunition" , ColorTranslator.FromHtml("#a5db6f") },
                    { "Armband" , ColorTranslator.FromHtml("#baadff") },
                    { "Attachments" , ColorTranslator.FromHtml("#ba8735") },
                    { "Backpacks" , ColorTranslator.FromHtml("#468400") },
                    { "Belts" , ColorTranslator.FromHtml("#044268") },
                    { "Communication" , ColorTranslator.FromHtml("#3856aa") },
                    { "Construction" , ColorTranslator.FromHtml("#edabf2") },
                    { "Cooking" , ColorTranslator.FromHtml("#015959") },
                    { "Crafting" , ColorTranslator.FromHtml("#43a3a8") },
                    { "Electricity" , ColorTranslator.FromHtml("#301fa3") },
                    { "Equipment" , ColorTranslator.FromHtml("#bfe4ff") },
                    { "Eyewear" , ColorTranslator.FromHtml("#a9d1e8") },
                    { "Fishing" , ColorTranslator.FromHtml("#efaef2") },
                    { "Food" , ColorTranslator.FromHtml("#a3d86e") },
                    { "Gloves" , ColorTranslator.FromHtml("#FF8000") },
                    { "Grenades" , ColorTranslator.FromHtml("#9db9dd") },
                    { "Ghillie" , ColorTranslator.FromHtml("#a4bda9") },
                    { "Headwear" , ColorTranslator.FromHtml("#f9bdcf") },
                    { "Holsters" , ColorTranslator.FromHtml("#52870d") },
                    { "Large Storage" , ColorTranslator.FromHtml("#0e8e4c") },
                    { "Light Sources" , ColorTranslator.FromHtml("#2d18a3") },
                    { "Liquid Containers" , ColorTranslator.FromHtml("#a1f4df") },
                    { "Magazine" , ColorTranslator.FromHtml("#4672c4") },
                    { "Traps" , ColorTranslator.FromHtml("#a1c438") },
                    { "Masks" , ColorTranslator.FromHtml("#1e1d7f") },
                    { "Medical" , ColorTranslator.FromHtml("#38f7c4") },
                    { "Melee Weapons" , ColorTranslator.FromHtml("#7bedb2") },
                    { "Miscellaneous" , ColorTranslator.FromHtml("#c6fc9c") },
                    { "Navigation" , ColorTranslator.FromHtml("#47ffbb") },
                    { "Optics" , ColorTranslator.FromHtml("#fcc8c4") },
                    { "Pants" , ColorTranslator.FromHtml("#3397a0") },
                    { "Protective Gear" , ColorTranslator.FromHtml("#aa2e55") },
                    { "Repair Kits" , ColorTranslator.FromHtml("#f25e3a") },
                    { "Seeds" , ColorTranslator.FromHtml("#9af9bc") },
                    { "Shirts" , ColorTranslator.FromHtml("#0faf22") },
                    { "Shoes" , ColorTranslator.FromHtml("#2d8989") },
                    { "Small Storage" , ColorTranslator.FromHtml("#baf713") },
                    { "Camping" , ColorTranslator.FromHtml("#c66ad8") },
                    { "Tools" , ColorTranslator.FromHtml("#988def") },
                    { "Vehicle Parts" , ColorTranslator.FromHtml("#390f8c") },
                    { "Vests" , ColorTranslator.FromHtml("#b00000") },
                    { "Weapons" , ColorTranslator.FromHtml("#efd25d") },
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
            public int MapResolution = 2;

            public float IconSizePlayers = 0.5f;
            public float IconSizeLoot = 0.5f;
            public float IconSizeInfected = 0.5f;
            public float TextScale = 1f;
            public bool ProximityAlert = true;
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