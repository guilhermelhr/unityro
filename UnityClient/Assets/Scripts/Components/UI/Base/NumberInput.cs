using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class NumberInput : MonoBehaviour {

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Button button;

    public async Task<int> AwaitConfirmation() {
        var hasClicked = false;
        button.onClick.AddListener(delegate {
            hasClicked = true;
        });

        while (!hasClicked) {
            await Task.Delay(1);
        }

        return int.Parse(inputField.text);
    }
}
