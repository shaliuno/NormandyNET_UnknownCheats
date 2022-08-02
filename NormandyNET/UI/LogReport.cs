using MetroFramework.Forms;
using System.Net;

namespace NormandyNET.UI
{
    public partial class LogReport : MetroForm
    {
        public WebClient wc;
        private string logurl;

        public LogReport()
        {
            InitializeComponent();

            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
        }
    }
}