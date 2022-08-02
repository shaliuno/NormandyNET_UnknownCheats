using ListViewSortAnyColumn;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using NormandyNET.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NormandyNET.Modules.DAYZ.UI
{
    public partial class LootSettings : MetroForm
    {
        public LootSettings()
        {
            InitializeComponent();
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);

            metroCheckBoxLootLive.Checked = ModuleDAYZ.settingsForm.settingsJson.Loot.LiveLoot;
            metroTrackBarLootLiveAmountPerCycle.Value = ModuleDAYZ.settingsForm.settingsJson.Loot.LiveLootPerCycle;
            metroTextBoxLootLiveAmountPerCycle.Text = metroTrackBarLootLiveAmountPerCycle.Value.ToString();
        }

        private void buttonLootColorsSave_Click(object sender, EventArgs e)
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
            var listViewLootCategoryColorsSender = ((MetroListView)sender);

            if (listViewLootCategoryColorsSender.SelectedItems.Count == 1)
            {
                var entityType = listViewLootCategoryColorsSender.SelectedItems[0].Text;

                ColorDialog colorDlg = new ColorDialog();
                if (colorDlg.ShowDialog() == DialogResult.OK)
                {
                    if (ModuleDAYZ.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(entityType, out Color entityColor))
                    {
                        ModuleDAYZ.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors[entityType] = colorDlg.Color;
                    }

                    ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
                }

                listViewLootCategoryColorsSender.Invalidate();
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

                    if (ModuleDAYZ.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(e.Item.Text, out Color entityColor))
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
            var listViewLootCategoryColorsSender = ((MetroListView)sender);

            NativeMethods.ShowScrollBar(listViewLootCategoryColorsSender.Handle, (int)NativeMethods.SB_BOTH, false);
        }

        private void LootColors_Load(object sender, EventArgs e)
        {
            RecolorListViewsOwnerDrawn();
            Color color;

            LootItemHelper.LootCategoriesCanShow.Sort();

            var lootCategoriesCountHalf = LootItemHelper.LootCategoriesCanShow.Count / 2;
            var limit = 0;

            foreach (string entityType in LootItemHelper.LootCategoriesCanShow)
            {
                if (ModuleDAYZ.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.TryGetValue(entityType, out color))
                {
                    if (limit <= lootCategoriesCountHalf)
                    {
                        metroListViewLootCategoryColors_AddItem(metroListViewLootCategoryColors, entityType, color);
                    }
                    else
                    {
                        metroListViewLootCategoryColors_AddItem(metroListViewLootCategoryColors2, entityType, color);
                    }
                }
                else
                {
                    if (limit < lootCategoriesCountHalf)
                    {
                        metroListViewLootCategoryColors_AddItem(metroListViewLootCategoryColors, entityType, Color.Gray);
                    }
                    else
                    {
                        metroListViewLootCategoryColors_AddItem(metroListViewLootCategoryColors2, entityType, Color.Gray);
                    }

                    ModuleDAYZ.settingsForm.settingsJson.Colors.LootColors.LootCategoryColors.Add(entityType, Color.Gray);
                }

                limit++;
            }
            this.Height = Math.Max(metroListViewLootCategoryColors.Items.Count, metroListViewLootCategoryColors2.Items.Count) * 20;
        }

        private void RecolorListViewsOwnerDrawn()
        {
            switch (metroStyleManager.Theme)
            {
                case MetroThemeStyle.Dark:
                    metroListViewLootCategoryColors.BackColor = ColorTranslator.FromHtml("#111111");
                    metroListViewLootCategoryColors2.BackColor = ColorTranslator.FromHtml("#111111");
                    break;

                default:
                    metroListViewLootCategoryColors.BackColor = SystemColors.Control;
                    metroListViewLootCategoryColors2.BackColor = SystemColors.Control;
                    break;
            }
        }

        private void metroListViewLootCategoryColors_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var listViewLootCategoryColorsSender = ((MetroListView)sender);

            if (listViewLootCategoryColorsSender.Items.Count < 2)
            {
                return;
            }

            ItemComparer sorter = listViewLootCategoryColorsSender.ListViewItemSorter as ItemComparer;
            if (sorter == null)
            {
                sorter = new ItemComparer(e.Column);
                sorter.Order = SortOrder.Ascending;
                listViewLootCategoryColorsSender.ListViewItemSorter = sorter;
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

            listViewLootCategoryColorsSender.Sort();
        }

        private void metroTrackBarLootLiveAmountPerCycle_Scroll(object sender, ScrollEventArgs e)
        {
            var value = metroTrackBarLootLiveAmountPerCycle.Value;
            metroTextBoxLootLiveAmountPerCycle.Text = $"{value}";
            ModuleDAYZ.settingsForm.settingsJson.Loot.LiveLootPerCycle = metroTrackBarLootLiveAmountPerCycle.Value;
            metroTextBoxLootLiveAmountPerCycle.Text = value.ToString();
        }

        private void metroCheckBoxLootLive_CheckedChanged(object sender, EventArgs e)
        {
            ModuleDAYZ.settingsForm.settingsJson.Loot.LiveLoot = metroCheckBoxLootLive.Checked;
        }
    }
}