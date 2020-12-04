using UnityEngine;
using UnityEngine.UI;

public class CharSelectionController : MonoBehaviour {

    public GridLayoutGroup GridLayout;
    public GameObject charSelectionItem;

    // Start is called before the first frame update
    void Start() {
        var charInfo = Core.NetworkClient.State.CurrentCharactersInfo;
        for(var i = 0; i < charInfo.MaxSlots; i++) {
            var item = Instantiate(charSelectionItem);
            item.transform.parent = GridLayout.transform;

            var controller = item.GetComponent<CharacterCellController>();
            if(i < charInfo.Chars.Length) {
                controller.BindData(charInfo.Chars[i]);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
