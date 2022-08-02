using System;

namespace NormandyNET.Modules.ARMA.UI
{
    partial class SettingsFormARMA
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
            this.metroCheckBoxMapNetworkBubble_ARMA = new MetroFramework.Controls.MetroCheckBox();
            this.metroListViewEntityTypes_ARMA = new MetroFramework.Controls.MetroListView();
            this.columnEntityType_ARMA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEntityChecked_ARMA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroButtonEntityDumpUnknown_ARMA = new MetroFramework.Controls.MetroButton();
            this.metroTextBoxIconSizeInfected_ARMA = new MetroFramework.Controls.MetroTextBox();
            this.metroTrackBarIconSizeInfected_ARMA = new MetroFramework.Controls.MetroTrackBar();
            this.metroLabelIconSizeInfected_ARMA = new MetroFramework.Controls.MetroLabel();
            this.metroCheckBoxProximityAlert_ARMA = new MetroFramework.Controls.MetroCheckBox();
            this.metroLabelWriteMemoryTESTSHIT_ARMA = new MetroFramework.Controls.MetroLabel();
            this.metroCheckBoxVehiclePassengers_ARMA = new MetroFramework.Controls.MetroCheckBox();
            this.metroLabelVehicles_ARMA = new MetroFramework.Controls.MetroLabel();
            this.metroCheckBoxVehiclePassengersDetailed_ARMA = new MetroFramework.Controls.MetroCheckBox();
            this.metroTabPageMain.SuspendLayout();
            this.metroTabPageLoot.SuspendLayout();
            this.metroTabPageOther.SuspendLayout();
            this.metroTabPageDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.metroTabPageMemoryWriting.SuspendLayout();
            this.SuspendLayout();
            
            
            
            this.metroTabPageMain.Controls.Add(this.metroLabelVehicles_ARMA);
            this.metroTabPageMain.Controls.Add(this.metroListViewEntityTypes_ARMA);
            this.metroTabPageMain.Controls.Add(this.metroCheckBoxVehiclePassengersDetailed_ARMA);
            this.metroTabPageMain.Controls.Add(this.metroCheckBoxVehiclePassengers_ARMA);
            this.metroTabPageMain.Controls.Add(this.metroCheckBoxProximityAlert_ARMA);
            this.metroTabPageMain.Controls.Add(this.metroCheckBoxMapNetworkBubble_ARMA);
            this.metroTabPageMain.Controls.SetChildIndex(this.metroCheckBoxMapNetworkBubble_ARMA, 0);
            this.metroTabPageMain.Controls.SetChildIndex(this.metroCheckBoxProximityAlert_ARMA, 0);
            this.metroTabPageMain.Controls.SetChildIndex(this.metroCheckBoxVehiclePassengers_ARMA, 0);
            this.metroTabPageMain.Controls.SetChildIndex(this.metroCheckBoxVehiclePassengersDetailed_ARMA, 0);
            this.metroTabPageMain.Controls.SetChildIndex(this.metroListViewEntityTypes_ARMA, 0);
            
            
            
            this.metroTabPageOther.Controls.Add(this.metroTextBoxIconSizeInfected_ARMA);
            this.metroTabPageOther.Controls.Add(this.metroTrackBarIconSizeInfected_ARMA);
            this.metroTabPageOther.Controls.Add(this.metroLabelIconSizeInfected_ARMA);
            this.metroTabPageOther.Controls.SetChildIndex(this.metroLabelIconSizeInfected_ARMA, 0);
            this.metroTabPageOther.Controls.SetChildIndex(this.metroTrackBarIconSizeInfected_ARMA, 0);
            this.metroTabPageOther.Controls.SetChildIndex(this.metroTextBoxIconSizeInfected_ARMA, 0);
            
            this.metroTabPageDebug.Controls.Add(this.metroButtonEntityDumpUnknown_ARMA);
            
            
            this.metroTabPageMemoryWriting.Controls.Add(this.metroLabelWriteMemoryTESTSHIT_ARMA);
            this.metroTabPageMemoryWriting.Controls.SetChildIndex(this.metroLabelWriteMemoryTESTSHIT_ARMA, 0);
            
            
            
            this.metroTabPageDebug.Controls.Add(this.metroButtonEntityDumpUnknown_ARMA);
            this.metroTabPageDebug.Controls.SetChildIndex(this.metroButtonEntityDumpUnknown_ARMA, 0);
            
            
            
