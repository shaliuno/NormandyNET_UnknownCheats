using MetroFramework;
using MetroFramework.Components;

namespace NormandyNET.UI
{
    internal static class MetroTheming
    {
        internal static void ApplyThemeAndStyle(ref MetroStyleManager metroStyleManager)
        {
            metroStyleManager.Theme = MetroThemeStyle.Dark;
            metroStyleManager.Style = MetroColorStyle.Teal;
        }
    }
}