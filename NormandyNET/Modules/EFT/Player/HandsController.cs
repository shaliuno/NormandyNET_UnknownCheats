using NormandyNET.Core;

namespace NormandyNET.Modules.EFT
{
    internal class HandsController
    {
        internal ulong address;
        private EntityPlayer entityPlayer;
        internal Item item;

        public HandsController(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            GetHandsController();
            GetItem();
        }

        internal void GetHandsController()
        {
            address = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_HandsController);
        }

        internal void GetItem()
        {
            item = new Item(Memory.Read<ulong>(address + 0x60));
        }

        internal void CurrentAmmoTemplate()
        {
            item.CurrentAmmoTemplateInitialSpeed();
        }
    }
}