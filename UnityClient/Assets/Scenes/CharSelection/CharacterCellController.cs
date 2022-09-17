using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterCellController : MonoBehaviour, IPointerClickHandler {

    private CharacterData data;

    public Text characterName;

    public bool IsEmpty => data == null;

    public Action<CharacterData> OnCharacterSelected;

    public void BindData(CharacterData data) {
        this.data = data;

        characterName.text = data.Name;

        GameObject player = new GameObject(data.Name);
        player.layer = LayerMask.NameToLayer("Characters");
        player.transform.SetParent(this.transform);
        player.transform.localScale = new Vector3(30f, 30f, 1f);
        player.transform.localPosition = new Vector3(0, -40f, 0f);

        Entity entity = player.AddComponent<Entity>();
        entity.Init(data, LayerMask.NameToLayer("Characters"), null, true);
        entity.SetReady(true, true);
    }
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            OnCharacterSelected?.Invoke(data);
        }
    }
}
