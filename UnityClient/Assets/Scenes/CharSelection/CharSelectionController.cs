using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharSelectionController : MonoBehaviour {

    public GridLayoutGroup GridLayout;
    public GameObject charSelectionItem;
    public GameObject textPanel;
    public GameObject MapUIPrefab;
    public EventSystem EventSystem;

    private HC.NOTIFY_ZONESVR2 currentMapInfo;
    private HC.ACCEPT_ENTER currentCharactersInfo;
    private CharacterData selectedCharacter;

    private EntityManager EntityManager;
    private NetworkClient NetworkClient;

    private List<CharacterCellController> characterSlots;

    private void Awake() {
        EntityManager = FindObjectOfType<EntityManager>();
        NetworkClient = FindObjectOfType<NetworkClient>();
    }

    void Start() {
        currentCharactersInfo = NetworkClient.State.CurrentCharactersInfo;
        NetworkClient.HookPacket(HC.NOTIFY_ZONESVR2.HEADER, OnCharacterSelectionAccepted);
        NetworkClient.HookPacket(HC.ACCEPT_MAKECHAR.HEADER, OnMakeCharAccepted);
        NetworkClient.HookPacket(ZC.ACCEPT_ENTER2.HEADER, OnMapServerLoginAccepted);

        PopulateUI();
    }

    private void OnMakeCharAccepted(ushort cmd, int size, InPacket packet) {
        if (packet is HC.ACCEPT_MAKECHAR ACCEPT_MAKECHAR) {
            currentCharactersInfo.Chars.Add(ACCEPT_MAKECHAR.characterData);
            characterSlots.Find(it => it.IsEmpty).BindData(ACCEPT_MAKECHAR.characterData);

            SceneManager.UnloadSceneAsync(6);
            EventSystem.gameObject.SetActive(true);
        }
    }

    private void OnMapServerLoginAccepted(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ACCEPT_ENTER2) {
            var pkt = packet as ZC.ACCEPT_ENTER2;
            var mapLoginInfo = new MapLoginInfo() {
                mapname = currentMapInfo.Mapname.Split('.')[0],
                PosX = pkt.PosX,
                PosY = pkt.PosY,
                Dir = pkt.Dir
            };
            NetworkClient.State.MapLoginInfo = mapLoginInfo;
            Session.CurrentSession.SetCurrentMap(mapLoginInfo.mapname);
            NetworkClient.StartHeatBeat();

            SceneManager.LoadScene("MapScene");
        }
    }

    private void OnCharacterSelectionAccepted(ushort cmd, int size, InPacket packet) {
        if (packet is HC.NOTIFY_ZONESVR2) {
            NetworkClient.Disconnect();

            currentMapInfo = packet as HC.NOTIFY_ZONESVR2;
            NetworkClient.State.SelectedCharacter = selectedCharacter;
            NetworkClient.ChangeServer(currentMapInfo.IP.ToString(), currentMapInfo.Port);
            NetworkClient.CurrentConnection.Start();

            var entity = EntityManager.SpawnPlayer(NetworkClient.State.SelectedCharacter);
            Session.StartSession(new Session(entity, NetworkClient.State.LoginInfo.AccountID));
            DontDestroyOnLoad(entity.gameObject);

            var mapUI = Instantiate(MapUIPrefab);
            DontDestroyOnLoad(mapUI);

            var loginInfo = NetworkClient.State.LoginInfo;
            new CZ.ENTER2(loginInfo.AccountID, selectedCharacter.GID, loginInfo.LoginID1, (int) new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(), loginInfo.Sex).Send();
        }
    }

    private void PopulateUI() {
        characterSlots = new List<CharacterCellController>();
        for (var i = 0; i < currentCharactersInfo.MaxSlots; i++) {
            var item = Instantiate(charSelectionItem);
            item.transform.SetParent(GridLayout.transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            var controller = item.GetComponent<CharacterCellController>();
            if (i < currentCharactersInfo.Chars.Count) {
                controller.BindData(currentCharactersInfo.Chars[i]);
            }
            controller.OnCharacterSelected = OnCharacterSelected;

            characterSlots.Add(controller);
        }
    }

    private void OnCharacterSelected(CharacterData character) {
        if (character == null) {
            EventSystem.gameObject.SetActive(false);
            SceneManager.sceneUnloaded += delegate (Scene scene) {
                if (scene.buildIndex == 6) {
                    EventSystem.gameObject.SetActive(true);
                }
            };
            SceneManager.LoadSceneAsync(6, LoadSceneMode.Additive);
        } else {
            selectedCharacter = character;
            SetTextFields(selectedCharacter);
        }
    }

    public void OnEnterGameClicked() {
        if (selectedCharacter == null)
            return;
        var charIndex = currentCharactersInfo.Chars.IndexOf(selectedCharacter);
        if (charIndex < 0)
            return;

        new CH.SELECT_CHAR(charIndex).Send();
    }

    public void CreateChar() {
        new CH.MAKE_CHAR2() {
            Name = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8),
            CharNum = (byte) currentCharactersInfo.Chars.Count
        }.Send();
    }
    
    private void SetTextFields(CharacterData character) {
        Text[] fields = textPanel.GetComponentsInChildren<Text>();
        int i = 0;
        foreach (Text field in fields) {
            switch (i) {
                case 0: // map
                    field.text = character.MapName; // @todo get map name
                    break;
                case 1: // job
                    field.text =
                        CultureInfo.InvariantCulture.TextInfo.ToTitleCase(
                            JobHelper.GetJobName(character.Job, character.Sex));
                    break;
                case 2: // lv.
                    field.text = character.BaseLevel.ToString();
                    break;
                case 3: // exp
                    field.text = character.Exp.ToString();
                    break;
                case 4: // hp
                    field.text = character.MaxHP.ToString();
                    break;
                case 5: // sp
                    field.text = character.MaxSP.ToString();
                    break;
                case 6: // str
                    field.text = character.Str.ToString();
                    break;
                case 7: // agi
                    field.text = character.Agi.ToString();
                    break;
                case 8: // vit
                    field.text = character.Vit.ToString();
                    break;
                case 9: // int
                    field.text = character.Int.ToString();
                    break;
                case 10: // dex
                    field.text = character.Dex.ToString();
                    break;
                case 11: // luk
                    field.text = character.Luk.ToString();
                    break;
            }
            i++;
        }
    }
}
