using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Connection : NetworkProtocol {

    private TcpClient _Client;

    private NetworkStream _Stream;
    private byte[] _ReceivedBuffer;
    private BinaryWriter _BinaryWriter;
    private PacketSerializer _Serializer;

    private NetworkListener networkListener;

    public Connection(NetworkListener networkListener) {
        this.networkListener = networkListener;
        _Serializer = new PacketSerializer();
    }

    override public void Connect() {
        _Client = new TcpClient() {
            ReceiveBufferSize = NetworkClient.DATA_BUFFER_SIZE,
            SendBufferSize = NetworkClient.DATA_BUFFER_SIZE
        };

        _ReceivedBuffer = new byte[NetworkClient.DATA_BUFFER_SIZE];
        _Client.Connect("127.0.0.1", 6900);
        OnClientConnected();
    }

    public override void Connect(IPAddress host, int port) {
        _Client = new TcpClient() {
            ReceiveBufferSize = NetworkClient.DATA_BUFFER_SIZE,
            SendBufferSize = NetworkClient.DATA_BUFFER_SIZE
        };
        _ReceivedBuffer = new byte[NetworkClient.DATA_BUFFER_SIZE];
        _Client.Connect(host, port);
        OnClientConnected();
    }

    public bool IsConnected => _Client.Connected;

    override public void Connect(string ip, int port) {
        _Client = new TcpClient() {
            ReceiveBufferSize = NetworkClient.DATA_BUFFER_SIZE,
            SendBufferSize = NetworkClient.DATA_BUFFER_SIZE
        };
        _ReceivedBuffer = new byte[NetworkClient.DATA_BUFFER_SIZE];
        _Client.Connect(ip, port);
        OnClientConnected();
    }

    override public BinaryWriter GetBinaryWriter() {
        if(_Client.Connected) {
            return _BinaryWriter;
        }

        return null;
    }

    override protected void OnClientConnected() {
        if(!_Client.Connected) return;

        _Stream = _Client.GetStream();
        _BinaryWriter = new BinaryWriter(_Stream);
        ListenToStream();
    }

    private void ListenToStream() {
        SocketError err;
        _Client.Client.BeginReceive(_ReceivedBuffer, 0, _ReceivedBuffer.Length, SocketFlags.None, out err, OnDataReceived, null);
    }

    override protected void OnDataReceived(IAsyncResult result) {
        SocketError error = SocketError.Success;
        int size = 0;
        try {
            size = _Client.Client.EndReceive(result, out error);
        } catch {
            // Disconnect() ??
            Debug.LogError($"TCP Client Exception");
            return;
        }

        if(size <= 0 || error != SocketError.Success) {
            Disconnect();
            Debug.LogWarning($"TCP Client Response size is {size} ({error})");
        } else {
            _Serializer.EnqueueBytes(_ReceivedBuffer, size);
        }

        ListenToStream();
    }

    override public void Disconnect() {
        if(_Client?.Connected == true) {
            try {
                _Client.Close();
                _Client.Client.Dispose();
            } catch { }
        }

        _Client = new TcpClient() {
            ReceiveBufferSize = NetworkClient.DATA_BUFFER_SIZE,
            SendBufferSize = NetworkClient.DATA_BUFFER_SIZE
        };
        _Serializer.Reset();
        //_Stream.Close();
        //_Stream.Dispose();
        //_Client.Close();

        //_Stream = null;
        //_ReceivedData = null;
        //_ReceivedBuffer = null;
        //_Client = null;
    }

    public void Hook(ushort cmd, PacketSerializer.OnPacketReceived onPackedReceived) {
        _Serializer.Hook(cmd, onPackedReceived);
    }
}

