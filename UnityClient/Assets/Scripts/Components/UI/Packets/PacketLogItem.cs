using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PacketLogItem : MonoBehaviour, IPointerClickHandler {

    public NetworkPacket Packet { get; private set; }
    public Action<NetworkPacket> OnPacketSelected;

    [SerializeField] private TextMeshProUGUI PacketHeaderText;
    [SerializeField] private TextMeshProUGUI PacketTitleText;
    [SerializeField] private Image PacketDirectionImage;

    public void OnPointerClick(PointerEventData eventData) {
        if (Packet == null)
            return;

        OnPacketSelected?.Invoke(Packet);
    }

    public void SetPacket(NetworkPacket packet) {
        Packet = packet;

        if (packet is OutPacket outpacket) {
            PacketHeaderText.text = string.Format("0x{0:x3}", (ushort) outpacket.Header);
            PacketTitleText.text = outpacket.Header.ToString();
            PacketDirectionImage.color = Color.green;
        } else if (packet is InPacket inpacket) {
            PacketHeaderText.text = string.Format("0x{0:x3}", (ushort) inpacket.Header);
            PacketTitleText.text = inpacket.Header.ToString();
            PacketDirectionImage.color = Color.blue;
        }

    }
}
