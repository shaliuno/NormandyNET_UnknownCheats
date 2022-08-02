using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace NormandyNET.Modules.EFT
{
    public static class LootItemHelper
    {
        public static DateTime lastUpdated = DateTime.MaxValue;

        private static string lootItemsFilePrices = "lootItems_EFT_prices.csv";
        private static string lootItemsFileCommon = "lootItems_EFT.csv";

        public static List<LootItemCSV> lootListCSV;
        public static List<LootItemWithPricesCSV> lootListWithPricesCSV;
        internal static bool FindLootRebuildTable;
        internal static DateTime FindLootRebuildTableTime = CommonHelpers.dateTimeHolder;
        internal static int FindLootRebuildOffsetMs = 1000;

        internal static List<string> EntityTypesCanShow = new List<string>();
        internal static List<string> LootCategoriesCanShow = new List<string>();
        internal static List<string> LootCategoriesToShow = new List<string>();

        internal static List<string> LootFriendlyNamesCanShow = new List<string>();
        internal static List<string> LootFriendlyNamesToShow = new List<string>();
        internal static List<string> LootShortNamesToShow = new List<string>();
        internal static List<string> LootShortNamesCanShow = new List<string>();

        public static LootItemCSV GetLootFromCSV(string templateID)
        {
            var listResult = lootListCSV.Find(r => r.TemplateId == ("_" + templateID));
            if (listResult != null)
            {
                                return listResult;
            }
            else
            {
                                var result = new LootItemCSV();
                result.TemplateId = "_" + templateID;
                result.FriendlyName = "";
                return result;
            }
        }

        public static LootItemWithPricesCSV GetLootPriceFromCSV(string templateID)
        {
            var listResult = lootListWithPricesCSV.Find(r => r.TemplateId == ("_" + templateID));
            if (listResult != null)
            {
                                return listResult;
            }
            else
            {
                                var result = new LootItemWithPricesCSV();
                result.TemplateId = "_" + templateID;
                result.FriendlyName = "";
                return result;
            }
        }

        public static void HelperLootInit()
        {
            lootListCSV = new List<LootItemCSV>();
            lootListWithPricesCSV = new List<LootItemWithPricesCSV>();
            LoadLootItemsAsCSV();
            LoadLootItemPricesFromServer();
            LoadLootItemPricesAsCSV();
        }

        private static void LoadLootItemPricesAsCSV()
        {
            if (!File.Exists($@"{lootItemsFilePrices}"))
            {
                return;
            }

            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvLootItemWithPricesMapping csvMapper = new CsvLootItemWithPricesMapping();
            CsvParser<LootItemWithPricesCSV> csvParser = new CsvParser<LootItemWithPricesCSV>(csvParserOptions, csvMapper);

            var result = csvParser
                .ReadFromFile($@"{lootItemsFilePrices}", Encoding.UTF8)
                .ToList();

            for (int i = 0; i < result.Count; i++)
            {
                lootListWithPricesCSV.Add(result[i].Result);
            }
        }

        private static void LoadLootItemPricesFromServer()
        {
            var forceDownload = false;

            if (File.Exists($@"{lootItemsFilePrices}"))
            {
                using (StreamReader reader = new StreamReader($@"{lootItemsFilePrices}"))
                {
                    var firstLine = reader.ReadLine() ?? "";

                    if (firstLine.Contains("PricePerSlot"))
                    {
                        forceDownload = true;
                    }
                }

                var lastModified = File.GetLastWriteTimeUtc($@"{lootItemsFilePrices}");
                if ((CommonHelpers.dateTimeHolder - lastModified).TotalHours > 6)
                {
                    forceDownload = true;
                }
            }
            else
            {
                forceDownload = true;
            }
        }

        public static void LoadLootItemsAsCSV()
        {
            if (!File.Exists($@"{lootItemsFileCommon}"))
            {
                File.WriteAllText($@"{lootItemsFileCommon}", Properties.Resources.lootItems_EFT);
            }

            var oldVersion = false;

            if (File.Exists($@"{lootItemsFileCommon}"))
            {
                using (StreamReader reader = new StreamReader($@"{lootItemsFileCommon}"))
                {
                    var firstLine = reader.ReadLine() ?? "";

                    if (!firstLine.Contains("ForceShow") && !firstLine.Contains("ArmorClass") && !firstLine.Contains("ShortName"))
                    {
                        oldVersion = true;
                    }
                }
            }

            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');

            List<CsvMappingResult<LootItemCSV>> result = new List<CsvMappingResult<LootItemCSV>>();

            if (oldVersion)
            {
                CsvLootItemMappingOld csvMapper = new CsvLootItemMappingOld();
                CsvParser<LootItemCSV> csvParser = new CsvParser<LootItemCSV>(csvParserOptions, csvMapper);
                result = csvParser
                    .ReadFromFile($@"{lootItemsFileCommon}", Encoding.UTF8)
                    .ToList();
            }
            else
            {
                CsvLootItemMapping csvMapper = new CsvLootItemMapping();
                CsvParser<LootItemCSV> csvParser = new CsvParser<LootItemCSV>(csvParserOptions, csvMapper);
                result = csvParser
                    .ReadFromFile($@"{lootItemsFileCommon}", Encoding.UTF8)
                    .ToList();
            }

            for (int i = 0; i < result.Count; i++)
            {
                lootListCSV.Add(result[i].Result);

                if (!LootCategoriesCanShow.Contains(lootListCSV.Last().Category))
                {
                    LootCategoriesCanShow.Add(lootListCSV.Last().Category);
                }

                if (!LootFriendlyNamesCanShow.Contains(lootListCSV.Last().FriendlyName))
                {
                    LootFriendlyNamesCanShow.Add(lootListCSV.Last().FriendlyName);
                }

                if (!LootShortNamesCanShow.Contains(lootListCSV.Last().ShortName))
                {
                    LootShortNamesCanShow.Add(lootListCSV.Last().ShortName);
                }

                LootFriendlyNamesCanShow.Sort((x, y) => string.Compare(x, y));
                LootShortNamesCanShow.Sort((x, y) => string.Compare(x, y));
                LootCategoriesCanShow.Sort((x, y) => string.Compare(x, y));
                EntityTypesCanShow.Sort((x, y) => string.Compare(x, y));
            }
        }

        public class LootItemCSV
        {
            public string TemplateId { get; set; } = "Unknown";
            public string FriendlyName { get; set; } = "Unknown";
            public string ShortName { get; set; } = "Unknown";
            public string Category { get; set; } = "Unknown";
            public string Priority { get; set; } = "Unknown";
            public bool ForceShow { get; set; } = false;
            public int ArmorClass { get; set; } = -1;
        }

        private class CsvLootItemMapping : CsvMapping<LootItemCSV>
        {
            public CsvLootItemMapping()
                : base()
            {
                MapProperty(0, x => x.TemplateId);
                MapProperty(1, x => x.FriendlyName);
                MapProperty(2, x => x.ShortName);
                MapProperty(3, x => x.Category);
                MapProperty(4, x => x.Priority);
                MapProperty(5, x => x.ForceShow);
                MapProperty(6, x => x.ArmorClass);
            }
        }

        private class CsvLootItemMappingOld : CsvMapping<LootItemCSV>
        {
            public CsvLootItemMappingOld()
                : base()
            {
                MapProperty(0, x => x.TemplateId);
                MapProperty(1, x => x.FriendlyName);
                MapProperty(2, x => x.Category);
                MapProperty(3, x => x.Priority);
            }
        }

        public class LootItemWithPricesCSV
        {
            public string TemplateId { get; set; } = "Unknown";
            public string FriendlyName { get; set; } = "Unknown";
            public string Category { get; set; } = "Unknown";
            public string Priority { get; set; } = "Unknown";
            public int Slots { get; set; } = -1;
            public float Price { get; set; } = -1;
            public float TraderPrice { get; set; } = -1;
            public bool BannedOnFlea { get; set; } = false;
        }

        private class CsvLootItemWithPricesMapping : CsvMapping<LootItemWithPricesCSV>
        {
            public CsvLootItemWithPricesMapping()
                : base()
            {
                MapProperty(0, x => x.TemplateId);
                MapProperty(1, x => x.FriendlyName);
                MapProperty(2, x => x.Category);
                MapProperty(3, x => x.Priority);
                MapProperty(4, x => x.Slots);
                MapProperty(5, x => x.Price);
                MapProperty(6, x => x.TraderPrice);
                MapProperty(7, x => x.BannedOnFlea);
            }
        }
    }
}