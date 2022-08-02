using MetroFramework.Forms;
using System;
using System.Windows.Forms;

namespace NormandyNET.UI
{
    public partial class DisclaimerWindowCustomDialog : MetroForm
    {
        internal DialogResult dialogResult;

        public DisclaimerWindowCustomDialog()
        {
            InitializeComponent();
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
            metroButtonNotOk.Focus();
        }

        private void metroButtonOk_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.OK;
            this.Close();
        }

        private void metroButtonNotOk_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.Abort;
            this.Close();
        }
    }
}