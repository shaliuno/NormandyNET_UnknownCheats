using NormandyNET.Core;

namespace NormandyNET.Modules.EFT
{
    internal class Physical
    {
        internal ulong address;
        internal ulong stamina;
        private EntityPlayer entityPlayer;

        public Physical(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_Physical);
            stamina = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_Physical_Stamina);
        }

        internal bool IsSprinting()
        {
            var value = Memory.Read<byte>(address + 0x14c, false);

            if (value == (byte)1)
            {
                return true;
            }

            return false;
        }

        internal void SetEncumberDisabled(bool disable)
        {
            var stateDisabled = Memory.Read<byte>(address + 0x11E) == (byte)1 ? true : false;

            if (stateDisabled != disable)
            {
                var setByte = disable == true ? (byte)1 : (byte)0;
                Memory.Write<byte>(address + 0x11E, setByte);
            }
        }
    }
}