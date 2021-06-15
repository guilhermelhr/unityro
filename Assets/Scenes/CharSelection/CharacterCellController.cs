using ROIO;
using ROIO.Models.FileTypes;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterCellController : MonoBehaviour, IPointerClickHandler {

    private CharacterData data;

    public Text characterName;
    public Image image;

    public bool IsEmpty => data == null;

    public Action<CharacterData> OnCharacterSelected;

    public void BindData(CharacterData data) {
        this.data = data;

        this.characterName.text = data.Name;
        var path = DBManager.GetBodyPath((Job)data.Job, data.Sex);
        SPR spr = FileManager.Load(path + ".spr") as SPR;
        var sprite = spr.GetSprites()[0];
        image.sprite = sprite;
        image.preserveAspect = true;
    }
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            OnCharacterSelected?.Invoke(data);
        }
    }
}
