namespace NormandyNET.UI
{
    partial class RadarForm
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
            this.tabPageLoot = new System.Windows.Forms.TabPage();
            this.tabPageOther = new System.Windows.Forms.TabPage();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroStyleExtender = new MetroFramework.Components.MetroStyleExtender(this.components);
            this.metroPanelOpenGL = new MetroFramework.Controls.MetroPanel();
            this.metroPanelShowHideOverlay = new MetroFramework.Controls.MetroPanel();
            this.metroButtonShowHideOverlay = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonShowHideUI = new MetroFramework.Controls.MetroPanel();
            this.metroButtonButtonShowHideUI = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonDownloadUpdate = new MetroFramework.Controls.MetroPanel();
            this.metroButtonDownloadUpdate = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonStartStop = new MetroFramework.Controls.MetroPanel();
            this.metroButtonStartStop = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonShowLoot = new MetroFramework.Controls.MetroPanel();
            this.metroButtonShowLoot = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonSettings = new MetroFramework.Controls.MetroPanel();
            this.metroButtonSettings = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonZoomIn = new MetroFramework.Controls.MetroPanel();
            this.metroButtonMapZoomIn = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonMapZoomOut = new MetroFramework.Controls.MetroPanel();
            this.metroButtonMapZoomOut = new MetroFramework.Controls.MetroButton();
            this.metroPanelUpdateLoot = new MetroFramework.Controls.MetroPanel();
            this.metroButtonUpdateLoot = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonFullScreen = new MetroFramework.Controls.MetroPanel();
            this.metroButtonFullScreen = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonDrawText = new MetroFramework.Controls.MetroPanel();
            this.metroButtonMapDrawText = new MetroFramework.Controls.MetroButton();
            this.metroPanelButtonCenterMap = new MetroFramework.Controls.MetroPanel();
            this.metroButtonCenterMap = new MetroFramework.Controls.MetroButton();
            this.metroContextMenuMap = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.toolStripMenuItemPutMeHere = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFindLootHere = new System.Windows.Forms.ToolStripMenuItem();
            this.metroToolTipTrackBars = new MetroFramework.Components.MetroToolTip();
            this.metroToolTipCommon = new MetroFramework.Components.MetroToolTip();
            this.metroLabelVersion = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.metroPanelOpenGL.SuspendLayout();
            this.metroPanelShowHideOverlay.SuspendLayout();
            this.metroPanelButtonShowHideUI.SuspendLayout();
            this.metroPanelButtonDownloadUpdate.SuspendLayout();
            this.metroPanelButtonStartStop.SuspendLayout();
            this.metroPanelButtonShowLoot.SuspendLayout();
            this.metroPanelButtonSettings.SuspendLayout();
            this.metroPanelButtonZoomIn.SuspendLayout();
            this.metroPanelButtonMapZoomOut.SuspendLayout();
            this.metroPanelUpdateLoot.SuspendLayout();
            this.metroPanelButtonFullScreen.SuspendLayout();
            this.metroPanelButtonDrawText.SuspendLayout();
            this.metroPanelButtonCenterMap.SuspendLayout();
            this.metroContextMenuMap.SuspendLayout();
            this.SuspendLayout();
            
            
            
            this.tabPageLoot.Location = new System.Drawing.Point(4, 38);
            this.tabPageLoot.Name = "tabPageLoot";
            this.tabPageLoot.Size = new System.Drawing.Size(223, 220);
            this.tabPageLoot.TabIndex = 1;
            this.tabPageLoot.Text = "Loot";
            
            
            
            this.tabPageOther.Location = new System.Drawing.Point(4, 38);
            this.tabPageOther.Name = "tabPageOther";
            this.tabPageOther.Size = new System.Drawing.Size(223, 220);
            this.tabPageOther.TabIndex = 2;
            this.tabPageOther.Text = "Other";
            
            
            
            this.tabPageDebug.Location = new System.Drawing.Point(4, 38);
            this.tabPageDebug.Name = "tabPageDebug";
            this.tabPageDebug.Size = new System.Drawing.Size(223, 220);
            this.tabPageDebug.TabIndex = 3;
            this.tabPageDebug.Text = "Debug";
            
            
            
            this.metroStyleManager.Owner = this;
            
            
            
