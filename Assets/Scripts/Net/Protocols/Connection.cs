using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Connection {

    private TcpClient _client;
    public TcpClient Client {
        get { return _client; }
    }

    private NetworkStream _ns;
    public NetworkStream Stream {
        get { return _ns; }
    }

    private BinaryWriter _bw;
    public BinaryWriter BinaryWriter {
        get { return _bw; }
    }

    private PacketSerializer _pserial;
    public PacketSerializer PacketSerializer {
        get { return _pserial; }
        set { _pserial = value; }
    }

    private byte[] receiveBuffer;

    public Connection() {
        _client = new TcpClient();
        _pserial = new PacketSerializer();
        receiveBuffer = new byte[16 * 1024];
    }

    public void Connect(string target, int port) {
        if(_client.Connected)
            Disconnect();

        _client.Connect(target, port);

        _ns = _client.GetStream();
        _bw = new BinaryWriter(_ns);

        Start();
    }

    public void Start() {
        SocketError err;

        _client.Client.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, out err, ReadComplete, null);
    }

    internal void SkipBytes(int bytesToSkip) {
        _pserial.BytesToSkip = bytesToSkip;
    }

    private void ReadComplete(IAsyncResult ar) {
        SocketError err = SocketError.Success;

        int size = 0;
        try {
            size = _client.Client.EndReceive(ar, out err);
        } catch {
            return; // IAsyncResult bla bla bla
        }

        if(size <= 0 || err != SocketError.Success) {
            //Disconnect
        } else {
            _pserial.EnqueueBytes(receiveBuffer, size);
        }

        Start();
    }

    public void Disconnect() {
        if(_client.Connected) {
            try {
                _client.Close();
                _client.Client.Dispose();
            } catch {

            }

            _client = new TcpClient();
            _pserial.Reset();
        }
    }

    public void Hook(ushort cmd, PacketSerializer.OnPacketReceived onPackedReceived) {
        _pserial.Hook(cmd, onPackedReceived);
    }
}

