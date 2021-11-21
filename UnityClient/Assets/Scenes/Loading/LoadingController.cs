using ROIO.Loaders;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public Text ProgressText;
    public Slider Slider;
    public RawImage background;

    private void Awake() {
        MapLoader.OnProgress += OnProgress;
    }

    private void Start() {
        background.SetLoading();
    }

    private void OnMapLoaded() {
        MapLoader.OnProgress -= OnProgress;
        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    private void OnProgress(int progress) {
        Slider.value = progress;
        ProgressText.text = $"{progress}%";

        if (progress == 100 && GameManager.IsMapRendererReady()) {
            GameManager.ResetMapLoadingProgress();
            OnMapLoaded();
        }
    }
}
