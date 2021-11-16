using ROIO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginController : MonoBehaviour {

    public InputField usernameField;
    public InputField passwordField;

    private NetworkClient NetworkClient;
    
    void Start() {
//        Texture2D texture = FileManager.Load($"{DBManager.INTERFACE_PATH}bgi_temp.bmp") as Texture2D;
//        RawImage image = GameObject.Find("Screen").GetComponent<RawImage>();
//        image.texture = texture;

        Background.SetImage("bgi_temp.bmp");
//        background = FindObjectOfType<Background>();
//        background.SetImage("bgi_temp.bmp");
        
        NetworkClient = FindObjectOfType<NetworkClient>();

        NetworkClient.ChangeServer("127.0.0.1", 6900);
        NetworkClient.HookPacket(AC.ACCEPT_LOGIN3.HEADER, this.OnLoginResponse);
        usernameField.text = "danilo3";
    }

    void Update() {
        TabBehaviour();
    }

    private void TabBehaviour() {
        EventSystem currentEvent = EventSystem.current;

        if (currentEvent.currentSelectedGameObject == null || !Input.GetKeyDown(KeyCode.Tab))
            return;

        Selectable current = currentEvent.currentSelectedGameObject.GetComponent<Selectable>();
        if (current == null)
            return;
 
        bool up = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        Selectable next = up ? current.FindSelectableOnUp() : current.FindSelectableOnDown();
        next = current == next || next == null ? Selectable.allSelectablesArray[0] : next;
        currentEvent.SetSelectedGameObject(next.gameObject);
    }

    public void OnLoginClicked() {
        var username = usernameField.text;
        var password = passwordField.text;

        if (username.Length == 0 || password.Length == 0) {
            return;
        }

        new CA.LOGIN(username, password, 10, 10).Send();
    }

    public void OnExitClicked() {

    }

    private void OnLoginResponse(ushort cmd, int size, InPacket packet) {
        if (packet is AC.ACCEPT_LOGIN3) {
            var pkt = packet as AC.ACCEPT_LOGIN3;

            NetworkClient.State.LoginInfo = pkt;
            SceneManager.LoadSceneAsync("CharServerSelectionScene");
        }
    }
}
