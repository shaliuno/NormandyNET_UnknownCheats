using NormandyNET;
using NormandyNET.Helpers;
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

internal class SynchronousSocketClient
{
    private static IPAddress ipAddress;
    private static IPEndPoint remoteEP;
    private static string ipAddressGiven;

    private static byte[] exchangeBuffer = new byte[4096];
    private static Socket socket;

    internal static void InitClient(string _ipAddress)
    {
        ipAddressGiven = _ipAddress;
        IPAddress.TryParse(ipAddressGiven, out ipAddress);
        remoteEP = new IPEndPoint(ipAddress, 11000);

        socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        IAsyncResult result = socket.BeginConnect(remoteEP, null, null);
        bool success = result.AsyncWaitHandle.WaitOne(2000, true);

        if (socket.Connected)
        {
            socket.EndConnect(result);
        }
        else
        {
            socket.Close();
            throw new SocketCanNotConnect(_ipAddress);
        }
    }

    internal static void ShutdownClient()
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close(1000);
        socket.Dispose();
    }

    internal static ulong ProcessOpen(uint pid, string moduleName)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writeBinary = new BinaryWriter(ms))
            {
                writeBinary.Write((byte)RemoteOperation.ProcessOpen);
                writeBinary.Write(pid);
                writeBinary.Write(moduleName.Length);
                writeBinary.Write(Encoding.ASCII.GetBytes(moduleName));
                writeBinary.Close();
                exchangeBuffer = ms.ToArray();
                            }
        }

        try
        {
            var byteSent = socket.Send(exchangeBuffer);

            var bytesReceived = socket.Receive(exchangeBuffer);
        }
        catch (Exception ex)
        {
        }

        ulong imageBaseAddress;

        using (MemoryStream ms = new MemoryStream(exchangeBuffer))
        {
            using (BinaryReader readerBinary = new BinaryReader(ms))
            {
                imageBaseAddress = readerBinary.ReadUInt64();
                readerBinary.Close();
                            }
        }

        return imageBaseAddress;
    }

    internal static void MemoryOperation(ulong address, bool write, ref byte[] buffer)
    {
        Array.Clear(exchangeBuffer, 0, exchangeBuffer.Length);

        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writeBinary = new BinaryWriter(ms))
            {
                writeBinary.Write((byte)RemoteOperation.MemoryOperation);
                writeBinary.Write(buffer.Length);
                writeBinary.Write(address);
                writeBinary.Write(write);
                writeBinary.Write(buffer);
                writeBinary.Close();
                exchangeBuffer = ms.ToArray();
                            }
        }

        try
        {
            var byteSent = socket.Send(exchangeBuffer);

            var bytesReceived = socket.Receive(exchangeBuffer);
        }
        catch
        {
        }

        using (MemoryStream ms = new MemoryStream(exchangeBuffer))
        {
            using (BinaryReader readerBinary = new BinaryReader(ms))
            {
                buffer = readerBinary.ReadBytes(buffer.Length);
                readerBinary.Close();
                            }
        }
    }
}