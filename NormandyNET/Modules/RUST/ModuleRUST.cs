using NormandyNET.Modules.RUST.UI;
using NormandyNET.UI;

namespace NormandyNET.Modules.RUST
{
    internal class ModuleRUST
    {
        internal static ReaderRUST readerRUST;

        internal static SettingsFormRUST settingsForm;
        internal static OffsetsRUST offsetsRUST;
        internal RendererRUST rendererRUST;
        internal static RadarForm radarForm;

        internal ModuleRUST(RadarForm _radarForm)
        {
            radarForm = _radarForm;
            offsetsRUST = new OffsetsRUST();
            readerRUST = new ReaderRUST();
            rendererRUST = new RendererRUST();
            settingsForm = new SettingsFormRUST();
        }
    }
}