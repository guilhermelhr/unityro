using UnityEngine;

public interface IUsable {
    string GetDisplayName();
    int GetDisplayNumber();
    Texture2D GetTexture();
    void OnUse();
    void OnRightClick();
}
