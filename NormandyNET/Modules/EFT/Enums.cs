using System;

namespace NormandyNET.Modules.EFT
{
    public enum EBodyPart
    {
        Head,
        Chest,
        Stomach,
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg,
        Common
    }

    internal enum Tilt
    {
        None = 0,
        Left = -5,
        Right = 5,
    }

    internal enum LeanAimState
    {
        None,
        Left,
        Right,
    }

    [Flags]
    public enum EPhysicalCondition
    {
        None = 0,
        OnPainkillers = 1,
        LeftLegDamaged = 2,
        RightLegDamaged = 4,
        ProneDisabled = 8,
        LeftArmDamaged = 16,
        RightArmDamaged = 32,
        Tremor = 64,
        UsingMeds = 128,
        HealingLegs = 256,
        JumpDisabled = 512,
        SprintDisabled = 1024,
        ProneMovementDisabled = 2048
    }

    public enum WildSpawnType
    {
        marksman = 1,
        assault = 2,
        bossTest = 4,
        bossBully = 8,
        followerTest = 16,
        followerBully = 32,
        bossKilla = 64,
        bossKojaniy = 128,
        followerKojaniy = 256,
        pmcBot = 512,
        cursedAssault = 1024,
        bossGluhar = 2048,
        followerGluharAssault = 4096,
        followerGluharSecurity = 8192,
        followerGluharScout = 16384,
        followerGluharSnipe = 32768,
        followerSanitar = 65536,
        bossSanitar = 131072,
        test = 262144,
        assaultGroup = 524288,
        sectantWarrior = 1048576,
        sectantPriest = 2097152,
        bossTagilla = 4194304,
        followerTagilla = 8388608,
        exUsec = 16777216,
        gifter = 33554432,
        bossKnight = 67108864,
        followerBigPipe = 134217728,
        followerBirdEye = 268435456
    }

    public enum Bone
    {
        HumanNone = -1,
        HumanBase = 0,
        HumanPelvis = 14,
        HumanLThigh1 = 15,
        HumanLThigh2 = 16,
        HumanLCalf = 17,
        HumanLFoot = 18,
        HumanLToe = 19,
        HumanRThigh1 = 20,
        HumanRThigh2 = 21,
        HumanRCalf = 22,
        HumanRFoot = 23,
        HumanRToe = 24,
        HumanSpine1 = 29,
        HumanSpine2 = 36,
        HumanSpine3 = 37,
        HumanLCollarbone = 89,
        HumanLUpperarm = 90,
        HumanLForearm1 = 91,
        HumanLForearm2 = 92,
        HumanLForearm3 = 93,
        HumanLPalm = 94,
        HumanRCollarbone = 110,
        HumanRUpperarm = 111,
        HumanRForearm1 = 112,
        HumanRForearm2 = 113,
        HumanRForearm3 = 114,
        HumanRPalm = 115,
        HumanNeck = 132,
        HumanHead = 133
    }

    public enum BoneReadable
    {
        None = -1,
        Base = 0,
        Pelvis = 14,
        LThigh1 = 15,
        LThigh2 = 16,
        LCalf = 17,
        LFoot = 18,
        LToe = 19,
        RThigh1 = 20,
        RThigh2 = 21,
        RCalf = 22,
        RFoot = 23,
        RToe = 24,
        Spine1 = 29,
        Spine2 = 36,
        Spine3 = 37,
        LCollarbone = 89,
        LUpperarm = 90,
        LForearm1 = 91,
        LForearm2 = 92,
        LForearm3 = 93,
        LPalm = 94,
        RCollarbone = 110,
        RUpperarm = 111,
        RForearm1 = 112,
        RForearm2 = 113,
        RForearm3 = 114,
        RPalm = 115,
        Neck = 132,
        Head = 133
    }

    public enum BodyPartsMaxValues
    {
        Head = 35,
        Chest = 80,
        Stomach = 70,
        LeftArm = 60,
        RightArm = 60,
        LeftLeg = 65,
        RightLeg = 65
    }

    public enum Pose
    {
        Stand,
        Crouch,
        Prone
    }

    public enum EPlayerSide
    {
        Usec = 1,
        Bear = 2,
        Savage = 4
    }

    public enum PlayerTypeEFT
    {
        Player,
        BotElite,
        BotHuman,
        Bot
    }

    [Flags]
    public enum EMemberCategory
    {
        Default = 0,
        Developer = 1,
        UniqueId = 2,
        Trader = 4,
        Group = 8,
        System = 16,
        ChatModerator = 32,
        ChatModeratorWithPermanentBan = 64,
        UnitTest = 128,
        Sherpa = 256,
        Emissary = 512
    }
}