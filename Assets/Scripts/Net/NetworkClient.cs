using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkClient : MonoBehaviour, NetworkListener {

    public const int DATA_BUFFER_SIZE = 4096;
    public static int CLIENT_ID = new System.Random().Next();

    public delegate void PacketHandler(Packet packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private UDP UDPClient;
    private TCP TCPClient;

    private void Start() {
        TCPClient = new TCP(this);
        UDPClient = new UDP();
    }

    public static void HandlePacket(Packet packet) => packetHandlers[packet.ReadInt()](packet);

    private void OnApplicationQuit() {
        Disconnect();
    }

    public void ConnectToServer() {
        TCPClient.Connect(0);
    }

    private void Disconnect() {
        TCPClient.Disconnect();
        UDPClient.Disconnect();
    }

    public void OnTcpConnected(int port) {
        UDPClient.Connect(port);
    }

    public void OnUdpConnected() {
        
    }

    /**
     * Are we gonna try to reconnect?
     */
    public void OnDisconnected(NetworkProtocol protocol) {
        throw new NotImplementedException();
    }
}