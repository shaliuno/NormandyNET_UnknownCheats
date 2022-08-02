using NormandyNET.Core;
using NormandyNET.Settings;
using System.Drawing;

namespace NormandyNET.Modules.EFT
{
    internal class OnScreedDisplayEFT : OSD
    {
        #region Internal Fields

        internal int countBot = 0;
        internal int countBotElite = 0;
        internal int countPmc = 0;
        internal int countPmcThermals = 0;
        internal int countScav = 0;

        #endregion Internal Fields

        #region Internal Methods

        internal override bool AddDateTime()
        {
            return ModuleEFT.settingsForm.settingsJson.OnScreenDisplay.DateTime;
        }

        internal override bool AddFPS()
        {
            return ModuleEFT.settingsForm.settingsJson.OnScreenDisplay.FPS;
        }

        internal override void AddOSD()
        {
            renderItem = new RenderItem();
            renderItem.Text = osdText.ToString();
            renderItem.TextOverlayOutline = true;
            renderItem.MapPosX = -(ModuleEFT.radarForm.GetOpenGlControlSize.Width / 2) + 10;
            renderItem.MapPosZ = (ModuleEFT.radarForm.GetOpenGlControlSize.Height / 2) - 50;
            renderItem.DrawColor = ModuleEFT.settingsForm.settingsJson.Colors.ColorOSD;
            renderItem.Size = (int)FontSizes.large;
            renderItem.renderLayer = RenderLayers.OSD;
            OpenGL.MapText.Add(renderItem);

            if (ModuleEFT.radarForm.Started == false)
            {
                renderItem = new RenderItem();
                renderItem.Text = "make EFT great again";
                renderItem.TextOverlayOutline = true;
                renderItem.CenterText = true;

                renderItem.MapPosX = 0;
                renderItem.MapPosZ = 0;
                renderItem.DrawColor = ModuleEFT.settingsForm.settingsJson.Colors.ColorOSD;
                renderItem.Size = (int)FontSizes.large2;
                renderItem.renderLayer = RenderLayers.OSD;
                OpenGL.MapText.Add(renderItem);

                renderItem = new RenderItem();
                renderItem.TextOverlayOutline = true;
                renderItem.CenterText = true;
                renderItem.MapPosX = 0;
                renderItem.MapPosZ = 25;

                //if (!Offsets.applied)
                //{
                //    renderItem.Text = "Initializing";
                //    renderItem.DrawColor = Color.Red;
                //}
                //else
                //{
                //    renderItem.Text = "Initialized";
                //    renderItem.DrawColor = Color.Lime;
                //}

                renderItem.Text = "";
                renderItem.DrawColor = Color.Lime;

                renderItem.Size = (int)FontSizes.large2;
                renderItem.renderLayer = RenderLayers.OSD;
                OpenGL.MapText.Add(renderItem);
            }
        }

        internal override void AddPostText()
        {
            Color gameWorldStatusColor;
            gameWorldStatusColor = GameWorld.address != 0 ? Color.Green : Color.Red;

            Color fpsCameraStatusColor;
            if (ModuleEFT.readerEFT.fpsCamera == null || ModuleEFT.readerEFT.fpsCamera?.address == 0)
            {
                fpsCameraStatusColor = Color.Red;
            }
            else
            {
                fpsCameraStatusColor = Color.Green;
            }

            osdText.Append($"©{CommonHelpers.ColorHexConverter(gameWorldStatusColor)}Gameworld").AppendLine();
            osdText.Append($"©{CommonHelpers.ColorHexConverter(fpsCameraStatusColor)}FPS Camera").AppendLine();

            if (!ModuleEFT.readerEFT.staticLootDone && !ModuleEFT.settingsForm.settingsJson.Loot.LiveLoot)
            {
                osdText.Append($"©{CommonHelpers.ColorHexConverter(Color.Red)}Updating Loot.").AppendLine();
            }

            if (DebugClass.Debug && Memory.SlowMode)
            {
                osdText.Append($"©{CommonHelpers.ColorHexConverter(Color.Red)}Memory SlowMode").AppendLine();
            }
        }

        internal override void AddPreText()
        {
        }

        internal override bool DisplayStats()
        {
            return ModuleEFT.settingsForm.settingsJson.OnScreenDisplay.Stats;
        }

        internal override void RenderOSDExtras()
        {
            osdText.Clear();

            osdText.Append($"©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.PMC)}PMC - {countPmc}");
            osdText.Append($"     ©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotHuman)}Scav - {countScav}");
            osdText.Append($"     ©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.Bot)}Bot - {countBot}");
            osdText.Append($"     ©{CommonHelpers.ColorHexConverter(ModuleEFT.settingsForm.settingsJson.Colors.EntityColors.BotElite)}Bot Elite - {countBotElite}").AppendLine();

            if (ModuleEFT.radarForm.Started == true)
            {
                var localplayer = ReaderEFT.GetLocalPlayer();

                if (localplayer != null && localplayer.improvementsController != null && localplayer.improvementsController.ShowOSDWarning(out string str))
                {
                    osdText.Append($"©{CommonHelpers.ColorHexConverter(Color.Red)}{str}");
                }
            }

            renderItem = new RenderItem();
            renderItem.Text = osdText.ToString();
            renderItem.TextOverlayOutline = true;
            renderItem.MapPosX = renderItem.MapPosX - 200;
            renderItem.MapPosZ = (ModuleEFT.radarForm.GetOpenGlControlSize.Height / 2);
            renderItem.DrawColor = ModuleEFT.settingsForm.settingsJson.Colors.ColorOSD;
            renderItem.Size = (int)FontSizes.name;
            renderItem.renderLayer = RenderLayers.OSD;
            OpenGL.MapText.Add(renderItem);
        }

        internal void ResetCounters()
        {
            countScav = 0;
            countPmc = 0;
            countPmcThermals = 0;
            countBot = 0;
            countBotElite = 0;
        }

        #endregion Internal Methods
    }
}