using NormandyNET.Core;
using NormandyNET.Modules.ARMA.UI;
using NormandyNET.UI;

namespace NormandyNET.Modules.ARMA
{
    internal class ModuleARMA
    {
        internal static ReaderARMA readerARMA;

        internal static SettingsFormARMA settingsForm;
        internal static OffsetsARMA offsetsARMA;
        internal RendererARMA rendererARMA;
        internal static RadarForm radarForm;

        internal ModuleARMA(RadarForm _radarForm)
        {
            radarForm = _radarForm;
            readerARMA = new ReaderARMA();
            rendererARMA = new RendererARMA();
            settingsForm = new SettingsFormARMA();
            offsetsARMA = new OffsetsARMA();
        }
    }
}