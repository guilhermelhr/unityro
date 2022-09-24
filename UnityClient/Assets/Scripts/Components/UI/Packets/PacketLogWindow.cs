using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PacketLogWindow : DraggableUIWindow {

    [SerializeField] private GameObject LinearLayout;
    [SerializeField] private GameObject TextLinePrefab;
    [SerializeField] private ScrollRect ScrollView;
    [SerializeField] private TextMeshProUGUI PacketTitle;
    [SerializeField] private TextMeshProUGUI PacketContent;

    private const int MaxPacketHistory = 100;
    private Queue<NetworkPacket> PacketHistory = new Queue<NetworkPacket>();
    private Queue<GameObject> PacketHistoryText = new Queue<GameObject>();

    private void Awake() {
        NetworkClient.OnPacketEvent += OnNetworkPacket;
    }

    private void OnDestroy() {
        NetworkClient.OnPacketEvent -= OnNetworkPacket;
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void OnNetworkPacket(NetworkPacket packet, bool isHandled) {
        GameObject textObject = null;
        if (packet is OutPacket outPacket) {
            textObject = OnPacketSent(outPacket);
        } else if (packet is InPacket inPacket) {
            textObject = OnPacketReceived(inPacket, isHandled);
        }

        if (PacketHistory.Count >= MaxPacketHistory) {
            PacketHistory.Dequeue();
            Destroy(PacketHistoryText.Dequeue());
        }
        PacketHistory.Enqueue(packet);
        PacketHistoryText.Enqueue(textObject);

        var trigger = textObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnPacketSelected(packet); });
        trigger.triggers.Add(entry);
    }

    private void OnPacketSelected(NetworkPacket packet) {
        PacketContent.text = null;
        PacketTitle.text = packet.GetType().Name;
        foreach (FieldInfo fi in packet.GetType().GetFields()) {
            PacketContent.text += $"{fi.Name}: {fi.GetValue(packet)?.ToString()}\n";
        }
    }

    private GameObject OnPacketReceived(InPacket packet, bool isHandled) {
        var appendText = isHandled ? "" : "NH";
        var text = $"<- {string.Format("0x{0:x3}", (ushort) packet.Header)} {packet.Header} {appendText}";
        var color = Color.green;

        return InstantiateText(text, color);
    }

    private GameObject OnPacketSent(OutPacket packet) {
        var text = $"-> {string.Format("0x{0:x3}", (ushort) packet.Header)} {packet.Header}";
        var color = Color.black;
        return InstantiateText(text, color);
    }

    private GameObject InstantiateText(string text, Color color) {
        var textObject = Instantiate(TextLinePrefab);
        var uiText = textObject.GetComponentInChildren<TextMeshProUGUI>();
        uiText.text = text;
        uiText.color = color;
        uiText.enableWordWrapping = false;
        textObject.transform.SetParent(LinearLayout.transform, false);

        return uiText.gameObject;
    }
}