using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDP : NetworkProtocol {

    public UdpClient Socket;
    public IPEndPoint Endpoint;

    public UDP() {
        Endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6900);
    }

    override public void Connect(int localPort) {
        Socket = new UdpClient(localPort);

        Socket.Connect(Endpoint);
        Socket.BeginReceive(OnDataReceived, null);

        using(Packet packet = new Packet()) {
            SendData(packet);
        }
    }

    override protected void SendData(Packet packet) {
        try {
            packet.InsertInt(NetworkClient.CLIENT_ID);
            if(Socket != null) {
                Socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
            }
        } catch(Exception ex) {
            Debug.LogError($"Error sending data to server via UDP: {ex}");
        }
    }

    override protected void OnDataReceived(IAsyncResult result) {
        try {
            byte[] _data = Socket.EndReceive(result, ref Endpoint);
            Socket.BeginReceive(OnDataReceived, null);

            if(_data.Length < 4) {
                Debug.LogWarning("UDP Client Received 0 Len message");
                return;
            }

            HandleData(_data);
        } catch(Exception ex) {
            Debug.LogError($"Error sending data to server via UDP: {ex}");
        }
    }

    override protected bool HandleData(byte[] _data) {
        using(Packet _packet = new Packet(_data)) {
            int _packetLength = _packet.ReadInt();
            _data = _packet.ReadBytes(_packetLength);
        }

        ThreadManager.ExecuteOnMainThread(() =>
        {
            using(Packet _packet = new Packet(_data)) {
                NetworkClient.HandlePacket(_packet);
            }
        });

        return false;
    }

    override public void Disconnect() {
        Socket.Close();

        Endpoint = null;
        Socket = null;
    }

    protected override void OnSocketConnected(IAsyncResult result) {}
}

