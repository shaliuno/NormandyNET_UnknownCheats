using ListViewSortAnyColumn;
using MetroFramework;
using MetroFramework.Forms;
using NormandyNET.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NormandyNET.Modules.EFT.UI
{
    public partial class LootSettings : MetroForm
    {
        public LootSettings()
        {
            InitializeComponent();
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);

            metroToggleLootASHigh.Checked = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[3];
            metroToggleLootASLow.Checked = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[1];
            metroToggleLootASMedium.Checked = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[2];
            metroToggleLootASNone.Checked = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[0];
            metroToggleLootASSuper.Checked = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[5];
            metroToggleLootASUltra.Checked = ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[4];
            metroToggleLootASByValue.Checked = ModuleEFT.settingsForm.settingsJson.Loot.ShowByValue;
            metroToggleLootASQuest.Checked = ModuleEFT.settingsForm.settingsJson.Loot.ShowQuestItems;

            metroTextBoxLootValuePerSlot.Text = ModuleEFT.settingsForm.settingsJson.Loot.Value.ToString();

            metroToggleLootValuePerSlot.Checked = ModuleEFT.settingsForm.settingsJson.Loot.ValuePerSlot;
            metroToggleShortNames.Checked = ModuleEFT.settingsForm.settingsJson.Loot.ShortNames;

            metroCheckBoxLootInContainersRead.Checked = ModuleEFT.settingsForm.settingsJson.Loot.ReadLootInContainers;

            metroCheckBoxLootShowPricesAlways.Checked = ModuleEFT.settingsForm.settingsJson.Loot.ShowPricesAlways;
            metroCheckBoxLootForceShow.Checked = ModuleEFT.settingsForm.settingsJson.Loot.ForceShow;

            metroCheckBoxLootLive.Checked = ModuleEFT.settingsForm.settingsJson.Loot.LiveLoot;

            metroTextBoxLootLiveAmountPerCycle.Text = ModuleEFT.settingsForm.settingsJson.Loot.LiveLootPerCycle.ToString();
            metroTextBoxNameLengthLimit.Text = ModuleEFT.settingsForm.settingsJson.Loot.NameLengthLimit.ToString();
        }

        private void buttonIconColorsSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
        }

        private class EntityTypeColor
        {
            public string EntityType;
            public Color EntityColorType;

            public EntityTypeColor(string entityType, Color entityColor)
            {
                EntityType = entityType;
                EntityColorType = entityColor;
            }
        }

        private void metroListViewLootCategoryColors_AddItem(ListView lvw, string entityType, Color entityColor)
        {
            ListViewItem item = new ListViewItem(entityType);

            EntityTypeColor entityTypeColor = new EntityTypeColor(entityType, entityColor);
            item.Tag = entityTypeColor;

            lvw.Items.Add(item);

            item.SubItems.Add(entityColor.Name);
        }

        private void metroListViewLootCategoryColors_DoubleClick(object sender, EventArgs e)
        {
            if (metroListViewLootCategoryColors.SelectedItems.Count == 1)
            {
                var entityType = metroListViewLootCategoryColors.SelectedItems[0].Text;

                ColorDialog colorDlg = new ColorDialog();
                if (colorDlg.ShowDialog() == DialogResult.OK)
                {
                    if (ModuleEFT.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(entityType, out Color entityColor))
                    {
                        ModuleEFT.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors[entityType] = colorDlg.Color;
                    }

                    ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
                }

                metroListViewLootCategoryColors.Invalidate();
            }
        }

        private void metroListViewLootCategoryColors_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            ListViewItem item = e.Item;
            EntityTypeColor server_status = item.Tag as EntityTypeColor;

            var statusBoxSizeW = 6;
            var statusBoxSizeH = 3;
            Color statusColor = Color.Gray;

            switch (e.ColumnIndex)
            {
                case 0:
                    break;

                case 1:

                    if (ModuleEFT.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(e.Item.Text, out Color entityColor))
                    {
                        statusColor = entityColor;
                    }

                    Rectangle rect = new Rectangle(
                       e.Bounds.Left,
                       e.Bounds.Top,
                       e.Bounds.Width,
                       e.Bounds.Height);

                    using (SolidBrush br = new SolidBrush(Color.FromArgb(255, 17, 17, 17)))
                    {
                        e.Graphics.FillRectangle(br, rect);
                    }

                    rect = new Rectangle(
                        e.Bounds.Left + statusBoxSizeW,
                        e.Bounds.Top + statusBoxSizeH,
                        e.Bounds.Width - statusBoxSizeW * 2,
                        e.Bounds.Height - statusBoxSizeH * 2);

                    using (SolidBrush br = new SolidBrush(statusColor))
                    {
                        e.Graphics.FillRectangle(br, rect);
                    }

                    rect = new Rectangle(
                       e.Bounds.Left + statusBoxSizeW - 1,
                       e.Bounds.Top + statusBoxSizeH - 1,
                       e.Bounds.Width - statusBoxSizeW * 2 + 1,
                       e.Bounds.Height - statusBoxSizeH * 2 + 1);

                    using (Pen br = new Pen(Color.FromArgb(255, 100, 100, 100)))
                    {
                        e.Graphics.DrawRectangle(br, rect);
                    }

                    break;
            }

            e.Graphics.ResetTransform();
            ListView lvw = e.Item.ListView;
            if (lvw.FullRowSelect)
            {
                e.DrawFocusRectangle(e.Item.Bounds);
            }
        }

        private void metroListViewLootCategoryColors_Layout(object sender, LayoutEventArgs e)
        {
            NativeMethods.ShowScrollBar(this.metroListViewLootCategoryColors.Handle, (int)NativeMethods.SB_BOTH, false);
        }

        private void IconColors_Load(object sender, EventArgs e)
        {
            RecolorListViewsOwnerDrawn();
            Color color;

            foreach (string entityType in LootItemHelper.LootCategoriesCanShow)
            {
                if (ModuleEFT.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(entityType, out color))
                {
                    metroListViewLootCategoryColors_AddItem(metroListViewLootCategoryColors, entityType, color);
                }
                else
                {
                    metroListViewLootCategoryColors_AddItem(metroListViewLootCategoryColors, entityType, Color.Gray);
                    ModuleEFT.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.Add(entityType, Color.Gray);
                }
            }
        }

        private void RecolorListViewsOwnerDrawn()
        {
            switch (metroStyleManager.Theme)
            {
                case MetroThemeStyle.Dark:
                    metroListViewLootCategoryColors.BackColor = ColorTranslator.FromHtml("#111111");
                    break;

                default:
                    metroListViewLootCategoryColors.BackColor = SystemColors.Control;
                    break;
            }
        }

        private void metroToggleLootASNone_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[0] = metroToggleLootASNone.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroToggleLootASLow_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[1] = metroToggleLootASLow.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroToggleLootASMedium_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[2] = metroToggleLootASMedium.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroToggleLootASHigh_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[3] = metroToggleLootASHigh.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroToggleLootASUltra_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[4] = metroToggleLootASUltra.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroToggleLootASSuper_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.AlwaysShowPriority[5] = metroToggleLootASSuper.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroToggleLootASByValue_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.ShowByValue = metroToggleLootASByValue.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;

            metroPanel1.Visible = !ModuleEFT.settingsForm.settingsJson.Loot.ShowByValue;
            metroPanel2.Visible = ModuleEFT.settingsForm.settingsJson.Loot.ShowByValue;
        }

        private void metroToggleLootASQuest_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.ShowQuestItems = metroToggleLootASQuest.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroListViewLootCategoryColors_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (metroListViewLootCategoryColors.Items.Count < 2)
            {
                return;
            }

            ItemComparer sorter = metroListViewLootCategoryColors.ListViewItemSorter as ItemComparer;
            if (sorter == null)
            {
                sorter = new ItemComparer(e.Column);
                sorter.Order = SortOrder.Ascending;
                metroListViewLootCategoryColors.ListViewItemSorter = sorter;
            }

            if (e.Column == sorter.Column)
            {
                if (sorter.Order == SortOrder.Ascending)
                    sorter.Order = SortOrder.Descending;
                else
                    sorter.Order = SortOrder.Ascending;
            }
            else
            {
                sorter.Column = e.Column;
                sorter.Order = SortOrder.Ascending;
            }

            this.metroListViewLootCategoryColors.Sort();
        }

        private void metroCheckBoxLootInContainersRead_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.ReadLootInContainers = metroCheckBoxLootInContainersRead.Checked;
        }

        private void metroToggleLootValuePerSlot_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.ValuePerSlot = metroToggleLootValuePerSlot.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroCheckBoxLootLive_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.LiveLoot = metroCheckBoxLootLive.Checked;
        }

        private void metroCheckBoxLootShowPricesAlways_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.ShowPricesAlways = metroCheckBoxLootShowPricesAlways.Checked;
        }

        private void metroCheckBoxLootForceShow_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.ForceShow = metroCheckBoxLootForceShow.Checked;
            ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
        }

        private void metroTextBoxLootValuePerSlot_TextChanged(object sender, EventArgs e)
        {
            var valueOk = Int32.TryParse(metroTextBoxLootValuePerSlot.Text, out int value);

            if (valueOk && value >= 1)
            {
                ModuleEFT.settingsForm.settingsJson.Loot.Value = value;
                ModuleEFT.settingsForm.settingsJson.Loot.ShowStatusChanged = true;
                metroTextBoxLootValuePerSlot.UseCustomForeColor = false;
                metroTextBoxLootValuePerSlot.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxLootValuePerSlot.UseCustomForeColor = true;
                metroTextBoxLootValuePerSlot.ForeColor = Color.Red;
            }
        }

        private void metroTextBoxLootLiveAmountPerCycle_TextChanged(object sender, EventArgs e)
        {
            var valueOk = Int32.TryParse(metroTextBoxLootLiveAmountPerCycle.Text, out int value);

            if (valueOk && (value >= 1 && value <= 200))
            {
                ModuleEFT.settingsForm.settingsJson.Loot.LiveLootPerCycle = value;
                metroTextBoxLootLiveAmountPerCycle.UseCustomForeColor = false;
                metroTextBoxLootLiveAmountPerCycle.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxLootLiveAmountPerCycle.UseCustomForeColor = true;
                metroTextBoxLootLiveAmountPerCycle.ForeColor = Color.Red;
            }
        }

        private void metroTextBoxNameLengthLimit_TextChanged(object sender, EventArgs e)
        {
            var valueOk = Int32.TryParse(metroTextBoxNameLengthLimit.Text, out int value);

            if (valueOk && (value >= 1 && value < 200))
            {
                ModuleEFT.settingsForm.settingsJson.Loot.NameLengthLimit = value;
                metroTextBoxNameLengthLimit.UseCustomForeColor = false;
                metroTextBoxNameLengthLimit.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxNameLengthLimit.UseCustomForeColor = true;
                metroTextBoxNameLengthLimit.ForeColor = Color.Red;
            }
        }

        private void metroToggleShortNames_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.Loot.ShortNames = metroToggleShortNames.Checked;
        }
    }
}