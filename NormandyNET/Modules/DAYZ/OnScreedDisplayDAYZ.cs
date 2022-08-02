using NormandyNET.Core;

namespace NormandyNET.Modules.DAYZ
{
    internal class OnScreedDisplayDAYZ : OSD
    {
        #region Internal Fields

        internal int countEntityFar = 0;
        internal int countEntityFast = 0;
        internal int countEntityItems = 0;
        internal int countEntityNear = 0;
        internal int countEntitySlow = 0;

        #endregion Internal Fields

        #region Internal Methods

        internal override bool AddDateTime()
        {
            return ModuleDAYZ.settingsForm.settingsJson.OnScreenDisplay.DateTime;
        }

        internal override bool AddFPS()
        {
            return ModuleDAYZ.settingsForm.settingsJson.OnScreenDisplay.FPS;
        }

        internal override void AddOSD()
        {
            renderItem = new RenderItem();
            renderItem.Text = osdText.ToString();
            renderItem.TextOverlayOutline = true;

            renderItem.MapPosX = -(ModuleDAYZ.radarForm.GetOpenGlControlSize.Width / 2) + 10;
            renderItem.MapPosZ = (ModuleDAYZ.radarForm.GetOpenGlControlSize.Height / 2) - 50;
            renderItem.DrawColor = ModuleDAYZ.settingsForm.settingsJson.Colors.ColorOSD;
            renderItem.Size = (int)FontSizes.large;
            renderItem.renderLayer = RenderLayers.OSD;
            OpenGL.MapText.Add(renderItem);
        }

        internal override void AddPostText()
        {
        }

        internal override void AddPreText()
        {
        }

        internal override bool DisplayStats()
        {
            return ModuleDAYZ.settingsForm.settingsJson.OnScreenDisplay.Stats;
        }

        internal override void RenderOSDExtras()
        {
            osdText.Clear();

            osdText.Append($"©{CommonHelpers.ColorHexConverter(ModuleDAYZ.settingsForm.settingsJson.Colors.ColorOSD)}Near - {countEntityNear}");
            osdText.Append($"     ©{CommonHelpers.ColorHexConverter(ModuleDAYZ.settingsForm.settingsJson.Colors.ColorOSD)}Far - {countEntityFar}");
            osdText.Append($"     ©{CommonHelpers.ColorHexConverter(ModuleDAYZ.settingsForm.settingsJson.Colors.ColorOSD)}Slow - {countEntitySlow}");
            osdText.Append($"     ©{CommonHelpers.ColorHexConverter(ModuleDAYZ.settingsForm.settingsJson.Colors.ColorOSD)}Fast - {countEntityFast}");
            osdText.Append($"     ©{CommonHelpers.ColorHexConverter(ModuleDAYZ.settingsForm.settingsJson.Colors.ColorOSD)}Items - {countEntityItems}");

            renderItem = new RenderItem();
            renderItem.Text = osdText.ToString();
            renderItem.TextOverlayOutline = true;
            renderItem.MapPosX = renderItem.MapPosX = -200;
            renderItem.MapPosZ = renderItem.MapPosZ = (ModuleDAYZ.radarForm.GetOpenGlControlSize.Height / 2);
            renderItem.DrawColor = ModuleDAYZ.settingsForm.settingsJson.Colors.ColorOSD;
            renderItem.Size = (int)FontSizes.name;
            renderItem.renderLayer = RenderLayers.OSD;
            OpenGL.MapText.Add(renderItem);
        }

        internal void ResetCounters()
        {
            countEntityNear = 0;
            countEntityFar = 0;
            countEntitySlow = 0;
            countEntityFast = 0;
            countEntityItems = 0;
        }

        #endregion Internal Methods
    }
}