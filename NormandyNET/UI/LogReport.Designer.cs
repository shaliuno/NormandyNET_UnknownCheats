namespace NormandyNET.UI
{
    partial class LogReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogReport));
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroLabelLogInfo = new MetroFramework.Controls.MetroLabel();
            this.metroButtonLogUrlSend = new MetroFramework.Controls.MetroButton();
            this.metroTextBoxLogUrl = new MetroFramework.Controls.MetroTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            
            
            
            this.metroStyleManager.Owner = this;
            
            
            
            this.metroLabelLogInfo.Location = new System.Drawing.Point(23, 60);
            this.metroLabelLogInfo.Name = "metroLabelLogInfo";
            this.metroLabelLogInfo.Size = new System.Drawing.Size(470, 120);
            this.metroLabelLogInfo.TabIndex = 4;
            this.metroLabelLogInfo.Text = resources.GetString("metroLabelLogInfo.Text");
            
            
            
            this.metroButtonLogUrlSend.Location = new System.Drawing.Point(418, 245);
            this.metroButtonLogUrlSend.Name = "metroButtonLogUrlSend";
            this.metroButtonLogUrlSend.Size = new System.Drawing.Size(75, 23);
            this.metroButtonLogUrlSend.TabIndex = 5;
            this.metroButtonLogUrlSend.Text = "Send Url";
            this.metroButtonLogUrlSend.UseSelectable = true;
            
            
            
            
            
            
            this.metroTextBoxLogUrl.CustomButton.Image = null;
            this.metroTextBoxLogUrl.CustomButton.Location = new System.Drawing.Point(321, 2);
            this.metroTextBoxLogUrl.CustomButton.Name = "";
            this.metroTextBoxLogUrl.CustomButton.Size = new System.Drawing.Size(41, 41);
            this.metroTextBoxLogUrl.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxLogUrl.CustomButton.TabIndex = 1;
            this.metroTextBoxLogUrl.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxLogUrl.CustomButton.UseSelectable = true;
            this.metroTextBoxLogUrl.CustomButton.Visible = false;
            this.metroTextBoxLogUrl.Lines = new string[] {
        "type url here"};
            this.metroTextBoxLogUrl.Location = new System.Drawing.Point(23, 193);
            this.metroTextBoxLogUrl.MaxLength = 32767;
            this.metroTextBoxLogUrl.Multiline = true;
            this.metroTextBoxLogUrl.Name = "metroTextBoxLogUrl";
            this.metroTextBoxLogUrl.PasswordChar = '\0';
            this.metroTextBoxLogUrl.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxLogUrl.SelectedText = "";
            this.metroTextBoxLogUrl.SelectionLength = 0;
            this.metroTextBoxLogUrl.SelectionStart = 0;
            this.metroTextBoxLogUrl.ShortcutsEnabled = true;
            this.metroTextBoxLogUrl.Size = new System.Drawing.Size(470, 46);
            this.metroTextBoxLogUrl.TabIndex = 6;
            this.metroTextBoxLogUrl.Text = "type url here";
            this.metroTextBoxLogUrl.UseSelectable = true;
            this.metroTextBoxLogUrl.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxLogUrl.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 281);
            this.Controls.Add(this.metroTextBoxLogUrl);
            this.Controls.Add(this.metroButtonLogUrlSend);
            this.Controls.Add(this.metroLabelLogInfo);
            this.Name = "LogReport";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StyleManager = this.metroStyleManager;
            this.Text = "Log Report";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Controls.MetroLabel metroLabelLogInfo;
        private MetroFramework.Controls.MetroTextBox metroTextBoxLogUrl;
        private MetroFramework.Controls.MetroButton metroButtonLogUrlSend;
    }
}