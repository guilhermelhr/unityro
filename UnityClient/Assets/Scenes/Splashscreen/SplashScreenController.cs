using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour {

    [SerializeField]
    private AssetLabelReference[] LabelsToPrefetch;

    [SerializeField]
    private TextMeshProUGUI labelText;

    void Start() {
        StartCoroutine(PrefetchAssets());
    }

    private IEnumerator PrefetchAssets() {
#if !UNITY_EDITOR
        foreach (var label in LabelsToPrefetch) {
            var handle = Addressables.DownloadDependenciesAsync(label, true);

            while(!handle.IsDone) {
                var downloadStatus = handle.GetDownloadStatus();
                var downloadedMbs = downloadStatus.DownloadedBytes / 1024 / 1024;
                var totalMbs = downloadStatus.TotalBytes / 1024 / 1024;
                var progress = (downloadedMbs / totalMbs) * 100;

                var text = $"Downloading {label.labelString}\n{downloadedMbs}MB / {totalMbs}MB\n{progress}%";
                labelText.text = text;
                yield return null;
            }

            yield return handle;
        }
#endif
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("LoginScene");
    }
}
