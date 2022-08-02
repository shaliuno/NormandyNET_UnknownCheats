using NormandyNET.Core;
using System;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class LeanHack
    {
        public int? flags;
        internal bool blindFireChanged = false;
        internal bool sideFireChanged = false;
        private bool blindFireOn;
        private EntityPlayer entityPlayer;
        private DateTime ExtrasTimeLast;
        private bool sideFireOn;
        private DateTime TimeLast;
        private int TimeLastRateMs = 200;
        private LeanAimState leanAimState;

        private bool disabled = false;

        internal LeanHack(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
        }

        internal void Check()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.LeanHack.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                    Enable();
                    disabled = false;
                }
            }
            else
            {
                if (disabled == false)
                {
                    Disable();
                    disabled = true;
                }
            }
        }

        internal bool ShowOSDWarning(out string text)
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.LeanHack.Enabled)
            {
                text = $"LeanHack\n Leaning State: {leanAimState}";
                return true;
            }

            text = "";
            return false;
        }

        private void Disable()
        {
            if (leanAimState != LeanAimState.None)
            {
                leanAimState = LeanAimState.None;
                SetValues(0f, 8f);
            }
        }

        private void SetValues(float PositionZeroSum, float CameraSmoothOut)
        {
            Memory.Write<float>(entityPlayer.proceduralWeaponAnimation.address + 0x2A0, PositionZeroSum);

            Memory.Write<float>(entityPlayer.proceduralWeaponAnimation.address + 0x208, CameraSmoothOut);
        }

        private void Enable()
        {
            GetLeanState();
        }

        private bool IsBlindFire()
        {
            return entityPlayer.proceduralWeaponAnimation.IsBlindFire();
        }

        private bool IsSideFire()
        {
            return entityPlayer.proceduralWeaponAnimation.IsSideFire();
        }

        private void GetLeanState()
        {
            if (IsSideFire())
            {
                var tilt = (Tilt)entityPlayer.movementContext.GetTilt();

                if (tilt == Tilt.Left)
                {
                    leanAimState = LeanAimState.Left;
                    SetValues(-(ModuleEFT.settingsForm.settingsJson.MemoryWriting.LeanHack.Distance), 32f);
                    return;
                }

                if (tilt == Tilt.Right)
                {
                    leanAimState = LeanAimState.Right;
                    SetValues(ModuleEFT.settingsForm.settingsJson.MemoryWriting.LeanHack.Distance, 32f);
                    return;
                }

                return;
            }

            if (IsBlindFire())
            {
                leanAimState = LeanAimState.None;
                SetValues(0f, 8f);
                return;
            }
        }
    }
}