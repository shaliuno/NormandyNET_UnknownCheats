using MetroFramework.Forms;
using System;

namespace NormandyNET.UI
{
    public partial class ChangeLog : MetroForm
    {
        public ChangeLog()
        {
            InitializeComponent();
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}