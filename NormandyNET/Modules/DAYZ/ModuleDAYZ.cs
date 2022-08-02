using NormandyNET.Core;
using NormandyNET.Modules.DAYZ.UI;
using NormandyNET.UI;

namespace NormandyNET.Modules.DAYZ
{
    internal class ModuleDAYZ
    {
        internal static ReaderDAYZ readerDAYZ;

        internal static SettingsFormDAYZ settingsForm;
        internal static OffsetsDAYZ offsetsDAYZ;
        internal RendererDAYZ rendererDAYZ;
        internal static RadarForm radarForm;

        internal ModuleDAYZ(RadarForm _radarForm)
        {
            radarForm = _radarForm;
            readerDAYZ = new ReaderDAYZ();
            rendererDAYZ = new RendererDAYZ();
            settingsForm = new SettingsFormDAYZ();
            offsetsDAYZ = new OffsetsDAYZ();
        }
    }
}