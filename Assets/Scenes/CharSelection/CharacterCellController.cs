using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCellController : MonoBehaviour {

    private CharacterData data;

    public Text characterName;
    public SPRRenderer SPRRenderer;

    public Action<CharacterData> OnCharacterSelected;

    public void BindData(CharacterData data) {
        this.data = data;

        this.characterName.text = data.Name;
        var path = DBManager.GetBodyPath((Job)data.Job, data.Sex);
        SPR spr = FileManager.Load(path + ".spr") as SPR;
        SPRRenderer.setSPR(spr, 0, 0);
    }

    private void Update() {
        if(Input.GetMouseButtonUp(0)) {
            OnCharacterSelected?.Invoke(data);
        }
    }
}
