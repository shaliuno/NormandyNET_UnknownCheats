using MetroFramework;
using MetroFramework.Forms;
using Newtonsoft.Json;
using NormandyNET.Core;
using NormandyNET.Modules.ARMA;
using NormandyNET.Modules.DAYZ;
using NormandyNET.Modules.EFT;
using NormandyNET.Modules.RUST;
using NormandyNET.Settings;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NormandyNET.UI
{
    internal partial class RadarForm : MetroForm
    {
        public async Task<bool> ValidateDeadSwitch()
        {
            allowedToRun = true;
            return true;
        }

        internal void AdjustMapZoomCoeff()
        {
            OnAdjustMapZoomCoeff?.Invoke(this, null);
        }

        private bool AdjustMapZoomIn(bool mwheel = false, int mouseX = 0, int mouseY = 0)
        {
            if (!mapManager.CurrentMap.Equals(string.Empty))
            {
                if (OpenGL.ZoomLevel + mapManager.mapObjects[mapManager.CurrentMap].zoomStepCoef <= 10f)
                {
                    var prevCanvasDiffCoeff = OpenGL.CanvasDiffCoeff;

                    OpenGL.ZoomLevel += mapManager.mapObjects[mapManager.CurrentMap].zoomStepCoef;
                    OpenGL.CanvasSize = OpenGL.CanvasSize + mapManager.mapObjects[mapManager.CurrentMap].zoomStep * OpenGL.ZoomLevel;
                    OpenGL.CanvasDiffCoeff = OpenGL.CanvasSize / OpenGL.CanvasSizeBase;

                    if (mwheel)
                    {
                        debugClickedX = mouseX - openglControlMap.Width / 2;
                        debugClickedY = -mouseY + openglControlMap.Height / 2;
                        debugClickedZoomedX = debugClickedX / prevCanvasDiffCoeff * OpenGL.CanvasDiffCoeff - debugClickedX;
                        debugClickedZoomedY = debugClickedY / prevCanvasDiffCoeff * OpenGL.CanvasDiffCoeff - debugClickedY;
                    }

                    mapDragOffsetX = mapDragOffsetX / prevCanvasDiffCoeff * OpenGL.CanvasDiffCoeff - debugClickedZoomedX;
                    mapDragOffsetZ = mapDragOffsetZ / prevCanvasDiffCoeff * OpenGL.CanvasDiffCoeff - debugClickedZoomedY;
                    mapDragOffsetXLast = mapDragOffsetX;
                    mapDragOffsetZLast = mapDragOffsetZ;
                }

                return true;
            }
            return false;
        }

        private bool AdjustMapZoomOut(bool mwheel = false, int mouseX = 0, int mouseY = 0)
        {
            if (!mapManager.CurrentMap.Equals(string.Empty))
            {
                if (OpenGL.ZoomLevel - mapManager.mapObjects[mapManager.CurrentMap].zoomStepCoef >= 1f)
                {
                    var prevCanvasDiffCoeff = OpenGL.CanvasDiffCoeff;

                    OpenGL.CanvasSize = OpenGL.CanvasSize - mapManager.mapObjects[mapManager.CurrentMap].zoomStep * OpenGL.ZoomLevel;
                    OpenGL.CanvasDiffCoeff = (float)OpenGL.CanvasSize / (float)OpenGL.CanvasSizeBase;
                    OpenGL.ZoomLevel -= mapManager.mapObjects[mapManager.CurrentMap].zoomStepCoef;

                    if (mwheel)
                    {
                        debugClickedX = mouseX - openglControlMap.Width / 2;
                        debugClickedY = -mouseY + openglControlMap.Height / 2;
                        debugClickedZoomedX = debugClickedX / prevCanvasDiffCoeff * OpenGL.CanvasDiffCoeff - debugClickedX;
                        debugClickedZoomedY = debugClickedY / prevCanvasDiffCoeff * OpenGL.CanvasDiffCoeff - debugClickedY;
                    }

                    mapDragOffsetX = mapDragOffsetX / prevCanvasDiffCoeff * OpenGL.CanvasDiffCoeff - debugClickedZoomedX;
                    mapDragOffsetZ = mapDragOffsetZ / prevCanvasDiffCoeff * OpenGL.CanvasDiffCoeff - debugClickedZoomedY;
                    mapDragOffsetXLast = mapDragOffsetX;
                    mapDragOffsetZLast = mapDragOffsetZ;
                }

                return true;
            }

            return false;
        }

        private void PopulateMaps()
        {
        }

        private void SettingsApplyJson()
        {
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
            ApplyButtonImages();
            ShowHideUI();
        }

        private void SettingsLoadJson()
        {
            if (File.Exists(settingsJsonFile))
            {
                try
                {
                    settingsRadar = JsonConvert.DeserializeObject<SettingsRadar>(File.ReadAllText(settingsJsonFile), new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });
                }
                catch (JsonSerializationException ex)
                {
                }
            }

            SettingsValidateJson();
        }

        private void SettingsSaveJson()
        {
            settingsRadar.UserInterface.WindowLocation.X = Location.X;
            settingsRadar.UserInterface.WindowLocation.Y = Location.Y;

            SettingsValidateJson();

            var settingsSerialized = JsonConvert.SerializeObject(settingsRadar, Formatting.Indented, new JsonSerializerSettings
            {
            });

            if (File.Exists(settingsJsonFile))
            {
                System.IO.File.Copy(settingsJsonFile, $"{settingsJsonFile}.bak", true);
            }

            File.WriteAllText(settingsJsonFile, settingsSerialized);
        }

        private void SettingsValidateJson()
        {
            if (settingsRadar.UserInterface.WindowDimensions.Width < 400)
            {
                settingsRadar.UserInterface.WindowDimensions.Width = 400;
            }

            if (settingsRadar.UserInterface.WindowDimensions.Height < 400)
            {
                settingsRadar.UserInterface.WindowDimensions.Height = 400;
            }
        }

        private void SetWindowDimensions()
        {
            if (settingsRadar.UserInterface.FullScreen)
            {
                return;
            }

            Width = settingsRadar.UserInterface.WindowDimensions.Width;
            Height = settingsRadar.UserInterface.WindowDimensions.Height;
        }

        private void SetWindowLocation()
        {
            if (settingsRadar.UserInterface.FullScreen)
            {
                return;
            }

            if (settingsRadar.UserInterface.WindowLocation.X != -32000 && settingsRadar.UserInterface.WindowLocation.Y != -32000)
            {
                Location = new Point(settingsRadar.UserInterface.WindowLocation.X, settingsRadar.UserInterface.WindowLocation.Y);
            }
        }

        private void ShowHideUI()
        {
            var show = settingsRadar.UserInterface.ShowUI;
            metroButtonCenterMap.Visible = show;
            metroButtonShowLoot.Visible = show;
            metroButtonMapDrawText.Visible = show;
            metroButtonFullScreen.Visible = show;
            metroButtonShowHideOverlay.Visible = show;

            metroPanelButtonCenterMap.Visible = show;
            metroPanelButtonShowLoot.Visible = show;
            metroPanelButtonDrawText.Visible = show;
            metroPanelButtonFullScreen.Visible = show;
            metroPanelShowHideOverlay.Visible = show;
        }

        private void StartStopClick(int x, int y)
        {
            if (Started)
            {
                StartStopButtonClickEvent?.Invoke(Started);
            }
            else
            {
                if (startupForm.appVersion < startupForm.minVersion)
                {
                    MetroMessageBox.Show(RadarForm.ActiveForm,
                        $"Your version is: {startupForm.appVersion}\n" +
                        $"Minimal supported version is: {startupForm.minVersion}\n" +
                        $"\n" +
                        $"Please update.\n" +
                        $"Click download button right next to start / stop button." +
                        $"", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }

                var mapForm = new MapSelectorDialog(settingsRadar.Network.ServerAddress, settingsRadar.Network.ServerPort, settingsRadar.Network.lastGameProcessID, this);
                mapForm.Location = new Point(x + 30, y);
                mapForm.TopMost = true;
                var mapFormResult = mapForm.ShowDialog();
                if (mapFormResult == DialogResult.Cancel)
                {
                    return;
                }

                settingsRadar.Network.ServerAddress = mapForm.serverAddress;
                settingsRadar.Network.ServerPort = mapForm.serverPort;
                settingsRadar.Network.lastGameProcessID = mapForm.lastPID;
                SettingsSaveJson();
                StartStopButtonClickEvent?.Invoke(Started);
            }
        }

        internal void ApplyButtonImages()
        {
            metroButtonMapZoomIn.BackgroundImage = Properties.Resources.icon_Plus_Dark;
            metroButtonMapZoomOut.BackgroundImage = Properties.Resources.icon_Minus_Dark;
            metroButtonMapDrawText.BackgroundImage = Properties.Resources.icon_TextOn_Dark;
            metroButtonFullScreen.BackgroundImage = settingsRadar.UserInterface.FullScreen ? Properties.Resources.icon_Collapse_Dark : Properties.Resources.icon_Expand_Dark;
            metroButtonSettings.BackgroundImage = Properties.Resources.icon_SettingsShow_Dark;

            metroButtonButtonShowHideUI.BackgroundImage = settingsRadar.UserInterface.ShowUI ? Properties.Resources.icon_UI_Show_Dark : Properties.Resources.icon_UI_Hide_Dark;
            metroButtonStartStop.BackgroundImage = Started ? Properties.Resources.icon_Stop_Dark : Properties.Resources.icon_Play_Dark;
            metroButtonShowLoot.BackgroundImage = Properties.Resources.icon_BoxOn_Dark;
            metroButtonCenterMap.BackgroundImage = Properties.Resources.icon_CenterOff_Dark;

            metroButtonUpdateLoot.BackgroundImage = Properties.Resources.icon_RefreshDark;
            metroButtonShowHideOverlay.BackgroundImage = Properties.Resources.icon_OverlayDark;
            metroButtonDownloadUpdate.BackgroundImage = Properties.Resources.icon_Download_Dark;
        }

        internal void ApplyButtonImagesFromModule()
        {
            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.eft)
            {
                metroButtonCenterMap.BackgroundImage = ModuleEFT.settingsForm.settingsJson.Map.CenterMap ? Properties.Resources.icon_CenterOn_Dark : Properties.Resources.icon_CenterOff_Dark;
                metroButtonShowLoot.BackgroundImage = ModuleEFT.settingsForm.settingsJson.Loot.Show ? Properties.Resources.icon_BoxOn_Dark : Properties.Resources.icon_BoxOff_Dark;
                metroButtonMapDrawText.BackgroundImage = ModuleEFT.settingsForm.settingsJson.Map.DrawText ? Properties.Resources.icon_TextOn_Dark : Properties.Resources.icon_TextOff_Dark;
                metroButtonCenterMap.BackgroundImage = ModuleEFT.settingsForm.settingsJson.Map.CenterMap ? Properties.Resources.icon_CenterOn_Dark : Properties.Resources.icon_CenterOff_Dark;
            }

            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.dayz)
            {
                metroButtonCenterMap.BackgroundImage = ModuleDAYZ.settingsForm.settingsJson.Map.CenterMap ? Properties.Resources.icon_CenterOn_Dark : Properties.Resources.icon_CenterOff_Dark;
                metroButtonShowLoot.BackgroundImage = ModuleDAYZ.settingsForm.settingsJson.Loot.Show ? Properties.Resources.icon_BoxOn_Dark : Properties.Resources.icon_BoxOff_Dark;
                metroButtonMapDrawText.BackgroundImage = ModuleDAYZ.settingsForm.settingsJson.Map.DrawText ? Properties.Resources.icon_TextOn_Dark : Properties.Resources.icon_TextOff_Dark;
                metroButtonCenterMap.BackgroundImage = ModuleDAYZ.settingsForm.settingsJson.Map.CenterMap ? Properties.Resources.icon_CenterOn_Dark : Properties.Resources.icon_CenterOff_Dark;
            }

            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.rust)
            {
                metroButtonCenterMap.BackgroundImage = ModuleRUST.settingsForm.settingsJson.Map.CenterMap ? Properties.Resources.icon_CenterOn_Dark : Properties.Resources.icon_CenterOff_Dark;
                metroButtonShowLoot.BackgroundImage = ModuleRUST.settingsForm.settingsJson.Loot.Show ? Properties.Resources.icon_BoxOn_Dark : Properties.Resources.icon_BoxOff_Dark;
                metroButtonMapDrawText.BackgroundImage = ModuleRUST.settingsForm.settingsJson.Map.DrawText ? Properties.Resources.icon_TextOn_Dark : Properties.Resources.icon_TextOff_Dark;
                metroButtonCenterMap.BackgroundImage = ModuleRUST.settingsForm.settingsJson.Map.CenterMap ? Properties.Resources.icon_CenterOn_Dark : Properties.Resources.icon_CenterOff_Dark;
            }

            if (SettingsRadar.GetGameType_VMP() == SettingsRadar.GameType.arma)
            {
                metroButtonCenterMap.BackgroundImage = ModuleARMA.settingsForm.settingsJson.Map.CenterMap ? Properties.Resources.icon_CenterOn_Dark : Properties.Resources.icon_CenterOff_Dark;
                metroButtonShowLoot.BackgroundImage = ModuleARMA.settingsForm.settingsJson.Loot.Show ? Properties.Resources.icon_BoxOn_Dark : Properties.Resources.icon_BoxOff_Dark;
                metroButtonMapDrawText.BackgroundImage = ModuleARMA.settingsForm.settingsJson.Map.DrawText ? Properties.Resources.icon_TextOn_Dark : Properties.Resources.icon_TextOff_Dark;
                metroButtonCenterMap.BackgroundImage = ModuleARMA.settingsForm.settingsJson.Map.CenterMap ? Properties.Resources.icon_CenterOn_Dark : Properties.Resources.icon_CenterOff_Dark;
            }

            metroButtonStartStop.BackgroundImage = Started ? Properties.Resources.icon_Stop_Dark : Properties.Resources.icon_Play_Dark;
        }

        private void ApplyFullScreen()
        {
            formResizeInProgress = true;

            if (settingsRadar.UserInterface.FullScreen)
            {
                MinimumSize = this.Size;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                Size = MinimumSize;
                MinimumSize = new Size(200, 200);
            }
            ApplyButtonImages();
            ApplyButtonImagesFromModule();

            formResizeInProgress = false;
        }

        private void InitializeGlControl()
        {
            GraphicsContextFlags flags = GraphicsContextFlags.Default;
            openglControlMap = new GLControl(
                               new GraphicsMode(new ColorFormat(8, 8, 8, 0),
                               24,
                               8,
                               0
                               ),
                               1, 0, flags);

            openglControlMap.AllowDrop = false;
            openglControlMap.BackColor = Color.Black;
            openglControlMap.Dock = DockStyle.Fill;
            openglControlMap.Location = new Point(0, 0);
            openglControlMap.Margin = new Padding(0);
            openglControlMap.Name = "glControl";
            openglControlMap.Size = new System.Drawing.Size(100, 100);
            openglControlMap.TabIndex = 0;
            openglControlMap.VSync = false;
            openglControlMap.Load += openglControlMap_Load;
            openglControlMap.Resize += openglControlMap_Resize;
            openglControlMap.Paint += openglControlMap_Paint;
            openglControlMap.Resize += openglControlMap_Resize;
            openglControlMap.MouseUp += openglControlMap_MouseUp;
            openglControlMap.MouseDown += openglControlMap_MouseDown;
            openglControlMap.MouseMove += openglControlMap_MouseMove;
            openglControlMap.MouseWheel += openglControlMap_MouseWheel;
            openglControlMap.MouseDoubleClick += openglControlMap_MouseDoubleClick;
            metroPanelOpenGL.Controls.Add(openglControlMap);
        }

        private void DeFocusElement() => Focus();

        private async void disableStartButton()
        {
            metroButtonStartStop.Enabled = false;
            await Task.Delay(TimeSpan.FromSeconds(2));
            metroButtonStartStop.Enabled = true;
        }

        internal bool IsVisibleOnControl(float entityX, float entityZ, int extraPixels = 50)
        {
            if (mapManager.CurrentMap.Equals(string.Empty) || mapManager.CurrentMap.Length == 0)
            {
                return true;
            }

            if (
              entityX < GetOpenGlControlSize.Width / 2 - GetOpenGlControlSize.Width - extraPixels - mapDragOffsetX - (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsX + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsXdebug) * OpenGL.CanvasDiffCoeff ||
              entityX > Math.Abs(GetOpenGlControlSize.Width / 2 - GetOpenGlControlSize.Width) + extraPixels - mapDragOffsetX - (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsX + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsXdebug) * OpenGL.CanvasDiffCoeff
              )
            {
                return false;
            }

            if (
              entityZ < GetOpenGlControlSize.Height / 2 - GetOpenGlControlSize.Height - extraPixels - mapDragOffsetZ - (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsY + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsYdebug) * OpenGL.CanvasDiffCoeff ||
              entityZ > Math.Abs(GetOpenGlControlSize.Height / 2 - GetOpenGlControlSize.Height) + extraPixels - mapDragOffsetZ - (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsY + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsYdebug) * OpenGL.CanvasDiffCoeff
              )
            {
                return false;
            }
            return true;
        }

        internal Point GetControlEdgeIntersectionPoint(float entityX, float entityZ, int extraPixels = 35)
        {
            if (mapManager.CurrentMap.Equals(string.Empty))
            {
                return new Point(0, 0);
            }

            var pX = 0;

            var varX1 = GetOpenGlControlSize.Width / 2 - GetOpenGlControlSize.Width - mapDragOffsetX - (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsX + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsXdebug) * OpenGL.CanvasDiffCoeff;
            var varX2 = Math.Abs(GetOpenGlControlSize.Width / 2 - GetOpenGlControlSize.Width) - mapDragOffsetX - (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsX + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsXdebug) * OpenGL.CanvasDiffCoeff;

            if (entityX < varX1)
            {
                pX = (int)Math.Round(varX1, 0) + extraPixels;
            }
            else if (entityX > varX2)
            {
                pX = (int)Math.Round(varX2, 0) - extraPixels;
            }
            else
            {
                pX = (int)entityX;
            }

            var pZ = 0;

            var varZ1 = GetOpenGlControlSize.Height / 2 - GetOpenGlControlSize.Height - mapDragOffsetZ - (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsY + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsYdebug) * OpenGL.CanvasDiffCoeff;
            var varZ2 = Math.Abs(GetOpenGlControlSize.Height / 2 - GetOpenGlControlSize.Height) - mapDragOffsetZ - (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsY + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsYdebug) * OpenGL.CanvasDiffCoeff;

            if (entityZ < varZ1)
            {
                pZ = (int)Math.Round(varZ1, 0) + extraPixels;
            }
            else if (entityZ > varZ2)
            {
                pZ = (int)Math.Round(varZ2, 0) - extraPixels;
            }
            else
            {
                pZ = (int)entityZ;
            }

            return new Point(pX, pZ);
        }

        private void OnLogSaved(object sender, EventArgs args)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
            });
        }

        private void RenderTimerTick()
        {
            do
            {
                CommonHelpers.dateTimeHolder = DateTime.UtcNow;

                if (CommonHelpers.dateTimeHolder > RenderUpdateTime && renderMapComplete && !formResizeInProgress && !mapManager.reloadMaps)
                {
                    if (OpenGL.CenterMap && !mouseFormButtonDown && !mapManager.CurrentMap.Equals(string.Empty))
                    {
                        var invertMap = mapManager.GetInvertMap();
                        var unitSize = mapManager.GetUnitSize();

                        switch (SettingsRadar.GetGameType_VMP())
                        {
                            case SettingsRadar.GameType.eft:
                                myCoordForMapCenterX = (int)Math.Round(CommonHelpers.myIngamePositionX) * OpenGL.CanvasDiffCoeff * invertMap / unitSize;
                                myCoordForMapCenterY = (int)Math.Round(CommonHelpers.myIngamePositionZ) * OpenGL.CanvasDiffCoeff * invertMap / unitSize;
                                break;

                            case SettingsRadar.GameType.rust:
                                myCoordForMapCenterX = (int)Math.Round(CommonHelpers.myIngamePositionX) * OpenGL.CanvasDiffCoeff * invertMap / unitSize;
                                myCoordForMapCenterY = (int)Math.Round(CommonHelpers.myIngamePositionZ) * OpenGL.CanvasDiffCoeff * invertMap / unitSize;
                                break;

                            case SettingsRadar.GameType.dayz:
                                myCoordForMapCenterX = (int)Math.Round(CommonHelpers.myIngamePositionX) * OpenGL.CanvasDiffCoeff * invertMap / unitSize;
                                myCoordForMapCenterY = (int)Math.Round(CommonHelpers.myIngamePositionZ) * OpenGL.CanvasDiffCoeff * invertMap / unitSize;
                                break;

                            case SettingsRadar.GameType.arma:
                                myCoordForMapCenterX = (int)Math.Round(CommonHelpers.myIngamePositionX) * OpenGL.CanvasDiffCoeff * invertMap / unitSize;
                                myCoordForMapCenterY = (int)Math.Round(CommonHelpers.myIngamePositionZ) * OpenGL.CanvasDiffCoeff * invertMap / unitSize;
                                break;

                            case SettingsRadar.GameType.pubg:
                                myCoordForMapCenterX = (int)Math.Round(CommonHelpers.myIngamePositionX) * OpenGL.CanvasDiffCoeff / unitSize;
                                myCoordForMapCenterY = (int)Math.Round(CommonHelpers.myIngamePositionY) * OpenGL.CanvasDiffCoeff * invertMap / unitSize;
                                break;
                        }

                        var hasMapObject = mapManager.mapObjects.TryGetValue(mapManager.CurrentMap, out MapManager.MapObject mapObject);

                        if (hasMapObject)
                        {
                            mapDragOffsetX = (int)Math.Round(myCoordForMapCenterX * -1) - (mapObject.offsetForObjectsX + mapObject.offsetForObjectsXdebug) * OpenGL.CanvasDiffCoeff;
                            mapDragOffsetZ = (int)Math.Round(myCoordForMapCenterY * -1) - (mapObject.offsetForObjectsY + mapObject.offsetForObjectsYdebug) * OpenGL.CanvasDiffCoeff;
                        }

                        mapDragOffsetXLast = mapDragOffsetX;
                        mapDragOffsetZLast = mapDragOffsetZ;
                    }

                    RenderUpdateTime = CommonHelpers.dateTimeHolder.AddMilliseconds(RenderUpdateRate);
                    renderMapComplete = false;
                    OpenGL.Invalidate(openglControlMap);
                }

                if (mapManager.reloadMaps)
                {
                    mapManager.mapObjects.Clear();
                    mapManager.GenerateMapObjects();
                    mapManager.reloadMaps = false;
                }

                Thread.Sleep(5);
            } while (true);
        }

        private void RenderObjects()
        {
            OpenGL.MapText.Sort((x, y) => y.renderLayer.CompareTo(x.renderLayer));
            OpenGL.MapGeometry.Sort((x, y) => y.renderLayer.CompareTo(x.renderLayer));
            OpenGL.MapIcons.Sort((x, y) => y.renderLayer.CompareTo(x.renderLayer));

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGL.openglMapTextureFonts);
            GL.Enable(EnableCap.Blend);

            OpenGL.DrawBitmapTextNew(OpenGL.MapText);

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Color4(Color.Transparent);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            OpenGL.MapGeometry.ForEach(u =>
            {
                GL.Color4(u.DrawColor);

                switch (u.Text)
                {
                    case "line":
                        OpenGL.DrawLines(u.Size, u.MapPosX, u.MapPosZ, u.MapPosXend, u.MapPosZend, u.DrawColor);
                        break;

                    case "point":
                        OpenGL.GlControl_DrawPoint(u.Size, u.MapPosX, u.MapPosZ, u.DrawColor);
                        break;

                    case "linestripple":
                        OpenGL.GlControl_DrawLinesStripped(u.Size, u.MapPosX, u.MapPosZ, u.MapPosXend, u.MapPosZend, u.DrawColor);
                        break;

                    case "linestripple_invert":
                        OpenGL.GlControl_DrawLinesStripped(u.Size, u.MapPosX, u.MapPosZ, u.MapPosXend, u.MapPosZend, u.DrawColor, true);
                        break;

                    case "circle":
                        OpenGL.GlControl_DrawCircle(u.Size, u.MapPosX, u.MapPosZ, u.DrawColor);
                        break;

                    case "circlefill":
                        OpenGL.GlControl_DrawCircleFill(u.Size, u.MapPosX, u.MapPosZ, u.DrawColor);
                        break;

                    case "quad":
                        OpenGL.DrawQuad(u.Size, u.MapPosX, u.MapPosZ, u.DrawColor);
                        break;

                    case "lineloop":
                        OpenGL.DrawLineLoop(u.Size, u.MapPosX, u.MapPosZ, u.MapPosXend, u.MapPosZend, u.DrawColor);
                        break;

                    case "triangle":
                        OpenGL.DrawTriangle(u.Size, u.MapPosX, u.MapPosZ, u.DrawColor);
                        break;

                    default:
                        break;
                }
            });

            GL.Disable(EnableCap.Blend);
            GL.Color4(Color.Transparent);

            GL.BindTexture(TextureTarget.Texture2D, OpenGL.openglMapTextureIcons);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            OpenGL.DrawMapIconsNew(OpenGL.MapIcons);

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Color4(Color.Transparent);

            OpenGL.MapText.Clear();
            OpenGL.MapGeometry.Clear();
            OpenGL.MapIcons.Clear();
        }
    }
}