using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharServerSelectionController : MonoBehaviour {

    public GameObject LinearLayout;
    public GameObject CharServerListItem;

    private CharServerInfo charServerInfo;

    // Start is called before the first frame update
    private void Start() {
        Core.NetworkClient.HookPacket(HC.ACCEPT_ENTER.HEADER, OnEnterResponse);
        BuildServerList();
    }

    private void Update() {
        Core.NetworkClient.Ping();
    }

    private void OnEnterResponse(ushort cmd, int size, InPacket packet) {
        if(packet is HC.ACCEPT_ENTER) {
            Core.NetworkClient.State.CurrentCharactersInfo = packet as HC.ACCEPT_ENTER;
            SceneManager.LoadSceneAsync("CharSelectionScene");
        }
    }

    private void BuildServerList() {
        var charServers = Core.NetworkClient.State.LoginInfo.Servers;
        if(charServers == null || charServers.Length == 0) {
            throw new Exception("Char server list is empty");
        }

        foreach(var server in charServers.Where(t => t.Name.Length > 0)) {
            var serverItem = Instantiate(CharServerListItem) as GameObject;
            var controller = serverItem.GetComponent<CharServerListItemController>();
            controller.BindData(server);
            controller.OnCharServerSelected = OnCharServerSelected;

            serverItem.transform.parent = LinearLayout.transform;
            serverItem.transform.localScale = Vector3.one;
        }
    }

    private void OnCharServerSelected(CharServerInfo charServerInfo) {
        this.charServerInfo = charServerInfo;
    }

    public void OnOkClicked() {
        var acceptLogin = Core.NetworkClient.State.LoginInfo;
        if(charServerInfo == null || acceptLogin == null) {
            throw new Exception("Invalid charserverinfo or login info");
        };
        Core.NetworkClient.State.CharServer = charServerInfo;
        Core.NetworkClient.ChangeServer(charServerInfo.IP, charServerInfo.Port);
        new CH.ENTER(acceptLogin.AccountID, acceptLogin.LoginID1, acceptLogin.LoginID2, acceptLogin.Sex).Send();
    }

    public void OnCancelClicked() {

    }
}
