using ROIO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxController : MonoBehaviour {

    [SerializeField] private InputField MessageInput;
    [SerializeField] private GameObject PMInput;
    [SerializeField] private GameObject LinearLayout;
    [SerializeField] private GameObject TextLinePrefab;

    private NetworkClient NetworkClient;
    private EntityManager EntityManager;

    private void Awake() {
        NetworkClient = FindObjectOfType<NetworkClient>();
        EntityManager = FindObjectOfType<EntityManager>();

        NetworkClient.HookPacket(ZC.NOTIFY_PLAYERCHAT.HEADER, OnMessageRecieved);
        NetworkClient.HookPacket(ZC.NOTIFY_CHAT.HEADER, OnMessageRecieved);
        NetworkClient.HookPacket(ZC.MSG.HEADER, OnMessageRecieved);
    }

    private void OnMessageRecieved(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_PLAYERCHAT) {
            var pkt = packet as ZC.NOTIFY_PLAYERCHAT;

            var prefab = Instantiate(TextLinePrefab);
            var uiText = prefab.GetComponentInChildren<TextMeshProUGUI>();
            uiText.text = pkt.Message;
            uiText.color = Color.green;

            prefab.transform.SetParent(LinearLayout.transform, false);
        } else if (packet is ZC.NOTIFY_CHAT) {
            var pkt = packet as ZC.NOTIFY_CHAT;

            var prefab = Instantiate(TextLinePrefab);
            var uiText = prefab.GetComponentInChildren<TextMeshProUGUI>();
            uiText.text = pkt.Message;
            uiText.color = Color.green;

            prefab.transform.SetParent(LinearLayout.transform, false);

            EntityManager.GetEntity(pkt.GID).DisplayChatBubble(pkt.Message);
        } else if (packet is ZC.MSG) {
            var pkt = packet as ZC.MSG;

            var prefab = Instantiate(TextLinePrefab);
            var uiText = prefab.GetComponentInChildren<TextMeshProUGUI>();
            uiText.text = (string)Tables.MsgStringTable[pkt.MessageID] ?? $"{pkt.MessageID}";
            uiText.color = Color.white;

            prefab.transform.SetParent(LinearLayout.transform, false);
        }
    }

    public void SendChatMessage() {
        var message = MessageInput.text;
        if (message.Length == 0) return;

        new CZ.REQUEST_CHAT(message).Send();
        MessageInput.text = "";
    }

    public void DisplayMessage(int messageID, int messageType) {
        var prefab = Instantiate(TextLinePrefab);
        var uiText = prefab.GetComponentInChildren<TextMeshProUGUI>();
        uiText.text = (string)Tables.MsgStringTable[$"{messageID}"] ?? $"{messageID}";
        uiText.color = Color.white;

        prefab.transform.SetParent(LinearLayout.transform, false);
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SendChatMessage();
        }
    }
}
