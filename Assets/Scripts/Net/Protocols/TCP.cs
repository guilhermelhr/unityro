using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TCP : NetworkProtocol {

    public TcpClient Socket;

    private NetworkStream _Stream;
    private Packet _ReceivedData;
    private byte[] _ReceivedBuffer;

    private NetworkListener networkListener;

    public TCP(NetworkListener networkListener) {
        this.networkListener = networkListener;
    }

    override public void Connect(int localPort) {
        Socket = new TcpClient {
            ReceiveBufferSize = NetworkClient.DATA_BUFFER_SIZE,
            SendBufferSize = NetworkClient.DATA_BUFFER_SIZE
        };

        _ReceivedBuffer = new byte[NetworkClient.DATA_BUFFER_SIZE];
        Socket.BeginConnect("127.0.0.1", 6900, OnSocketConnected, Socket);
    }

    override protected void SendData(Packet packet) {
        try {
            if(Socket != null) {
                _Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
        } catch(Exception e) {
            Debug.LogError($"Error sending data to server via TCP: {e}");
        }
    }

    override protected void OnSocketConnected(IAsyncResult _result) {
        Socket.EndConnect(_result);

        if(!Socket.Connected) return;

        _Stream = Socket.GetStream();
        _ReceivedData = new Packet();
        _Stream.BeginRead(_ReceivedBuffer, 0, NetworkClient.DATA_BUFFER_SIZE, OnDataReceived, null);
        this.networkListener?.OnTcpConnected(((IPEndPoint)Socket.Client.LocalEndPoint).Port);
    }

    override protected void OnDataReceived(IAsyncResult result) {
        try {
            int byteLen = _Stream.EndRead(result);
            if(byteLen <= 0) {
                Debug.LogWarning("TCP Client Received 0 Len message");
                return;
            }

            byte[] data = new byte[byteLen];
            Array.Copy(_ReceivedBuffer, data, byteLen);

            _ReceivedData.Reset(HandleData(data));
            _Stream.BeginRead(_ReceivedBuffer, 0, NetworkClient.DATA_BUFFER_SIZE, OnDataReceived, null);
        } catch(Exception e) {
            // Disconnect() ??
            Debug.LogError($"TCP Client Exception {e.Message}");
        }
    }

    override protected bool HandleData(byte[] data) {
        int _packetLength = 0;

        _ReceivedData.SetBytes(data);

        if(_ReceivedData.UnreadLength() >= 4) {
            _packetLength = _ReceivedData.ReadInt();
            if(_packetLength <= 0) {
                return true;
            }
        }

        while(_packetLength > 0 && _packetLength <= _ReceivedData.UnreadLength()) {
            byte[] _packetBytes = _ReceivedData.ReadBytes(_packetLength);
            ThreadManager.ExecuteOnMainThread(() => {
                using(Packet _packet = new Packet(_packetBytes)) {
                    NetworkClient.HandlePacket(_packet);
                }
            });

            _packetLength = 0;
            if(_ReceivedData.UnreadLength() >= 4) {
                _packetLength = _ReceivedData.ReadInt();
                if(_packetLength <= 0) {
                    return true;
                }
            }
        }

        if(_packetLength <= 1) {
            return true;
        }

        return false;
    }

    override public void Disconnect() {
        _Stream.Close();
        _Stream.Dispose();
        Socket.Close();

        _Stream = null;
        _ReceivedData = null;
        _ReceivedBuffer = null;
        Socket = null;
    }
}

