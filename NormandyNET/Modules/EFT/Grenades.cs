using NormandyNET.Core;
using NormandyNET.Modules.EFT.Objects.Components;
using System;

namespace NormandyNET.Modules.EFT
{
    internal class Grenade
    {
        internal bool notPresentTest;
        internal ulong address;
        internal bool notPresent;
        internal GameObject gameObject;
        private string ItemBasicName;
        internal Transform transform;
        private UnityEngine.Vector3 Position;
        internal DateTime timeNotPresent;
        private int timeNotPresentMax = 10;
        internal bool canRender = true;
        private RenderItem renderItem;

        private DateTime TimeLast;
        private int TimeLastRateMs = 125;
        private int color = 32;

        public Grenade(ulong nadeAddress)
        {
            this.address = nadeAddress;
        }

        private void GetGameObject()
        {
            if (gameObject == null)
            {
                var gameObjectEntry = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.GameObjectEntry);
                var gameObjectAddress = Memory.Read<ulong>(gameObjectEntry + ModuleEFT.offsetsEFT.GameObject);
                gameObject = new GameObject(gameObjectAddress);
                ItemBasicName = gameObject.GetName();
            }

            if (transform == null && Memory.IsValidPointer(gameObject.address))
            {
                transform = gameObject.GetComponentByName<Transform>("Transform");
            }
        }

        internal void GetData()
        {
            GetGameObject();

            if (transform != null)
            {
                Position = transform.GetPosition();
            }
        }

        internal void StaleObjectAction()
        {
            if ((CommonHelpers.dateTimeHolder - timeNotPresent).Seconds > timeNotPresentMax)
            {
                canRender = false;
            }
        }

        internal void GenerateRenderItem()
        {
            if (!canRender)
            {
                return;
            }

            var invertMap = ModuleEFT.radarForm.mapManager.GetInvertMap();

            var entityPosXingame = Position.x;
            var entityPosYingame = Position.y;
            var entityPosZingame = Position.z;

            var lootPosXmap = (entityPosXingame * OpenGL.CanvasDiffCoeff * invertMap);
            var lootPosZmap = (entityPosZingame * OpenGL.CanvasDiffCoeff * invertMap);
            var lootPosYmap = entityPosYingame;

            if (!ModuleEFT.radarForm.IsVisibleOnControl(lootPosXmap, lootPosZmap))
            {
                return;
            }

            if (Position == UnityEngine.Vector3.zero)
            {
                return;
            }

            if (renderItem == null)
            {
                renderItem = new RenderItem();
            }

            renderItem.MapPosX = lootPosXmap;
            renderItem.MapPosZ = lootPosZmap;
            renderItem.IconPositionTexture = IconPositionTexture.player_dead;

            if (CommonHelpers.dateTimeHolder > TimeLast)
            {
                TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
                color += 32;
                if (color > 256)
                {
                    color = 32;
                }
                renderItem.DrawColor = System.Drawing.Color.FromArgb(color - 1, ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Grenade);
            }

            renderItem.IconSize = ModuleEFT.settingsForm.settingsJson.Map.IconSizePlayers;
            renderItem.renderLayer = RenderLayers.LootPriority1;
            OpenGL.MapIcons.Add(renderItem);
        }
    }
}