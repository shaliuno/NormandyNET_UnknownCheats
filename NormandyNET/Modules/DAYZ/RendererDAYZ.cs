using NormandyNET.Settings;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace NormandyNET.Modules.DAYZ
{
    internal class RendererDAYZ
    {
        internal OnScreedDisplayDAYZ onScreedDisplayDAYZ = new OnScreedDisplayDAYZ();

        private int[] nearTableBubble = new int[] { 0, 100 };
        private int[] farTableBubble = new int[] { 100, 1000 };

        private RenderItem renderItem;
        private RenderItem renderItemOverlay;

        internal RendererDAYZ()
        {
            ModuleDAYZ.radarForm.OnPrepareRenderObjectsEvent += MapPrepareObjects;
            ModuleDAYZ.radarForm.overlay.OnPrepareOverlayRenderObjectsEvent += MapPrepareObjectsOverlay;
        }

        private string GetDistanceUnit(float meters)
        {
            if (meters < 1000)
            {
                return $"{meters}m ";
            }
            else
            {
                var km = (float)Math.Round(meters / 1000, 1);
                return $"{km}km ";
            }
        }

        private void MapPrepareObjects(object sender, EventArgs args)
        {
            PopulateListViewCategories();
            RepopulateListViewSearch();

            PreparePlayers();
            PrepareLoot();
            PrepareNetworkBubble();

            onScreedDisplayDAYZ.RenderOSD();
            onScreedDisplayDAYZ.ResetCounters();

            ModuleDAYZ.settingsForm.settingsJson.Entity.ShowStatusChanged = false;
            ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = false;
            ModuleDAYZ.settingsForm.settingsJson.Loot.ShowStatusChanged = false;

            ModuleDAYZ.radarForm.modulesDone = true;
        }

        private void RepopulateListViewSearch()
        {
            if (LootItemHelper.FindLootRebuildTable && CommonHelpers.dateTimeHolder > LootItemHelper.FindLootRebuildTableTime)
            {
                
                LootItemHelper.FindLootRebuildTable = false;

                foreach (string entry in LootItemHelper.LootFriendlyNamesCanShow)
                {
                    if (ModuleDAYZ.settingsForm.lootSearchRegexp != null && (ModuleDAYZ.settingsForm.lootSearchRegexp.IsMatch(entry)))
                    {
                        ListViewItem item = new ListViewItem(new[] { entry });

                        if (ModuleDAYZ.settingsForm.metroListViewLootSearchHighlight.Items.Contains(item) == false)
                        {
                            ModuleDAYZ.settingsForm.metroListViewLootSearchHighlight.Items.Add(item);
                            LootItemHelper.LootFriendlyNamesToShow.Add(entry);
                        }
                    }
                }

                ModuleDAYZ.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
                return;
            }
        }

        private void PopulateListViewCategories()
        {
        }

        private void PrepareNetworkBubble()
        {
            if (ModuleDAYZ.settingsForm.settingsJson.Map.NetBubbleCircles && ModuleDAYZ.readerDAYZ.localPlayerFound)
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
            if (ModuleDAYZ.radarForm.overlay != null)
            {
                OverlayPrepareWindowBorder();
                OverlayPrepareCrossHair();
                OverlayPreparePlayers();
                OverlayPrepareLoot();
                OverlayPrepareTestObjects();
                ModuleDAYZ.radarForm.overlay.modulesDone = true;
            }
        }

        private void PreparePlayers()
        {
            

            if (ReaderDAYZ.playersList.Count > 0)
            {
                for (int i = 0; i < ReaderDAYZ.playersList.Count; i++)
                {
                    if (ReaderDAYZ.playersList[i] == null)
                    {
                        continue;
                    }

                    ReaderDAYZ.playersList[i].GenerateRenderItem();
                }
            }
        }

        private void PrepareLoot()
        {
        }

        private void OverlayPrepareCrossHair()
        {
            var crosshairOffset = 8;
            var crosshairLength = crosshairOffset + 8;

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleDAYZ.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = -crosshairOffset;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.MapPosXend = -crosshairLength;
            renderItemOverlay.MapPosZend = 0;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleDAYZ.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = crosshairOffset;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.MapPosXend = crosshairLength;
            renderItemOverlay.MapPosZend = 0;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleDAYZ.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = -crosshairOffset;
            renderItemOverlay.MapPosXend = 0;
            renderItemOverlay.MapPosZend = -crosshairLength;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleDAYZ.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = crosshairOffset;
            renderItemOverlay.MapPosXend = 0;
            renderItemOverlay.MapPosZend = crosshairLength;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);
        }

        private void OverlayPreparePlayers()
        {
            if (ReaderDAYZ.playersList.Count > 0)
            {
                for (int i = 0; i < ReaderDAYZ.playersList.Count; i++)
                {
                    if (!ReaderDAYZ.playersList[i].canRender)
                    {
                        continue;
                    }

                    if (ReaderDAYZ.playersList[i].EntityType.Equals("Unknown"))
                    {
                        continue;
                    }

                    if (ReaderDAYZ.playersList[i].isLocalPlayer == false)
                    {
                        switch (ReaderDAYZ.playersList[i].tableType)
                        {
                            case TableType.Near:
                                break;

                            case TableType.Far:
                                break;

                            case TableType.Slow:
                                break;

                            case TableType.Fast:
                                break;
                        }
                    }

                    if (ReaderDAYZ.playersList[i].Position == Vector3.Zero)
                    {
                        continue;
                    }

                    var myPosX = CommonHelpers.myIngamePositionX;
                    var myPosY = CommonHelpers.myIngamePositionY;
                    var myPosZ = CommonHelpers.myIngamePositionZ;

                    var vectorMe = new Vector3(myPosX, myPosY, myPosZ);

                    var entityPosXingame = ReaderDAYZ.playersList[i].Position.X;
                    var entityPosYingame = ReaderDAYZ.playersList[i].Position.Y;
                    var entityPosZingame = ReaderDAYZ.playersList[i].Position.Z;

                    var vectorTarget = new Vector3(entityPosXingame, entityPosYingame + ModuleDAYZ.radarForm.overlay.playerFeetVector, entityPosZingame);
                    var vectorTargetHead = new Vector3(vectorTarget.X, vectorTarget.Y + ModuleDAYZ.radarForm.overlay.heightToHead, vectorTarget.Z);

                    float distance = (int)Math.Round(Vector3.Distance(vectorMe, vectorTarget));

                    var drawDistance = ModuleDAYZ.settingsForm.settingsJson.Overlay.DrawDistanceLoot;

                    switch (ReaderDAYZ.playersList[i].EntityType)
                    {
                        

                        case "Infected":
                            drawDistance = ModuleDAYZ.settingsForm.settingsJson.Overlay.DrawDistance;
                            break;

                        case "Player":
                            drawDistance = ModuleDAYZ.settingsForm.settingsJson.Overlay.DrawDistance;
                            break;
                    }

                    if (distance < drawDistance && distance > 2)
                    {
                        var outPos = Camera.WorldToScreen(vectorTarget);
                        var outPosHead = Camera.WorldToScreen(vectorTargetHead);

                        ModuleDAYZ.radarForm.overlay.heightToHead = ModuleDAYZ.radarForm.overlay.heightForStand;
                        if (outPos.Z >= 1.0f)
                        {
                            ReaderDAYZ.playersList[i].wtsRender = true;
                            
                            var rectX1 = outPosHead.X - ((outPosHead.Y - outPos.Y) / 3);
                            var rectY1 = outPosHead.Y;
                            var rectX2 = outPos.X + ((outPosHead.Y - outPos.Y) / 3);
                            var rectY2 = outPos.Y;
                            var offsetY = 0;

                            if (ReaderDAYZ.playersList[i].EntityType.Equals("Player") || ReaderDAYZ.playersList[i].EntityType.Equals("Infected"))
                            {
                                renderItemOverlay = new RenderItem();
                                renderItemOverlay.Text = "rectangle";
                                renderItemOverlay.Size = ModuleDAYZ.radarForm.overlay.geometryPixelSize;
                                renderItemOverlay.DrawColor = ReaderDAYZ.playersList[i].Color;
                                renderItemOverlay.MapPosX = rectX1 + 7;
                                renderItemOverlay.MapPosZ = rectY1 - 7;
                                renderItemOverlay.MapPosXend = rectX2 - 7;
                                renderItemOverlay.MapPosZend = rectY2 + 7;
                                OpenGL.OverlayGeometry.Add(renderItemOverlay);
                            }
                            else
                            {
                                var radius = (int)Math.Ceiling(5 / distance / ModuleDAYZ.radarForm.overlay.fixedHeadCircleSize / 2);

                                renderItemOverlay = new RenderItem();
                                renderItemOverlay.Text = "rectangle";
                                renderItemOverlay.Size = ModuleDAYZ.radarForm.overlay.geometryPixelSize;
                                renderItemOverlay.DrawColor = ReaderDAYZ.playersList[i].Color;
                                renderItemOverlay.MapPosX = outPos.X + radius;
                                renderItemOverlay.MapPosZ = outPos.Y - radius;
                                renderItemOverlay.MapPosXend = outPos.X - radius;
                                renderItemOverlay.MapPosZend = outPos.Y + radius;
                                OpenGL.OverlayGeometry.Add(renderItemOverlay);

                                renderItemOverlay = new RenderItem();
                                renderItemOverlay.Text = ReaderDAYZ.playersList[i].FriendlyName + "";
                                renderItemOverlay.TextOverlayOutline = true;
                                renderItemOverlay.MapPosX = outPos.X + 15;
                                renderItemOverlay.MapPosZ = outPos.Y;
                                renderItemOverlay.Size = (int)FontSizes.misc;
                                renderItemOverlay.DrawColor = ReaderDAYZ.playersList[i].Color;
                                OpenGL.OverlayText.Add(renderItemOverlay);
                            }
                        }
                        else
                        {
                            ReaderDAYZ.playersList[i].wtsRender = false;
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
            renderItem.MapPosX = -ModuleDAYZ.radarForm.overlay.Width / 2f + 1;
            renderItem.MapPosZ = -ModuleDAYZ.radarForm.overlay.Height / 2f + 1;
            renderItem.MapPosXend = (ModuleDAYZ.radarForm.overlay.Width / 2) + 1;
            renderItem.MapPosZend = (ModuleDAYZ.radarForm.overlay.Height / 2);
            OpenGL.OverlayGeometry.Add(renderItem);
        }

        private void DrawFOVLines(float degressFOVcenter, float entityPosXmap, float entityPosZmap, int i)
        {
            var lengthFOV = ModuleDAYZ.settingsForm.settingsJson.Entity.LineOfSight.Enemy * OpenGL.CanvasDiffCoeff;

            if (ReaderDAYZ.playersList[i].EntityType.Equals("You"))
            {
                lengthFOV = ModuleDAYZ.settingsForm.settingsJson.Entity.LineOfSight.You * OpenGL.CanvasDiffCoeff;
            }

            var fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOVcenter * Math.PI) / 180) + (Math.PI / 2)) * lengthFOV));
            var fov_line_Z = (float)(entityPosZmap + (Math.Sin(((degressFOVcenter * Math.PI) / 180) + (Math.PI / 2)) * lengthFOV));

            renderItem = new RenderItem();
            renderItem.Text = "linestripple";
            renderItem.MapPosX = entityPosXmap;
            renderItem.MapPosZ = entityPosZmap;
            renderItem.MapPosXend = fov_line_X;
            renderItem.MapPosZend = fov_line_Z;
            renderItem.DrawColor = ReaderDAYZ.playersList[i].Color;
            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
            renderItem.Size = 2;
            OpenGL.MapGeometry.Add(renderItem);

            renderItem = new RenderItem();
            renderItem.Text = "linestripple_invert";
            renderItem.MapPosX = entityPosXmap;
            renderItem.MapPosZ = entityPosZmap;
            renderItem.MapPosXend = fov_line_X;
            renderItem.MapPosZend = fov_line_Z;
            renderItem.DrawColor = System.Windows.Forms.ControlPaint.Dark(ReaderDAYZ.playersList[i].Color);
            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
            renderItem.Size = 2;
            OpenGL.MapGeometry.Add(renderItem);
        }
    }
}