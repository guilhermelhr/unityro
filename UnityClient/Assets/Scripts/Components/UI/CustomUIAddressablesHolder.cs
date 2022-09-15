using UnityEngine;
using UnityEngine.AddressableAssets;

public class CustomUIAddressablesHolder : MonoBehaviour {
    public AssetReferenceTexture2D backgroundTexture;
    public AssetReferenceTexture2D hoverTexture;
    public AssetReferenceTexture2D pressedTexture;
    public AssetReferenceTexture2D disabledTexture;
    public AssetReferenceTexture2D selectedTexture;

    private void OnDestroy() {
        backgroundTexture.ReleaseAsset();
        hoverTexture.ReleaseAsset();
        pressedTexture.ReleaseAsset();
        disabledTexture.ReleaseAsset();
        selectedTexture.ReleaseAsset();
    }
}