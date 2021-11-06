using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public Text ProgressText;
    public Slider Slider;

    private void Awake() {
        MapRenderer.OnProgress += delegate (float progress) {
            OnProgress((int) progress);
        };
    }

    private void OnProgress(int progress) {
        Slider.value = progress;
        ProgressText.text = $"{progress}%";
    }
}
