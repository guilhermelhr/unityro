using UnityEngine;
using UnityEngine.AddressableAssets;

public class CustomUIAddressablesHolder : MonoBehaviour {
    public AssetReferenceTexture2D backgroundTexture;
    public AssetReferenceTexture2D hoverTexture;
    public AssetReferenceTexture2D pressedTexture;
    public AssetReferenceTexture2D disabledTexture;

    private void OnDestroy() {
        backgroundTexture.ReleaseAsset();
        hoverTexture.ReleaseAsset();
        pressedTexture.ReleaseAsset();
        disabledTexture.ReleaseAsset();
    }
}