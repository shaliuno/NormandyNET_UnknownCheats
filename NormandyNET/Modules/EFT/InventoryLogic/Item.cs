using NormandyNET.Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static NormandyNET.Modules.EFT.InventoryController;

namespace NormandyNET.Modules.EFT
{
    internal class Item
    {
        private Slot slot;
        internal ulong address;
        internal ulong itemTemplateIdPtr;

        internal List<Item> containedItems = new List<Item>();
        internal EntityLoot entityLoot;

        internal Item item;
        private bool debugLog = false;
        internal Regex weaponRegex = new Regex("(n.a)|weapon_|(izhmash_|izhmeh_|toz_|tochmash_|lobaev_|molot_)|_[0-9]+([0-9]|[a-zA-Z])+");

        internal ulong templateAddress;
        internal string templateId;
        internal string templateName;
        internal bool? spawnedInSession;
        internal int? stackObjectsCount;

        public string FriendlyName;
        public string ShortName;
        public string Category;
        public string Priority;
        public bool ForceShow;
        public int ArmorClass;

        public int? Price;
        public int? PricePerSlot;

        private bool hasDataAsSlot;

        public Item(EntityLoot entityLoot)
        {
            this.entityLoot = entityLoot;
            address = Memory.Read<ulong>(entityLoot.lootAddress + ModuleEFT.offsetsEFT.Interactive_LootItem_ContainedItem, false);
                    }

        public enum EDeafStrength : byte
        {
            None,
            Low,
            High
        }

        internal void GetAvailableFireModes()
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            var weapFireTypeArray = Memory.Read<ulong>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_WeapFireType);
            var arraySize = Memory.Read<int>(weapFireTypeArray + ModuleEFT.offsetsEFT.ListClassEntryCount);

            for (uint i = 0; i < arraySize; i++)
            {
            }
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        internal void AdjustMagLoadUnloadModifiers(float modifier)
        {
            containedItems = GetDataRecursive();

            foreach (var item in containedItems)
            {
                item.GetData();
                if (string.Equals(item.Category, "Magazines"))
                {
                    if (Memory.IsValidPointer(address) && item.templateAddress == 0)
                    {
                        item.templateAddress = Memory.Read<ulong>(item.address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
                    }

                    var LoadUnloadModifier = Memory.Read<float>(item.templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_MagazineTemplate_LoadUnloadModifier);
                    if (LoadUnloadModifier != modifier)
                    {
                        Memory.Write<float>(item.templateAddress + 0x174, modifier);
                        item.SetTaxonomyColor(TaxonomyColor.green);
                    }

                    if (LoadUnloadModifier == modifier)
                    {
                        item.SetTaxonomyColor(TaxonomyColor.green);
                    }
                }
            }
        }

        private void SetTaxonomyColor(TaxonomyColor color)
        {
            Memory.Write<int>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_ItemTemplate_BackgroundColor, (int)color);
        }

        public enum TaxonomyColor
        {
            blue,
            yellow,
            green,
            red,
            black,
            grey,
            violet,
            orange,
            tracerYellow,
            tracerGreen,
            tracerRed,
            @default
        }

