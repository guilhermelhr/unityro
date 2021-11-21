using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public Text ProgressText;
    public Slider Slider;
    public RawImage background;

    private void Awake() {
        MapRenderer.OnProgress += OnProgress;
    }

    private void Start() {
        background.SetLoading();
    }

    private void OnDestroy() {
        MapRenderer.OnProgress -= OnProgress;
    }

    private void OnProgress(float progress) {
        Slider.value = progress;
        ProgressText.text = $"{(int)(progress * 100)}%";
    }
}
