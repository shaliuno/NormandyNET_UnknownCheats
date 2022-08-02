using NormandyNET.UI;
using System;

namespace NormandyNET.Settings
{
    [Serializable]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class SettingsRadar
    {
        public enum GameType
        {
            eft,
            dayz,
            rust,
            pubg,
            ark,
            arma
        }

        internal static GameType GetGameType_VMP()
        {
            switch (StartupForm.gameType)
            {
                case string a when a.Contains("EFT"):
                    return GameType.eft;

                case string a when a.Contains("DAYZ"):
                    return GameType.dayz;

                case string a when a.Contains("RUST"):
                    return GameType.rust;

                case string a when a.Contains("ARMA"):
                    return GameType.arma;
            }

            return GameType.eft;
        }

        public UserInterfaceSetting UserInterface = new UserInterfaceSetting();
        public NetworkSetting Network = new NetworkSetting();
        public DebugSetting Debug = new DebugSetting();

        public SettingsRadar()
        {
            FillDefaults();
        }

        private void FillDefaults()
        {
            UserInterface.WindowDimensions.Height = 660;
            UserInterface.WindowDimensions.Width = 660;

            UserInterface.WindowLocation.X = -32000;
            UserInterface.WindowLocation.Y = -32000;
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class NetworkSetting
        {
            public string ServerAddress = "enter IP";
            public int ServerPort = 9999;
            public uint lastGameProcessID = 0;
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class DebugSetting
        {
            public bool TraceLogEnabled = false;
            public bool OpenGlDebugEnabled = false;
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public class UserInterfaceSetting
        {
            public bool ShowUI = true;

            public WindowDimensionsSetting WindowDimensions = new WindowDimensionsSetting();
            public WindowsLocationSetting WindowLocation = new WindowsLocationSetting();
            public bool FullScreen = false;
            public float Opacity = 100;
            public bool TopMost = false;

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public struct WindowDimensionsSetting
            {
                public int Height;
                public int Width;
            }

            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            public struct WindowsLocationSetting
            {
                public int X;
                public int Y;
            }
        }
    }
}