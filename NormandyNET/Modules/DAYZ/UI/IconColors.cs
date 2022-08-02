using MetroFramework;
using MetroFramework.Forms;
using NormandyNET.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NormandyNET.Modules.DAYZ.UI
{
    public partial class IconColors : MetroForm
    {
        public IconColors()
        {
            InitializeComponent();
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
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

        private void metroListViewEntityTypeColors_AddItem(ListView lvw, string entityType, Color entityColor)
        {
            ListViewItem item = new ListViewItem(entityType);

            EntityTypeColor entityTypeColor = new EntityTypeColor(entityType, entityColor);
            item.Tag = entityTypeColor;

            lvw.Items.Add(item);

            item.SubItems.Add(entityColor.Name);
        }

        private void metroListViewEntityTypeColors_DoubleClick(object sender, EventArgs e)
        {
            if (metroListViewEntityTypeColors.SelectedItems.Count == 1)
            {
                var entityType = metroListViewEntityTypeColors.SelectedItems[0].Text;
                var haveEntityTypeColor = ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.TryGetValue(entityType, out Color entityColor);

                ColorDialog colorDlg = new ColorDialog();

                if (haveEntityTypeColor)
                {
                    colorDlg.Color = entityColor;
                    colorDlg.FullOpen = true;
                }

                if (colorDlg.ShowDialog() == DialogResult.OK)
                {
                    if (haveEntityTypeColor)
                    {
                        ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors[entityType] = colorDlg.Color;
                    }

                    ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.ColorsChanged = true;
                }

                metroListViewEntityTypeColors.Invalidate();
            }
        }

        private void metroListViewEntityTypeColors_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
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

                    if (ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.TryGetValue(e.Item.Text, out Color entityColor))
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

        private void metroListViewEntityTypeColors_Layout(object sender, LayoutEventArgs e)
        {
            NativeMethods.ShowScrollBar(this.metroListViewEntityTypeColors.Handle, (int)NativeMethods.SB_BOTH, false);
        }

        private void IconColors_Load(object sender, EventArgs e)
        {
            RecolorListViewsOwnerDrawn();
            Color color;

            foreach (string entityType in LootItemHelper.EntityTypesCanShow)
            {
                if (!entityType.Equals("Loot") && !entityType.Equals("NULL"))
                {
                    if (ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.TryGetValue(entityType, out color))
                    {
                        metroListViewEntityTypeColors_AddItem(metroListViewEntityTypeColors, entityType, color);
                    }
                    else
                    {
                        metroListViewEntityTypeColors_AddItem(metroListViewEntityTypeColors, entityType, Color.Gray);
                        ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.Add(entityType, Color.Gray);
                    }
                }
            }

            if (ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.TryGetValue("You", out color))
            {
                metroListViewEntityTypeColors_AddItem(metroListViewEntityTypeColors, "You", color);
            }
            else
            {
                metroListViewEntityTypeColors_AddItem(metroListViewEntityTypeColors, "You", Color.Gray);
                ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.Add("You", Color.Gray);
            }

            if (ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.TryGetValue("Unknown", out color))
            {
                metroListViewEntityTypeColors_AddItem(metroListViewEntityTypeColors, "Unknown", color);
            }
            else
            {
                metroListViewEntityTypeColors_AddItem(metroListViewEntityTypeColors, "Unknown", Color.Gray);
                ModuleDAYZ.settingsForm.settingsJson.Colors.EntityColors.EntityTypeColors.Add("Unknown", Color.Gray);
            }

            this.Height = 106 + metroListViewEntityTypeColors.Items.Count * 40;
        }

        private void RecolorListViewsOwnerDrawn()
        {
            switch (metroStyleManager.Theme)
            {
                case MetroThemeStyle.Dark:
                    metroListViewEntityTypeColors.BackColor = ColorTranslator.FromHtml("#111111");
                    break;

                default:
                    metroListViewEntityTypeColors.BackColor = SystemColors.Control;
                    break;
            }
        }
    }
}