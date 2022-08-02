using MetroFramework;
using MetroFramework.Forms;
using NormandyNET.Core;
using NormandyNET.Modules.ARMA;
using NormandyNET.Modules.DAYZ;
using NormandyNET.Modules.EFT;
using NormandyNET.Modules.RUST;
using NormandyNET.Settings;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NormandyNET.Core.MapManager;

namespace NormandyNET.UI
{
    internal partial class RadarForm : MetroForm
    {
        public RadarForm(StartupForm _startupForm)
        {
            InitializeComponent();
            startupForm = _startupForm;
            metroLabelVersion.Text = $"{startupForm.appVersion.Major}.{startupForm.appVersion.Minor}.{startupForm.appVersion.Build}.{startupForm.appVersion.Revision} - commit: {AssemblyInfo.GetGitHash()}";
        }

        #region AllowResizeWindowAnySide

        private Rectangle Bottom
        { get { return new Rectangle(0, this.ClientSize.Height - windowGripSizeBorder, this.ClientSize.Width, windowGripSizeBorder); } }

        private Rectangle BottomLeft
        { get { return new Rectangle(0, this.ClientSize.Height - windowGripSizeBorder, windowGripSizeBorder, windowGripSizeBorder); } }

        private Rectangle BottomRight
        { get { return new Rectangle(this.ClientSize.Width - windowGripSizeBorder, this.ClientSize.Height - windowGripSizeBorder, windowGripSizeBorder, windowGripSizeBorder); } }

        private Rectangle Left
        { get { return new Rectangle(0, 0, windowGripSizeBorder, this.ClientSize.Height); } }

        private Rectangle Right
        { get { return new Rectangle(this.ClientSize.Width - windowGripSizeBorder, 0, windowGripSizeBorder, this.ClientSize.Height); } }

        private Rectangle Top
        { get { return new Rectangle(0, 0, this.ClientSize.Width, windowGripSizeBorder); } }

        private Rectangle TopLeft
        { get { return new Rectangle(0, 0, windowGripSizeBorder, windowGripSizeBorder); } }

