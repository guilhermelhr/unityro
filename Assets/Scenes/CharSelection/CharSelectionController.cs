using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharSelectionController : MonoBehaviour {

    public GridLayoutGroup GridLayout;
    public GameObject charSelectionItem;

    private HC.NOTIFY_ZONESVR2 currentMapInfo;
    private HC.ACCEPT_ENTER currentCharactersInfo;
    private CharacterData selectedCharacter;

    void Start() {
        currentCharactersInfo = Core.NetworkClient.State.CurrentCharactersInfo;
        Core.NetworkClient.HookPacket(HC.NOTIFY_ZONESVR2.HEADER, OnCharacterSelectionAccepted);
        Core.NetworkClient.HookPacket(ZC.ACCEPT_ENTER2.HEADER, OnMapServerLoginAccepted);

        PopulateUI();
    }

    private void OnMapServerLoginAccepted(ushort cmd, int size, InPacket packet) {
        if(packet is ZC.ACCEPT_ENTER2) {
            var pkt = packet as ZC.ACCEPT_ENTER2;
            var mapLoginInfo = new MapLoginInfo() {
                mapname = currentMapInfo.Mapname.Split('.')[0],
                PosX = pkt.PosX,
                PosY = pkt.PosY,
                Dir = pkt.Dir
            };
            Core.NetworkClient.State.MapLoginInfo = mapLoginInfo;
            SceneManager.LoadScene("MapScene");
        }
    }

    private void OnCharacterSelectionAccepted(ushort cmd, int size, InPacket packet) {
        if(packet is HC.NOTIFY_ZONESVR2) {
            Core.NetworkClient.Disconnect();

            currentMapInfo = packet as HC.NOTIFY_ZONESVR2;
            Core.NetworkClient.State.SelectedCharacter = selectedCharacter;
            Core.NetworkClient.ChangeServer(currentMapInfo.IP.ToString(), currentMapInfo.Port);
            Core.NetworkClient.CurrentConnection.Start();

            var loginInfo = Core.NetworkClient.State.LoginInfo;
            new CZ.ENTER(loginInfo.AccountID, selectedCharacter.GID, loginInfo.LoginID1, (int)new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds(), loginInfo.Sex).Send();
        }
    }

    private void PopulateUI() {
        for(var i = 0; i < currentCharactersInfo.MaxSlots; i++) {
            var item = Instantiate(charSelectionItem);
            item.transform.SetParent(GridLayout.transform);

            var controller = item.GetComponent<CharacterCellController>();
            if(i < currentCharactersInfo.Chars.Length) {
                controller.BindData(currentCharactersInfo.Chars[i]);
                controller.OnCharacterSelected = OnCharacterSelected;
            }
        }
    }

    private void OnCharacterSelected(CharacterData character) {
        this.selectedCharacter = character;
    }

    public void OnEnterGameClicked() {
        if(selectedCharacter == null) return;
        var charIndex = new List<CharacterData>(currentCharactersInfo.Chars).IndexOf(selectedCharacter);
        if(charIndex < 0) return;

        new CH.SELECT_CHAR(charIndex).Send();
    }
}
