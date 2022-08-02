using NormandyNET.Core;

namespace NormandyNET.Modules.RUST
{
    internal class OnScreedDisplayRUST : OSD
    {
        #region Internal Fields

        internal int countHumans = 0;
        internal int countAI = 0;
        internal int countEntities = 0;

        #endregion Internal Fields

        #region Internal Methods

        internal override bool AddDateTime()
        {
            return ModuleRUST.settingsForm.settingsJson.OnScreenDisplay.DateTime;
        }

        internal override bool AddFPS()
        {
            return ModuleRUST.settingsForm.settingsJson.OnScreenDisplay.FPS;
        }

        internal override void AddOSD()
        {
            renderItem = new RenderItem();
            renderItem.Text = osdText.ToString();
            renderItem.TextOverlayOutline = true;

            renderItem.MapPosX = -(ModuleRUST.radarForm.GetOpenGlControlSize.Width / 2) + 10;
            renderItem.MapPosZ = (ModuleRUST.radarForm.GetOpenGlControlSize.Height / 2) - 50;
            renderItem.DrawColor = ModuleRUST.settingsForm.settingsJson.Colors.ColorOSD;
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
            return ModuleRUST.settingsForm.settingsJson.OnScreenDisplay.Stats;
        }

        internal override void RenderOSDExtras()
        {
            osdText.Clear();

            osdText.Append($"©{CommonHelpers.ColorHexConverter(ModuleRUST.settingsForm.settingsJson.Colors.ColorOSD)}Visible Players - {Pointers.EntityCountPlayersOnly}");
            osdText.Append($"     ©{CommonHelpers.ColorHexConverter(ModuleRUST.settingsForm.settingsJson.Colors.ColorOSD)}Entities - {Pointers.EntityCountAll}");

            renderItem = new RenderItem();
            renderItem.Text = osdText.ToString();
            renderItem.TextOverlayOutline = true;
            renderItem.MapPosX = renderItem.MapPosX = -200;
            renderItem.MapPosZ = renderItem.MapPosZ = (ModuleRUST.radarForm.GetOpenGlControlSize.Height / 2);
            renderItem.DrawColor = ModuleRUST.settingsForm.settingsJson.Colors.ColorOSD;
            renderItem.Size = (int)FontSizes.name;
            renderItem.renderLayer = RenderLayers.OSD;
            OpenGL.MapText.Add(renderItem);
        }

        internal void ResetCounters()
        {
            countHumans = 0;
            countAI = 0;
            countEntities = 0;
        }

        #endregion Internal Methods
    }
}