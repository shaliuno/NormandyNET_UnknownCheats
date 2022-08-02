public enum Movements
{
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down,
}

public enum ElevationType
{
    None,
    Absolute,
    Relative
}

public enum WindowStyleEnum
{
    FullScreen,
    Standalone,
    OBS,
    Moonlight
}

public enum BodyParts
{
    Head,
    Chest,
    Stomach,
    LeftArm,
    RightArm,
    LeftLeg,
    RightLeg
}

public enum RenderLayers
{
    Default,
    LootPriority0,
    LootPriority1,
    LootPriority2,
    LootPriority3,
    LootPriority4,
    LootPriority5,
    DeadBodies,
    Misc,
    PlayersPriorityLow,
    PlayersPriorityMedium,
    PlayersPriorityHigh,
    You,
    OSD
}

public enum IconPositionTexture
{
    none = 0,
    player = 1,
    player_dead,
    npc,
    npc_dead,
    vehicle,
    vehicle_broken,
    loot,
    animal,
    helicrash,
    tentbox,
    parachute,
    money,
    house,
    statictech,
    pin,
    unknown
}

internal enum FontSizes
{
    misc = 15,
    name = 16,
    large = 18,
    large2 = 26,
}