            this.metroCheckBoxMapNetworkBubble_ARMA.AutoSize = true;
            this.metroCheckBoxMapNetworkBubble_ARMA.Location = new System.Drawing.Point(140, 22);
            this.metroCheckBoxMapNetworkBubble_ARMA.Name = "metroCheckBoxMapNetworkBubble_ARMA";
            this.metroCheckBoxMapNetworkBubble_ARMA.Size = new System.Drawing.Size(120, 15);
            this.metroCheckBoxMapNetworkBubble_ARMA.TabIndex = 6;
            this.metroCheckBoxMapNetworkBubble_ARMA.Text = "Net Bubble Circles";
            this.metroCheckBoxMapNetworkBubble_ARMA.UseSelectable = true;
            this.metroCheckBoxMapNetworkBubble_ARMA.CheckedChanged += new System.EventHandler(this.metroCheckBoxMapNetworkBubble_CheckedChanged);
            
            
            
            this.metroListViewEntityTypes_ARMA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroListViewEntityTypes_ARMA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroListViewEntityTypes_ARMA.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnEntityType_ARMA,
            this.columnEntityChecked_ARMA});
            this.metroListViewEntityTypes_ARMA.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.metroListViewEntityTypes_ARMA.FullRowSelect = true;
            this.metroListViewEntityTypes_ARMA.LabelWrap = false;
            this.metroListViewEntityTypes_ARMA.Location = new System.Drawing.Point(0, 280);
            this.metroListViewEntityTypes_ARMA.Name = "metroListViewEntityTypes_ARMA";
            this.metroListViewEntityTypes_ARMA.OwnerDraw = true;
            this.metroListViewEntityTypes_ARMA.Size = new System.Drawing.Size(292, 255);
            this.metroListViewEntityTypes_ARMA.TabIndex = 7;
            this.metroListViewEntityTypes_ARMA.UseCompatibleStateImageBehavior = false;
            this.metroListViewEntityTypes_ARMA.UseSelectable = true;
            this.metroListViewEntityTypes_ARMA.View = System.Windows.Forms.View.Details;
            this.metroListViewEntityTypes_ARMA.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.metroListViewEntityTypes_DrawSubItem);
            this.metroListViewEntityTypes_ARMA.DoubleClick += new System.EventHandler(this.metroListViewEntityTypes_DoubleClick);
            this.metroListViewEntityTypes_ARMA.Layout += new System.Windows.Forms.LayoutEventHandler(this.metroListViewEntityTypes_Layout);
            
            
            
            this.columnEntityType_ARMA.Text = "Type";
            this.columnEntityType_ARMA.Width = 260;
            
            
            
            this.columnEntityChecked_ARMA.Text = "";
            this.columnEntityChecked_ARMA.Width = 30;
            
            
            
            this.metroButtonEntityDumpUnknown_ARMA.Location = new System.Drawing.Point(137, 266);
            this.metroButtonEntityDumpUnknown_ARMA.Name = "metroButtonEntityDumpUnknown_ARMA";
            this.metroButtonEntityDumpUnknown_ARMA.Size = new System.Drawing.Size(152, 23);
            this.metroButtonEntityDumpUnknown_ARMA.TabIndex = 31;
            this.metroButtonEntityDumpUnknown_ARMA.Text = "Dump Unknown Entities";
            this.metroButtonEntityDumpUnknown_ARMA.UseSelectable = true;
            this.metroButtonEntityDumpUnknown_ARMA.Click += new System.EventHandler(this.metroButtonEntityDumpUnknown_Click);
            
            
            
            this.metroTextBoxIconSizeInfected_ARMA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            
            
            
            this.metroTextBoxIconSizeInfected_ARMA.CustomButton.Image = null;
            this.metroTextBoxIconSizeInfected_ARMA.CustomButton.Location = new System.Drawing.Point(19, 1);
            this.metroTextBoxIconSizeInfected_ARMA.CustomButton.Name = "";
            this.metroTextBoxIconSizeInfected_ARMA.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBoxIconSizeInfected_ARMA.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxIconSizeInfected_ARMA.CustomButton.TabIndex = 1;
            this.metroTextBoxIconSizeInfected_ARMA.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxIconSizeInfected_ARMA.CustomButton.UseSelectable = true;
            this.metroTextBoxIconSizeInfected_ARMA.CustomButton.Visible = false;
            this.metroTextBoxIconSizeInfected_ARMA.Lines = new string[0];
            this.metroTextBoxIconSizeInfected_ARMA.Location = new System.Drawing.Point(251, 262);
            this.metroTextBoxIconSizeInfected_ARMA.MaxLength = 32767;
            this.metroTextBoxIconSizeInfected_ARMA.Name = "metroTextBoxIconSizeInfected_ARMA";
            this.metroTextBoxIconSizeInfected_ARMA.PasswordChar = '\0';
            this.metroTextBoxIconSizeInfected_ARMA.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxIconSizeInfected_ARMA.SelectedText = "";
            this.metroTextBoxIconSizeInfected_ARMA.SelectionLength = 0;
            this.metroTextBoxIconSizeInfected_ARMA.SelectionStart = 0;
            this.metroTextBoxIconSizeInfected_ARMA.ShortcutsEnabled = true;
            this.metroTextBoxIconSizeInfected_ARMA.Size = new System.Drawing.Size(41, 23);
            this.metroTextBoxIconSizeInfected_ARMA.TabIndex = 28;
            this.metroTextBoxIconSizeInfected_ARMA.UseSelectable = true;
            this.metroTextBoxIconSizeInfected_ARMA.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxIconSizeInfected_ARMA.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            
            
            
            this.metroTrackBarIconSizeInfected_ARMA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroTrackBarIconSizeInfected_ARMA.BackColor = System.Drawing.Color.Transparent;
            this.metroTrackBarIconSizeInfected_ARMA.Location = new System.Drawing.Point(0, 262);
            this.metroTrackBarIconSizeInfected_ARMA.Maximum = 200;
            this.metroTrackBarIconSizeInfected_ARMA.Minimum = 10;
            this.metroTrackBarIconSizeInfected_ARMA.Name = "metroTrackBarIconSizeInfected_ARMA";
            this.metroTrackBarIconSizeInfected_ARMA.Size = new System.Drawing.Size(247, 23);
            this.metroTrackBarIconSizeInfected_ARMA.TabIndex = 27;
            this.metroTrackBarIconSizeInfected_ARMA.Text = "metroTrackBar1";
            this.metroTrackBarIconSizeInfected_ARMA.Scroll += new System.Windows.Forms.ScrollEventHandler(this.metroTrackBarIconSizeInfected_Scroll);
            
            
            
            this.metroLabelIconSizeInfected_ARMA.AutoSize = true;
            this.metroLabelIconSizeInfected_ARMA.Location = new System.Drawing.Point(0, 240);
            this.metroLabelIconSizeInfected_ARMA.Name = "metroLabelIconSizeInfected_ARMA";
            this.metroLabelIconSizeInfected_ARMA.Size = new System.Drawing.Size(110, 19);
            this.metroLabelIconSizeInfected_ARMA.TabIndex = 26;
            this.metroLabelIconSizeInfected_ARMA.Text = "Icon Size Infected";
            
            
            
            this.metroCheckBoxProximityAlert_ARMA.AutoSize = true;
            this.metroCheckBoxProximityAlert_ARMA.Location = new System.Drawing.Point(140, 43);
            this.metroCheckBoxProximityAlert_ARMA.Name = "metroCheckBoxProximityAlert_ARMA";
            this.metroCheckBoxProximityAlert_ARMA.Size = new System.Drawing.Size(101, 15);
            this.metroCheckBoxProximityAlert_ARMA.TabIndex = 6;
            this.metroCheckBoxProximityAlert_ARMA.Text = "Proximity Alert";
            this.metroCheckBoxProximityAlert_ARMA.UseSelectable = true;
            this.metroCheckBoxProximityAlert_ARMA.CheckedChanged += new System.EventHandler(this.metroCheckBoxProximityAlert_CheckedChanged);
            
            
            
            this.metroLabelWriteMemoryTESTSHIT_ARMA.BackColor = System.Drawing.Color.LightCoral;
            this.metroLabelWriteMemoryTESTSHIT_ARMA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroLabelWriteMemoryTESTSHIT_ARMA.Location = new System.Drawing.Point(0, 242);
            this.metroLabelWriteMemoryTESTSHIT_ARMA.Name = "metroLabelWriteMemoryTESTSHIT_ARMA";
            this.metroLabelWriteMemoryTESTSHIT_ARMA.Size = new System.Drawing.Size(114, 37);
            this.metroLabelWriteMemoryTESTSHIT_ARMA.TabIndex = 4;
            this.metroLabelWriteMemoryTESTSHIT_ARMA.Text = "TEST SHIT";
            this.metroLabelWriteMemoryTESTSHIT_ARMA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroLabelWriteMemoryTESTSHIT_ARMA.UseCustomBackColor = true;
            this.metroLabelWriteMemoryTESTSHIT_ARMA.UseCustomForeColor = true;
            this.metroLabelWriteMemoryTESTSHIT_ARMA.Visible = false;
            this.metroLabelWriteMemoryTESTSHIT_ARMA.Click += new System.EventHandler(this.metroLabelWriteMemoryTESTSHIT_Click);
            
            
            
            this.metroCheckBoxVehiclePassengers_ARMA.AutoSize = true;
            this.metroCheckBoxVehiclePassengers_ARMA.Location = new System.Drawing.Point(140, 106);
            this.metroCheckBoxVehiclePassengers_ARMA.Name = "metroCheckBoxVehiclePassengers_ARMA";
            this.metroCheckBoxVehiclePassengers_ARMA.Size = new System.Drawing.Size(81, 15);
            this.metroCheckBoxVehiclePassengers_ARMA.TabIndex = 6;
            this.metroCheckBoxVehiclePassengers_ARMA.Text = "Passengers";
            this.metroCheckBoxVehiclePassengers_ARMA.UseSelectable = true;
            this.metroCheckBoxVehiclePassengers_ARMA.CheckedChanged += new System.EventHandler(this.metroCheckBoxVehiclePassengers_ARMA_CheckedChanged);
            
            
            
            this.metroLabelVehicles_ARMA.Location = new System.Drawing.Point(140, 80);
            this.metroLabelVehicles_ARMA.Name = "metroLabelVehicles";
            this.metroLabelVehicles_ARMA.Size = new System.Drawing.Size(152, 23);
            this.metroLabelVehicles_ARMA.TabIndex = 8;
            this.metroLabelVehicles_ARMA.Text = "Vehicle";
            
            
            
            this.metroCheckBoxVehiclePassengersDetailed_ARMA.AutoSize = true;
            this.metroCheckBoxVehiclePassengersDetailed_ARMA.Location = new System.Drawing.Point(140, 127);
            this.metroCheckBoxVehiclePassengersDetailed_ARMA.Name = "metroCheckBoxVehiclePassengersDetailed_ARMA";
            this.metroCheckBoxVehiclePassengersDetailed_ARMA.Size = new System.Drawing.Size(77, 15);
            this.metroCheckBoxVehiclePassengersDetailed_ARMA.TabIndex = 6;
            this.metroCheckBoxVehiclePassengersDetailed_ARMA.Text = " - Detailed";
            this.metroCheckBoxVehiclePassengersDetailed_ARMA.UseSelectable = true;
            this.metroCheckBoxVehiclePassengersDetailed_ARMA.CheckedChanged += new System.EventHandler(this.metroCheckBoxVehiclePassengersDetailed_ARMA_CheckedChanged);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(308, 656);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "SettingsFormARMA";
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

        private MetroFramework.Controls.MetroCheckBox metroCheckBoxMapNetworkBubble_ARMA;
        public MetroFramework.Controls.MetroListView metroListViewEntityTypes_ARMA;
        private System.Windows.Forms.ColumnHeader columnEntityChecked_ARMA;
        private System.Windows.Forms.ColumnHeader columnEntityType_ARMA;
        private MetroFramework.Controls.MetroButton metroButtonEntityDumpUnknown_ARMA;
        protected internal MetroFramework.Controls.MetroTextBox metroTextBoxIconSizeInfected_ARMA;
        protected internal MetroFramework.Controls.MetroTrackBar metroTrackBarIconSizeInfected_ARMA;
        protected internal MetroFramework.Controls.MetroLabel metroLabelIconSizeInfected_ARMA;
        private MetroFramework.Controls.MetroCheckBox metroCheckBoxProximityAlert_ARMA;
        private MetroFramework.Controls.MetroLabel metroLabelWriteMemoryTESTSHIT_ARMA;
        private MetroFramework.Controls.MetroCheckBox metroCheckBoxVehiclePassengers_ARMA;
        private MetroFramework.Controls.MetroLabel metroLabelVehicles_ARMA;
        private MetroFramework.Controls.MetroCheckBox metroCheckBoxVehiclePassengersDetailed_ARMA;
    }
}
