using NormandyNET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using static NormandyNET.Modules.EFT.InventoryController;

namespace NormandyNET.Modules.EFT.Player
{
    internal class CorpseData
    {
        internal ContainedItem containedItem;
        internal EntityPlayer entityPlayer;
        internal ulong playerBody;

        internal EPlayerSide Side;
        internal List<ContainedItem> containedItems = new List<ContainedItem>();
        internal bool diedInRaid = true;
        private Dictionary<EquipmentSlots, Slot> slotsDict = new Dictionary<EquipmentSlots, Slot>();

        private bool InventoryRead;
        private bool StaticDataRead;
        internal bool FullInventoryRead;
        internal uint inventoryValueTotal;
        internal uint inventoryValueFilteredByPrice;
        internal uint inventoryValueFilteredByPriority;
        internal string inventoryValueTotalStr;
        internal string inventoryValueFilteredByPriceStr;
        internal string inventoryValueFilteredByPriorityStr;
        internal bool inventoryValueFilteredByPriority4Ultra;
        internal bool inventoryValueFilteredByPriority5Super;
        internal List<Item> items = new List<Item>();

        public CorpseData(ContainedItem containedItem)
        {
            this.containedItem = containedItem;
        }

        internal void GetData()
        {
            if (!StaticDataRead)
            {
                GetSide();

                playerBody = Memory.Read<ulong>(containedItem.parentAddress + ModuleEFT.offsetsEFT.Interactive_Corpse_PlayerBody);
                StaticDataRead = true;
            }
        }

        internal void GetContainedItemsData()
        {
            ReadItemsViaSlotsView();
            GetFullPlayerInventory();
        }

        private void GetInventory(ulong playerBody)
        {
            var playerBones = Memory.Read<ulong>(playerBody + 0x20);
            var playerClass = Memory.Read<ulong>(playerBones + 0x18);
            entityPlayer = new EntityPlayer(playerBody + 0x18, playerClass);

            if (entityPlayer.inventoryController == null) { entityPlayer.inventoryController = new InventoryController(entityPlayer); }

            entityPlayer.inventoryController.InventoryInfoUpdateUpdateTimeLast = CommonHelpers.dateTimeHolder.AddDays(-1);
            entityPlayer.inventoryController.GetPlayerInventory();
            entityPlayer.inventoryController.GetFullPlayerInventory();
        }

        internal void ReadItemsViaSlotsView()
        {
            if (InventoryRead)
            {
                return;
            }

            var slotViews = Memory.Read<ulong>(playerBody + ModuleEFT.offsetsEFT.Player_PlayerBody_SlotsView);

            if (!Memory.IsValidPointer(slotViews))
            {
                return;
            }

            var slotViewsList = Memory.Read<ulong>(slotViews + 0x18);

            if (!Memory.IsValidPointer(slotViewsList))
            {
                return;
            }

            var pList = Memory.Read<ulong>(slotViewsList + 0x10);

            var slotViewsListSize = Memory.Read<uint>(slotViewsList + 0x18);

            for (uint i = 0; i < slotViewsListSize; i++)
            {
                var slotAddress = Memory.Read<ulong>(pList + ModuleEFT.offsetsEFT.ArrayFirstEntry + (0x8 * i));
                
                if (!Memory.IsValidPointer(slotAddress))
                {
                    continue;
                }

                var itemInSlot = Memory.Read<ulong>(slotAddress + 0x10);
                if (!Memory.IsValidPointer(slotAddress))
                {
                    continue;
                }

                var nameAddress = Memory.Read<ulong>(itemInSlot + 0x10);
                
                var name = CommonHelpers.GetStringFromMemory_Unity(nameAddress, true);
                
                if (Enum.TryParse(name, out EquipmentSlots slotsEnum))
                {
                    if (allowedSlots.Contains(slotsEnum))
                    {
                        if (slotsDict.ContainsKey(slotsEnum))
                        {
                            slotsDict.Remove(slotsEnum);
                        }

                        slotsDict.Add(slotsEnum, new Slot(itemInSlot, slotsEnum));
                    }
                }
            }

            InventoryRead = true;
        }

