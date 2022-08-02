namespace NormandyNET.Modules.DAYZ.UI
{
    partial class SettingsFormDAYZ
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
            this.metroCheckBoxMapNetworkBubble_DAYZ = new MetroFramework.Controls.MetroCheckBox();
            this.metroListViewEntityTypes_DAYZ = new MetroFramework.Controls.MetroListView();
            this.columnEntityType_DAYZ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEntityChecked_DAYZ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroButtonEntityDumpUnknown_DAYZ = new MetroFramework.Controls.MetroButton();
            this.metroTextBoxIconSizeInfected_DAYZ = new MetroFramework.Controls.MetroTextBox();
            this.metroTrackBarIconSizeInfected_DAYZ = new MetroFramework.Controls.MetroTrackBar();
            this.metroLabelIconSizeInfected_DAYZ = new MetroFramework.Controls.MetroLabel();
            this.metroCheckBoxProximityAlert_DAYZ = new MetroFramework.Controls.MetroCheckBox();
            this.metroLabelWriteMemoryNoGrass_DAYZ = new MetroFramework.Controls.MetroLabel();
            this.metroTabPageMain.SuspendLayout();
            this.metroTabPageLoot.SuspendLayout();
            this.metroTabPageOther.SuspendLayout();
            this.metroTabPageDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.metroTabPageMemoryWriting.SuspendLayout();
            this.SuspendLayout();
            
            
            
            this.metroTabPageMain.Controls.Add(this.metroListViewEntityTypes_DAYZ);
            this.metroTabPageMain.Controls.Add(this.metroCheckBoxProximityAlert_DAYZ);
            this.metroTabPageMain.Controls.Add(this.metroCheckBoxMapNetworkBubble_DAYZ);
            this.metroTabPageMain.Controls.SetChildIndex(this.metroCheckBoxMapNetworkBubble_DAYZ, 0);
            this.metroTabPageMain.Controls.SetChildIndex(this.metroCheckBoxProximityAlert_DAYZ, 0);
            this.metroTabPageMain.Controls.SetChildIndex(this.metroListViewEntityTypes_DAYZ, 0);
            
            
            
            this.metroTabPageOther.Controls.Add(this.metroTextBoxIconSizeInfected_DAYZ);
            this.metroTabPageOther.Controls.Add(this.metroTrackBarIconSizeInfected_DAYZ);
            this.metroTabPageOther.Controls.Add(this.metroLabelIconSizeInfected_DAYZ);
            this.metroTabPageOther.Controls.SetChildIndex(this.metroLabelIconSizeInfected_DAYZ, 0);
            this.metroTabPageOther.Controls.SetChildIndex(this.metroTrackBarIconSizeInfected_DAYZ, 0);
            this.metroTabPageOther.Controls.SetChildIndex(this.metroTextBoxIconSizeInfected_DAYZ, 0);
            
            
            
            this.metroTabPageMemoryWriting.Controls.Add(this.metroLabelWriteMemoryNoGrass_DAYZ);
            this.metroTabPageMemoryWriting.Controls.SetChildIndex(this.metroLabelWriteMemoryNoGrass_DAYZ, 0);
            
            
            
            this.metroTabPageDebug.Controls.Add(this.metroButtonEntityDumpUnknown_DAYZ);
            this.metroTabPageDebug.Controls.SetChildIndex(this.metroButtonEntityDumpUnknown_DAYZ, 0);
            
            
            
            this.metroCheckBoxMapNetworkBubble_DAYZ.AutoSize = true;
            this.metroCheckBoxMapNetworkBubble_DAYZ.Location = new System.Drawing.Point(140, 22);
            this.metroCheckBoxMapNetworkBubble_DAYZ.Name = "metroCheckBoxMapNetworkBubble_DAYZ";
            this.metroCheckBoxMapNetworkBubble_DAYZ.Size = new System.Drawing.Size(120, 15);
            this.metroCheckBoxMapNetworkBubble_DAYZ.TabIndex = 6;
            this.metroCheckBoxMapNetworkBubble_DAYZ.Text = "Net Bubble Circles";
            this.metroCheckBoxMapNetworkBubble_DAYZ.UseSelectable = true;
            this.metroCheckBoxMapNetworkBubble_DAYZ.CheckedChanged += new System.EventHandler(this.metroCheckBoxMapNetworkBubble_CheckedChanged);
            
            
            
            this.metroListViewEntityTypes_DAYZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroListViewEntityTypes_DAYZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroListViewEntityTypes_DAYZ.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnEntityType_DAYZ,
            this.columnEntityChecked_DAYZ});
            this.metroListViewEntityTypes_DAYZ.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.metroListViewEntityTypes_DAYZ.FullRowSelect = true;
            this.metroListViewEntityTypes_DAYZ.LabelWrap = false;
            this.metroListViewEntityTypes_DAYZ.Location = new System.Drawing.Point(0, 280);
            this.metroListViewEntityTypes_DAYZ.Name = "metroListViewEntityTypes_DAYZ";
            this.metroListViewEntityTypes_DAYZ.OwnerDraw = true;
            this.metroListViewEntityTypes_DAYZ.Size = new System.Drawing.Size(292, 255);
            this.metroListViewEntityTypes_DAYZ.TabIndex = 7;
            this.metroListViewEntityTypes_DAYZ.UseCompatibleStateImageBehavior = false;
            this.metroListViewEntityTypes_DAYZ.UseSelectable = true;
            this.metroListViewEntityTypes_DAYZ.View = System.Windows.Forms.View.Details;
            this.metroListViewEntityTypes_DAYZ.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.metroListViewEntityTypes_DrawSubItem);
            this.metroListViewEntityTypes_DAYZ.DoubleClick += new System.EventHandler(this.metroListViewEntityTypes_DoubleClick);
            this.metroListViewEntityTypes_DAYZ.Layout += new System.Windows.Forms.LayoutEventHandler(this.metroListViewEntityTypes_Layout);
            
            
            
            this.columnEntityType_DAYZ.Text = "Type";
            this.columnEntityType_DAYZ.Width = 260;
            
            
            
            this.columnEntityChecked_DAYZ.Text = "";
            this.columnEntityChecked_DAYZ.Width = 30;
            
            
            
            this.metroButtonEntityDumpUnknown_DAYZ.Location = new System.Drawing.Point(137, 266);
            this.metroButtonEntityDumpUnknown_DAYZ.Name = "metroButtonEntityDumpUnknown_DAYZ";
            this.metroButtonEntityDumpUnknown_DAYZ.Size = new System.Drawing.Size(152, 23);
            this.metroButtonEntityDumpUnknown_DAYZ.TabIndex = 31;
            this.metroButtonEntityDumpUnknown_DAYZ.Text = "Dump Unknown Entities";
            this.metroButtonEntityDumpUnknown_DAYZ.UseSelectable = true;
            this.metroButtonEntityDumpUnknown_DAYZ.Click += new System.EventHandler(this.metroButtonEntityDumpUnknown_Click);
            
            
            
            this.metroTrackBarOverlayDrawDistanceLoot.Maximum = 1000;
            
            
            
            this.metroTrackBarOverlayDrawDistance.Maximum = 1000;
            
            
            
            this.metroTextBoxIconSizeInfected_DAYZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            
            
            
            this.metroTextBoxIconSizeInfected_DAYZ.CustomButton.Image = null;
            this.metroTextBoxIconSizeInfected_DAYZ.CustomButton.Location = new System.Drawing.Point(19, 1);
            this.metroTextBoxIconSizeInfected_DAYZ.CustomButton.Name = "";
            this.metroTextBoxIconSizeInfected_DAYZ.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBoxIconSizeInfected_DAYZ.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxIconSizeInfected_DAYZ.CustomButton.TabIndex = 1;
            this.metroTextBoxIconSizeInfected_DAYZ.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxIconSizeInfected_DAYZ.CustomButton.UseSelectable = true;
            this.metroTextBoxIconSizeInfected_DAYZ.CustomButton.Visible = false;
            this.metroTextBoxIconSizeInfected_DAYZ.Lines = new string[0];
            this.metroTextBoxIconSizeInfected_DAYZ.Location = new System.Drawing.Point(251, 262);
            this.metroTextBoxIconSizeInfected_DAYZ.MaxLength = 32767;
            this.metroTextBoxIconSizeInfected_DAYZ.Name = "metroTextBoxIconSizeInfected_DAYZ";
            this.metroTextBoxIconSizeInfected_DAYZ.PasswordChar = '\0';
            this.metroTextBoxIconSizeInfected_DAYZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxIconSizeInfected_DAYZ.SelectedText = "";
            this.metroTextBoxIconSizeInfected_DAYZ.SelectionLength = 0;
            this.metroTextBoxIconSizeInfected_DAYZ.SelectionStart = 0;
            this.metroTextBoxIconSizeInfected_DAYZ.ShortcutsEnabled = true;
            this.metroTextBoxIconSizeInfected_DAYZ.Size = new System.Drawing.Size(41, 23);
            this.metroTextBoxIconSizeInfected_DAYZ.TabIndex = 28;
            this.metroTextBoxIconSizeInfected_DAYZ.UseSelectable = true;
            this.metroTextBoxIconSizeInfected_DAYZ.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxIconSizeInfected_DAYZ.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            
            
            
            this.metroTrackBarIconSizeInfected_DAYZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroTrackBarIconSizeInfected_DAYZ.BackColor = System.Drawing.Color.Transparent;
            this.metroTrackBarIconSizeInfected_DAYZ.Location = new System.Drawing.Point(0, 262);
            this.metroTrackBarIconSizeInfected_DAYZ.Maximum = 200;
            this.metroTrackBarIconSizeInfected_DAYZ.Minimum = 10;
            this.metroTrackBarIconSizeInfected_DAYZ.Name = "metroTrackBarIconSizeInfected_DAYZ";
            this.metroTrackBarIconSizeInfected_DAYZ.Size = new System.Drawing.Size(247, 23);
            this.metroTrackBarIconSizeInfected_DAYZ.TabIndex = 27;
            this.metroTrackBarIconSizeInfected_DAYZ.Text = "metroTrackBar1";
            this.metroTrackBarIconSizeInfected_DAYZ.Scroll += new System.Windows.Forms.ScrollEventHandler(this.metroTrackBarIconSizeInfected_Scroll);
            
            
            
            this.metroLabelIconSizeInfected_DAYZ.AutoSize = true;
            this.metroLabelIconSizeInfected_DAYZ.Location = new System.Drawing.Point(0, 240);
            this.metroLabelIconSizeInfected_DAYZ.Name = "metroLabelIconSizeInfected_DAYZ";
            this.metroLabelIconSizeInfected_DAYZ.Size = new System.Drawing.Size(110, 19);
            this.metroLabelIconSizeInfected_DAYZ.TabIndex = 26;
            this.metroLabelIconSizeInfected_DAYZ.Text = "Icon Size Infected";
            
            
            
            this.metroCheckBoxProximityAlert_DAYZ.AutoSize = true;
            this.metroCheckBoxProximityAlert_DAYZ.Location = new System.Drawing.Point(140, 43);
            this.metroCheckBoxProximityAlert_DAYZ.Name = "metroCheckBoxProximityAlert_DAYZ";
            this.metroCheckBoxProximityAlert_DAYZ.Size = new System.Drawing.Size(101, 15);
            this.metroCheckBoxProximityAlert_DAYZ.TabIndex = 6;
            this.metroCheckBoxProximityAlert_DAYZ.Text = "Proximity Alert";
            this.metroCheckBoxProximityAlert_DAYZ.UseSelectable = true;
            this.metroCheckBoxProximityAlert_DAYZ.CheckedChanged += new System.EventHandler(this.metroCheckBoxProximityAlert_CheckedChanged);
            
            
            
            this.metroLabelWriteMemoryNoGrass_DAYZ.BackColor = System.Drawing.Color.LightCoral;
            this.metroLabelWriteMemoryNoGrass_DAYZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroLabelWriteMemoryNoGrass_DAYZ.Location = new System.Drawing.Point(0, 242);
            this.metroLabelWriteMemoryNoGrass_DAYZ.Name = "metroLabelWriteMemoryNoGrass_DAYZ";
            this.metroLabelWriteMemoryNoGrass_DAYZ.Size = new System.Drawing.Size(114, 37);
            this.metroLabelWriteMemoryNoGrass_DAYZ.TabIndex = 4;
            this.metroLabelWriteMemoryNoGrass_DAYZ.Text = "No Grass";
            this.metroLabelWriteMemoryNoGrass_DAYZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroLabelWriteMemoryNoGrass_DAYZ.UseCustomBackColor = true;
            this.metroLabelWriteMemoryNoGrass_DAYZ.UseCustomForeColor = true;
            this.metroLabelWriteMemoryNoGrass_DAYZ.Click += new System.EventHandler(this.metroLabelWriteMemoryNoGrass_Click);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(308, 656);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "SettingsFormDAYZ";
            this.metroTabPageMain.ResumeLayout(false);
            this.metroTabPageMain.PerformLayout();
            this.metroTabPageLoot.ResumeLayout(false);
            this.metroTabPageLoot.PerformLayout();
            this.metroTabPageOther.ResumeLayout(false);
            this.metroTabPageOther.PerformLayout();
            this.metroTabPageDebug.ResumeLayout(false);
            this.metroTabPageDebug.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.metroTabPageMemoryWriting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroCheckBox metroCheckBoxMapNetworkBubble_DAYZ;
        public MetroFramework.Controls.MetroListView metroListViewEntityTypes_DAYZ;
        private System.Windows.Forms.ColumnHeader columnEntityChecked_DAYZ;
        private System.Windows.Forms.ColumnHeader columnEntityType_DAYZ;
        private MetroFramework.Controls.MetroButton metroButtonEntityDumpUnknown_DAYZ;
        protected internal MetroFramework.Controls.MetroTextBox metroTextBoxIconSizeInfected_DAYZ;
        protected internal MetroFramework.Controls.MetroTrackBar metroTrackBarIconSizeInfected_DAYZ;
        protected internal MetroFramework.Controls.MetroLabel metroLabelIconSizeInfected_DAYZ;
        private MetroFramework.Controls.MetroCheckBox metroCheckBoxProximityAlert_DAYZ;
        private MetroFramework.Controls.MetroLabel metroLabelWriteMemoryNoGrass_DAYZ;

    }
}
