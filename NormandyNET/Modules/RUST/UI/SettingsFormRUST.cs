using MetroFramework;
using Newtonsoft.Json;
using NormandyNET.Settings;
using NormandyNET.UI;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static NormandyNET.Core.MapManager;

namespace NormandyNET.Modules.RUST.UI
{
    public partial class SettingsFormRUST : NormandyNET.SettingsBase.SettingsFormBase
    {
        #region Internal Fields

        internal SettingsRUSTJson settingsJson = new SettingsRUSTJson();

        #endregion Internal Fields

        #region Public Constructors

        public SettingsFormRUST()
        {
            InitializeComponent();
            RegisterEvents();
            AfterInit();
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler OnSettingsLoadedEvent;

        public event EventHandler OnShowLootChecked;

        #endregion Public Events

        #region Public Methods

        public void AdjustMapZoomCoeff(object sender, EventArgs e)
        {
            AdjustMapZoomCoeff();
        }

        public override void AdjustMapZoomCoeff()
        {
            OpenGL.CanvasSize = OpenGL.CanvasSize + ModuleRUST.radarForm.mapManager.mapObjects[ModuleRUST.radarForm.mapManager.CurrentMap].zoomStep * OpenGL.ZoomLevel;
            OpenGL.CanvasDiffCoeff = OpenGL.CanvasSize / OpenGL.CanvasSizeBase;
            return;
        }

        public override void AfterInit()
        {
            settingsJsonFile = "settingsRUST.json";

            foreach (string resolution in ModuleRUST.radarForm.mapManager.resolutions)
            {
                metroComboBoxMapResolution.Items.Add(resolution);
            }

            metroCheckBoxEntityBodies.Text = "Show Bodies / Sleeping";

            SettingsLoadJson();
            SettingsApplyJson();
            ShowChangeLog();
        }

        private void ShowChangeLog()
        {
            var lastModified = File.GetLastWriteTimeUtc($@"{Application.ExecutablePath}");
            if ((CommonHelpers.dateTimeHolder - lastModified).TotalMinutes < 1)
            {
                if (DebugClass.Debug == false)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = "NormandyNET.Modules.RUST.changelog.txt";
                    string result = "";

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }

                    var colorForm = new ChangeLog();

                    colorForm.metroTextBoxChangeLog.Text = result;

                    var mainFormTopMost = this.TopMost;
                    this.TopMost = false;

                    if (colorForm.ShowDialog() == DialogResult.OK)
                    {
                    }

                    this.TopMost = mainFormTopMost;
                }
            }
        }

        public override void ClearMetroListViews()
        {
            metroListViewLootCategoriesShown.Items.Clear();
            metroListViewLootSearchHighlight.Items.Clear();
        }

        public override void Form_Shown(object sender, EventArgs e)
        {
        }

        public override int LootListToLookForIndex(string friendlyName)
        {
            return -1;
        }

        public override void MapManagerSwitchMap(string mapResText, int mapResIndex)
        {
            ModuleRUST.radarForm.mapManager.resolutionFolder = mapResText;
            ModuleRUST.radarForm.mapManager.reloadMaps = true;
            settingsJson.Map.MapResolution = mapResIndex;
        }

        public override void metroButtonAbout_Click(object sender, EventArgs e)
        {
        }

        public override void metroButtonDebugReloadMap_Click(object sender, EventArgs e)
        {
            ModuleRUST.radarForm.reloadMap = true;
        }

        public override void metroButtonLootSettings_Click(object sender, EventArgs e)
        {
            var colorForm = new LootSettings();
            colorForm.StartPosition = FormStartPosition.Manual;

            var mainFormTopMost = this.TopMost;
            this.TopMost = false;
            colorForm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);

