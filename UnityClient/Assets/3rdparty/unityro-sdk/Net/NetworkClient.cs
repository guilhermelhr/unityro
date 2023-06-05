using System;
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

    private Dictionary<PacketHeader, List<Delegate>> PacketHooks { get; set; } = new();

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

    public void HookPacket<T>(PacketHeader cmd, OnPacketReceived<T> onPackedReceived) where T : InPacket {
        if (PacketHooks.TryGetValue(cmd, out var delegates)) {
            delegates.Add(onPackedReceived);
        } else {
            PacketHooks.Add(cmd, new List<Delegate> { onPackedReceived });
        }
    }

    public void HookPacket(PacketHeader cmd, OnPacketReceived<InPacket> onPackedReceived) {
        if (PacketHooks.TryGetValue(cmd, out var delegates)) {
            delegates.Add(onPackedReceived);
        } else {
            PacketHooks.Add(cmd, new List<Delegate> { onPackedReceived });
        }
    }

    public void UnhookPacket<T>(PacketHeader cmd, OnPacketReceived<T> onPackedReceived) where T : InPacket {
        if (PacketHooks.TryGetValue(cmd, out var delegates) && delegates.Contains(onPackedReceived)) {
            delegates.Remove(onPackedReceived);
        }
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
        if (IsPaused) {
            InPacketQueue.Enqueue(packet);
        } else {
            HandleIncomingPacket(packet);
        }
    }

    public static void SendPacket(OutPacket packet) {
        if (Instance?.IsPaused == true) {
            Instance?.OutPacketQueue.Enqueue(packet);
        } else {
            Instance?.HandleOutPacket(packet);
        }
    }

    private void TrySendPacket() {
        if (OutPacketQueue.Count == 0 || CurrentConnection == null) {
            return;
        }

        while (OutPacketQueue.TryDequeue(out var packet)) {
            HandleOutPacket(packet);
        }
    }

    private void HandleOutPacket(OutPacket packet) {
        if (CurrentConnection.GetStream().CanWrite) {
            OnPacketEvent?.Invoke(packet, false);
            packet.Send(CurrentConnection.GetStream());
            
            OnPacketEvent?.Invoke(packet, true);
        }
    }

    private void TryHandleReceivedPacket() {
        if (InPacketQueue.Count == 0 || CurrentConnection == null) {
            return;
        }

        while (InPacketQueue.TryDequeue(out var packet)) {
            HandleIncomingPacket(packet); }
    }

    private bool HandleIncomingPacket(InPacket packet) {
        var isHandled = PacketHooks.TryGetValue(packet.Header, out var delegates);

        if (delegates != null) {
            foreach (var d in delegates) {
                d.DynamicInvoke((ushort)packet.Header, -1, packet);
            }
        }
        
        OnPacketEvent?.Invoke(packet, isHandled);

        return isHandled;
    }

    #endregion

    private IEnumerator ServerHeartBeat() {
        for (;;) {
            if (!CurrentConnection.IsConnected()) yield break;
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