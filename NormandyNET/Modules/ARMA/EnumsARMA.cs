namespace NormandyNET.Modules.ARMA
{
    public enum TableType
    {
        None,
        Bullet,
        Near,
        Far,
        FarFar,
        FarFarFar,
        BuildingLoot,
    }

    public enum Side
    {
        NONE = -1,
        BLUEFOR,
        OPFOR,
        INDI,
        CIV
    }

    internal enum ItemPickState
    {
        NotItem,
        OnGround,
        PickedUpOrFar
    }
}