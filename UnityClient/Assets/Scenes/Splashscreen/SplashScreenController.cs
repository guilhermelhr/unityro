using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ROIO;
using ROIO.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenController : MonoBehaviour {

    [SerializeField]
    private AssetLabelReference[] LabelsToPrefetch;

    [SerializeField]
    private TextMeshProUGUI labelText;

    [SerializeField]
    private TextMeshProUGUI DownloadSizeText;

    [SerializeField]
    private Slider Slider;

    void Start() {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() {
        labelText.text = "Checking for updates...";
        yield return Addressables.InitializeAsync();
        yield return new WaitForSeconds(1f);

        StartCoroutine(PrefetchAssets());
    }

    private IEnumerator PrefetchAssets() {
        var downloadSize = Addressables.GetDownloadSizeAsync(LabelsToPrefetch).WaitForCompletion();

        if (downloadSize <= 0) {
            yield return FetchConfigs();
        }

        foreach (var label in LabelsToPrefetch) {
            var handle = Addressables.DownloadDependenciesAsync(label, true);

            while(!handle.IsDone) {
                var downloadStatus = handle.GetDownloadStatus();
                var downloadedMbs = downloadStatus.DownloadedBytes / 1024f / 1024f;
                var totalMbs = (downloadStatus.TotalBytes / 1024f / 1024f);

                var progress = Conversions.SafeDivide(downloadedMbs, totalMbs);

                var text = $"Downloading {label.labelString}";
                labelText.text = text;
                DownloadSizeText.text = $"{downloadedMbs}MB / {totalMbs}MB";
                Slider.value = progress;

                yield return null;
            }

            yield return handle;
        }

        yield return FetchConfigs();
    }

    private IEnumerator FetchConfigs() {
        labelText.text = "Fetching remote configuration...";
        var localRequest = Resources.Load<TextAsset>("Configuration/LocalConfigs.json");
        yield return localRequest;

        var localConfiguration = JsonConvert.DeserializeObject<LocalConfiguration>(JObject.Parse(localRequest.text).ToString());

        var remoteRequest = UnityWebRequest.Get(localConfiguration.remoteConfigLocation);
        yield return remoteRequest.SendWebRequest();

        var remoteConfiguration = JsonConvert.DeserializeObject<RemoteConfiguration>(JObject.Parse(remoteRequest.downloadHandler.text).ToString());

        labelText.text = "Initializing GameManager...";

        InitializeGameManager(remoteConfiguration, localConfiguration);
    }

    private async void InitializeGameManager(RemoteConfiguration remoteConfiguration, LocalConfiguration localConfiguration) {
        var gameManager = FindObjectOfType<GameManager>();
        gameManager.SetConfigurations(remoteConfiguration, localConfiguration);
        await gameManager.Init();

        await gameManager.LoadScene("LoginScene", LoadSceneMode.Single);
    }
}
