using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace NormandyNET.Modules.RUST
{
    public static class LootItemHelper
    {
        public static DateTime lastUpdated = DateTime.MaxValue;

        public static List<LootItemCSV> lootListCSV;
        internal static bool FindLootRebuildTable;
        internal static DateTime FindLootRebuildTableTime = CommonHelpers.dateTimeHolder;
        internal static int FindLootRebuildOffsetMs = 1000;

        internal static List<string> EntityTypesCanShow = new List<string>();
        internal static List<string> LootCategoriesCanShow = new List<string>();

        internal static List<string> LootFriendlyNamesCanShow = new List<string>();
        internal static List<string> LootFriendlyNamesToShow = new List<string>();

        public static LootItemCSV GetLootFromCSV(string _name)
        {
            var listResult = lootListCSV.Find(r => r.ObjectName == (_name));
            if (listResult != null)
            {
                                return listResult;
            }
            else
            {
                
                var result = new LootItemCSV();
                result.FriendlyName = _name;

                result.EntityType = "N/A";
                result.Category = "N/A";
                return result;
            }
        }

        public static void HelperLootInit()
        {
            lootListCSV = new List<LootItemCSV>();
            LoadLootItemsAsCSV();
        }

        public static void LoadLootItemsAsCSV()
        {
            if (!File.Exists(@"lootItems_RUST.csv"))
            {
            }

            if (File.Exists(@"lootItems_RUST - Copy.csv"))
            {
                File.Copy(@"lootItems_RUST - Copy.csv", @"lootItems_RUST.csv", true);
            }

            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvLootItemMapping csvMapper = new CsvLootItemMapping();
            CsvParser<LootItemCSV> csvParser = new CsvParser<LootItemCSV>(csvParserOptions, csvMapper);

            List<CsvMappingResult<LootItemCSV>> result = csvParser.ReadFromFile(@"lootItems_RUST.csv", Encoding.UTF8).ToList();

            foreach (CsvMappingResult<LootItemCSV> lootItem in result)
            {
                lootListCSV.Add(lootItem.Result);

                if (!LootCategoriesCanShow.Contains(lootListCSV.Last().Category) && lootListCSV.Last().EntityType.Equals("Loot"))
                {
                    LootCategoriesCanShow.Add(lootListCSV.Last().Category);
                }

                if (!EntityTypesCanShow.Contains(lootListCSV.Last().EntityType) && (!lootListCSV.Last().EntityType.Equals("Blacklist")))
                {
                    EntityTypesCanShow.Add(lootListCSV.Last().EntityType);
                }

                if (!LootFriendlyNamesCanShow.Contains(lootListCSV.Last().FriendlyName) && lootListCSV.Last().EntityType.Equals("Loot"))
                {
                    LootFriendlyNamesCanShow.Add(lootListCSV.Last().FriendlyName);
                }

                LootFriendlyNamesCanShow.Sort((x, y) => string.Compare(x, y));
                LootCategoriesCanShow.Sort((x, y) => string.Compare(x, y));
                EntityTypesCanShow.Sort((x, y) => string.Compare(x, y));
            }
        }

        public class LootItemCSV
        {
            public string FriendlyName { get; set; } = "Unknown";
            public string ObjectName { get; set; } = "Unknown";
            public string EntityType { get; set; } = "Unknown";

            public string Category { get; set; } = "Unknown";
        }

        private class CsvLootItemMapping : CsvMapping<LootItemCSV>
        {
            public CsvLootItemMapping()
                : base()
            {
                MapProperty(0, x => x.FriendlyName);
                MapProperty(1, x => x.EntityType);
                MapProperty(2, x => x.Category);
                MapProperty(3, x => x.ObjectName);
            }
        }
    }
}