using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {

    public InputField usernameField;
    public InputField passwordField;

    void Start() {
        Core.NetworkClient.ConnectToServer();
        Core.NetworkClient.HookPacket(AC.ACCEPT_LOGIN.HEADER, this.OnLoginResponse);
    }

    // Update is called once per frame
    void Update() {

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
        if (packet is AC.ACCEPT_LOGIN) {
            var pkt = packet as AC.ACCEPT_LOGIN;

            Core.NetworkClient.State.LoginInfo = pkt;
            SceneManager.LoadSceneAsync("CharServerSelectionScene");
        }
    }
}
