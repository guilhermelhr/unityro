using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CustomUIAddressablesHolder), typeof(RawImage))]
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
        if (rawImage == null) {
            rawImage = GetComponent<RawImage>();
        }

        rawImage.texture = null;

        if (AddressablesHolder == null) {
            AddressablesHolder = GetComponent<CustomUIAddressablesHolder>();
        }

        if (AddressablesHolder.backgroundTexture.Asset != null) {
            rawImage.texture = (Texture2D) AddressablesHolder.backgroundTexture.Asset;
        }
    }

    protected override void Start() {
        rawImage.texture = null;
        LoadTextures();
    }

    private void LoadTextures() {
        LoadIdleTexture();
        LoadHoverTexture();
        LoadPressedTexture();
    }

    private void LoadPressedTexture() {
        try {
            if (pressedTexture == null && AddressablesHolder.pressedTexture.AssetGUID.Length > 0) {
                pressedTexture = AddressablesHolder.pressedTexture.LoadAssetAsync().WaitForCompletion();
            }
        } catch (Exception e) {
            Debug.LogError($"Failed to load pressed image from {this} {e}");
        }
    }

    private async void LoadHoverTexture() {
        try {
            if (hoverTexture == null && AddressablesHolder.hoverTexture.AssetGUID.Length > 0) {
                hoverTexture = AddressablesHolder.hoverTexture.LoadAssetAsync().WaitForCompletion();
            }
        } catch (Exception e) {
            Debug.LogError($"Failed to load hover image from {this} {e}");
        }
    }

    private void LoadIdleTexture() {
        try {
            if (backgroundTexture == null && AddressablesHolder.backgroundTexture.AssetGUID.Length > 0) {
                backgroundTexture = AddressablesHolder.backgroundTexture.LoadAssetAsync().WaitForCompletion();
                rawImage.texture = backgroundTexture;
            }
        } catch (Exception e) {
            Debug.LogError($"Failed to load background image from {this} {e}");
        }
    }

    override public void OnPointerDown(PointerEventData eventData) {
        if (pressedTexture == null)
            return;
        rawImage.texture = pressedTexture;
    }

    override public void OnPointerUp(PointerEventData eventData) {
        if (hoverTexture == null)
            return;
        rawImage.texture = hoverTexture;
    }

    override public void OnPointerEnter(PointerEventData pointerEventData) {
        if (hoverTexture == null)
            return;
        rawImage.texture = hoverTexture;
    }

    override public void OnPointerExit(PointerEventData pointerEventData) {
        if (backgroundTexture == null)
            return;
        rawImage.texture = backgroundTexture;
    }

    override public void OnSelect(BaseEventData eventData) {
        if (hoverTexture == null)
            return;
        rawImage.texture = hoverTexture;
    }

    override public void OnDeselect(BaseEventData eventData) {
        if (backgroundTexture == null)
            return;
        rawImage.texture = backgroundTexture;
    }
}
