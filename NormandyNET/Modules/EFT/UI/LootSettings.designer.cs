namespace NormandyNET.Modules.EFT.UI
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
            this.buttonIconColorsSave = new MetroFramework.Controls.MetroButton();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroListViewLootCategoryColors = new MetroFramework.Controls.MetroListView();
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroToolTipCommon = new MetroFramework.Components.MetroToolTip();
            this.metroLabelLootLiveAmountPerCycle = new MetroFramework.Controls.MetroLabel();
            this.metroCheckBoxLootForceShow = new MetroFramework.Controls.MetroCheckBox();
            this.metroToggleLootASQuest = new MetroFramework.Controls.MetroToggle();
            this.metroCheckBoxLootInContainersRead = new MetroFramework.Controls.MetroCheckBox();
            this.metroCheckBoxLootLive = new MetroFramework.Controls.MetroCheckBox();
            this.metroTextBoxLootLiveAmountPerCycle = new MetroFramework.Controls.MetroTextBox();
            this.metroCheckBoxLootShowPricesAlways = new MetroFramework.Controls.MetroCheckBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroToggleLootASMedium = new MetroFramework.Controls.MetroToggle();
            this.metroToggleLootASSuper = new MetroFramework.Controls.MetroToggle();
            this.metroToggleLootASUltra = new MetroFramework.Controls.MetroToggle();
            this.metroToggleLootASHigh = new MetroFramework.Controls.MetroToggle();
            this.metroToggleLootASLow = new MetroFramework.Controls.MetroToggle();
            this.metroToggleLootASNone = new MetroFramework.Controls.MetroToggle();
            this.metroLabelLootASQuest = new MetroFramework.Controls.MetroLabel();
            this.metroLabelLootASMedium = new MetroFramework.Controls.MetroLabel();
            this.metroLabelLootASSuper = new MetroFramework.Controls.MetroLabel();
            this.metroLabelLootASUltra = new MetroFramework.Controls.MetroLabel();
            this.metroLabelLootASHigh = new MetroFramework.Controls.MetroLabel();
            this.metroLabelLootASLow = new MetroFramework.Controls.MetroLabel();
            this.metroLabelLootASNone = new MetroFramework.Controls.MetroLabel();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroTextBoxLootValuePerSlot = new MetroFramework.Controls.MetroTextBox();
            this.metroLabelLootValuePerItem = new MetroFramework.Controls.MetroLabel();
            this.metroLabelLootValuePerSlot = new MetroFramework.Controls.MetroLabel();
            this.metroLabelLootValue = new MetroFramework.Controls.MetroLabel();
            this.metroToggleLootValuePerSlot = new MetroFramework.Controls.MetroToggle();
            this.metroToggleLootASByValue = new MetroFramework.Controls.MetroToggle();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroToggleShortNames = new MetroFramework.Controls.MetroToggle();
            this.metroLabelNameLengthLimit = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxNameLengthLimit = new MetroFramework.Controls.MetroTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.metroPanel1.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            this.SuspendLayout();
            
            
            
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            
            
            
            this.buttonIconColorsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonIconColorsSave.Location = new System.Drawing.Point(208, 301);
            this.buttonIconColorsSave.Name = "buttonIconColorsSave";
            this.buttonIconColorsSave.Size = new System.Drawing.Size(75, 23);
            this.buttonIconColorsSave.TabIndex = 7;
            this.buttonIconColorsSave.Text = "Save";
            this.buttonIconColorsSave.UseSelectable = true;
            this.buttonIconColorsSave.Click += new System.EventHandler(this.buttonIconColorsSave_Click);
            
            
            
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
            this.metroListViewLootCategoryColors.Size = new System.Drawing.Size(260, 232);
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
            
            
            
            this.metroToolTipCommon.Style = MetroFramework.MetroColorStyle.Default;
            this.metroToolTipCommon.StyleManager = null;
            this.metroToolTipCommon.Theme = MetroFramework.MetroThemeStyle.Default;
            
            
            
            this.metroLabelLootLiveAmountPerCycle.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroLabelLootLiveAmountPerCycle.Location = new System.Drawing.Point(636, 137);
            this.metroLabelLootLiveAmountPerCycle.Name = "metroLabelLootLiveAmountPerCycle";
            this.metroLabelLootLiveAmountPerCycle.Size = new System.Drawing.Size(106, 23);
            this.metroLabelLootLiveAmountPerCycle.TabIndex = 22;
            this.metroLabelLootLiveAmountPerCycle.Text = "Reads per Cycle";
            this.metroToolTipCommon.SetToolTip(this.metroLabelLootLiveAmountPerCycle, resources.GetString("metroLabelLootLiveAmountPerCycle.ToolTip"));
            
            
            
            this.metroCheckBoxLootForceShow.Location = new System.Drawing.Point(636, 163);
            this.metroCheckBoxLootForceShow.Name = "metroCheckBoxLootForceShow";
            this.metroCheckBoxLootForceShow.Size = new System.Drawing.Size(178, 23);
            this.metroCheckBoxLootForceShow.TabIndex = 24;
            this.metroCheckBoxLootForceShow.Text = "Force Show Item";
            this.metroToolTipCommon.SetToolTip(this.metroCheckBoxLootForceShow, "See CSV file (ForceShow column)\r\nIt\'s an override so if you set that to TRUE\r\ntha" +
        "t exact item will be shown regardless of the filter\r\n");
            this.metroCheckBoxLootForceShow.UseSelectable = true;
            this.metroCheckBoxLootForceShow.CheckedChanged += new System.EventHandler(this.metroCheckBoxLootForceShow_CheckedChanged);
            
            
            
            this.metroToggleLootASQuest.AutoSize = true;
            this.metroToggleLootASQuest.Location = new System.Drawing.Point(82, 83);
            this.metroToggleLootASQuest.Name = "metroToggleLootASQuest";
            this.metroToggleLootASQuest.Size = new System.Drawing.Size(80, 17);
            this.metroToggleLootASQuest.TabIndex = 27;
            this.metroToggleLootASQuest.Text = "Off";
            this.metroToolTipCommon.SetToolTip(this.metroToggleLootASQuest, "Highlight quest items with custom color.");
            this.metroToggleLootASQuest.UseSelectable = true;
            this.metroToggleLootASQuest.CheckedChanged += new System.EventHandler(this.metroToggleLootASQuest_CheckedChanged);
            
            
            
            this.metroCheckBoxLootInContainersRead.Location = new System.Drawing.Point(636, 92);
            this.metroCheckBoxLootInContainersRead.Name = "metroCheckBoxLootInContainersRead";
            this.metroCheckBoxLootInContainersRead.Size = new System.Drawing.Size(159, 23);
            this.metroCheckBoxLootInContainersRead.TabIndex = 24;
            this.metroCheckBoxLootInContainersRead.Text = "Read Loot in containers";
            this.metroCheckBoxLootInContainersRead.UseSelectable = true;
            this.metroCheckBoxLootInContainersRead.CheckedChanged += new System.EventHandler(this.metroCheckBoxLootInContainersRead_CheckedChanged);
            
            
            
            this.metroCheckBoxLootLive.Location = new System.Drawing.Point(636, 111);
            this.metroCheckBoxLootLive.Name = "metroCheckBoxLootLive";
            this.metroCheckBoxLootLive.Size = new System.Drawing.Size(159, 23);
            this.metroCheckBoxLootLive.TabIndex = 24;
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
            this.metroTextBoxLootLiveAmountPerCycle.Location = new System.Drawing.Point(748, 137);
            this.metroTextBoxLootLiveAmountPerCycle.MaxLength = 32767;
            this.metroTextBoxLootLiveAmountPerCycle.Name = "metroTextBoxLootLiveAmountPerCycle";
            this.metroTextBoxLootLiveAmountPerCycle.PasswordChar = '\0';
            this.metroTextBoxLootLiveAmountPerCycle.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxLootLiveAmountPerCycle.SelectedText = "";
            this.metroTextBoxLootLiveAmountPerCycle.SelectionLength = 0;
            this.metroTextBoxLootLiveAmountPerCycle.SelectionStart = 0;
            this.metroTextBoxLootLiveAmountPerCycle.ShortcutsEnabled = true;
            this.metroTextBoxLootLiveAmountPerCycle.Size = new System.Drawing.Size(42, 23);
            this.metroTextBoxLootLiveAmountPerCycle.TabIndex = 23;
            this.metroTextBoxLootLiveAmountPerCycle.UseSelectable = true;
            this.metroTextBoxLootLiveAmountPerCycle.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxLootLiveAmountPerCycle.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBoxLootLiveAmountPerCycle.TextChanged += new System.EventHandler(this.metroTextBoxLootLiveAmountPerCycle_TextChanged);
            
            
            
            this.metroCheckBoxLootShowPricesAlways.Location = new System.Drawing.Point(636, 73);
            this.metroCheckBoxLootShowPricesAlways.Name = "metroCheckBoxLootShowPricesAlways";
            this.metroCheckBoxLootShowPricesAlways.Size = new System.Drawing.Size(130, 23);
            this.metroCheckBoxLootShowPricesAlways.TabIndex = 24;
            this.metroCheckBoxLootShowPricesAlways.Text = "Always Show Prices";
            this.metroCheckBoxLootShowPricesAlways.UseSelectable = true;
            this.metroCheckBoxLootShowPricesAlways.CheckedChanged += new System.EventHandler(this.metroCheckBoxLootShowPricesAlways_CheckedChanged);
            
            
            
            this.metroLabel2.Location = new System.Drawing.Point(485, 50);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(122, 17);
            this.metroLabel2.TabIndex = 22;
            this.metroLabel2.Text = "Show by Price";
            this.metroLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            
            
            this.metroLabel3.Location = new System.Drawing.Point(306, 50);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(122, 17);
            this.metroLabel3.TabIndex = 22;
            this.metroLabel3.Text = "Show by Priority";
            this.metroLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            
            
            
            this.metroPanel1.Controls.Add(this.metroToggleLootASQuest);
            this.metroPanel1.Controls.Add(this.metroToggleLootASMedium);
            this.metroPanel1.Controls.Add(this.metroToggleLootASSuper);
            this.metroPanel1.Controls.Add(this.metroToggleLootASUltra);
            this.metroPanel1.Controls.Add(this.metroToggleLootASHigh);
            this.metroPanel1.Controls.Add(this.metroToggleLootASLow);
            this.metroPanel1.Controls.Add(this.metroToggleLootASNone);
            this.metroPanel1.Controls.Add(this.metroLabelLootASQuest);
            this.metroPanel1.Controls.Add(this.metroLabelLootASMedium);
            this.metroPanel1.Controls.Add(this.metroLabelLootASSuper);
            this.metroPanel1.Controls.Add(this.metroLabelLootASUltra);
            this.metroPanel1.Controls.Add(this.metroLabelLootASHigh);
            this.metroPanel1.Controls.Add(this.metroLabelLootASLow);
            this.metroPanel1.Controls.Add(this.metroLabelLootASNone);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(289, 73);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(341, 114);
            this.metroPanel1.TabIndex = 25;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            
            
            
            this.metroToggleLootASMedium.AutoSize = true;
            this.metroToggleLootASMedium.Location = new System.Drawing.Point(82, 51);
            this.metroToggleLootASMedium.Name = "metroToggleLootASMedium";
            this.metroToggleLootASMedium.Size = new System.Drawing.Size(80, 17);
            this.metroToggleLootASMedium.TabIndex = 28;
            this.metroToggleLootASMedium.Text = "Off";
            this.metroToggleLootASMedium.UseSelectable = true;
            this.metroToggleLootASMedium.CheckedChanged += new System.EventHandler(this.metroToggleLootASMedium_CheckedChanged);
            
            
            
            this.metroToggleLootASSuper.AutoSize = true;
            this.metroToggleLootASSuper.Location = new System.Drawing.Point(251, 53);
            this.metroToggleLootASSuper.Name = "metroToggleLootASSuper";
            this.metroToggleLootASSuper.Size = new System.Drawing.Size(80, 17);
            this.metroToggleLootASSuper.TabIndex = 29;
            this.metroToggleLootASSuper.Text = "Off";
            this.metroToggleLootASSuper.UseSelectable = true;
            this.metroToggleLootASSuper.CheckedChanged += new System.EventHandler(this.metroToggleLootASSuper_CheckedChanged);
            
            
            
            this.metroToggleLootASUltra.AutoSize = true;
            this.metroToggleLootASUltra.Location = new System.Drawing.Point(251, 30);
            this.metroToggleLootASUltra.Name = "metroToggleLootASUltra";
            this.metroToggleLootASUltra.Size = new System.Drawing.Size(80, 17);
            this.metroToggleLootASUltra.TabIndex = 30;
            this.metroToggleLootASUltra.Text = "Off";
            this.metroToggleLootASUltra.UseSelectable = true;
            this.metroToggleLootASUltra.CheckedChanged += new System.EventHandler(this.metroToggleLootASUltra_CheckedChanged);
            
            
            
            this.metroToggleLootASHigh.AutoSize = true;
            this.metroToggleLootASHigh.Location = new System.Drawing.Point(251, 5);
            this.metroToggleLootASHigh.Name = "metroToggleLootASHigh";
            this.metroToggleLootASHigh.Size = new System.Drawing.Size(80, 17);
            this.metroToggleLootASHigh.TabIndex = 31;
            this.metroToggleLootASHigh.Text = "Off";
            this.metroToggleLootASHigh.UseSelectable = true;
            this.metroToggleLootASHigh.CheckedChanged += new System.EventHandler(this.metroToggleLootASHigh_CheckedChanged);
            
            
            
            this.metroToggleLootASLow.AutoSize = true;
            this.metroToggleLootASLow.Location = new System.Drawing.Point(82, 28);
            this.metroToggleLootASLow.Name = "metroToggleLootASLow";
            this.metroToggleLootASLow.Size = new System.Drawing.Size(80, 17);
            this.metroToggleLootASLow.TabIndex = 32;
            this.metroToggleLootASLow.Text = "Off";
            this.metroToggleLootASLow.UseSelectable = true;
            this.metroToggleLootASLow.CheckedChanged += new System.EventHandler(this.metroToggleLootASLow_CheckedChanged);
            
            
            
            this.metroToggleLootASNone.AutoSize = true;
            this.metroToggleLootASNone.Location = new System.Drawing.Point(82, 5);
            this.metroToggleLootASNone.Name = "metroToggleLootASNone";
            this.metroToggleLootASNone.Size = new System.Drawing.Size(80, 17);
            this.metroToggleLootASNone.TabIndex = 33;
            this.metroToggleLootASNone.Text = "Off";
            this.metroToggleLootASNone.UseSelectable = true;
            this.metroToggleLootASNone.CheckedChanged += new System.EventHandler(this.metroToggleLootASNone_CheckedChanged);
            
            
            
            this.metroLabelLootASQuest.AutoSize = true;
            this.metroLabelLootASQuest.Location = new System.Drawing.Point(3, 81);
            this.metroLabelLootASQuest.Name = "metroLabelLootASQuest";
            this.metroLabelLootASQuest.Size = new System.Drawing.Size(73, 19);
            this.metroLabelLootASQuest.TabIndex = 20;
            this.metroLabelLootASQuest.Text = "Quest Item";
            
            
            
            this.metroLabelLootASMedium.AutoSize = true;
            this.metroLabelLootASMedium.Location = new System.Drawing.Point(3, 49);
            this.metroLabelLootASMedium.Name = "metroLabelLootASMedium";
            this.metroLabelLootASMedium.Size = new System.Drawing.Size(58, 19);
            this.metroLabelLootASMedium.TabIndex = 21;
            this.metroLabelLootASMedium.Text = "Medium";
            
            
            
            this.metroLabelLootASSuper.AutoSize = true;
            this.metroLabelLootASSuper.Location = new System.Drawing.Point(172, 51);
            this.metroLabelLootASSuper.Name = "metroLabelLootASSuper";
            this.metroLabelLootASSuper.Size = new System.Drawing.Size(43, 19);
            this.metroLabelLootASSuper.TabIndex = 22;
            this.metroLabelLootASSuper.Text = "Super";
            
            
            
            this.metroLabelLootASUltra.AutoSize = true;
            this.metroLabelLootASUltra.Location = new System.Drawing.Point(172, 28);
            this.metroLabelLootASUltra.Name = "metroLabelLootASUltra";
            this.metroLabelLootASUltra.Size = new System.Drawing.Size(37, 19);
            this.metroLabelLootASUltra.TabIndex = 23;
            this.metroLabelLootASUltra.Text = "Ultra";
            
            
            
            this.metroLabelLootASHigh.AutoSize = true;
            this.metroLabelLootASHigh.Location = new System.Drawing.Point(172, 3);
            this.metroLabelLootASHigh.Name = "metroLabelLootASHigh";
            this.metroLabelLootASHigh.Size = new System.Drawing.Size(36, 19);
            this.metroLabelLootASHigh.TabIndex = 24;
            this.metroLabelLootASHigh.Text = "High";
            
            
            
            this.metroLabelLootASLow.AutoSize = true;
            this.metroLabelLootASLow.Location = new System.Drawing.Point(3, 26);
            this.metroLabelLootASLow.Name = "metroLabelLootASLow";
            this.metroLabelLootASLow.Size = new System.Drawing.Size(32, 19);
            this.metroLabelLootASLow.TabIndex = 25;
            this.metroLabelLootASLow.Text = "Low";
            
            
            
            this.metroLabelLootASNone.AutoSize = true;
            this.metroLabelLootASNone.Location = new System.Drawing.Point(3, 3);
            this.metroLabelLootASNone.Name = "metroLabelLootASNone";
            this.metroLabelLootASNone.Size = new System.Drawing.Size(41, 19);
            this.metroLabelLootASNone.TabIndex = 26;
            this.metroLabelLootASNone.Text = "None";
            
            
            
            this.metroPanel2.Controls.Add(this.metroTextBoxLootValuePerSlot);
            this.metroPanel2.Controls.Add(this.metroLabelLootValuePerItem);
            this.metroPanel2.Controls.Add(this.metroLabelLootValuePerSlot);
            this.metroPanel2.Controls.Add(this.metroLabelLootValue);
            this.metroPanel2.Controls.Add(this.metroToggleLootValuePerSlot);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(289, 73);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(341, 114);
            this.metroPanel2.TabIndex = 26;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            
            
            
            
            
            
            this.metroTextBoxLootValuePerSlot.CustomButton.Image = null;
            this.metroTextBoxLootValuePerSlot.CustomButton.Location = new System.Drawing.Point(115, 1);
            this.metroTextBoxLootValuePerSlot.CustomButton.Name = "";
            this.metroTextBoxLootValuePerSlot.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBoxLootValuePerSlot.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxLootValuePerSlot.CustomButton.TabIndex = 1;
            this.metroTextBoxLootValuePerSlot.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxLootValuePerSlot.CustomButton.UseSelectable = true;
            this.metroTextBoxLootValuePerSlot.CustomButton.Visible = false;
            this.metroTextBoxLootValuePerSlot.Lines = new string[0];
            this.metroTextBoxLootValuePerSlot.Location = new System.Drawing.Point(6, 53);
            this.metroTextBoxLootValuePerSlot.MaxLength = 32767;
            this.metroTextBoxLootValuePerSlot.Name = "metroTextBoxLootValuePerSlot";
            this.metroTextBoxLootValuePerSlot.PasswordChar = '\0';
            this.metroTextBoxLootValuePerSlot.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxLootValuePerSlot.SelectedText = "";
            this.metroTextBoxLootValuePerSlot.SelectionLength = 0;
            this.metroTextBoxLootValuePerSlot.SelectionStart = 0;
            this.metroTextBoxLootValuePerSlot.ShortcutsEnabled = true;
            this.metroTextBoxLootValuePerSlot.Size = new System.Drawing.Size(137, 23);
            this.metroTextBoxLootValuePerSlot.TabIndex = 30;
            this.metroTextBoxLootValuePerSlot.UseSelectable = true;
            this.metroTextBoxLootValuePerSlot.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxLootValuePerSlot.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBoxLootValuePerSlot.TextChanged += new System.EventHandler(this.metroTextBoxLootValuePerSlot_TextChanged);
            
            
            
            this.metroLabelLootValuePerItem.Location = new System.Drawing.Point(2, 3);
            this.metroLabelLootValuePerItem.Name = "metroLabelLootValuePerItem";
            this.metroLabelLootValuePerItem.Size = new System.Drawing.Size(122, 17);
            this.metroLabelLootValuePerItem.TabIndex = 27;
            this.metroLabelLootValuePerItem.Text = "Price Per Item";
            this.metroLabelLootValuePerItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            
            
            
            this.metroLabelLootValuePerSlot.Location = new System.Drawing.Point(181, 3);
            this.metroLabelLootValuePerSlot.Name = "metroLabelLootValuePerSlot";
            this.metroLabelLootValuePerSlot.Size = new System.Drawing.Size(122, 17);
            this.metroLabelLootValuePerSlot.TabIndex = 28;
            this.metroLabelLootValuePerSlot.Text = "Price Per Slot";
            this.metroLabelLootValuePerSlot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            
            
            this.metroLabelLootValue.AutoSize = true;
            this.metroLabelLootValue.Location = new System.Drawing.Point(3, 31);
            this.metroLabelLootValue.Name = "metroLabelLootValue";
            this.metroLabelLootValue.Size = new System.Drawing.Size(70, 19);
            this.metroLabelLootValue.TabIndex = 29;
            this.metroLabelLootValue.Text = "Price Limit";
            
            
            
            this.metroToggleLootValuePerSlot.AutoSize = true;
            this.metroToggleLootValuePerSlot.DisplayStatus = false;
            this.metroToggleLootValuePerSlot.Location = new System.Drawing.Point(130, 3);
            this.metroToggleLootValuePerSlot.Name = "metroToggleLootValuePerSlot";
            this.metroToggleLootValuePerSlot.Size = new System.Drawing.Size(50, 17);
            this.metroToggleLootValuePerSlot.TabIndex = 25;
            this.metroToggleLootValuePerSlot.Text = "Off";
            this.metroToggleLootValuePerSlot.UseSelectable = true;
            this.metroToggleLootValuePerSlot.CheckedChanged += new System.EventHandler(this.metroToggleLootValuePerSlot_CheckedChanged);
            
            
            
            this.metroToggleLootASByValue.AutoSize = true;
            this.metroToggleLootASByValue.DisplayStatus = false;
            this.metroToggleLootASByValue.Location = new System.Drawing.Point(434, 50);
            this.metroToggleLootASByValue.Name = "metroToggleLootASByValue";
            this.metroToggleLootASByValue.Size = new System.Drawing.Size(50, 17);
            this.metroToggleLootASByValue.TabIndex = 27;
            this.metroToggleLootASByValue.Text = "Off";
            this.metroToggleLootASByValue.UseSelectable = true;
            this.metroToggleLootASByValue.CheckedChanged += new System.EventHandler(this.metroToggleLootASByValue_CheckedChanged);
            
            
            
            this.metroLabel1.Location = new System.Drawing.Point(470, 193);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(122, 17);
            this.metroLabel1.TabIndex = 22;
            this.metroLabel1.Text = "Show Short Name";
            this.metroLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            
            
            this.metroLabel4.Location = new System.Drawing.Point(291, 193);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(122, 17);
            this.metroLabel4.TabIndex = 22;
            this.metroLabel4.Text = "Show Full Name";
            this.metroLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            
            
            
            this.metroToggleShortNames.AutoSize = true;
            this.metroToggleShortNames.DisplayStatus = false;
            this.metroToggleShortNames.Location = new System.Drawing.Point(419, 193);
            this.metroToggleShortNames.Name = "metroToggleShortNames";
            this.metroToggleShortNames.Size = new System.Drawing.Size(50, 17);
            this.metroToggleShortNames.TabIndex = 27;
            this.metroToggleShortNames.Text = "Off";
            this.metroToggleShortNames.UseSelectable = true;
            this.metroToggleShortNames.CheckedChanged += new System.EventHandler(this.metroToggleShortNames_CheckedChanged);
            
            
            
            this.metroLabelNameLengthLimit.Cursor = System.Windows.Forms.Cursors.Help;
            this.metroLabelNameLengthLimit.Location = new System.Drawing.Point(289, 219);
            this.metroLabelNameLengthLimit.Name = "metroLabelNameLengthLimit";
            this.metroLabelNameLengthLimit.Size = new System.Drawing.Size(139, 23);
            this.metroLabelNameLengthLimit.TabIndex = 22;
            this.metroLabelNameLengthLimit.Text = "Name length limit";
            
            
            
            
            
            
            this.metroTextBoxNameLengthLimit.CustomButton.Image = null;
            this.metroTextBoxNameLengthLimit.CustomButton.Location = new System.Drawing.Point(20, 1);
            this.metroTextBoxNameLengthLimit.CustomButton.Name = "";
            this.metroTextBoxNameLengthLimit.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBoxNameLengthLimit.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxNameLengthLimit.CustomButton.TabIndex = 1;
            this.metroTextBoxNameLengthLimit.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxNameLengthLimit.CustomButton.UseSelectable = true;
            this.metroTextBoxNameLengthLimit.CustomButton.Visible = false;
            this.metroTextBoxNameLengthLimit.Lines = new string[0];
            this.metroTextBoxNameLengthLimit.Location = new System.Drawing.Point(409, 219);
            this.metroTextBoxNameLengthLimit.MaxLength = 32767;
            this.metroTextBoxNameLengthLimit.Name = "metroTextBoxNameLengthLimit";
            this.metroTextBoxNameLengthLimit.PasswordChar = '\0';
            this.metroTextBoxNameLengthLimit.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxNameLengthLimit.SelectedText = "";
            this.metroTextBoxNameLengthLimit.SelectionLength = 0;
            this.metroTextBoxNameLengthLimit.SelectionStart = 0;
            this.metroTextBoxNameLengthLimit.ShortcutsEnabled = true;
            this.metroTextBoxNameLengthLimit.Size = new System.Drawing.Size(42, 23);
            this.metroTextBoxNameLengthLimit.TabIndex = 23;
            this.metroTextBoxNameLengthLimit.UseSelectable = true;
            this.metroTextBoxNameLengthLimit.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxNameLengthLimit.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBoxNameLengthLimit.TextChanged += new System.EventHandler(this.metroTextBoxNameLengthLimit_TextChanged);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 337);
            this.Controls.Add(this.metroToggleShortNames);
            this.Controls.Add(this.metroToggleLootASByValue);
            this.Controls.Add(this.metroPanel2);
            this.Controls.Add(this.metroPanel1);
            this.Controls.Add(this.metroCheckBoxLootForceShow);
            this.Controls.Add(this.metroCheckBoxLootShowPricesAlways);
            this.Controls.Add(this.metroCheckBoxLootLive);
            this.Controls.Add(this.metroCheckBoxLootInContainersRead);
            this.Controls.Add(this.metroListViewLootCategoryColors);
            this.Controls.Add(this.buttonIconColorsSave);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroTextBoxNameLengthLimit);
            this.Controls.Add(this.metroTextBoxLootLiveAmountPerCycle);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabelNameLengthLimit);
            this.Controls.Add(this.metroLabelLootLiveAmountPerCycle);
            this.Name = "LootSettings";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.StyleManager = this.metroStyleManager;
            this.Text = "Loot Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.IconColors_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolTip toolTipSpecial;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Controls.MetroButton buttonIconColorsSave;
        public MetroFramework.Controls.MetroListView metroListViewLootCategoryColors;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnColor;
        private MetroFramework.Components.MetroToolTip metroToolTipCommon;
        private MetroFramework.Controls.MetroCheckBox metroCheckBoxLootInContainersRead;
        private MetroFramework.Controls.MetroCheckBox metroCheckBoxLootLive;
        private MetroFramework.Controls.MetroTextBox metroTextBoxLootLiveAmountPerCycle;
        private MetroFramework.Controls.MetroLabel metroLabelLootLiveAmountPerCycle;
        private MetroFramework.Controls.MetroCheckBox metroCheckBoxLootShowPricesAlways;
        private MetroFramework.Controls.MetroCheckBox metroCheckBoxLootForceShow;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroToggle metroToggleLootASQuest;
        private MetroFramework.Controls.MetroToggle metroToggleLootASMedium;
        private MetroFramework.Controls.MetroToggle metroToggleLootASSuper;
        private MetroFramework.Controls.MetroToggle metroToggleLootASUltra;
        private MetroFramework.Controls.MetroToggle metroToggleLootASHigh;
        private MetroFramework.Controls.MetroToggle metroToggleLootASLow;
        private MetroFramework.Controls.MetroToggle metroToggleLootASNone;
        private MetroFramework.Controls.MetroLabel metroLabelLootASQuest;
        private MetroFramework.Controls.MetroLabel metroLabelLootASMedium;
        private MetroFramework.Controls.MetroLabel metroLabelLootASSuper;
        private MetroFramework.Controls.MetroLabel metroLabelLootASUltra;
        private MetroFramework.Controls.MetroLabel metroLabelLootASHigh;
        private MetroFramework.Controls.MetroLabel metroLabelLootASLow;
        private MetroFramework.Controls.MetroLabel metroLabelLootASNone;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroTextBox metroTextBoxLootValuePerSlot;
        private MetroFramework.Controls.MetroLabel metroLabelLootValuePerItem;
        private MetroFramework.Controls.MetroLabel metroLabelLootValuePerSlot;
        private MetroFramework.Controls.MetroLabel metroLabelLootValue;
        private MetroFramework.Controls.MetroToggle metroToggleLootValuePerSlot;
        private MetroFramework.Controls.MetroToggle metroToggleLootASByValue;
        private MetroFramework.Controls.MetroToggle metroToggleShortNames;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroTextBox metroTextBoxNameLengthLimit;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabelNameLengthLimit;
    }
}