using Assets.Scripts.Utils.Extensions;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CustomUIAddressablesHolder))]
public class CustomPanel : RawImage,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler {

    public string backgroundImage;
    public string hoverImage;
    public string pressedImage;
    public bool overrideSize = false;

    private Texture2D backgroundTexture;
    private Texture2D hoverTexture;
    private Texture2D pressedTexture;

    private CustomUIAddressablesHolder AddressablesHolder;

    protected override void OnEnable() {
        texture = null;
        if (AddressablesHolder == null) {
            AddressablesHolder = gameObject.GetComponent<CustomUIAddressablesHolder>();
        }

        if (AddressablesHolder.backgroundTexture.Asset != null) {
            texture = (Texture2D) AddressablesHolder.backgroundTexture.Asset;
        }
    }

    protected override void Start() {
        texture = null;
        LoadTextures();
    }

    private void LoadTextures() {
        LoadIdleTexture();
        LoadHoverTexture();
        LoadPressedTexture();
    }

    private async void LoadPressedTexture() {
        try {
            if (pressedTexture == null && AddressablesHolder.pressedTexture.AssetGUID.Length > 0) {
                pressedTexture = await AddressablesHolder.pressedTexture.LoadAsync();
            }
        } catch (Exception e) {
            Debug.LogError($"Failed to load pressed image from {this} {e}");
        }
    }

    private async void LoadHoverTexture() {
        try {
            if (hoverTexture == null && AddressablesHolder.hoverTexture.AssetGUID.Length > 0) {
                hoverTexture = await AddressablesHolder.hoverTexture.LoadAsync();
            }
        } catch (Exception e) {
            Debug.LogError($"Failed to load hover image from {this} {e}");
        }
    }

    private async void LoadIdleTexture() {
        try {
            if (backgroundTexture == null && AddressablesHolder.backgroundTexture.AssetGUID.Length > 0) {
                backgroundTexture = await AddressablesHolder.backgroundTexture.LoadAsync();
                texture = backgroundTexture;
                if (overrideSize)
                    SetNativeSize();
            }
        } catch (Exception e) {
            Debug.LogError($"Failed to load background image from {this} {e}");
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
}