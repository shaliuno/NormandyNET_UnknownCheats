namespace NormandyNET.Modules.DAYZ
{
    public enum TableType
    {
        Near,
        Far,
        Fast,
        Slow,
        Items
    }

    internal enum ItemPickState
    {
        NotItem,
        OnGround,
        PickedUpOrFar
    }
}