using NormandyNET.Core;
using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class SkillHack
    {
        private EntityPlayer entityPlayer;
        private bool skillDone = false;

        private Skill MagDrillsLoadSpeed;
        private Skill MagDrillsUnloadSpeed;
        private Skill StrengthBuffJumpHeightInc;
        private Skill BotReloadSpeed;
        private Skill AttentionExamine;

        internal static string Tooltip = "" +
            "Mag Load Speed\n" +
            "Mag Unload Speed\n" +
            "Jump Height";

        private ulong skills = 0;

        private DateTime TimeLast;
        private int TimeLastRateMs = 4000;

        internal SkillHack(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
            skills = Memory.Read<ulong>(entityPlayer.profile.address + ModuleEFT.offsetsEFT.Player_Profile_Skills);

            MagDrillsLoadSpeed = new Skill(Memory.Read<ulong>(skills + ModuleEFT.offsetsEFT.Skills_MagDrillsLoadSpeed));
            MagDrillsUnloadSpeed = new Skill(Memory.Read<ulong>(skills + ModuleEFT.offsetsEFT.Skills_MagDrillsUnloadSpeed));

            StrengthBuffJumpHeightInc = new Skill(Memory.Read<ulong>(skills + ModuleEFT.offsetsEFT.Skills_StrengthBuffJumpHeightInc));
            BotReloadSpeed = new Skill(Memory.Read<ulong>(skills + ModuleEFT.offsetsEFT.Skills_BotReloadSpeed));
            AttentionExamine = new Skill(Memory.Read<ulong>(skills + ModuleEFT.offsetsEFT.Skills_AttentionExamine));
        }

        private class Skill
        {
            private ulong address = 0;
            private ulong valueAddress = 0;
            private float originalValue;
            private bool originalValueStored;
            private uint offset = 0x30;

            internal Skill(ulong address)
            {
                this.address = address;
                this.valueAddress = address + offset;
            }

            internal void WriteValue(float value)
            {
                if (!originalValueStored)
                {
                    originalValue = Memory.Read<float>(valueAddress);
                    originalValueStored = true;
                }

                Memory.Write<float>(valueAddress, value);
            }

            internal void RevertValue()
            {
                if (originalValueStored)
                {
                    Memory.Write<float>(valueAddress, originalValue);
                }
            }

            internal float GetValue()
            {
                return Memory.Read<float>(valueAddress);
            }
        }

        internal void SkillsCheck()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.SkillHack.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                    Enable();
                }
            }
        }

        private void Enable()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.SkillHack.MagDrills)
            {
                MagDrillsLoadSpeed.WriteValue(85f);
                MagDrillsUnloadSpeed.WriteValue(60f);

                
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.SkillHack.SuperJump)
            {
                StrengthBuffJumpHeightInc.WriteValue(0.6f);
            }
        }
    }
}