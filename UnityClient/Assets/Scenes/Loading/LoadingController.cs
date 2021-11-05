using ROIO.Loaders;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public Text ProgressText;
    public Slider Slider;

    private void OnProgress(int progress) {
        Slider.value = progress;
        ProgressText.text = $"{progress}%";
    }
}
