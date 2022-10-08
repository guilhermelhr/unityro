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
        var textObject = Instantiate(TextLinePrefab).GetComponent<PacketLogItem>();
        textObject.transform.SetParent(LinearLayout.transform, false);

        if (PacketHistory.Count >= MaxPacketHistory) {
            PacketHistory.Dequeue();
            Destroy(PacketHistoryText.Dequeue());
        }
        PacketHistory.Enqueue(packet);
        PacketHistoryText.Enqueue(textObject.gameObject);

        textObject.SetPacket(packet);
        textObject.OnPacketSelected = OnPacketSelected;
    }

    private void OnPacketSelected(NetworkPacket packet) {
        PacketContent.text = null;
        PacketTitle.text = packet.GetType().Name;
        foreach (FieldInfo fi in packet.GetType().GetFields()) {
            PacketContent.text += $"{fi.Name}: {fi.GetValue(packet)?.ToString()}\n";
        }
    }
}