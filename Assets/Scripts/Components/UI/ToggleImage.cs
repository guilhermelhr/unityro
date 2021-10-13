using ROIO;
using System;
using UnityEngine;
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

    public void Awake() {
        try {
            Image = GetComponent<Image>();
            Toggle = GetComponent<Toggle>();

            var NormalTexture = NormalImage != null ? FileManager.Load(DBManager.INTERFACE_PATH + NormalImage) as Texture2D : null;
            var HoverTexture = NormalImage != null ? FileManager.Load(DBManager.INTERFACE_PATH + HoverImage) as Texture2D : null;
            var PressedTexture = NormalImage != null ? FileManager.Load(DBManager.INTERFACE_PATH + PressedImage) as Texture2D : null;
            var SelectedTexture = NormalImage != null ? FileManager.Load(DBManager.INTERFACE_PATH + SelectedImage) as Texture2D : null;

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

    public void SetImage(string path, int index) {
        Index = index;
        var texture = path != null ? FileManager.Load(DBManager.INTERFACE_PATH + path) as Texture2D : null;
        InnerImage.texture = texture;
    }

    private Sprite CreateSprite(Texture2D texture) {
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
    }
}
