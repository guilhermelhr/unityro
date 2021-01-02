using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomPanel : RawImage,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler {

    public string backgroundImage;
    public string hoverImage;
    public string pressedImage;
    public bool overrideSize = true;

    private Texture2D backgroundTexture;
    private Texture2D hoverTexture;
    private Texture2D pressedTexture;

    protected override void Start() {
        try {
            if (backgroundImage != null && backgroundTexture == null) {
                backgroundTexture = FileManager.Load(DBManager.INTERFACE_PATH + backgroundImage) as Texture2D;
                texture = backgroundTexture;
                if (overrideSize)
                    GetComponent<RectTransform>().sizeDelta = new Vector2(backgroundTexture.width, backgroundTexture.height);
            }
        } catch {
            Debug.LogError("Failed to load background image from " + this);
        }

        try {
            if (hoverImage != null && hoverImage == null) {
                hoverTexture = FileManager.Load(DBManager.INTERFACE_PATH + hoverImage) as Texture2D;
            }
        } catch {
            Debug.LogError("Failed to load hover image from " + this);
        }

        try {
            if (pressedImage != null && pressedImage == null) {
                pressedTexture = FileManager.Load(DBManager.INTERFACE_PATH + pressedImage) as Texture2D;
            }
        } catch {
            Debug.LogError("Failed to load pressed image from " + this);
        }
    }

    public void SetBackground(string path) {
        backgroundTexture = (Texture2D)FileManager.Load(DBManager.INTERFACE_PATH + path);
        texture = backgroundTexture;
        GetComponent<RectTransform>().sizeDelta = new Vector2(backgroundTexture.width, backgroundTexture.height);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (pressedTexture != null) {
            texture = pressedTexture;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (hoverTexture != null) {
            texture = hoverTexture;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (backgroundTexture != null) {
            texture = backgroundTexture;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (hoverTexture != null) {
            texture = hoverTexture;
        }
    }
}
