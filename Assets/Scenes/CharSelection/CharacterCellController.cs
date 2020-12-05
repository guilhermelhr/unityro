using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCellController : MonoBehaviour {

    private CharacterData data;

    public Text characterName;
    public Image image;

    public Action<CharacterData> OnCharacterSelected;

    public void BindData(CharacterData data) {
        this.data = data;

        this.characterName.text = data.Name;
        var path = DBManager.GetBodyPath((Job)data.Job, data.Sex);
        SPR spr = FileManager.Load(path + ".spr") as SPR;
        var sprite = spr.GetSprites()[0];
        image.sprite = sprite;
    }

    private void Update() {
        if(Input.GetMouseButtonUp(0)) {
            OnCharacterSelected?.Invoke(data);
        }
    }
}
