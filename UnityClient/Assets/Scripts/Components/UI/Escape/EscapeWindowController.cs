using UnityEngine;
using UnityEngine.UI;

public class EscapeWindowController : DraggableUIWindow, IEscapeWindowController {

    [SerializeField]
    private GameObject ResurrectButton;
    
    [SerializeField]
    private GameObject ReturnToSavePointButton;

    private EntityControl EntityControl;
    private Toggle CurrentToggle;

    private void Awake() {
        EntityControl = FindObjectOfType<EntityControl>();
    }

    public void Resurrect() {
    }

    public void ToggleSoundUI() {
    }

    public void ToggleGraphicUI() {
    }

    public void ReturnToSavePoint() {
        new CZ.RESTART(0).Send();
    }

    public void ReturnToCharSelection() {
        new CZ.RESTART(1).Send();
    }

    public void Exit() {
        Application.Quit();
    }

    public void EnableReturnToSavePoint() {
        ReturnToSavePointButton.SetActive(true);
    }

    public void DisableReturnToSavePoint() {
        ReturnToSavePointButton.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
