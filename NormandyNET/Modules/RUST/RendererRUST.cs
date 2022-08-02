using NormandyNET.Settings;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace NormandyNET.Modules.RUST
{
    internal class RendererRUST
    {
        internal OnScreedDisplayRUST onScreedDisplayRUST = new OnScreedDisplayRUST();

        private int[] nearTableBubble = new int[] { 0, 100 };
        private int[] farTableBubble = new int[] { 100, 1000 };

        private RenderItem renderItem;
        private RenderItem renderItemOverlay;

        internal RendererRUST()
        {
            ModuleRUST.radarForm.OnPrepareRenderObjectsEvent += MapPrepareObjects;
            ModuleRUST.radarForm.overlay.OnPrepareOverlayRenderObjectsEvent += MapPrepareObjectsOverlay;
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

            PrepareEntities();

            PrepareTestObjects();
            onScreedDisplayRUST.RenderOSD();
            onScreedDisplayRUST.ResetCounters();

            ModuleRUST.settingsForm.settingsJson.Entity.ShowStatusChanged = false;
            ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = false;
            ModuleRUST.settingsForm.settingsJson.Loot.ShowStatusChanged = false;

            ModuleRUST.radarForm.modulesDone = true;
        }

        private void PrepareLoot()
        {
        }

        private void PrepareTestObjects()
        {
            if (!DebugClass.DebugDraw)
            {
                return;
            }
            var invertMap = ModuleRUST.radarForm.mapManager.GetInvertMap();

            var entityPosXmap = (1 * OpenGL.CanvasDiffCoeff * invertMap);
            var entityPosZmap = (1 * OpenGL.CanvasDiffCoeff * invertMap);
            var entityPosYmap = 1;

            renderItem = new RenderItem();
            renderItem.IconPositionTexture = IconPositionTexture.player;
            renderItem.MapPosX = entityPosXmap;
            renderItem.MapPosZ = entityPosZmap;
            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
            renderItem.DrawColor = Color.Red;
            renderItem.Rotation = 0 * -1;
            renderItem.IconSize = ModuleRUST.settingsForm.settingsJson.Map.IconSizePlayers;

            OpenGL.MapIcons.Add(renderItem);

            renderItem = new RenderItem();

            renderItem.MapPosX = entityPosXmap + (20 * ModuleRUST.settingsForm.settingsJson.Map.IconSizePlayers) + OpenGL.CanvasDiffCoeff;
            renderItem.MapPosZ = entityPosZmap + 15;
            renderItem.DrawColor = ModuleRUST.settingsForm.settingsJson.Colors.ColorText;
            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
            renderItem.TextOverlayOutline = true;
            renderItem.Size = (int)FontSizes.misc;

            renderItem.Text = $"©{CommonHelpers.ColorHexConverter(ModuleRUST.settingsForm.settingsJson.Colors.ColorText)}One\n";
            renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleRUST.settingsForm.settingsJson.Colors.ColorText)}Two\n";
            renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleRUST.settingsForm.settingsJson.Colors.ColorText)}Three\n";
            renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleRUST.settingsForm.settingsJson.Colors.ColorText)}Four\n";
            OpenGL.MapText.Add(renderItem);
        }

        private void PrepareEntities()
        {
            if (ReaderRUST.entityList.Count > 0)
            {
                var invertMap = ModuleRUST.radarForm.mapManager.GetInvertMap();
                var unitSize = ModuleRUST.radarForm.mapManager.GetUnitSize();

                for (int i = 0; i < ReaderRUST.entityList.Count; i++)
                {
                    if (!ModuleRUST.settingsForm.settingsJson.Loot.Show && ReaderRUST.entityList[i].EntityType == EntityTypeRUST.Loot)
                    {
                        continue;
                    }

                    if (ReaderRUST.entityList[i].EntityType == EntityTypeRUST.ESP)
                    {
                        continue;
                    }

                    if (ModuleRUST.settingsForm.settingsJson.Loot.ShowStatusChanged)
                    {
                        ReaderRUST.entityList[i].updateRenderStatus = true;
                        ReaderRUST.entityList[i].GetCanRender();
                    }

                    if (ModuleRUST.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged)
                    {
                        ReaderRUST.entityList[i].updateColors = true;
                        ReaderRUST.entityList[i].GetColor();
                    }

                    if (ModuleRUST.settingsForm.settingsJson.Entity.ShowStatusChanged)
                    {
                        ReaderRUST.entityList[i].updateRenderStatus = true;
                        ReaderRUST.entityList[i].GetCanRender();
                    }

                    if (!ReaderRUST.entityList[i].canRender)
                    {
                        continue;
                    }

                    if (ReaderRUST.entityList[i].isLocalPlayer == true)
                    {
                        CommonHelpers.myIngamePositionX = ReaderRUST.entityList[i].Position.X;
                        CommonHelpers.myIngamePositionY = ReaderRUST.entityList[i].Position.Y;
                        CommonHelpers.myIngamePositionZ = ReaderRUST.entityList[i].Position.Z;
                    }

                    if (ReaderRUST.entityList[i].Position == Vector3.Zero)
                    {
                        continue;
                    }

                    if (ReaderRUST.entityList[i].EntityType == EntityTypeRUST.Player && ReaderRUST.entityList[i].isConnected == false && ModuleRUST.settingsForm.settingsJson.Entity.Bodies == false)
                    {
                        continue;
                    }

                    var positionOnRadar = new Vector3(
                        ReaderRUST.entityList[i].Position.X * OpenGL.CanvasDiffCoeff * invertMap,
                        ReaderRUST.entityList[i].Position.Y * OpenGL.CanvasDiffCoeff * invertMap,
                        ReaderRUST.entityList[i].Position.Z * OpenGL.CanvasDiffCoeff * invertMap
                        );

                    Vector3 vectorTarget = new Vector3(ReaderRUST.entityList[i].Position.X, 0, ReaderRUST.entityList[i].Position.Z);
                    Vector3 vectorMe = new Vector3(CommonHelpers.myIngamePositionX, 0, CommonHelpers.myIngamePositionZ);
                    var distance = (int)Math.Round(Vector3.Distance(vectorTarget, vectorMe) / unitSize);

                    if (!ModuleRUST.radarForm.IsVisibleOnControl(positionOnRadar.X, positionOnRadar.Z, 0))
                    {
                        continue;
                    }

                    if (ModuleRUST.settingsForm.settingsJson.Entity.LOS)
                    {
                        switch (ReaderRUST.entityList[i].EntityType)
                        {
                            case EntityTypeRUST.Player:

                                if (ReaderRUST.entityList[i].isConnected == true && ReaderRUST.entityList[i].isDead == false)
                                {
                                    DrawFOVLines(ReaderRUST.entityList[i].Direction.X, positionOnRadar.X, positionOnRadar.Z, i);
                                }
                                break;

                            case EntityTypeRUST.PlayerNPC:

                                break;

                            case EntityTypeRUST.LocalPlayer:
                                DrawFOVLines(ReaderRUST.entityList[i].Direction.X, positionOnRadar.X, positionOnRadar.Z, i);
                                break;
                        }
                    }

                    renderItem = new RenderItem();
                    renderItem.IconSize = ModuleRUST.settingsForm.settingsJson.Map.IconSizePlayers;
                    renderItem.MapPosX = positionOnRadar.X;
                    renderItem.MapPosZ = positionOnRadar.Z;
                    renderItem.DrawColor = ReaderRUST.entityList[i].ColorEntity;

                    switch (ReaderRUST.entityList[i].EntityType)
                    {
                        case EntityTypeRUST.Animal:
                            renderItem.IconPositionTexture = IconPositionTexture.animal;
                            renderItem.renderLayer = RenderLayers.PlayersPriorityLow;
                            break;

                        case EntityTypeRUST.Loot:
                            renderItem.IconPositionTexture = IconPositionTexture.loot;
                            renderItem.renderLayer = RenderLayers.LootPriority1;
                            renderItem.IconSize = ModuleRUST.settingsForm.settingsJson.Map.IconSizeLoot;
                            break;

                        case EntityTypeRUST.House:
                            renderItem.IconPositionTexture = IconPositionTexture.house;
                            renderItem.renderLayer = RenderLayers.PlayersPriorityLow;

                            break;

                        case EntityTypeRUST.Turret:
                            renderItem.IconPositionTexture = IconPositionTexture.statictech;
                            renderItem.renderLayer = RenderLayers.PlayersPriorityLow;

                            break;

                        case EntityTypeRUST.Vehicle:
                            renderItem.IconPositionTexture = IconPositionTexture.vehicle;
                            renderItem.Rotation = ReaderRUST.entityList[i].Direction.X;
                            renderItem.renderLayer = RenderLayers.PlayersPriorityLow;

                            break;

                        case EntityTypeRUST.VehicleNPC:
                            renderItem.IconPositionTexture = IconPositionTexture.vehicle;
                            renderItem.Rotation = ReaderRUST.entityList[i].Direction.X;
                            renderItem.renderLayer = RenderLayers.PlayersPriorityMedium;

                            break;

                        case EntityTypeRUST.Player:
                            renderItem.IconPositionTexture = IconPositionTexture.player;
                            renderItem.Rotation = ReaderRUST.entityList[i].Direction.X;
                            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;

                            if (ReaderRUST.entityList[i].isConnected == false)
                            {
                                renderItem.IconPositionTexture = IconPositionTexture.player_dead;
                            }

                            if (ReaderRUST.entityList[i].isDead == true)
                            {
                                renderItem.IconPositionTexture = IconPositionTexture.player_dead;
                            }

                            break;

                        case EntityTypeRUST.PlayerNPC:
                            renderItem.IconPositionTexture = IconPositionTexture.player;
                            renderItem.Rotation = ReaderRUST.entityList[i].Direction.X;
                            renderItem.renderLayer = RenderLayers.PlayersPriorityMedium;

                            break;

                        case EntityTypeRUST.LocalPlayer:
                            renderItem.IconPositionTexture = IconPositionTexture.player;
                            renderItem.Rotation = ReaderRUST.entityList[i].Direction.X;
                            renderItem.renderLayer = RenderLayers.You;
                            break;
                    }

                    OpenGL.MapIcons.Add(renderItem);

                    renderItem = new RenderItem();
                    renderItem.TextOverlayOutline = true;
                    renderItem.Size = (int)FontSizes.misc;
                    renderItem.MapPosX = positionOnRadar.X + (20 * ModuleRUST.settingsForm.settingsJson.Map.IconSizePlayers) + OpenGL.CanvasDiffCoeff;
                    renderItem.MapPosZ = positionOnRadar.Z + (15 * ModuleRUST.settingsForm.settingsJson.Map.IconSizePlayers);
                    renderItem.DrawColor = ModuleRUST.settingsForm.settingsJson.Colors.ColorText;

                    renderItem.Text = $"©{CommonHelpers.ColorHexConverter(ModuleRUST.settingsForm.settingsJson.Colors.ColorText)}";

                    var newLineRequired = false;
                    var canDoDistanceAndElevation = false;

                    if (ModuleRUST.settingsForm.settingsJson.Entity.Distance)
                    {
                        switch (ReaderRUST.entityList[i].EntityType)
                        {
                            case EntityTypeRUST.Animal:
                                break;

                            case EntityTypeRUST.Loot:
                                break;

                            case EntityTypeRUST.House:
                                break;

                            case EntityTypeRUST.Turret:
                                break;

                            case EntityTypeRUST.Vehicle:
                                canDoDistanceAndElevation = true;
                                break;

                            case EntityTypeRUST.VehicleNPC:
                                canDoDistanceAndElevation = true;
                                break;

                            case EntityTypeRUST.Player:
                                canDoDistanceAndElevation = true;
                                break;

                            case EntityTypeRUST.PlayerNPC:
                                canDoDistanceAndElevation = true;
                                break;

                            case EntityTypeRUST.LocalPlayer:
                                break;
                        }

                        if (canDoDistanceAndElevation)
                        {
                            renderItem.Text += GetDistanceUnit(distance);
                            newLineRequired = true;
                        }
                    }

                    var elevation = Math.Round((ReaderRUST.entityList[i].Position.Y - CommonHelpers.myIngamePositionY) / unitSize, 0);

                    if (canDoDistanceAndElevation)
                    {
                        if (ModuleRUST.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Absolute))
                        {
                            renderItem.Text += $"({(int)ReaderRUST.entityList[i].Position.Y}) ";
                            newLineRequired = true;
                        }

                        if (ModuleRUST.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Relative))
                        {
                            renderItem.Text += $"({elevation}) ";
                            newLineRequired = true;
                        }

                        if (ModuleRUST.settingsForm.settingsJson.Entity.Elevation.Arrows)
                        {
                            renderItem.Text += CommonHelpers.GetElevationData(elevation);
                        }
                    }

                    if (newLineRequired)
                    {
                        renderItem.Text += $"\n";
                    }

                    if (ModuleRUST.settingsForm.settingsJson.Entity.Name)
                    {
                        renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ReaderRUST.entityList[i].ColorEntity)}";

                        switch (ReaderRUST.entityList[i].EntityType)
                        {
                            case EntityTypeRUST.Animal:
                                renderItem.Text += $"{ReaderRUST.entityList[i].FriendlyName}\n";
                                break;

                            case EntityTypeRUST.Loot:
                                renderItem.Text += $"{ReaderRUST.entityList[i].FriendlyName}\n";
                                break;

                            case EntityTypeRUST.House:
                                if (ReaderRUST.entityList[i].authorizedPlayers != null && ModuleRUST.settingsForm.settingsJson.Map.HouseOwners)
                                {
                                    renderItem.Text += $"{ReaderRUST.entityList[i].authorizedPlayers.protectedMinutes}\n";

                                    foreach (var a in ReaderRUST.entityList[i].authorizedPlayers.authorizedPlayers)
                                    {
                                        renderItem.Text += $"{a.nickname}\n";
                                    }
                                }
                                break;

                            case EntityTypeRUST.Turret:
                                break;

                            case EntityTypeRUST.Vehicle:
                                renderItem.Text += $"{ReaderRUST.entityList[i].FriendlyName}\n";

                                break;

                            case EntityTypeRUST.VehicleNPC:
                                renderItem.Text += $"{ReaderRUST.entityList[i].FriendlyName}\n";
                                break;

                            case EntityTypeRUST.Player:
                                renderItem.Text += $"{ReaderRUST.entityList[i].DisplayName}\n";

                                if (ModuleRUST.settingsForm.settingsJson.Entity.Bodies)
                                {
                                    if (ReaderRUST.entityList[i].isConnected == false && ReaderRUST.entityList[i].isDead == false)
                                    {
                                        renderItem.Text += $" (Sleeping)\n";
                                    }

                                    if (ReaderRUST.entityList[i].isConnected == true && ReaderRUST.entityList[i].isDead == true)
                                    {
                                        renderItem.Text += $" (Dead)\n";
                                    }
                                }
                                break;

                            case EntityTypeRUST.PlayerNPC:

                                break;

                            case EntityTypeRUST.LocalPlayer:
                                break;
                        }
                    }

                    if (ModuleRUST.settingsForm.settingsJson.Entity.Weapon && ReaderRUST.entityList[i].Weapons.Length > 0)
                    {
                        switch (ReaderRUST.entityList[i].EntityType)
                        {
                            case EntityTypeRUST.Player:
                                renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleRUST.settingsForm.settingsJson.Colors.ColorText)}{ReaderRUST.entityList[i].Weapons.Truncate(22)}\n";
                                break;

                            case EntityTypeRUST.PlayerNPC:
                                renderItem.Text += $"©{CommonHelpers.ColorHexConverter(ModuleRUST.settingsForm.settingsJson.Colors.ColorText)}{ReaderRUST.entityList[i].Weapons.Truncate(22)}\n";
                                break;
                        }
                    }

                    if (DebugClass.DebugDraw)
                    {
                    }
                    OpenGL.MapText.Add(renderItem);
                }
            }
        }

        private void RepopulateListViewSearch()
        {
            if (LootItemHelper.FindLootRebuildTable && CommonHelpers.dateTimeHolder > LootItemHelper.FindLootRebuildTableTime)
            {
                
                LootItemHelper.FindLootRebuildTable = false;

                foreach (string entry in LootItemHelper.LootFriendlyNamesCanShow)
                {
                    if (ModuleRUST.settingsForm.lootSearchRegexp != null && (ModuleRUST.settingsForm.lootSearchRegexp.IsMatch(entry)))
                    {
                        ListViewItem item = new ListViewItem(new[] { entry });

                        if (ModuleRUST.settingsForm.metroListViewLootSearchHighlight.Items.Contains(item) == false)
                        {
                            ModuleRUST.settingsForm.metroListViewLootSearchHighlight.Items.Add(item);
                            LootItemHelper.LootFriendlyNamesToShow.Add(entry);
                        }
                    }
                }

                ModuleRUST.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
                return;
            }
        }

        private void PopulateListViewCategories()
        {
        }

        private void MapPrepareObjectsOverlay(object sender, EventArgs args)
        {
            if (ModuleRUST.radarForm.overlay != null)
            {
                OverlayPrepareWindowBorder();
                OverlayPrepareCrossHair();
                OverlayPrepareEntities();

                ModuleRUST.radarForm.overlay.modulesDone = true;
            }
        }

        private void DrawFOVLines(float degressFOVcenter, float entityPosXmap, float entityPosYmap, int i)
        {
            var lengthFOV = ModuleRUST.settingsForm.settingsJson.Entity.LineOfSight.Enemy * OpenGL.CanvasDiffCoeff;
            if (ReaderRUST.entityList[i].EntityType == EntityTypeRUST.LocalPlayer)
            {
                lengthFOV = ModuleRUST.settingsForm.settingsJson.Entity.LineOfSight.You * OpenGL.CanvasDiffCoeff;
            }

            var fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOVcenter * Math.PI) / 180) + (Math.PI / 2)) * lengthFOV));
            var fov_line_Z = (float)(entityPosYmap + (Math.Sin(((degressFOVcenter * Math.PI) / 180) + (Math.PI / 2)) * lengthFOV));

            renderItem = new RenderItem();
            renderItem.Text = "linestripple";
            renderItem.MapPosX = entityPosXmap;
            renderItem.MapPosZ = entityPosYmap;
            renderItem.MapPosXend = fov_line_X;
            renderItem.MapPosZend = fov_line_Z;
            renderItem.DrawColor = ReaderRUST.entityList[i].ColorEntity;
            renderItem.renderLayer = RenderLayers.PlayersPriorityMedium;
            renderItem.Size = 2;
            OpenGL.MapGeometry.Add(renderItem);

            renderItem = new RenderItem();
            renderItem.Text = "linestripple_invert";
            renderItem.MapPosX = entityPosXmap;
            renderItem.MapPosZ = entityPosYmap;
            renderItem.MapPosXend = fov_line_X;
            renderItem.MapPosZend = fov_line_Z;
            renderItem.DrawColor = System.Windows.Forms.ControlPaint.Dark(ReaderRUST.entityList[i].ColorEntity);
            renderItem.renderLayer = RenderLayers.PlayersPriorityMedium;
            renderItem.Size = 2;
            OpenGL.MapGeometry.Add(renderItem);
        }

        private void OverlayPrepareWindowBorder()
        {
            RenderItem renderItem;

            renderItem = new RenderItem();
            renderItem.Text = "rectangle";
            renderItem.Size = 1;
            renderItem.DrawColor = Color.Purple;
            renderItem.MapPosX = -ModuleRUST.radarForm.overlay.Width / 2f + 1;
            renderItem.MapPosZ = -ModuleRUST.radarForm.overlay.Height / 2f + 1;
            renderItem.MapPosXend = (ModuleRUST.radarForm.overlay.Width / 2) + 1;
            renderItem.MapPosZend = (ModuleRUST.radarForm.overlay.Height / 2);
            OpenGL.OverlayGeometry.Add(renderItem);
        }

        private void OverlayPrepareCrossHair()
        {
            var crosshairOffset = 8;
            var crosshairLength = crosshairOffset + 8;

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleRUST.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = -crosshairOffset;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.MapPosXend = -crosshairLength;
            renderItemOverlay.MapPosZend = 0;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleRUST.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = crosshairOffset;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.MapPosXend = crosshairLength;
            renderItemOverlay.MapPosZend = 0;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleRUST.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = -crosshairOffset;
            renderItemOverlay.MapPosXend = 0;
            renderItemOverlay.MapPosZend = -crosshairLength;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleRUST.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = crosshairOffset;
            renderItemOverlay.MapPosXend = 0;
            renderItemOverlay.MapPosZend = crosshairLength;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);
        }

        private void OverlayPrepareEntities()
        {
            if (ReaderRUST.entityList.Count == 0)
            {
                return;
            }

            for (int i = 0; i < ReaderRUST.entityList.Count; i++)
            {
                if (!ReaderRUST.entityList[i].canRender)
                {
                    continue;
                }

                if (ReaderRUST.entityList[i].Position == Vector3.Zero)
                {
                    continue;
                }

                if (ReaderRUST.entityList[i].EntityType != EntityTypeRUST.Player && ReaderRUST.entityList[i].EntityType != EntityTypeRUST.PlayerNPC && ReaderRUST.entityList[i].EntityType != EntityTypeRUST.ESP)
                {
                    continue;
                }

                if (ReaderRUST.entityList[i].EntityType == EntityTypeRUST.Player && ReaderRUST.entityList[i].isConnected == false && ModuleRUST.settingsForm.settingsJson.Entity.Bodies == false)
                {
                    continue;
                }

                var myPosX = CommonHelpers.myIngamePositionX;
                var myPosY = CommonHelpers.myIngamePositionY;
                var myPosZ = CommonHelpers.myIngamePositionZ;

                var vectorMe = new OpenTK.Vector3(myPosX, myPosY, myPosZ);

                var entityPosXingame = ReaderRUST.entityList[i].Position.X;
                var entityPosYingame = ReaderRUST.entityList[i].Position.Y;
                var entityPosZingame = ReaderRUST.entityList[i].Position.Z;

                var vectorTarget = new OpenTK.Vector3(entityPosXingame, entityPosYingame + ModuleRUST.radarForm.overlay.playerFeetVector, entityPosZingame);
                var vectorTargetHead = new OpenTK.Vector3(vectorTarget.X, vectorTarget.Y + ModuleRUST.radarForm.overlay.heightToHead, vectorTarget.Z);

                float distance = (int)Math.Round(CommonHelpers.GetDistance(vectorMe, vectorTarget));

                if (distance < ModuleRUST.settingsForm.settingsJson.Overlay.DrawDistance && distance > 2)
                {
                    var wts = ModuleRUST.radarForm.overlay.WorldToScreenRUST(vectorTarget, out OpenTK.Vector3 coords, Camera.matrix, ModuleRUST.radarForm.overlay.Width, ModuleRUST.radarForm.overlay.Height);
                    var wtsHead = ModuleRUST.radarForm.overlay.WorldToScreenRUST(vectorTargetHead, out OpenTK.Vector3 coordsHead, Camera.matrix, ModuleRUST.radarForm.overlay.Width, ModuleRUST.radarForm.overlay.Height);
                    ModuleRUST.radarForm.overlay.heightToHead = ModuleRUST.radarForm.overlay.heightForStand;

                    if (wts)
                    {
                        ReaderRUST.entityList[i].wtsRender = true;
                      
                        var rectX1 = coordsHead.X - ((coordsHead.Y - coords.Y) / 3);
                        var rectY1 = coordsHead.Y;
                        var rectX2 = coords.X + ((coordsHead.Y - coords.Y) / 3);
                        var rectY2 = coords.Y;
                        var offsetY = 0;

                        renderItemOverlay = new RenderItem();
                        renderItemOverlay.Text = "rectangle";
                        renderItemOverlay.Size = 1;
                        renderItemOverlay.DrawColor = ReaderRUST.entityList[i].ColorEntity;
                        renderItemOverlay.MapPosX = rectX1 + 7;
                        renderItemOverlay.MapPosZ = rectY1 - 7;
                        renderItemOverlay.MapPosXend = rectX2 - 7;
                        renderItemOverlay.MapPosZend = rectY2 + 7;
                        OpenGL.OverlayGeometry.Add(renderItemOverlay);

                        if (ReaderRUST.entityList[i].EntityType == EntityTypeRUST.ESP)
                        {
                            renderItemOverlay = new RenderItem();
                            renderItemOverlay.Text = ReaderRUST.entityList[i].FriendlyName;
                            renderItemOverlay.TextOverlayOutline = true;
                            renderItemOverlay.MapPosX = coords.X;
                            renderItemOverlay.MapPosZ = coords.Y + 10 + offsetY;
                            renderItemOverlay.Size = (int)FontSizes.misc;
                            renderItemOverlay.DrawColor = ReaderRUST.entityList[i].ColorEntity;
                            OpenGL.OverlayText.Add(renderItemOverlay);
                        }
                    }
                    else
                    {
                        ReaderRUST.entityList[i].wtsRender = false;
                    }
                }
            }
        }
    }
}