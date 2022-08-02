using NormandyNET.Helpers;
using NormandyNET.Modules.EFT.Player;
using NormandyNET.Settings;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NormandyNET.Modules.EFT
{
    internal class RendererEFT
    {
        internal OnScreedDisplayEFT onScreedDisplayEFT = new OnScreedDisplayEFT();

        private RenderItem renderItem;
        private RenderItem renderItemOverlay;
        private StringBuilder stringBuilder;

        internal RendererEFT()
        {
            ModuleEFT.radarForm.OnPrepareRenderObjectsEvent += MapPrepareObjects;
            ModuleEFT.radarForm.overlay.OnPrepareOverlayRenderObjectsEvent += MapPrepareObjectsOverlay;
            stringBuilder = new StringBuilder();
        }

        private void MapPrepareObjects(object sender, EventArgs args)
        {
            RepopulateListViewSearch();

            PreparePlayers();
            PrepareLoot();
            PrepareGrenades();
            PrepareExfilPoints();
            PrepareTestObjects();
            onScreedDisplayEFT.RenderOSD();
            onScreedDisplayEFT.ResetCounters();

            ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = false;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = false;
            ReaderEFT.followPlayerUpdateRequested = false;

            ModuleEFT.radarForm.modulesDone = true;
        }

        private void PrepareGrenades()
        {
            var invertMap = ModuleEFT.radarForm.mapManager.GetInvertMap();

            if (ReaderEFT.grenadesList.Count > 0)
            {
                for (int i = 0; i < ReaderEFT.grenadesList.Count; i++)
                {
                    if (ReaderEFT.grenadesList[i].notPresent == true && ReaderEFT.grenadesList[i].canRender == false)
                    {
                        continue;
                    }

                    ReaderEFT.grenadesList[i].GenerateRenderItem();
                }
            }
        }

        private void PrepareExfilPoints()
        {
            if (ModuleEFT.readerEFT.localGameWorld == null || ModuleEFT.readerEFT.localGameWorld.exfiltrationController == null || !ModuleEFT.settingsForm.settingsJson.Map.ExfiltrationPoint)
            {
                return;
            }

            var localPlayer = ReaderEFT.GetLocalPlayer();

            if (localPlayer == null)
            {
                return;
            }

            if (ModuleEFT.readerEFT.localGameWorld.exfiltrationController.exfiltrationPointPMC.Count > 0 && localPlayer.info.Side != EPlayerSide.Savage)
            {
                for (int i = 0; i < ModuleEFT.readerEFT.localGameWorld.exfiltrationController.exfiltrationPointPMC.Count; i++)
                {
                    ModuleEFT.readerEFT.localGameWorld.exfiltrationController.exfiltrationPointPMC[i].GenerateRenderItem();
                }
            }

            if (ModuleEFT.readerEFT.localGameWorld.exfiltrationController.exfiltrationPointPMC.Count > 0 && localPlayer.info.Side == EPlayerSide.Savage)
            {
                for (int i = 0; i < ModuleEFT.readerEFT.localGameWorld.exfiltrationController.exfiltrationPointSCAV.Count; i++)
                {
                    ModuleEFT.readerEFT.localGameWorld.exfiltrationController.exfiltrationPointSCAV[i].GenerateRenderItem();
                }
            }
        }

        private void MapPrepareObjectsOverlay(object sender, EventArgs args)
        {
            if (ModuleEFT.radarForm.overlay != null)
            {
                OverlayPrepareWindowBorder();
                OverlayPrepareCrossHair();
                OverlayPreparePlayers();
                OverlayPrepareLoot();
                OverlayPrepareTestObjects();
                ModuleEFT.radarForm.overlay.modulesDone = true;
            }
        }

        private void PreparePlayers()
        {
            var invertMap = ModuleEFT.radarForm.mapManager.GetInvertMap();

            

            var distance = 0f;


            if (ReaderEFT.playersList.Count > 0)
            {
                var localPlayer = ReaderEFT.GetLocalPlayer();
                for (int i = 0; i < ReaderEFT.playersList.Count; i++)
                {
                    stringBuilder.Clear();

                    if (ReaderEFT.followPlayerUpdateRequested)
                    {
                        ReaderEFT.playersList[i].followPlayerUpdateRequested = true;
                        ReaderEFT.playersList[i].GetPlayerIsFollowThisOne();
                    }

                    if (ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged)
                    {
                        ReaderEFT.playersList[i].updateColors = true;
                        ReaderEFT.playersList[i].GetPlayerColor();
                    }

                    if (!ReaderEFT.playersList[i].canRender)
                    {
                        continue;
                    }

                    if (ReaderEFT.playersList[i].isLocalPlayer == false &&
                        (ReaderEFT.playersList[i].isTeammate != null && (bool)ReaderEFT.playersList[i].isTeammate == false) &&
                        ReaderEFT.playersList[i].IsDeadAlready == false
                        && ReaderEFT.playersList[i].notPresent == false)
                    {
                        switch (ReaderEFT.playersList[i].playerType)
                        {
                            case PlayerTypeEFT.Bot:
                                onScreedDisplayEFT.countBot++;
                                break;

                            case PlayerTypeEFT.BotElite:
                                onScreedDisplayEFT.countBotElite++;
                                break;

                            case PlayerTypeEFT.BotHuman:
                                onScreedDisplayEFT.countScav++;
                                break;

                            case PlayerTypeEFT.Player:
                                onScreedDisplayEFT.countPmc++;
                                break;
                        }
                    }

                    if (ReaderEFT.playersList[i].Position == UnityEngine.Vector3.zero)
                    {
                        continue;
                    }

                    var entityPosXingame = ReaderEFT.playersList[i].Position.x;
                    var entityPosYingame = ReaderEFT.playersList[i].Position.y;
                    var entityPosZingame = ReaderEFT.playersList[i].Position.z;

                    var entityPosXmap = (entityPosXingame * OpenGL.CanvasDiffCoeff * invertMap);
                    var entityPosZmap = (entityPosZingame * OpenGL.CanvasDiffCoeff * invertMap);
                    var entityPosYmap = entityPosYingame;

                    if (ReaderEFT.playersList[i].isFollowThisPlayer == true)
                    {
                        CommonHelpers.myIngamePositionX = entityPosXingame;
                        CommonHelpers.myIngamePositionY = entityPosYingame;
                        CommonHelpers.myIngamePositionZ = entityPosZingame;
                    }

                    if (!ModuleEFT.radarForm.IsVisibleOnControl(entityPosXmap, entityPosZmap))
                    {
                        continue;
                    }

                    

                    UnityEngine.Vector3 vectorMe = new UnityEngine.Vector3(CommonHelpers.myIngamePositionX, 0, CommonHelpers.myIngamePositionZ);
                    UnityEngine.Vector3 vectorTarget = new UnityEngine.Vector3(entityPosXingame, 0, entityPosZingame);

                    var viewAngle = ReaderEFT.playersList[i].ViewAngle.x;

                    if (invertMap == -1)
                    {
                        viewAngle += 180;
                    }

                    viewAngle = Geometry.NormalizeAngle(viewAngle);

                    if (ModuleEFT.settingsForm.settingsJson.Entity.LOS && (!ReaderEFT.playersList[i].notPresent && !ReaderEFT.playersList[i].IsDeadAlready))
                    {
                        float vievAngleFovLeft = viewAngle - 22;
                        float vievAngleFovRight = viewAngle + 22;

                        vievAngleFovLeft = Geometry.NormalizeAngle(vievAngleFovLeft);
                        vievAngleFovRight = Geometry.NormalizeAngle(vievAngleFovRight);

                        var lengthFOV = ModuleEFT.settingsForm.settingsJson.Entity.LineOfSight.Enemy * OpenGL.CanvasDiffCoeff;

                        if (ReaderEFT.playersList[i].isFollowThisPlayer == true)
                        {
                            lengthFOV = ModuleEFT.settingsForm.settingsJson.Entity.LineOfSight.You * OpenGL.CanvasDiffCoeff;
                        }

                        var fov_line_X = (float)(entityPosXmap + (Math.Cos(((viewAngle * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
                        var fov_line_Z = (float)(entityPosZmap + (Math.Sin(((viewAngle * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));

                        var fov_line_X_left = (float)(entityPosXmap + (Math.Cos(((vievAngleFovLeft * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
                        var fov_line_Z_left = (float)(entityPosZmap + (Math.Sin(((vievAngleFovLeft * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
                        var fov_line_X_right = (float)(entityPosXmap + (Math.Cos(((vievAngleFovRight * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
                        var fov_line_Z_right = (float)(entityPosZmap + (Math.Sin(((vievAngleFovRight * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
                        var vectorFOV_Left = new UnityEngine.Vector3(fov_line_X_left, 0, fov_line_Z_left);
                        var vectorFOV_Right = new UnityEngine.Vector3(fov_line_X_right, 0, fov_line_Z_right);

                        var drawLines = true;

                        if (ModuleEFT.settingsForm.settingsJson.Entity.LineOfSight.Solid)
                        {
                            renderItem = new RenderItem();
                            renderItem.Text = "line";
                            renderItem.MapPosX = entityPosXmap;
                            renderItem.MapPosZ = entityPosZmap;
                            renderItem.MapPosXend = fov_line_X;
                            renderItem.MapPosZend = fov_line_Z;
                            renderItem.DrawColor = ReaderEFT.playersList[i].Color;
                            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                            renderItem.Size = 2;
                            OpenGL.MapGeometry.Add(renderItem);
                        }
                        else
                        {
                            renderItem = new RenderItem();
                            renderItem.Text = "linestripple";
                            renderItem.MapPosX = entityPosXmap;
                            renderItem.MapPosZ = entityPosZmap;
                            renderItem.MapPosXend = fov_line_X;
                            renderItem.MapPosZend = fov_line_Z;
                            renderItem.DrawColor = ReaderEFT.playersList[i].Color;
                            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                            renderItem.Size = 2;
                            OpenGL.MapGeometry.Add(renderItem);

                            renderItem = new RenderItem();
                            renderItem.Text = "linestripple_invert";
                            renderItem.MapPosX = entityPosXmap;
                            renderItem.MapPosZ = entityPosZmap;
                            renderItem.MapPosXend = fov_line_X;
                            renderItem.MapPosZend = fov_line_Z;
                            renderItem.DrawColor = System.Windows.Forms.ControlPaint.Dark(ReaderEFT.playersList[i].Color);
                            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                            renderItem.Size = 2;
                            OpenGL.MapGeometry.Add(renderItem);
                        }
                    }

                    renderItem = new RenderItem();
                    renderItem.DrawColor = ReaderEFT.playersList[i].Color;
                    renderItem.IconPositionTexture = IconPositionTexture.player;

                    if (ReaderEFT.playersList[i].isFollowThisPlayer == true)
                    {
                        renderItem.IconPositionTexture = IconPositionTexture.player;
                    }

                    if ((ReaderEFT.playersList[i].notPresent || ReaderEFT.playersList[i].IsDeadAlready) && ModuleEFT.settingsForm.settingsJson.Entity.Bodies)
                    {
                        renderItem.DrawColor = ReaderEFT.playersList[i].Color;

                        if (ReaderEFT.playersList[i].playerType == PlayerTypeEFT.Player)
                        {
                            renderItem.IconPositionTexture = IconPositionTexture.player_dead;
                        }
                        else
                        {
                            renderItem.IconPositionTexture = IconPositionTexture.npc_dead;
                        }
                    }

                    renderItem.MapPosX = entityPosXmap;
                    renderItem.MapPosZ = entityPosZmap;
                    renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;

                    if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Highlight && localPlayer != null && ReaderEFT.playersList[i].playerAddress == localPlayer.improvementsController.aimbot?.lockedOnAddress)
                    {
                        renderItem.DrawColor = Color.Fuchsia;
                    }

                    renderItem.Rotation = viewAngle * -1;
                    renderItem.IconSize = ModuleEFT.settingsForm.settingsJson.Map.IconSizePlayers;

                    if (true)
                    {
                        OpenGL.MapIcons.Add(renderItem);
                    }

                    if (ReaderEFT.playersList[i].notPresent)
                    {
                        continue;
                    }

                    renderItem = new RenderItem();
                    renderItem.MapPosX = entityPosXmap + 20 + OpenGL.CanvasDiffCoeff;
                    renderItem.MapPosZ = entityPosZmap + 15;
                    renderItem.DrawColor = ModuleEFT.settingsForm.settingsJson.Colors.ColorText;
                    renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                    renderItem.TextOverlayOutline = true;
                    renderItem.Size = (int)FontSizes.misc;

                    stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}");

                    if (ReaderEFT.playersList[i].isFollowThisPlayer == false)
                    {
                        var elevation = Math.Round((ReaderEFT.playersList[i].Position.y - CommonHelpers.myIngamePositionY), 0);

                        if (ModuleEFT.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Absolute))
                        {
                            stringBuilder.Append($"({(int)ReaderEFT.playersList[i].Position.y}) ");
                        }

                        if (ModuleEFT.settingsForm.settingsJson.Entity.Elevation.Type.Equals(ElevationType.Relative))
                        {
                            stringBuilder.Append($"({elevation}) ");
                        }

                        if (ModuleEFT.settingsForm.settingsJson.Map.HideTextAroundPlayer && ReaderEFT.playersList[i].distanceToFollowedPlayer < ModuleEFT.settingsForm.settingsJson.Map.HideTextAroundPlayerDistance)
                        {
                            renderItem.Text = stringBuilder.ToString();
                            OpenGL.MapText.Add(renderItem);
                            continue;
                        }

                        if (ModuleEFT.settingsForm.settingsJson.Entity.Elevation.Arrows)
                        {
                            var elevationDiff = (int)Math.Round(elevation / 4, 0);
                            var strSigh = ' ';
                            var renderGlyph = false;

                            if (elevationDiff > 0)
                            {
                                if (elevationDiff > 3)
                                {
                                    elevationDiff = 3;
                                }

                                strSigh = (char)9650;

                                renderGlyph = true;
                            }

                            if (elevationDiff < 0)
                            {
                                if (elevationDiff < -3)
                                {
                                    elevationDiff = -3;
                                }
                                strSigh = (char)9660;

                                renderGlyph = true;
                            }

                            if (renderGlyph)
                            {
                                if (strSigh == (char)9650)
                                {
                                    stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(Color.LimeGreen)}{strSigh}");
                                    stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}");
                                }
                                if (strSigh == (char)9660)
                                {
                                    stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(Color.Red)}{strSigh}");
                                    stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}");
                                }
                            }
                        }

                        if (ModuleEFT.settingsForm.settingsJson.Entity.Distance)
                        {
                            distance = (int)Math.Round(UnityEngine.Vector3.Distance(vectorTarget, vectorMe));
                            stringBuilder.Append($"{distance}m ");
                        }

                        if (ModuleEFT.settingsForm.settingsJson.Entity.Health && ReaderEFT.playersList[i].playerType == PlayerTypeEFT.Player)
                        {
                            if (ReaderEFT.playersList[i].HealthPercent > 0)
                            {
                                stringBuilder.Append($"HP {ReaderEFT.playersList[i].HealthPercent}%");
                            }
                        }

                        stringBuilder.Append($"\n");

                        if (ModuleEFT.settingsForm.settingsJson.Entity.Name)
                        {
                            stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ReaderEFT.playersList[i].Color)}");

                            if (
                                ReaderEFT.playersList[i].info.Nickname.Length > 0 && ReaderEFT.playersList[i].isFollowThisPlayer != true &&

                                (ReaderEFT.playersList[i].info.Side != EPlayerSide.Savage || ReaderEFT.playersList[i].isBoss || ReaderEFT.playersList[i].info.IsStreamerModeAvailable)
                                )
                            {
                                stringBuilder.Append($"{ReaderEFT.playersList[i].info.Nickname}\n");
                            }
                        }

                        if (
                            (
                            ModuleEFT.settingsForm.settingsJson.Entity.Level ||
                            ModuleEFT.settingsForm.settingsJson.Entity.Side ||
                            ModuleEFT.settingsForm.settingsJson.Entity.KDRatio ||
                            ModuleEFT.settingsForm.settingsJson.Entity.ArmorClass

                            ) && ReaderEFT.playersList[i].playerType == PlayerTypeEFT.Player)
                        {
                            var newline = false;
                            stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ReaderEFT.playersList[i].Color)}");

                            if (ModuleEFT.settingsForm.settingsJson.Entity.Side)
                            {
                                if (ReaderEFT.playersList[i].info.Side == EPlayerSide.Bear)
                                {
                                    stringBuilder.Append($"B ");
                                    newline = true;
                                }

                                if (ReaderEFT.playersList[i].info.Side == EPlayerSide.Usec)
                                {
                                    stringBuilder.Append($"U ");
                                    newline = true;
                                }
                            }

                            if (ModuleEFT.settingsForm.settingsJson.Entity.Level)
                            {
                                if (ReaderEFT.playersList[i].info.Level > 0)
                                {
                                    stringBuilder.Append($"L {ReaderEFT.playersList[i].info.Level} ");
                                    newline = true;
                                }
                            }

                            if (ModuleEFT.settingsForm.settingsJson.Entity.KDRatio)
                            {
                                if (ReaderEFT.playersList[i].profile.KillDeathRatio >= ModuleEFT.settingsForm.settingsJson.Entity.KDRatioThreshold)
                                {
                                    stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(Color.Red)}KD {ReaderEFT.playersList[i].profile.KillDeathRatio}©{CommonHelpers.ColorHexConverter(ReaderEFT.playersList[i].Color)} ");
                                }
                                else
                                {
                                    stringBuilder.Append($"KD {ReaderEFT.playersList[i].profile.KillDeathRatio} ");
                                }

                                newline = true;
                            }

                            if (ModuleEFT.settingsForm.settingsJson.Entity.ArmorClass)
                            {
                                stringBuilder.Append($"{ReaderEFT.playersList[i].inventoryController.ArmorClass} ");
                                newline = true;
                            }

                            if (newline)
                            {
                                stringBuilder.Append($"\n");
                            }
                        }

                        if (ModuleEFT.settingsForm.settingsJson.Entity.InventoryValue && ReaderEFT.playersList[i].inventoryController.FullInventoryRead)
                        {
                            if (ModuleEFT.settingsForm.settingsJson.Entity.InventoryValueUseLootFilters == false)
                            {
                                stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}{ReaderEFT.playersList[i].inventoryController.inventoryValueTotalStr}");
                            }
                            else
                            {
                                if (ModuleEFT.settingsForm.settingsJson.Loot.ShowByValue)
                                {
                                    stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}{ReaderEFT.playersList[i].inventoryController.inventoryValueFilteredByPriceStr}");
                                }
                                else
                                {
                                    stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}{ReaderEFT.playersList[i].inventoryController.inventoryValueFilteredByPriorityStr}");
                                }
                            }

                            if (ReaderEFT.playersList[i].inventoryController.inventoryValueFilteredByPriority4Ultra)
                            {
                                stringBuilder.Append($" ©{CommonHelpers.ColorHexConverter(Color.DodgerBlue)}Ultra");
                            }

                            if (ReaderEFT.playersList[i].inventoryController.inventoryValueFilteredByPriority5Super)
                            {
                                stringBuilder.Append($" ©{CommonHelpers.ColorHexConverter(Color.LightCoral)}Super");
                            }

                            stringBuilder.Append($"\n");
                        }

                        if (ModuleEFT.settingsForm.settingsJson.Entity.Weapon && ReaderEFT.playersList[i].inventoryController.Weapons != null && ReaderEFT.playersList[i].inventoryController.Weapons.Length > 0)
                        {
                            stringBuilder.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.ColorText)}{ReaderEFT.playersList[i].inventoryController.Weapons.Truncate(22)}");
                        }

                        renderItem.Text = stringBuilder.ToString();
                        OpenGL.MapText.Add(renderItem);
                    }
                }
            }
        }

        private void PrepareLoot()
        {
            var invertMap = ModuleEFT.radarForm.mapManager.GetInvertMap();

            if (ReaderEFT.lootList.Count > 0)
            {
                for (int i = 0; i < ReaderEFT.lootList.Count; i++)
                {
                    if (ReaderEFT.lootList[i].blacklist == true)
                    {
                        continue;
                    }
                    if (ReaderEFT.lootList[i].notPresent == true)
                    {
                        continue;
                    }
                    ReaderEFT.lootList[i].GenerateRenderItem();
                }
            }
        }

        public static float testCoordX = 0;
        public static float testCoordY = 0;
        public static float testCoordZ = 0;

        private void PrepareTestObjects()
        {
            if (!DebugClass.DebugDraw)
            {
                return;
            }

            var entityPosXingame = testCoordX;
            var entityPosYingame = testCoordY;
            var entityPosZingame = testCoordZ;

            var invertMap = ModuleEFT.radarForm.mapManager.GetInvertMap();

            var entityPosXmap = (entityPosXingame * OpenGL.CanvasDiffCoeff * invertMap);
            var entityPosZmap = (entityPosZingame * OpenGL.CanvasDiffCoeff * invertMap);
            var entityPosYmap = entityPosYingame;

            string text = $"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.PMC)}Test\n";
            text += $"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotHuman)}Test\n";
            text += $"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Bot)}Test\n";
            text += $"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotElite)}Test\n";

            var count = 50;
            var offset = 2;

            for (int i = 0; i < count; i++)
            {
                renderItem = new RenderItem();
                renderItem.Text = text;
                renderItem.TextOverlayOutline = true;
                renderItem.MapPosX = i * offset;
                renderItem.MapPosZ = i * offset;
                renderItem.DrawColor = ModuleEFT.settingsForm.settingsJson.Colors.ColorOSD;
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
                renderItem.DrawColor = ModuleEFT.settingsForm.settingsJson.Colors.ColorOSD;
                renderItem.Size = (int)FontSizes.name;
                renderItem.renderLayer = RenderLayers.OSD;
                OpenGL.MapText.Add(renderItem);
            }

            var gridSize = 50;
            var gridCount = 10;

            for (int i = -10; i < gridCount; i++)
            {
                renderItem = new RenderItem();
                renderItem.Text = "line";
                renderItem.Size = 1;
                renderItem.MapPosX = -500;
                renderItem.MapPosZ = gridSize * i;
                renderItem.MapPosXend = 500;
                renderItem.MapPosZend = gridSize * i;
                renderItem.DrawColor = Color.Blue;
                renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                OpenGL.MapGeometry.Add(renderItem);
            }

            for (int i = -10; i < gridCount; i++)
            {
                renderItem = new RenderItem();
                renderItem.Text = "line";
                renderItem.Size = 1;

                renderItem.MapPosX = gridSize * i;
                renderItem.MapPosZ = -500;
                renderItem.MapPosXend = gridSize * i;
                renderItem.MapPosZend = 500;
                renderItem.DrawColor = Color.Red;
                renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;
                OpenGL.MapGeometry.Add(renderItem);
            }

            return;

            return;
            GL.PushMatrix();

            float width = 20;
            /*задняя*/
            GL.Color3(Color.Red);
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, width, 0);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*левая*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(0, width, width);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*нижняя*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, 0, 0);
            GL.End();

            /*верхняя*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, width, 0);
            GL.Vertex3(0, width, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            /*передняя*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(0, width, width);
            GL.End();

            /*правая*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, width, 0);
            GL.Vertex3(width, width, 0);
            GL.Vertex3(width, 0, 0);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(0, width, width);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(0, width, width);
            GL.Vertex3(0, width, 0);
            GL.End();

            GL.Color3(Color.White);
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(50, 0, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 50, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 50);
            GL.End();

            GL.PopMatrix();

            int of = 0;

            OpenGL.MapGeometry.Add(new RenderItem()
            {
                Text = "point",
                MapPosX = 0,
                MapPosZ = 0,
                DrawColor = Color.Fuchsia,
                Size = 6
            }
            );

            renderItem = new RenderItem();
            renderItem.Text = "linestripple";
            renderItem.MapPosX = 60;
            renderItem.MapPosZ = 60;
            renderItem.MapPosXend = 120;
            renderItem.MapPosZend = 120;
            renderItem.DrawColor = Color.Red;
            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;

            renderItem.Size = 2;
            OpenGL.MapGeometry.Add(renderItem);

            renderItem = new RenderItem();
            renderItem.IconPositionTexture = IconPositionTexture.player;
            renderItem.MapPosX = 60;
            renderItem.MapPosZ = 60;
            renderItem.DrawColor = Color.Red;
            renderItem.Rotation = 180 * -1;
            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;

            OpenGL.MapIcons.Add(renderItem);

            renderItem = new RenderItem();
            renderItem.Text = "testtext";

            renderItem.MapPosX = 20 + OpenGL.CanvasDiffCoeff;
            renderItem.MapPosZ = 15;
            renderItem.DrawColor = ModuleEFT.settingsForm.settingsJson.Colors.ColorText;

            renderItem.TextOverlayOutline = true;
            renderItem.renderLayer = RenderLayers.PlayersPriorityHigh;

            OpenGL.MapText.Add(renderItem);
        }

        private void RenderTimerTickEFT()
        {
        }

        private void OverlayPrepareCrossHair()
        {
            var crosshairOffset = 8;
            var crosshairLength = crosshairOffset + 8;

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleEFT.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = -crosshairOffset;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.MapPosXend = -crosshairLength;
            renderItemOverlay.MapPosZend = 0;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleEFT.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = crosshairOffset;
            renderItemOverlay.MapPosZ = 0;
            renderItemOverlay.MapPosXend = crosshairLength;
            renderItemOverlay.MapPosZend = 0;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleEFT.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = -crosshairOffset;
            renderItemOverlay.MapPosXend = 0;
            renderItemOverlay.MapPosZend = -crosshairLength;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);

            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = ModuleEFT.radarForm.overlay.geometryPixelSize;
            renderItemOverlay.DrawColor = Color.Fuchsia;
            renderItemOverlay.MapPosX = 0;
            renderItemOverlay.MapPosZ = crosshairOffset;
            renderItemOverlay.MapPosXend = 0;
            renderItemOverlay.MapPosZend = crosshairLength;
            OpenGL.OverlayGeometry.Add(renderItemOverlay);
        }

        private void OverlayPreparePlayers()
        {
            if (ReaderEFT.playersList.Count > 0)
            {
                for (int i = 0; i < ReaderEFT.playersList.Count; i++)
                {
                    if (!ReaderEFT.playersList[i].canRender)
                    {
                        continue;
                    }

                    if (ReaderEFT.playersList[i].Position == UnityEngine.Vector3.zero)
                    {
                        continue;
                    }

                    if (ReaderEFT.playersList[i].isFollowThisPlayer == true)
                    {
                        continue;
                    }

                    if (ReaderEFT.playersList[i].notPresent || ReaderEFT.playersList[i].IsDeadAlready)
                    {
                        continue;
                    }

                    if (ModuleEFT.readerEFT.fpsCamera == null)
                    {
                        return;
                    }

                    var myPosX = CommonHelpers.myIngamePositionX;
                    var myPosY = CommonHelpers.myIngamePositionY;
                    var myPosZ = CommonHelpers.myIngamePositionZ;

                    var vectorMe = new OpenTK.Vector3(myPosX, myPosY, myPosZ);

                    var entityPosXingame = ReaderEFT.playersList[i].Position.x;
                    var entityPosYingame = ReaderEFT.playersList[i].Position.y;
                    var entityPosZingame = ReaderEFT.playersList[i].Position.z;

                    var vectorTarget = new OpenTK.Vector3(entityPosXingame, entityPosYingame + ModuleEFT.radarForm.overlay.playerFeetVector, entityPosZingame);
                    var vectorTargetHead = new OpenTK.Vector3(vectorTarget.X, vectorTarget.Y + ModuleEFT.radarForm.overlay.heightToHead, vectorTarget.Z);

                    float distance = (int)Math.Round(CommonHelpers.GetDistance(vectorMe, vectorTarget));

                    if (distance < ModuleEFT.settingsForm.settingsJson.Overlay.DrawDistance && distance > 2)
                    {
                        var wts = ModuleEFT.radarForm.overlay.WorldToScreen(
                            vectorTarget, out OpenTK.Vector3 coords, ModuleEFT.readerEFT.fpsCamera.GetViewMartix(), ModuleEFT.radarForm.overlay.Width, ModuleEFT.radarForm.overlay.Height);

                        var wtsHead = ModuleEFT.radarForm.overlay.WorldToScreen(
                            vectorTargetHead, out OpenTK.Vector3 coordsHead, ModuleEFT.readerEFT.fpsCamera.GetViewMartix(), ModuleEFT.radarForm.overlay.Width, ModuleEFT.radarForm.overlay.Height);

                        ModuleEFT.radarForm.overlay.heightToHead = ModuleEFT.radarForm.overlay.heightForStand;
                        if (wts)
                        {
                            ReaderEFT.playersList[i].wtsRender = true;
                            

                            var rectX1 = coordsHead.X - ((coordsHead.Y - coords.Y) / 3);
                            var rectY1 = coordsHead.Y;
                            var rectX2 = coords.X + ((coordsHead.Y - coords.Y) / 3);
                            var rectY2 = coords.Y;
                            var offsetY = 0;

                            renderItemOverlay = new RenderItem();
                            renderItemOverlay.Text = "rectangle";
                            renderItemOverlay.Size = 1;
                            renderItemOverlay.DrawColor = ReaderEFT.playersList[i].Color;
                            renderItemOverlay.MapPosX = rectX1 + 7;
                            renderItemOverlay.MapPosZ = rectY1 - 7;
                            renderItemOverlay.MapPosXend = rectX2 - 7;
                            renderItemOverlay.MapPosZend = rectY2 + 7;
                            OpenGL.OverlayGeometry.Add(renderItemOverlay);

                            if (ModuleEFT.settingsForm.settingsJson.Entity.Distance)
                            {
                                renderItemOverlay = new RenderItem();
                                renderItemOverlay.Text = (int)distance + "m";
                                renderItemOverlay.TextOverlayOutline = true;
                                renderItemOverlay.MapPosX = coords.X;
                                renderItemOverlay.MapPosZ = coords.Y + 15 + offsetY;
                                renderItemOverlay.Size = (int)FontSizes.misc;
                                renderItemOverlay.DrawColor = ReaderEFT.playersList[i].Color;
                                OpenGL.OverlayText.Add(renderItemOverlay);
                                offsetY += 15;
                            }

                            bool drawBones = false;

                            if (distance < ModuleEFT.settingsForm.settingsJson.Overlay.Bones.DrawDistance)
                            {
                                if ((ReaderEFT.playersList[i].playerType == PlayerTypeEFT.Player || ReaderEFT.playersList[i].playerType == PlayerTypeEFT.BotHuman) && ModuleEFT.settingsForm.settingsJson.Overlay.Bones.Humans == true)
                                {
                                    drawBones = true;
                                }

                                if ((ReaderEFT.playersList[i].playerType == PlayerTypeEFT.Bot || ReaderEFT.playersList[i].playerType == PlayerTypeEFT.BotElite) && ModuleEFT.settingsForm.settingsJson.Overlay.Bones.AI == true)
                                {
                                    drawBones = true;
                                }
                            }

                            if (drawBones)
                            {
                                if (ModuleEFT.settingsForm.settingsJson.Overlay.Bones.HighDetail)
                                {
                                    DrawBonesHD(i, distance);
                                }
                                else
                                {
                                    DrawBonesLD(i, distance);
                                }
                            }
                        }
                        else
                        {
                            ReaderEFT.playersList[i].wtsRender = false;
                        }
                    }
                }
            }
        }

        private void DrawBonesLD(int i, float distance)
        {
            for (int i2 = 0; i2 < Sketelon.skeleton_low_detail.Count; i2++)

            {
                OpenTK.Vector3 boneTarget = OpenTK.Vector3.Zero;
                OpenTK.Vector3 boneTargetPrev = OpenTK.Vector3.Zero;
                OpenTK.Vector3 coordsResult = OpenTK.Vector3.Zero;
                OpenTK.Vector3 coordsPrevResult = OpenTK.Vector3.Zero;

                for (int boneVal = 0; boneVal < Sketelon.skeleton_low_detail[i2].Count; boneVal++)
                {
                    if (ReaderEFT.playersList[i].playerBody.bonesClassDict.TryGetValue(Sketelon.skeleton_low_detail[i2][boneVal], out BoneClass boneClass))
                    {
                        boneTarget = new OpenTK.Vector3(boneClass.position.x, boneClass.position.y, boneClass.position.z);
                        ModuleEFT.radarForm.overlay.WorldToScreen(
                            boneTarget, out OpenTK.Vector3 _coordsResult, ModuleEFT.readerEFT.fpsCamera.GetViewMartix(), ModuleEFT.radarForm.overlay.Width * 2, ModuleEFT.radarForm.overlay.Height);

                        coordsResult = _coordsResult;

                        renderItemOverlay = new RenderItem();
                        renderItemOverlay.Text = "line";
                        renderItemOverlay.Size = 2;
                        renderItemOverlay.DrawColor = ReaderEFT.playersList[i].Color;
                        renderItemOverlay.MapPosX = coordsResult.X;
                        renderItemOverlay.MapPosZ = coordsResult.Y;
                        renderItemOverlay.MapPosXend = coordsResult.X + 1;
                        renderItemOverlay.MapPosZend = coordsResult.Y + 1;

                        if (boneVal == 0)
                        {
                            boneTargetPrev = boneTarget;
                            coordsPrevResult = _coordsResult;
                        }

                        if (boneVal != 0)
                        {
                            renderItemOverlay.MapPosXend = coordsPrevResult.X;
                            renderItemOverlay.MapPosZend = coordsPrevResult.Y;
                            boneTargetPrev = boneTarget;
                            coordsPrevResult = _coordsResult;
                        }

                        OpenGL.OverlayGeometry.Add(renderItemOverlay);

                        if (Sketelon.skeleton_low_detail[i2][boneVal] == Bone.HumanHead)
                        {
                            var radius = (int)Math.Ceiling(20 / distance / ModuleEFT.radarForm.overlay.fixedHeadCircleSize / 2);

                            renderItemOverlay = new RenderItem();
                            renderItemOverlay.Text = "circle";
                            renderItemOverlay.Size = radius;
                            renderItemOverlay.DrawColor = ReaderEFT.playersList[i].Color;
                            renderItemOverlay.MapPosX = coordsResult.X;
                            renderItemOverlay.MapPosZ = coordsResult.Y;
                            OpenGL.OverlayGeometry.Add(renderItemOverlay);
                        }
                    }
                }
            }
        }

        private void DrawBonesHD(int i, float distance)
        {
            for (int i2 = 0; i2 < Sketelon.skeleton.Count; i2++)

            {
                OpenTK.Vector3 boneTarget = OpenTK.Vector3.Zero;
                OpenTK.Vector3 boneTargetPrev = OpenTK.Vector3.Zero;
                OpenTK.Vector3 coordsResult = OpenTK.Vector3.Zero;
                OpenTK.Vector3 coordsPrevResult = OpenTK.Vector3.Zero;

                for (int boneVal = 0; boneVal < Sketelon.skeleton[i2].Count; boneVal++)
                {
                    if (ReaderEFT.playersList[i].playerBody.bonesClassDict.TryGetValue(Sketelon.skeleton[i2][boneVal], out BoneClass boneClass))
                    {
                        boneTarget = new OpenTK.Vector3(boneClass.position.x, boneClass.position.y, boneClass.position.z);
                        ModuleEFT.radarForm.overlay.WorldToScreen(
                            boneTarget, out OpenTK.Vector3 _coordsResult, ModuleEFT.readerEFT.fpsCamera.GetViewMartix(), ModuleEFT.radarForm.overlay.Width * 2, ModuleEFT.radarForm.overlay.Height);

                        coordsResult = _coordsResult;

                        renderItemOverlay = new RenderItem();
                        renderItemOverlay.Text = "line";
                        renderItemOverlay.Size = 2;
                        renderItemOverlay.DrawColor = ReaderEFT.playersList[i].Color;
                        renderItemOverlay.MapPosX = coordsResult.X;
                        renderItemOverlay.MapPosZ = coordsResult.Y;
                        renderItemOverlay.MapPosXend = coordsResult.X + 1;
                        renderItemOverlay.MapPosZend = coordsResult.Y + 1;

                        if (boneVal == 0)
                        {
                            boneTargetPrev = boneTarget;
                            coordsPrevResult = _coordsResult;
                        }

                        if (boneVal != 0)
                        {
                            renderItemOverlay.MapPosXend = coordsPrevResult.X;
                            renderItemOverlay.MapPosZend = coordsPrevResult.Y;
                            boneTargetPrev = boneTarget;
                            coordsPrevResult = _coordsResult;
                        }

                        OpenGL.OverlayGeometry.Add(renderItemOverlay);

                        if (Sketelon.skeleton[i2][boneVal] == Bone.HumanHead)
                        {
                            var radius = (int)Math.Ceiling(20 / distance / ModuleEFT.radarForm.overlay.fixedHeadCircleSize / 2);

                            renderItemOverlay = new RenderItem();
                            renderItemOverlay.Text = "circle";
                            renderItemOverlay.Size = radius;
                            renderItemOverlay.DrawColor = ReaderEFT.playersList[i].Color;
                            renderItemOverlay.MapPosX = coordsResult.X;
                            renderItemOverlay.MapPosZ = coordsResult.Y;
                            OpenGL.OverlayGeometry.Add(renderItemOverlay);
                        }
                    }
                }
            }
        }

        private void OverlayPrepareLoot()
        {
            if (!ModuleEFT.settingsForm.settingsJson.Loot.Show)
            {
                return;
            }

            if (ReaderEFT.lootList.Count > 0)
            {
                for (int i = 0; i < ReaderEFT.lootList.Count; i++)
                {
                    if (ModuleEFT.readerEFT.pointer.PlayerCount == 0)
                    {
                        break;
                    }

                    try
                    {
                        if (ReaderEFT.lootList[i] == null)
                        {
                                                        break;
                        }

                        if (ReaderEFT.lootList[i].blacklist == true)
                        {
                            continue;
                        }

                        if (ReaderEFT.lootList[i].notPresent == true)
                        {
                            continue;
                        }
                    }
                    catch
                    {
                                                break;
                    }

                    ReaderEFT.lootList[i].GenerateRenderItemOverlay();
                }
            }
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
            renderItemOverlay = new RenderItem();
            renderItemOverlay.Text = "rectangle";
            renderItemOverlay.Size = 1;
            renderItemOverlay.DrawColor = Color.Purple;
            renderItemOverlay.MapPosX = -ModuleEFT.radarForm.overlay.Width / 2f + 1;
            renderItemOverlay.MapPosZ = -ModuleEFT.radarForm.overlay.Height / 2f + 1;
            renderItemOverlay.MapPosXend = (ModuleEFT.radarForm.overlay.Width / 2) + 1;
            renderItemOverlay.MapPosZend = (ModuleEFT.radarForm.overlay.Height / 2);
            OpenGL.OverlayGeometry.Add(renderItemOverlay);
        }

        private void RepopulateListViewSearch()
        {
            if (LootItemHelper.FindLootRebuildTable && CommonHelpers.dateTimeHolder > LootItemHelper.FindLootRebuildTableTime)
            {
                
                LootItemHelper.FindLootRebuildTable = false;

                foreach (string entryPre in LootItemHelper.LootFriendlyNamesCanShow)
                {
                    var entry = Regex.Replace(entryPre, @"\s+", "");

                    if (ModuleEFT.settingsForm.lootSearchRegexp != null && (ModuleEFT.settingsForm.lootSearchRegexp.IsMatch(entry) || ModuleEFT.settingsForm.lootSearchRegexp.IsMatch(entryPre)))
                    {
                        ListViewItem item = new ListViewItem(new[] { entryPre });

                        if (ModuleEFT.settingsForm.metroListViewLootSearchHighlight.Items.Contains(item) == false)
                        {
                            ModuleEFT.settingsForm.metroListViewLootSearchHighlight.Items.Add(item);
                            LootItemHelper.LootFriendlyNamesToShow.Add(entryPre);
                        }
                    }
                }

                foreach (string entryPre in LootItemHelper.LootShortNamesCanShow)
                {
                    var entry = Regex.Replace(entryPre, @"\s+", "");

                    if (ModuleEFT.settingsForm.lootSearchRegexp != null && (ModuleEFT.settingsForm.lootSearchRegexp.IsMatch(entry) || ModuleEFT.settingsForm.lootSearchRegexp.IsMatch(entryPre)))
                    {
                        ListViewItem item = new ListViewItem(new[] { entry });

                        if (ModuleEFT.settingsForm.metroListViewLootSearchHighlight.Items.Contains(item) == false)
                        {
                            ModuleEFT.settingsForm.metroListViewLootSearchHighlight.Items.Add(item);
                            LootItemHelper.LootShortNamesToShow.Add(entry);
                        }
                    }
                }

                foreach (string entryPre in LootItemHelper.LootCategoriesCanShow)
                {
                    var entry = Regex.Replace(entryPre, @"\s+", "");

                    if (ModuleEFT.settingsForm.lootSearchRegexp != null && (ModuleEFT.settingsForm.lootSearchRegexp.IsMatch(entry) || ModuleEFT.settingsForm.lootSearchRegexp.IsMatch(entryPre)))
                    {
                        ListViewItem item = new ListViewItem(new[] { entry });

                        if (ModuleEFT.settingsForm.metroListViewLootSearchHighlight.Items.Contains(item) == false)
                        {
                            ModuleEFT.settingsForm.metroListViewLootSearchHighlight.Items.Add(item);
                            LootItemHelper.LootCategoriesToShow.Add(entry);
                        }
                    }
                }

                foreach (string entry in LootItemHelper.LootCategoriesCanShow)
                {
                    if (ModuleEFT.settingsForm.lootSearchRegexp != null && (ModuleEFT.settingsForm.lootSearchRegexp.IsMatch(entry)))
                    {
                        ListViewItem item = new ListViewItem(new[] { entry });

                        if (ModuleEFT.settingsForm.metroListViewLootSearchHighlight.Items.Contains(item) == false)
                        {
                            ModuleEFT.settingsForm.metroListViewLootSearchHighlight.Items.Add(item);
                            LootItemHelper.LootCategoriesToShow.Add(entry);
                        }
                    }
                }

                ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
                return;
            }
        }
    }
}