using NormandyNET.Core;
using static NormandyNET.Modules.EFT.InventoryController;

namespace NormandyNET.Modules.EFT
{
    internal class Slot
    {
        internal ulong address;
        private bool debugLog = false;
        internal Item item;
        internal EquipmentSlots equipmentSlot;
        private ulong chamberSlot;

        public Slot(ulong slotAddress, EquipmentSlots equipmentSlot)
        {
            address = slotAddress;
            this.equipmentSlot = equipmentSlot;
            item = new Item(this);
        }

        public Slot(ulong chamberSlot)
        {
            this.address = chamberSlot;
            item = new Item(this);
        }

        internal void GetAllItemsRecursive()
        {
            if (Memory.IsValidPointer(address))
            {
                item.GetItemsRecursive();
            }
        }

        internal string GetItemNameInSlot()
        {
            if (Memory.IsValidPointer(address))
            {
                item.GetDataAsSlot();
                var weaponName = item.GetItemName();

                if (weaponName.Length > 1)
                {
                    return weaponName;
                }
            }

            return string.Empty;
        }

        internal int GetArmorClass()
        {
            if (Memory.IsValidPointer(address))
            {
                item.GetDataAsSlot();
                return item.ArmorClass;
            }
            return -1;
        }

        internal void NoWeaponMalfunctions(bool allowMalfunctions)
        {
            if (Memory.IsValidPointer(address))
            {
                item.NoWeaponMalfunctions(allowMalfunctions);
            }
        }

        internal void RemoveArmorMovementPenalties(EquipmentSlots equipmentSlot)
        {
            if (Memory.IsValidPointer(address))
            {
                item.RemoveArmorMovementPenalties(equipmentSlot);
            }
        }

        internal void AdjustMagLoadUnloadModifiers(float modifier)
        {
            if (Memory.IsValidPointer(address))
            {
                item.AdjustMagLoadUnloadModifiers(modifier);
            }
        }

        internal void SetSingleFireRate(int fireRate)
        {
            if (Memory.IsValidPointer(address))
            {
                item.SetSingleFireRate(fireRate);
            }
        }

        internal void SetFireRate(int fireRate)
        {
            if (Memory.IsValidPointer(address))
            {
                item.SetFireRate(fireRate);
            }
        }
    }
}