using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharServerSelectionController : MonoBehaviour {

    public GameObject LinearLayout;
    public GameObject CharServerListItem;

    private NetworkClient NetworkClient;

    private CharServerInfo charServerInfo;

    private void Awake() {
        NetworkClient = FindObjectOfType<NetworkClient>();
    }

    // Start is called before the first frame update
    private void Start() {
        // Disconnect from Login Server
        NetworkClient.Disconnect();

        // Hook packets
        NetworkClient.HookPacket(HC.ACCEPT_ENTER.HEADER, OnEnterResponse);

        BuildServerList();
    }

    private void OnEnterResponse(ushort cmd, int size, InPacket packet) {
        if(packet is HC.ACCEPT_ENTER) {
            NetworkClient.State.CurrentCharactersInfo = packet as HC.ACCEPT_ENTER;
            SceneManager.LoadSceneAsync("CharSelectionScene");
        }
    }

    private void BuildServerList() {
        var charServers = NetworkClient.State.LoginInfo.Servers;
        if(charServers == null || charServers.Length == 0) {
            throw new Exception("Char server list is empty");
        }

        foreach(var server in charServers.Where(t => t.Name.Length > 0)) {
            var serverItem = Instantiate(CharServerListItem) as GameObject;
            var controller = serverItem.GetComponent<CharServerListItemController>();
            var toggle = serverItem.GetComponent<Toggle>();
            toggle.group = LinearLayout.GetComponent<ToggleGroup>();
            controller.BindData(server);
            controller.OnCharServerSelected = OnCharServerSelected;
            toggle.onValueChanged.AddListener(delegate {controller.OnValueChanged();});

            toggle.isOn = true;
            serverItem.transform.SetParent(LinearLayout.transform);
            serverItem.transform.localScale = Vector3.one;
        }
    }

    private void OnCharServerSelected(CharServerInfo charServerInfo) {
        this.charServerInfo = charServerInfo;
    }

    public void OnOkClicked() {
        var loginInfo = NetworkClient.State.LoginInfo;
        if(charServerInfo == null || loginInfo == null) {
            throw new Exception("Invalid charserverinfo or login info");
        };
        NetworkClient.State.CharServer = charServerInfo;
        NetworkClient.ChangeServer(charServerInfo.IP.ToString(), charServerInfo.Port);
        NetworkClient.SkipBytes(4);

        new CH.ENTER(loginInfo.AccountID, loginInfo.LoginID1, loginInfo.LoginID2, loginInfo.Sex).Send();
    }

    public void OnCancelClicked() {

    }
}
