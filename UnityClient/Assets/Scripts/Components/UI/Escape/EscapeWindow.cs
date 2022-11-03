using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EscapeWindow : DraggableUIWindow, IEscapeWindowController {

    [SerializeField]
    private PacketLogWindow PacketLogWindow;

    [SerializeField]
    private SoundSettingsWindow SoundSettingsWindow;

    [SerializeField]
    private GameObject ButtonPrefab;
    
    [SerializeField]
    private GameObject Body;

    private EntityControl EntityControl;
    private Toggle CurrentToggle;

    private void Awake() {
        EntityControl = FindObjectOfType<EntityControl>();
    }

    void Start() {
        BuildButtons();
    }
    
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void BuildButtons(bool isPlayerDead = false) {
        foreach (Transform child in Body.transform) {
            Destroy(child.gameObject);
        }

        if (isPlayerDead) {
            BuildButton("Return to Save Point", () => {
                new CZ.RESTART(CZ.RESTART.TYPE_SAVE_POINT).Send();
                BuildButtons();
                Hide();
            });
        }

        BuildButton("Character select", () => new CZ.RESTART(CZ.RESTART.TYPE_CHAR_SELECT).Send());

#if DEBUG
        BuildButton("Packet log", () => { PacketLogWindow.Show(); });
        BuildButton("Close shop", () => { new CZ.NPC_TRADE_QUIT().Send(); });
#endif

        BuildButton("Sound Settings", () => { SoundSettingsWindow.Show(); });
        BuildButton("Exit game", () => Application.Quit());
        BuildButton("Cancel", () => Hide());
    }

    private void BuildButton(string label, UnityAction onClick) {
        GameObject goButton = Instantiate(ButtonPrefab);
        goButton.transform.SetParent(Body.transform, false);

        TextMeshProUGUI textMeshProUGUI = goButton.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.text = label;
        
        Button button = goButton.GetComponent<Button>();
        button.onClick.AddListener(onClick);
    }
}
