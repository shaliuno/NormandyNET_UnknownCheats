namespace NormandyNET
{
    partial class StringPromptDialog
    {
        
        
        
        private System.ComponentModel.IContainer components = null;

        
        
        
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        
        
        
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.textBoxStringValue = new MetroFramework.Controls.MetroTextBox();
            this.buttonOK = new MetroFramework.Controls.MetroButton();
            this.buttonCancel = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            
            
            
            this.metroStyleManager.Owner = this;
            
            
            
            this.textBoxStringValue.CustomButton.Image = null;
            this.textBoxStringValue.CustomButton.Location = new System.Drawing.Point(237, 2);
            this.textBoxStringValue.CustomButton.Name = "";
            this.textBoxStringValue.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.textBoxStringValue.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBoxStringValue.CustomButton.TabIndex = 1;
            this.textBoxStringValue.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBoxStringValue.CustomButton.UseSelectable = true;
            this.textBoxStringValue.CustomButton.Visible = false;
            this.textBoxStringValue.Lines = new string[0];
            this.textBoxStringValue.Location = new System.Drawing.Point(23, 63);
            this.textBoxStringValue.MaxLength = 32767;
            this.textBoxStringValue.Name = "textBoxStringValue";
            this.textBoxStringValue.PasswordChar = '\0';
            this.textBoxStringValue.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxStringValue.SelectedText = "";
            this.textBoxStringValue.SelectionLength = 0;
            this.textBoxStringValue.SelectionStart = 0;
            this.textBoxStringValue.ShortcutsEnabled = true;
            this.textBoxStringValue.Size = new System.Drawing.Size(255, 20);
            this.textBoxStringValue.TabIndex = 2;
            this.textBoxStringValue.UseSelectable = true;
            this.textBoxStringValue.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textBoxStringValue.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            
            
            
            this.buttonOK.Location = new System.Drawing.Point(122, 89);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseSelectable = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            
            
            
            this.buttonCancel.Location = new System.Drawing.Point(203, 89);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseSelectable = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 129);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxStringValue);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StringPromptDialog";
            this.Resizable = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StyleManager = this.metroStyleManager;
            this.Text = "Enter value";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox textBoxStringValue;
        private MetroFramework.Controls.MetroButton buttonOK;
        private MetroFramework.Controls.MetroButton buttonCancel;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
    }
}