using System.Collections;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using static PacketSerializer;

public class NetworkClient : MonoBehaviour {

    public struct NetworkClientState {
        public MapLoginInfo MapLoginInfo;
        public CharServerInfo CharServer;
        public CharacterData SelectedCharacter;
        public AC.ACCEPT_LOGIN3 LoginInfo;
        public HC.ACCEPT_ENTER CurrentCharactersInfo;
    }

    public static int CLIENT_ID = new System.Random().Next();

    public static NetworkClient Instance { get; private set; }
    public Connection CurrentConnection;
    public NetworkClientState State;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void Start() {
        CurrentConnection = new Connection();
        State = new NetworkClientState();
    }

    private void OnDestroy() {
        Instance = null;
    }

    private void OnApplicationQuit() {
        Disconnect();
    }

    public void ChangeServer(string ip, int port) {
        CurrentConnection.Connect(ip, port);
    }

    public void StartHeatBeat() {
        StartCoroutine(ServerHeartBeat());
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

    public BinaryWriter GetBinaryWriter() => CurrentConnection.GetBinaryWriter();

    public NetworkStream GetStream() => CurrentConnection.GetStream();

    private IEnumerator ServerHeartBeat() {
        for(; ;) {
            // TODO check if connection is still alive. If not, disconnect client
            new CZ.REQUEST_TIME2().Send();
            yield return new WaitForSeconds(10f);
        }
    }
}