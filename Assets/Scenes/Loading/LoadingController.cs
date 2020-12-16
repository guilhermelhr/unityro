using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public Text ProgressText;
    public Slider Slider;

    private void Awake() {
        Core.MapLoader.OnProgress += OnProgress;
    }

    private void OnMapLoaded() {
        Core.MapLoader.OnProgress -= OnProgress;
        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    private void OnProgress(int progress) {
        Slider.value = progress;
        ProgressText.text = $"{progress}%";

        if (progress == 100 && Core.MapRenderer.Ready) {
            Core.MapLoader.Progress = 0;
            OnMapLoaded();
        }
    }
}
