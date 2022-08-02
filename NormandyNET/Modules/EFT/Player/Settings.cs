using NormandyNET.Core;

namespace NormandyNET.Modules.EFT
{
    internal class Settings
    {
        internal ulong address;
        private WildSpawnType? role;
        private EntityPlayer entityPlayer;

        public Settings(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.info.address + ModuleEFT.offsetsEFT.Player_Profile_Info_Settings);
        }

        internal WildSpawnType Role
        {
            get
            {
                if (role == null)
                {
                    role = (WildSpawnType)Memory.Read<int>(address + ModuleEFT.offsetsEFT.Player_Profile_Info_Settings_Role);
                }

                return (WildSpawnType)role;
            }
        }
    }
}