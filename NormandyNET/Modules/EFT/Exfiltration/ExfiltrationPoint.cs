using NormandyNET.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using Color = System.Drawing.Color;
using Transform = NormandyNET.Modules.EFT.Objects.Components.Transform;

namespace NormandyNET.Modules.EFT.Exfiltration
{
    internal class ExfiltrationPoint
    {
        internal ulong address;
        internal string name;
        private GameObject gameObject;
        private Transform transform;
        internal RenderItem renderItem;
        internal Vector3 position;
        internal List<ExfiltrationRequirement> requirements = new List<ExfiltrationRequirement>();
        internal List<string> eligibleEntryPoints = new List<string>();

        public enum EExfiltrationStatus : byte
        {
            NotPresent = 1,
            UncompleteRequirements,
            Countdown,
            RegularMode,
            Pending,
            AwaitsManualActivation
        }

        internal EExfiltrationStatus status;

        public ExfiltrationPoint(ulong gameObjectAddress, string name)
        {
            address = gameObjectAddress;
            this.name = name;
            gameObject = new GameObject(Memory.ReadChain<ulong>(address, new uint[] { ModuleEFT.offsetsEFT.GameObjectEntry, ModuleEFT.offsetsEFT.GameObject }));
            transform = gameObject.GetComponentByName<Transform>("Transform");
            position = transform.GetPositionViaIndices();
            status = (EExfiltrationStatus)Memory.Read<byte>(address + ModuleEFT.offsetsEFT.ExfiltrationPoint_ExfiltrationStatus);
            GetRequirements();
            GetEligibleEntryPoints();
        }

        private void GetEligibleEntryPoints()
        {
            var eligibleEntryPointsArray = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.ExfiltrationPoint_EligibleEntryPoints);

            var count = Memory.Read<uint>(eligibleEntryPointsArray + 0x18);

            for (uint i = 0; i < count; i++)
            {
                var address = Memory.Read<ulong>(eligibleEntryPointsArray + ModuleEFT.offsetsEFT.ArrayFirstEntry + (0x8 * i));
                var name = CommonHelpers.GetStringFromMemory_Unity(address, true).ToLower();
                eligibleEntryPoints.Add(name);
            }
        }

        private void GetRequirements()
        {
            var requirementsArray = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.ExfiltrationPoint_Requirements);

            var count = Memory.Read<uint>(requirementsArray + 0x18);

            for (uint i = 0; i < count; i++)
            {
                var address = Memory.Read<ulong>(requirementsArray + ModuleEFT.offsetsEFT.ArrayFirstEntry + (0x8 * i));
                requirements.Add(new ExfiltrationRequirement(this, address));
            }
        }

        internal void GenerateRenderItem()
        {
            var localPlayer = ReaderEFT.GetLocalPlayer();

            if (localPlayer == null)
            {
                return;
            }

            var invertMap = ModuleEFT.radarForm.mapManager.GetInvertMap();

            var entityPosXingame = position.x;
            var entityPosYingame = position.y;
            var entityPosZingame = position.z;

            var lootPosXmap = (entityPosXingame * OpenGL.CanvasDiffCoeff * invertMap);
            var lootPosZmap = (entityPosZingame * OpenGL.CanvasDiffCoeff * invertMap);
            var lootPosYmap = entityPosYingame;

            if (!ModuleEFT.radarForm.IsVisibleOnControl(lootPosXmap, lootPosZmap))
            {
                return;
            }

            if (position == UnityEngine.Vector3.zero)
            {
                return;
            }

            if (status == EExfiltrationStatus.NotPresent)
            {
                return;
            }

            if (status == EExfiltrationStatus.UncompleteRequirements && requirements.FindIndex(t => t.requirement == ERequirementState.WorldEvent) >= 0)
            {
            }

            if (status == EExfiltrationStatus.Pending)
            {
                return;
            }

            if (eligibleEntryPoints.Count > 0 && !eligibleEntryPoints.Contains(localPlayer.info.EntryPoint))
            {
                return;
            }

            renderItem = new RenderItem();
            renderItem.Text = "circlefill";
            renderItem.MapPosX = lootPosXmap;
            renderItem.MapPosZ = lootPosZmap;
            renderItem.Size = (int)Math.Round(10 * OpenGL.CanvasDiffCoeff, 0);
            renderItem.DrawColor = Color.FromArgb(128, 0, 255, 255);

            if (status == EExfiltrationStatus.UncompleteRequirements)
            {
                renderItem.DrawColor = Color.FromArgb(128, 255, 255, 0);
            }

            OpenGL.MapGeometry.Add(renderItem);
        }

        internal void Update()
        {
            status = (EExfiltrationStatus)Memory.Read<byte>(ModuleEFT.offsetsEFT.ExfiltrationPoint_ExfiltrationStatus + 0xA8);
        }
    }
}