        internal bool OverrideAvailableFireModes()
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            var weapFireTypeArray = Memory.Read<ulong>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_WeapFireType);
            var arraySize = Memory.Read<int>(weapFireTypeArray + ModuleEFT.offsetsEFT.ListClassEntryCount);

            if (arraySize != 1)
            {
                return false;
            }

            byte[] byteArray = new byte[arraySize * sizeof(int)];

            Memory.WriteBytes(weapFireTypeArray + ModuleEFT.offsetsEFT.ArrayFirstEntry, ref byteArray);
            return true;
        }

        internal float CurrentAmmoTemplateInitialSpeed()
        {
            var ChambersArray = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Weapon_Chambers);
            var ChambersArraySize = Memory.Read<int>(ChambersArray + ModuleEFT.offsetsEFT.ListClassEntryCount);

            if (ChambersArraySize > 0)
            {
                var chamberSlot = Memory.Read<ulong>(ChambersArray + ModuleEFT.offsetsEFT.ArrayFirstEntry + (0x8 * 0));

                if (Memory.IsValidPointer(chamberSlot))
                {
                    var slot = new Slot(chamberSlot);

                    return slot.item.InitialSpeed();
                }
            }

            return 0;
        }

        private float InitialSpeed()
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            return Memory.Read<float>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_AmmoTemplate_InitialSpeed);
        }

        internal int SingleFireRate()
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            return Memory.Read<int>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_SingleFireRate);
        }

        internal void SetSingleFireRate(int firerate)
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            Memory.Write<int>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_SingleFireRate, firerate);
        }

        internal int FireRate()
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            return Memory.Read<int>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_FireRate);
        }

        internal void SetFireRate(int firerate)
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            Memory.Write<int>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_FireRate, firerate);
        }

        internal bool IsBoltAction()
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            return Memory.Read<byte>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_BoltAction) == (byte)1 ? true : false;
        }

        internal void SetIsBoltAction(bool isBoltAction)
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            if (isBoltAction)
            {
                Memory.Write<byte>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_BoltAction, (byte)1);
            }
            else
            {
                Memory.Write<byte>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_BoltAction, (byte)0);
            }
        }

        internal void NoWeaponMalfunctions(bool allowMalfunctions)
        {
            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            if (allowMalfunctions)
            {
                var bytes = new byte[] { 0x1, 0x1, 0x1, 0x1 };
                Memory.WriteBytes(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_AllowJam, ref bytes);
                Memory.Write<byte>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_AllowOverheat, (byte)1);
            }
            else
            {
                var bytes = new byte[] { 0x0, 0x0, 0x0, 0x0 };
                Memory.WriteBytes(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_AllowJam, ref bytes);
                Memory.Write<byte>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_WeaponTemplate_AllowOverheat, (byte)1);
                SetTaxonomyColor(TaxonomyColor.blue);
            }
        }

        internal void RemoveArmorMovementPenalties(EquipmentSlots equipmentSlot)
        {
            if (slot == null)
            {
                return;
            }

            if (Memory.IsValidPointer(address) && templateAddress == 0)
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            var components = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_Components);

            var componentsList = Memory.Read<ulong>(components + ModuleEFT.offsetsEFT.ListClass);
            var componentsCount = Memory.Read<ulong>(componentsList + ModuleEFT.offsetsEFT.ListClassEntryCount);

            for (uint i = 0; i < componentsCount; i++)
            {
                var componentEntry = Memory.Read<ulong>(componentsList + 0x20 + (i * 0x8));

                if (Memory.IsValidPointer(componentEntry))
                {
                    

                    var template = Memory.Read<ulong>(componentEntry + ModuleEFT.offsetsEFT.InventoryLogic_ArmorComponent_Template);

                    if (equipmentSlot == EquipmentSlots.Headwear || equipmentSlot == EquipmentSlots.ArmorVest)
                    {
                        var speedPenaltyPercent = Memory.Read<int>(template + ModuleEFT.offsetsEFT.InventoryLogic_ArmorComponent_Template_SpeedPenaltyPercent);
                        var mousePenalty = Memory.Read<int>(template + ModuleEFT.offsetsEFT.InventoryLogic_ArmorComponent_Template_MousePenalty);
                        var deafStrength = (EDeafStrength)Memory.Read<byte>(template + 0x188);

                        if (speedPenaltyPercent != 0 || mousePenalty != 0)
                        {
                            Memory.Write<int>(template + ModuleEFT.offsetsEFT.InventoryLogic_ArmorComponent_Template_SpeedPenaltyPercent, 0);
                            Memory.Write<int>(template + ModuleEFT.offsetsEFT.InventoryLogic_ArmorComponent_Template_MousePenalty, 0);
                            SetTaxonomyColor(TaxonomyColor.green);
                        }

                        if (deafStrength != EDeafStrength.None)
                        {
                            Memory.Write<byte>(template + 0x17C, (byte)EDeafStrength.None);
                        }
                    }

                    if (equipmentSlot == EquipmentSlots.TacticalVest)
                    {
                        var speedPenaltyPercent = Memory.Read<int>(template + ModuleEFT.offsetsEFT.InventoryLogic_ArmorComponent_Template_SpeedPenaltyPercent + 0xC);
                        var mousePenalty = Memory.Read<int>(template + ModuleEFT.offsetsEFT.InventoryLogic_ArmorComponent_Template_MousePenalty + 0xC);

                        if (speedPenaltyPercent != 0 || mousePenalty != 0)
                        {
                            Memory.Write<int>(template + ModuleEFT.offsetsEFT.InventoryLogic_ArmorComponent_Template_SpeedPenaltyPercent + 0xC, 0);
                            Memory.Write<int>(template + ModuleEFT.offsetsEFT.InventoryLogic_ArmorComponent_Template_MousePenalty + 0xC, 0);
                            SetTaxonomyColor(TaxonomyColor.green);
                        }
                    }
                }
            }
        }

        public Item(Item item, ulong address)
        {
            this.item = item;
            this.address = address;
        }

        public Item(Slot slot)
        {
            this.slot = slot;
            address = Memory.Read<ulong>(slot.address + ModuleEFT.offsetsEFT.InventoryLogic_Slot_ContainedItem, false);
                    }

        public Item(ulong address)
        {
            this.address = address;
            GetTemplateName();
        }

        internal void GetData()
        {
            if (!Memory.IsValidPointer(address))
            {
                return;
            }

            GetTemplateId();
            GetTemplateName();
            GetStackObjectsCount();
            GetSpawnedInSession();
            GetCSVData();
            GetPrice();
        }

        internal void GetItemsRecursive()
        {
            if (!Memory.IsValidPointer(address))
            {
                return;
            }

            GetData();
            containedItems = GetDataRecursive();

            for (int i = 0; i < containedItems.Count; i++)
            {
                containedItems[i].GetItemsRecursive();
            }
        }

        internal void GetDataAsSlot()
        {
            if (!Memory.IsValidPointer(address) || hasDataAsSlot)
            {
                return;
            }

            GetData();

            hasDataAsSlot = true;
        }

        private void GetSpawnedInSession()
        {
            if (spawnedInSession == null)
            {
                var spawnedInSessionFlagPre = Memory.Read<byte>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_SpawnedInSession);
                spawnedInSession = spawnedInSessionFlagPre == 0x1 ? true : false;
                            }
        }

        private void GetStackObjectsCount()
        {
            if (stackObjectsCount == null)
            {
                stackObjectsCount = Memory.Read<int>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_StackedObjectCount);
                            }
        }

        private void GetTemplateId()
        {
            if (templateId != null)
            {
                return;
            }

            
            if (Memory.IsValidPointer(address))
            {
                templateAddress = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            
            if (Memory.IsValidPointer(templateAddress))
            {
                itemTemplateIdPtr = Memory.Read<ulong>(templateAddress + ModuleEFT.offsetsEFT.InventoryLogic_ItemTemplate_Id);
            }

            
            if (Memory.IsValidPointer(itemTemplateIdPtr))
            {
                templateId = CommonHelpers.GetStringFromMemory_Unity(itemTemplateIdPtr, true);
                                                return;
            }
        }

        private void GetTemplateName()
        {
            if (templateName != null)
            {
                return;
            }

            ulong itemTemplate = 0;
            ulong itemTemplateNamePtr = 0;

            if (Memory.IsValidPointer(address))
            {
                itemTemplate = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            }

            
            if (Memory.IsValidPointer(itemTemplate))
            {
                itemTemplateNamePtr = Memory.Read<ulong>(itemTemplate + ModuleEFT.offsetsEFT.InventoryLogic_ItemTemplate_Name);
            }

            if (Memory.IsValidPointer(itemTemplateNamePtr))
            {
                templateName = CommonHelpers.GetStringFromMemory_Unity(itemTemplateNamePtr, true);
                                return;
            }
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
                ArmorClass = lootCSV.ArmorClass;
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
                    Price = Math.Max((int)lootCSV.Price, (int)(lootCSV.TraderPrice));
                }

                PricePerSlot = (int)(Price / Math.Abs(lootCSV.Slots));
            }
        }

        internal string GetItemName()
        {
            if (Memory.IsValidPointer(address))
            {
                return ShortName;
            }

            return string.Empty;
        }

        internal List<Item> GetDataRecursive()
        {
            List<Item> _containedItems = new List<Item>();

            if (stackObjectsCount == 0)
            {
                                return _containedItems;
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

                        var containedItem = new Item(this, itemInGrid);
                        _containedItems.Add(containedItem);
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

                    var containedItem = new Item(this, containedItemInSlot);
                    _containedItems.Add(containedItem);
                }
            }

            return _containedItems;
        }

        public List<Item> GetAllChildsForItem()
        {
            List<Item> ret = new List<Item>();
            GetAllChilds(this, ret);
            return ret;
        }

        private void GetAllChilds(Item item, List<Item> ret)
        {
            ret.Add(item);

            if (item.containedItems != null)
            {
                foreach (Item itemEntry in item.containedItems)
                {
                    GetAllChilds(itemEntry, ret);
                }
            }
        }
    }
}