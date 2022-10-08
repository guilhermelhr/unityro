using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static PacketSerializer;

public class NetworkClient : MonoBehaviour, IPacketHandler {

    public static UnityAction<NetworkPacket, bool> OnPacketEvent;

    #region Singleton
    private static NetworkClient _instance;
    private static NetworkClient Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<NetworkClient>();
            }

            return _instance;
        }
    }
    #endregion

    #region Members
    public bool IsConnected => CurrentConnection.IsConnected();
    public static int CLIENT_ID = new System.Random().Next();

    private Dictionary<PacketHeader, OnPacketReceived> PacketHooks { get; set; } = new Dictionary<PacketHeader, OnPacketReceived>();

    private bool IsPaused = false;

    public NetworkClientState State;
    public Connection CurrentConnection;

    private Queue<OutPacket> OutPacketQueue;
    private Queue<InPacket> InPacketQueue;
    #endregion

    #region Lifecycle
    private void Awake() {
        DontDestroyOnLoad(this);
    }

    public void Start() {
        CurrentConnection = new Connection(this);
        State = new NetworkClientState();

        OutPacketQueue = new Queue<OutPacket>();
        InPacketQueue = new Queue<InPacket>();
    }

    private void Update() {
        if (IsPaused) {
            return;
        }
        TrySendPacket();
        TryHandleReceivedPacket();
    }

    private void OnApplicationQuit() {
        Disconnect();
    }
    #endregion

    public async Task ChangeServer(string ip, int port) {
        await CurrentConnection.Connect(ip, port);

        OutPacketQueue.Clear();
        InPacketQueue.Clear();
    }

    public void StartHeatBeat() {
        StartCoroutine(ServerHeartBeat());
    }

    public void Disconnect() {
        CurrentConnection?.Disconnect();
    }

    public void HookPacket(PacketHeader cmd, OnPacketReceived onPackedReceived) {
        PacketHooks[cmd] = onPackedReceived;
    }

    public void SkipBytes(int bytesToSkip) {
        CurrentConnection?.SkipBytes(bytesToSkip);
    }

    #region Packet Handling
    public void PausePacketHandling() {
        IsPaused = true;
    }

    public void ResumePacketHandling() {
        IsPaused = false;
    }

    public void OnPacketReceived(InPacket packet) {
        InPacketQueue.Enqueue(packet);
    }

    public static void SendPacket(OutPacket packet) {
        Instance?.OutPacketQueue.Enqueue(packet);
    }

    private void TrySendPacket() {
        if (OutPacketQueue.Count == 0) {
            return;
        }

        var packet = OutPacketQueue.Dequeue();
        if (CurrentConnection.GetStream().CanWrite) {
            OnPacketEvent?.Invoke(packet, false);
            packet.Send(CurrentConnection.GetStream());
        }
    }

    private void TryHandleReceivedPacket() {
        if (InPacketQueue.Count == 0) {
            return;
        }

        var packet = InPacketQueue.Dequeue();
        var isHandled = PacketHooks.TryGetValue(packet.Header, out var hook);
        if (hook != null) {
            hook?.DynamicInvoke((ushort) packet.Header, -1, packet);
        }
        OnPacketEvent?.Invoke(packet, isHandled);
    }
    #endregion

    private IEnumerator ServerHeartBeat() {
        for (; ; ) {
            // TODO check if connection is still alive. If not, disconnect client
            new CZ.REQUEST_TIME2().Send();
            yield return new WaitForSeconds(10f);
        }
    }

    public struct NetworkClientState {
        public MapLoginInfo MapLoginInfo;
        public CharServerInfo CharServer;
        public CharacterData SelectedCharacter;
        public AC.ACCEPT_LOGIN3 LoginInfo;
        public HC.ACCEPT_ENTER CurrentCharactersInfo;
    }
}