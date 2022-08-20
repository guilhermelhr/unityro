using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour {

    [SerializeField]
    private AssetLabelReference[] LabelsToPrefetch;

    [SerializeField]
    private TextMeshProUGUI labelText;

    AsyncOperationHandle<IList<GameObject>> loadHandle;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(PrefetchAssets());
    }

    private IEnumerator PrefetchAssets() {
        loadHandle = Addressables.LoadAssetsAsync<GameObject>(LabelsToPrefetch, null, Addressables.MergeMode.Union);

        while (!loadHandle.IsDone) {
            var downloadStatus = loadHandle.GetDownloadStatus();
            var text = $"Downloading - {downloadStatus.DownloadedBytes / 1024 / 1024}kB / {downloadStatus.TotalBytes / 1024 / 1024}MB ({loadHandle.PercentComplete}%)";
            labelText.text = text;
            yield return null;
        }

        yield return loadHandle;
    }

    void OnDestroy() {
        Addressables.Release(loadHandle);
    }


    //void Awake() {
    //    GameManager.OnGrfLoaded += OnGrfLoaded;
    //}

    //private void OnGrfLoaded() {
    //    SceneManager.LoadScene("LoginScene");
    //}

    //// Update is called once per frame
    //void Destroy() {
    //    GameManager.OnGrfLoaded -= OnGrfLoaded;
    //}
}
