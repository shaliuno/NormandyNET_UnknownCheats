using MetroFramework.Forms;
using NormandyNET.UI;
using System;
using System.Windows.Forms;

namespace NormandyNET.Modules.EFT.UI
{
    public partial class IconColors : MetroForm
    {
        public IconColors()
        {
            InitializeComponent();

            buttonIconYouColor.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.You;
            buttonIconTeammateColor.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Teammate;
            buttonIconPMCColor.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.PMC;
            buttonIconBotColor.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Bot;
            buttonIconBotEliteColor.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotElite;
            buttonIconBotHumanColor.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotHuman;
            buttonIconSpecialColor.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Special;
            buttonIconColorCorpse.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Corpse;
            buttonIconBossColor.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Boss;
            buttonIconGrenadeColor.BackColor = ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Grenade;
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
        }

        private void buttonIconYou_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconYouColor.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.You = buttonIconYouColor.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }

        private void buttonIconTeammate_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconTeammateColor.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Teammate = buttonIconTeammateColor.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }

        private void buttonIconPMC_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconPMCColor.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.PMC = buttonIconPMCColor.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }

        private void buttonIconBot_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconBotColor.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Bot = buttonIconBotColor.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }

        private void buttonIconBotElite_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconBotEliteColor.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotElite = buttonIconBotEliteColor.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }

        private void buttonIconBotHuman_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconBotHumanColor.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotHuman = buttonIconBotHumanColor.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }

        private void buttonIconSpecial_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconSpecialColor.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Special = buttonIconSpecialColor.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }

        private void buttonIconColorsSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
        }

        private void buttonIconCorpse_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconCorpse.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Corpse = buttonIconCorpse.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }

        private void buttonIconBoss_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconBossColor.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Boss = buttonIconBossColor.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }

        private void metroButtonGrenade_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                buttonIconGrenadeColor.BackColor = colorDlg.Color;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Grenade = buttonIconGrenadeColor.BackColor;
                ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
            }
        }
    }
}