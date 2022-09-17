using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleImage : MonoBehaviour {

    private Image Image;

    public Toggle Toggle;
    public RawImage InnerImage;
    public int Index { get; private set; }

    public string NormalImage;
    public string HoverImage;
    public string PressedImage;
    public string SelectedImage;

    public UnityEvent<int> onValueChanged;

    private CustomUIAddressablesHolder AddressablesHolder;

    public async void Awake() {
        AddressablesHolder = GetComponent<CustomUIAddressablesHolder>();
        try {
            Image = GetComponent<Image>();
            Toggle = GetComponent<Toggle>();

            var NormalTexture = await AddressablesHolder.backgroundTexture.LoadAssetAsync().Task;
            var HoverTexture = await AddressablesHolder.hoverTexture.LoadAssetAsync().Task;
            var PressedTexture = await AddressablesHolder.pressedTexture.LoadAssetAsync().Task;
            var SelectedTexture = await AddressablesHolder.selectedTexture.LoadAssetAsync().Task;

            var NormalSprite = NormalTexture != null ? CreateSprite(NormalTexture) : null;
            var HoverSprite = HoverTexture != null ? CreateSprite(HoverTexture) : null;
            var PressedSprite = PressedTexture != null ? CreateSprite(PressedTexture) : null;
            var SelectedSprite = SelectedTexture != null ? CreateSprite(SelectedTexture) : null;

            var spriteState = new UnityEngine.UI.SpriteState {
                highlightedSprite = HoverSprite,
                selectedSprite = SelectedSprite,
                pressedSprite = PressedSprite
            };

            Image.sprite = NormalSprite;
            Toggle.spriteState = spriteState;
            Toggle.onValueChanged.AddListener(OnValueChanged);
        } catch (Exception e) {
            Debug.LogError(e);
        }
    }

    private void OnValueChanged(bool isOn) {
        if (isOn) {
            onValueChanged?.Invoke(Index);
        }
    }

    public async void SetImage(string path, int index) {
        Index = index;
        var texture = path != null ? await Addressables.LoadAssetAsync<Texture2D>(DBManager.INTERFACE_PATH + path).Task : null;
        InnerImage.texture = texture;
    }

    private Sprite CreateSprite(Texture2D texture) {
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
    }
}
