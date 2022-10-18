using ROIO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {

    public InputField usernameField;
    public InputField passwordField;
    public RawImage background;
    
    private NetworkClient NetworkClient;
    private RemoteConfiguration RemoteConfiguration;
    
    void Start() {
        background.SetLoginBackground();
        
        NetworkClient = FindObjectOfType<NetworkClient>();
        RemoteConfiguration = FindObjectOfType<GameManager>().RemoteConfiguration;

        NetworkClient.HookPacket(AC.ACCEPT_LOGIN3.HEADER, this.OnLoginResponse);
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

        TryConnectAndLogin(username, password);
    }

    public void OnExitClicked() {

    }

    private async void TryConnectAndLogin(string username, string password) {
        await NetworkClient.ChangeServer(RemoteConfiguration.loginServer, int.Parse(RemoteConfiguration.loginPort));
        new CA.LOGIN(username, password, 10, 10).Send();
    }

    private void OnLoginResponse(ushort cmd, int size, InPacket packet) {
        if (packet is AC.ACCEPT_LOGIN3) {
            var pkt = packet as AC.ACCEPT_LOGIN3;

            NetworkClient.State.LoginInfo = pkt;
            SceneManager.LoadSceneAsync("CharServerSelectionScene");
        }
    }
}
