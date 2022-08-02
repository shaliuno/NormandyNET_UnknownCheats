using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using System.Drawing;
using System.Windows.Forms;

namespace Helpers
{
    public static class ThreadHelper
    {
        private delegate void SetTextCallback(Form f, Control ctrl, string text);

        private delegate void SetButtonColorCallback(Form f, Control ctrl, Color color);

        private delegate void SetTextMetroTileCallback(MetroForm f, MetroTile ctrl, string text);

        private delegate void SetStyleMetroTileCallback(MetroForm f, MetroTile ctrl, MetroColorStyle style);

        private delegate void RefreshFormCallback(MetroForm f);

        public static void SetText(Form form, Control ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                var d = new SetTextCallback(SetText);
                form.Invoke(d, new object[] { form, ctrl, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }

        public static void SetTextMetroTile(MetroForm form, MetroTile ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                var d = new SetTextMetroTileCallback(SetTextMetroTile);
                form.Invoke(d, new object[] { form, ctrl, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }

        public static void SetButtonColor(Form form, Control ctrl, Color color)
        {
            if (ctrl.InvokeRequired)
            {
                var d = new SetButtonColorCallback(SetButtonColor);
                form.Invoke(d, new object[] { form, ctrl, color });
            }
            else
            {
                ctrl.BackColor = color;
            }
        }

        public static void SetStyleMetroTile(MetroForm form, MetroTile ctrl, MetroColorStyle style)
        {
            if (ctrl.InvokeRequired)
            {
                var d = new SetStyleMetroTileCallback(SetStyleMetroTile);
                form.Invoke(d, new object[] { form, ctrl, style });
            }
            else
            {
                ctrl.Style = style;
                form.Refresh();
            }
        }

        public static void RefreshForm(MetroForm form)
        {
            if (form.InvokeRequired)
            {
                var d = new RefreshFormCallback(RefreshForm);
                form.Invoke(d, new object[] { form });
            }
            else
            {
                form.Refresh();
            }
        }

        private delegate void SetTextMetroLabelCallback(MetroForm f, MetroLabel ctrl, string text);

        public static void SetTextMetroLabel(MetroForm form, MetroLabel ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                var d = new SetTextMetroLabelCallback(SetTextMetroLabel);
                form.Invoke(d, new object[] { form, ctrl, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }

        private delegate void SetMetroProgressBarValueCallback(MetroForm f, MetroProgressBar ctrl, int value);

        public static void SetMetroProgressBarValue(MetroForm form, MetroProgressBar ctrl, int value)
        {
            if (ctrl.InvokeRequired)
            {
                var d = new SetMetroProgressBarValueCallback(SetMetroProgressBarValue);
                form.Invoke(d, new object[] { form, ctrl, value });
            }
            else
            {
                ctrl.Value = value;
            }
        }

        private delegate void EnableDisableControlCallback(Form f, Control ctrl, bool enabled);

        public static void EnableDisableControl(Form form, Control ctrl, bool enabled)
        {
            if (ctrl.InvokeRequired)
            {
                var d = new EnableDisableControlCallback(EnableDisableControl);
                form.Invoke(d, new object[] { form, ctrl, enabled });
            }
            else
            {
                ctrl.Enabled = enabled;
            }
        }
    }
}