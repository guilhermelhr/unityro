using System.IO;
using UnityEngine;
using static PacketSerializer;

public class NetworkClient : MonoBehaviour {

    public struct NetworkClientState {
        public MapLoginInfo MapLoginInfo;
        public CharServerInfo CharServer;
        public CharacterData SelectedCharacter;
        public AC.ACCEPT_LOGIN LoginInfo;
        public HC.ACCEPT_ENTER CurrentCharactersInfo;
    }

    public static int CLIENT_ID = new System.Random().Next();

    public Connection CurrentConnection;
    public NetworkClientState State;

    public void Start() {
        CurrentConnection = new Connection();
        State = new NetworkClientState();
    }

    private void OnApplicationQuit() {
        Disconnect();
    }

    public void ChangeServer(string ip, int port) {
        CurrentConnection.Connect(ip, port);
    }

    public void Disconnect() {
        CurrentConnection.Disconnect();
    }

    public bool IsConnected => CurrentConnection.IsConnected();

    public void HookPacket(PacketHeader cmd, OnPacketReceived onPackedReceived) {
        CurrentConnection?.Hook((ushort)cmd, onPackedReceived);
    }

    public void SkipBytes(int bytesToSkip) {
        CurrentConnection.SkipBytes(bytesToSkip);
    }

    public void Ping() {
        if(!IsConnected) return;
        var ticks = Time.realtimeSinceStartup;
        if(ticks % 12 < 1f) {
            new Ping((int)Time.realtimeSinceStartup).Send(CurrentConnection.GetBinaryWriter());
        }
    }

    public BinaryWriter GetBinaryWriter() => CurrentConnection.GetBinaryWriter();
}