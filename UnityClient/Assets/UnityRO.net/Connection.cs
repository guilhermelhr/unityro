using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

public class Connection {

    public const int DATA_BUFFER_SIZE = 16 * 1024;

    public static System.Action OnDisconnect;

    private TcpClient TcpClient;
    private NetworkStream Stream;
    private BinaryWriter BinaryWriter;
    private PacketSerializer PacketSerializer;
    private byte[] receiveBuffer;

    public bool IsConnected() => TcpClient.Connected;
    public BinaryWriter GetBinaryWriter() => BinaryWriter;
    public NetworkStream GetStream() => TcpClient.GetStream();

    public Connection(IPacketHandler packetHandler) {
        TcpClient = new TcpClient();
        PacketSerializer = new PacketSerializer(packetHandler);
        receiveBuffer = new byte[DATA_BUFFER_SIZE];
    }

    public async Task Connect(string target, int port) {
        if(TcpClient.Connected)
            Disconnect();

        await TcpClient.ConnectAsync(target, port);

        Stream = TcpClient.GetStream();
        BinaryWriter = new BinaryWriter(Stream);

        Start();
    }

    public void Start() {
        TcpClient
            .Client
            .BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, out var err, OnReceivedCallback, null);
    }

    public void SkipBytes(int bytesToSkip) {
        PacketSerializer.BytesToSkip = bytesToSkip;
    }

    private void OnReceivedCallback(IAsyncResult ar) {
        int size = 0;
        SocketError err;
        try {
            size = TcpClient.Client.EndReceive(ar, out err);
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
        if(TcpClient.Connected) {
            try {
                TcpClient.Close();
                TcpClient.Client.Dispose();
            } catch {

            }

            TcpClient = new TcpClient();
            Stream = null;
            BinaryWriter = null;
            PacketSerializer.Reset();
        }
    }
}

