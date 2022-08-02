namespace NormandyNET
{
    partial class MapSelectorDialog
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
            this.metroListViewMapsList = new MetroFramework.Controls.MetroListView();
            this.columnHeaderMap = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroTileGameProcessID = new MetroFramework.Controls.MetroTile();
            this.metroTextBoxGameProcessID = new MetroFramework.Controls.MetroTextBox();
            this.metroLabelGameProcessID = new MetroFramework.Controls.MetroLabel();
            this.metroTileServerAddressValid = new MetroFramework.Controls.MetroTile();
            this.metroTextBoxServerAddress = new MetroFramework.Controls.MetroTextBox();
            this.metroLabelServerAddress = new MetroFramework.Controls.MetroLabel();
            this.metroButtonGo = new MetroFramework.Controls.MetroButton();
            this.metroLabelServerPort = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxServerPort = new MetroFramework.Controls.MetroTextBox();
            this.metroTilelServerPortValid = new MetroFramework.Controls.MetroTile();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            
            
            
            this.metroListViewMapsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.metroListViewMapsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderMap});
            this.metroListViewMapsList.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.metroListViewMapsList.FullRowSelect = true;
            this.metroListViewMapsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.metroListViewMapsList.Location = new System.Drawing.Point(20, 158);
            this.metroListViewMapsList.Name = "metroListViewMapsList";
            this.metroListViewMapsList.OwnerDraw = true;
            this.metroListViewMapsList.Size = new System.Drawing.Size(227, 0);
            this.metroListViewMapsList.TabIndex = 96;
            this.metroListViewMapsList.UseCompatibleStateImageBehavior = false;
            this.metroListViewMapsList.UseSelectable = true;
            this.metroListViewMapsList.View = System.Windows.Forms.View.Details;
            this.metroListViewMapsList.Click += new System.EventHandler(this.metroListViewMapsList_Click);
            this.metroListViewMapsList.DoubleClick += new System.EventHandler(this.metroListViewMapsList_DoubleClick);
            
            
            
            this.columnHeaderMap.Width = 223;
            
            
            
            this.metroStyleManager.Owner = this;
            
            
            
            this.metroTileGameProcessID.ActiveControl = null;
            this.metroTileGameProcessID.DisplayFocusBorder = false;
            this.metroTileGameProcessID.Location = new System.Drawing.Point(110, 129);
            this.metroTileGameProcessID.Name = "metroTileGameProcessID";
            this.metroTileGameProcessID.Size = new System.Drawing.Size(23, 23);
            this.metroTileGameProcessID.TabIndex = 114;
            this.metroTileGameProcessID.UseSelectable = true;
            
            
            
            
            
            
            this.metroTextBoxGameProcessID.CustomButton.Image = null;
            this.metroTextBoxGameProcessID.CustomButton.Location = new System.Drawing.Point(86, 1);
            this.metroTextBoxGameProcessID.CustomButton.Name = "";
            this.metroTextBoxGameProcessID.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBoxGameProcessID.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxGameProcessID.CustomButton.TabIndex = 1;
            this.metroTextBoxGameProcessID.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxGameProcessID.CustomButton.UseSelectable = true;
            this.metroTextBoxGameProcessID.CustomButton.Visible = false;
            this.metroTextBoxGameProcessID.Lines = new string[0];
            this.metroTextBoxGameProcessID.Location = new System.Drawing.Point(139, 129);
            this.metroTextBoxGameProcessID.MaxLength = 32767;
            this.metroTextBoxGameProcessID.Name = "metroTextBoxGameProcessID";
            this.metroTextBoxGameProcessID.PasswordChar = '\0';
            this.metroTextBoxGameProcessID.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxGameProcessID.SelectedText = "";
            this.metroTextBoxGameProcessID.SelectionLength = 0;
            this.metroTextBoxGameProcessID.SelectionStart = 0;
            this.metroTextBoxGameProcessID.ShortcutsEnabled = true;
            this.metroTextBoxGameProcessID.Size = new System.Drawing.Size(108, 23);
            this.metroTextBoxGameProcessID.TabIndex = 113;
            this.metroTextBoxGameProcessID.UseSelectable = true;
            this.metroTextBoxGameProcessID.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxGameProcessID.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBoxGameProcessID.TextChanged += new System.EventHandler(this.metroTextBoxGameProcessID_TextChanged);
            
            
            
            this.metroLabelGameProcessID.AutoSize = true;
            this.metroLabelGameProcessID.Location = new System.Drawing.Point(110, 107);
            this.metroLabelGameProcessID.Name = "metroLabelGameProcessID";
            this.metroLabelGameProcessID.Size = new System.Drawing.Size(108, 19);
            this.metroLabelGameProcessID.TabIndex = 112;
            this.metroLabelGameProcessID.Text = "Game Process ID";
            
            
            
            this.metroTileServerAddressValid.ActiveControl = null;
            this.metroTileServerAddressValid.DisplayFocusBorder = false;
            this.metroTileServerAddressValid.Location = new System.Drawing.Point(110, 33);
            this.metroTileServerAddressValid.Name = "metroTileServerAddressValid";
            this.metroTileServerAddressValid.Size = new System.Drawing.Size(23, 23);
            this.metroTileServerAddressValid.TabIndex = 111;
            this.metroTileServerAddressValid.UseSelectable = true;
            
            
            
            
            
            
            this.metroTextBoxServerAddress.CustomButton.Image = null;
            this.metroTextBoxServerAddress.CustomButton.Location = new System.Drawing.Point(86, 1);
            this.metroTextBoxServerAddress.CustomButton.Name = "";
            this.metroTextBoxServerAddress.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBoxServerAddress.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxServerAddress.CustomButton.TabIndex = 1;
            this.metroTextBoxServerAddress.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxServerAddress.CustomButton.UseSelectable = true;
            this.metroTextBoxServerAddress.CustomButton.Visible = false;
            this.metroTextBoxServerAddress.Lines = new string[0];
            this.metroTextBoxServerAddress.Location = new System.Drawing.Point(139, 33);
            this.metroTextBoxServerAddress.MaxLength = 32767;
            this.metroTextBoxServerAddress.Name = "metroTextBoxServerAddress";
            this.metroTextBoxServerAddress.PasswordChar = '\0';
            this.metroTextBoxServerAddress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxServerAddress.SelectedText = "";
            this.metroTextBoxServerAddress.SelectionLength = 0;
            this.metroTextBoxServerAddress.SelectionStart = 0;
            this.metroTextBoxServerAddress.ShortcutsEnabled = true;
            this.metroTextBoxServerAddress.Size = new System.Drawing.Size(108, 23);
            this.metroTextBoxServerAddress.TabIndex = 110;
            this.metroTextBoxServerAddress.UseSelectable = true;
            this.metroTextBoxServerAddress.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxServerAddress.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBoxServerAddress.TextChanged += new System.EventHandler(this.metroTextBoxServerAddress_TextChanged);
            
            
            
            this.metroLabelServerAddress.AutoSize = true;
            this.metroLabelServerAddress.Location = new System.Drawing.Point(110, 11);
            this.metroLabelServerAddress.Name = "metroLabelServerAddress";
            this.metroLabelServerAddress.Size = new System.Drawing.Size(113, 19);
            this.metroLabelServerAddress.TabIndex = 109;
            this.metroLabelServerAddress.Text = "Server IP Address";
            
            
            
            this.metroButtonGo.Location = new System.Drawing.Point(23, 33);
            this.metroButtonGo.Name = "metroButtonGo";
            this.metroButtonGo.Size = new System.Drawing.Size(75, 75);
            this.metroButtonGo.TabIndex = 115;
            this.metroButtonGo.Text = "Go";
            this.metroButtonGo.UseSelectable = true;
            this.metroButtonGo.Click += new System.EventHandler(this.metroButtonGo_Click);
            
            
            
            this.metroLabelServerPort.AutoSize = true;
            this.metroLabelServerPort.Location = new System.Drawing.Point(110, 59);
            this.metroLabelServerPort.Name = "metroLabelServerPort";
            this.metroLabelServerPort.Size = new System.Drawing.Size(76, 19);
            this.metroLabelServerPort.TabIndex = 109;
            this.metroLabelServerPort.Text = "Server Port";
            
            
            
            
            
            
            this.metroTextBoxServerPort.CustomButton.Image = null;
            this.metroTextBoxServerPort.CustomButton.Location = new System.Drawing.Point(86, 1);
            this.metroTextBoxServerPort.CustomButton.Name = "";
            this.metroTextBoxServerPort.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBoxServerPort.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxServerPort.CustomButton.TabIndex = 1;
            this.metroTextBoxServerPort.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxServerPort.CustomButton.UseSelectable = true;
            this.metroTextBoxServerPort.CustomButton.Visible = false;
            this.metroTextBoxServerPort.Lines = new string[0];
            this.metroTextBoxServerPort.Location = new System.Drawing.Point(139, 81);
            this.metroTextBoxServerPort.MaxLength = 32767;
            this.metroTextBoxServerPort.Name = "metroTextBoxlServerPort";
            this.metroTextBoxServerPort.PasswordChar = '\0';
            this.metroTextBoxServerPort.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxServerPort.SelectedText = "";
            this.metroTextBoxServerPort.SelectionLength = 0;
            this.metroTextBoxServerPort.SelectionStart = 0;
            this.metroTextBoxServerPort.ShortcutsEnabled = true;
            this.metroTextBoxServerPort.Size = new System.Drawing.Size(108, 23);
            this.metroTextBoxServerPort.TabIndex = 110;
            this.metroTextBoxServerPort.UseSelectable = true;
            this.metroTextBoxServerPort.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxServerPort.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBoxServerPort.TextChanged += new System.EventHandler(this.metroTextBoxlServerPort_TextChanged);
            
            
            
            this.metroTilelServerPortValid.ActiveControl = null;
            this.metroTilelServerPortValid.DisplayFocusBorder = false;
            this.metroTilelServerPortValid.Location = new System.Drawing.Point(110, 81);
            this.metroTilelServerPortValid.Name = "metroTilelServerPortValid";
            this.metroTilelServerPortValid.Size = new System.Drawing.Size(23, 23);
            this.metroTilelServerPortValid.TabIndex = 111;
            this.metroTilelServerPortValid.UseSelectable = true;
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 166);
            this.Controls.Add(this.metroButtonGo);
            this.Controls.Add(this.metroTileGameProcessID);
            this.Controls.Add(this.metroTextBoxGameProcessID);
            this.Controls.Add(this.metroLabelGameProcessID);
            this.Controls.Add(this.metroTilelServerPortValid);
            this.Controls.Add(this.metroTextBoxServerPort);
            this.Controls.Add(this.metroTileServerAddressValid);
            this.Controls.Add(this.metroLabelServerPort);
            this.Controls.Add(this.metroTextBoxServerAddress);
            this.Controls.Add(this.metroLabelServerAddress);
            this.Controls.Add(this.metroListViewMapsList);
            this.DisplayHeader = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapSelectorDialog";
            this.Padding = new System.Windows.Forms.Padding(20, 30, 20, 20);
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.StyleManager = this.metroStyleManager;
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroListView metroListViewMapsList;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private System.Windows.Forms.ColumnHeader columnHeaderMap;
        private MetroFramework.Controls.MetroButton metroButtonGo;
        private MetroFramework.Controls.MetroTile metroTileGameProcessID;
        private MetroFramework.Controls.MetroTextBox metroTextBoxGameProcessID;
        private MetroFramework.Controls.MetroLabel metroLabelGameProcessID;
        private MetroFramework.Controls.MetroTile metroTileServerAddressValid;
        private MetroFramework.Controls.MetroTextBox metroTextBoxServerAddress;
        private MetroFramework.Controls.MetroLabel metroLabelServerAddress;
        private MetroFramework.Controls.MetroTile metroTilelServerPortValid;
        private MetroFramework.Controls.MetroTextBox metroTextBoxServerPort;
        private MetroFramework.Controls.MetroLabel metroLabelServerPort;
    }
}