            if (colorForm.ShowDialog() == DialogResult.OK)
            {
                SettingsSaveJson();
            }
            this.TopMost = mainFormTopMost;
        }

        public override void metroButtonOverlayGameResolutionApply_Click(object sender, EventArgs e)
        {
            var resultWidth = Int32.TryParse(metroTextBoxOverlayGameResolutionWidth.Text, out int valueWidth);

            var resultHeight = Int32.TryParse(metroTextBoxOverlayGameResolutionHeight.Text, out int valueHeight);

            if (resultWidth && resultHeight)
            {
                settingsJson.Overlay.GameResolution.Width = valueWidth;
                settingsJson.Overlay.GameResolution.Height = valueHeight;

                if (ModuleRUST.radarForm.overlay != null)
                {
                    ModuleRUST.radarForm.overlay.UpdateAspectRatio(valueWidth, valueHeight);
                }
            }
            else
            {
                MetroMessageBox.Show(this, "Enter correct values like 1920x1080", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void metroButtonSaveLog_Click(object sender, EventArgs e)
        {
            metroButtonSaveLog.Text = "Saving";
            HelperLogger.SaveLogToFile(ModuleRUST.radarForm.Started);
            metroButtonSaveLog.Text = "Save Log";
        }

        public override void metroButtonSetupIconColors_Click(object sender, EventArgs e)
        {
            var colorForm = new IconColors();
            var mainFormTopMost = this.TopMost;
            this.TopMost = false;
            colorForm.StartPosition = FormStartPosition.Manual;
            colorForm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);

            if (colorForm.ShowDialog() == DialogResult.OK)
            {
                SettingsSaveJson();
            }
            this.TopMost = mainFormTopMost;
        }

        public override void metroCheckBoxAggregateLoot_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Loot.Aggregate = metroCheckBoxAggregateLoot.Checked;
        }

        public override void metroCheckBoxElevationArrows_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Elevation.Arrows = metroCheckBoxElevationArrows.Checked;
        }

        public override void metroCheckBoxEntityBodies_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Bodies = metroCheckBoxEntityBodies.Checked;
        }

        public override void metroCheckBoxEntityDistance_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Distance = metroCheckBoxEntityDistance.Checked;
        }

        public override void metroCheckBoxEntityLOS_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.LOS = metroCheckBoxEntityLOS.Checked;
        }

        public override void metroCheckBoxEntityNames_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Name = metroCheckBoxEntityNames.Checked;
        }

        public override void metroCheckBoxEntityWeapon_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Weapon = metroCheckBoxEntityWeapon.Checked;
        }

        public override void metroCheckBoxOSDDateTime_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.OnScreenDisplay.DateTime = metroCheckBoxOSDDateTime.Checked;
        }

        public override void metroCheckBoxOSDFPS_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.OnScreenDisplay.FPS = metroCheckBoxOSDFPS.Checked;
        }

        public override void metroCheckBoxOSDShowStats_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.OnScreenDisplay.Stats = metroCheckBoxOSDShowStats.Checked;
        }

        public override void metroCheckBoxShowLoot_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Loot.Show = metroCheckBoxShowLoot.Checked;
            OnShowLootChecked?.Invoke(this, null);
        }

        public override void metroListViewLootCategoriesShown_DoubleClick(object sender, EventArgs e)
        {
            if (metroListViewLootCategoriesShown.SelectedItems.Count == 1)
            {
                var entityType = metroListViewLootCategoriesShown.SelectedItems[0].Text;

                if (settingsJson.Loot.LootCategorySuppressed.Contains(entityType))
                {
                    settingsJson.Loot.LootCategorySuppressed.Remove(entityType);
                }
                else
                {
                    settingsJson.Loot.LootCategorySuppressed.Add(entityType);
                }
            }

            metroListViewLootCategoriesShown.Invalidate();
            ModuleRUST.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        public override void metroRadioButtonElevationAbsolute_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Elevation.Type = ElevationType.Absolute;
        }

        public override void metroRadioButtonElevationNone_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Elevation.Type = ElevationType.None;
        }

        public override void metroRadioButtonElevationRelative_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Elevation.Type = ElevationType.Relative;
        }

        public override void metroRadioButtonOverlayWindowStyleFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.WindowStyle = WindowStyleEnum.FullScreen;
            ModuleRUST.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        public override void metroRadioButtonOverlayWindowStyleOBSPreviewWindow_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.WindowStyle = WindowStyleEnum.OBS;
            ModuleRUST.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        public override void metroRadioButtonOverlayWindowStyleMoonlight_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.WindowStyle = WindowStyleEnum.Moonlight;
            ModuleRUST.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        public override void metroRadioButtonOverlayWindowStyleStandalone_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.WindowStyle = WindowStyleEnum.Standalone;
            ModuleRUST.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        public override void metroTextBoxLootSearch_TextChanged(object sender, EventArgs e)
        {
            if (metroTextBoxLootSearch.Text.Length > 0)
            {
                try
                {
                    metroTextBoxLootSearch.ForeColor = System.Drawing.SystemColors.ControlText;

                    lootSearchRegexp = new Regex($@"{metroTextBoxLootSearch.Text}", RegexOptions.IgnoreCase);

                    metroListViewLootSearchHighlight.Items.Clear();
                    LootItemHelper.LootFriendlyNamesToShow.Clear();
                    LootItemHelper.FindLootRebuildTable = true;
                    LootItemHelper.FindLootRebuildTableTime = CommonHelpers.dateTimeHolder.AddMilliseconds(LootItemHelper.FindLootRebuildOffsetMs);
                }
                catch
                {
                    metroTextBoxLootSearch.ForeColor = Color.Red;
                }
            }

            if (metroTextBoxLootSearch.Text.Length == 0)
            {
                metroListViewLootSearchHighlight.Items.Clear();
                LootItemHelper.LootFriendlyNamesToShow.Clear();
                lootSearchRegexp = null;
                LootItemHelper.FindLootRebuildTable = true;
                LootItemHelper.FindLootRebuildTableTime = CommonHelpers.dateTimeHolder.AddMilliseconds(LootItemHelper.FindLootRebuildOffsetMs);
            }
        }

        public override void metroTrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            metroTextBox1.Text = ((float)metroTrackBar1.Value).ToString();
            ModuleRUST.radarForm.mapManager.mapObjects[ModuleRUST.radarForm.mapManager.CurrentMap].offsetForObjectsXdebug = metroTrackBar1.Value;
        }

        public override void metroTrackBar2_Scroll(object sender, ScrollEventArgs e)
        {
            metroTextBox2.Text = ((float)metroTrackBar2.Value).ToString();
            ModuleRUST.radarForm.mapManager.mapObjects[ModuleRUST.radarForm.mapManager.CurrentMap].offsetForObjectsYdebug = metroTrackBar2.Value;
        }

        public override void metroTrackBarIconSizePlayers_Scroll(object sender, ScrollEventArgs e)
        {
            settingsJson.Map.IconSizePlayers = metroTrackBarIconSizePlayers.Value / 100f;
            metroTextBoxIconSizePlayers.Text = $"{metroTrackBarIconSizePlayers.Value}%";
            OpenGL.IconSizePlayers = settingsJson.Map.IconSizePlayers;
        }

        public override void metroTrackBarIconSizeLoot_Scroll(object sender, ScrollEventArgs e)
        {
            settingsJson.Map.IconSizeLoot = metroTrackBarIconSizeLoot.Value / 100f;
            metroTextBoxIconSizeLoot.Text = $"{metroTrackBarIconSizeLoot.Value}%";
            OpenGL.IconSizeLoot = settingsJson.Map.IconSizeLoot;
        }

        public override void metroTrackBarLOSEnemy_Scroll(object sender, ScrollEventArgs e)
        {
            settingsJson.Entity.LineOfSight.Enemy = metroTrackBarLOSEnemy.Value;
            metroTextBoxLOSEnemy.Text = settingsJson.Entity.LineOfSight.Enemy.ToString();
        }

        public override void metroTrackBarLOSPlayer_Scroll(object sender, ScrollEventArgs e)
        {
            settingsJson.Entity.LineOfSight.You = metroTrackBarLOSPlayer.Value;
            metroTextBoxLOSPlayer.Text = settingsJson.Entity.LineOfSight.You.ToString();
        }

        public override void metroTrackBarOverlayDrawDistance_Scroll(object sender, ScrollEventArgs e)
        {
            settingsJson.Overlay.DrawDistance = metroTrackBarOverlayDrawDistance.Value;
            metroTextBoxOverlayDrawDistance.Text = $"{metroTrackBarOverlayDrawDistance.Value}m";
        }

        public override void metroTrackBarOverlayDrawDistanceLoot_Scroll(object sender, ScrollEventArgs e)
        {
            settingsJson.Overlay.DrawDistanceLoot = metroTrackBarOverlayDrawDistanceLoot.Value;
            metroTextBoxOverlayDrawDistanceLoot.Text = $"{metroTrackBarOverlayDrawDistanceLoot.Value}m";
        }

        public override void metroTrackBarTextScale_Scroll(object sender, ScrollEventArgs e)
        {
            settingsJson.Map.TextScale = metroTrackBarTextScale.Value / 100f;
            metroTextBoxTextScale.Text = $"{metroTrackBarTextScale.Value}%";
            OpenGL.TextScale = settingsJson.Map.TextScale;
        }

        public override void PollSettingCenterMap(object sender, EventArgs e)
        {
            settingsJson.Map.CenterMap = !settingsJson.Map.CenterMap;
            OpenGL.CenterMap = settingsJson.Map.CenterMap;
        }

        public override void PollSettingMapDrawText(object sender, EventArgs e)
        {
            settingsJson.Map.DrawText = !settingsJson.Map.DrawText;
            OpenGL.DrawText = settingsJson.Map.DrawText;
        }

        public override void PollSettingMapShowLoot(object sender, EventArgs e)
        {
            settingsJson.Loot.Show = !settingsJson.Loot.Show;
            metroCheckBoxShowLoot.Checked = settingsJson.Loot.Show;
        }

        public void ShowForm(int x, int y)
        {
            if (Visible)
            {
                this.Hide();
            }
            else
            {
                this.Show();
                Location = new Point(x + 50, y);
            }
        }

        public override void RegisterEvents()
        {
            ModuleRUST.radarForm.OnSettingButtonClick += new RadarForm.SettingButtonClickHandler(this.ShowForm);
            ModuleRUST.radarForm.OnCenterMapButtonClick += PollSettingCenterMap;
            ModuleRUST.radarForm.OnMapDrawTextButtonClick += PollSettingMapDrawText;
            ModuleRUST.radarForm.OnMapShowLootButtonClick += PollSettingMapShowLoot;
            ModuleRUST.radarForm.OnRadarFormCloseEvent += SettingsSaveJson;
            ModuleRUST.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        public override void SettingsApplyJson()
        {
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
            RecolorListViews();

            metroTrackBarOverlayOpacity.Value = (int)ModuleRUST.radarForm.settingsRadar.UserInterface.Opacity;
            metroTextBoxOverlayOpacity.Text = ModuleRUST.radarForm.settingsRadar.UserInterface.Opacity.ToString();

            metroToggleWindowAlwaysOnTop.Checked = ModuleRUST.radarForm.settingsRadar.UserInterface.TopMost;

            metroCheckBoxEntityNames.Checked = settingsJson.Entity.Name;
            metroCheckBoxEntityWeapon.Checked = settingsJson.Entity.Weapon;
            metroCheckBoxEntityLOS.Checked = settingsJson.Entity.LOS;
            metroCheckBoxEntityDistance.Checked = settingsJson.Entity.Distance;
            metroCheckBoxEntityBodies.Checked = settingsJson.Entity.Bodies;

            metroCheckBoxElevationArrows.Checked = settingsJson.Entity.Elevation.Arrows;
            metroRadioButtonElevationNone.Checked = settingsJson.Entity.Elevation.Type == ElevationType.None;
            metroRadioButtonElevationAbsolute.Checked = settingsJson.Entity.Elevation.Type == ElevationType.Absolute;
            metroRadioButtonElevationRelative.Checked = settingsJson.Entity.Elevation.Type == ElevationType.Relative;

            metroCheckBoxShowLoot.Checked = settingsJson.Loot.Show;
            metroCheckBoxAggregateLoot.Checked = settingsJson.Loot.Aggregate;

            metroCheckBoxOSDShowStats.Checked = settingsJson.OnScreenDisplay.Stats;
            metroCheckBoxOSDDateTime.Checked = settingsJson.OnScreenDisplay.DateTime;
            metroCheckBoxOSDFPS.Checked = settingsJson.OnScreenDisplay.FPS;

            metroTrackBarLOSEnemy.Value = settingsJson.Entity.LineOfSight.Enemy;
            metroTrackBarLOSPlayer.Value = settingsJson.Entity.LineOfSight.You;
            metroTrackBarIconSizePlayers.Value = (int)(settingsJson.Map.IconSizePlayers * 100);
            metroTrackBarIconSizeLoot.Value = (int)(settingsJson.Map.IconSizeLoot * 100);

            metroTrackBarTextScale.Value = (int)(settingsJson.Map.TextScale * 100);

            metroTextBoxLOSPlayer.Text = settingsJson.Entity.LineOfSight.You.ToString();
            metroTextBoxLOSEnemy.Text = settingsJson.Entity.LineOfSight.Enemy.ToString();
            metroTextBoxIconSizePlayers.Text = $"{metroTrackBarIconSizePlayers.Value}%";
            metroTextBoxIconSizeLoot.Text = $"{metroTrackBarIconSizeLoot.Value}%";

            metroTextBoxTextScale.Text = $"{metroTrackBarTextScale.Value}%";
            OpenGL.TextScale = settingsJson.Map.TextScale;
            OpenGL.IconSizePlayers = settingsJson.Map.IconSizePlayers;

            metroTrackBarOverlayDrawDistance.Value = settingsJson.Overlay.DrawDistance;
            metroTextBoxOverlayDrawDistance.Text = $"{metroTrackBarOverlayDrawDistance.Value}m";

            metroTrackBarOverlayDrawDistanceLoot.Value = settingsJson.Overlay.DrawDistanceLoot;
            metroTextBoxOverlayDrawDistanceLoot.Text = $"{metroTrackBarOverlayDrawDistanceLoot.Value}m";

            metroRadioButtonOverlayWindowStyleFullScreen.Checked = settingsJson.Overlay.WindowStyle == WindowStyleEnum.FullScreen;
            metroRadioButtonOverlayWindowStyleStandalone.Checked = settingsJson.Overlay.WindowStyle == WindowStyleEnum.Standalone;
            metroRadioButtonOverlayWindowStyleOBSPreviewWindow.Checked = settingsJson.Overlay.WindowStyle == WindowStyleEnum.OBS;
            metroRadioButtonOverlayWindowStyleMoonlight.Checked = settingsJson.Overlay.WindowStyle == WindowStyleEnum.Moonlight;

            metroTextBoxOverlayGameResolutionHeight.Text = settingsJson.Overlay.GameResolution.Height.ToString();
            metroTextBoxOverlayGameResolutionWidth.Text = settingsJson.Overlay.GameResolution.Width.ToString();

            OpenGL.CenterMap = settingsJson.Map.CenterMap;
            metroCheckBoxMapNetworkBubble_RUST.Checked = settingsJson.Map.NetBubbleCircles;
            metroCheckBoxProximityAlert_RUST.Checked = settingsJson.Map.ProximityAlert;
            metroCheckBoxHouseOwners_RUST.Checked = settingsJson.Map.HouseOwners;

            metroComboBoxMapResolution.SelectedIndex = settingsJson.Map.MapResolution;
            MapManagerSwitchMap(metroComboBoxMapResolution.SelectedItem.ToString(), settingsJson.Map.MapResolution);
            OpenGL.DrawText = settingsJson.Map.DrawText;

            metroTextBoxRUSTMapSize.Text = settingsJson.Map.MapSize.ToString();
            metroComboBoxMapResolution.SelectedIndex = metroComboBoxMapResolution.Items.Count - 1;

            LootItemHelper.HelperLootInit();

            foreach (string entityType in LootItemHelper.EntityTypesCanShow)
            {
                if (!entityType.Equals("Loot") && !entityType.Equals("NULL"))
                {
                    if (settingsJson.Entity.EntityTypesSuppressed.Contains(entityType))
                    {
                        metroListView_AddItem(metroListViewEntityTypes_RUST, entityType, "Disabled");
                    }
                    else
                    {
                        metroListView_AddItem(metroListViewEntityTypes_RUST, entityType, "Enabled");
                    }
                }
            }

            metroListView_AddItem(metroListViewEntityTypes_RUST, "Unknown", "Enabled");

            foreach (string lootCategory in LootItemHelper.LootCategoriesCanShow)
            {
                if (settingsJson.Loot.LootCategorySuppressed.Contains(lootCategory))
                {
                    metroListView_AddItem(metroListViewLootCategoriesShown, lootCategory, "Disabled");
                }
                else
                {
                    metroListView_AddItem(metroListViewLootCategoriesShown, lootCategory, "Enabled");
                }
            }
        }

        public override void SettingsLoadJson()
        {
            if (File.Exists(settingsJsonFile))
            {
                try
                {
                    settingsJson = JsonConvert.DeserializeObject<SettingsRUSTJson>(File.ReadAllText(settingsJsonFile), new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });
                }
                catch (JsonSerializationException ex)
                {
                }
            }

            OnSettingsLoadedEvent?.Invoke(this, null);
        }

        public override void SettingsSaveJson()
        {
            var settingsSerialized = JsonConvert.SerializeObject(settingsJson, Formatting.Indented, new JsonSerializerSettings
            {
            });

            if (File.Exists(settingsJsonFile))
            {
                System.IO.File.Copy(settingsJsonFile, $"{settingsJsonFile}.bak", true);
            }

            File.WriteAllText(settingsJsonFile, settingsSerialized);
        }

        public override void toolStripMenuItemPutMeHere_Click(object sender, EventArgs e)
        {
        }

        #endregion Public Methods

        private void metroCheckBoxMapNetworkBubble_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Map.NetBubbleCircles = metroCheckBoxMapNetworkBubble_RUST.Checked;
        }

        private void RecolorListViewsOwnerDrawn()
        {
            switch (metroStyleManager.Theme)
            {
                case MetroThemeStyle.Dark:
                    metroListViewEntityTypes_RUST.BackColor = ColorTranslator.FromHtml("#111111");
                    metroListViewLootCategoriesShown.BackColor = ColorTranslator.FromHtml("#111111");
                    break;

                default:
                    metroListViewEntityTypes_RUST.BackColor = SystemColors.Control;
                    metroListViewLootCategoriesShown.BackColor = SystemColors.Control;
                    break;
            }
        }

        public override void Form_Loaded(object sender, EventArgs e)
        {
            RecolorListViewsOwnerDrawn();
        }

        public override void metroListViewLootCategoriesShown_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            ListViewItem item = e.Item;
            ItemStatus server_status = item.Tag as ItemStatus;

            var statusBoxSize = 6;
            Color MetroGreen = Color.FromArgb(0, 177, 89);
            Color MetroRed = Color.FromArgb(209, 17, 65);
            Color statusColor = MetroRed;

            switch (e.ColumnIndex)
            {
                case 0:
                    break;

                case 1:
                    if (settingsJson.Loot.LootCategorySuppressed.Contains(e.Item.Text))
                    {
                        statusColor = MetroRed;
                    }
                    else
                    {
                        statusColor = MetroGreen;
                    }

                    Rectangle rect = new Rectangle(
                        e.Bounds.Left + (e.Bounds.Width / 2 - statusBoxSize),
                        e.Bounds.Top + (e.Bounds.Height / 2 - statusBoxSize),
                        statusBoxSize * 2,
                        statusBoxSize * 2);

                    using (SolidBrush br = new SolidBrush(statusColor))
                    {
                        e.Graphics.FillRectangle(br, rect);
                    }

                    Color pen_color = Color.FromArgb(255,
                       0,
                       0,
                       0);

                    rect = new Rectangle(
                        e.Bounds.Left + (e.Bounds.Width / 2 - statusBoxSize) - 2,
                        e.Bounds.Top + (e.Bounds.Height / 2 - statusBoxSize) - 2,
                        statusBoxSize * 2 + 3,
                        statusBoxSize * 2 + 3);

                    using (Pen br = new Pen(Color.FromArgb(255, 100, 100, 100)))
                    {
                        e.Graphics.DrawRectangle(br, rect);
                    }

                    break;
            }

            e.Graphics.ResetTransform();
            ListView lvw = e.Item.ListView;
            if (lvw.FullRowSelect)
            {
                e.DrawFocusRectangle(e.Item.Bounds);
            }
        }

        private void metroListViewEntityTypes_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            ListViewItem item = e.Item;
            ItemStatus server_status = item.Tag as ItemStatus;

            var statusBoxSize = 6;
            Color MetroGreen = Color.FromArgb(0, 177, 89);
            Color MetroRed = Color.FromArgb(209, 17, 65);
            Color statusColor = MetroRed;

            switch (e.ColumnIndex)
            {
                case 0:
                    break;

                case 1:

                    if (settingsJson.Entity.EntityTypesSuppressed.Contains(e.Item.Text))
                    {
                        statusColor = MetroRed;
                    }
                    else
                    {
                        statusColor = MetroGreen;
                    }

                    Rectangle rect = new Rectangle(
                        e.Bounds.Left + (e.Bounds.Width / 2 - statusBoxSize),
                        e.Bounds.Top + (e.Bounds.Height / 2 - statusBoxSize),
                        statusBoxSize * 2,
                        statusBoxSize * 2);

                    using (SolidBrush br = new SolidBrush(statusColor))
                    {
                        e.Graphics.FillRectangle(br, rect);
                    }

                    rect = new Rectangle(
                       e.Bounds.Left + (e.Bounds.Width / 2 - statusBoxSize) - 2,
                       e.Bounds.Top + (e.Bounds.Height / 2 - statusBoxSize) - 2,
                       statusBoxSize * 2 + 3,
                       statusBoxSize * 2 + 3);

                    using (Pen br = new Pen(Color.FromArgb(255, 100, 100, 100)))
                    {
                        e.Graphics.DrawRectangle(br, rect);
                    }

                    break;
            }

            e.Graphics.ResetTransform();
            ListView lvw = e.Item.ListView;
            if (lvw.FullRowSelect)
            {
                e.DrawFocusRectangle(e.Item.Bounds);
            }
        }

        private void metroListViewEntityTypes_DoubleClick(object sender, EventArgs e)
        {
            if (metroListViewEntityTypes_RUST.SelectedItems.Count == 1)
            {
                var entityType = metroListViewEntityTypes_RUST.SelectedItems[0].Text;

                if (settingsJson.Entity.EntityTypesSuppressed.Contains(entityType))
                {
                    settingsJson.Entity.EntityTypesSuppressed.Remove(entityType);
                }
                else
                {
                    settingsJson.Entity.EntityTypesSuppressed.Add(entityType);
                }

                metroListViewEntityTypes_RUST.Invalidate();
                ModuleRUST.settingsForm.settingsJson.Entity.ShowStatusChanged = true;
            }
        }

        private void metroListViewEntityTypes_Layout(object sender, LayoutEventArgs e)
        {
            ShowScrollBar(this.metroListViewEntityTypes_RUST.Handle, (int)SB_BOTH, false);
        }

        private void metroButtonEntityDumpUnknown_Click(object sender, EventArgs e)
        {
            var dumpList = ReaderRUST.entityList.FindAll(x => x.EntityType == EntityTypeRUST.Unknown);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"dump.csv", true, System.Text.Encoding.Unicode))
            {
                file.WriteLine($"FriendlyName\tEntityType\tCategory\tGameObjectName");

                foreach (BaseEntity eP in dumpList)
                {
                    file.WriteLine($"{eP.FriendlyName}\t{eP.EntityType.ToString()}\t{eP.Category}\t{eP.GameObjectName}");
                }
            }
        }

        public override void metroListViewLootCategoriesShown_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (metroListViewLootCategoriesShown.SelectedItems.Count > 0)
            {
                var enableItems = true;

                foreach (ListViewItem entry in metroListViewLootCategoriesShown.SelectedItems)
                {
                    var itemDisabled = settingsJson.Loot.LootCategorySuppressed.Contains(entry.Text);

                    if (itemDisabled == false)
                    {
                        enableItems = false;
                        break;
                    }
                    else
                    {
                    }
                }

                foreach (ListViewItem entry in metroListViewLootCategoriesShown.SelectedItems)
                {
                    var entityType = entry.Text;

                    if (enableItems)
                    {
                        settingsJson.Loot.LootCategorySuppressed.Remove(entityType);
                    }
                    else
                    {
                        if (settingsJson.Loot.LootCategorySuppressed.Contains(entityType) == false)
                        {
                            settingsJson.Loot.LootCategorySuppressed.Add(entityType);
                        }
                    }
                }
            }

            metroListViewLootCategoriesShown.Invalidate();
            ModuleRUST.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        public override void toolStripMenuItemEnable_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem entry in metroListViewLootCategoriesShown.SelectedItems)
            {
                var entityType = entry.Text;

                if (true)
                {
                    settingsJson.Loot.LootCategorySuppressed.Remove(entityType);
                }
            }

            metroListViewLootCategoriesShown.Invalidate();
            ModuleRUST.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        public override void toolStripMenuItemDisable_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem entry in metroListViewLootCategoriesShown.SelectedItems)
            {
                var entityType = entry.Text;

                if (settingsJson.Loot.LootCategorySuppressed.Contains(entityType) == false)
                {
                    settingsJson.Loot.LootCategorySuppressed.Add(entityType);
                }
            }

            metroListViewLootCategoriesShown.Invalidate();
            ModuleRUST.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroCheckBoxProximityAlert_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Map.ProximityAlert = metroCheckBoxProximityAlert_RUST.Checked;
        }

        private void metroLabelWriteMemoryFakeAdmin_Click(object sender, EventArgs e)
        {
            if (settingsJson.MemoryWriting.FakeAdmin)
            {
                settingsJson.MemoryWriting.FakeAdmin = false;
                settingsJson.MemoryWriting.FakeAdminDo = true;
                metroLabelWriteMemoryFakeAdmin_RUST.BackColor = Color.LightCoral;
            }
            else
            {
                if (settingsJson.MemoryWriting.FakeAdminShowWarning)
                {
                    settingsJson.MemoryWriting.FakeAdminShowWarning = false;

                    MetroMessageBox.Show(ModuleRUST.radarForm,
                      "\n" +
                      "Using Admin mode allows admin features like debug camera, noclip and others.\n" +
                      "While noclip is instantly bannable you can use DEBUG CAMERA to see weak spots in raided building and where stuff is really hidden.\n" +
                      "\n" +
                      "ALWAYS DISABLE ADMIN MODE if you're not using it for this feature.\n" +
                      "\n" +
                      "WHY?" +
                      "\n" +
                      "Admin can trick you into noclip mode (yeah no shit they enable that on you themselves)\n" +
                      "and if you jump - you're fucked!\n" +
                      "\n" +
                      "To use debug camera, open RUST Console using F1 button\n" +
                      "Type: debug.debugcamera\n" +
                      "To enable or disable.",
                      "WARNING! MINES AHEAD!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (int)(this.Height * 0.75));
                }

                settingsJson.MemoryWriting.FakeAdmin = true;
                settingsJson.MemoryWriting.FakeAdminDo = true;
                metroLabelWriteMemoryFakeAdmin_RUST.BackColor = Color.LightGreen;
            }
        }

        private void metroButtonRUSTMapApply_Click(object sender, EventArgs e)
        {
            var mapSizeOk = Int32.TryParse(metroTextBoxRUSTMapSize.Text, out int _mapSize);

            if (mapSizeOk)
            {
                settingsJson.Map.MapSize = _mapSize + 1000;

                MapObject mapObjectSwitchTo;
                mapObjectSwitchTo = ModuleRUST.radarForm.mapManager.mapObjects[ModuleRUST.radarForm.mapManager.CurrentMap];
                mapObjectSwitchTo.CanvasSizeBase = settingsJson.Map.MapSize / 2;
                ModuleRUST.radarForm.reloadMap = true;
            }
            else
            {
                MetroMessageBox.Show(this, "Enter correct values i.e. numbers!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void metroCheckBoxDebugStuff_CheckedChanged(object sender, EventArgs e)
        {
            DebugClass.DebugDraw = metroCheckBoxDebugStuff.Checked;
        }

        public override void metroButtonLogReport_Click(object sender, EventArgs e)
        {
            var dialog = new LogReport();
            dialog.ShowDialog();
        }

        private void metroLabelWriteMemorySpiderMan_RUST_Click(object sender, EventArgs e)
        {
            if (settingsJson.MemoryWriting.SpiderMan)
            {
                settingsJson.MemoryWriting.SpiderMan = false;
                settingsJson.MemoryWriting.SpiderManDo = true;
                metroLabelWriteMemorySpiderMan_RUST.BackColor = Color.LightCoral;
            }
            else
            {
                settingsJson.MemoryWriting.SpiderMan = true;
                settingsJson.MemoryWriting.SpiderManDo = true;
                metroLabelWriteMemorySpiderMan_RUST.BackColor = Color.LightGreen;
            }
        }

        private void metroLabelWriteMemoryBrightDay_RUST_Click(object sender, EventArgs e)
        {
            if (settingsJson.MemoryWriting.BrightDay)
            {
                settingsJson.MemoryWriting.BrightDay = false;
                settingsJson.MemoryWriting.BrightDayDo = true;

                metroLabelWriteMemoryBrightDay_RUST.BackColor = Color.LightCoral;
            }
            else
            {
                MetroMessageBox.Show(ModuleRUST.radarForm,
                 "\n" +
                 "This gives you a nightvision sort of.\n So enjoy being Khajiit.\n\n" +
                 "Keep in mind: Normal players don't see well at night.\n" +
                 "Don't to anything suspicious with this mode enabled like raiding without light in pitch black night.\n" +
                 "\n" +
                 "Otherwise this feature is ok.",
                 "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (int)(this.Height * 0.75));

                settingsJson.MemoryWriting.BrightDay = true;
                settingsJson.MemoryWriting.BrightDayDo = true;
                metroLabelWriteMemoryBrightDay_RUST.BackColor = Color.LightGreen;
            }
        }

        public override void toolStripMenuItemEnableAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem entry in metroListViewLootCategoriesShown.Items)
            {
                var entityType = entry.Text;

                settingsJson.Loot.LootCategorySuppressed.Remove(entityType);
            }

            metroListViewLootCategoriesShown.Invalidate();
            ModuleRUST.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        public override void toolStripMenuItemDisableAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem entry in metroListViewLootCategoriesShown.Items)
            {
                var entityType = entry.Text;

                if (settingsJson.Loot.LootCategorySuppressed.Contains(entityType) == false)
                {
                    settingsJson.Loot.LootCategorySuppressed.Add(entityType);
                }
            }

            metroListViewLootCategoriesShown.Invalidate();
            ModuleRUST.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroCheckBoxHouseOwners_RUST_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Map.HouseOwners = metroCheckBoxHouseOwners_RUST.Checked;
        }
    }
}