        internal void GetFullPlayerInventory()
        {
            if (ModuleEFT.settingsForm.settingsJson.Entity.InventoryValue == false)
            {
                return;
            }

            if (!InventoryRead || slotsDict.Count == 0 || FullInventoryRead)
            {
                return;
            }

            for (int index = 0; index < slotsDict.Count; index++)
            {
                slotsDict.ElementAt(index).Value.GetAllItemsRecursive();
            }

            FullInventoryRead = true;
            GetInventoryValue();
        }

        internal void GetInventoryValue()
        {
            items.Clear();
            inventoryValueTotal = 0;
            inventoryValueFilteredByPrice = 0;
            inventoryValueFilteredByPriority = 0;
            inventoryValueFilteredByPriority4Ultra = false;
            inventoryValueFilteredByPriority5Super = false;

            for (int index = 0; index < slotsDict.Count; index++)
            {
                var res = slotsDict.ElementAt(index).Value.item.GetAllChildsForItem();
                items.AddRange(res);
            }

            MoveItemsToContainedItemsInventory();

            foreach (Item i in items)
            {
                if (Memory.IsValidPointer(i.address))
                {
                    var okToAddTotal = false;
                    var okToAddByPrice = false;
                    var okToAddByPriority = false;

                    switch (i.Priority)
                    {
                        case "-1 Blacklist":
                            okToAddByPriority = false;
                            break;

                        case "0 None":
                            okToAddByPriority = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[0];
                            break;

                        case "1 Low":
                            okToAddByPriority = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[1];
                            break;

                        case "2 Medium":
                            okToAddByPriority = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[2];
                            break;

                        case "3 High":
                            okToAddByPriority = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[3];
                            break;

                        case "4 Ultra":
                            okToAddByPriority = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[4];
                            inventoryValueFilteredByPriority4Ultra = true;
                            break;

                        case "5 Super":
                            okToAddByPriority = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[5];
                            inventoryValueFilteredByPriority5Super = true;
                            break;

                        default:
                            break;
                    }

                    if (okToAddByPriority)
                    {
                        inventoryValueFilteredByPriority += (uint)i.Price;
                    }

                    if (ModuleEFT.settingsForm.settingsJson.Loot.ValuePerSlot == false && i.Price >= (ModuleEFT.settingsForm.settingsJson.Loot.Value))
                    {
                        okToAddByPrice = true;
                    }
                    else if (ModuleEFT.settingsForm.settingsJson.Loot.ValuePerSlot == true && i.PricePerSlot >= (ModuleEFT.settingsForm.settingsJson.Loot.Value))
                    {
                        okToAddByPrice = true;
                    }
                    else if (ModuleEFT.settingsForm.settingsJson.Loot.ForceShow == true && i.ForceShow)
                    {
                        okToAddByPrice = true;
                    }
                    else
                    {
                        okToAddByPrice = false;
                    }

                    if (okToAddByPrice)
                    {
                        inventoryValueFilteredByPrice += (uint)i.Price;
                    }

                    inventoryValueTotal += (uint)i.Price;

                    inventoryValueTotalStr = ToKMB(inventoryValueTotal);
                    inventoryValueFilteredByPriorityStr = ToKMB(inventoryValueFilteredByPriority);
                    inventoryValueFilteredByPriceStr = ToKMB(inventoryValueFilteredByPrice);
                }
            }
        }

        private void MoveItemsToContainedItemsInventory()
        {
            containedItems.Clear();

            for (int i = 0; i < items.Count; i++)
            {
                var containedItem = new ContainedItem(this);
                containedItem.isContainer = false;
                containedItem.address = items[i].address;

                containedItem.parentAddress = this.containedItem.parentAddress;
                containedItems.Add(containedItem);
            }
        }

        internal void GetSide()
        {
            Side = GetPlayerSide();
        }

        private EPlayerSide GetPlayerSide()
        {
            return (EPlayerSide)Memory.Read<int>(containedItem.parentAddress + ModuleEFT.offsetsEFT.Interactive_Corpse_PlayerSide);
        }

        private void MoveItemsToContainedItems()
        {
        }
    }
}