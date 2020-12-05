using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public Text ProgressText;
    public Slider Slider;

    private void Awake() {
        Core.MapLoader.OnProgress += OnProgress;
        Core.MapRenderer.OnMapLoaded += OnMapLoaded;
    }

    private void OnMapLoaded(GameObject world) {
        Core.MapLoader.OnProgress -= OnProgress;
        Core.MapRenderer.OnMapLoaded -= OnMapLoaded;
        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    private void OnProgress(int progress) {
        Slider.value = progress;
        ProgressText.text = $"{progress}%";
    }
}
