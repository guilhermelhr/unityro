using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class NumberInput : MonoBehaviour {

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Button btnConfirm;

    [SerializeField]
    private TextMeshProUGUI lblTitle;

    public void SetTitle(string title) {
        lblTitle.text = title;
    }

    public async Task<int> AwaitConfirmation() {
        var hasClicked = false;
        btnConfirm.onClick.AddListener(delegate {
            hasClicked = true;
        });

        while (!hasClicked) {
            await Task.Delay(1);
        }

        return int.Parse(inputField.text);
    }
}
