using ROIO;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

    private async void LoadPressedTexture() {
        try {
            if (pressedImage != null && pressedImage.Length > 0 && pressedTexture == null) {
                pressedTexture = await LoadImage(pressedImage);
            }
        } catch {
            Debug.LogError("Failed to load pressed image from " + this);
        }
    }

    private async void LoadHoverTexture() {
        try {
            if (hoverImage != null && hoverImage.Length > 0 && hoverTexture == null) {
                hoverTexture = await LoadImage(hoverImage);
            }
        } catch {
            Debug.LogError("Failed to load hover image from " + this);
        }
    }

    private async void LoadIdleTexture() {
        try {
            if (backgroundImage != null && backgroundImage.Length > 0 && backgroundTexture == null) {
                backgroundTexture = await LoadImage(backgroundImage);
                texture = backgroundTexture;
                if (overrideSize)
                    SetNativeSize();
            }
        } catch (Exception e) {
            Debug.LogError("Failed to load background image from " + this);
            Debug.LogException(e);
        }
    }

    private void Update() {
        if (texture == null) {
            LoadTextures();
        }
    }

    public async void SetBackground(string path) {
        backgroundTexture = await Addressables.LoadAssetAsync<Texture2D>(DBManager.INTERFACE_PATH + path).Task;
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

    private async Task<Texture2D> LoadImage(string path) => await Addressables.LoadAssetAsync<Texture2D>(DBManager.INTERFACE_PATH + path).Task;
}
