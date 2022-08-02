using Microsoft.SqlServer.MessageBox;
using NDepend.Product.ErrorHandling;
using NormandyNET.UI;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Security.Principal;
using System.Windows.Forms;

namespace NormandyNET
{
    internal static class Program
    {
        [STAThread]
        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        private static void Main(string[] args)
        {
            EmbeddedAssembly.Load("NormandyNET.DLL.Microsoft.ExceptionMessageBox.dll", "Microsoft.ExceptionMessageBox.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.MetroFramework.dll", "MetroFramework.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.MetroFramework.Design.dll", "MetroFramework.Design.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.MetroFramework.Fonts.dll", "MetroFramework.Fonts.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.Newtonsoft.Json.dll", "Newtonsoft.Json.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.netstandard.dll", "netstandard.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.OpenTK.dll", "OpenTK.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.OpenTK.GLControl.dll", "OpenTK.GLControl.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.QuickFont.dll", "QuickFont.dll");

            EmbeddedAssembly.Load("NormandyNET.DLL.System.Runtime.CompilerServices.Unsafe.dll", "System.Runtime.CompilerServices.Unsafe.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.Mono.Simd.dll", "Mono.Simd.dll");

            EmbeddedAssembly.Load("NormandyNET.DLL.UnityEngine.dll", "UnityEngine.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.UnityEngine.CoreModule.dll", "UnityEngine.CoreModule.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.UnityEngine.Networking.dll", "UnityEngine.Networking.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.UnityEngine.SharedInternalsModule.dll", "UnityEngine.SharedInternalsModule.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.zlib.net.dll", "zlib.net.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.TinyCsvParser.dll", "TinyCsvParser.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.Cyotek.Drawing.BitmapFont.dll", "Cyotek.Drawing.BitmapFont.dll");

            EmbeddedAssembly.Load("NormandyNET.DLL.SharpDX.dll", "SharpDX.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.SharpDX.Direct2D1.dll", "SharpDX.Direct2D1.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.SharpDX.DXGI.dll", "SharpDX.DXGI.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.SharpDX.Mathematics.dll", "SharpDX.Mathematics.dll");
            EmbeddedAssembly.Load("NormandyNET.DLL.NormandyNET_InheritedForms.dll", "NormandyNET_InheritedForms.dll");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(OnThreadException);
            StartMainApp(args);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void StartMainApp(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartupForm(args));
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var str = StackTraceHelper.FormatUnlocalizedStackTraceWithILOffset(e.ExceptionObject as Exception);
            var exc = e.ExceptionObject as Exception;

            Program.ShowException($"{Assembly.GetEntryAssembly().GetName().Version}\n\n---\n\n{exc.Message}\n\n---\n\n{str}\n\n---OnUnhandledException");
        }

        private static void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs args)
        {
            var str = StackTraceHelper.FormatUnlocalizedStackTraceWithILOffset(args.Exception);
            var exc = args.Exception;

            Program.ShowException($"{Assembly.GetEntryAssembly().GetName().Version}\n\n---\n\n{exc.Message}\n\n---\n\n{str}\n\n---OnThreadException");
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void ShowException(Exception ex)
        {
            var msg = new ExceptionMessageBox(ex)
            {
                Beep = false,
                ShowToolBar = true,
                Symbol = ExceptionMessageBoxSymbol.Error
            };
            msg.Show(null);
        }

        public static void ShowException(string exceptionText)
        {
            var msg = new ExceptionMessageBox(exceptionText)
            {
                Beep = false,
                ShowToolBar = true,
                Symbol = ExceptionMessageBoxSymbol.Error
            };
            msg.Show(null);
        }
    }
}