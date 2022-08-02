namespace NormandyNET
{
    public partial class Overlay
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
            this.SuspendLayout();
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Overlay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Overlay";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosedOverlay);
            this.Load += new System.EventHandler(this.OverlayLoaded);
            this.ResizeBegin += new System.EventHandler(this.Overlay_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.Overlay_ResizeEnd);
            this.Resize += new System.EventHandler(this.Overlay_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}