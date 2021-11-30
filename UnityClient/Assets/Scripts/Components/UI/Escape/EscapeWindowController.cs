using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EscapeWindowController : DraggableUIWindow, IEscapeWindowController {
    
    private const float BUTTONS_Y_STEP = 25;

    [SerializeField]
    private GameObject ButtonPrefab;
    
    [SerializeField]
    private GameObject Body;

    private EntityControl EntityControl;
    private Toggle CurrentToggle;
    private float CurrentButtonsY;
    private int CurrentButtonsNum;

    private void Awake() {
        EntityControl = FindObjectOfType<EntityControl>();
    }

    // Start is called before the first frame update
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

        CurrentButtonsY = isPlayerDead ? 40 : 25;
        CurrentButtonsNum = 0;

        if (isPlayerDead) {
            BuildButton("Return to Save Point", () => {
                new CZ.RESTART(CZ.RESTART.TYPE_SAVE_POINT).Send();
                BuildButtons();
                Hide();
            });
        }

        BuildButton("Character select", () => new CZ.RESTART(CZ.RESTART.TYPE_CHAR_SELECT).Send());
        BuildButton("Exit game", () => Application.Quit());
        BuildButton("Cancel", () => Hide());

        AdjustHeight();
    }

    private void BuildButton(string label, UnityAction onClick) {
        GameObject goButton = Instantiate(ButtonPrefab);
        goButton.transform.SetParent(Body.transform);
        goButton.transform.localScale = Vector3.one;
        goButton.transform.localPosition = new Vector3(115, CurrentButtonsY, 1);

        TextMeshProUGUI textMeshProUGUI = goButton.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.text = label;
        
        Button button = goButton.GetComponent<Button>();
        button.onClick.AddListener(onClick);
        CurrentButtonsY -= BUTTONS_Y_STEP;
        CurrentButtonsNum++;
    }

    private void AdjustHeight() {
        float newY = CurrentButtonsNum == 4 ? -60 : -45;
        float newHeight = CurrentButtonsNum == 4 ? 120 : 95;
        RectTransform bodyRectTransform = Body.GetComponent<RectTransform>();
        bodyRectTransform.localPosition = new Vector3(140, newY, 0);
        bodyRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
}
