using NormandyNET.Core;
using NormandyNET.Modules.EFT.Player;
using NormandyNET.Modules.EFT.UI;
using System;
using System.Linq;
using UnityEngine;
using static NormandyNET.Modules.EFT.UI.AimBotSettings;
using Transform = NormandyNET.Modules.EFT.Objects.Components.Transform;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class Aimbot
    {
        private EntityPlayer entityPlayer;
        private float radiansToDegrees = 57.29578f;
        private Vector3 fireportPos;
        private bool isAiming;
        private float bestFov = 500.0f;
        internal bool lockedOn = false;
        internal ulong lockedOnAddress = 0;
        private DateTime lastBoneTime;
        private int lastBoneIdx;

        private SelectedBones boneAimOption;
        private Bone selectedBone = Bone.HumanHead;

        private DateTime TimeLast;
        private DateTime ExtrasTimeLast;
        private int TimeLastRateMs = 1500;
        private ulong fireportTransformBifacial;
        private ulong fireportTransformBifacial2;
        private Transform localPlayerFireport;
        private Vector3 localPlayerFireportPosition;
        private float ammoInitialSpeedCached;

        internal Aimbot(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
            lastBoneTime = CommonHelpers.dateTimeHolder;
        }

        internal void CheckSelectedBones()
        {
            if (entityPlayer.isLocalPlayer)
            {
                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.SelectedBonesUpdate || boneAimOption != ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.SelectedBones)
                {
                    UpdateSelectedBones(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.SelectedBones);
                    ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.SelectedBonesUpdate = false;
                }

                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.SelectedBonesUpdate = true;
            }
        }

        internal void DoAimBot()
        {
            if (!ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Enabled)
            {
                lockedOn = false;
                lockedOnAddress = 0;
                return;
            }

            bool isSideFire = IsSideFire();

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.LeanHack.Enabled && (isSideFire))
            {
                return;
            }

            CheckSelectedBones();

            bool canProceed = AimingOrTilting();

            if (canProceed == false)
            {
                lockedOn = false;
                lockedOnAddress = 0;
                return;
            }

            EntityPlayer target = ReaderEFT.playersList.Find(
                x => x.isLocalPlayer == false &&
                x.isFollowThisPlayer == false &&
                x.playerAddress == lockedOnAddress &&
                x.IsDeadAlready == false
            );

            if (target == null)
            {
                var potentialTargetsList = ReaderEFT.playersList.Where(
                    x => x.isLocalPlayer == false &&
                    x.isFollowThisPlayer == false &&
                    x.distanceToFollowedPlayer <= ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Distance
                    && x.entityInSightArcAngleXZ <= ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.AngleX
                    && x.IsDeadAlready == false
                    && x.notPresent == false
                    );

                if (!ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.TargetTeammates)
                {
                    potentialTargetsList = potentialTargetsList.Where(x => x.isTeammate != null && x.isTeammate == false);
                }

                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.ClosestByDistance)
                {
                    potentialTargetsList = potentialTargetsList.OrderBy(t => t.distanceToFollowedPlayer);
                }
                else
                {
                    bool overriden = false;

                    if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.ClosestTargetOverride)
                    {
                        var closestTargetsCount = potentialTargetsList.Count(x => x.distanceToFollowedPlayer <= ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.DistanceOverride);

                        if (closestTargetsCount > 0)
                        {
                            potentialTargetsList = potentialTargetsList.Where(t => t.distanceToFollowedPlayer <= ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.DistanceOverride);
                            potentialTargetsList = potentialTargetsList.OrderBy(t => t.entityInSightArcAngleXY).ThenBy(t => t.distanceToFollowedPlayer);
                            overriden = true;
                        }
                    }

                    if (!overriden)
                    {
                        potentialTargetsList = potentialTargetsList.Where(t => t.entityInSightArcAngleXZ <= ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.AngleX);

                        potentialTargetsList = potentialTargetsList.Where(t => t.entityInSightArcAngleXY <= ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.AngleY);

                        potentialTargetsList = potentialTargetsList.OrderBy(t => t.entityInSightArcAngleXZ).ThenBy(t => t.entityInSightArcAngleXY);
                    }
                }

                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Prioritizing)
                {
                    potentialTargetsList = potentialTargetsList.OrderBy(t => t.playerType).ThenBy(t => t.entityInSightArcAngleXZ).ThenBy(t => t.entityInSightArcAngleXY);
                }

                if (potentialTargetsList.Count() == 0)
                {
                    lockedOn = false;
                    lockedOnAddress = 0;
                    return;
                }

                target = potentialTargetsList.First();
            }

            if (target.playerBody == null)
            {
                return;
            }

            if (target.GetIsDeadAlready())
            {
                return;
            }

            do
            {
                DoAimingAlready(target);
                entityPlayer.improvementsController.noRecoil?.Check();
            } while (AimingOrTilting() && (target.GetIsDeadAlready(true) == false || target.healthController.IsAlive()));
        }

        private void DoAimingAlready(EntityPlayer target)
        {
            if (boneAimOption == SelectedBones.RandomLowerParts && CommonHelpers.dateTimeHolder > lastBoneTime)
            {
                selectedBone = AimBotSettings.GetRandomBoneLowerPart();
                lastBoneTime = CommonHelpers.dateTimeHolder.AddMilliseconds(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CycleBonesDelay);
            }

            if (boneAimOption == SelectedBones.RandomUpperParts && CommonHelpers.dateTimeHolder > lastBoneTime)
            {
                selectedBone = AimBotSettings.GetRandomBoneUpperPart();
                lastBoneTime = CommonHelpers.dateTimeHolder.AddMilliseconds(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CycleBonesDelay);
            }

            if ((boneAimOption == SelectedBones.CustomPattern_1 || boneAimOption == SelectedBones.CustomPattern_2 || boneAimOption == SelectedBones.CustomPattern_3) && CommonHelpers.dateTimeHolder > lastBoneTime)
            {
                selectedBone = AimBotSettings.GetRandomBoneCustomPattern(boneAimOption, selectedBone);
                lastBoneTime = CommonHelpers.dateTimeHolder.AddMilliseconds(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CycleBonesDelay);
            }

            UnityEngine.Vector3 bonePosition = Vector3.zero;

            if (target.playerBody.bonesClassDict.TryGetValue(selectedBone, out BoneClass boneClass))
            {
                bonePosition = boneClass.GetPosition();
                target.Position = bonePosition;
            }

            if (CommonHelpers.dateTimeHolder > TimeLast)
            {
                TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                entityPlayer.proceduralWeaponAnimation.GetHandsContainer();
                fireportTransformBifacial = Memory.Read<ulong>(entityPlayer.proceduralWeaponAnimation.handsContainer + ModuleEFT.offsetsEFT.Player_ProceduralWeaponAnimation_HandsContainer_FireportTransformBifacial);
                fireportTransformBifacial2 = Memory.Read<ulong>(fireportTransformBifacial + 0x10);
                localPlayerFireport = new Transform(Memory.Read<ulong>(fireportTransformBifacial + 0x10));

                entityPlayer.handsController.GetHandsController();
                entityPlayer.handsController.GetItem();
                entityPlayer.handsController.CurrentAmmoTemplate();
                ammoInitialSpeedCached = entityPlayer.handsController.item.CurrentAmmoTemplateInitialSpeed();
            }

            localPlayerFireportPosition = localPlayerFireport.GetPositionViaIndices();
            entityPlayer.Position = localPlayerFireportPosition;

            var targetVelocity = target.characterController.UpdateVelocity();

            var distance = Vector3.Distance(localPlayerFireportPosition, bonePosition);

            var travelTime = distance / ammoInitialSpeedCached;

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Prediction)
            {
                bonePosition.x += (targetVelocity.x * ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.PredictionForce);
                bonePosition.y += (targetVelocity.y * ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.PredictionForce);
                bonePosition.z += (targetVelocity.z * ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.PredictionForce);
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Randomizer)
            {
                bonePosition = bonePosition + new Vector3(
                    ModuleEFT.radarForm.fastRandom.Next(100, 300) / 1000f / 2 * ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.RandomizerStrength,
                    ModuleEFT.radarForm.fastRandom.Next(100, 300) / 1000f / 2 * ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.RandomizerStrength,
                    ModuleEFT.radarForm.fastRandom.Next(100, 300) / 1000f / 2 * ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.RandomizerStrength
                    );
            }

            Vector3 angle = CalcAngle(localPlayerFireportPosition, bonePosition);

            var angleToWrite = new Vector2(angle.x, angle.y);

            if (target.playerAddress == lockedOnAddress)
            {
                entityPlayer.movementContext.WriteAngle(angleToWrite);
            }
            else if (entityPlayer.entityInSightArcAngleXZ <= ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.AngleX &&
                entityPlayer.entityInSightArcAngleXY <= ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.AngleY)
            {
                lockedOn = true;
                lockedOnAddress = target.playerAddress;
                entityPlayer.movementContext.WriteAngle(angleToWrite);
            }
        }

        private bool AimingOrTilting()
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.TriggerByAiming && entityPlayer.proceduralWeaponAnimation != null && entityPlayer.proceduralWeaponAnimation.IsAiming())
            {
                return true;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.TriggerByTilt)
            {
                var tilt = (Tilt)entityPlayer.movementContext.GetTilt();

                if (tilt == Tilt.Left || tilt == Tilt.Right)
                {
                    return true;
                }
            }

            return false;
        }

        internal Vector3 CalcAngle(Vector3 Src, Vector3 Dst)
        {
            Vector3 Dir = Src - Dst;

            float Magnitude = Length(Dir);

            var Pitch = (float)Math.Asin(Dir.y / Magnitude) * radiansToDegrees;
            var Yaw = -(float)Math.Atan2(Dir.x, -Dir.z) * radiansToDegrees;

            return new Vector3(Yaw, Pitch, 0f);
        }

        internal float CalcFov(Vector3 ViewAngle, Vector3 AimAngle)
        {
            Vector3 Dir = ViewAngle - AimAngle;

            if (Dir.x < -180f)
            {
                Dir.x += 360f;
            }

            if (Dir.x > 180f)
            {
                Dir.x -= 360f;
            }

            return Math.Abs(Length(Dir));
        }

        private float Length(Vector3 v)
        {
            var q = (float)Math.Sqrt(Dot(v, v));
            return q;
        }

        public static float Dot(Vector3 vector1, Vector3 vector2)
        {
            return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
        }

        internal Vector3 find_ideal_pos(Vector3 position)
        {
            fireportPos = entityPlayer.proceduralWeaponAnimation.fireportTransform.GetPosition();

            var cameraTransform = ModuleEFT.readerEFT.fpsCamera.transform;
            if (cameraTransform == null)
            {
                return Vector3.negativeInfinity;
            }

            var forward_fireport = fireportPos + cameraTransform.forward * 2f;
            var upwards_fireport = fireportPos + cameraTransform.up * 2f;
            var downwards_fireport = fireportPos + cameraTransform.up * -2f;
            var right_fireport = fireportPos + cameraTransform.right * 2f;
            var left_fireport = fireportPos + cameraTransform.right * -2f;

            return forward_fireport;
        }

        internal void UpdateSelectedBones(SelectedBones selectedBones)
        {
            this.boneAimOption = selectedBones;

            switch (boneAimOption)
            {
                case SelectedBones.OnlyHead:
                    selectedBone = Bone.HumanHead;
                    break;

                case SelectedBones.OnlyNeck:
                    selectedBone = Bone.HumanNeck;
                    break;

                case SelectedBones.OnlyPelvis:
                    selectedBone = Bone.HumanPelvis;
                    break;

                case SelectedBones.OnlyChest:
                    selectedBone = Bone.HumanSpine2;
                    break;
            }
        }

        private bool IsSideFire()
        {
            return entityPlayer.proceduralWeaponAnimation.IsSideFire();
        }
    }
}