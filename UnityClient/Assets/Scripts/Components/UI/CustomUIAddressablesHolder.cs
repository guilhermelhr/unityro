using UnityEngine;
using UnityEngine.AddressableAssets;

public class CustomUIAddressablesHolder : MonoBehaviour {
    public AssetReferenceTexture2D backgroundTexture;
    public AssetReferenceTexture2D hoverTexture;
    public AssetReferenceTexture2D pressedTexture;
    public AssetReferenceTexture2D disabledTexture;
    public AssetReferenceTexture2D selectedTexture;

    private void OnDestroy() {
        if (backgroundTexture.Asset != null) {
            backgroundTexture.ReleaseAsset();
        }
        if (hoverTexture.Asset != null) {
            hoverTexture.ReleaseAsset();
        }
        if (pressedTexture.Asset != null) {
            pressedTexture.ReleaseAsset();
        }
        if (disabledTexture.Asset != null) {
            disabledTexture.ReleaseAsset();
        }
        if (selectedTexture.Asset != null) {
            selectedTexture.ReleaseAsset();
        }
    }
}