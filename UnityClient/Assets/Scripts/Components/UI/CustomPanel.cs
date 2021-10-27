using ROIO;
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
        LoadTextures();
    }

    private void LoadTextures() {
        LoadIdleTexture();
        LoadHoverTexture();
        LoadPressedTexture();
    }

    private void LoadPressedTexture() {
        try {
            if(pressedImage != null && pressedImage.Length > 0 && pressedImage == null) {
                pressedTexture = LoadImage(pressedImage);
            }
        } catch {
            Debug.LogError("Failed to load pressed image from " + this);
        }
    }

    private void LoadHoverTexture() {
        try {
            if(hoverImage != null && hoverImage.Length > 0 && hoverImage == null) {
                hoverTexture = LoadImage(hoverImage);
            }
        } catch {
            Debug.LogError("Failed to load hover image from " + this);
        }
    }

    private void LoadIdleTexture() {
        try {
            if(backgroundImage != null && backgroundImage.Length > 0 && backgroundTexture == null) {
                backgroundTexture = LoadImage(backgroundImage);
                texture = backgroundTexture;
                if(overrideSize)
                    GetComponent<RectTransform>().sizeDelta = new Vector2(backgroundTexture.width, backgroundTexture.height);
            }
        } catch {
            Debug.LogError("Failed to load background image from " + this);
        }
    }

    private void Update() {
        if(texture == null) {
            LoadTextures();
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

    private Texture2D LoadImage(string path) => FileManager.Load(DBManager.INTERFACE_PATH + path) as Texture2D;
}
