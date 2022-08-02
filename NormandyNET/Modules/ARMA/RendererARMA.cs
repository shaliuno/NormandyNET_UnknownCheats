using NormandyNET.Settings;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace NormandyNET.Modules.ARMA
{
    internal class RendererARMA
    {
        internal OnScreedDisplayARMA onScreedDisplayARMA = new OnScreedDisplayARMA();

        private int[] nearTableBubble = new int[] { 0, 100 };
        private int[] farTableBubble = new int[] { 100, 1000 };

        private RenderItem renderItem;
        private RenderItem renderItemOverlay;

        internal RendererARMA()
        {
            ModuleARMA.radarForm.OnPrepareRenderObjectsEvent += MapPrepareObjects;
            ModuleARMA.radarForm.overlay.OnPrepareOverlayRenderObjectsEvent += MapPrepareObjectsOverlay;
        }

        private void MapPrepareObjects(object sender, EventArgs args)
        {
            PopulateListViewCategories();
            RepopulateListViewSearch();

            PreparePlayers();
            PrepareLoot();
            PrepareNetworkBubble();
            PrepareTestObjects();
            onScreedDisplayARMA.RenderOSD();
            onScreedDisplayARMA.ResetCounters();

            ModuleARMA.settingsForm.settingsJson.Entity.ShowStatusChanged = false;
            ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = false;
            ModuleARMA.settingsForm.settingsJson.Loot.ShowStatusChanged = false;

            ModuleARMA.radarForm.modulesDone = true;
        }

        private void RepopulateListViewSearch()
        {
            if (LootItemHelper.FindLootRebuildTable && CommonHelpers.dateTimeHolder > LootItemHelper.FindLootRebuildTableTime)
            {
                
                LootItemHelper.FindLootRebuildTable = false;

                foreach (string entry in LootItemHelper.LootFriendlyNamesCanShow)
                {
                    if (ModuleARMA.settingsForm.lootSearchRegexp != null && (ModuleARMA.settingsForm.lootSearchRegexp.IsMatch(entry)))
                    {
                        ListViewItem item = new ListViewItem(new[] { entry });

                        if (ModuleARMA.settingsForm.metroListViewLootSearchHighlight.Items.Contains(item) == false)
                        {
                            ModuleARMA.settingsForm.metroListViewLootSearchHighlight.Items.Add(item);
                            LootItemHelper.LootFriendlyNamesToShow.Add(entry);
                        }
                    }
                }

                ModuleARMA.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
                return;
            }
        }

        private void PopulateListViewCategories()
        {
        }

        private void PrepareNetworkBubble()
        {
            if (ModuleARMA.settingsForm.settingsJson.Map.NetBubbleCircles && ModuleARMA.readerARMA.localPlayerFound)
            {
                renderItem = new RenderItem();
                renderItem.Text = "circle";
                renderItem.MapPosX = CommonHelpers.myIngamePositionX * OpenGL.CanvasDiffCoeff;
                renderItem.MapPosZ = CommonHelpers.myIngamePositionZ * OpenGL.CanvasDiffCoeff;
                renderItem.DrawColor = Color.FromArgb(128, Color.Blue);
                renderItem.Size = (int)Math.Round(nearTableBubble[1] * OpenGL.CanvasDiffCoeff, 0);
                OpenGL.MapGeometry.Add(renderItem);

                renderItem = new RenderItem();
                renderItem.Text = "circle";
                renderItem.MapPosX = CommonHelpers.myIngamePositionX * OpenGL.CanvasDiffCoeff;
                renderItem.MapPosZ = CommonHelpers.myIngamePositionZ * OpenGL.CanvasDiffCoeff;
                renderItem.DrawColor = Color.FromArgb(128, Color.Blue);
                renderItem.Size = (int)Math.Round(farTableBubble[1] * OpenGL.CanvasDiffCoeff, 0);
                OpenGL.MapGeometry.Add(renderItem);
            }
        }

        private void MapPrepareObjectsOverlay(object sender, EventArgs args)
        {
            if (ModuleARMA.radarForm.overlay != null)
            {
                OverlayPrepareWindowBorder();
                OverlayPrepareCrossHair();
                OverlayPreparePlayers();
                OverlayPrepareLoot();
                OverlayPrepareTestObjects();
                ModuleARMA.radarForm.overlay.modulesDone = true;
            }
        }

        private void PreparePlayers()
        {
            

            if (ModuleARMA.readerARMA.mainEntityList.Count > 0)
            {
                for (int i = 0; i < ModuleARMA.readerARMA.mainEntityList.Count; i++)
                {
                    ModuleARMA.readerARMA.mainEntityList[i].GenerateRenderItem();
                }
            }
        }

        private void PrepareLoot()
        {
            if (!ModuleARMA.settingsForm.settingsJson.Loot.Show)
            {
                return;
            }

            if (ModuleARMA.readerARMA.mainEntityList.Count > 0)
            {
                for (int i = 0; i < ModuleARMA.readerARMA.mainEntityList.Count; i++)
                {
                    if (ModuleARMA.settingsForm.settingsJson.Loot.ShowStatusChanged)
                    {
                        ModuleARMA.readerARMA.mainEntityList[i].updateRenderStatus = true;
                        ModuleARMA.readerARMA.mainEntityList[i].GetCanRender();
                    }

                    if (ModuleARMA.readerARMA.mainEntityList[i].EntityType == null || !ModuleARMA.readerARMA.mainEntityList[i].EntityType.Equals("Loot") || ModuleARMA.readerARMA.mainEntityList[i].EntityType.Equals("NULL") || !ModuleARMA.readerARMA.mainEntityList[i].canRender)
                    {
                        continue;
                    }

                    if (ModuleARMA.readerARMA.mainEntityList[i].isLocalPlayer == false)
                    {
                    }

                    if (ModuleARMA.readerARMA.mainEntityList[i].Position == Vector3.Zero)
                    {
                        continue;
                    }

                    var entityPosXingame = ModuleARMA.readerARMA.mainEntityList[i].Position.X;
                    var entityPosYingame = ModuleARMA.readerARMA.mainEntityList[i].Position.Y;
                    var entityPosZingame = ModuleARMA.readerARMA.mainEntityList[i].Position.Z;

                    var entityPosXmap = (entityPosXingame * OpenGL.CanvasDiffCoeff);
                    var entityPosZmap = (entityPosZingame * OpenGL.CanvasDiffCoeff);
                    var entityPosYmap = entityPosYingame;

                    if (!ModuleARMA.radarForm.IsVisibleOnControl(entityPosXmap, entityPosZmap))
                    {
                        continue;
                    }

                    Vector3 vectorTarget = new Vector3(entityPosXingame, 0, entityPosZingame);
                    Vector3 vectorMe = new Vector3(CommonHelpers.myIngamePositionX, 0, CommonHelpers.myIngamePositionZ);

                    renderItem = new RenderItem();
                    renderItem.IconPositionTexture = IconPositionTexture.loot;
                    renderItem.renderLayer = RenderLayers.LootPriority1;
                    renderItem.Rotation = 0;
                    renderItem.IconSize = ModuleARMA.settingsForm.settingsJson.Map.IconSizeLoot;

                    renderItem.MapPosX = entityPosXmap;
                    renderItem.MapPosZ = entityPosZmap;
                    renderItem.DrawColor = ModuleARMA.readerARMA.mainEntityList[i].Color;
                    OpenGL.MapIcons.Add(renderItem);

                    renderItem = new RenderItem();
                    renderItem.MapPosX = entityPosXmap + 20 + OpenGL.CanvasDiffCoeff;
                    renderItem.MapPosZ = entityPosZmap + 15;
                    renderItem.DrawColor = ModuleARMA.readerARMA.mainEntityList[i].Color;
                    renderItem.renderLayer = RenderLayers.LootPriority1;
                    renderItem.TextOverlayOutline = true;
                    renderItem.Size = (int)FontSizes.misc;

                    renderItem.Text = $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.ColorText)}";

                    var newLine = false;
                    var elevation = Math.Round((ModuleARMA.readerARMA.mainEntityList[i].Position.Y - CommonHelpers.myIngamePositionY), 0);

                    if (ModuleARMA.settingsForm.settingsJson.Entity.Elevation.Arrows)
                    {
                        renderItem.Text += CommonHelpers.GetElevationData(elevation);
                        newLine = true;
                    }

                    if (newLine)
                    {
                        renderItem.Text += $"\n";
                    }

                    if (ModuleARMA.settingsForm.settingsJson.Entity.Name)
                    {
                        renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleARMA.readerARMA.mainEntityList[i].Color)}";
                        renderItem.Text += $"{ModuleARMA.readerARMA.mainEntityList[i].Nickname}\n";
                    }
                    OpenGL.MapText.Add(renderItem);
                }
            }
        }

        private void OverlayPrepareCrossHair()
        {
            var crosshairOffset = 8;
            var crosshairLength = crosshairOffset + 8;

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleARMA.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = -crosshairOffset;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.MapPosXend = -crosshairLength;
            renderItemOverlay.MapPosZend = 0;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleARMA.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = crosshairOffset;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.MapPosXend = crosshairLength;
            renderItemOverlay.MapPosZend = 0;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleARMA.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = -crosshairOffset;
            renderItemOverlay.MapPosXend = 0;
            renderItemOverlay.MapPosZend = -crosshairLength;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleARMA.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = crosshairOffset;
            renderItemOverlay.MapPosXend = 0;
            renderItemOverlay.MapPosZend = crosshairLength;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);
        }

        private void OverlayPreparePlayers()
        {
            if (ModuleARMA.readerARMA.mainEntityList.Count > 0)
            {
                for (int i = 0; i < ModuleARMA.readerARMA.mainEntityList.Count; i++)
                {
                    if (!ModuleARMA.readerARMA.mainEntityList[i].canRender)
                    {
                        continue;
                    }

                    if (ModuleARMA.readerARMA.mainEntityList[i].EntityType.Equals("Unknown"))
                    {
                        continue;
                    }

                    if (ModuleARMA.readerARMA.mainEntityList[i].isLocalPlayer == false)
                    {
                    }

                    if (ModuleARMA.readerARMA.mainEntityList[i].Position == Vector3.Zero)
                    {
                        continue;
                    }

                    var myPosX = CommonHelpers.myIngamePositionX;
                    var myPosY = CommonHelpers.myIngamePositionY;
                    var myPosZ = CommonHelpers.myIngamePositionZ;

                    var vectorMe = new Vector3(myPosX, myPosY, myPosZ);

                    var entityPosXingame = ModuleARMA.readerARMA.mainEntityList[i].Position.X;
                    var entityPosYingame = ModuleARMA.readerARMA.mainEntityList[i].Position.Y;
                    var entityPosZingame = ModuleARMA.readerARMA.mainEntityList[i].Position.Z;

                    var vectorTarget = new Vector3(entityPosXingame, entityPosYingame + ModuleARMA.radarForm.overlay.playerFeetVector, entityPosZingame);
                    var vectorTargetHead = new Vector3(vectorTarget.X, vectorTarget.Y + ModuleARMA.radarForm.overlay.heightToHead, vectorTarget.Z);

                    float distance = (int)Math.Round(Vector3.Distance(vectorMe, vectorTarget));

                    var drawDistance = ModuleARMA.settingsForm.settingsJson.Overlay.DrawDistanceLoot;

                    switch (ModuleARMA.readerARMA.mainEntityList[i].EntityType)
                    {
                        

                        case "Infected":
                            drawDistance = ModuleARMA.settingsForm.settingsJson.Overlay.DrawDistance;
                            break;

                        case "Soldier":
                            drawDistance = ModuleARMA.settingsForm.settingsJson.Overlay.DrawDistance;
                            break;
                    }

                    if (distance < drawDistance && distance > 2)
                    {
                        var outPos = Camera.WorldToScreen(vectorTarget);
                        var outPosHead = Camera.WorldToScreen(vectorTargetHead);

                        ModuleARMA.radarForm.overlay.heightToHead = ModuleARMA.radarForm.overlay.heightForStand;
                        if (outPos.Z >= 1.0f)
                        {
                            ModuleARMA.readerARMA.mainEntityList[i].wtsRender = true;
                            
                            var rectX1 = outPosHead.X - ((outPosHead.Y - outPos.Y) / 3);
                            var rectY1 = outPosHead.Y;
                            var rectX2 = outPos.X + ((outPosHead.Y - outPos.Y) / 3);
                            var rectY2 = outPos.Y;
                            var offsetY = 0;

                            if (ModuleARMA.readerARMA.mainEntityList[i].EntityType.Equals("Player") || ModuleARMA.readerARMA.mainEntityList[i].EntityType.Equals("Infected"))
                            {
                                renderItemOverlay = new RenderItem();
                                renderItemOverlay.Text = "rectangle";
                                renderItemOverlay.Size = ModuleARMA.radarForm.overlay.geometryPixelSize;
                                renderItemOverlay.DrawColor = ModuleARMA.readerARMA.mainEntityList[i].Color;
                                renderItemOverlay.MapPosX = rectX1 + 7;
                                renderItemOverlay.MapPosZ = rectY1 - 7;
                                renderItemOverlay.MapPosXend = rectX2 - 7;
                                renderItemOverlay.MapPosZend = rectY2 + 7;
                                OpenGL.OverlayGeometry.Add(renderItemOverlay);
                            }
                        }
                        else
                        {
                            ModuleARMA.readerARMA.mainEntityList[i].wtsRender = false;
                        }
                    }
                }
            }
        }

        private void OverlayPrepareLoot()
        {
        }

        private void OverlayPrepareTestObjects()
        {
            if (!DebugClass.DebugDrawOverlay)
            {
                return;
            }
            var degressFOV = DateTime.Now.Second * 6;
            var fov_line_X = (float)(Math.Cos(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * 100);
            var fov_line_Y = (float)(Math.Sin(((degressFOV * Math.PI) / -180) - (Math.PI / 2)) * 100);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "line";
            renderItemOverlay.Size = 2;
            renderItemOverlay.DrawColor = Color.LimeGreen;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.MapPosXend = fov_line_X;
            renderItemOverlay.MapPosZend = fov_line_Y;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "circle";
            renderItemOverlay.Size = 2;
            renderItemOverlay.DrawColor = Color.LimeGreen;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.Size = 80;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = DateTime.Now.ToString();
            renderItemOverlay.Size = 2;
            renderItemOverlay.DrawColor = Color.LimeGreen;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = 100;
            renderItemOverlay.Size = (int)FontSizes.misc;
            renderItemOverlay.TextOverlayOutline = true;
            OpenGL.OverlayText.Add(renderItemOverlay);
        }

        private void OverlayPrepareWindowBorder()
        {
            RenderItem renderItem;

            renderItem = new RenderItem();
            renderItem.Text = "rectangle";
            renderItem.Size = 1;
            renderItem.DrawColor = Color.Purple;
            renderItem.MapPosX = -ModuleARMA.radarForm.overlay.Width / 2f + 1;
            renderItem.MapPosZ = -ModuleARMA.radarForm.overlay.Height / 2f + 1;
            renderItem.MapPosXend = (ModuleARMA.radarForm.overlay.Width / 2) + 1;
            renderItem.MapPosZend = (ModuleARMA.radarForm.overlay.Height / 2);
            OpenGL.OverlayGeometry.Add(renderItem);
        }

        private void PrepareTestObjects()
        {
            if (!DebugClass.DebugDraw)
            {
                return;
            }

            string text = $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.OtherColors["Side - BLUEFOR"])}Test\n";
            text += $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.OtherColors["Side - OPFOR"])}Test\n";
            text += $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.OtherColors["Side - INDI"])}Test\n";
            text += $"©{CommonHelpers.ColorHexConverter(ModuleARMA.settingsForm.settingsJson.Colors.EntityColors.OtherColors["Side - CIV"])}Test\n";

            var count = 50;
            var offset = 2;

            for (int i = 0; i < count; i++)
            {
                renderItem = new RenderItem();
                renderItem.Text = text;
                renderItem.TextOverlayOutline = true;
                renderItem.MapPosX = i * offset;
                renderItem.MapPosZ = i * offset;
                renderItem.DrawColor = ModuleARMA.settingsForm.settingsJson.Colors.ColorOSD;
                renderItem.Size = (int)FontSizes.name;
                renderItem.renderLayer = RenderLayers.OSD;
                OpenGL.MapText.Add(renderItem);
            }

            for (int i = 0; i < count; i++)
            {
                renderItem = new RenderItem();
                renderItem.Text = text;
                renderItem.TextOverlayOutline = true;
                renderItem.MapPosX = i * -offset;
                renderItem.MapPosZ = i * offset;
                renderItem.DrawColor = ModuleARMA.settingsForm.settingsJson.Colors.ColorOSD;
                renderItem.Size = (int)FontSizes.name;
                renderItem.renderLayer = RenderLayers.PlayersPriorityLow;
                OpenGL.MapText.Add(renderItem);
            }

            renderItem = new RenderItem();
            renderItem.IconPositionTexture = IconPositionTexture.loot;
            renderItem.MapPosX = 0;
            renderItem.MapPosZ = 0;
            renderItem.renderLayer = RenderLayers.DeadBodies;
            renderItem.DrawColor = Color.Red;
            renderItem.Rotation = 0;
            OpenGL.MapIcons.Add(renderItem);

            return;
        }
    }
}