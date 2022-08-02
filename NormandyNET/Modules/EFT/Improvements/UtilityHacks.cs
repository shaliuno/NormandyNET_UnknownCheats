using NormandyNET.Core;
using NormandyNET.Modules.EFT.Objects;
using System;
using static NormandyNET.Modules.EFT.InventoryController;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class UtilityHacks
    {
        private EntityPlayer entityPlayer;

        private DateTime TimeLast;
        private int TimeLastRateMs = 5000;

        private DateTime TimeLastFast;
        private int TimeLastRateFastMs = 500;
        private FPSCamera opticCamera;
        private FPSCamera fpsCamera;

        internal static string Tooltip = "No weapon jam\nNo weapon overheat\nNo armor movement penalties\n" +
            "(examine item, if penalties gone, put it off and on)";

        private float magLoadUnloadModifier = -10f;

        internal UtilityHacks(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check(bool allowMalfunctions)
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.UtilityHack.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                    NoWeaponMalfunctions(allowMalfunctions);
                    RemoveArmorMovementPenalties();
                    PreSprintAcceleration();
                    AdjustMagLoadUnloadModifiers(magLoadUnloadModifier);
                }

                if (CommonHelpers.dateTimeHolder > TimeLastFast)
                {
                    TimeLastFast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateFastMs);
                    AdjustRotationClamp();
                }
            }
        }

        private void SetEncumberDisabled(bool disable)
        {
            if (entityPlayer.physical != null)
            {
                entityPlayer.physical.SetEncumberDisabled(disable);
            }
        }

        internal bool IsSprinting()
        {
            if (entityPlayer.physical == null)
            {
                return false;
            }

            return entityPlayer.physical.IsSprinting();
        }

        private void AdjustRotationClamp()
        {
            if (IsSprinting())
            {
                if (entityPlayer.movementContext != null)
                {
                    entityPlayer.movementContext.AdjustRotationClamp();
                }
            }
        }

        private void AdjustMagLoadUnloadModifiers(float modifier)
        {
            if (entityPlayer.inventoryController.slotsDict.ContainsKey(EquipmentSlots.TacticalVest))
            {
                entityPlayer.inventoryController.slotsDict[EquipmentSlots.TacticalVest].AdjustMagLoadUnloadModifiers(modifier);
            }
        }

        public enum EPlayerState : byte
        {
            None,
            Idle,
            ProneIdle,
            ProneMove,
            Run,
            Sprint,
            Jump,
            FallDown,
            Transition,
            BreachDoor,
            Loot,
            Pickup,
            Open,
            Close,
            Unlock,
            Sidestep,
            DoorInteraction,
            Approach,
            Prone2Stand,
            Transit2Prone,
            Plant,
            Stationary,
            Roll
        }

        private void TestProne(bool prone)
        {
            var MovementState = Memory.Read<ulong>(entityPlayer.movementContext.address + 0xB8);

            var state = (EPlayerState)Memory.Read<byte>(MovementState + 0x2C);

            if (prone)
            {
                Memory.Write<byte>(MovementState + 0x2C, (byte)EPlayerState.ProneIdle);
            }
            else
            {
                Memory.Write<byte>(MovementState + 0x2C, (byte)EPlayerState.Idle);
            }
        }

        private void PreSprintAcceleration()
        {
            var SprintAcceleration = Memory.Read<float>(entityPlayer.physical.address + ModuleEFT.offsetsEFT.Player_Physical_SprintAcceleration);
            var PreSprintAcceleration = Memory.Read<float>(entityPlayer.physical.address + ModuleEFT.offsetsEFT.Player_Physical_PreSprintAcceleration);
            var SprintAccelerationDefault = 1f;
            var PreSprintAccelerationDefault = 3f;

            if (SprintAcceleration != SprintAccelerationDefault)
            {
                Memory.Write<float>(entityPlayer.physical.address + ModuleEFT.offsetsEFT.Player_Physical_SprintAcceleration, SprintAccelerationDefault);
            }

            if (PreSprintAcceleration != PreSprintAccelerationDefault)
            {
                Memory.Write<float>(entityPlayer.physical.address + ModuleEFT.offsetsEFT.Player_Physical_PreSprintAcceleration, PreSprintAccelerationDefault);
            }
        }

        private void RemoveArmorMovementPenalties()
        {
            if (entityPlayer.inventoryController.slotsDict.ContainsKey(EquipmentSlots.Headwear))
            {
                entityPlayer.inventoryController.slotsDict[EquipmentSlots.Headwear].RemoveArmorMovementPenalties(EquipmentSlots.Headwear);
            }

            if (entityPlayer.inventoryController.slotsDict.ContainsKey(EquipmentSlots.ArmorVest))
            {
                entityPlayer.inventoryController.slotsDict[EquipmentSlots.ArmorVest].RemoveArmorMovementPenalties(EquipmentSlots.ArmorVest);
            }

            if (entityPlayer.inventoryController.slotsDict.ContainsKey(EquipmentSlots.TacticalVest))
            {
                entityPlayer.inventoryController.slotsDict[EquipmentSlots.TacticalVest].RemoveArmorMovementPenalties(EquipmentSlots.TacticalVest);
            }
        }

        private void NoWeaponMalfunctions(bool allowMalfunctions)
        {
            if (entityPlayer.inventoryController.slotsDict.ContainsKey(EquipmentSlots.FirstPrimaryWeapon))
            {
                entityPlayer.inventoryController.slotsDict[EquipmentSlots.FirstPrimaryWeapon].NoWeaponMalfunctions(allowMalfunctions);
            }

            if (entityPlayer.inventoryController.slotsDict.ContainsKey(EquipmentSlots.SecondPrimaryWeapon))
            {
                entityPlayer.inventoryController.slotsDict[EquipmentSlots.SecondPrimaryWeapon].NoWeaponMalfunctions(allowMalfunctions);
            }

            if (entityPlayer.inventoryController.slotsDict.ContainsKey(EquipmentSlots.Holster))
            {
                entityPlayer.inventoryController.slotsDict[EquipmentSlots.Holster].NoWeaponMalfunctions(allowMalfunctions);
            }
        }
    }
}