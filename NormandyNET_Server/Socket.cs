using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

public enum RemoteOperation
{
    ProcessOpen = 100,
    MemoryOperation,
}

public class SynchronousSocketListener
{
    static IPAddress ipAddress;
    static IPEndPoint localEndPoint;

    static byte[] exchangeBuffer = new Byte[4096];
    static byte[] memoryOpBuffer = new byte[32];

    static Socket listener;
    static Socket handler;

    public static void InitServer()
    {
        IPAddress.TryParse("0.0.0.0", out ipAddress);
        localEndPoint = new IPEndPoint(ipAddress, 11000);

        listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listener.NoDelay = true;

        listener.Bind(localEndPoint);
        listener.Listen(10);

        handler = listener.Accept();
    }

    public static void ShutdownServer()
    {
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }

    public static void StartListening()
    {
        try
        {
            while (true)
            {
                handler.Receive(exchangeBuffer);
                var bytesReply = ProcessReceivedData();
                handler.Send(bytesReply);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            handler = listener.Accept();
            StartListening();
        }
    }

    internal static byte[] ProcessReceivedData()
    {
        RemoteOperation operationType;

        using (MemoryStream ms = new MemoryStream(exchangeBuffer))
        {
            using (BinaryReader readerBinary = new BinaryReader(ms))
            {
                operationType = (RemoteOperation)readerBinary.ReadByte();

                if (operationType == 0)
                {
                    throw new ArgumentException("Disconnected");
                }

                if (operationType == RemoteOperation.ProcessOpen)
                {
                    var pid = readerBinary.ReadUInt32();
                    var strLength = readerBinary.ReadInt32();
                    var strByte = readerBinary.ReadBytes(strLength);
                    string strFromBytes = Encoding.ASCII.GetString(strByte);

                    Console.WriteLine($"- {operationType} {pid} {strFromBytes}");

                    Memory.moduleBaseAddress = (ulong)Memory.OpenTargetProcessByPid((int)pid, strFromBytes);

                }

                if (operationType == RemoteOperation.MemoryOperation)
                {
                    var size = readerBinary.ReadInt32();
                    var address = readerBinary.ReadUInt64();
                    var write = readerBinary.ReadBoolean();
                    if (write)
                    {
                        memoryOpBuffer = readerBinary.ReadBytes(size);
                    }
                    else
                    {
                        memoryOpBuffer = new byte[size];
                    }
                   
                    if (write)
                    {
                        Memory.WriteBytes((long)address, memoryOpBuffer);
                    }
                    else
                    {
                        memoryOpBuffer = Memory.ReadBytes((long)address, size);
                    }
                }
                readerBinary.Close();
            }
        }

        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writeBinary = new BinaryWriter(ms))
            {
                if (operationType == RemoteOperation.ProcessOpen)
                {
                    writeBinary.Write(Memory.moduleBaseAddress);
                }

                if (operationType == RemoteOperation.MemoryOperation)
                {
                    writeBinary.Write(memoryOpBuffer);
                }

                writeBinary.Close();
                return ms.ToArray();
            }
        }


    }

}