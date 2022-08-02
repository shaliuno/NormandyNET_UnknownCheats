using NormandyNET.Core;
using System;
using Transform = NormandyNET.Modules.EFT.Objects.Components.Transform;

namespace NormandyNET.Modules.EFT
{
    internal sealed class EntityLoot : LootItem, IEquatable<EntityLoot>
    {
        internal Transform transform;
        internal bool transformPositionStandardOk = true;
        internal bool transformPositionIndicesOk = true;

        internal GameObject gameObject;
        public UnityEngine.Vector3 Position;
        public bool PositionBasicDone;
        public bool PositionContainerDone;
        public bool Airdrop;

        internal string ItemBasicName;

        internal ContainedItem containedItem;
        internal int containedDepth = 0;

        public bool? isContainer;

        public bool readContainedItem = true;

        public bool canDelete = false;
        public bool blacklist = false;
        internal bool canReadData = true;
        public bool notPresentTest = false;
        public bool notPresent = false;

        public bool updateColors = true;
        public bool updateRenderStatus = true;

        internal DateTime ExtraInfoUpdateTimeLast;
        internal bool ExtraInfoUpdateAllowed;
        internal int ExtraInfoUpdateMSec = 6000;

        internal readonly ulong lootAddress;

        public EntityLoot(ulong addr)
        {
            lootAddress = addr;
            ExtraInfoUpdateTimeLast = CommonHelpers.dateTimeHolder;
        }

        public bool Equals(EntityLoot other)
        {
            
            if (lootAddress == other.lootAddress)
            {
                return true;
            }

            return false;
        }

        public void GetData()
        {
            if (blacklist)
            {
                return;
            }

            if (notPresent)
            {
                return;
            }

                        
            if (CommonHelpers.dateTimeHolder > ExtraInfoUpdateTimeLast)
            {
                ExtraInfoUpdateAllowed = true;
                ExtraInfoUpdateTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(ExtraInfoUpdateMSec + ModuleEFT.radarForm.fastRandom.Next(ExtraInfoUpdateMSec, ExtraInfoUpdateMSec * 2));
            }
            else
            {
                ExtraInfoUpdateAllowed = false;
            }

            if (ExtraInfoUpdateAllowed == false)
            {
                return;
            }

            GetGameObject();
            GetPosition(Airdrop);

            if (Position.x == 0 && Position.y == 0 && Position.z == 0)
            {
                return;
            }

            if (!blacklist)
            {
                GetContainedItem_Item();
                GetItemOwnerContainedItem_Item();

                if (containedItem != null)
                {
                    containedItem.GetData(containedDepth);

                    if (isContainer == true)
                    {
                        if (!PositionContainerDone)
                        {
                            Position = transform.GetPositionViaIndices();
                            PositionContainerDone = true;
                        }
                    }
                }
            }

                    }

        private void GetGameObject()
        {
            
            if (gameObject == null)
            {
                var gameObjectEntry = Memory.Read<ulong>(lootAddress + ModuleEFT.offsetsEFT.GameObjectEntry);
                var gameObjectAddress = Memory.Read<ulong>(gameObjectEntry + ModuleEFT.offsetsEFT.GameObject);
                gameObject = new GameObject(gameObjectAddress);
                ItemBasicName = gameObject.GetName();

                if (ItemBasicName.Equals("Script") && ItemBasicName.Length > 3)
                {
                    
                    blacklist = true;
                }
            }

            if (transform == null && Memory.IsValidPointer(gameObject.address))
            {
                transform = gameObject.GetComponentByName<Transform>("Transform");
            }
        }

        private void GetPosition(bool force = false)
        {
            if ((!PositionBasicDone || force) && transform != null)
            {
                Position = transform.GetPosition();
                PositionBasicDone = true;
            }
        }

        private void GetContainedItem_Item()
        {
            
            if (containedItem != null)
            {
                return;
            }

            var containedItemAddress = Memory.Read<ulong>(lootAddress + ModuleEFT.offsetsEFT.Interactive_LootItem_ContainedItem, false);
            
            if (!Memory.IsValidPointer(containedItemAddress))
            {
                return;
            }

            var containedItemTemplateAddress = Memory.Read<ulong>(containedItemAddress + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate, false);
            
            if (!Memory.IsValidPointer(containedItemTemplateAddress))
            {
                return;
            }

            var containedItemTemplateNameAddress = Memory.Read<ulong>(containedItemTemplateAddress + ModuleEFT.offsetsEFT.InventoryLogic_ItemTemplate_Id);

            
            if (Memory.IsValidPointer(containedItemTemplateNameAddress))
            {
                isContainer = false;
                containedItem = new ContainedItem(this);
                containedItem.isContainer = false;
                containedItem.address = containedItemAddress;

                containedItem.parentAddress = lootAddress;
                containedItem.parentItemBasicName = ItemBasicName;
            }

                    }

        private void GetItemOwnerContainedItem_Item()
        {
            
            if (containedItem != null)
            {
                return;
            }

            var itemOwnerAddress = Memory.Read<ulong>(lootAddress + ModuleEFT.offsetsEFT.Interactive_LootableContainer_ItemOwner, false);
            
            if (!Memory.IsValidPointer(itemOwnerAddress))
            {
                return;
            }

            var itemOwnerContainedItemAddress = Memory.Read<ulong>(itemOwnerAddress + ModuleEFT.offsetsEFT.Interactive_LootableContainer_ItemOwner_ContainedItem);
            
            if (!Memory.IsValidPointer(itemOwnerContainedItemAddress))
            {
                return;
            }

            var itemOwnerContainedItemTemplateAddress = Memory.Read<ulong>(itemOwnerContainedItemAddress + ModuleEFT.offsetsEFT.InventoryLogic_Item_ItemTemplate);
            
            if (!Memory.IsValidPointer(itemOwnerContainedItemTemplateAddress))
            {
                return;
            }

            var itemOwnerContainedItemTemplateNameAddress = Memory.Read<ulong>(itemOwnerContainedItemTemplateAddress + ModuleEFT.offsetsEFT.InventoryLogic_ItemTemplate_Id);

            
            if (Memory.IsValidPointer(itemOwnerContainedItemTemplateNameAddress))
            {
                isContainer = true;
                containedItem = new ContainedItem(this);
                containedItem.isContainer = true;
                containedItem.address = itemOwnerContainedItemAddress;

                containedItem.parentAddress = lootAddress;
                containedItem.parentItemBasicName = ItemBasicName;
            }

                    }

        internal bool CanUpdateLootOverTime()
        {
            if (CommonHelpers.dateTimeHolder > ExtraInfoUpdateTimeLast)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void GenerateRenderItem()
        {
            if (containedItem != null)
            {
                containedItem.GenerateRenderItem();
            }
        }

        internal void GenerateRenderItemOverlay()
        {
            if (containedItem != null)
            {
                containedItem.GenerateRenderItemOverlay();
            }
        }
    }
}