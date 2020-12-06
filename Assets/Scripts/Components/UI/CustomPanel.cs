using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomPanel : RawImage,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler {

    [SerializeField] public string backgroundImage;
    [SerializeField] public string hoverImage;
    [SerializeField] public string pressedImage;

    private Texture2D backgroundTexture;
    private Texture2D hoverTexture;
    private Texture2D pressedTexture;

    override protected void Awake() {
        try {
            if(backgroundImage != null) {
                backgroundTexture = (Texture2D)FileManager.Load(DBManager.INTERFACE_PATH + backgroundImage);
                texture = backgroundTexture;
                GetComponent<RectTransform>().sizeDelta = new Vector2(backgroundTexture.width, backgroundTexture.height);
            }
            if(hoverImage != null) {
                hoverTexture = FileManager.Load(DBManager.INTERFACE_PATH + hoverImage) as Texture2D;
            }
            if(pressedImage != null) {
                pressedTexture = FileManager.Load(DBManager.INTERFACE_PATH + pressedImage) as Texture2D;
            }
        } catch {

        }
    }

    private void Update() {

    }

    public void OnPointerDown(PointerEventData eventData) {
        if(pressedTexture != null) {
            texture = pressedTexture;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(hoverTexture != null) {
            texture = hoverTexture;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(backgroundTexture != null) {
            texture = backgroundTexture;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if(hoverTexture != null) {
            texture = hoverTexture;
        }
    }
}
