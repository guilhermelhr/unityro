using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public Text ProgressText;
    public Slider Slider;

    private void Awake() {
        MapRenderer.OnProgress += OnProgress;
    }

    private void OnDestroy() {
        MapRenderer.OnProgress -= OnProgress;
    }

    private void OnProgress(float progress) {
        Slider.value = progress;
        ProgressText.text = $"{(int)(progress * 100)}%";
    }
}
