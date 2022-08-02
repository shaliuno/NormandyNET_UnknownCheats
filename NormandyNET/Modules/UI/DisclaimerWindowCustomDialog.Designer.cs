namespace NormandyNET.UI
{
    partial class DisclaimerWindowCustomDialog
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
            this.metroButtonNotOk = new MetroFramework.Controls.MetroButton();
            this.metroButtonOkay = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            
            
            
            this.metroStyleManager.Owner = this;
            
            
            
            this.metroButtonNotOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.metroButtonNotOk.Location = new System.Drawing.Point(267, 37);
            this.metroButtonNotOk.Name = "metroButtonNotOk";
            this.metroButtonNotOk.Size = new System.Drawing.Size(222, 29);
            this.metroButtonNotOk.TabIndex = 1;
            this.metroButtonNotOk.Text = "I am an idiot and I don\'t understand.";
            this.metroButtonNotOk.UseSelectable = true;
            this.metroButtonNotOk.Click += new System.EventHandler(this.metroButtonNotOk_Click);
            
            
            
            this.metroButtonOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonOkay.Location = new System.Drawing.Point(23, 37);
            this.metroButtonOkay.Name = "metroButtonOk";
            this.metroButtonOkay.Size = new System.Drawing.Size(222, 29);
            this.metroButtonOkay.TabIndex = 2;
            this.metroButtonOkay.Text = "I am not an idiot and I understand.";
            this.metroButtonOkay.UseSelectable = true;
            this.metroButtonOkay.Click += new System.EventHandler(this.metroButtonOk_Click);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 90);
            this.ControlBox = false;
            this.Controls.Add(this.metroButtonOkay);
            this.Controls.Add(this.metroButtonNotOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DisclaimerWindowCustomDialog";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StyleManager = this.metroStyleManager;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Controls.MetroButton metroButtonNotOk;
        private MetroFramework.Controls.MetroButton metroButtonOkay;
    }
}