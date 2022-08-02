using NormandyNET.Modules.EFT.UI;
using NormandyNET.UI;

namespace NormandyNET.Modules.EFT
{
    internal class ModuleEFT
    {
        internal static ReaderEFT readerEFT;

        internal static SettingsFormEFT settingsForm;
        internal static OffsetsEFT offsetsEFT;
        internal RendererEFT rendererEFT;
        internal static RadarForm radarForm;

        internal ModuleEFT(RadarForm _radarForm)
        {
            radarForm = _radarForm;
            readerEFT = new ReaderEFT();
            rendererEFT = new RendererEFT();
            settingsForm = new SettingsFormEFT();
            offsetsEFT = new OffsetsEFT();
        }
    }
}