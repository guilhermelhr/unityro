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

    private bool hasConfirmed = false;

    private void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            hasConfirmed = true;
        }
    }

    public void SetTitle(string title, int defaultValue = 0) {
        lblTitle.text = title;

        if (defaultValue > 0) {
            inputField.text = $"{defaultValue}";
        }
        inputField.ActivateInputField();
        inputField.Select();
    }

    public async Task<int> AwaitConfirmation() {
        hasConfirmed = false;
        btnConfirm.onClick.AddListener(delegate {
            hasConfirmed = true;
        });

        while (!hasConfirmed) {
            await Task.Delay(1);
        }

        return int.Parse(inputField.text);
    }
}
