namespace NormandyNET.Modules.ARMA.UI
{
    partial class LootSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LootSettings));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolTipSpecial = new System.Windows.Forms.ToolTip(this.components);
            this.metroLabelLootLiveAmountPerCycle = new MetroFramework.Controls.MetroLabel();
            this.buttonLootColorsSave = new MetroFramework.Controls.MetroButton();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroListViewLootCategoryColors = new MetroFramework.Controls.MetroListView();
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroListViewLootCategoryColors2 = new MetroFramework.Controls.MetroListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroCheckBoxLootLive = new MetroFramework.Controls.MetroCheckBox();
            this.metroTextBoxLootLiveAmountPerCycle = new MetroFramework.Controls.MetroTextBox();
            this.metroTrackBarLootLiveAmountPerCycle = new MetroFramework.Controls.MetroTrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            
            
            
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            
            
            
            this.metroLabelLootLiveAmountPerCycle.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroLabelLootLiveAmountPerCycle.Location = new System.Drawing.Point(564, 89);
            this.metroLabelLootLiveAmountPerCycle.Name = "metroLabelLootLiveAmountPerCycle";
            this.metroLabelLootLiveAmountPerCycle.Size = new System.Drawing.Size(162, 23);
            this.metroLabelLootLiveAmountPerCycle.TabIndex = 26;
            this.metroLabelLootLiveAmountPerCycle.Text = "Loot Readings per Cycle";
            this.toolTipSpecial.SetToolTip(this.metroLabelLootLiveAmountPerCycle, resources.GetString("metroLabelLootLiveAmountPerCycle.ToolTip"));
            
            
            
            this.buttonLootColorsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLootColorsSave.Location = new System.Drawing.Point(208, 518);
            this.buttonLootColorsSave.Name = "buttonLootColorsSave";
            this.buttonLootColorsSave.Size = new System.Drawing.Size(75, 23);
            this.buttonLootColorsSave.TabIndex = 7;
            this.buttonLootColorsSave.Text = "Save";
            this.buttonLootColorsSave.UseSelectable = true;
            this.buttonLootColorsSave.Click += new System.EventHandler(this.buttonLootColorsSave_Click);
            
            
            
            this.metroStyleManager.Owner = this;
            
            
            
            this.metroListViewLootCategoryColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.metroListViewLootCategoryColors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnType,
            this.columnColor});
            this.metroListViewLootCategoryColors.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.metroListViewLootCategoryColors.FullRowSelect = true;
            this.metroListViewLootCategoryColors.Location = new System.Drawing.Point(23, 63);
            this.metroListViewLootCategoryColors.Name = "metroListViewLootCategoryColors";
            this.metroListViewLootCategoryColors.OwnerDraw = true;
            this.metroListViewLootCategoryColors.Size = new System.Drawing.Size(260, 449);
            this.metroListViewLootCategoryColors.TabIndex = 20;
            this.metroListViewLootCategoryColors.UseCompatibleStateImageBehavior = false;
            this.metroListViewLootCategoryColors.UseSelectable = true;
            this.metroListViewLootCategoryColors.View = System.Windows.Forms.View.Details;
            this.metroListViewLootCategoryColors.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.metroListViewLootCategoryColors_ColumnClick);
            this.metroListViewLootCategoryColors.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.metroListViewLootCategoryColors_DrawSubItem);
            this.metroListViewLootCategoryColors.DoubleClick += new System.EventHandler(this.metroListViewLootCategoryColors_DoubleClick);
            this.metroListViewLootCategoryColors.Layout += new System.Windows.Forms.LayoutEventHandler(this.metroListViewLootCategoryColors_Layout);
            
            
            
            this.columnType.Text = "Type";
            this.columnType.Width = 180;
            
            
            
            this.columnColor.Text = "Color";
            this.columnColor.Width = 80;
            
            
            
            this.metroListViewLootCategoryColors2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.metroListViewLootCategoryColors2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.metroListViewLootCategoryColors2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.metroListViewLootCategoryColors2.FullRowSelect = true;
            this.metroListViewLootCategoryColors2.Location = new System.Drawing.Point(301, 63);
            this.metroListViewLootCategoryColors2.Name = "metroListViewLootCategoryColors2";
            this.metroListViewLootCategoryColors2.OwnerDraw = true;
            this.metroListViewLootCategoryColors2.Size = new System.Drawing.Size(260, 449);
            this.metroListViewLootCategoryColors2.TabIndex = 20;
            this.metroListViewLootCategoryColors2.UseCompatibleStateImageBehavior = false;
            this.metroListViewLootCategoryColors2.UseSelectable = true;
            this.metroListViewLootCategoryColors2.View = System.Windows.Forms.View.Details;
            this.metroListViewLootCategoryColors2.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.metroListViewLootCategoryColors_ColumnClick);
            this.metroListViewLootCategoryColors2.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.metroListViewLootCategoryColors_DrawSubItem);
            this.metroListViewLootCategoryColors2.DoubleClick += new System.EventHandler(this.metroListViewLootCategoryColors_DoubleClick);
            this.metroListViewLootCategoryColors2.Layout += new System.Windows.Forms.LayoutEventHandler(this.metroListViewLootCategoryColors_Layout);
            
            
            
            this.columnHeader1.Text = "Type";
            this.columnHeader1.Width = 180;
            
            
            
            this.columnHeader2.Text = "Color";
            this.columnHeader2.Width = 80;
            
            
            
            this.metroCheckBoxLootLive.Location = new System.Drawing.Point(567, 63);
            this.metroCheckBoxLootLive.Name = "metroCheckBoxLootLive";
            this.metroCheckBoxLootLive.Size = new System.Drawing.Size(159, 23);
            this.metroCheckBoxLootLive.TabIndex = 28;
            this.metroCheckBoxLootLive.Text = "Live Loot";
            this.metroCheckBoxLootLive.UseSelectable = true;
            this.metroCheckBoxLootLive.CheckedChanged += new System.EventHandler(this.metroCheckBoxLootLive_CheckedChanged);
            
            
            
            
            
            
            this.metroTextBoxLootLiveAmountPerCycle.CustomButton.Image = null;
            this.metroTextBoxLootLiveAmountPerCycle.CustomButton.Location = new System.Drawing.Point(20, 1);
            this.metroTextBoxLootLiveAmountPerCycle.CustomButton.Name = "";
            this.metroTextBoxLootLiveAmountPerCycle.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBoxLootLiveAmountPerCycle.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxLootLiveAmountPerCycle.CustomButton.TabIndex = 1;
            this.metroTextBoxLootLiveAmountPerCycle.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxLootLiveAmountPerCycle.CustomButton.UseSelectable = true;
            this.metroTextBoxLootLiveAmountPerCycle.CustomButton.Visible = false;
            this.metroTextBoxLootLiveAmountPerCycle.Lines = new string[0];
            this.metroTextBoxLootLiveAmountPerCycle.Location = new System.Drawing.Point(710, 115);
            this.metroTextBoxLootLiveAmountPerCycle.MaxLength = 32767;
            this.metroTextBoxLootLiveAmountPerCycle.Name = "metroTextBoxLootLiveAmountPerCycle";
            this.metroTextBoxLootLiveAmountPerCycle.PasswordChar = '\0';
            this.metroTextBoxLootLiveAmountPerCycle.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxLootLiveAmountPerCycle.SelectedText = "";
            this.metroTextBoxLootLiveAmountPerCycle.SelectionLength = 0;
            this.metroTextBoxLootLiveAmountPerCycle.SelectionStart = 0;
            this.metroTextBoxLootLiveAmountPerCycle.ShortcutsEnabled = true;
            this.metroTextBoxLootLiveAmountPerCycle.Size = new System.Drawing.Size(42, 23);
            this.metroTextBoxLootLiveAmountPerCycle.TabIndex = 27;
            this.metroTextBoxLootLiveAmountPerCycle.UseSelectable = true;
            this.metroTextBoxLootLiveAmountPerCycle.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxLootLiveAmountPerCycle.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            
            
            
            this.metroTrackBarLootLiveAmountPerCycle.BackColor = System.Drawing.Color.Transparent;
            this.metroTrackBarLootLiveAmountPerCycle.Location = new System.Drawing.Point(567, 115);
            this.metroTrackBarLootLiveAmountPerCycle.Maximum = 200;
            this.metroTrackBarLootLiveAmountPerCycle.Minimum = 10;
            this.metroTrackBarLootLiveAmountPerCycle.MouseWheelBarPartitions = 1;
            this.metroTrackBarLootLiveAmountPerCycle.Name = "metroTrackBarLootLiveAmountPerCycle";
            this.metroTrackBarLootLiveAmountPerCycle.Size = new System.Drawing.Size(137, 23);
            this.metroTrackBarLootLiveAmountPerCycle.TabIndex = 25;
            this.metroTrackBarLootLiveAmountPerCycle.Text = "metroTrackBarLootValuePerSlot";
            this.metroTrackBarLootLiveAmountPerCycle.Value = 10;
            this.metroTrackBarLootLiveAmountPerCycle.Scroll += new System.Windows.Forms.ScrollEventHandler(this.metroTrackBarLootLiveAmountPerCycle_Scroll);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 554);
            this.Controls.Add(this.metroCheckBoxLootLive);
            this.Controls.Add(this.metroTextBoxLootLiveAmountPerCycle);
            this.Controls.Add(this.metroLabelLootLiveAmountPerCycle);
            this.Controls.Add(this.metroTrackBarLootLiveAmountPerCycle);
            this.Controls.Add(this.metroListViewLootCategoryColors2);
            this.Controls.Add(this.metroListViewLootCategoryColors);
            this.Controls.Add(this.buttonLootColorsSave);
            this.Name = "LootSettings";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.StyleManager = this.metroStyleManager;
            this.Text = "Loot Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LootColors_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolTip toolTipSpecial;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Controls.MetroButton buttonLootColorsSave;
        public MetroFramework.Controls.MetroListView metroListViewLootCategoryColors;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnColor;
        public MetroFramework.Controls.MetroListView metroListViewLootCategoryColors2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private MetroFramework.Controls.MetroCheckBox metroCheckBoxLootLive;
        private MetroFramework.Controls.MetroTextBox metroTextBoxLootLiveAmountPerCycle;
        private MetroFramework.Controls.MetroLabel metroLabelLootLiveAmountPerCycle;
        private MetroFramework.Controls.MetroTrackBar metroTrackBarLootLiveAmountPerCycle;
    }
}