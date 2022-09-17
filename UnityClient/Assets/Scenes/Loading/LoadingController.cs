using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public Text ProgressText;
    public Slider Slider;
    public RawImage background;
    public RawImage spinner;

    private void Awake() {
        MapRenderer.OnProgress += OnProgress;
    }

    private async void Start() {
        await background.SetLoading();
    }

    private void OnDestroy() {
        MapRenderer.OnProgress -= OnProgress;
    }

    void Update() {
        spinner.transform.Rotate(Vector3.back * (20f * Time.deltaTime));
    }

    private void OnProgress(float progress) {
        Slider.value = progress;
        ProgressText.text = $"{(int) (progress * 100)}%";
    }
}
