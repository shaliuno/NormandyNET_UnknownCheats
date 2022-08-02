using MetroFramework.Forms;
using NormandyNET.Settings;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NormandyNET.UI
{
    internal partial class StartupForm : MetroForm
    {
        internal string filenamePassword = "............................................";
        internal string appHashLocal;

        public Assembly radar;
        public int radarModuleSize;
        public bool radarReady;
        private string branch;
        internal Version appVersion = Assembly.GetEntryAssembly().GetName().Version;
        internal Version minVersion = new Version(0, 0, 0, 0);
        internal Version onsiteVersion = new Version(0, 0, 0, 0);
        internal static Version appVersionExt;
        private System.Timers.Timer heartbeatTimer;

        public event EventHandler OnDownloadAvailable;

        private RadarForm mainForm;
        internal static string gameType = "EFT";

        internal bool canRun = true;

        internal static string GetProduct_VMP()
        {
            switch (SettingsRadar.GetGameType_VMP())
            {
                case SettingsRadar.GameType.eft:
                    return "radar_eft";
                    break;

                case SettingsRadar.GameType.dayz:
                    return "radar_dayz";
                    break;

                case SettingsRadar.GameType.pubg:
                    return "radar_pubg";
                    break;

                case SettingsRadar.GameType.rust:
                    return "radar_rust";
                    break;

                case SettingsRadar.GameType.arma:
                    return "radar_arma";
                    break;

                default:
                    return "radar_eft";
                    break;
            }
        }

        internal static Version GetVersion()
        {
            return appVersionExt;
        }

        private string GetAppHashLocal()
        {
            return CommonHelpers.GetFileHashMD5(Assembly.GetEntryAssembly().Location);
        }

        public StartupForm(string[] args)
        {
            if (DebugClass.Debug)
            {
                CustomConsole.CreateConsole_VMP();
            }

            InitializeComponent();
            this.StyleManager = this.metroStyleManager;
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
            appVersionExt = appVersion;
            this.Text = $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}.x";
            metroLabelVersion.Text = $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}.x";
            GetCustomLogo();

            appHashLocal = GetAppHashLocal();
            mainForm = new RadarForm(this);
        }

        private void GetCustomLogo()
        {
            var logoFile = @"Logo.png";
            if (File.Exists(logoFile))
            {
                pictureBox1.Image = Image.FromFile(logoFile);
            }
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        private void ShownForm(object sender, EventArgs e)
        {
            SetProcessPriority();
            Helpers.WebClientHelper.InitWebClientHelper_VMP();
            Task.Run(() => FinalCheck());
        }

        private void FinalCheck()
        {
            Thread.Sleep(2500);

            this.Invoke((MethodInvoker)delegate ()
            {
                mainForm.Show();
            });

            this.Invoke(new MethodInvoker(this.Hide));
        }

        private void SetProcessPriority()
        {
            using (Process p = Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.High;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}