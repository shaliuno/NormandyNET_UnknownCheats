using NormandyNET.Core;
using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class NoRecoil
    {
        private EntityPlayer entityPlayer;

        private DateTime TimeLast;
        private DateTime ExtrasTimeLast;
        private int TimeLastRateMs = 300;

        public NoRecoil(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.NoRecoil.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                    Enable();
                }
            }
        }

        internal void Enable()
        {
            if (CommonHelpers.dateTimeHolder > ExtrasTimeLast)
            {
                ExtrasTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);

                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.NoRecoil.StreamerSafe == true)
                {
                    ApplyNoRecoilStreamSafe(ModuleEFT.settingsForm.settingsJson.MemoryWriting.NoRecoil.Intensity);
                }

                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.NoRecoil.StreamerSafe == false)
                {
                    NoSwayDo(0f);
                }
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.NoRecoil.StreamerSafe == false)
            {
                Memory.Write<int>(entityPlayer.proceduralWeaponAnimation.address + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_Mask, 1);
            }
        }

        private void ApplyNoRecoilStreamSafe(float intensity)
        {
            var currentIntensity = Memory.Read<float>(entityPlayer.proceduralWeaponAnimation.shotEffector + ModuleEFT.offsetsEFT.Player_ShotEffector_Intensity);

            if (currentIntensity != intensity)
            {
                Console.WriteLine($"ApplyNoRecoil {intensity}");
                Memory.Write<float>(entityPlayer.proceduralWeaponAnimation.shotEffector + ModuleEFT.offsetsEFT.Player_ShotEffector_Intensity, intensity);
            }
        }

        internal void NoSwayDo(float intensity)
        {
            var value = Memory.Read<float>(entityPlayer.proceduralWeaponAnimation.breathEffector + ModuleEFT.offsetsEFT.Player_BreathEffector_Intensity);

            if (value != intensity)
            {
                Memory.Write(entityPlayer.proceduralWeaponAnimation.breathEffector + ModuleEFT.offsetsEFT.Player_BreathEffector_Intensity, intensity);
            }
        }

        internal void NoSwayUndo()
        {
            Memory.Write(entityPlayer.proceduralWeaponAnimation.breathEffector + ModuleEFT.offsetsEFT.Player_BreathEffector_NoSway, 1f);
        }
    }
}