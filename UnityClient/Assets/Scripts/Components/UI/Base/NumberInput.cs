using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class NumberInput : MonoBehaviour {

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Button button;

    public async Task<int> AwaitResult() {

        var hasClicked = false;
        button.onClick.AddListener(delegate { hasClicked = true; });
        await new Task(() => {
            while(!hasClicked) {
                continue;
            }
        });

        return int.Parse(inputField.text);
    }
}
