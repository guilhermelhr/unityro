using Assets.Scripts.Renderer.Sprite;
using System.Collections;
using UnityEngine;

public enum GameEntityViewerType {
    MESH, SPRITE
}

public abstract class GameEntityViewer : MonoBehaviour {
    public SpriteState State = SpriteState.Idle;

    public abstract void Init(SpriteData spriteData, Texture2D atlas);
    public abstract void Init(bool reloadSprites = false);
    public abstract void ChangeMotion(MotionRequest motion, MotionRequest? nextMotion = null);
    public abstract IEnumerator FadeOut();
}

public struct MotionRequest {
    public SpriteMotion Motion;
    public double delay;
}

public struct PaletteData {
    public int hair;
    public int hairColor;
    public int clothesColor;
}
