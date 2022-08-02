namespace NormandyNET.Modules.EFT
{
    internal class LootableContainer
    {
        internal ulong address;
        internal EntityLoot entityLoot;

        private bool debugLog = false;

        public LootableContainer(EntityLoot entityLoot)
        {
            this.entityLoot = entityLoot;
            address = entityLoot.lootAddress;
                        GetData();
        }

        private void GetData()
        {
        }
    }
}