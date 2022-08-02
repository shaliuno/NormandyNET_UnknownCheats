using MetroFramework.Forms;
using NormandyNET.UI;
using System;
using System.Windows.Forms;

namespace NormandyNET
{
    public partial class StringPromptDialog : MetroForm
    {
        public string resultString = string.Empty;

        public string StringPrompt
        {
            get
            {
                char[] charsToTrim = { ' ', '"' };
                string result = textBoxStringValue.Text.Trim(charsToTrim);
                return result;
            }
        }

        public StringPromptDialog(string labelText, string existingValue = "")
        {
            InitializeComponent();

            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);

            this.Text = labelText;
            textBoxStringValue.Text = existingValue;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}