using NormandyNET.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NormandyNET.Modules.EFT
{
    internal class InventoryController
    {
        internal ulong address;
        internal ulong addressOriginal;
        private EntityPlayer entityPlayer;

        internal bool InventoryRead;
        internal bool FullInventoryRead;
        internal string Weapons;
        internal string ArmorClass;
        internal bool WeaponRead;
        internal bool ArmorClassRead;

        internal DateTime InventoryInfoUpdateUpdateTimeLast;
        internal bool InventoryInfoUpdateAllowed;
        internal int InventoryInfoUpdateRateMsec = 25000;

        internal uint inventoryValueTotal;
        internal uint inventoryValueFilteredByPrice;
        internal uint inventoryValueFilteredByPriority;
        internal string inventoryValueTotalStr;
        internal string inventoryValueFilteredByPriceStr;
        internal string inventoryValueFilteredByPriorityStr;
        internal bool inventoryValueFilteredByPriority4Ultra;
        internal bool inventoryValueFilteredByPriority5Super;
        internal List<Item> items = new List<Item>();

        internal Dictionary<EquipmentSlots, Slot> slotsDict = new Dictionary<EquipmentSlots, Slot>();

        internal InventoryController(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_InventoryController);
            addressOriginal = address;
            InventoryInfoUpdateUpdateTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(ModuleEFT.radarForm.fastRandom.Next(3000, 6000));
        }

        public enum EquipmentSlots
        {
            FirstPrimaryWeapon,
            SecondPrimaryWeapon,
            Holster,
            Scabbard,
            Backpack,
            SecuredContainer,
            TacticalVest,
            ArmorVest,
            Pockets,
            Eyewear,
            FaceCover,
            Headwear,
            Earpiece,
            Dogtag,
            ArmBand,
            Compass
        }

        internal static List<EquipmentSlots> allowedSlots = new List<EquipmentSlots>{
            EquipmentSlots.FirstPrimaryWeapon,
            EquipmentSlots.SecondPrimaryWeapon,
            EquipmentSlots.Holster,
            EquipmentSlots.Backpack,
            EquipmentSlots.TacticalVest,
            EquipmentSlots.ArmorVest,
            EquipmentSlots.Pockets,
            EquipmentSlots.Headwear,
            };

        private List<EquipmentSlots> allowedWeaponSlots = new List<EquipmentSlots>{
            EquipmentSlots.FirstPrimaryWeapon,
            EquipmentSlots.SecondPrimaryWeapon,
            EquipmentSlots.Holster,
            };

        internal void GetPlayerInventory()
        {
            if (CommonHelpers.dateTimeHolder > InventoryInfoUpdateUpdateTimeLast)
            {
                InventoryInfoUpdateAllowed = true;
                InventoryInfoUpdateUpdateTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(InventoryInfoUpdateRateMsec + ModuleEFT.radarForm.fastRandom.Next(3000, 6000));
            }
            else
            {
                InventoryInfoUpdateAllowed = false;
            }

            
            if ((entityPlayer.playerType == PlayerTypeEFT.Bot || entityPlayer.playerType == PlayerTypeEFT.BotElite) && InventoryRead == true)
            {
                                return;
            }

            if (InventoryInfoUpdateAllowed == false)
            {
                                return;
            }

            if (InventoryRead)
            {
            }

            if (!Memory.IsValidPointer(address))
            {
                return;
            }

            
            var Inventory = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_InventoryController_Inventory);
            var Equipment = Memory.Read<ulong>(Inventory + ModuleEFT.offsetsEFT.Player_InventoryController_Inventory_Equipment);

                        
            var EquipmentSlotsArray = Memory.Read<ulong>(Equipment + ModuleEFT.offsetsEFT.Player_InventoryController_Inventory_Equipment_EquipmentSlotsArray);
            var EquipmentSlotsCount = Memory.Read<int>(EquipmentSlotsArray + ModuleEFT.offsetsEFT.ListClassEntryCount);

                        
            for (uint i = 0; i < EquipmentSlotsCount; i++)
            {
                var slotAddress = Memory.Read<ulong>(EquipmentSlotsArray + ModuleEFT.offsetsEFT.ArrayFirstEntry + (i * 0x8));
                
                var nameAddress = Memory.Read<ulong>(slotAddress + 0x10);
                
                var name = CommonHelpers.GetStringFromMemory_Unity(nameAddress, true);
                
                if (Enum.TryParse(name, out EquipmentSlots slotsEnum))
                {
                    if (allowedSlots.Contains(slotsEnum))
                    {
                        if (slotsDict.ContainsKey(slotsEnum))
                        {
                            slotsDict.Remove(slotsEnum);
                        }

                        slotsDict.Add(slotsEnum, new Slot(slotAddress, slotsEnum));
                    }
                }
            }

            InventoryRead = true;

            if (entityPlayer.isLocalPlayer == true)
            {
                                return;
            }

            GetPlayerWeapons();
            GetPlayerArmorClass();
                    }

        private void GetPlayerArmorClass()
        {
            if (ArmorClass != null)
            {
                return;
            }

            if (address == 0)
            {
                ArmorClass = "err";
            }

            if (entityPlayer.isLocalPlayer == true)
            {
                                return;
            }

            if ((entityPlayer.playerType == PlayerTypeEFT.Bot || entityPlayer.playerType == PlayerTypeEFT.BotElite) && WeaponRead == true)
            {
                                return;
            }

            if (InventoryInfoUpdateAllowed == false)
            {
                                return;
            }

            if (ArmorClassRead)
            {
            }

            var armorClasses = string.Empty;
            var HeadArmorClass = -1;
            var BodyArmorClass = -1;
            var TacVestArmorClass = -1;

            if (slotsDict.ContainsKey(EquipmentSlots.Headwear))
            {
                HeadArmorClass = slotsDict[EquipmentSlots.Headwear].GetArmorClass();
            }

            if (slotsDict.ContainsKey(EquipmentSlots.ArmorVest))
            {
                BodyArmorClass = slotsDict[EquipmentSlots.ArmorVest].GetArmorClass();
            }

            if (slotsDict.ContainsKey(EquipmentSlots.TacticalVest))
            {
                TacVestArmorClass = slotsDict[EquipmentSlots.TacticalVest].GetArmorClass();
            }

            ArmorClass = $"H{HeadArmorClass} A{Math.Max(BodyArmorClass, TacVestArmorClass)}";
            ArmorClassRead = true;
        }

        private void GetPlayerWeapons()
        {
            if (Weapons == null)
            {
                Weapons = "";
            }

                        if (address == 0)
            {
                                return;
            }

            if (entityPlayer.isLocalPlayer == true)
            {
                                return;
            }

            if ((entityPlayer.playerType == PlayerTypeEFT.Bot || entityPlayer.playerType == PlayerTypeEFT.BotElite) && WeaponRead == true)
            {
                                return;
            }

            if (InventoryInfoUpdateAllowed == false)
            {
                                return;
            }

            if (WeaponRead)
            {
            }

            var weapons = string.Empty;

            for (int i = 0; i < allowedWeaponSlots.Count; i++)
            {
                if (slotsDict.ContainsKey(allowedWeaponSlots[i]))
                {
                    weapons = $"{weapons} {slotsDict[allowedWeaponSlots[i]].GetItemNameInSlot()}";
                }
            }

            if (weapons.Length == 0)
            {
                weapons = "n/a";
            }

            Weapons = weapons;
            WeaponRead = true;
                    }

        private void GetContainedItem(ulong slotPointer)
        {
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

            Memory.SlowMode = true;

            for (int index = 0; index < slotsDict.Count; index++)
            {
                slotsDict.ElementAt(index).Value.GetAllItemsRecursive();
            }

            Memory.SlowMode = false;

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

        internal static string ToKMB(uint num)
        {
            if (num > 999999999 || num < -999999999)
            {
                return num.ToString("0,,,.B", CultureInfo.InvariantCulture);
            }
            else
            if (num > 999999 || num < -999999)
            {
                return num.ToString("0,,.M", CultureInfo.InvariantCulture);
            }
            else
            if (num > 999 || num < -999)
            {
                return num.ToString("0,.K", CultureInfo.InvariantCulture);
            }
            else
            {
                return num.ToString(CultureInfo.InvariantCulture);
            }
        }

        internal void SwapGear(EntityPlayer newGear)
        {
            if (newGear.isLocalPlayer)
            {
                Memory.Write(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_InventoryController, newGear.inventoryController.addressOriginal);
            }
            else
            {
                Memory.Write(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_InventoryController, newGear.inventoryController.address);
            }
        }
    }
}