using ListViewSortAnyColumn;
using MetroFramework;
using MetroFramework.Forms;
using NormandyNET.UI;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NormandyNET.SettingsBase
{
    public partial class SettingsFormBase : MetroForm
    {
        #region Public Fields

        public Regex lootSearchRegexp;

        public string settingsJsonFile;

        #endregion Public Fields

        #region Private Fields

        public Timer _tmrDelaySearch;
        public const int delayedTextChangedTimeout = 500;

        public const int SB_BOTH = 3;
        private const int SB_CTL = 2;
        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;

        #endregion Private Fields

        #region Public Constructors

        public SettingsFormBase()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        [DllImport("user32.dll")]
        public static extern bool ShowScrollBar(System.IntPtr hWnd, int wBar, bool bShow);

        public virtual void AdjustMapZoomCoeff()
        { }

        public virtual void AfterInit()
        { }

        public virtual void ClearMetroListViews()
        {
        }

        public virtual void Form_Loaded(object sender, EventArgs e)
        {
        }

        public virtual void Form_Shown(object sender, EventArgs e)
        { }

        public virtual int LootListToLookForIndex(string friendlyName)
        { return -1; }

        public virtual void MapManagerSwitchMap(string mapResText, int mapResIndex)
        { }

        public virtual void metroButtonAbout_Click(object sender, EventArgs e)
        { }

        public virtual void metroButtonDebugReloadMap_Click(object sender, EventArgs e)
        { }

        public virtual void metroButtonLootSettings_Click(object sender, EventArgs e)
        { }

        public virtual void metroButtonOverlayGameResolutionApply_Click(object sender, EventArgs e)
        { }

        public virtual void metroButtonSaveLog_Click(object sender, EventArgs e)
        { }

        public virtual void metroButtonSetupIconColors_Click(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxAggregateLoot_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxElevationArrows_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxEntityBodies_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxEntityDistance_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxEntityInventoryValue_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxEntityLevels_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxEntityLOS_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxEntityNames_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxEntityWeapon_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxMapAutoHeightDisable_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxOSDDateTime_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxOSDFPS_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxOSDShowStats_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxOverlayShowBonesAI_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxOverlayShowBonesHighDetail_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxOverlayShowBonesHumans_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroCheckBoxShowLoot_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroRadioButtonElevationAbsolute_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroRadioButtonElevationNone_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroRadioButtonElevationRelative_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroRadioButtonOverlayWindowStyleFullScreen_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroRadioButtonOverlayWindowStyleOBSPreviewWindow_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroRadioButtonOverlayWindowStyleStandalone_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroTextBoxLootSearch_TextChanged(object sender, EventArgs e)
        { }

        public virtual void metroTrackBar1_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroTrackBar2_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroTrackBarIconSizePlayers_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroTrackBarLOSEnemy_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroTrackBarLOSPlayer_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroTrackBarOverlayBonesDrawDistance_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroTrackBarOverlayDrawDistance_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroTrackBarOverlayDrawDistanceLoot_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroTrackBarTextScale_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void PollSettingCenterMap(object sender, EventArgs e)
        { }

        public virtual void PollSettingMapDrawText(object sender, EventArgs e)
        { }

        public virtual void PollSettingMapShowLoot(object sender, EventArgs e)
        { }

        public void RecolorListViews()
        {
            switch (metroStyleManager.Theme)
            {
                case MetroThemeStyle.Dark:
                    metroListViewLootCategoriesShown.BackColor = ColorTranslator.FromHtml("#111111");
                    metroListViewLootSearchHighlight.BackColor = ColorTranslator.FromHtml("#111111");
                    break;

                default:
                    metroListViewLootCategoriesShown.BackColor = SystemColors.Control;
                    metroListViewLootSearchHighlight.BackColor = SystemColors.Control;
                    break;
            }
        }

        public virtual void RegisterEvents()
        { }

        public virtual void SettingsApplyJson()
        { }

        public virtual void SettingsLoadJson()
        {
        }

        public void SettingsSaveJson(object sender, EventArgs e)
        {
            SettingsSaveJson();
        }

        public virtual void SettingsSaveJson()
        { }

        public virtual void toolStripMenuItemPutMeHere_Click(object sender, EventArgs e)
        { }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SettingsSaveJson();
            this.Hide();
            e.Cancel = true;
        }

        #endregion Protected Methods

        #region Private Methods

        private void ApplyThemeAndStyle()
        {
        }

        private void DeFocusElement() => Focus();

        private void metroComboBoxMapResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mapResText = metroComboBoxMapResolution.SelectedItem.ToString();
            MapManagerSwitchMap(mapResText, metroComboBoxMapResolution.SelectedIndex);
        }

        private void metroListViewLootCategoriesShown_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (metroListViewLootCategoriesShown.Items.Count < 2)
            {
                return;
            }

            ItemComparer sorter = metroListViewLootCategoriesShown.ListViewItemSorter as ItemComparer;
            if (sorter == null)
            {
                sorter = new ItemComparer(e.Column);
                sorter.Order = SortOrder.Ascending;
                metroListViewLootCategoriesShown.ListViewItemSorter = sorter;
            }

            if (e.Column == sorter.Column)
            {
                if (sorter.Order == SortOrder.Ascending)
                    sorter.Order = SortOrder.Descending;
                else
                    sorter.Order = SortOrder.Ascending;
            }
            else
            {
                sorter.Column = e.Column;
                sorter.Order = SortOrder.Ascending;
            }

            this.metroListViewLootCategoriesShown.Sort();
        }

        private void metroListViewLootCategoriesShown_Layout(object sender, LayoutEventArgs e)
        {
            ShowScrollBar(this.metroListViewLootCategoriesShown.Handle, (int)SB_BOTH, false);
        }

        private void metroListViewLootSearchHighlight_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (metroListViewLootSearchHighlight.Items.Count < 2)
            {
                return;
            }

            ItemComparer sorter = metroListViewLootSearchHighlight.ListViewItemSorter as ItemComparer;

            if (sorter == null)
            {
                sorter = new ItemComparer(e.Column);
                sorter.Order = SortOrder.Ascending;
                metroListViewLootSearchHighlight.ListViewItemSorter = sorter;
            }

            if (e.Column == sorter.Column)
            {
                if (sorter.Order == SortOrder.Ascending)
                    sorter.Order = SortOrder.Descending;
                else
                    sorter.Order = SortOrder.Ascending;
            }
            else
            {
                sorter.Column = e.Column;
                sorter.Order = SortOrder.Ascending;
            }

            this.metroListViewLootSearchHighlight.Sort();
        }

        private void metroListViewLootSearchHighlight_Layout(object sender, LayoutEventArgs e)
        {
            ShowScrollBar(this.metroListViewLootSearchHighlight.Handle, (int)SB_BOTH, false);
        }

        private void metroToggleWindowOnTop_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void OnLogSaved(object sender, EventArgs args)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                metroButtonSaveLog.Text = "Save Log";
            });
        }

        private void PopulateMaps()
        {
        }

        private bool ValidateTextBox(MetroFramework.Controls.MetroTextBox intext, ushort maxvalue, out ushort result)
        {
            intext.UseCustomBackColor = true;
            intext.BackColor = Color.Red;
            intext.Text = System.Text.RegularExpressions.Regex.Replace(intext.Text, "[^0-9]", "");

            var parse = UInt16.TryParse(intext.Text, out ushort _result);

            if (intext.Text.Length > 0)
            {
                result = _result;

                if (_result > maxvalue)
                {
                    result = maxvalue;
                }

                intext.Text = result.ToString();
                intext.SelectionStart = intext.Text.Length;
                intext.SelectionLength = 0;

                intext.BackColor = System.Drawing.SystemColors.Control;
                intext.UseCustomBackColor = false;

                return true;
            }

            result = 0;
            return false;
        }

        #endregion Private Methods

        public virtual void metroListViewLootCategoriesShown_DoubleClick(object sender, EventArgs e)
        { }

        public virtual void metroListViewLootCategoriesShown_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        { }

        public virtual void metroListViewLootCategoriesShown_KeyPress(object sender, KeyPressEventArgs e)
        { }

        public virtual void toolStripMenuItemEnableAll_Click(object sender, EventArgs e)
        { }

        public virtual void toolStripMenuItemDisableAll_Click(object sender, EventArgs e)
        { }

        public virtual void toolStripMenuItemEnable_Click(object sender, EventArgs e)
        { }

        public virtual void toolStripMenuItemDisable_Click(object sender, EventArgs e)
        { }

        private void metroListViewLootCategoriesShown_MouseDown(object sender, MouseEventArgs e)
        {
            if (metroListViewLootCategoriesShown.SelectedItems.Count > 0 && e.Button == MouseButtons.Right)
            {
                metroContextMenuLootCategoriesShown.Show(metroListViewLootCategoriesShown, new Point(e.X, e.Y));
            }
        }

        public void metroListView_AddItem(ListView lvw, string entity, string status)
        {
            ListViewItem item = new ListViewItem(entity.Replace("&", "&&"));

            ItemStatus entityTypeStatus = new ItemStatus(entity, status);
            item.Tag = entityTypeStatus;

            lvw.Items.Add(item);

            item.SubItems.Add("");
        }

        public class ItemStatus
        {
            public string ServerName;
            public string Status;

            public ItemStatus(string serverName, string status)
            {
                ServerName = serverName;
                Status = status;
            }
        }

        public virtual void metroTrackBar3_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroTrackBarIconSizeLoot_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroCheckBoxDebugStuff_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroButtonLogReport_Click(object sender, EventArgs e)
        { }

        public virtual void metroTextBox1_TextChanged(object sender, EventArgs e)
        { }

        public virtual void metroTextBox2_TextChanged(object sender, EventArgs e)
        { }

        public virtual void metroTextBox3_TextChanged(object sender, EventArgs e)
        { }

        public virtual void metroToggleTraceLog_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroToggleOpenGlDebug_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroRadioButtonOverlayWindowStyleMoonlight_CheckedChanged(object sender, EventArgs e)
        { }

        public delegate void RadarFormOpacitySliderHandler(float str);

        public delegate void RadarFormTopMostToggleHandler(bool value);

        public event RadarFormOpacitySliderHandler OnRadarFormOpacitySliderValueChangeEvent;

        public event RadarFormTopMostToggleHandler OnRadarFormTopMostToggleChangeEvent;

        private void metroTrackBarOverlayOpacity_Scroll(object sender, ScrollEventArgs e)
        {
            SetOpacityRadarForm(metroTrackBarOverlayOpacity.Value);
        }

        public void SetOpacityRadarForm(float value)
        {
            OnRadarFormOpacitySliderValueChangeEvent.Invoke(value);
            metroTextBoxOverlayOpacity.Text = metroTrackBarOverlayOpacity.Value.ToString();
        }

        private void metroToggleWindowAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            SetTopMostRadarForm(metroToggleWindowAlwaysOnTop.Checked);
        }

        private void SetTopMostRadarForm(bool value)
        {
            OnRadarFormTopMostToggleChangeEvent?.Invoke(value);
            BringToFront();
        }

        public virtual void metroButtonWriteMemoryHelp_Click(object sender, EventArgs e)
        { }

        public event DisclaimerHandler OnDisclaimerResult;

        public delegate void DisclaimerHandler(bool result);

        internal void ShowDisclaimer()
        {
            DisclaimerWindow disclaimerWindow = new DisclaimerWindow();
            string result = "";
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "NormandyNET.Modules.UI.WriteMemoryDisclaimer.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            disclaimerWindow.Text = "Write Memory Disclaimer";
            disclaimerWindow.metroTextBoxChangeLog.Text = result;
            disclaimerWindow.ShowDialog();

            if (disclaimerWindow.dialogResult == DialogResult.OK)
            {
                OnDisclaimerResult?.Invoke(true);
            }
        }

        public virtual void metroCheckBoxHideTextAroundPlayer_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroTrackBarHideTextRadius_Scroll(object sender, ScrollEventArgs e)
        { }

        public virtual void metroCheckBoxEntityLOSLineType_CheckedChanged(object sender, EventArgs e)
        { }

        public virtual void metroButtonWriteMemoryDisclaimer_Click(object sender, EventArgs e)
        {
            ShowDisclaimer();
        }
    }
}