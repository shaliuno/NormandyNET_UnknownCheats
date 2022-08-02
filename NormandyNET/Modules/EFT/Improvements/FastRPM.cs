using NormandyNET.Core;
using System;
using static NormandyNET.Modules.EFT.InventoryController;

namespace NormandyNET.Modules.EFT.Improvements
{
    [Flags]
    public enum EFireMode : byte
    {
        fullauto = 0,
        single = 1,
        doublet = 2,
        burst = 3,
        doubleaction = 4
    }

    internal class FastRPM
    {
        private ImprovementsController improvementsController;
        private EntityPlayer entityPlayer;
        private byte flags;

        private int fireRateMax = 1200;
        private int singleFireRateMax = 700;

        private int currentFireRate = -1;
        private int currentSingleFireRate = -1;

        private DateTime TimeLast;
        private int TimeLastRateMs = 5000;

        internal static string Tooltip = "" +
           "Firerate and single fire rate will be applied over time\nand set to highest firerate gun in EFT." +
            "\nOSD will show info.";

        public FastRPM(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastRPM.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                    AdjustFireRate();
                }
            }
        }

        private void AdjustFireRate()
        {
            entityPlayer.handsController.GetHandsController();
            entityPlayer.handsController.GetItem();

            currentFireRate = entityPlayer.handsController.item.FireRate();
            currentSingleFireRate = entityPlayer.handsController.item.SingleFireRate();

            if (entityPlayer.inventoryController.slotsDict.ContainsKey(EquipmentSlots.FirstPrimaryWeapon))
            {
                if (entityPlayer.inventoryController.slotsDict[EquipmentSlots.FirstPrimaryWeapon].item.address ==
                    entityPlayer.handsController.item.address)
                {
                    if (currentFireRate != fireRateMax)
                    {
                        entityPlayer.inventoryController.slotsDict[EquipmentSlots.FirstPrimaryWeapon].SetFireRate(fireRateMax);
                    }
                    if (currentSingleFireRate != singleFireRateMax)
                    {
                        entityPlayer.inventoryController.slotsDict[EquipmentSlots.FirstPrimaryWeapon].SetSingleFireRate(singleFireRateMax);
                    }
                }
            }

            if (entityPlayer.inventoryController.slotsDict.ContainsKey(EquipmentSlots.SecondPrimaryWeapon))
            {
                if (entityPlayer.inventoryController.slotsDict[EquipmentSlots.SecondPrimaryWeapon].item.address ==
                  entityPlayer.handsController.item.address)
                {
                    if (currentFireRate != fireRateMax)
                    {
                        entityPlayer.inventoryController.slotsDict[EquipmentSlots.SecondPrimaryWeapon].SetFireRate(fireRateMax);
                    }
                    if (currentSingleFireRate != singleFireRateMax)
                    {
                        entityPlayer.inventoryController.slotsDict[EquipmentSlots.SecondPrimaryWeapon].SetSingleFireRate(singleFireRateMax);
                    }
                }
            }
        }

        internal void DoFullAuto()
        {
        }

        private void OverrideAvailableFireModes()
        {
            entityPlayer.handsController.item.OverrideAvailableFireModes();
        }

        private void GetAvailableFireModes()
        {
            entityPlayer.handsController.item.GetAvailableFireModes();
        }

        private float FireRate()
        {
            return entityPlayer.handsController.item.FireRate();
        }

        internal bool ShowOSDWarning(out string text)
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.FastRPM.Enabled)
            {
                if (currentFireRate == -1 && currentSingleFireRate == -1)
                {
                    text = "";
                    return false;
                }

                if (currentFireRate != fireRateMax && currentSingleFireRate != singleFireRateMax)
                {
                    text = $"AutomaticGun\nFirerate: Pending change to {fireRateMax}, SingleRate: Pending change to {singleFireRateMax},";
                    return true;
                }

                if (currentFireRate == fireRateMax && currentSingleFireRate == singleFireRateMax)
                {
                    text = $"AutomaticGun\nFirerate: {fireRateMax}, SingleRate: {singleFireRateMax}";
                    return true;
                }
            }

            text = "";
            return false;
        }

        private void SetSingleFireRate(int fireRateToSet)
        {
            if (entityPlayer.handsController.item.SingleFireRate() != fireRateToSet)
            {
                entityPlayer.handsController.item.SetSingleFireRate(fireRateToSet);
            }
        }

        private void SetIsBoltAction(bool isBoltAction)
        {
            if (entityPlayer.handsController.item.IsBoltAction() != isBoltAction)
            {
                entityPlayer.handsController.item.SetIsBoltAction(isBoltAction);
            }
        }

        private void SetFlag(ulong address, uint offset, EFireMode flag, bool setFlag)
        {
            GetFlags(address, offset);

            byte flagToWrite;

            if (!setFlag)
            {
                flagToWrite = (byte)(flags | (byte)flag);
            }
            else
            {
                flagToWrite = (byte)(flags & ~(byte)flag);
            }

            Memory.Write(address + offset, flagToWrite);
        }

        private void GetFlags(ulong address, uint offset)
        {
            flags = Memory.Read<byte>(address + offset);
        }
    }
}