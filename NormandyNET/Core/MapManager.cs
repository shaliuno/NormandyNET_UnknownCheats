using NormandyNET.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NormandyNET.Core
{
    public class MapManager
    {
        public Dictionary<string, MapObject> mapObjects = new Dictionary<string, MapObject>();
        public static string dataFolder = "Data";

        public static readonly Dictionary<string, string> exfilToMap = new Dictionary<string, string>
        {
            { "Dorms V-Ex", "Customs" },
            { "ZB-1012", "Customs" },
            { "ZB-1011", "Customs" },
            { "Crossroads", "Customs" },
            { "Old Gas Station", "Customs" },
            { "Trailer Park", "Customs" },
            { "RUAF Roadblock", "Customs" },
            { "Smuggler's Boat", "Customs" },

            { "Cellars", "Factory" },
            { "Gate 3", "Factory" },
            { "Gate 0", "Factory" },

            { "SE Exfil", "Interchange" },
            { "NW Exfil", "Interchange" },
            { "PP Exfil", "Interchange" },
            { "Interchange Cooperation", "Interchange" },
            { "Hole Exfill", "Interchange" },
            { "Saferoom Exfil", "Interchange" },

            { "lab_Elevator_Cargo", "Laboratory" },
            { "lab_Elevator_Main", "Laboratory" },
            { "lab_Vent", "Laboratory" },
            { "lab_Elevator_Med", "Laboratory" },
            { "lab_Under_Storage_Collector", "Laboratory" },
            { "lab_Parking_Gate", "Laboratory" },
            { "lab_Hangar_Gate", "Laboratory" },

            { "EXFIL_Bunker", "Reserve" },

            { "EXFIL_ScavCooperation", "Reserve" },
            { "EXFIL_vent", "Reserve" },

            { "Nothern_Checkpoint", "Lighthouse" },
            { "Coastal_South_Road", "Lighthouse" },

            { "Pier Boat", "Shoreline" },
            { "Road to Customs", "Shoreline" },
            { "CCP Temporary", "Shoreline" },
            { "Tunnel", "Shoreline" },
            { "Rock Passage", "Shoreline" },

            { "Factory Gate", "Woods" },

            { "RUAF Gate", "Woods" },
            { "ZB-016", "Woods" },
            { "ZB-014", "Woods" },
            { "UN Roadblock", "Woods" },
            { "South V-Ex", "Woods" },
            { "Outskirts", "Woods" }
        };

        public static readonly Dictionary<string, string> pubgToMap = new Dictionary<string, string>
        {
            {"Desert_Main_C","Miramar"},
            {"DihorOtok_Main_C","Vikendi"},
            {"Erangel_Main_C","Erangel"},
            {"Heaven_Main_C","Haven"},
            {"Range_Main_C","Training"},
            {"Savage_Main_C","Sanhok"},
            {"Summerland_Main_C","Karakin"}
        };

        public readonly List<string> resolutions = new List<string>()
        {
            @"1K",
            @"4K",
            @"8K",
            @"Alternative"
        };

        public MapManager.MapLevels DesiredMapLevel;
        public MapManager.MapLevels CurrentMapLevel;
        public string CurrentMap = string.Empty;
        public string CurrentMapSwitchTo = string.Empty;
        public float testLevel;
        public string resolutionFolder = @"8K";
        public bool reloadMaps;
        public bool suppressMapSelection = false;

        public MapManager()
        {
            GenerateMapObjects();
        }

        public void GenerateMapObjects()
        {
            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.eft)
            {
                dataFolder = "Data_EFT";
                suppressMapSelection = true;

                var customsMap = new MapObject();
                customsMap.levelsRange = new int[] { 1 };
                customsMap.checkElevation = false;
                customsMap.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Customs", "Ground"));
                customsMap.defaultLevel = MapLevels.Ground;
                customsMap.invertMap = 1;
                customsMap.elevationLevelsRange = new float[] { -100f };
                customsMap.CanvasSizeBase = 517f;
                customsMap.offsetForObjectsX = -178f;
                customsMap.offsetForObjectsY = -4f;
                mapObjects.Add("Customs", customsMap);

                var factoryMap = new MapObject();
                factoryMap.levelsRange = new int[] { 1 };
                factoryMap.checkElevation = false;
                factoryMap.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Factory", "Ground"));
                factoryMap.defaultLevel = MapLevels.Ground;
                factoryMap.invertMap = 1;
                factoryMap.elevationLevelsRange = new float[] { -100f };
                factoryMap.CanvasSizeBase = 104.4f;
                factoryMap.zoomStep = 16;
                mapObjects.Add("Factory", factoryMap);

                var interchangeMap = new MapObject();
                interchangeMap.levelsRange = new int[] { 0, 2, 3 };
                interchangeMap.checkElevation = false;
                interchangeMap.mapFileBytes.Add(MapLevels.Basement, GetMapByFileNameWildCard("Interchange", "Basement"));
                interchangeMap.mapFileBytes.Add(MapLevels.FirstFloor, GetMapByFileNameWildCard("Interchange", "Floor1"));
                interchangeMap.mapFileBytes.Add(MapLevels.SecondFloor, GetMapByFileNameWildCard("Interchange", "Floor2"));
                interchangeMap.defaultLevel = MapLevels.Basement;
                interchangeMap.invertMap = -1;
                interchangeMap.elevationLevelsRange = new float[] { -100f, 25f, 35f };
                interchangeMap.CanvasSizeBase = 482f;
                interchangeMap.offsetForObjectsX = 65f;
                interchangeMap.offsetForObjectsY = 2f;
                interchangeMap.zoomStep = 96;
                interchangeMap.ZoomLevel = 2;
                mapObjects.Add("Interchange", interchangeMap);

                var laboratoryMap = new MapObject();
                laboratoryMap.levelsRange = new int[] { 0, 2, 3 };
                laboratoryMap.checkElevation = false;
                laboratoryMap.mapFileBytes.Add(MapLevels.Basement, GetMapByFileNameWildCard("Laboratory", "Basement"));
                laboratoryMap.mapFileBytes.Add(MapLevels.FirstFloor, GetMapByFileNameWildCard("Laboratory", "Level1"));
                laboratoryMap.mapFileBytes.Add(MapLevels.SecondFloor, GetMapByFileNameWildCard("Laboratory", "Level2"));
                laboratoryMap.defaultLevel = MapLevels.FirstFloor;
                laboratoryMap.invertMap = 1;
                laboratoryMap.CanvasSizeBase = 149f;
                laboratoryMap.offsetForObjectsX = 190f;
                laboratoryMap.offsetForObjectsY = 337f;
                laboratoryMap.zoomStep = 16;

                laboratoryMap.elevationLevelsRange = new float[] { -100f, -1.0f, 3.5f };
                mapObjects.Add("Laboratory", laboratoryMap);

                var reserveMap = new MapObject();
                reserveMap.levelsRange = new int[] { 0, 1 };
                reserveMap.checkElevation = false;
                reserveMap.mapFileBytes.Add(MapLevels.Basement, GetMapByFileNameWildCard("Reserve", "Basement"));
                reserveMap.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Reserve", "Ground"));
                reserveMap.defaultLevel = MapLevels.Ground;
                reserveMap.invertMap = 1;
                reserveMap.CanvasSizeBase = 296f;
                reserveMap.offsetForObjectsX = 11f;
                reserveMap.offsetForObjectsY = 42f;
                reserveMap.zoomStep = 96;
                reserveMap.elevationLevelsRange = new float[] { -100f, -9.5f };
                mapObjects.Add("Reserve", reserveMap);

                var shorelineMap = new MapObject();
                shorelineMap.levelsRange = new int[] { 0, 2, 3, 4, 5 };
                shorelineMap.checkElevation = false;
                shorelineMap.mapFileBytes.Add(MapLevels.Basement, GetMapByFileNameWildCard("Shoreline", "Basement"));
                shorelineMap.mapFileBytes.Add(MapLevels.FirstFloor, GetMapByFileNameWildCard("Shoreline", "Floor1"));
                shorelineMap.mapFileBytes.Add(MapLevels.SecondFloor, GetMapByFileNameWildCard("Shoreline", "Floor2"));
                shorelineMap.mapFileBytes.Add(MapLevels.ThirdFloor, GetMapByFileNameWildCard("Shoreline", "Floor3"));
                shorelineMap.mapFileBytes.Add(MapLevels.Roof, GetMapByFileNameWildCard("Shoreline", "Roof"));
                shorelineMap.defaultLevel = MapLevels.Basement;
                shorelineMap.invertMap = -1;
                shorelineMap.CanvasSizeBase = 811f;
                shorelineMap.offsetForObjectsX = -187f;
                shorelineMap.offsetForObjectsY = 165f;
                shorelineMap.zoomStep = 80;
                shorelineMap.ZoomLevel = 3;
                shorelineMap.elevationLevelsRange = new float[] { -100f, -3.8f, -0.9f, 2.0f, 6.8f };
                mapObjects.Add("Shoreline", shorelineMap);

                var woodsMap = new MapObject();
                woodsMap.levelsRange = new int[] { 1 };
                woodsMap.checkElevation = false;
                woodsMap.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Woods", "Ground"));
                woodsMap.defaultLevel = MapLevels.Ground;
                woodsMap.invertMap = 1;
                woodsMap.ZoomLevel = 3;
                woodsMap.CanvasSizeBase = 702f;
                woodsMap.offsetForObjectsX = 59f;
                woodsMap.offsetForObjectsY = 241f;

                woodsMap.zoomStep = 64;
                woodsMap.elevationLevelsRange = new float[] { -100f };
                mapObjects.Add("Woods", woodsMap);

                var lighthouseMap = new MapObject();
                lighthouseMap.levelsRange = new int[] { 1 };
                lighthouseMap.checkElevation = false;
                lighthouseMap.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Lighthouse", "Ground"));
                lighthouseMap.defaultLevel = MapLevels.Ground;
                lighthouseMap.invertMap = 1;
                lighthouseMap.ZoomLevel = 3;
                lighthouseMap.CanvasSizeBase = 811f;
                lighthouseMap.offsetForObjectsX = 0f;
                lighthouseMap.offsetForObjectsY = 217f;
                lighthouseMap.zoomStep = 80;
                lighthouseMap.elevationLevelsRange = new float[] { -100f };

                mapObjects.Add("Lighthouse", lighthouseMap);
            }

            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.dayz)
            {
                dataFolder = "Data_DAYZ";

                var banovSat = new MapObject();
                banovSat.levelsRange = new int[] { 1 };
                banovSat.checkElevation = false;
                banovSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Banov-Sat", "Ground"));
                banovSat.defaultLevel = MapLevels.Ground;
                banovSat.invertMap = 1;
                banovSat.CanvasSizeBase = 15360 / 2;
                banovSat.zoomStep = 256;
                banovSat.ZoomLevel = 5;
                banovSat.offsetForObjectsX = banovSat.CanvasSizeBase * -1;
                banovSat.offsetForObjectsY = banovSat.CanvasSizeBase * -1;

                mapObjects.Add("Banov Satellite", banovSat);

                var banovTop = new MapObject();
                banovTop.levelsRange = new int[] { 1 };
                banovTop.checkElevation = false;
                banovTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Banov-Top", "Ground"));
                banovTop.defaultLevel = MapLevels.Ground;
                banovTop.invertMap = 1;
                banovTop.CanvasSizeBase = 15360 / 2;
                banovTop.zoomStep = 256;
                banovTop.ZoomLevel = 5;
                banovTop.offsetForObjectsX = banovTop.CanvasSizeBase * -1;
                banovTop.offsetForObjectsY = banovTop.CanvasSizeBase * -1;

                mapObjects.Add("Banov Topographic", banovTop);

                var chernarusPlusSat = new MapObject();
                chernarusPlusSat.levelsRange = new int[] { 1 };
                chernarusPlusSat.checkElevation = false;
                chernarusPlusSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("ChernarusPlus-Sat", "Ground"));
                chernarusPlusSat.defaultLevel = MapLevels.Ground;
                chernarusPlusSat.invertMap = 1;
                chernarusPlusSat.CanvasSizeBase = 15360 / 2;
                chernarusPlusSat.zoomStep = 256;
                chernarusPlusSat.ZoomLevel = 5;
                chernarusPlusSat.offsetForObjectsX = chernarusPlusSat.CanvasSizeBase * -1;
                chernarusPlusSat.offsetForObjectsY = chernarusPlusSat.CanvasSizeBase * -1;

                mapObjects.Add("ChernarusPlus Satellite", chernarusPlusSat);

                var chernarusPlusTop = new MapObject();
                chernarusPlusTop.levelsRange = new int[] { 1 };
                chernarusPlusTop.checkElevation = false;
                chernarusPlusTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("ChernarusPlus-Top", "Ground"));
                chernarusPlusTop.defaultLevel = MapLevels.Ground;
                chernarusPlusTop.invertMap = 1;
                chernarusPlusTop.CanvasSizeBase = 15360 / 2;
                chernarusPlusTop.zoomStep = 256;
                chernarusPlusTop.ZoomLevel = 5;
                chernarusPlusTop.offsetForObjectsX = chernarusPlusTop.CanvasSizeBase * -1;
                chernarusPlusTop.offsetForObjectsY = chernarusPlusTop.CanvasSizeBase * -1;

                mapObjects.Add("ChernarusPlus Topographic", chernarusPlusTop);

                var livoniaSat = new MapObject();
                livoniaSat.levelsRange = new int[] { 1 };
                livoniaSat.checkElevation = false;
                livoniaSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Livonia-Sat", "Ground"));
                livoniaSat.defaultLevel = MapLevels.Ground;
                livoniaSat.invertMap = 1;
                livoniaSat.CanvasSizeBase = 12800 / 2;
                livoniaSat.zoomStep = 256;
                livoniaSat.ZoomLevel = 5;
                livoniaSat.offsetForObjectsX = livoniaSat.CanvasSizeBase * -1;
                livoniaSat.offsetForObjectsY = livoniaSat.CanvasSizeBase * -1;

                mapObjects.Add("Livonia Satellite", livoniaSat);

                var livoniaTop = new MapObject();
                livoniaTop.levelsRange = new int[] { 1 };
                livoniaTop.checkElevation = false;
                livoniaTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Livonia-Top", "Ground"));
                livoniaTop.defaultLevel = MapLevels.Ground;
                livoniaTop.invertMap = 1;
                livoniaTop.CanvasSizeBase = 12800 / 2;
                livoniaTop.zoomStep = 256;
                livoniaTop.ZoomLevel = 5;
                livoniaTop.offsetForObjectsX = livoniaTop.CanvasSizeBase * -1;
                livoniaTop.offsetForObjectsY = livoniaTop.CanvasSizeBase * -1;

                mapObjects.Add("Livonia Topographic", livoniaTop);

                var namalskTop = new MapObject();
                namalskTop.levelsRange = new int[] { 1 };
                namalskTop.checkElevation = false;
                namalskTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Namalsk-Top", "Ground"));
                namalskTop.defaultLevel = MapLevels.Ground;
                namalskTop.invertMap = 1;
                namalskTop.CanvasSizeBase = 12800 / 2;
                namalskTop.zoomStep = 256;
                namalskTop.ZoomLevel = 5;
                namalskTop.offsetForObjectsX = namalskTop.CanvasSizeBase * -1;
                namalskTop.offsetForObjectsY = namalskTop.CanvasSizeBase * -1;

                mapObjects.Add("Namalsk Topographic", namalskTop);

                var namalskSat = new MapObject();
                namalskSat.levelsRange = new int[] { 1 };
                namalskSat.checkElevation = false;
                namalskSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Namalsk-Sat", "Ground"));
                namalskSat.defaultLevel = MapLevels.Ground;
                namalskSat.invertMap = 1;
                namalskSat.CanvasSizeBase = 12800 / 2;
                namalskSat.zoomStep = 256;
                namalskSat.ZoomLevel = 5;
                namalskSat.offsetForObjectsX = namalskSat.CanvasSizeBase * -1;
                namalskSat.offsetForObjectsY = namalskSat.CanvasSizeBase * -1;

                mapObjects.Add("Namalsk Satellite", namalskSat);

                var essekerTop = new MapObject();
                essekerTop.levelsRange = new int[] { 1 };
                essekerTop.checkElevation = false;
                essekerTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Esseker-Top", "Ground"));
                essekerTop.defaultLevel = MapLevels.Ground;
                essekerTop.invertMap = 1;
                essekerTop.CanvasSizeBase = 12800 / 2;
                essekerTop.zoomStep = 256;
                essekerTop.ZoomLevel = 5;
                essekerTop.offsetForObjectsX = essekerTop.CanvasSizeBase * -1;
                essekerTop.offsetForObjectsY = essekerTop.CanvasSizeBase * -1;
                mapObjects.Add("Esseker Topographic", essekerTop);

                var essekerSat = new MapObject();
                essekerSat.levelsRange = new int[] { 1 };
                essekerSat.checkElevation = false;
                essekerSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Esseker-Sat", "Ground"));
                essekerSat.defaultLevel = MapLevels.Ground;
                essekerSat.invertMap = 1;
                essekerSat.CanvasSizeBase = 12800 / 2;
                essekerSat.zoomStep = 256;
                essekerSat.ZoomLevel = 5;
                essekerSat.offsetForObjectsX = essekerSat.CanvasSizeBase * -1;
                essekerSat.offsetForObjectsY = essekerSat.CanvasSizeBase * -1;
                mapObjects.Add("Esseker Satellite", essekerSat);

                var deerIsleTop = new MapObject();
                deerIsleTop.levelsRange = new int[] { 1 };
                deerIsleTop.checkElevation = false;
                deerIsleTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("DeerIsle-Top", "Ground"));
                deerIsleTop.defaultLevel = MapLevels.Ground;
                deerIsleTop.invertMap = 1;
                deerIsleTop.CanvasSizeBase = 16375 / 2;
                deerIsleTop.zoomStep = 256;
                deerIsleTop.ZoomLevel = 5;
                deerIsleTop.offsetForObjectsX = deerIsleTop.CanvasSizeBase * -1;
                deerIsleTop.offsetForObjectsY = deerIsleTop.CanvasSizeBase * -1;
                mapObjects.Add("DeerIsle Topographic", deerIsleTop);

                var deerIsleSat = new MapObject();
                deerIsleSat.levelsRange = new int[] { 1 };
                deerIsleSat.checkElevation = false;
                deerIsleSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("DeerIsle-Sat", "Ground"));
                deerIsleSat.defaultLevel = MapLevels.Ground;
                deerIsleSat.invertMap = 1;
                deerIsleSat.CanvasSizeBase = 16375 / 2;
                deerIsleSat.zoomStep = 256;
                deerIsleSat.ZoomLevel = 5;
                deerIsleSat.offsetForObjectsX = deerIsleSat.CanvasSizeBase * -1;
                deerIsleSat.offsetForObjectsY = deerIsleSat.CanvasSizeBase * -1;
                mapObjects.Add("DeerIsle Satellite", deerIsleSat);

                var chiemseeTop = new MapObject();
                chiemseeTop.levelsRange = new int[] { 1 };
                chiemseeTop.checkElevation = false;
                chiemseeTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Chiemsee-Top", "Ground"));
                chiemseeTop.defaultLevel = MapLevels.Ground;
                chiemseeTop.invertMap = 1;
                chiemseeTop.CanvasSizeBase = 10240 / 2;
                chiemseeTop.zoomStep = 160;
                chiemseeTop.ZoomLevel = 5;
                chiemseeTop.offsetForObjectsX = chiemseeTop.CanvasSizeBase * -1;
                chiemseeTop.offsetForObjectsY = chiemseeTop.CanvasSizeBase * -1;
                mapObjects.Add("Chiemsee Topographic", chiemseeTop);

                var chiemseeSat = new MapObject();
                chiemseeSat.levelsRange = new int[] { 1 };
                chiemseeSat.checkElevation = false;
                chiemseeSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Chiemsee-Sat", "Ground"));
                chiemseeSat.defaultLevel = MapLevels.Ground;
                chiemseeSat.invertMap = 1;
                chiemseeSat.CanvasSizeBase = 10240 / 2;
                chiemseeSat.zoomStep = 160;
                chiemseeSat.ZoomLevel = 5;
                chiemseeSat.offsetForObjectsX = chiemseeSat.CanvasSizeBase * -1;
                chiemseeSat.offsetForObjectsY = chiemseeSat.CanvasSizeBase * -1;
                mapObjects.Add("Chiemsee Satellite", chiemseeSat);

                var rostowTop = new MapObject();
                rostowTop.levelsRange = new int[] { 1 };
                rostowTop.checkElevation = false;
                rostowTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Rostow-Top", "Ground"));
                rostowTop.defaultLevel = MapLevels.Ground;
                rostowTop.invertMap = 1;
                rostowTop.CanvasSizeBase = 14336 / 2;
                rostowTop.zoomStep = 256;
                rostowTop.ZoomLevel = 5;
                rostowTop.offsetForObjectsX = rostowTop.CanvasSizeBase * -1;
                rostowTop.offsetForObjectsY = rostowTop.CanvasSizeBase * -1;
                mapObjects.Add("Rostow Topographic", rostowTop);

                var rostowSat = new MapObject();
                rostowSat.levelsRange = new int[] { 1 };
                rostowSat.checkElevation = false;
                rostowSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Rostow-Sat", "Ground"));
                rostowSat.defaultLevel = MapLevels.Ground;
                rostowSat.invertMap = 1;
                rostowSat.CanvasSizeBase = 14336 / 2;
                rostowSat.zoomStep = 256;
                rostowSat.ZoomLevel = 5;
                rostowSat.offsetForObjectsX = rostowSat.CanvasSizeBase * -1;
                rostowSat.offsetForObjectsY = rostowSat.CanvasSizeBase * -1;
                mapObjects.Add("Rostow Satellite", rostowSat);

                var mapTakistan = new MapObject();
                mapTakistan.levelsRange = new int[] { 1 };
                mapTakistan.checkElevation = false;
                mapTakistan.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Takistan", "Ground"));
                mapTakistan.defaultLevel = MapLevels.Ground;
                mapTakistan.invertMap = 1;
                mapTakistan.CanvasSizeBase = 12800 / 2;
                mapTakistan.zoomStep = 416;
                mapTakistan.ZoomLevel = 4;
                mapTakistan.offsetForObjectsX = mapTakistan.CanvasSizeBase * -1;
                mapTakistan.offsetForObjectsY = mapTakistan.CanvasSizeBase * -1;
                mapObjects.Add("Takistan", mapTakistan);

                var mapSwansIsland = new MapObject();
                mapSwansIsland.levelsRange = new int[] { 1 };
                mapSwansIsland.checkElevation = false;
                mapSwansIsland.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("SwansIsland", "Ground"));
                mapSwansIsland.defaultLevel = MapLevels.Ground;
                mapSwansIsland.invertMap = 1;
                mapSwansIsland.CanvasSizeBase = 2048 / 2;
                mapSwansIsland.zoomStep = 416;
                mapSwansIsland.ZoomLevel = 4;
                mapSwansIsland.offsetForObjectsX = mapSwansIsland.CanvasSizeBase * -1;
                mapSwansIsland.offsetForObjectsY = mapSwansIsland.CanvasSizeBase * -1;
                mapObjects.Add("SwansIsland", mapSwansIsland);

                var pripyatTop = new MapObject();
                pripyatTop.levelsRange = new int[] { 1 };
                pripyatTop.checkElevation = false;
                pripyatTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Pripyat-Top", "Ground"));
                pripyatTop.defaultLevel = MapLevels.Ground;
                pripyatTop.invertMap = 1;
                pripyatTop.CanvasSizeBase = 20480 / 2;
                pripyatTop.zoomStep = 256;
                pripyatTop.ZoomLevel = 5;
                pripyatTop.offsetForObjectsX = pripyatTop.CanvasSizeBase * -1;
                pripyatTop.offsetForObjectsY = pripyatTop.CanvasSizeBase * -1;
                mapObjects.Add("Pripyat Topographic", pripyatTop);

                var pripyatSat = new MapObject();
                pripyatSat.levelsRange = new int[] { 1 };
                pripyatSat.checkElevation = false;
                pripyatSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Pripyat-Sat", "Ground"));
                pripyatSat.defaultLevel = MapLevels.Ground;
                pripyatSat.invertMap = 1;
                pripyatSat.CanvasSizeBase = 20480 / 2;
                pripyatSat.zoomStep = 256;
                pripyatSat.ZoomLevel = 5;
                pripyatSat.offsetForObjectsX = pripyatSat.CanvasSizeBase * -1;
                pripyatSat.offsetForObjectsY = pripyatSat.CanvasSizeBase * -1;
                mapObjects.Add("Pripyat Satellite", pripyatSat);

                var iztekTop = new MapObject();
                iztekTop.levelsRange = new int[] { 1 };
                iztekTop.checkElevation = false;
                iztekTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Iztek-Top", "Ground"));
                iztekTop.defaultLevel = MapLevels.Ground;
                iztekTop.invertMap = 1;
                iztekTop.CanvasSizeBase = 8192 / 2;
                iztekTop.zoomStep = 256;
                iztekTop.ZoomLevel = 5;
                iztekTop.offsetForObjectsX = iztekTop.CanvasSizeBase * -1;
                iztekTop.offsetForObjectsY = iztekTop.CanvasSizeBase * -1;
                mapObjects.Add("Iztek Topographic", iztekTop);

                var iztekSat = new MapObject();
                iztekSat.levelsRange = new int[] { 1 };
                iztekSat.checkElevation = false;
                iztekSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Iztek-Sat", "Ground"));
                iztekSat.defaultLevel = MapLevels.Ground;
                iztekSat.invertMap = 1;
                iztekSat.CanvasSizeBase = 8192 / 2;
                iztekSat.zoomStep = 256;
                iztekSat.ZoomLevel = 5;
                iztekSat.offsetForObjectsX = iztekSat.CanvasSizeBase * -1;
                iztekSat.offsetForObjectsY = iztekSat.CanvasSizeBase * -1;
                mapObjects.Add("Iztek Satellite", iztekSat);

                var valningTop = new MapObject();
                valningTop.levelsRange = new int[] { 1 };
                valningTop.checkElevation = false;
                valningTop.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Valning-Top", "Ground"));
                valningTop.defaultLevel = MapLevels.Ground;
                valningTop.invertMap = 1;
                valningTop.CanvasSizeBase = 10240 / 2;
                valningTop.zoomStep = 256;
                valningTop.ZoomLevel = 5;
                valningTop.offsetForObjectsX = valningTop.CanvasSizeBase * -1;
                valningTop.offsetForObjectsY = valningTop.CanvasSizeBase * -1;
                mapObjects.Add("Valning Topographic", valningTop);

                var valningSat = new MapObject();
                valningSat.levelsRange = new int[] { 1 };
                valningSat.checkElevation = false;
                valningSat.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Valning-Sat", "Ground"));
                valningSat.defaultLevel = MapLevels.Ground;
                valningSat.invertMap = 1;
                valningSat.CanvasSizeBase = 10240 / 2;
                valningSat.zoomStep = 256;
                valningSat.ZoomLevel = 5;
                valningSat.offsetForObjectsX = valningSat.CanvasSizeBase * -1;
                valningSat.offsetForObjectsY = valningSat.CanvasSizeBase * -1;
                mapObjects.Add("Valning Satellite", valningSat);
            }

            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.pubg)
            {
                dataFolder = "Data_PUBG";
                suppressMapSelection = true;

                var erangel = new MapObject();
                erangel.levelsRange = new int[] { 1 };
                erangel.checkElevation = false;
                erangel.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Erangel", "Ground"));
                erangel.defaultLevel = MapLevels.Ground;
                erangel.invertMap = -1;
                erangel.CanvasSizeBase = 7940 / 2;
                erangel.zoomStep = 132;
                erangel.ZoomLevel = 5;
                erangel.unitSize = 100;
                erangel.offsetForObjectsX = -3967;
                erangel.offsetForObjectsY = 3970;
                mapObjects.Add("Erangel", erangel);

                var haven = new MapObject();
                haven.levelsRange = new int[] { 1 };
                haven.checkElevation = false;
                haven.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Haven", "Ground"));
                haven.defaultLevel = MapLevels.Ground;
                haven.invertMap = -1;
                haven.CanvasSizeBase = 510;
                haven.zoomStep = 33;
                haven.ZoomLevel = 5;
                haven.unitSize = 100;
                haven.offsetForObjectsX = -510;
                haven.offsetForObjectsY = 510;
                mapObjects.Add("Haven", haven);

                var karakin = new MapObject();
                karakin.levelsRange = new int[] { 1 };
                karakin.checkElevation = false;
                karakin.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Karakin", "Ground"));
                karakin.defaultLevel = MapLevels.Ground;
                karakin.invertMap = -1;
                karakin.CanvasSizeBase = 2040 / 2;
                karakin.zoomStep = 34;
                karakin.ZoomLevel = 5;
                karakin.unitSize = 100;
                karakin.offsetForObjectsX = -karakin.CanvasSizeBase;
                karakin.offsetForObjectsY = karakin.CanvasSizeBase;
                mapObjects.Add("Karakin", karakin);

                var miramar = new MapObject();
                miramar.levelsRange = new int[] { 1 };
                miramar.checkElevation = false;
                miramar.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Miramar", "Ground"));
                miramar.defaultLevel = MapLevels.Ground;
                miramar.invertMap = -1;
                miramar.CanvasSizeBase = 7936 / 2;
                miramar.zoomStep = 132;
                miramar.ZoomLevel = 5;
                miramar.unitSize = 100;
                miramar.offsetForObjectsX = -miramar.CanvasSizeBase;
                miramar.offsetForObjectsY = miramar.CanvasSizeBase;
                mapObjects.Add("Miramar", miramar);

                var sanhok = new MapObject();
                sanhok.levelsRange = new int[] { 1 };
                sanhok.checkElevation = false;
                sanhok.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Sanhok", "Ground"));
                sanhok.defaultLevel = MapLevels.Ground;
                sanhok.invertMap = -1;
                sanhok.CanvasSizeBase = 3952 / 2;
                sanhok.zoomStep = 67;
                sanhok.ZoomLevel = 5;
                sanhok.unitSize = 100;
                sanhok.offsetForObjectsX = -sanhok.CanvasSizeBase;
                sanhok.offsetForObjectsY = sanhok.CanvasSizeBase;
                mapObjects.Add("Sanhok", sanhok);

                var training = new MapObject();
                training.levelsRange = new int[] { 1 };
                training.checkElevation = false;
                training.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Training", "Ground"));
                training.defaultLevel = MapLevels.Ground;
                training.invertMap = -1;
                training.CanvasSizeBase = 1024 - 10;
                training.zoomStep = 33;
                training.ZoomLevel = 5;
                training.unitSize = 100;
                training.offsetForObjectsX = 454;
                training.offsetForObjectsY = -393;
                mapObjects.Add("Training", training);

                var vikendi = new MapObject();
                vikendi.levelsRange = new int[] { 1 };
                vikendi.checkElevation = false;
                vikendi.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Vikendi", "Ground"));
                vikendi.defaultLevel = MapLevels.Ground;
                vikendi.invertMap = -1;
                vikendi.CanvasSizeBase = 5930 / 2;
                vikendi.zoomStep = 98;
                vikendi.ZoomLevel = 5;
                vikendi.unitSize = 100;
                vikendi.offsetForObjectsX = -vikendi.CanvasSizeBase;
                vikendi.offsetForObjectsY = vikendi.CanvasSizeBase;

                mapObjects.Add("Vikendi", vikendi);
            }

            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.rust)
            {
                dataFolder = "Data_RUST";
                resolutionFolder = @"Alternative";
                suppressMapSelection = true;

                var defaultMap = new MapObject();
                defaultMap.levelsRange = new int[] { 1 };
                defaultMap.checkElevation = false;
                defaultMap.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("", "", ".png"));
                defaultMap.defaultLevel = MapLevels.Ground;
                defaultMap.invertMap = 1;
                defaultMap.CanvasSizeBase = 2250;
                defaultMap.zoomStep = 128;
                defaultMap.ZoomLevel = 4;
                defaultMap.offsetForObjectsX = 0;
                defaultMap.offsetForObjectsY = 0;
                mapObjects.Add("Default", defaultMap);
            }

            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.arma)
            {
                dataFolder = "Data_ARMA";

                var mapAltis = new MapObject();
                mapAltis.levelsRange = new int[] { 1 };
                mapAltis.checkElevation = false;
                mapAltis.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Altis", "Ground"));
                mapAltis.defaultLevel = MapLevels.Ground;
                mapAltis.invertMap = 1;
                mapAltis.CanvasSizeBase = 30720 / 2;
                mapAltis.zoomStep = 1024;
                mapAltis.ZoomLevel = 4;
                mapAltis.offsetForObjectsX = mapAltis.CanvasSizeBase * -1;
                mapAltis.offsetForObjectsY = mapAltis.CanvasSizeBase * -1;
                mapObjects.Add("Altis", mapAltis);

                var mapStratis = new MapObject();
                mapStratis.levelsRange = new int[] { 1 };
                mapStratis.checkElevation = false;
                mapStratis.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Stratis", "Ground"));
                mapStratis.defaultLevel = MapLevels.Ground;
                mapStratis.invertMap = 1;
                mapStratis.CanvasSizeBase = 8192 / 2;
                mapStratis.zoomStep = 320;
                mapStratis.ZoomLevel = 4;
                mapStratis.offsetForObjectsX = mapStratis.CanvasSizeBase * -1;
                mapStratis.offsetForObjectsY = mapStratis.CanvasSizeBase * -1;
                mapObjects.Add("Stratis", mapStratis);

                var mapMalden = new MapObject();
                mapMalden.levelsRange = new int[] { 1 };
                mapMalden.checkElevation = false;
                mapMalden.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Malden", "Ground"));
                mapMalden.defaultLevel = MapLevels.Ground;
                mapMalden.invertMap = 1;
                mapMalden.CanvasSizeBase = 12800 / 2;
                mapMalden.zoomStep = 416;
                mapMalden.ZoomLevel = 4;
                mapMalden.offsetForObjectsX = mapMalden.CanvasSizeBase * -1;
                mapMalden.offsetForObjectsY = mapMalden.CanvasSizeBase * -1;
                mapObjects.Add("Malden", mapMalden);

                var livonia = new MapObject();
                livonia.levelsRange = new int[] { 1 };
                livonia.checkElevation = false;
                livonia.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Livonia", "Ground"));
                livonia.defaultLevel = MapLevels.Ground;
                livonia.invertMap = 1;
                livonia.CanvasSizeBase = 12800 / 2;
                livonia.zoomStep = 256;
                livonia.ZoomLevel = 5;
                livonia.offsetForObjectsX = livonia.CanvasSizeBase * -1;
                livonia.offsetForObjectsY = livonia.CanvasSizeBase * -1;
                mapObjects.Add("Livonia", livonia);

                var mapTanoa = new MapObject();
                mapTanoa.levelsRange = new int[] { 1 };
                mapTanoa.checkElevation = false;
                mapTanoa.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Tanoa", "Ground"));
                mapTanoa.defaultLevel = MapLevels.Ground;
                mapTanoa.invertMap = 1;
                mapTanoa.CanvasSizeBase = 15360 / 2;
                mapTanoa.zoomStep = 512;
                mapTanoa.ZoomLevel = 4;
                mapTanoa.offsetForObjectsX = mapTanoa.CanvasSizeBase * -1;
                mapTanoa.offsetForObjectsY = mapTanoa.CanvasSizeBase * -1;
                mapObjects.Add("Tanoa", mapTanoa);

                var mapTaunus = new MapObject();
                mapTaunus.levelsRange = new int[] { 1 };
                mapTaunus.checkElevation = false;
                mapTaunus.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Taunus", "Ground"));
                mapTaunus.defaultLevel = MapLevels.Ground;
                mapTaunus.invertMap = 1;
                mapTaunus.CanvasSizeBase = 20480 / 2;
                mapTaunus.zoomStep = 640;
                mapTaunus.ZoomLevel = 4;
                mapTaunus.offsetForObjectsX = mapTaunus.CanvasSizeBase * -1;
                mapTaunus.offsetForObjectsY = mapTaunus.CanvasSizeBase * -1;
                mapObjects.Add("Taunus", mapTaunus);

                var mapTakistan = new MapObject();
                mapTakistan.levelsRange = new int[] { 1 };
                mapTakistan.checkElevation = false;
                mapTakistan.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Takistan", "Ground"));
                mapTakistan.defaultLevel = MapLevels.Ground;
                mapTakistan.invertMap = 1;
                mapTakistan.CanvasSizeBase = 12800 / 2;
                mapTakistan.zoomStep = 416;
                mapTakistan.ZoomLevel = 4;
                mapTakistan.offsetForObjectsX = mapTakistan.CanvasSizeBase * -1;
                mapTakistan.offsetForObjectsY = mapTakistan.CanvasSizeBase * -1;
                mapObjects.Add("Takistan", mapTakistan);

                var mapLingor = new MapObject();
                mapLingor.levelsRange = new int[] { 1 };
                mapLingor.checkElevation = false;
                mapLingor.mapFileBytes.Add(MapLevels.Ground, GetMapByFileNameWildCard("Lingor", "Ground"));
                mapLingor.defaultLevel = MapLevels.Ground;
                mapLingor.invertMap = 1;
                mapLingor.CanvasSizeBase = 10240 / 2;
                mapLingor.zoomStep = 304;
                mapLingor.ZoomLevel = 4;
                mapLingor.offsetForObjectsX = mapLingor.CanvasSizeBase * -1;
                mapLingor.offsetForObjectsY = mapLingor.CanvasSizeBase * -1;
                mapObjects.Add("Lingor", mapLingor);

                mapObjects = mapObjects.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            }
        }

        public int GetInvertMap()
        {
            if (CurrentMap.Equals(string.Empty))
            {
                return 1;
            }
            else
            {
                var result = mapObjects[CurrentMap].invertMap;
                return result;
            }
        }

        public int GetUnitSize()
        {
            if (CurrentMap.Equals(string.Empty))
            {
                return 1;
            }
            else
            {
                var result = mapObjects[CurrentMap].unitSize;
                return result;
            }
        }

        public byte[] GetMapByFileNameWildCardOld(string mapName, string mapLevel, string extension = ".jpg")
        {
            string fullFilePath = $@".\{dataFolder}\{resolutionFolder}\map_{mapName}*{mapLevel}*{extension}";

            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.rust)
            {
                fullFilePath = $@".\{dataFolder}\{resolutionFolder}\map{mapName}*{mapLevel}*{extension}";
            }

            string fullFilePathExact = $@".\{dataFolder}\{resolutionFolder}\map_{mapName}_{mapLevel}_{resolutionFolder}{extension}";

            string fileNamePattern = Path.GetFileName(fullFilePath);
            string sourceDirectory = fullFilePath.Replace(fileNamePattern, string.Empty);

            try
            {
                var foundFiles = Directory.GetFiles(sourceDirectory, fileNamePattern);

                if (foundFiles.Length > 0)
                {
                                        return File.ReadAllBytes(foundFiles[0]);
                }
                return new byte[0];
            }
            catch (Exception e)
            {
                return new byte[0];
            }
        }

        public string GetMapByFileNameWildCard(string mapName, string mapLevel, string extension = ".jpg")
        {
            string fullFilePath = $@".\{dataFolder}\{resolutionFolder}\map_{mapName}*{mapLevel}*{extension}";

            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.rust)
            {
                fullFilePath = $@".\{dataFolder}\{resolutionFolder}\map{mapName}*{mapLevel}*{extension}";
            }

            string fullFilePathExact = $@".\{dataFolder}\{resolutionFolder}\map_{mapName}_{mapLevel}_{resolutionFolder}{extension}";

            string fileNamePattern = Path.GetFileName(fullFilePath);
            string sourceDirectory = fullFilePath.Replace(fileNamePattern, string.Empty);

            try
            {
                var foundFiles = Directory.GetFiles(sourceDirectory, fileNamePattern);

                if (foundFiles.Length > 0)
                {
                                        return Path.GetFullPath(foundFiles[0]);
                }

                return $@".\{dataFolder}\map_---.jpg";
            }
            catch (Exception e)
            {
                return $@".\{dataFolder}\map_---.jpg";
            }
        }

        public enum MapLevels
        {
            Nothing = -1,
            Basement,
            Ground,
            FirstFloor,
            SecondFloor,
            ThirdFloor,
            Roof
        }

        public class MapObject
        {
            public int[] levelsRange;
            public bool checkElevation = false;

            public Dictionary<MapLevels, string> mapFileBytes = new Dictionary<MapLevels, string>();
            public int invertMap = 1;
            public float[] elevationLevelsRange;
            private DateTime lastCheckedTime = CommonHelpers.dateTimeHolder;
            private int lastCheckedRateSec = 3;
            public MapLevels defaultLevel = MapLevels.Nothing;
            public float CanvasSizeBase = 1024;

            public float offsetForObjectsX = 0;
            public float offsetForObjectsY = 0;
            public float offsetForObjectsXdebug = 0;
            public float offsetForObjectsYdebug = 0;
            public int unitSize = 1;
            public bool offsetForMapToo = true;
            public int zoomStep = 128;
            public int ZoomLevel = 1;
            public float zoomStepCoef = 0.5f;

            public byte[] GetMapBytes(MapLevels level)
            {
                var result = mapFileBytes[level];
                
                return File.ReadAllBytes(result);
            }

            public MapLevels GetMapLevelByElevation(float playerElevation)
            {
                if (CommonHelpers.dateTimeHolder > lastCheckedTime)
                {
                    lastCheckedTime = CommonHelpers.dateTimeHolder.AddSeconds(lastCheckedRateSec);
                    var resultingInt = 0;

                    for (int startInt = 0; startInt < levelsRange.Length; startInt++)
                    {
                        if (playerElevation >= elevationLevelsRange[startInt])
                        {
                            resultingInt = startInt;
                        }
                    }

                    return (MapLevels)levelsRange[resultingInt];
                }

                return MapLevels.Nothing;
            }
        }
    }
}