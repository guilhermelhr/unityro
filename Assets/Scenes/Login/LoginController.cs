using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginController : MonoBehaviour {

    public InputField usernameField;
    public InputField passwordField;

    private EventSystem system;

    void Start() {
        Core.NetworkClient.ChangeServer("127.0.0.1", 6900);
        Core.NetworkClient.HookPacket(AC.ACCEPT_LOGIN3.HEADER, this.OnLoginResponse);

        usernameField.text = "danilo3";

        system = EventSystem.current;
    }

    void Update() {
        TabBehaviour();
    }

    private void TabBehaviour() {
        if (system.currentSelectedGameObject == null || !Input.GetKeyDown(KeyCode.Tab))
            return;

        Selectable current = system.currentSelectedGameObject.GetComponent<Selectable>();
        if (current == null)
            return;
 
        bool up = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        Selectable next = up ? current.FindSelectableOnUp() : current.FindSelectableOnDown();
        next = current == next || next == null ? Selectable.allSelectablesArray[0] : next;
        system.SetSelectedGameObject(next.gameObject);
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

            Core.NetworkClient.State.LoginInfo = pkt;
            SceneManager.LoadSceneAsync("CharServerSelectionScene");
        }
    }
}
