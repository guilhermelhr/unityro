
using UnityEngine;

public static class CanvasExtensions {
    public static Canvas FindMainCanvas(this Canvas br) {
        return GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
    }
}
