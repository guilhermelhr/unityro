﻿public interface IEscapeWindowController {
    
    void Resurrect();
    
    void ToggleSoundUI();
    
    void ToggleGraphicUI();
    
    void ReturnToSavePoint();
    
    void ReturnToCharSelection();
    
    void Exit();
    
    void EnableReturnToSavePoint();
    
    void DisableReturnToSavePoint();
    
    void Show();
    
    void Hide();
}