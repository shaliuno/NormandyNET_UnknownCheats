namespace NormandyNET.UI
{
    partial class ChangeLog
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
            this.metroTextBoxChangeLog = new MetroFramework.Controls.MetroTextBox();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            
            
            
            this.metroStyleManager.Owner = this;
            
            
            
            
            
            
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
            this.metroTextBoxChangeLog.TabIndex = 0;
            this.metroTextBoxChangeLog.UseSelectable = true;
            this.metroTextBoxChangeLog.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxChangeLog.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            
            
            
            this.metroButton1.Location = new System.Drawing.Point(702, 358);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(75, 23);
            this.metroButton1.TabIndex = 2;
            this.metroButton1.Text = "OK";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 404);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.metroTextBoxChangeLog);
            this.Name = "ChangeLog";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StyleManager = this.metroStyleManager;
            this.Text = "Change Log";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        internal MetroFramework.Controls.MetroTextBox metroTextBoxChangeLog;
        private MetroFramework.Controls.MetroButton metroButton1;
    }
}