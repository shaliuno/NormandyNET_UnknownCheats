using MetroFramework.Forms;
using System;
using System.Windows.Forms;

namespace NormandyNET.UI
{
    public partial class AimBotReferences : MetroForm
    {
        internal DialogResult dialogResult;

        public AimBotReferences()
        {
            InitializeComponent();
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
        }

        private void metroButtonYes_Click(object sender, EventArgs e)
        {
            DisclaimerWindowCustomDialog customDialog = new DisclaimerWindowCustomDialog();
            customDialog.ShowDialog();

            if (customDialog.dialogResult == DialogResult.OK)
            {
                dialogResult = customDialog.dialogResult;
                this.Close();
            }
        }

        private void metroButtonNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}