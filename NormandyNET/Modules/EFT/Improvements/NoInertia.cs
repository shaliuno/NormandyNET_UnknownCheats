using NormandyNET.Core;
using System;
using System.Numerics;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class NoInertia
    {
        private Vector2 ExitMovementStateSpeedThreshold = new Vector2(0.02f, 0.02f);
        private Vector2 SpeedLimitAfterFallMin = new Vector2(0f, 1f);
        private Vector2 SpeedLimitAfterFallMax = new Vector2(4f, 0f);
        private Vector2 SpeedLimitDurationMin = new Vector2(0.3f, 0f);
        private Vector2 SpeedLimitDurationMax = new Vector2(2f, 1f);
        private Vector2 WalkInertia = new Vector2(0.05f, 0.5f);
        private Vector2 SideTime = new Vector2(2f, 2f);
        private Vector2 SpeedInertiaAfterJump = new Vector2(1f, 1.4f);
        private float PenaltyPower = 1.12f;
        private float MoveTime = 0.1f;
        private float MinDirectionBlendTime = 0.1f;
        private float FallThreshold = 0.15f;
        private float InertiaLimitsStep = 0.3f;
        private float BaseJumpPenalty = 0.15f;
        private float BaseJumpPenaltyDuration = 0.3f;
        private float DurationPower = 0f;
        private float SuddenChangesSmoothness = 0f;

        private bool applied;
        private ulong chainToInertia;

        private DateTime TimeLast;
        private int TimeLastRateMs = 1500;

        public NoInertia(ulong chainToInertia)
        {
            this.chainToInertia = chainToInertia;
        }

        internal void Check()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.NoInertia.Enabled)
            {
                if (CommonHelpers.dateTimeHolder > TimeLast)
                {
                    TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(4000);
                    Enable();
                }
            }
        }

        private void Enable()
        {
            if (Memory.IsValidPointer(GameObjectManager.gameObjectManagerAddress) && Memory.IsValidPointer(ModuleEFT.readerEFT.mainApplication.mainApplicationComponent.component))
            {
                Console.WriteLine($"Enable");

                {
                    PrintSettings();

                    //RemoveInertia(); // this mush makes no sense

                    applied = true;
                }
            }
        }

        private void Disable()
        {
            if (Memory.IsValidPointer(GameObjectManager.gameObjectManagerAddress) && Memory.IsValidPointer(ModuleEFT.readerEFT.mainApplication.mainApplicationComponent.component))
            {
                if (applied)
                {
                    RevertOriginalSettings();
                    PrintSettings();

                    applied = false;
                }
            }
        }

        internal void PrintSettings()
        {
            Console.WriteLine($"PrintSettings");

            Console.WriteLine($"PenaltyPower {Memory.Read<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_PenaltyPower)}");

            Console.WriteLine($"MinDirectionBlendTime {Memory.Read<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_MinDirectionBlendTime)}");
            Console.WriteLine($"FallThreshold {Memory.Read<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_FallThreshold)}");

            Console.WriteLine($"BaseJumpPenalty {Memory.Read<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_BaseJumpPenalty)}");
            Console.WriteLine($"BaseJumpPenaltyDuration {Memory.Read<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_BaseJumpPenaltyDuration)}");
            Console.WriteLine($"DurationPower {Memory.Read<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_DurationPower)}");
            Console.WriteLine($"SuddenChangesSmoothness {Memory.Read<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_SuddenChangesSmoothness)}");
        }

        private void RevertOriginalSettings()
        {
        }

        internal void RemoveInertia()
        {
            Console.WriteLine($"chainToInertia {chainToInertia:x2}");

            var allInOne = 0f;
            Memory.Write<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_PenaltyPower, allInOne);
            Memory.Write<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_DurationPower, allInOne);
            Memory.Write<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_SuddenChangesSmoothness, allInOne);

            Memory.Write<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_MinDirectionBlendTime, allInOne);
            Memory.Write<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_FallThreshold, 99999);

            Memory.Write<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_BaseJumpPenalty, allInOne);
            Memory.Write<float>(chainToInertia + ModuleEFT.offsetsEFT.Inertia_BaseJumpPenaltyDuration, allInOne);

            
        }

        internal bool ShowOSDWarning(out string text)
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.NoInertia.Enabled)
            {
                text = "";
                return false;
            }

            text = "";
            return false;
        }
    }
}