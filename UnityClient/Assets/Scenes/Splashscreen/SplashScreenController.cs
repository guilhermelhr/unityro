using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ROIO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
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

        yield return new WaitForSeconds(1f);

        StartCoroutine(FetchConfigs());
    }

    private IEnumerator FetchConfigs() {
        var localRequest = Addressables.LoadAssetAsync<TextAsset>("LocalConfigs.json.txt");
        yield return localRequest;

        var localConfig = JObject.Parse(localRequest.Result.text);
        var localConfiguration = JsonConvert.DeserializeObject<LocalConfiguration>(localConfig.ToString());

        var remoteRequest = UnityWebRequest.Get(localConfiguration.remoteConfigLocation);
        yield return remoteRequest.SendWebRequest();

        var remoteConfig = JObject.Parse(remoteRequest.downloadHandler.text);
        var remoteConfiguration = JsonConvert.DeserializeObject<RemoteConfiguration>(remoteConfig.ToString());

        FindObjectOfType<GameManager>().SetConfigurations(remoteConfiguration, localConfiguration);

        SceneManager.LoadScene("LoginScene");
    }
}
