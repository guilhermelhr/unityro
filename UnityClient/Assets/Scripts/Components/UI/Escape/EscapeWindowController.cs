using UnityEngine;
using UnityEngine.UI;

public class EscapeWindowController : DraggableUIWindow, IEscapeWindowController {

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

    public void Cancel() {
        gameObject.SetActive(false);
    }
}