            this.metroPanelOpenGL.BackColor = System.Drawing.Color.DimGray;
            this.metroPanelOpenGL.Controls.Add(this.metroPanelShowHideOverlay);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonShowHideUI);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonDownloadUpdate);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonStartStop);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonShowLoot);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonSettings);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonZoomIn);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonMapZoomOut);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelUpdateLoot);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonFullScreen);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonDrawText);
            this.metroPanelOpenGL.Controls.Add(this.metroPanelButtonCenterMap);
            this.metroPanelOpenGL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanelOpenGL.HorizontalScrollbarBarColor = true;
            this.metroPanelOpenGL.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelOpenGL.HorizontalScrollbarSize = 10;
            this.metroPanelOpenGL.Location = new System.Drawing.Point(4, 30);
            this.metroPanelOpenGL.Name = "metroPanelOpenGL";
            this.metroPanelOpenGL.Size = new System.Drawing.Size(592, 566);
            this.metroPanelOpenGL.TabIndex = 35;
            this.metroPanelOpenGL.UseCustomBackColor = true;
            this.metroPanelOpenGL.VerticalScrollbarBarColor = true;
            this.metroPanelOpenGL.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelOpenGL.VerticalScrollbarSize = 10;
            
            
            
            this.metroPanelShowHideOverlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanelShowHideOverlay.Controls.Add(this.metroButtonShowHideOverlay);
            this.metroPanelShowHideOverlay.HorizontalScrollbarBarColor = true;
            this.metroPanelShowHideOverlay.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelShowHideOverlay.HorizontalScrollbarSize = 10;
            this.metroPanelShowHideOverlay.Location = new System.Drawing.Point(501, 6);
            this.metroPanelShowHideOverlay.Name = "metroPanelShowHideOverlay";
            this.metroPanelShowHideOverlay.Size = new System.Drawing.Size(40, 40);
            this.metroPanelShowHideOverlay.TabIndex = 41;
            this.metroPanelShowHideOverlay.VerticalScrollbarBarColor = true;
            this.metroPanelShowHideOverlay.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelShowHideOverlay.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonShowHideOverlay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonShowHideOverlay.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonShowHideOverlay.BackgroundImage = global::NormandyNET.Properties.Resources.icon_Overlay;
            this.metroButtonShowHideOverlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonShowHideOverlay.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonShowHideOverlay.Location = new System.Drawing.Point(0, 0);
            this.metroButtonShowHideOverlay.Name = "metroButtonShowHideOverlay";
            this.metroButtonShowHideOverlay.Size = new System.Drawing.Size(40, 40);
            this.metroButtonShowHideOverlay.TabIndex = 35;
            this.metroToolTipCommon.SetToolTip(this.metroButtonShowHideOverlay, "ESP [Hide/Show]");
            this.metroButtonShowHideOverlay.UseSelectable = true;
            this.metroButtonShowHideOverlay.Click += new System.EventHandler(this.metroButtonShowHideOverlay_Click);
            
            
            
            this.metroPanelButtonShowHideUI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanelButtonShowHideUI.Controls.Add(this.metroButtonButtonShowHideUI);
            this.metroPanelButtonShowHideUI.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonShowHideUI.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonShowHideUI.HorizontalScrollbarSize = 10;
            this.metroPanelButtonShowHideUI.Location = new System.Drawing.Point(547, 6);
            this.metroPanelButtonShowHideUI.Name = "metroPanelButtonShowHideUI";
            this.metroPanelButtonShowHideUI.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonShowHideUI.TabIndex = 41;
            this.metroPanelButtonShowHideUI.VerticalScrollbarBarColor = true;
            this.metroPanelButtonShowHideUI.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonShowHideUI.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonButtonShowHideUI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonButtonShowHideUI.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonButtonShowHideUI.BackgroundImage = global::NormandyNET.Properties.Resources.icon_UI_Show;
            this.metroButtonButtonShowHideUI.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonButtonShowHideUI.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonButtonShowHideUI.Location = new System.Drawing.Point(0, 0);
            this.metroButtonButtonShowHideUI.Name = "metroButtonButtonShowHideUI";
            this.metroButtonButtonShowHideUI.Size = new System.Drawing.Size(40, 40);
            this.metroButtonButtonShowHideUI.TabIndex = 35;
            this.metroToolTipCommon.SetToolTip(this.metroButtonButtonShowHideUI, "Icons [Hide / Show]");
            this.metroButtonButtonShowHideUI.UseSelectable = true;
            this.metroButtonButtonShowHideUI.Click += new System.EventHandler(this.metroButtonButtonShowHideUI_Click);
            
            
            
            this.metroPanelButtonDownloadUpdate.Controls.Add(this.metroButtonDownloadUpdate);
            this.metroPanelButtonDownloadUpdate.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonDownloadUpdate.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonDownloadUpdate.HorizontalScrollbarSize = 10;
            this.metroPanelButtonDownloadUpdate.Location = new System.Drawing.Point(98, 6);
            this.metroPanelButtonDownloadUpdate.Name = "metroPanelButtonDownloadUpdate";
            this.metroPanelButtonDownloadUpdate.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonDownloadUpdate.TabIndex = 47;
            this.metroPanelButtonDownloadUpdate.VerticalScrollbarBarColor = true;
            this.metroPanelButtonDownloadUpdate.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonDownloadUpdate.VerticalScrollbarSize = 10;
            this.metroPanelButtonDownloadUpdate.Visible = false;
            
            
            
            this.metroButtonDownloadUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonDownloadUpdate.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonDownloadUpdate.BackgroundImage = global::NormandyNET.Properties.Resources.icon_Download;
            this.metroButtonDownloadUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonDownloadUpdate.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonDownloadUpdate.Location = new System.Drawing.Point(0, 0);
            this.metroButtonDownloadUpdate.Name = "metroButtonDownloadUpdate";
            this.metroButtonDownloadUpdate.Size = new System.Drawing.Size(40, 40);
            this.metroButtonDownloadUpdate.TabIndex = 35;
            this.metroButtonDownloadUpdate.UseSelectable = true;
            this.metroButtonDownloadUpdate.Click += new System.EventHandler(this.metroButtonDownloadUpdate_Click);
            
            
            
            this.metroPanelButtonStartStop.Controls.Add(this.metroButtonStartStop);
            this.metroPanelButtonStartStop.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonStartStop.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonStartStop.HorizontalScrollbarSize = 10;
            this.metroPanelButtonStartStop.Location = new System.Drawing.Point(52, 6);
            this.metroPanelButtonStartStop.Name = "metroPanelButtonStartStop";
            this.metroPanelButtonStartStop.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonStartStop.TabIndex = 47;
            this.metroPanelButtonStartStop.VerticalScrollbarBarColor = true;
            this.metroPanelButtonStartStop.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonStartStop.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonStartStop.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonStartStop.BackgroundImage = global::NormandyNET.Properties.Resources.icon_Play;
            this.metroButtonStartStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonStartStop.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonStartStop.Location = new System.Drawing.Point(0, 0);
            this.metroButtonStartStop.Name = "metroButtonStartStop";
            this.metroButtonStartStop.Size = new System.Drawing.Size(40, 40);
            this.metroButtonStartStop.TabIndex = 35;
            this.metroToolTipCommon.SetToolTip(this.metroButtonStartStop, "Start / Stop");
            this.metroButtonStartStop.UseSelectable = true;
            this.metroButtonStartStop.Click += new System.EventHandler(this.metroButtonStartStop_Click);
            
            
            
            this.metroPanelButtonShowLoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanelButtonShowLoot.Controls.Add(this.metroButtonShowLoot);
            this.metroPanelButtonShowLoot.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonShowLoot.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonShowLoot.HorizontalScrollbarSize = 10;
            this.metroPanelButtonShowLoot.Location = new System.Drawing.Point(547, 98);
            this.metroPanelButtonShowLoot.Name = "metroPanelButtonShowLoot";
            this.metroPanelButtonShowLoot.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonShowLoot.TabIndex = 46;
            this.metroPanelButtonShowLoot.VerticalScrollbarBarColor = true;
            this.metroPanelButtonShowLoot.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonShowLoot.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonShowLoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonShowLoot.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonShowLoot.BackgroundImage = global::NormandyNET.Properties.Resources.icon_BoxOn;
            this.metroButtonShowLoot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonShowLoot.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonShowLoot.DisplayFocus = true;
            this.metroButtonShowLoot.Location = new System.Drawing.Point(0, 0);
            this.metroButtonShowLoot.Name = "metroButtonShowLoot";
            this.metroButtonShowLoot.Size = new System.Drawing.Size(40, 40);
            this.metroButtonShowLoot.TabIndex = 35;
            this.metroToolTipCommon.SetToolTip(this.metroButtonShowLoot, "Show Loot [On / Off]");
            this.metroButtonShowLoot.UseSelectable = true;
            this.metroButtonShowLoot.Click += new System.EventHandler(this.metroButtonShowLoot_Click);
            this.metroButtonShowLoot.MouseEnter += new System.EventHandler(this.metroButtonShowLoot_MouseEnter);
            this.metroButtonShowLoot.MouseLeave += new System.EventHandler(this.metroButtonShowLoot_MouseLeave);
            
            
            
            this.metroPanelButtonSettings.Controls.Add(this.metroButtonSettings);
            this.metroPanelButtonSettings.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonSettings.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonSettings.HorizontalScrollbarSize = 10;
            this.metroPanelButtonSettings.Location = new System.Drawing.Point(6, 6);
            this.metroPanelButtonSettings.Name = "metroPanelButtonSettings";
            this.metroPanelButtonSettings.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonSettings.TabIndex = 45;
            this.metroPanelButtonSettings.VerticalScrollbarBarColor = true;
            this.metroPanelButtonSettings.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonSettings.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonSettings.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonSettings.BackgroundImage = global::NormandyNET.Properties.Resources.icon_SettingsShow;
            this.metroButtonSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonSettings.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonSettings.Location = new System.Drawing.Point(0, 0);
            this.metroButtonSettings.Name = "metroButtonSettings";
            this.metroButtonSettings.Size = new System.Drawing.Size(40, 40);
            this.metroButtonSettings.TabIndex = 35;
            this.metroToolTipCommon.SetToolTip(this.metroButtonSettings, "Settings Show [On / Off]");
            this.metroButtonSettings.UseSelectable = true;
            this.metroButtonSettings.Click += new System.EventHandler(this.metroButtonSettings_Click);
            
            
            
            this.metroPanelButtonZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanelButtonZoomIn.Controls.Add(this.metroButtonMapZoomIn);
            this.metroPanelButtonZoomIn.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonZoomIn.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonZoomIn.HorizontalScrollbarSize = 10;
            this.metroPanelButtonZoomIn.Location = new System.Drawing.Point(548, 473);
            this.metroPanelButtonZoomIn.Name = "metroPanelButtonZoomIn";
            this.metroPanelButtonZoomIn.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonZoomIn.TabIndex = 44;
            this.metroPanelButtonZoomIn.VerticalScrollbarBarColor = true;
            this.metroPanelButtonZoomIn.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonZoomIn.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonMapZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonMapZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonMapZoomIn.BackgroundImage = global::NormandyNET.Properties.Resources.icon_Plus;
            this.metroButtonMapZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonMapZoomIn.Location = new System.Drawing.Point(0, 0);
            this.metroButtonMapZoomIn.Name = "metroButtonMapZoomIn";
            this.metroButtonMapZoomIn.Size = new System.Drawing.Size(40, 40);
            this.metroButtonMapZoomIn.TabIndex = 39;
            this.metroButtonMapZoomIn.UseSelectable = true;
            this.metroButtonMapZoomIn.Click += new System.EventHandler(this.metroButtonMapZoomIn_Click);
            
            
            
            this.metroPanelButtonMapZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanelButtonMapZoomOut.Controls.Add(this.metroButtonMapZoomOut);
            this.metroPanelButtonMapZoomOut.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonMapZoomOut.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonMapZoomOut.HorizontalScrollbarSize = 10;
            this.metroPanelButtonMapZoomOut.Location = new System.Drawing.Point(548, 519);
            this.metroPanelButtonMapZoomOut.Name = "metroPanelButtonMapZoomOut";
            this.metroPanelButtonMapZoomOut.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonMapZoomOut.TabIndex = 43;
            this.metroPanelButtonMapZoomOut.VerticalScrollbarBarColor = true;
            this.metroPanelButtonMapZoomOut.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonMapZoomOut.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonMapZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonMapZoomOut.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonMapZoomOut.BackgroundImage = global::NormandyNET.Properties.Resources.icon_Minus;
            this.metroButtonMapZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonMapZoomOut.Location = new System.Drawing.Point(0, 0);
            this.metroButtonMapZoomOut.Name = "metroButtonMapZoomOut";
            this.metroButtonMapZoomOut.Size = new System.Drawing.Size(40, 40);
            this.metroButtonMapZoomOut.TabIndex = 38;
            this.metroButtonMapZoomOut.UseSelectable = true;
            this.metroButtonMapZoomOut.Click += new System.EventHandler(this.metroButtonMapZoomOut_Click);
            
            
            
            this.metroPanelUpdateLoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanelUpdateLoot.Controls.Add(this.metroButtonUpdateLoot);
            this.metroPanelUpdateLoot.HorizontalScrollbarBarColor = true;
            this.metroPanelUpdateLoot.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelUpdateLoot.HorizontalScrollbarSize = 10;
            this.metroPanelUpdateLoot.Location = new System.Drawing.Point(501, 98);
            this.metroPanelUpdateLoot.Name = "metroPanelUpdateLoot";
            this.metroPanelUpdateLoot.Size = new System.Drawing.Size(40, 40);
            this.metroPanelUpdateLoot.TabIndex = 42;
            this.metroPanelUpdateLoot.VerticalScrollbarBarColor = true;
            this.metroPanelUpdateLoot.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelUpdateLoot.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonUpdateLoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonUpdateLoot.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonUpdateLoot.BackgroundImage = global::NormandyNET.Properties.Resources.icon_Refresh;
            this.metroButtonUpdateLoot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonUpdateLoot.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonUpdateLoot.Location = new System.Drawing.Point(0, 0);
            this.metroButtonUpdateLoot.Name = "metroButtonUpdateLoot";
            this.metroButtonUpdateLoot.Size = new System.Drawing.Size(40, 40);
            this.metroButtonUpdateLoot.TabIndex = 36;
            this.metroToolTipCommon.SetToolTip(this.metroButtonUpdateLoot, "Refresh loot.");
            this.metroButtonUpdateLoot.UseSelectable = true;
            this.metroButtonUpdateLoot.Click += new System.EventHandler(this.metroButtonUpdateLoot_Click);
            this.metroButtonUpdateLoot.MouseEnter += new System.EventHandler(this.metroButtonUpdateLoot_MouseEnter);
            this.metroButtonUpdateLoot.MouseLeave += new System.EventHandler(this.metroButtonUpdateLoot_MouseLeave);
            
            
            
            this.metroPanelButtonFullScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanelButtonFullScreen.Controls.Add(this.metroButtonFullScreen);
            this.metroPanelButtonFullScreen.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonFullScreen.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonFullScreen.HorizontalScrollbarSize = 10;
            this.metroPanelButtonFullScreen.Location = new System.Drawing.Point(547, 190);
            this.metroPanelButtonFullScreen.Name = "metroPanelButtonFullScreen";
            this.metroPanelButtonFullScreen.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonFullScreen.TabIndex = 42;
            this.metroPanelButtonFullScreen.VerticalScrollbarBarColor = true;
            this.metroPanelButtonFullScreen.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonFullScreen.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonFullScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonFullScreen.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonFullScreen.BackgroundImage = global::NormandyNET.Properties.Resources.icon_Expand;
            this.metroButtonFullScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonFullScreen.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonFullScreen.Location = new System.Drawing.Point(0, 0);
            this.metroButtonFullScreen.Name = "metroButtonFullScreen";
            this.metroButtonFullScreen.Size = new System.Drawing.Size(40, 40);
            this.metroButtonFullScreen.TabIndex = 36;
            this.metroToolTipCommon.SetToolTip(this.metroButtonFullScreen, "Full Screen [On / Off]");
            this.metroButtonFullScreen.UseSelectable = true;
            this.metroButtonFullScreen.Click += new System.EventHandler(this.metroButtonFullScreen_Click);
            
            
            
            this.metroPanelButtonDrawText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanelButtonDrawText.Controls.Add(this.metroButtonMapDrawText);
            this.metroPanelButtonDrawText.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonDrawText.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonDrawText.HorizontalScrollbarSize = 10;
            this.metroPanelButtonDrawText.Location = new System.Drawing.Point(547, 144);
            this.metroPanelButtonDrawText.Name = "metroPanelButtonDrawText";
            this.metroPanelButtonDrawText.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonDrawText.TabIndex = 42;
            this.metroPanelButtonDrawText.VerticalScrollbarBarColor = true;
            this.metroPanelButtonDrawText.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonDrawText.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonMapDrawText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonMapDrawText.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonMapDrawText.BackgroundImage = global::NormandyNET.Properties.Resources.icon_TextOn;
            this.metroButtonMapDrawText.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonMapDrawText.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonMapDrawText.DisplayFocus = true;
            this.metroButtonMapDrawText.Location = new System.Drawing.Point(0, 0);
            this.metroButtonMapDrawText.Name = "metroButtonMapDrawText";
            this.metroButtonMapDrawText.Size = new System.Drawing.Size(40, 40);
            this.metroButtonMapDrawText.TabIndex = 36;
            this.metroToolTipCommon.SetToolTip(this.metroButtonMapDrawText, "Draw Text [On / Off]");
            this.metroButtonMapDrawText.UseSelectable = true;
            this.metroButtonMapDrawText.Click += new System.EventHandler(this.metroButtonMapDrawText_Click);
            
            
            
            this.metroPanelButtonCenterMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanelButtonCenterMap.Controls.Add(this.metroButtonCenterMap);
            this.metroPanelButtonCenterMap.HorizontalScrollbarBarColor = true;
            this.metroPanelButtonCenterMap.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonCenterMap.HorizontalScrollbarSize = 10;
            this.metroPanelButtonCenterMap.Location = new System.Drawing.Point(547, 52);
            this.metroPanelButtonCenterMap.Name = "metroPanelButtonCenterMap";
            this.metroPanelButtonCenterMap.Size = new System.Drawing.Size(40, 40);
            this.metroPanelButtonCenterMap.TabIndex = 41;
            this.metroPanelButtonCenterMap.VerticalScrollbarBarColor = true;
            this.metroPanelButtonCenterMap.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelButtonCenterMap.VerticalScrollbarSize = 10;
            
            
            
            this.metroButtonCenterMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButtonCenterMap.BackColor = System.Drawing.Color.Transparent;
            this.metroButtonCenterMap.BackgroundImage = global::NormandyNET.Properties.Resources.icon_CenterOn;
            this.metroButtonCenterMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.metroButtonCenterMap.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroButtonCenterMap.DisplayFocus = true;
            this.metroButtonCenterMap.Location = new System.Drawing.Point(0, 0);
            this.metroButtonCenterMap.Name = "metroButtonCenterMap";
            this.metroButtonCenterMap.Size = new System.Drawing.Size(40, 40);
            this.metroButtonCenterMap.TabIndex = 36;
            this.metroToolTipCommon.SetToolTip(this.metroButtonCenterMap, "Follow Player [On / Off]");
            this.metroButtonCenterMap.UseSelectable = true;
            this.metroButtonCenterMap.Click += new System.EventHandler(this.metroButtonCenterMap_Click);
            
            
            
            
            
            
            this.metroContextMenuMap.Name = "metroContextMenuMap";
            this.metroContextMenuMap.Size = new System.Drawing.Size(165, 48);
            
            
            
            this.toolStripMenuItemPutMeHere.Enabled = false;
            this.toolStripMenuItemPutMeHere.Name = "toolStripMenuItemPutMeHere";
            this.toolStripMenuItemPutMeHere.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItemPutMeHere.Text = "Put my icon here";
            this.toolStripMenuItemPutMeHere.ToolTipText = "If you drift, use this one.";
            this.toolStripMenuItemPutMeHere.Visible = false;
            this.toolStripMenuItemPutMeHere.Click += new System.EventHandler(this.toolStripMenuItemPutMeHere_Click);
            
            
            
            this.toolStripMenuItemFindLootHere.Name = "toolStripMenuItemFindLootHere";
            this.toolStripMenuItemFindLootHere.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItemFindLootHere.Text = "Find Loot Here";
            this.toolStripMenuItemFindLootHere.ToolTipText = "Look for loot around this spot.\nRight click to disable.";
            this.toolStripMenuItemFindLootHere.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStripMenuItemFindLootHere_MouseDown);
            
            
            
            this.metroToolTipTrackBars.AutomaticDelay = 10000;
            this.metroToolTipTrackBars.AutoPopDelay = 10000;
            this.metroToolTipTrackBars.InitialDelay = 10000;
            this.metroToolTipTrackBars.ReshowDelay = 10000;
            this.metroToolTipTrackBars.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTipTrackBars.StyleManager = null;
            this.metroToolTipTrackBars.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroToolTipTrackBars.UseAnimation = false;
            this.metroToolTipTrackBars.UseFading = false;
            
            
            
            this.metroToolTipCommon.Style = MetroFramework.MetroColorStyle.Default;
            this.metroToolTipCommon.StyleManager = null;
            this.metroToolTipCommon.Theme = MetroFramework.MetroThemeStyle.Default;
            
            
            
            this.metroLabelVersion.Location = new System.Drawing.Point(0, 7);
            this.metroLabelVersion.Name = "metroLabelVersion";
            this.metroLabelVersion.Size = new System.Drawing.Size(200, 23);
            this.metroLabelVersion.TabIndex = 36;
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(this.metroLabelVersion);
            this.Controls.Add(this.metroPanelOpenGL);
            this.DisplayHeader = false;
            this.KeyPreview = true;
            this.Name = "RadarForm";
            this.Padding = new System.Windows.Forms.Padding(4, 30, 4, 4);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.StyleManager = this.metroStyleManager;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.metroPanelOpenGL.ResumeLayout(false);
            this.metroPanelShowHideOverlay.ResumeLayout(false);
            this.metroPanelButtonShowHideUI.ResumeLayout(false);
            this.metroPanelButtonDownloadUpdate.ResumeLayout(false);
            this.metroPanelButtonStartStop.ResumeLayout(false);
            this.metroPanelButtonShowLoot.ResumeLayout(false);
            this.metroPanelButtonSettings.ResumeLayout(false);
            this.metroPanelButtonZoomIn.ResumeLayout(false);
            this.metroPanelButtonMapZoomOut.ResumeLayout(false);
            this.metroPanelUpdateLoot.ResumeLayout(false);
            this.metroPanelButtonFullScreen.ResumeLayout(false);
            this.metroPanelButtonDrawText.ResumeLayout(false);
            this.metroPanelButtonCenterMap.ResumeLayout(false);
            this.metroContextMenuMap.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPageLoot;
        private System.Windows.Forms.TabPage tabPageOther;
        private System.Windows.Forms.TabPage tabPageDebug;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Components.MetroStyleExtender metroStyleExtender;
        private MetroFramework.Controls.MetroPanel metroPanelOpenGL;
        private MetroFramework.Controls.MetroPanel metroPanelButtonCenterMap;
        private MetroFramework.Controls.MetroPanel metroPanelButtonDrawText;
        private MetroFramework.Controls.MetroButton metroButtonMapDrawText;
        private MetroFramework.Controls.MetroButton metroButtonCenterMap;
        private MetroFramework.Controls.MetroPanel metroPanelButtonZoomIn;
        private MetroFramework.Controls.MetroButton metroButtonMapZoomIn;
        private MetroFramework.Controls.MetroPanel metroPanelButtonMapZoomOut;
        private MetroFramework.Controls.MetroButton metroButtonMapZoomOut;
        private MetroFramework.Controls.MetroPanel metroPanelButtonFullScreen;
        private MetroFramework.Controls.MetroButton metroButtonFullScreen;
        private MetroFramework.Controls.MetroPanel metroPanelButtonStartStop;
        private MetroFramework.Controls.MetroButton metroButtonStartStop;
        private MetroFramework.Controls.MetroPanel metroPanelButtonShowLoot;
        private MetroFramework.Controls.MetroButton metroButtonShowLoot;
        private MetroFramework.Controls.MetroPanel metroPanelButtonSettings;
        private MetroFramework.Controls.MetroPanel metroPanelButtonShowHideUI;
        private MetroFramework.Controls.MetroButton metroButtonButtonShowHideUI;
        private MetroFramework.Controls.MetroButton metroButtonSettings;
        internal MetroFramework.Controls.MetroContextMenu metroContextMenuMap;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPutMeHere;
        private MetroFramework.Components.MetroToolTip metroToolTipTrackBars;
        private MetroFramework.Components.MetroToolTip metroToolTipCommon;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFindLootHere;
        private MetroFramework.Controls.MetroPanel metroPanelShowHideOverlay;
        private MetroFramework.Controls.MetroButton metroButtonShowHideOverlay;
        private MetroFramework.Controls.MetroPanel metroPanelUpdateLoot;
        private MetroFramework.Controls.MetroButton metroButtonUpdateLoot;
        private MetroFramework.Controls.MetroPanel metroPanelButtonDownloadUpdate;
        private MetroFramework.Controls.MetroButton metroButtonDownloadUpdate;
        private MetroFramework.Controls.MetroLabel metroLabelVersion;
    }
}