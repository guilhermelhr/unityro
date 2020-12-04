using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectionController : MonoBehaviour {

    public GridLayoutGroup GridLayout;
    public GameObject charSelectionItem;
    
    private HC.ACCEPT_ENTER currentCharactersInfo;
    private CharacterData selectedCharacter;

    void Start() {
        currentCharactersInfo = Core.NetworkClient.State.CurrentCharactersInfo;
        Core.NetworkClient.HookPacket(HC.NOTIFY_ZONESVR2.HEADER, OnCharacterSelectionAccepted);

        PopulateUI();
    }

    private void OnCharacterSelectionAccepted(ushort cmd, int size, InPacket packet) {
        if(packet is HC.NOTIFY_ZONESVR2) {
            //this.packet = packet;
            var pkt = packet as HC.NOTIFY_ZONESVR2;
            //CurrentConnection.Disconnect();
            //CurrentConnection.Connect(pkt.IP.ToString(), pkt.Port);
        }
    }

    private void PopulateUI() {
        for(var i = 0; i < currentCharactersInfo.MaxSlots; i++) {
            var item = Instantiate(charSelectionItem);
            item.transform.parent = GridLayout.transform;

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
