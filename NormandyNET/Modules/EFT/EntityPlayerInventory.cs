using System;

namespace NormandyNET.Modules.EFT
{
    [Serializable]
    public sealed class EntityPlayerInventory
    {
        public int ChannelID;
        public string TotalValue = string.Empty;
        public string Weapons = string.Empty;
        public byte[] RawData;
        public string PriorityItems = string.Empty;
    }
}