using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUiController : MonoBehaviour {

    public InputField chatField;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SendChatMessage();
        }
    }

    public void SendChatMessage() {
        var message = chatField.text;
        if (message.Length == 0) return;

        new CZ.REQUEST_CHAT(message).Send();
    }
}
