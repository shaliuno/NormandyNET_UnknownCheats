using NormandyNET.Core;

namespace NormandyNET.Modules.EFT
{
    internal class Info
    {
        internal ulong address;
        private EntityPlayer entityPlayer;

        internal int RegistrationDate;
        internal EPlayerSide Side;
        internal EMemberCategory MemberCategory;
        internal string GroupID;
        internal string Nickname;
        internal string EntryPoint;
        internal int Level;
        internal bool IsStreamerModeAvailable;

        public Info(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.profile.address + ModuleEFT.offsetsEFT.Player_Profile_Info);
            GetStaticData();
        }

        private void GetStaticData()
        {
            Nickname = GetNickname();
            GroupID = GetGroupID();
            MemberCategory = GetMemberCategory();
            Side = GetPlayerSide();
            RegistrationDate = GetRegistrationDate();
            Level = GetPlayerLevel();
            EntryPoint = GetEntryPoint();
            IsStreamerModeAvailable = GetIsStreamerModeAvailable();
        }

        private bool GetIsStreamerModeAvailable()
        {
            return Memory.Read<byte>(address + 0x30) == (byte)1 ? true : false;
        }

        private string GetGroupID()
        {
            var playerGroup = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_Profile_Info_GroupID, false);

            GroupID = "n/a";

            if (Memory.IsValidPointer(playerGroup))
            {
                GroupID = CommonHelpers.GetStringFromMemory_Unity(playerGroup, true);
            }

            return GroupID;
        }

        private string GetEntryPoint()
        {
            return CommonHelpers.GetStringFromMemory_Unity(Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_Profile_Info_EntryPoint), true).ToLower();
        }

        private string GetNickname()
        {
            return CommonHelpers.GetStringFromMemory_Unity(Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_Profile_Info_Nickname), true);
        }

        private EMemberCategory GetMemberCategory()
        {
            return (EMemberCategory)Memory.Read<int>(address + ModuleEFT.offsetsEFT.Player_Profile_Info_MemberCategory);
        }

        private EPlayerSide GetPlayerSide()
        {
            return (EPlayerSide)Memory.Read<int>(address + ModuleEFT.offsetsEFT.Player_Profile_Info_Side);
        }

        private int GetRegistrationDate()
        {
            return Memory.Read<int>(address + ModuleEFT.offsetsEFT.Player_Profile_Info_RegistrationDate);
        }

        private int GetPlayerLevel()
        {
            return EFTHelpers.GetLevelByExperience(Memory.Read<int>(address + ModuleEFT.offsetsEFT.Player_Profile_Info_Experience));
        }
    }
}