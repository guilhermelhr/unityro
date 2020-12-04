using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCellController : MonoBehaviour {

    public Text characterName;
    private CharacterData data;

    public void BindData(CharacterData data) {
        this.data = data;

        this.characterName.text = data.Name;
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
