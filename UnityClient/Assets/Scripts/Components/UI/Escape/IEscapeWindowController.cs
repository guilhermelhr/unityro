using UnityEngine.Events;

public interface IEscapeWindowController {
    
    void Show();
    
    void Hide();

    void BuildButtons(bool isPlayerDead = false);
}