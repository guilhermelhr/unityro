using System;
using System.IO;
using System.Net.Sockets;

public class Connection {

    public const int DATA_BUFFER_SIZE = 16 * 1024;

    public static System.Action OnDisconnect;

    private TcpClient Client;
    public NetworkStream Stream;
    private BinaryWriter BinaryWriter;
    private PacketSerializer PacketSerializer;
    private byte[] receiveBuffer;

    public Connection() {
        Client = new TcpClient();
        PacketSerializer = new PacketSerializer();
        receiveBuffer = new byte[DATA_BUFFER_SIZE];
    }

    public void Connect(string target, int port) {
        if(Client.Connected)
            Disconnect();

        Client.Connect(target, port);

        Stream = Client.GetStream();
        BinaryWriter = new BinaryWriter(Stream);

        Start();
    }

    public void Start() {
        Client.Client.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, out var err, ReadComplete, null);
    }

    public void SkipBytes(int bytesToSkip) {
        PacketSerializer.BytesToSkip = bytesToSkip;
    }

    public bool IsConnected() => Client.Connected;

    public BinaryWriter GetBinaryWriter() => BinaryWriter;

    private void ReadComplete(IAsyncResult ar) {
        int size = 0;
        SocketError err;
        try {
            size = Client.Client.EndReceive(ar, out err);
        } catch {
            return;
        }

        if(err != SocketError.Success) {
            Disconnect();
            OnDisconnect?.Invoke();
        } else {
            PacketSerializer.EnqueueBytes(receiveBuffer, size);
        }

        Start();
    }

    public void Disconnect() {
        if(Client.Connected) {
            try {
                Client.Close();
                Client.Client.Dispose();
            } catch {

            }

            Client = new TcpClient();
            PacketSerializer.Reset();
        }
    }

    public void Hook(ushort cmd, PacketSerializer.OnPacketReceived onPackedReceived) {
        PacketSerializer.Hook(cmd, onPackedReceived);
    }
}

