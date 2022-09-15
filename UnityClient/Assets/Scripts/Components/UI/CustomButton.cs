using B83.Image.BMP;
using ROIO;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    ISelectHandler {

    public string backgroundImage;
    public string hoverImage;
    public string pressedImage;

    private Texture2D backgroundTexture;
    private Texture2D hoverTexture;
    private Texture2D pressedTexture;
    private RawImage rawImage;

    private CustomUIAddressablesHolder AddressablesHolder;

    protected override void OnEnable() {
        rawImage = GetComponent<RawImage>();
        AddressablesHolder = GetComponent<CustomUIAddressablesHolder>();
    }

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
            if (pressedTexture == null) {
                pressedTexture = await AddressablesHolder.pressedTexture.LoadAssetAsync().Task;
            }
        } catch (Exception e) {
            Debug.LogError($"Failed to load pressed image from {this} {e}");
        }
    }

    private async void LoadHoverTexture() {
        try {
            if (hoverTexture == null) {
                hoverTexture = await AddressablesHolder.hoverTexture.LoadAssetAsync().Task;
            }
        } catch (Exception e) {
            Debug.LogError($"Failed to load hover image from {this} {e}");
        }
    }

    private async void LoadIdleTexture() {
        try {
            if (backgroundTexture == null) {
                backgroundTexture = await AddressablesHolder.backgroundTexture.LoadAssetAsync().Task;
                rawImage.texture = backgroundTexture;
            }
        } catch (Exception e) {
            Debug.LogError($"Failed to load background image from {this} {e}");
        }
    }

    override public void OnPointerDown(PointerEventData eventData) {
        rawImage.texture = pressedTexture;
    }

    override public void OnPointerUp(PointerEventData eventData) {
        rawImage.texture = hoverTexture;
    }

    override public void OnPointerEnter(PointerEventData pointerEventData) {
        rawImage.texture = hoverTexture;
    }

    override public void OnPointerExit(PointerEventData pointerEventData) {
        rawImage.texture = backgroundTexture;
    }

    override public void OnSelect(BaseEventData eventData) {
		rawImage.texture = hoverTexture;
	}

    override public void OnDeselect(BaseEventData eventData) {
        rawImage.texture = backgroundTexture;
    }
}
