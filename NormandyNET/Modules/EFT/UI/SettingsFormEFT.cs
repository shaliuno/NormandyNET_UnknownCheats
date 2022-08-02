using MetroFramework;
using Newtonsoft.Json;
using NormandyNET.Modules.EFT.Improvements;
using NormandyNET.Settings;
using NormandyNET.UI;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NormandyNET.Modules.EFT.UI
{
    public partial class SettingsFormEFT : NormandyNET.SettingsBase.SettingsFormBase
    {
        private AimBotSettings aimbotSettingForm;
        private ChangeLog writeMemoryHelp;
        private LootSettings lootSettings;

        internal SettingsEFTJson settingsJson = new SettingsEFTJson();

        public SettingsFormEFT()
        {
            InitializeComponent();
            RegisterEvents();
            AfterInit();
        }

        public event EventHandler OnSettingsLoadedEvent;

        public event EventHandler OnShowLootChecked;

        public void AdjustMapZoomCoeff(object sender, EventArgs e)
        {
            AdjustMapZoomCoeff();
        }

        public override void AdjustMapZoomCoeff()
        {
            OpenGL.CanvasSize = OpenGL.CanvasSize + ModuleEFT.radarForm.mapManager.mapObjects[ModuleEFT.radarForm.mapManager.CurrentMap].zoomStep * OpenGL.ZoomLevel;
            OpenGL.CanvasDiffCoeff = OpenGL.CanvasSize / OpenGL.CanvasSizeBase;
            return;
        }

        public override void AfterInit()
        {
            var metroContextMenuMap = new MetroFramework.Controls.MetroContextMenu(this.components);

            settingsJsonFile = "settingsEFT.json";

            foreach (string resolution in ModuleEFT.radarForm.mapManager.resolutions)
            {
                metroComboBoxMapResolution.Items.Add(resolution);
            }

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
                    var resourceName = "NormandyNET.Modules.EFT.changelog.txt";
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
            return settingsJson.Loot.LootListToLookFor.IndexOf(friendlyName);
        }

        public override void MapManagerSwitchMap(string mapResText, int mapResIndex)
        {
            ModuleEFT.radarForm.mapManager.resolutionFolder = mapResText;
            ModuleEFT.radarForm.mapManager.reloadMaps = true;
            settingsJson.Map.MapResolution = mapResIndex;
        }

        public override void metroButtonAbout_Click(object sender, EventArgs e)
        {
        }

        public override void metroButtonDebugReloadMap_Click(object sender, EventArgs e)
        {
            ModuleEFT.radarForm.reloadMap = true;
        }

        public override void metroButtonLootSettings_Click(object sender, EventArgs e)
        {
            if (lootSettings == null || lootSettings.IsDisposed)
            {
                lootSettings = new LootSettings();
                lootSettings.StartPosition = FormStartPosition.Manual;
                lootSettings.Location = new Point(Cursor.Position.X - lootSettings.Width / 2, Cursor.Position.Y - lootSettings.Height / 2);
                lootSettings.Show();
            }

            var mainFormTopMost = this.TopMost;
            this.TopMost = false;
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

                if (ModuleEFT.radarForm.overlay != null)
                {
                    ModuleEFT.radarForm.overlay.UpdateAspectRatio(valueWidth, valueHeight);
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
            HelperLogger.SaveLogToFile(ModuleEFT.radarForm.Started);
            metroButtonSaveLog.Text = "Save Log";
        }

        public override void metroButtonSetupIconColors_Click(object sender, EventArgs e)
        {
            var colorForm = new IconColors();
            colorForm.StartPosition = FormStartPosition.Manual;

            var mainFormTopMost = this.TopMost;
            this.TopMost = false;
            colorForm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);

            if (colorForm.ShowDialog() == DialogResult.OK)
            {
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
            settingsJson.Loot.ShowStatusChanged = true;
        }

        public override void metroCheckBoxEntityDistance_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Distance = metroCheckBoxEntityDistance.Checked;
        }

        public override void metroCheckBoxEntityLevels_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Level = metroCheckBoxEntityLevels_EFT.Checked;
        }

        public override void metroCheckBoxEntityLOS_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.LOS = metroCheckBoxEntityLOS.Checked;
        }

        public override void metroCheckBoxEntityLOSLineType_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.LineOfSight.Solid = metroCheckBoxEntityLOSLineType.Checked;
        }

        public override void metroCheckBoxEntityNames_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Name = metroCheckBoxEntityNames.Checked;
        }

        public override void metroCheckBoxEntityWeapon_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Weapon = metroCheckBoxEntityWeapon.Checked;
        }

        public override void metroCheckBoxMapAutoHeightDisable_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Map.AutoHeight = metroCheckBoxMapAutoHeight_EFT.Checked;
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

        public override void metroCheckBoxOverlayShowBonesAI_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.Bones.AI = metroCheckBoxOverlayShowBonesAI.Checked;
        }

        public override void metroCheckBoxOverlayShowBonesHighDetail_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.Bones.HighDetail = metroCheckBoxOverlayShowBonesHighDetail.Checked;
        }

        public override void metroCheckBoxOverlayShowBonesHumans_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.Bones.Humans = metroCheckBoxOverlayShowBonesHumans.Checked;
        }

        public override void metroCheckBoxShowLoot_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Loot.Show = metroCheckBoxShowLoot.Checked;
            OnShowLootChecked?.Invoke(this, null);
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
            ModuleEFT.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        public override void metroRadioButtonOverlayWindowStyleOBSPreviewWindow_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.WindowStyle = WindowStyleEnum.OBS;
            ModuleEFT.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        public override void metroRadioButtonOverlayWindowStyleMoonlight_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.WindowStyle = WindowStyleEnum.Moonlight;
            ModuleEFT.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        public override void metroRadioButtonOverlayWindowStyleStandalone_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Overlay.WindowStyle = WindowStyleEnum.Standalone;
            ModuleEFT.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        private void _tmrDelaySearch_Tick(object sender, EventArgs e)
        {
            if (metroTextBoxLootSearch.Text.Length > 2)
            {
                try
                {
                    metroTextBoxLootSearch.UseCustomForeColor = false;
                    metroTextBoxLootSearch.ForeColor = System.Drawing.SystemColors.ControlText;
                    lootSearchRegexp = new Regex($@"{metroTextBoxLootSearch.Text}", RegexOptions.IgnoreCase);
                    metroListViewLootSearchHighlight.Items.Clear();
                    LootItemHelper.LootFriendlyNamesToShow.Clear();
                    LootItemHelper.LootShortNamesToShow.Clear();
                    LootItemHelper.LootCategoriesToShow.Clear();
                    LootItemHelper.FindLootRebuildTable = true;
                    LootItemHelper.FindLootRebuildTableTime = CommonHelpers.dateTimeHolder.AddMilliseconds(LootItemHelper.FindLootRebuildOffsetMs);
                }
                catch
                {
                    metroTextBoxLootSearch.UseCustomForeColor = true;
                    metroTextBoxLootSearch.ForeColor = Color.Red;
                }
            }

            if (metroTextBoxLootSearch.Text.Length == 0)
            {
                metroListViewLootSearchHighlight.Items.Clear();
                LootItemHelper.LootFriendlyNamesToShow.Clear();
                LootItemHelper.LootShortNamesToShow.Clear();
                LootItemHelper.LootCategoriesToShow.Clear();
                lootSearchRegexp = null;
                LootItemHelper.FindLootRebuildTable = true;
                LootItemHelper.FindLootRebuildTableTime = CommonHelpers.dateTimeHolder.AddMilliseconds(LootItemHelper.FindLootRebuildOffsetMs);
            }

            if (_tmrDelaySearch != null)
                _tmrDelaySearch.Stop();
        }

        public override void metroTextBoxLootSearch_TextChanged(object sender, EventArgs e)
        {
            if (_tmrDelaySearch != null)
                _tmrDelaySearch.Stop();

            if (_tmrDelaySearch == null)
            {
                _tmrDelaySearch = new Timer();
                _tmrDelaySearch.Tick += _tmrDelaySearch_Tick;
                _tmrDelaySearch.Interval = delayedTextChangedTimeout;
            }

            _tmrDelaySearch.Start();
        }

        public override void metroTrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            metroTextBox1.Text = ((float)metroTrackBar1.Value).ToString();
            ModuleEFT.radarForm.mapManager.mapObjects[ModuleEFT.radarForm.mapManager.CurrentMap].offsetForObjectsXdebug = metroTrackBar1.Value;
        }

        public override void metroTrackBar2_Scroll(object sender, ScrollEventArgs e)
        {
            metroTextBox2.Text = ((float)metroTrackBar2.Value).ToString();
            ModuleEFT.radarForm.mapManager.mapObjects[ModuleEFT.radarForm.mapManager.CurrentMap].offsetForObjectsYdebug = metroTrackBar2.Value;
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

        public override void metroTrackBarOverlayBonesDrawDistance_Scroll(object sender, ScrollEventArgs e)
        {
            settingsJson.Overlay.Bones.DrawDistance = metroTrackBarOverlayBonesDrawDistance.Value;
            metroTextBoxOverlayBonesDrawDistance.Text = $"{metroTrackBarOverlayBonesDrawDistance.Value}m";
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

        public override void RegisterEvents()
        {
            ModuleEFT.radarForm.OnSettingButtonClick += new RadarForm.SettingButtonClickHandler(this.ShowForm);
            OnDisclaimerResult += new DisclaimerHandler(SetDisclaimerResult);

            ModuleEFT.radarForm.OnCenterMapButtonClick += PollSettingCenterMap;
            ModuleEFT.radarForm.OnMapDrawTextButtonClick += PollSettingMapDrawText;
            ModuleEFT.radarForm.OnMapShowLootButtonClick += PollSettingMapShowLoot;
            ModuleEFT.radarForm.OnRadarFormCloseEvent += SettingsSaveJson;
            ModuleEFT.radarForm.overlay.windowStyle = settingsJson.Overlay.WindowStyle;
        }

        private void SetDisclaimerResult(bool result)
        {
            if (!settingsJson.MemoryWriting.DisclaimerAgreed && true)
            {
                settingsJson.MemoryWriting.DisclaimerAgreed = true;
                ApplySettingsWriteMemoryFeatures();
            }
        }

        public override void SettingsApplyJson()
        {
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
            RecolorListViews();

            metroTrackBarOverlayOpacity.Value = (int)ModuleEFT.radarForm.settingsRadar.UserInterface.Opacity;
            metroTextBoxOverlayOpacity.Text = ModuleEFT.radarForm.settingsRadar.UserInterface.Opacity.ToString();

            metroToggleWindowAlwaysOnTop.Checked = ModuleEFT.radarForm.settingsRadar.UserInterface.TopMost;

            metroCheckBoxEntityNames.Checked = settingsJson.Entity.Name;
            metroCheckBoxEntityLevels_EFT.Checked = settingsJson.Entity.Level;
            metroCheckBoxEntitySide_EFT.Checked = settingsJson.Entity.Side;
            metroCheckBoxEntityWeapon.Checked = settingsJson.Entity.Weapon;
            metroCheckBoxEntityInventoryValue_EFT.Checked = settingsJson.Entity.InventoryValue;
            metroCheckBoxEntityInventoryValueUseLootFilters_EFT.Checked = settingsJson.Entity.InventoryValueUseLootFilters;

            metroCheckBoxEntityLOS.Checked = settingsJson.Entity.LOS;
            metroCheckBoxEntityLOSLineType.Checked = settingsJson.Entity.LineOfSight.Solid;

            metroCheckBoxEntityDistance.Checked = settingsJson.Entity.Distance;
            metroCheckBoxEntityBodies.Checked = settingsJson.Entity.Bodies;
            metroCheckBoxEntityKillDeathRatio_EFT.Checked = settingsJson.Entity.KDRatio;
            metroCheckBoxEntityHealth_EFT.Checked = settingsJson.Entity.Health;
            metroCheckBoxEntityArmorClass_EFT.Checked = settingsJson.Entity.ArmorClass;

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
            metroTrackBarIconSizeLoot.Value = (int)(settingsJson.Map.IconSizePlayers * 100);
            metroTrackBarTextScale.Value = (int)(settingsJson.Map.TextScale * 100);

            metroTextBoxLOSPlayer.Text = settingsJson.Entity.LineOfSight.You.ToString();
            metroTextBoxLOSEnemy.Text = settingsJson.Entity.LineOfSight.Enemy.ToString();
            metroTextBoxIconSizePlayers.Text = $"{metroTrackBarIconSizePlayers.Value}%";
            metroTextBoxIconSizeLoot.Text = $"{metroTrackBarIconSizeLoot.Value}%";
            metroTextBoxTextScale.Text = $"{metroTrackBarTextScale.Value}%";
            OpenGL.TextScale = settingsJson.Map.TextScale;
            OpenGL.IconSizePlayers = settingsJson.Map.IconSizePlayers;

            metroCheckBoxExfiltrationPoints.Checked = settingsJson.Map.ExfiltrationPoint;

            metroCheckBoxHideTextAroundPlayer.Checked = settingsJson.Map.HideTextAroundPlayer;
            metroTrackBarHideTextRadius.Value = settingsJson.Map.HideTextAroundPlayerDistance;
            metroTextBoxHideTextRadius.Text = settingsJson.Map.HideTextAroundPlayerDistance.ToString();

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

            metroCheckBoxOverlayShowBonesHumans.Checked = settingsJson.Overlay.Bones.Humans;
            metroCheckBoxOverlayShowBonesAI.Checked = settingsJson.Overlay.Bones.AI;
            metroCheckBoxOverlayShowBonesHighDetail.Checked = settingsJson.Overlay.Bones.HighDetail;

            metroTrackBarOverlayBonesDrawDistance.Value = settingsJson.Overlay.Bones.DrawDistance;
            metroTextBoxOverlayBonesDrawDistance.Text = $"{metroTrackBarOverlayBonesDrawDistance.Value}m";

            metroCheckBoxMapAutoHeight_EFT.Checked = settingsJson.Map.AutoHeight;
            OpenGL.CenterMap = settingsJson.Map.CenterMap;

            metroComboBoxMapResolution.SelectedIndex = settingsJson.Map.MapResolution;
            MapManagerSwitchMap(metroComboBoxMapResolution.SelectedItem.ToString(), settingsJson.Map.MapResolution);
            OpenGL.DrawText = settingsJson.Map.DrawText;

            LootItemHelper.HelperLootInit();

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

            metroToggleTraceLog.Checked = ModuleEFT.radarForm.settingsRadar.Debug.TraceLogEnabled;
            metroToggleOpenGlDebug.Checked = ModuleEFT.radarForm.settingsRadar.Debug.OpenGlDebugEnabled;
            DebugClass.DebugOpenGL = ModuleEFT.radarForm.settingsRadar.Debug.OpenGlDebugEnabled;

            ApplySettingsWriteMemoryFeatures();
        }

        private void ApplySettingsWriteMemoryFeatures()
        {
            if (settingsJson.MemoryWriting.DisclaimerAgreed)
            {
                metroButtonWriteMemoryAimbotSettings_EFT.Visible = true;

                metroCheckBoxWriteMemorySkillHackMagDrills_EFT.Visible = true;
                metroCheckBoxWriteMemorySkillHackSuperJump_EFT.Visible = true;
                metroLabelFlyHack.Visible = true;
                metroLabelBsodWaring.Visible = true;

                metroLabelWriteMemoryAimbot_EFT.Visible = true;
                metroLabelWriteMemoryAlwaysSprint_EFT.Visible = true;
                metroLabelWriteMemoryAlwaysSprintAltMode_EFT.Visible = true;
                metroLabelWriteMemoryFastReload_EFT.Visible = true;
                metroLabelWriteMemoryFastRPM_EFT.Visible = true;
                metroLabelWriteMemoryFastRunning_EFT.Visible = true;

                metroLabelWriteMemoryLeanHack_EFT.Visible = true;
                metroComboBoxWriteMemoryLeanHack_EFT.Visible = true;

                metroLabelWriteMemoryLootThroughWalls_EFT.Visible = true;
                metroComboBoxWriteMemoryLootThroughWalls_EFT.Visible = true;
                metroLabelWriteMemoryNightVisionToggle_EFT.Visible = true;

                metroLabelWriteMemoryNoRecoil_EFT.Visible = true;
                metroCheckBoxWriteMemoryNoRecoilStreamSafe_EFT.Visible = true;
                metroComboBoxWriteMemoryNoRecoilIntensity_EFT.Visible = true;
                metroLabelWriteMemoryInstantADS_EFT.Visible = true;
                metroLabelWriteMemoryPinkDudes_EFT.Visible = true;
                metroLabelWriteMemoryRemoveVisor_EFT.Visible = true;
                metroLabelWriteMemorySkillHack_EFT.Visible = true;
                metroLabelWriteMemoryThermalToggle_EFT.Visible = true;
                metroLabelWriteMemoryUnlimitedStamina_EFT.Visible = true;
                metroLabelWriteMemoryUtilityHacks_EFT.Visible = true;

                if (settingsJson.MemoryWriting.LootThroughWalls.Distance == 4) { settingsJson.MemoryWriting.LootThroughWalls.Distance = 3; }
                metroComboBoxWriteMemoryLootThroughWalls_EFT.SelectedIndex = (int)settingsJson.MemoryWriting.LootThroughWalls.Distance;

                for (int i = 0; i < metroComboBoxWriteMemoryNoRecoilIntensity_EFT.Items.Count; ++i)
                {
                    if (string.Equals(metroComboBoxWriteMemoryNoRecoilIntensity_EFT.Items[i].ToString(), settingsJson.MemoryWriting.NoRecoil.Intensity.ToString()))
                    {
                        metroComboBoxWriteMemoryNoRecoilIntensity_EFT.SelectedIndex = i;
                    }
                }

                for (int i = 0; i < metroComboBoxWriteMemoryLeanHack_EFT.Items.Count; ++i)
                {
                    if (string.Equals(metroComboBoxWriteMemoryLeanHack_EFT.Items[i].ToString(), settingsJson.MemoryWriting.LeanHack.Distance.ToString()))
                    {
                        metroComboBoxWriteMemoryLeanHack_EFT.SelectedIndex = i;
                    }
                }

                metroCheckBoxWriteMemorySkillHackMagDrills_EFT.Checked = settingsJson.MemoryWriting.SkillHack.MagDrills;
                metroCheckBoxWriteMemorySkillHackSuperJump_EFT.Checked = settingsJson.MemoryWriting.SkillHack.SuperJump;
                metroCheckBoxWriteMemoryNoRecoilStreamSafe_EFT.Checked = settingsJson.MemoryWriting.NoRecoil.StreamerSafe;

                NoRecoilCheck(false);
                NoRecoilStreamerSafeCheck();
                InstantADSCheck(false);
                UtilityHackCheck(false);
                PinkDudesCheck(false);
                NoVisorCheck(false);
                UnlimitedStaminaCheck(false);
                AlwaysSprintAltModeCheck(false);
                FastRPMCheck(false);
                SkillHackCheck(false);
                AimBotCheck(false);
                LeanHackCheck(false);
                FastReloadCheck(false);
                FastRunningCheck(false);
                NoInertiaCheck(false);
            }
        }

        public override void SettingsLoadJson()
        {
            if (File.Exists(settingsJsonFile))
            {
                try
                {
                    settingsJson = JsonConvert.DeserializeObject<SettingsEFTJson>(File.ReadAllText(settingsJsonFile), new JsonSerializerSettings
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
            settingsJson.Loot.ShowStatusChanged = true;
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
            settingsJson.Loot.ShowStatusChanged = true;
        }

        public override void metroTrackBar3_Scroll(object sender, ScrollEventArgs e)
        {
            metroTextBox3.Text = ((float)metroTrackBar3.Value).ToString();

            OpenGL.CanvasSize = ModuleEFT.radarForm.mapManager.mapObjects[ModuleEFT.radarForm.mapManager.CurrentMap].CanvasSizeBase + metroTrackBar3.Value;
            OpenGL.CanvasSizeBase = ModuleEFT.radarForm.mapManager.mapObjects[ModuleEFT.radarForm.mapManager.CurrentMap].CanvasSizeBase + metroTrackBar3.Value;
            OpenGL.ZoomLevel = ModuleEFT.radarForm.mapManager.mapObjects[ModuleEFT.radarForm.mapManager.CurrentMap].ZoomLevel;

            AdjustMapZoomCoeff();
        }

        private void metroButtonWriteMemoryMoveForward_Click(object sender, EventArgs e)
        {
            if (!settingsJson.MemoryWriting.MoveDo)
            {
                settingsJson.MemoryWriting.MoveDo = true;
                settingsJson.MemoryWriting.MoveDirection = Movements.Forward;
            }
        }

        private void metroButtonWriteMemoryMoveBackward_Click(object sender, EventArgs e)
        {
            if (!settingsJson.MemoryWriting.MoveDo)
            {
                settingsJson.MemoryWriting.MoveDo = true;
                settingsJson.MemoryWriting.MoveDirection = Movements.Backward;
            }
        }

        private void metroButtonWriteMemoryMoveLeft_Click(object sender, EventArgs e)
        {
            if (!settingsJson.MemoryWriting.MoveDo)
            {
                settingsJson.MemoryWriting.MoveDo = true;
                settingsJson.MemoryWriting.MoveDirection = Movements.Left;
            }
        }

        private void metroButtonWriteMemoryMoveRight_Click(object sender, EventArgs e)
        {
            if (!settingsJson.MemoryWriting.MoveDo)
            {
                settingsJson.MemoryWriting.MoveDo = true;
                settingsJson.MemoryWriting.MoveDirection = Movements.Right;
            }
        }

        private void metroButtonWriteMemoryMoveUp_Click(object sender, EventArgs e)
        {
            if (!settingsJson.MemoryWriting.MoveDo)
            {
                settingsJson.MemoryWriting.MoveDo = true;
                settingsJson.MemoryWriting.MoveDirection = Movements.Up;
            }
        }

        private void metroButtonWriteMemoryMoveDown_Click(object sender, EventArgs e)
        {
            if (!settingsJson.MemoryWriting.MoveDo)
            {
                settingsJson.MemoryWriting.MoveDo = true;
                settingsJson.MemoryWriting.MoveDirection = Movements.Down;
            }
        }

        public override void metroButtonLogReport_Click(object sender, EventArgs e)
        {
            var dialog = new LogReport();
            dialog.ShowDialog();
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
            settingsJson.Loot.ShowStatusChanged = true;
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
            settingsJson.Loot.ShowStatusChanged = true;
        }

        public override void toolStripMenuItemEnableAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem entry in metroListViewLootCategoriesShown.Items)
            {
                var entityType = entry.Text;

                settingsJson.Loot.LootCategorySuppressed.Remove(entityType);
            }

            metroListViewLootCategoriesShown.Invalidate();
            settingsJson.Loot.ShowStatusChanged = true;
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
            settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroLabelWriteMemoryRemoveVisor_EFT_Click(object sender, EventArgs e)
        {
            NoVisorCheck();
        }

        private void NoVisorCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.NoVisor.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.NoVisor.Enabled = false;
                    metroLabelWriteMemoryRemoveVisor_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryRemoveVisor_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.NoVisor.Enabled = true;
                    metroLabelWriteMemoryRemoveVisor_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryRemoveVisor_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabelWriteMemoryThermalToggle_EFT_Click(object sender, EventArgs e)
        {
            if (settingsJson.MemoryWriting.ThermalVision.Enabled)
            {
                settingsJson.MemoryWriting.ThermalVision.Enabled = false;
                metroLabelWriteMemoryThermalToggle_EFT.BackColor = Color.LightCoral;
            }
            else
            {
                settingsJson.MemoryWriting.ThermalVision.Enabled = true;
                metroLabelWriteMemoryThermalToggle_EFT.BackColor = Color.LightGreen;
            }
        }

        public override void metroToggleTraceLog_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.radarForm.settingsRadar.Debug.TraceLogEnabled = metroToggleTraceLog.Checked;
            HelperLogger.saveToFile = ModuleEFT.radarForm.settingsRadar.Debug.TraceLogEnabled;
        }

        public override void metroToggleOpenGlDebug_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.radarForm.settingsRadar.Debug.OpenGlDebugEnabled = metroToggleTraceLog.Checked;
            DebugClass.DebugOpenGL = ModuleEFT.radarForm.settingsRadar.Debug.OpenGlDebugEnabled;
        }

        private void metroLabelWriteMemoryLootThroughWalls_Click(object sender, EventArgs e)
        {
            if (settingsJson.MemoryWriting.LootThroughWalls.Enabled)
            {
                settingsJson.MemoryWriting.LootThroughWalls.Enabled = false;
                metroLabelWriteMemoryLootThroughWalls_EFT.BackColor = Color.LightCoral;
            }
            else
            {
                settingsJson.MemoryWriting.LootThroughWalls.Enabled = true;
                metroLabelWriteMemoryLootThroughWalls_EFT.BackColor = Color.LightGreen;
            }
        }

        private void metroLabelWriteMemoryUnlimitedStamina_EFT_Click(object sender, EventArgs e)
        {
            UnlimitedStaminaCheck();
        }

        private void UnlimitedStaminaCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.UnlimitedStamina.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.UnlimitedStamina.Enabled = false;
                    metroLabelWriteMemoryUnlimitedStamina_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryUnlimitedStamina_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.UnlimitedStamina.Enabled = true;
                    metroLabelWriteMemoryUnlimitedStamina_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryUnlimitedStamina_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroCheckBoxEntityKillDeathRatio_EFT_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.KDRatio = metroCheckBoxEntityKillDeathRatio_EFT.Checked;
        }

        private void metroCheckBoxEntityHealth_EFT_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Health = metroCheckBoxEntityHealth_EFT.Checked;
        }

        public override void metroButtonWriteMemoryHelp_Click(object sender, EventArgs e)
        {
            if (writeMemoryHelp == null || writeMemoryHelp.IsDisposed)
            {
                writeMemoryHelp = new ChangeLog();
                writeMemoryHelp.StartPosition = FormStartPosition.Manual;
                writeMemoryHelp.Location = new Point(Cursor.Position.X - writeMemoryHelp.Width / 2, Cursor.Position.Y - writeMemoryHelp.Height / 2);

                string result = "";
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "NormandyNET.Modules.EFT.WriteMemoryHelp.txt";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }

                writeMemoryHelp.Text = "Write Memory Help";
                writeMemoryHelp.metroTextBoxChangeLog.Text = result;
                writeMemoryHelp.Show();
            }
        }

        public override void metroButtonWriteMemoryDisclaimer_Click(object sender, EventArgs e)
        {
            base.ShowDisclaimer();
            Console.WriteLine("eft");
        }

        private void metroComboBoxWriteMemoryLootThroughWalls_EFT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBoxWriteMemoryLootThroughWalls_EFT.SelectedIndex == -1)
            {
                return;
            }

            settingsJson.MemoryWriting.LootThroughWalls.Distance = metroComboBoxWriteMemoryLootThroughWalls_EFT.SelectedIndex;
        }

        private void metroCheckBoxEntityInventoryValue_EFT_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.InventoryValue = metroCheckBoxEntityInventoryValue_EFT.Checked;
        }

        private void metroCheckBoxEntityInventoryValueUseLootFilters_EFT_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.InventoryValueUseLootFilters = metroCheckBoxEntityInventoryValueUseLootFilters_EFT.Checked;
        }

        private void metroLabelWriteMemoryAimbot_EFT_Click(object sender, EventArgs e)
        {
            AimBotCheck();
        }

        private void AimBotCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.AimBot.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.AimBot.Enabled = false;
                    metroLabelWriteMemoryAimbot_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryAimbot_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.AimBot.Enabled = true;
                    metroLabelWriteMemoryAimbot_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryAimbot_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroButtonWriteMemoryAimbotSettings_EFT_Click(object sender, EventArgs e)
        {
            if (aimbotSettingForm == null || aimbotSettingForm.IsDisposed)
            {
                aimbotSettingForm = new AimBotSettings();
                aimbotSettingForm.StartPosition = FormStartPosition.Manual;
                aimbotSettingForm.Location = new Point(Cursor.Position.X - aimbotSettingForm.Width / 2, Cursor.Position.Y - aimbotSettingForm.Height / 2);
                aimbotSettingForm.Show();
            }
        }

        private void metroLabelFlyHack_Click(object sender, EventArgs e)
        {
            if (settingsJson.MemoryWriting.FlyHack.Enabled)
            {
                settingsJson.MemoryWriting.FlyHack.Enabled = false;
                metroLabelFlyHack.BackColor = Color.LightCoral;
            }
            else
            {
                settingsJson.MemoryWriting.FlyHack.Enabled = true;
                metroLabelFlyHack.BackColor = Color.LightGreen;
            }
        }

        internal void WriteMemoryFlyHackDisable()
        {
            settingsJson.MemoryWriting.FlyHack.Enabled = false;
            metroLabelFlyHack.BackColor = Color.LightCoral;
        }

        private void metroLabelWriteMemorySkillHack_EFT_Click(object sender, EventArgs e)
        {
            SkillHackCheck();
        }

        private void SkillHackCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.SkillHack.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.SkillHack.Enabled = false;
                    metroLabelWriteMemorySkillHack_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemorySkillHack_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.SkillHack.Enabled = true;
                    metroLabelWriteMemorySkillHack_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemorySkillHack_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabelWriteMemorySkillHack_EFT_MouseHover(object sender, EventArgs e)
        {
            metroToolTip1.Show(SkillHack.Tooltip, metroLabelWriteMemorySkillHack_EFT);
        }

        private void metroLabelWriteMemoryAlwaysSprint_EFT_Click(object sender, EventArgs e)
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprintAltMode.Enabled)
            {
                MetroMessageBox.Show(this, "Disable Always Sprint - Alt Mode first", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (settingsJson.MemoryWriting.AlwaysSprint.Enabled)
            {
                settingsJson.MemoryWriting.AlwaysSprint.Enabled = false;
                metroLabelWriteMemoryAlwaysSprint_EFT.BackColor = Color.LightCoral;
            }
            else
            {
                settingsJson.MemoryWriting.AlwaysSprint.Enabled = true;
                metroLabelWriteMemoryAlwaysSprint_EFT.BackColor = Color.LightGreen;
            }
        }

        private void AlwaysSprintAltModeCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.AlwaysSprintAltMode.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.AlwaysSprintAltMode.Enabled = false;
                    metroLabelWriteMemoryAlwaysSprintAltMode_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryAlwaysSprintAltMode_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.AlwaysSprintAltMode.Enabled = true;
                    metroLabelWriteMemoryAlwaysSprintAltMode_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryAlwaysSprintAltMode_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabelWriteMemoryFastReload_EFT_Click(object sender, EventArgs e)
        {
            FastReloadCheck();
        }

        private void FastReloadCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.FastReload.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.FastReload.Enabled = false;
                    metroLabelWriteMemoryFastReload_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryFastReload_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.FastReload.Enabled = true;
                    metroLabelWriteMemoryFastReload_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryFastReload_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabelWriteMemoryFastRunning_EFT_Click(object sender, EventArgs e)
        {
            FastRunningCheck();
        }

        private void FastRunningCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.FastRunning.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.FastRunning.Enabled = false;
                    metroLabelWriteMemoryFastRunning_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryFastRunning_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.FastRunning.Enabled = true;
                    metroLabelWriteMemoryFastRunning_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryFastRunning_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabelWriteMemoryRemoveInertia_EFT_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MetroMessageBox.Show(this, "Are you in lobby and radar started bro?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                if (settingsJson.MemoryWriting.NoInertia.Enabled)
                {
                    settingsJson.MemoryWriting.NoInertia.Enabled = false;
                }
                else
                {
                    settingsJson.MemoryWriting.NoInertia.Enabled = true;
                }

                ModuleEFT.radarForm.BringToFront();
            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }

        private void metroCheckBoxWriteMemorySkillHackMagDrills_EFT_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.MemoryWriting.SkillHack.MagDrills = metroCheckBoxWriteMemorySkillHackMagDrills_EFT.Checked;
        }

        private void metroCheckBoxWriteMemorySkillHackSuperJump_EFT_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.MemoryWriting.SkillHack.SuperJump = metroCheckBoxWriteMemorySkillHackSuperJump_EFT.Checked;
        }

        private void metroCheckBoxEntityArmorClass_EFT_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.ArmorClass = metroCheckBoxEntityArmorClass_EFT.Checked;
        }

        private void metroLabelWriteMemoryUtilityHacks_EFT_Click(object sender, EventArgs e)
        {
            UtilityHackCheck();
        }

        private void UtilityHackCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.UtilityHack.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.UtilityHack.Enabled = false;
                    metroLabelWriteMemoryUtilityHacks_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryUtilityHacks_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.UtilityHack.Enabled = true;
                    metroLabelWriteMemoryUtilityHacks_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryUtilityHacks_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabelWriteMemoryNightVisionToggle_EFT_Click(object sender, EventArgs e)
        {
            if (settingsJson.MemoryWriting.NightVision.Enabled)
            {
                settingsJson.MemoryWriting.NightVision.Enabled = false;
                metroLabelWriteMemoryNightVisionToggle_EFT.BackColor = Color.LightCoral;
            }
            else
            {
                settingsJson.MemoryWriting.NightVision.Enabled = true;
                metroLabelWriteMemoryNightVisionToggle_EFT.BackColor = Color.LightGreen;
            }
        }

        public override void metroCheckBoxHideTextAroundPlayer_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Map.HideTextAroundPlayer = metroCheckBoxHideTextAroundPlayer.Checked;
        }

        public override void metroTrackBarHideTextRadius_Scroll(object sender, ScrollEventArgs e)
        {
            settingsJson.Map.HideTextAroundPlayerDistance = metroTrackBarHideTextRadius.Value;
            metroTextBoxHideTextRadius.Text = $"{metroTrackBarHideTextRadius.Value}m";
        }

        private void metroCheckBoxExfiltrationPoints_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Map.ExfiltrationPoint = metroCheckBoxExfiltrationPoints.Checked;
        }

        private void metroLabelWriteMemoryFastRPM_EFT_Click(object sender, EventArgs e)
        {
            FastRPMCheck();
        }

        private void FastRPMCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.FastRPM.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.FastRPM.Enabled = false;
                    metroLabelWriteMemoryFastRPM_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryFastRPM_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.FastRPM.Enabled = true;
                    metroLabelWriteMemoryFastRPM_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryFastRPM_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabel1_Click_1(object sender, EventArgs e)
        {
            ModuleEFT.readerEFT.searchRequested = true;
        }

        private void metroLabelWriteMemoryAutomaticGunHacks_EFT_MouseHover(object sender, EventArgs e)
        {
            metroToolTip1.Show(FastRPM.Tooltip, metroLabelWriteMemoryFastRPM_EFT);
        }

        private void metroLabelWriteMemoryUtilityHacks_EFT_MouseHover(object sender, EventArgs e)
        {
            metroToolTip1.Show(UtilityHacks.Tooltip, metroLabelWriteMemoryUtilityHacks_EFT);
        }

        private void metroTextBoxEntityKillDeathRatioThreshold_EFT_TextChanged(object sender, EventArgs e)
        {
            var valueOk = Single.TryParse(metroTextBoxEntityKillDeathRatioThreshold_EFT.Text, out float value);

            if (valueOk && value > 0)
            {
                settingsJson.Entity.KDRatioThreshold = value;
                metroTextBoxEntityKillDeathRatioThreshold_EFT.UseCustomForeColor = false;
                metroTextBoxEntityKillDeathRatioThreshold_EFT.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxEntityKillDeathRatioThreshold_EFT.UseCustomForeColor = true;
                metroTextBoxEntityKillDeathRatioThreshold_EFT.ForeColor = Color.Red;
            }
        }

        private void metroToggleWriteMemoryNoInertia_EFT_CheckedChanged(object sender, EventArgs e)
        {
            if (settingsJson.MemoryWriting.NoInertia.Enabled)
            {
                settingsJson.MemoryWriting.NoInertia.Enabled = false;
            }
            else
            {
                settingsJson.MemoryWriting.NoInertia.Enabled = true;
            }
        }

        private void metroLabelWriteMemoryNoRecoil_EFT_Click(object sender, EventArgs e)
        {
            NoRecoilCheck();
        }

        private void NoRecoilCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.NoRecoil.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.NoRecoil.Enabled = false;
                    metroLabelWriteMemoryNoRecoil_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryNoRecoil_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.NoRecoil.Enabled = true;
                    metroLabelWriteMemoryNoRecoil_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryNoRecoil_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabelWriteMemoryPinkDudes_EFT_Click(object sender, EventArgs e)
        {
            PinkDudesCheck();
        }

        private void PinkDudesCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.PinkDudes.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.PinkDudes.Enabled = false;
                    metroLabelWriteMemoryPinkDudes_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryPinkDudes_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.PinkDudes.Enabled = true;
                    metroLabelWriteMemoryPinkDudes_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryPinkDudes_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabelWriteMemoryLeanHack_EFT_Click(object sender, EventArgs e)
        {
            LeanHackCheck();
        }

        private void LeanHackCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.LeanHack.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.LeanHack.Enabled = false;
                    metroLabelWriteMemoryLeanHack_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryLeanHack_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.LeanHack.Enabled = true;
                    metroLabelWriteMemoryLeanHack_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryLeanHack_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroLabelWriteMemoryAlwaysSprintAltMode_EFT_Click(object sender, EventArgs e)
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Enabled)
            {
                MetroMessageBox.Show(this, "Disable Always Sprint - Alt Mode first", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AlwaysSprintAltModeCheck();
        }

        private void metroLabelWriteMemoryNoInertia_EFT_Click(object sender, EventArgs e)
        {
            NoInertiaCheck();
        }

        private void NoInertiaCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.NoInertia.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.NoInertia.Enabled = false;
                    metroLabelWriteMemoryNoInertia_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryNoInertia_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.NoInertia.Enabled = true;
                    metroLabelWriteMemoryNoInertia_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryNoInertia_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroComboBoxWriteMemoryLeanHack_EFT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBoxWriteMemoryLeanHack_EFT.SelectedIndex == -1)
            {
                return;
            }

            var result = Single.TryParse((string)metroComboBoxWriteMemoryLeanHack_EFT.Items[metroComboBoxWriteMemoryLeanHack_EFT.SelectedIndex], out float value);

            if (result)
            {
                settingsJson.MemoryWriting.LeanHack.Distance = value;
            }
        }

        private void metroCheckBoxEntitySide_EFT_CheckedChanged(object sender, EventArgs e)
        {
            settingsJson.Entity.Side = metroCheckBoxEntitySide_EFT.Checked;
        }

        private void metroComboBoxWriteMemoryNoRecoilIntensity_EFT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBoxWriteMemoryNoRecoilIntensity_EFT.SelectedIndex == -1)
            {
                return;
            }

            if (Single.TryParse(metroComboBoxWriteMemoryNoRecoilIntensity_EFT.SelectedItem.ToString(), out float value))
            {
                settingsJson.MemoryWriting.NoRecoil.Intensity = value;
            }
        }

        private void metroLabelWriteMemoryInstantADS_EFT_Click(object sender, EventArgs e)
        {
            InstantADSCheck();
        }

        private void InstantADSCheck(bool userClick = true)
        {
            if (settingsJson.MemoryWriting.InstantADS.Enabled)
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.InstantADS.Enabled = false;
                    metroLabelWriteMemoryInstantADS_EFT.BackColor = Color.LightCoral;
                }
                else
                {
                    metroLabelWriteMemoryInstantADS_EFT.BackColor = Color.LightGreen;
                }
            }
            else
            {
                if (userClick)
                {
                    settingsJson.MemoryWriting.InstantADS.Enabled = true;
                    metroLabelWriteMemoryInstantADS_EFT.BackColor = Color.LightGreen;
                }
                else
                {
                    metroLabelWriteMemoryInstantADS_EFT.BackColor = Color.LightCoral;
                }
            }
        }

        private void metroCheckBoxWriteMemoryNoRecoilStreamSafe_EFT_CheckedChanged(object sender, EventArgs e)
        {
            NoRecoilStreamerSafeCheck();
        }

        private void NoRecoilStreamerSafeCheck()
        {
            settingsJson.MemoryWriting.NoRecoil.StreamerSafe = metroCheckBoxWriteMemoryNoRecoilStreamSafe_EFT.Checked;

            if (settingsJson.MemoryWriting.NoRecoil.StreamerSafe)
            {
                metroComboBoxWriteMemoryNoRecoilIntensity_EFT.Enabled = true;
            }
            else
            {
                metroComboBoxWriteMemoryNoRecoilIntensity_EFT.Enabled = false;
            }
        }
    }
}