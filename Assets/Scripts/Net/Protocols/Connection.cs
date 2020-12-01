using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Connection : NetworkProtocol {

    private TcpClient _Client;

    private NetworkStream _Stream;
    private Packet _ReceivedData;
    private byte[] _ReceivedBuffer;
    private BinaryWriter _BinaryWriter;
    private PacketSerializer _Serializer;

    private NetworkListener networkListener;

    public Connection(NetworkListener networkListener) {
        this.networkListener = networkListener;
        _Serializer = new PacketSerializer();
    }

    override public void Connect(int localPort) {
        _Client = new TcpClient();

        _ReceivedBuffer = new byte[NetworkClient.DATA_BUFFER_SIZE];
        _Client.BeginConnect("127.0.0.1", 6900, OnSocketConnected, _Client);
    }

    public override BinaryWriter GetBinaryWriter() {
        if(_Client.Connected) {
            return _BinaryWriter;
        }

        return null;
    }

    override protected void OnSocketConnected(IAsyncResult _result) {
        _Client.EndConnect(_result);

        if(!_Client.Connected) return;

        _Stream = _Client.GetStream();
        _BinaryWriter = new BinaryWriter(_Stream);
        _ReceivedData = new Packet();
        ListenToStream();
        this.networkListener?.OnTcpConnected();
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
        } catch(Exception e) {
            // Disconnect() ??
            Debug.LogError($"TCP Client Exception {e.Message}");
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

        _Client = new TcpClient();
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

