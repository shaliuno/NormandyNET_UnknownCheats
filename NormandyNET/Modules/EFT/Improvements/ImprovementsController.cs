using NormandyNET.Modules.EFT.Objects;
using System;
using System.Text;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class ImprovementsController
    {
        internal Aimbot aimbot;
        internal AlwaysSprint alwaysSprint;
        internal AlwaysSprintAltMode alwaysSprintAltMode;
        internal FastRPM fastRPM;
        internal EntityPlayer entityPlayer;
        internal FastReload fastReload;
        internal FastRunning fastRunning;
        internal FlyHack flyHack;
        internal LootThroughWalls lootThroughWalls;
        internal LeanHack leanHack;
        internal NoRecoil noRecoil;
        internal InstantADS instantADS;
        internal NoVisor noVisor;
        internal PinkDudes pinkDudes;
        internal SkillHack skillHack;
        internal UnlimitedStamina unlimitedStamina;
        internal UtilityHacks utilityHacks;
        public bool adsConfirmed;
        private DateTime ExtrasTimeLast;
        private DateTime TimeLast;
        private int TimeLastRateMs = 1000;
        internal StringBuilder osdText = new StringBuilder();

        internal ImprovementsController(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            PinkDudes.emergencyDepink = false;

            if (alwaysSprint == null) { alwaysSprint = new AlwaysSprint(this); }
            if (alwaysSprintAltMode == null) { alwaysSprintAltMode = new AlwaysSprintAltMode(this); }
            if (fastRPM == null) { fastRPM = new FastRPM(this); }
            if (fastReload == null) { fastReload = new FastReload(this); }
            if (fastRunning == null) { fastRunning = new FastRunning(this); }
            if (flyHack == null) { flyHack = new FlyHack(this); }
            if (lootThroughWalls == null) { lootThroughWalls = new LootThroughWalls(this); }
            if (leanHack == null) { leanHack = new LeanHack(this); }
            if (noRecoil == null) { noRecoil = new NoRecoil(this); }
            if (instantADS == null) { instantADS = new InstantADS(this); }
            if (noVisor == null) { noVisor = new NoVisor(this); }
            if (skillHack == null) { skillHack = new SkillHack(this); }
            if (unlimitedStamina == null) { unlimitedStamina = new UnlimitedStamina(this); }
            if (utilityHacks == null) { utilityHacks = new UtilityHacks(this); }

            if (aimbot == null) { aimbot = new Aimbot(this); }

            if (pinkDudes == null) { pinkDudes = new PinkDudes(this); }
        }

        internal void Check()
        {
            if (!ModuleEFT.settingsForm.settingsJson.MemoryWriting.DisclaimerAgreed)
            {
                return;
            }

            if (entityPlayer.localEntity != null && entityPlayer.localEntity.improvementsController.adsConfirmed == false)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                    entityPlayer.localEntity.improvementsController.adsConfirmed = (entityPlayer.localEntity.proceduralWeaponAnimation != null && entityPlayer.localEntity.proceduralWeaponAnimation.IsAiming());
                }

                return;
            }

            if (entityPlayer.isLocalPlayer)
            {
                aimbot.DoAimBot();
                alwaysSprint?.Check();
                alwaysSprintAltMode?.Check();
                fastRPM.Check();
                fastReload?.Check();
                fastRunning?.Check();
                flyHack?.Check();
                lootThroughWalls?.Check();
                leanHack?.Check();
                noRecoil?.Check();
                instantADS?.Check();
                noVisor?.Check();
                skillHack?.SkillsCheck();
                TimeScale.Check();
                unlimitedStamina?.Check();
                utilityHacks.Check(false);
            }

            if (!entityPlayer.isLocalPlayer)
            {
                pinkDudes?.Check();
            }
        }

        internal bool ShowOSDWarning(out string str)
        {
            if (!ModuleEFT.settingsForm.settingsJson.MemoryWriting.DisclaimerAgreed)
            {
                str = "";
                return false;
            }

            var localplayer = ReaderEFT.GetLocalPlayer();
            osdText.Clear();

            if (localplayer == null)
            {
                str = osdText.ToString();
                return false;
            }

            if (!adsConfirmed)
            {
                osdText.Append("Please aim down sight\nto activate write memory features in raid.").AppendLine();
                str = osdText.ToString();
                return true;
            }

            if (TimeScale.working)
            {
                osdText.Append($"Timescale On").AppendLine();
            }

            if (localplayer.movementContext != null && localplayer.improvementsController.flyHack.ShowOSDWarning(out string text))
            {
                osdText.Append($"{text}").AppendLine();
            }

            if (localplayer.improvementsController.alwaysSprint != null && localplayer.improvementsController.alwaysSprint.ShowOSDWarning(out string textAlwaysSprint))
            {
                osdText.Append($"{textAlwaysSprint}").AppendLine();
            }

            if (localplayer.improvementsController.leanHack != null && localplayer.improvementsController.leanHack.ShowOSDWarning(out string textLeanHack))
            {
                osdText.Append($"{textLeanHack}").AppendLine();
            }

            if (localplayer.improvementsController.fastReload != null && localplayer.improvementsController.fastReload.ShowOSDWarning(out string textFastReload))
            {
                osdText.Append($"{textFastReload}").AppendLine();
            }

            if (localplayer.improvementsController.fastRPM != null && localplayer.improvementsController.fastRPM.ShowOSDWarning(out string textAutomaticGun))
            {
                osdText.Append($"{textAutomaticGun}").AppendLine();
            }

            str = osdText.ToString();
            return true;
        }
    }
}