        private Rectangle TopRight
        { get { return new Rectangle(this.ClientSize.Width - windowGripSizeBorder, 0, windowGripSizeBorder, windowGripSizeBorder); } }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);
            if (message.Msg == 0x84)
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }

        #endregion AllowResizeWindowAnySide

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs ex)
        {
            string xo = ex.KeyChar.ToString();

            if (xo == "-")
            {
                AdjustMapZoomOut();
            }
            if (xo == "+")
            {
                AdjustMapZoomIn();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingsSaveJson();
            OnRadarFormCloseEvent?.Invoke(this, null);
            DialogResult dialogResult = MetroMessageBox.Show(this, "Close App?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                SettingsSaveJson();
                Environment.Exit(0);
            }
            else if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Visible = false;
            fastRandom = new SharpNeatLib.Maths.FastRandom();

            Icon = Properties.Resources.Nautilus;
            Text = CommonHelpers.RandomString(fastRandom.Next(10, 30));

            SettingsLoadJson();
            SettingsApplyJson();

            mapManager = new MapManager();
            overlay = new Overlay();
            InitializeGlControl();
            GetOpenGlControlSize = openglControlMap.Size;

            switch (SettingsRadar.GetGameType_VMP())
            {
                case SettingsRadar.GameType.eft:
                    gameExecutable = "EFT_Radar";
                    moduleEFT = new ModuleEFT(this);
                    ModuleEFT.settingsForm.OnShowLootChecked += OnShowLootChecked;
                    ModuleEFT.settingsForm.OnRadarFormOpacitySliderValueChangeEvent += SetOpacityRadarForm;
                    ModuleEFT.settingsForm.OnRadarFormTopMostToggleChangeEvent += SetTopMostRadarForm;

                    break;

                case SettingsRadar.GameType.dayz:
                    gameExecutable = "DAYZ_Radar";
                    moduleDAYZ = new ModuleDAYZ(this);
                    ModuleDAYZ.settingsForm.OnShowLootChecked += OnShowLootChecked;
                    ModuleDAYZ.settingsForm.OnRadarFormOpacitySliderValueChangeEvent += SetOpacityRadarForm;
                    ModuleDAYZ.settingsForm.OnRadarFormTopMostToggleChangeEvent += SetTopMostRadarForm;

                    break;

                case SettingsRadar.GameType.arma:
                    gameExecutable = "ARMA_Radar";
                    moduleARMA = new ModuleARMA(this);
                    ModuleARMA.settingsForm.OnShowLootChecked += OnShowLootChecked;
                    ModuleARMA.settingsForm.OnRadarFormOpacitySliderValueChangeEvent += SetOpacityRadarForm;
                    ModuleARMA.settingsForm.OnRadarFormTopMostToggleChangeEvent += SetTopMostRadarForm;

                    break;

                case SettingsRadar.GameType.rust:
                    gameExecutable = "RUST_Radar";
                    moduleRUST = new ModuleRUST(this);
                    ModuleRUST.settingsForm.OnShowLootChecked += OnShowLootChecked;
                    ModuleRUST.settingsForm.OnRadarFormOpacitySliderValueChangeEvent += SetOpacityRadarForm;
                    ModuleRUST.settingsForm.OnRadarFormTopMostToggleChangeEvent += SetTopMostRadarForm;

                    break;

                default:
                    break;
            }

            HelperLogger.Init($"{gameExecutable}_{startupForm.appVersion.Major}_{startupForm.appVersion.Minor}_{startupForm.appVersion.Build}_{startupForm.appVersion.Revision}");
            HelperLogger.saveToFile = settingsRadar.Debug.TraceLogEnabled;
            HelperLogger.OnLogSavedEvent += OnLogSaved;

            SetOpacityRadarForm(settingsRadar.UserInterface.Opacity);
            SetTopMostRadarForm(settingsRadar.UserInterface.TopMost);

            ApplyButtonImagesFromModule();
            ApplyFullScreen();
            SetWindowLocation();
            SetWindowDimensions();

            PopulateMaps();
        }

        private void SetOpacityRadarForm(float str)
        {
            Opacity = str / 100;
            settingsRadar.UserInterface.Opacity = str;
        }

        private void SetTopMostRadarForm(bool value)
        {
            TopMost = value;
            settingsRadar.UserInterface.TopMost = TopMost;
            if (TopMost)
            {
                BringToFront();
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        public static bool IsTouchEnabled()
        {
            const int MAXTOUCHES_INDEX = 95;
            int maxTouches = GetSystemMetrics(MAXTOUCHES_INDEX);

            return maxTouches > 0;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            formResizeInProgress = true;

            var h = Size.Height;
            var w = Size.Width;

            while (CommonHelpers.IsOdd(h))
            {
                h -= 1;
            }

            while (CommonHelpers.IsOdd(w))
            {
                w -= 1;
            }

            Width = w;
            Height = h;

            formResizeInProgress = false;
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            formResizeInProgress = true;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            settingsRadar.UserInterface.WindowDimensions.Width = Width;
            settingsRadar.UserInterface.WindowDimensions.Height = Height;
            formResizeInProgress = false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            OpenGL.LoadFont();

            startupForm.OnDownloadAvailable += OnDownloadAvailable;

            renderThread = new Thread(RenderTimerTick);
            renderThread.Start();

            Thread.Sleep(100);
        }

        private async void OnDownloadAvailable(object sender, EventArgs e)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            this.Invoke((MethodInvoker)delegate ()
            {
                metroPanelButtonDownloadUpdate.Visible = true;
                metroButtonDownloadUpdate.Visible = true;
                metroButtonDownloadUpdate.BackColor = Color.DarkGreen;
            });
        }

        private void metroButtonButtonShowHideUI_Click(object sender, EventArgs e)
        {
            settingsRadar.UserInterface.ShowUI = !settingsRadar.UserInterface.ShowUI;
            ApplyButtonImages();
            ShowHideUI();
        }

        private void metroButtonCenterMap_Click(object sender, EventArgs e)
        {
            OnCenterMapButtonClick?.Invoke(this, null);
            ApplyButtonImagesFromModule();
        }

        private void metroButtonFullScreen_Click(object sender, EventArgs e)
        {
            settingsRadar.UserInterface.FullScreen = !settingsRadar.UserInterface.FullScreen;
            ApplyFullScreen();
        }

        private void metroButtonMapDrawText_Click(object sender, EventArgs e)
        {
            OnMapDrawTextButtonClick?.Invoke(this, null);
            ApplyButtonImagesFromModule();
        }

        private void metroButtonMapZoomIn_Click(object sender, EventArgs e)
        {
            AdjustMapZoomIn();
        }

        private void metroButtonMapZoomOut_Click(object sender, EventArgs e)
        {
            AdjustMapZoomOut();
        }

        private void metroButtonSettings_Click(object sender, EventArgs e)
        {
            OnSettingButtonClick?.Invoke(this.Location.X, this.Location.Y);
        }

        private void OnShowLootChecked(object sender, EventArgs e)
        {
            ApplyButtonImagesFromModule();
        }

        private void metroButtonShowLoot_Click(object sender, EventArgs e)
        {
            OnMapShowLootButtonClick?.Invoke(this, null);
            ApplyButtonImagesFromModule();
        }

        private void metroButtonStartStop_Click(object sender, EventArgs e)
        {
            StartStopClick(this.Location.X, this.Location.Y);
        }

        private void metroButtonUpdateLoot_Click(object sender, EventArgs e)
        {
            OnUpdateLootButtonClick?.Invoke(this, null);
        }

        private void openglControlMap_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Black);
            OpenGL.GlControl_LoadTextures();
            openglControlMapLoaded = true;

            OpenGL.SetupViewport(openglControlMap.Width, openglControlMap.Height);
            OpenGL.Invalidate(openglControlMap);

            int major, minor;
            GL.GetInteger(GetPName.MajorVersion, out major);
            GL.GetInteger(GetPName.MinorVersion, out minor);
        }

        private void openglControlMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            return;
            if (e.Button == MouseButtons.Right)
            {
            }
        }

        private void openglControlMap_MouseDown(object sender, MouseEventArgs e)
        {
            mouseFormButtonDown = true;
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
            {
                mousePositionStartX = e.X;
                mousePositionStartY = e.Y;
            }

            if (e.Button == MouseButtons.Right)
            {
                var cursorPos = System.Windows.Forms.Cursor.Position;

                OpenContextMenuMap(mousePositionStartX, mousePositionStartY, cursorPos);
                metroContextMenuMap.Show(this, new Point(e.X, e.Y));
            }
        }

        private void OpenContextMenuMap(int x, int y, Point cursorPos)
        {
            OnMapMouseButtonClick?.Invoke(x, y, cursorPos);
        }

        private void openglControlMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseFormButtonDown)
            {
                var mouseDeltaX = e.X - mousePositionStartX;
                var mouseDeltaY = e.Y - mousePositionStartY;

                if (e.Button == MouseButtons.Left)
                {
                    mapDragOffsetX = mapDragOffsetXLast + mouseDeltaX;
                    mapDragOffsetZ = mapDragOffsetZLast - mouseDeltaY;
                }

                if (e.Button == MouseButtons.Middle)
                {
                    ActiveForm.Location = new Point(ActiveForm.Location.X + mouseDeltaX, ActiveForm.Location.Y + mouseDeltaY);
                }
            }
        }

        private void openglControlMap_MouseUp(object sender, MouseEventArgs e)
        {
            mouseFormButtonDown = false;

            mapDragOffsetXLast = mapDragOffsetX;
            mapDragOffsetZLast = mapDragOffsetZ;
        }

        private void openglControlMap_MouseWheel(object sender, MouseEventArgs e)
        {
            if (mapManager.CurrentMap.Equals(string.Empty))
            {
                return;
            }

            if (e.Delta > 0)
            {
                AdjustMapZoomIn(true, e.X, e.Y);
            }
            else
            {
                AdjustMapZoomOut(true, e.X, e.Y);
            }
        }

        internal bool modulesDone = false;
        private string gameExecutable = "None";

        private void openglControlMap_Paint(object sender, PaintEventArgs e)
        {
            if (renderMapComplete || !openglControlMapLoaded)
            {
                return;
            }

            if (reloadMap && mapManager.CurrentMap.Length > 0)
            {
                MapObject mapObjectSwitchTo;
                mapObjectSwitchTo = mapManager.mapObjects[mapManager.CurrentMap];

                OpenGL.CanvasSize = mapObjectSwitchTo.CanvasSizeBase;
                OpenGL.CanvasSizeBase = mapObjectSwitchTo.CanvasSizeBase;
                OpenGL.ZoomLevel = mapObjectSwitchTo.ZoomLevel;
                var mapObjectSwitchToBytes = mapObjectSwitchTo.GetMapBytes(mapObjectSwitchTo.defaultLevel);
                mapManager.CurrentMapLevel = mapObjectSwitchTo.defaultLevel;
                mapManager.DesiredMapLevel = mapObjectSwitchTo.defaultLevel;
                OpenGL.LoadTexture(mapObjectSwitchToBytes);
                AdjustMapZoomCoeff();
                reloadMap = false;
            }

            if (mapManager.CurrentMap.Length > 0 && mapManager.CurrentMapLevel != mapManager.DesiredMapLevel && mapManager.DesiredMapLevel != MapLevels.Nothing)
            {
                if (CommonHelpers.dateTimeHolder > lastMapChange)
                {
                    lastMapChange = CommonHelpers.dateTimeHolder.AddSeconds(lastMapChangeSec);
                    mapManager.CurrentMapLevel = mapManager.DesiredMapLevel;
                    MapObject mapObjectSwitchTo;
                    mapObjectSwitchTo = mapManager.mapObjects[mapManager.CurrentMap];
                    var mapObjectSwitchToBytes = mapObjectSwitchTo.GetMapBytes(mapManager.DesiredMapLevel);
                    OpenGL.LoadTexture(mapObjectSwitchToBytes);
                }
            }

            OpenGL.MakeCurrent(openglControlMap);

            OpenGL.DrawStart();

            if (!mapManager.CurrentMap.Equals(string.Empty) && mapManager.CurrentMap.Length != 0)
            {
                OpenGL.DrawMap();

                GL.Translate(
                  (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsX + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsXdebug) * OpenGL.CanvasDiffCoeff,
                  (mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsY + mapManager.mapObjects[mapManager.CurrentMap].offsetForObjectsYdebug) * OpenGL.CanvasDiffCoeff,
                  0
                );
            }
            else
            {
            }

            modulesDone = false;

            OnPrepareRenderObjectsEvent?.Invoke(this, null);

            do
            {
                CommonHelpers.NOP(100);
            } while (modulesDone == false);

            RenderObjects();

            OpenGL.DrawEnd(openglControlMap);
            renderMapComplete = true;
        }

        private void openglControlMap_Resize(object sender, EventArgs e)
        {
            OpenGL.SetupViewport(openglControlMap.Width, openglControlMap.Height);
            OpenGL.Invalidate(openglControlMap);
            GetOpenGlControlSize = openglControlMap.Size;
        }

        private void toolStripMenuItemFindLootHere_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void toolStripMenuItemPutMeHere_Click(object sender, EventArgs e)
        {
        }

        private void metroButtonShowHideOverlay_Click(object sender, EventArgs e)
        {
            if (overlay != null)
            {
                if (overlay.canRun)
                {
                    overlay.stopRequested = true;
                    overlay.Close();
                }
                else
                {
                    overlay.Init();
                    overlay.Show();
                }
            }
        }

        private void metroButtonShowLoot_MouseEnter(object sender, EventArgs e)
        {
        }

        private void metroButtonShowLoot_MouseLeave(object sender, EventArgs e)
        {
        }

        private void metroButtonUpdateLoot_MouseEnter(object sender, EventArgs e)
        {
        }

        private void metroButtonUpdateLoot_MouseLeave(object sender, EventArgs e)
        {
        }

        private void metroButtonDownloadUpdate_Click(object sender, EventArgs e)
        {
            OnDownloadUpdateClick?.Invoke(gameExecutable);
        }
    }
}