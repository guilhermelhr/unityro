using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharServerSelectionController : MonoBehaviour {

    public GameObject LinearLayout;
    public GameObject CharServerListItem;
    public RawImage background;

    private NetworkClient NetworkClient;
    private GameManager GameManager;

    private CharServerInfo charServerInfo;

    private void Awake() {
        GameManager = FindObjectOfType<GameManager>();
        NetworkClient = FindObjectOfType<NetworkClient>();
    }

    // Start is called before the first frame update
    private void Start() {
        background.SetLoginBackground();
        
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
        var remoteConfig = GameManager.RemoteConfiguration;
        if (charServerInfo == null || loginInfo == null) {
            throw new Exception("Invalid charserverinfo or login info");
        };
        NetworkClient.State.CharServer = charServerInfo;

        string charIp;
        if (remoteConfig.useSameIpForEveryServer) {
            charIp = remoteConfig.loginServer;
        } else {
            charIp = charServerInfo.IP.ToString();
        }

        ConnectToCharServer(loginInfo, charIp);
    }

    private async System.Threading.Tasks.Task ConnectToCharServer(AC.ACCEPT_LOGIN3 loginInfo, string charIp) {
        await NetworkClient.ChangeServer(charIp, charServerInfo.Port);
        NetworkClient.SkipBytes(4);

        new CH.ENTER(loginInfo.AccountID, loginInfo.LoginID1, loginInfo.LoginID2, loginInfo.Sex).Send();
    }

    public void OnCancelClicked() {

    }
}
