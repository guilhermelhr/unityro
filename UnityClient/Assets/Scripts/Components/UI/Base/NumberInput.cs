using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class NumberInput : MonoBehaviour {

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Button btnConfirm;

    [SerializeField]
    private TextMeshProUGUI lblTitle;

    private bool hasConfirmed = false;

    private void Update() {
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

    public Task<int> AwaitConfirmation() {
        hasConfirmed = false;
        var t = new TaskCompletionSource<int>();

        btnConfirm.onClick.AddListener(delegate {
            hasConfirmed = true;
            t.TrySetResult(int.Parse(inputField.text));
        });

        return t.Task;
    }
}
