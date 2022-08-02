namespace NormandyNET.Modules.RUST.UI
{
    partial class IconColors
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolTipSpecial = new System.Windows.Forms.ToolTip(this.components);
            this.buttonIconColorsSave = new MetroFramework.Controls.MetroButton();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroListViewEntityTypeColors = new MetroFramework.Controls.MetroListView();
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            
            
            
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            
            
            
            this.buttonIconColorsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonIconColorsSave.Location = new System.Drawing.Point(208, 70);
            this.buttonIconColorsSave.Name = "buttonIconColorsSave";
            this.buttonIconColorsSave.Size = new System.Drawing.Size(75, 23);
            this.buttonIconColorsSave.TabIndex = 7;
            this.buttonIconColorsSave.Text = "Save";
            this.buttonIconColorsSave.UseSelectable = true;
            this.buttonIconColorsSave.Click += new System.EventHandler(this.buttonIconColorsSave_Click);
            
            
            
            this.metroStyleManager.Owner = this;
            
            
            
            this.metroListViewEntityTypeColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.metroListViewEntityTypeColors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnType,
            this.columnColor});
            this.metroListViewEntityTypeColors.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.metroListViewEntityTypeColors.FullRowSelect = true;
            this.metroListViewEntityTypeColors.Location = new System.Drawing.Point(23, 63);
            this.metroListViewEntityTypeColors.Name = "metroListViewEntityTypeColors";
            this.metroListViewEntityTypeColors.OwnerDraw = true;
            this.metroListViewEntityTypeColors.Size = new System.Drawing.Size(260, 1);
            this.metroListViewEntityTypeColors.TabIndex = 20;
            this.metroListViewEntityTypeColors.UseCompatibleStateImageBehavior = false;
            this.metroListViewEntityTypeColors.UseSelectable = true;
            this.metroListViewEntityTypeColors.View = System.Windows.Forms.View.Details;
            this.metroListViewEntityTypeColors.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.metroListViewEntityTypeColors_DrawSubItem);
            this.metroListViewEntityTypeColors.DoubleClick += new System.EventHandler(this.metroListViewEntityTypeColors_DoubleClick);
            this.metroListViewEntityTypeColors.Layout += new System.Windows.Forms.LayoutEventHandler(this.metroListViewEntityTypeColors_Layout);
            
            
            
            this.columnType.Text = "Type";
            this.columnType.Width = 180;
            
            
            
            this.columnColor.Text = "Color";
            this.columnColor.Width = 80;
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 106);
            this.Controls.Add(this.metroListViewEntityTypeColors);
            this.Controls.Add(this.buttonIconColorsSave);
            this.Name = "IconColors";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.StyleManager = this.metroStyleManager;
            this.Text = "Icon Colors";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.IconColors_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolTip toolTipSpecial;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Controls.MetroButton buttonIconColorsSave;
        public MetroFramework.Controls.MetroListView metroListViewEntityTypeColors;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnColor;
    }
}