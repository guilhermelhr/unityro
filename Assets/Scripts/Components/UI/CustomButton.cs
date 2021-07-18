using B83.Image.BMP;
using ROIO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    ISelectHandler {

    [SerializeField] public string backgroundImage;
    [SerializeField] public string hoverImage;
    [SerializeField] public string pressedImage;

    private Texture2D backgroundBMP;
    private Texture2D hoverBMP;
    private Texture2D pressedBMP;
    private RawImage rawImage;

    protected override void Awake() {
        enabled = true;
        rawImage = GetComponent<RawImage>();
        try {
            if(backgroundImage != null) {
                backgroundBMP = (Texture2D)FileManager.Load(DBManager.INTERFACE_PATH + backgroundImage);
                rawImage.texture = backgroundBMP;
                //GetComponent<RectTransform>().sizeDelta = new Vector2(backgroundBMP.width, backgroundBMP.height);
            }
            if(hoverImage != null) {
                hoverBMP = FileManager.Load(DBManager.INTERFACE_PATH + hoverImage) as Texture2D;
            }
            if(pressedImage != null) {
                pressedBMP = FileManager.Load(DBManager.INTERFACE_PATH + pressedImage) as Texture2D;
            }
        } catch { }
    }

    override public void OnPointerDown(PointerEventData eventData) {
        rawImage.texture = pressedBMP;
    }

    override public void OnPointerUp(PointerEventData eventData) {
        rawImage.texture = hoverBMP;
    }

    override public void OnPointerEnter(PointerEventData pointerEventData) {
        //Output to console the GameObject's name and the following message
        rawImage.texture = hoverBMP;
    }

    //Detect when Cursor leaves the GameObject
    override public void OnPointerExit(PointerEventData pointerEventData) {
        //Output the following message with the GameObject's name
        rawImage.texture = backgroundBMP;
    }

    override public void OnSelect(BaseEventData eventData) {
		rawImage.texture = hoverBMP;
	}

    override public void OnDeselect(BaseEventData eventData) {
        rawImage.texture = backgroundBMP;
    }
}
