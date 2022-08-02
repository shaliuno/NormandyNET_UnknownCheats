namespace NormandyNET.UI
{
    partial class DisclaimerWindow
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
            this.metroButtonYes = new MetroFramework.Controls.MetroButton();
            this.metroTextBoxChangeLog = new MetroFramework.Controls.MetroTextBox();
            this.metroButtonNo = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            
            
            
            this.metroStyleManager.Owner = this;
            
            
            
            this.metroButtonYes.Location = new System.Drawing.Point(702, 358);
            this.metroButtonYes.Name = "metroButtonYes";
            this.metroButtonYes.Size = new System.Drawing.Size(75, 29);
            this.metroButtonYes.TabIndex = 2;
            this.metroButtonYes.Text = "I agree";
            this.metroButtonYes.UseSelectable = true;
            this.metroButtonYes.Click += new System.EventHandler(this.metroButtonYes_Click);
            
            
            
            
            
            
            this.metroTextBoxChangeLog.CustomButton.Image = null;
            this.metroTextBoxChangeLog.CustomButton.Location = new System.Drawing.Point(467, 2);
            this.metroTextBoxChangeLog.CustomButton.Name = "";
            this.metroTextBoxChangeLog.CustomButton.Size = new System.Drawing.Size(287, 287);
            this.metroTextBoxChangeLog.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxChangeLog.CustomButton.TabIndex = 1;
            this.metroTextBoxChangeLog.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxChangeLog.CustomButton.UseSelectable = true;
            this.metroTextBoxChangeLog.CustomButton.Visible = false;
            this.metroTextBoxChangeLog.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.metroTextBoxChangeLog.Lines = new string[0];
            this.metroTextBoxChangeLog.Location = new System.Drawing.Point(20, 60);
            this.metroTextBoxChangeLog.MaxLength = 32767;
            this.metroTextBoxChangeLog.Multiline = true;
            this.metroTextBoxChangeLog.Name = "metroTextBoxChangeLog";
            this.metroTextBoxChangeLog.PasswordChar = '\0';
            this.metroTextBoxChangeLog.ReadOnly = true;
            this.metroTextBoxChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.metroTextBoxChangeLog.SelectedText = "";
            this.metroTextBoxChangeLog.SelectionLength = 0;
            this.metroTextBoxChangeLog.SelectionStart = 0;
            this.metroTextBoxChangeLog.ShortcutsEnabled = true;
            this.metroTextBoxChangeLog.Size = new System.Drawing.Size(757, 292);
            this.metroTextBoxChangeLog.TabIndex = 3;
            this.metroTextBoxChangeLog.UseSelectable = true;
            this.metroTextBoxChangeLog.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxChangeLog.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            
            
            
            this.metroButtonNo.Location = new System.Drawing.Point(621, 358);
            this.metroButtonNo.Name = "metroButtonNo";
            this.metroButtonNo.Size = new System.Drawing.Size(75, 29);
            this.metroButtonNo.TabIndex = 2;
            this.metroButtonNo.Text = "Not now";
            this.metroButtonNo.UseSelectable = true;
            this.metroButtonNo.Click += new System.EventHandler(this.metroButtonNo_Click);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 404);
            this.Controls.Add(this.metroTextBoxChangeLog);
            this.Controls.Add(this.metroButtonNo);
            this.Controls.Add(this.metroButtonYes);
            this.Name = "InfoWindow";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StyleManager = this.metroStyleManager;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Controls.MetroButton metroButtonYes;
        internal MetroFramework.Controls.MetroTextBox metroTextBoxChangeLog;
        private MetroFramework.Controls.MetroButton metroButtonNo;
    }
}