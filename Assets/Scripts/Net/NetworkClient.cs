using System;
using UnityEngine;
using static PacketSerializer;

public class NetworkClient : MonoBehaviour, NetworkListener {

    public const int DATA_BUFFER_SIZE = 16 * 1024;
    public static int CLIENT_ID = new System.Random().Next();

    private TCP TCPClient;

    public void Start() {
        TCPClient = new TCP(this);
    }

    private void OnApplicationQuit() {
        Disconnect();
    }

    public void ConnectToServer() {
        TCPClient.Connect(0);
    }

    private void Disconnect() {
        TCPClient.Disconnect();
    }

    public void OnTcpConnected() {
        Hook(AC.AcceptLogin.HEADER, (ushort cmd, int size, InPacket packet) => {
            if (packet is AC.AcceptLogin) {

            }
        });
        new CA.Login("danilo", "123456", 10, 10).Send(TCPClient.GetBinaryWriter());
    }

    public void OnUdpConnected() {
        
    }

    public void Hook(ushort cmd, OnPacketReceived onPackedReceived) {
        TCPClient.Hook(cmd, onPackedReceived);
    }

    /**
     * Are we gonna try to reconnect?
     */
    public void OnDisconnected(NetworkProtocol protocol) {
        throw new NotImplementedException();
    }
}