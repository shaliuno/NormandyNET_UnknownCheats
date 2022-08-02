using Helpers;
using MetroFramework;
using MetroFramework.Forms;
using NormandyNET.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static NormandyNET.Core.MapManager;

namespace NormandyNET
{
    internal partial class MapSelectorDialog : MetroForm
    {
        internal string serverAddress { get; set; }
        internal int serverPort { get; set; }
        internal uint lastPID { get; set; }
        internal static RadarForm radarForm;

        internal MapSelectorDialog(string _serverAddress, int _serverPort, uint _lastGameProcessID, RadarForm _radarForm, bool switchTo = false)
        {
            InitializeComponent();
            serverAddress = _serverAddress;
            serverPort = _serverPort;

            lastPID = _lastGameProcessID;
            metroTextBoxServerAddress.Text = serverAddress;
            metroTextBoxServerPort.Text = serverPort.ToString();
            metroTextBoxGameProcessID.Text = lastPID.ToString();

            radarForm = _radarForm;

            if (radarForm.mapManager.suppressMapSelection == false)
            {
                foreach (KeyValuePair<string, MapObject> mapObject in radarForm.mapManager.mapObjects)
                {
                    metroListViewMapsList.Items.Add(new ListViewItem(mapObject.Key));
                }

                this.Height = 131 + metroListViewMapsList.Items.Count * 40;
            }

            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
            RecolorListViews();

            foreach (ListViewItem item in metroListViewMapsList.Items)
            {
                if (item.Text == radarForm.mapManager.CurrentMap)
                {
                    item.Selected = true;
                    return;
                }
            }
        }

        private void RecolorListViews()
        {
            switch (metroStyleManager.Theme)
            {
                case MetroThemeStyle.Dark:
                    metroListViewMapsList.BackColor = ColorTranslator.FromHtml("#111111");
                    break;

                default:
                    metroListViewMapsList.BackColor = SystemColors.Control;
                    break;
            }
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        private void metroListViewMapsList_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void metroTextBoxServerAddress_TextChanged(object sender, EventArgs e)
        {
            var validIP = CommonHelpers.IsValidIP(metroTextBoxServerAddress.Text);

            if (validIP)
            {
                ThreadHelper.SetStyleMetroTile(this, metroTileServerAddressValid, MetroColorStyle.Green);
                serverAddress = metroTextBoxServerAddress.Text;
            }
            else
            {
                ThreadHelper.SetStyleMetroTile(this, metroTileServerAddressValid, MetroColorStyle.Red);
            }
        }

        private void metroTextBoxGameProcessID_TextChanged(object sender, EventArgs e)
        {
            uint i;
            bool result = uint.TryParse(metroTextBoxGameProcessID.Text, out i);

            if (result && i > 0)
            {
                ThreadHelper.SetStyleMetroTile(this, metroTileGameProcessID, MetroColorStyle.Green);
                lastPID = i;
            }
            else
            {
                ThreadHelper.SetStyleMetroTile(this, metroTileGameProcessID, MetroColorStyle.Red);
            }
        }

        private void metroButtonGo_Click(object sender, EventArgs e)
        {
            if (radarForm.mapManager.suppressMapSelection == false && radarForm.mapManager.CurrentMap == "")
            {
                MetroMessageBox.Show(RadarForm.ActiveForm, "Select a map!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void metroListViewMapsList_Click(object sender, EventArgs e)
        {
            radarForm.mapManager.CurrentMap = metroListViewMapsList.SelectedItems[0].Text;
        }

        private void metroTextBoxlServerPort_TextChanged(object sender, EventArgs e)
        {
            var validPort = CommonHelpers.IsValidPort(metroTextBoxServerPort.Text, out int port);

            if (validPort)
            {
                ThreadHelper.SetStyleMetroTile(this, metroTilelServerPortValid, MetroColorStyle.Green);
                serverPort = port;
            }
            else
            {
                ThreadHelper.SetStyleMetroTile(this, metroTilelServerPortValid, MetroColorStyle.Red);
            }
        }
    }
}