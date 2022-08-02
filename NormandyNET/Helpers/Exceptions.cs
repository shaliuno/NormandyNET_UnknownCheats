using MetroFramework;
using System;
using System.Windows.Forms;

namespace NormandyNET.Helpers
{
    public static class ExceptionHandler
    {
        public static void HandleExceptionWithExtraText(IWin32Window owner, Exception ex)
        {
            switch (ex)
            {
                case Exception a when a is SocketCanNotConnect:
                    MetroMessageBox.Show(owner, $"{ex.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case Exception a when a is MemoryModuleNotFound:
                    MetroMessageBox.Show(owner, $"{ex.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case Exception a when a is System.Net.Sockets.SocketException:
                    MetroMessageBox.Show(owner, $"{ex.Message}\n" +
                        $"\n" +
                        $"Check if nginx running and tool is injected and working.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default:
                    MetroMessageBox.Show(owner, $"{ex.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }

    internal class MemoryModuleNotFound : Exception
    {
        public MemoryModuleNotFound()
        {
        }

        public MemoryModuleNotFound(string pid)
            : base($"Can not open process." +
                  $"\n" +
                  $"\nMake sure you have entered right PID." +
                  $"\nYou entered: {pid}.")
        {
        }
    }

    internal class SocketCanNotConnect : Exception
    {
        public SocketCanNotConnect()
        {
        }

        public SocketCanNotConnect(string name)
            : base($"Failed to connect." +
                  "Check this:\n" +
                 " Didn't disable all 6 firewalls (3 on each pc)\n" +
                  "Didn't inject TOOL on game PC\n" +
                  "Both pcs do not have the same sub-net(connect both pcs to same router)\n" +
                  "Using wrong server port (check nginx.conf file) \n" +
                  $"Using wrong ipv4 address ({name})\n"
            )
        {
        }
    }

    internal class SocketNotConnected : Exception
    {
        public SocketNotConnected()
        {
        }

        public SocketNotConnected(string name)
            : base($"Socket not connected." +
                  $"\n" +
                  $"\nSomething happenned.")
        {
        }
    }

    internal class OpenGlException : Exception
    {
        public OpenGlException()
        {
        }

        public OpenGlException(string name)
            : base($"Open GL Exceptton: {name}.")
        {
        }
    }
}