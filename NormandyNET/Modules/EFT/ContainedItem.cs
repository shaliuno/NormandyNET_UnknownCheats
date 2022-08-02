using NormandyNET.Core;
using NormandyNET.Modules.EFT.Player;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NormandyNET.Modules.EFT
{
    internal class ContainedItem
    {
        internal StringBuilder stringBuilder = new StringBuilder();

        internal string parentItemBasicName;

        public bool? isContainer;
        public bool? spawnedInSession;
        internal bool isCorpse = false;
        public Color Color;
        public bool updateColors = true;
        public bool canRender = false;
        public bool updateRenderStatus = true;
        public bool markedForRemoval = false;
        internal bool wtsRender;
        internal CorpseData corpseData;

        public ulong address;
        public ulong parentAddress;

        internal string templateId;
        public string FriendlyName;
        public string ShortName;
        public string Category;
        public string Priority;
        public bool ForceShow;
        public int? Price;
        public int? PricePerSlot;

        private RenderItem renderItem = new RenderItem();
        private RenderItem renderItemOverlay = new RenderItem();
        internal List<ContainedItem> containedItems = new List<ContainedItem>();
        internal int containedDepth;
        private bool debugLog = false;

        private EntityLoot entityLoot;

        public ContainedItem(EntityLoot entityLoot)
        {
            this.entityLoot = entityLoot;
        }

        public ContainedItem(CorpseData corpseData)
        {
            this.corpseData = corpseData;
        }

        public UnityEngine.Vector3 Position
        {
            get
            {
                if (entityLoot != null) { return entityLoot.Position; }
                if (corpseData != null) { return corpseData.containedItem.Position; }

                return UnityEngine.Vector3.zero;
            }
        }

        internal void GetData(int _containedDepth)
        {
            containedDepth = _containedDepth;
            containedDepth++;

            
            GetTemplateId();
            GetCSVData();
            GetIsCorpse();

            GetSpawnedInSession();
            GetPrice();
            GetIsAirdrop();

            if (isCorpse == false)
            {
                if (ModuleEFT.settingsForm.settingsJson.Loot.ReadLootInContainers)
                {
                    GetContainedItems();

                    for (int i = 0; i < containedItems.Count; i++)
                    {
                        containedItems[i].GetData(containedDepth);
                    }
                }
            }

            if (isCorpse == true)
            {
                corpseData.GetData();

                if (ModuleEFT.settingsForm.settingsJson.Loot.ReadLootInContainers)
                {
                    corpseData.GetContainedItemsData();

                    for (int i = 0; i < corpseData.containedItems.Count; i++)
                    {
                        corpseData.containedItems[i].GetData(-9999);
                    }

                    containedItems = corpseData.containedItems;
                }
            }

            GetColor();
            GetCanRender();

                    }

        private void GetIsAirdrop()
        {
            if (string.Equals(FriendlyName, "Airdrop") || templateId.Equals("61a89e5445a2672acf66c877"))
            {
                entityLoot.Airdrop = true;
            }
        }

        private void GetTemplateId()
        {
            if (templateId != null)
            {
                return;
            }

            
            var itemTemplate = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate);
            var itemTemplateName = Memory.Read<ulong>(itemTemplate + ModuleEFT.offsetsEFT.InventoryLogic_ItemTemplate_Id);

            templateId = CommonHelpers.GetStringFromMemory_Unity(itemTemplateName, true);

                                            }

        public void GetCSVData()
        {
            if (FriendlyName == null || ShortName == null)
            {
                
                var lootCSV = LootItemHelper.GetLootFromCSV(templateId);
                FriendlyName = lootCSV.FriendlyName;
                ShortName = lootCSV.ShortName;
                Category = lootCSV.Category;
                Priority = lootCSV.Priority;
                ForceShow = lootCSV.ForceShow;
                                            }
        }

        private void GetIsCorpse()
        {
            if (
                (FriendlyName.Equals("Corpse") || templateId.Equals("55d7217a4bdc2d86028b456d") ||
                (parentItemBasicName != null && parentItemBasicName.ToLower().Contains("lootcorpse_playersuperior"))
                )
                && corpseData == null)
            {
                isCorpse = true;
                corpseData = new CorpseData(this);
                corpseData.diedInRaid = true;

                if (parentItemBasicName.ToLower().Contains("lootcorpse_playersuperior"))
                {
                    corpseData.diedInRaid = false;
                }
                else
                {
                    corpseData.diedInRaid = true;
                }
            }
        }

        private void GetSpawnedInSession()
        {
            if (spawnedInSession == null)
            {
                
                                var spawnedInSessionFlagPre = Memory.Read<byte>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_SpawnedInSession);
                spawnedInSession = spawnedInSessionFlagPre == 0x1 ? true : false;
                
                            }
        }

        private void GetPrice()
        {
            if (Price == null)
            {
                var lootCSV = LootItemHelper.GetLootPriceFromCSV(templateId);

                if (lootCSV.BannedOnFlea)
                {
                    Price = (int)(lootCSV.TraderPrice);
                }
                else
                {
                    Price = (int)lootCSV.Price;
                }

                PricePerSlot = (int)(Price / Math.Abs(lootCSV.Slots));
            }
        }

        internal void GetColor()
        {
            if (Color.IsEmpty || updateColors)
            {
                updateColors = false;
                Color = System.Drawing.Color.White;

                if (Category != null && ModuleEFT.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(Category, out System.Drawing.Color entityColor))
                {
                    
                    Color = entityColor;
                }
                else
                {
                                    }
            }

            if (isCorpse)
            {
                Color = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Corpse;
            }
        }

        internal void GetCanRender()
        {
            if (updateRenderStatus)
            {
                canRender = false;
                updateRenderStatus = false;

                
                var filteredItemFound = false;
                var filterEnabled = false;

                if (LootItemHelper.LootFriendlyNamesToShow.Count > 0)
                {
                    filterEnabled = true;

                    if (FriendlyName != null && LootItemHelper.LootFriendlyNamesToShow.Contains(FriendlyName))
                    {
                        canRender = true;
                        filteredItemFound = true;
                    }
                }

                if (LootItemHelper.LootShortNamesToShow.Count > 0)
                {
                    filterEnabled = true;

                    if (ShortName != null && LootItemHelper.LootShortNamesToShow.Contains(ShortName))
                    {
                        canRender = true;
                        filteredItemFound = true;
                    }
                }

                if (LootItemHelper.LootCategoriesToShow.Count > 0)
                {
                    filterEnabled = true;

                    if (Category != null && LootItemHelper.LootCategoriesToShow.Contains(Category))
                    {
                        canRender = true;
                        filteredItemFound = true;
                    }
                }

                if (filteredItemFound || filterEnabled)
                {
                    return;
                }

                if (Category != null)
                {
                    if (Category.Equals("Quest"))
                    {
                        canRender = ModuleEFT.settingsForm.settingsJson.Loot.ShowQuestItems;
                        return;
                    }

                    if (isCorpse)
                    {
                        canRender = ModuleEFT.settingsForm.settingsJson.Entity.Bodies;
                        return;
                    }

                    if (ModuleEFT.settingsForm.settingsJson.Loot.LootCategorySuppressed.Contains(Category) == true)
                    {
                        canRender = false;
                        return;
                    }

                    switch (Priority)
                    {
                        case "-1 Blacklist":
                            canRender = false;
                            return;
                            break;

                        case "0 None":
                            canRender = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[0];
                            break;

                        case "1 Low":
                            canRender = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[1];
                            break;

                        case "2 Medium":
                            canRender = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[2];
                            break;

                        case "3 High":
                            canRender = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[3];
                            break;

                        case "4 Ultra":
                            canRender = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[4];
                            break;

                        case "5 Super":
                            canRender = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[5];
                            break;

                        default:
                            break;
                    }

                    if (ModuleEFT.settingsForm.settingsJson.Loot.ShowByValue)
                    {
                        if (ModuleEFT.settingsForm.settingsJson.Loot.ValuePerSlot == false && Price >= (ModuleEFT.settingsForm.settingsJson.Loot.Value))
                        {
                            canRender = true;
                            return;
                        }
                        else if (ModuleEFT.settingsForm.settingsJson.Loot.ValuePerSlot == true && PricePerSlot >= (ModuleEFT.settingsForm.settingsJson.Loot.Value))
                        {
                            canRender = true;
                            return;
                        }
                        else if (ModuleEFT.settingsForm.settingsJson.Loot.ForceShow == true && ForceShow)
                        {
                            canRender = true;
                            return;
                        }
                        else
                        {
                            canRender = false;
                            return;
                        }
                    }
                }
            }
        }

        internal void GetContainedItems()
        {
            if (containedItems.Count > 0)
            {
                for (int i = 0; i < containedItems.Count; i++)
                {
                    containedItems[i].markedForRemoval = true;
                }
            }

            
            var stackObjectsCount = Memory.Read<int>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_StackedObjectCount);
            
            if (stackObjectsCount == 0)
            {
                                return;
            }

            var containedItemGrids = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemGrids);
            var containedItemSlots = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemSlots);
            
            var canReadGrids = false;
            var canReadSlots = false;

            var canReadGridsOrSlots = Memory.IsValidPointer(Memory.Read<ulong>(address + 0x78));

            
            if (canReadGridsOrSlots && Memory.IsValidPointer(containedItemGrids))
            {
                canReadGrids = true;
            }

            
            if (canReadGridsOrSlots && Memory.IsValidPointer(containedItemSlots))
            {
                canReadSlots = true;
            }

            
            var itemGridsCount = 0;

            if (canReadGrids)
            {
                itemGridsCount = Memory.Read<int>(containedItemGrids + 0x18);
                
                if (itemGridsCount > 50 && canReadGrids)
                {
                                        canReadGrids = false;
                }
            }

            if (canReadGrids == true && itemGridsCount > 0)
            {
                for (uint z = 0; z < itemGridsCount; z++)
                {
                    var gridStart = Memory.Read<ulong>(containedItemGrids + 0x20 + (z * 0x8));
                    
                    var gridDict = Memory.ReadChain<ulong>(gridStart, new uint[] { 0x40, 0x10 });
                    
                    var gridDictSize = Memory.Read<uint>(gridDict + 0x40);
                    
                    var gridDictEntires = Memory.Read<ulong>(gridDict + 0x18);
                    
                    for (uint x = 0; x < gridDictSize; x++)
                    {
                        var itemInGrid = Memory.Read<ulong>(gridDictEntires + 0x28 + 0x18 * x);
                        
                        if (itemInGrid == 0)
                        {
                            continue;
                        }

                        var containedItem = new ContainedItem(entityLoot);
                        var containedItemIndex = containedItems.IndexOf(containedItem);

                        if (containedItemIndex < 0)
                        {
                            containedItem.isContainer = true;
                            containedItem.address = itemInGrid;

                            containedItem.parentAddress = parentAddress;
                            containedItems.Add(containedItem);
                        }
                        else
                        {
                            containedItems[containedItemIndex].markedForRemoval = false;
                        }

                                            }
                }
            }

            var itemSlotsCount = 0;

            if (canReadSlots)
            {
                itemSlotsCount = Memory.Read<int>(containedItemSlots + 0x18);
                
                if (itemSlotsCount > 50 && canReadSlots)
                {
                                        canReadSlots = false;
                }
            }

            if (canReadSlots == true && itemSlotsCount > 0)
            {
                for (uint z = 0; z < itemSlotsCount; z++)
                {
                    var slotsStart = Memory.Read<ulong>(containedItemSlots + 0x20 + (z * 0x8));

                    var containedItemInSlot = Memory.Read<ulong>(slotsStart + ModuleEFT.offsetsEFT.InventoryLogic_Slot_ContainedItem);
                    if (containedItemInSlot == 0)
                    {
                        continue;
                    }

                    var containedItem = new ContainedItem(entityLoot);
                    var containedItemIndex = containedItems.IndexOf(containedItem);

                    if (containedItemIndex < 0)
                    {
                        containedItem.isContainer = true;
                        containedItem.address = containedItemInSlot;

                        containedItem.parentAddress = parentAddress;
                        containedItems.Add(containedItem);
                    }
                    else
                    {
                        containedItems[containedItemIndex].markedForRemoval = false;
                    }
                }
            }

            containedItems.RemoveAll(x => x.markedForRemoval);
        }

        internal void GenerateRenderItem()
        {
            if (ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged)
            {
                updateColors = true;
                GetColor();
            }

            if (ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged)
            {
                updateRenderStatus = true;
                GetCanRender();
            }
         
            {
                if (containedItems != null && containedItems.Count != 0)
                {
                    for (int i = 0; i < containedItems.Count; i++)
                    {
                        containedItems[i].GenerateRenderItem();
                    }
                }
            }

            if (!canRender)
            {
                return;
            }

            var invertMap = ModuleEFT.radarForm.mapManager.GetInvertMap();

            var entityPosXingame = Position.x;
            var entityPosYingame = Position.y;
            var entityPosZingame = Position.z;

            var lootPosXmap = (entityPosXingame * OpenGL.CanvasDiffCoeff * invertMap);
            var lootPosZmap = (entityPosZingame * OpenGL.CanvasDiffCoeff * invertMap);
            var lootPosYmap = entityPosYingame;

            if (!ModuleEFT.radarForm.IsVisibleOnControl(lootPosXmap, lootPosZmap))
            {
                return;
            }

            if (Position == UnityEngine.Vector3.zero)
            {
                return;
            }

            if (ModuleEFT.settingsForm.settingsJson.Loot.Show == false && isCorpse != true)
            {
                return;
            }

            if (ModuleEFT.settingsForm.settingsJson.Loot.Show == true && ModuleEFT.settingsForm.settingsJson.Entity.Bodies == false && isCorpse == true)
            {
                return;
            }

            renderItem = new RenderItem();
            renderItem.MapPosX = lootPosXmap;
            renderItem.MapPosZ = lootPosZmap;
            renderItem.IconPositionTexture = IconPositionTexture.loot;
            renderItem.DrawColor = Color;
            renderItem.IconSize = ModuleEFT.settingsForm.settingsJson.Map.IconSizeLoot;

            if (isContainer == true)
            {
                renderItem.IconPositionTexture = IconPositionTexture.tentbox;
            }

            if (isCorpse == true)
            {
                if (corpseData != null)
                {
                    if (corpseData.Side == EPlayerSide.Bear || corpseData.Side == EPlayerSide.Bear)
                    {
                        renderItem.DrawColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.PMC;
                        renderItem.IconPositionTexture = IconPositionTexture.player_dead;
                    }

                    if (corpseData.Side == EPlayerSide.Savage)
                    {
                        renderItem.DrawColor = Color;
                        renderItem.DrawColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Bot;
                        renderItem.IconPositionTexture = IconPositionTexture.npc_dead;
                    }
                }
            }

            renderItem.renderLayer = RenderLayers.LootPriority1;
            OpenGL.MapIcons.Add(renderItem);

            stringBuilder.Clear();
            renderItem = new RenderItem();
            renderItem.MapPosX = lootPosXmap + 10 + OpenGL.CanvasDiffCoeff;
            renderItem.MapPosZ = lootPosZmap + 8;

            if (ModuleEFT.settingsForm.settingsJson.Loot.ShortNames)
            {
                stringBuilder.Append($"{ShortName.Truncate(ModuleEFT.settingsForm.settingsJson.Loot.NameLengthLimit)}");
            }
            else
            {
                stringBuilder.Append($"{FriendlyName.Truncate(ModuleEFT.settingsForm.settingsJson.Loot.NameLengthLimit)}");
            }

            if (isContainer == true)
            {
            }

            if (isCorpse == true)
            {
                stringBuilder.Clear();

                if (ModuleEFT.settingsForm.settingsJson.Entity.InventoryValue && corpseData != null && corpseData.FullInventoryRead)
                {
                    if (ModuleEFT.settingsForm.settingsJson.Entity.InventoryValueUseLootFilters == false)
                    {
                        stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}{corpseData.inventoryValueTotalStr}");
                    }
                    else
                    {
                        if (ModuleEFT.settingsForm.settingsJson.Loot.ShowByValue)
                        {
                            stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}{corpseData.inventoryValueFilteredByPriceStr}");
                        }
                        else
                        {
                            stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}{corpseData.inventoryValueFilteredByPriorityStr}");
                        }
                    }

                    if (corpseData.inventoryValueFilteredByPriority4Ultra)
                    {
                        stringBuilder.Append($" ©{CommonHelpers.ColorHexConverter(Color.DodgerBlue)}Ultra");
                    }

                    if (corpseData.inventoryValueFilteredByPriority5Super)
                    {
                        stringBuilder.Append($" ©{CommonHelpers.ColorHexConverter(Color.LightCoral)}Super");
                    }

                    stringBuilder.Append($"\n");
                }
            }

            renderItem.DrawColor = Color;

            renderItem.Size = (int)FontSizes.misc;
            renderItem.TextOverlayOutline = true;
            renderItem.renderLayer = RenderLayers.LootPriority1;

            var elevation = Math.Round((Position.y - CommonHelpers.myIngamePositionY), 0);

            if (ModuleEFT.settingsForm.settingsJson.Loot.Aggregate == false && ModuleEFT.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Absolute))
            {
                stringBuilder.Append($" ©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}({(int)Position.y})");
            }

            if (ModuleEFT.settingsForm.settingsJson.Loot.Aggregate == false && ModuleEFT.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Relative))
            {
                stringBuilder.Append($" ©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}({elevation})");
            }

            if (false)
            {
            }

            if (ModuleEFT.settingsForm.settingsJson.Loot.ShowByValue || ModuleEFT.settingsForm.settingsJson.Loot.ShowPricesAlways)
            {
                if (ModuleEFT.settingsForm.settingsJson.Loot.ValuePerSlot == false && Price != null)
                {
                    stringBuilder.Append($"\n ©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}({Price / 1000}k)");
                }

                if (ModuleEFT.settingsForm.settingsJson.Loot.ValuePerSlot == true && PricePerSlot >= (ModuleEFT.settingsForm.settingsJson.Loot.Value))
                {
                    stringBuilder.Append($"\n ©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}({PricePerSlot / 1000}k)");
                }
            }

            if (ModuleEFT.settingsForm.settingsJson.Loot.Aggregate == false && ModuleEFT.settingsForm.settingsJson.Entity.Elevation.Arrows)
            {
                var elevationDiff = (int)Math.Round(elevation / 4, 0);
                var strSigh = ' ';
                var renderGlyph = false;

                if (elevationDiff > 0)
                {
                    if (elevationDiff > 3)
                    {
                        elevationDiff = 3;
                    }

                    strSigh = (char)9650;
                    renderGlyph = true;
                }

                if (elevationDiff < 0)
                {
                    if (elevationDiff < -3)
                    {
                        elevationDiff = -3;
                    }

                    strSigh = (char)9660;
                    renderGlyph = true;
                }

                if (renderGlyph)
                {
                    if (strSigh == (char)9650)
                    {
                        stringBuilder.Append($" ©{CommonHelpers.ColorHexConverter(Color.LimeGreen)}{strSigh}");
                    }
                    if (strSigh == (char)9660)
                    {
                        stringBuilder.Append($" ©{CommonHelpers.ColorHexConverter(Color.Red)}{strSigh}");
                    }
                }
            }

            if (ModuleEFT.settingsForm.settingsJson.Loot.Aggregate)
            {
                renderItem.Aggregate = true;

                float gridSize = 50f;
                float aggregatedPosX = (float)Math.Ceiling(Math.Abs(lootPosXmap / gridSize)) * (lootPosXmap >= 0 ? 1 : -1) * gridSize - gridSize / 2 * -1;
                float aggregatedPosY = (float)Math.Ceiling(Math.Abs(lootPosZmap / gridSize)) * (lootPosXmap >= 0 ? 1 : -1) * gridSize - gridSize / 2 * -1;

                var result = OpenGL.MapText.FindIndex(x => (x.MapPosX == aggregatedPosX) && (x.MapPosZ == aggregatedPosY));

                renderItem.MapPosX = aggregatedPosX;
                renderItem.MapPosZ = aggregatedPosY;

                if (result > -1)
                {
                    var existingString = OpenGL.MapText[result].textNew.FindIndex(x => x.text == renderItem.Text);

                    if (existingString > -1)
                    {
                        OpenGL.MapText[result].textNew[existingString].count++;
                    }
                    else
                    {
                        OpenGL.MapText[result].textNew.Add(new RenderItem.TextNew { text = renderItem.Text, count = 1 });
                    }
                }
                else
                {
                    renderItem.textNew.Add(new RenderItem.TextNew { text = renderItem.Text, count = 1 });
                }
            }

            renderItem.Text = stringBuilder.ToString();
            OpenGL.MapText.Add(renderItem);
        }

        internal void GenerateRenderItemOverlay()
        {
            {
                for (int i = 0; i < containedItems.Count; i++)
                {
                    containedItems[i].GenerateRenderItemOverlay();
                }
            }

            if (!canRender)
            {
                return;
            }

            if (Position == UnityEngine.Vector3.zero)
            {
                return;
            }

            var myPosX = CommonHelpers.myIngamePositionX;
            var myPosY = CommonHelpers.myIngamePositionY;
            var myPosZ = CommonHelpers.myIngamePositionZ;

            var vectorMe = new OpenTK.Vector3(myPosX, myPosY, myPosZ);

            var entityPosXingame = Position.x;
            var entityPosYingame = Position.y;
            var entityPosZingame = Position.z;

            var vectorTarget = new OpenTK.Vector3(entityPosXingame, entityPosYingame, entityPosZingame);

            float distance = (int)Math.Round(CommonHelpers.GetDistance(vectorMe, vectorTarget));

            if (distance < ModuleEFT.settingsForm.settingsJson.Overlay.DrawDistanceLoot)
            {
                var wts = ModuleEFT.radarForm.overlay.WorldToScreen(vectorTarget, out OpenTK.Vector3 coords, ModuleEFT.readerEFT.fpsCamera.GetViewMartix(), ModuleEFT.radarForm.overlay.Width, ModuleEFT.radarForm.overlay.Height);

                if (wts)
                {
               
                    var radius = (int)Math.Ceiling(5 / distance / ModuleEFT.radarForm.overlay.fixedHeadCircleSize / 2);

                    renderItemOverlay = new RenderItem();
                    renderItemOverlay.Text = "rectangle";
                    renderItemOverlay.Size = ModuleEFT.radarForm.overlay.geometryPixelSize;
                    renderItemOverlay.DrawColor = Color;
                    renderItemOverlay.MapPosX = coords.X + radius;
                    renderItemOverlay.MapPosZ = coords.Y - radius;
                    renderItemOverlay.MapPosXend = coords.X - radius;
                    renderItemOverlay.MapPosZend = coords.Y + radius;
                    OpenGL.OverlayGeometry.Add(renderItemOverlay);

                    renderItemOverlay = new RenderItem();
                    renderItemOverlay.Text = FriendlyName + "";
                    renderItemOverlay.TextOverlayOutline = true;
                    renderItemOverlay.MapPosX = coords.X + 15;
                    renderItemOverlay.MapPosZ = coords.Y;
                    renderItemOverlay.Size = (int)FontSizes.misc;
                    renderItemOverlay.DrawColor = Color;
                    OpenGL.OverlayText.Add(renderItemOverlay);
                }
            }
        }
    }
}