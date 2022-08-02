using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormandyNET_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SynchronousSocketListener.InitServer();
            SynchronousSocketListener.StartListening();
            SynchronousSocketListener.ShutdownServer();
        }


    }
}
