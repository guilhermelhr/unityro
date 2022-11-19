#if !DUMP_RECEIVED_PACKET
//#define DUMP_RECEIVED_PACKET
#endif


using Assets.Scripts.Renderer.Map;
using ROIO;
using ROIO.Loaders;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Inspector
    [Header(":: Game Setup")]
    public bool OfflineOnly = false;
    public bool LocalHost = false;

    [Header(":: Rendering Setup")]
    public AudioMixerGroup BGMMixerGroup;
    public AudioMixerGroup EffectsMixerGroup;
    public Light WorldLight;

    public Camera CursorCamera;
    public Camera MainCamera;
    #endregion

    private MapLoader MapLoader;
    private MapRenderer MapRenderer;
    private AudioSource AudioSource;

    #region Components
    private EntityManager EntityManager;
    private NetworkClient NetworkClient;
    #endregion

    public RemoteConfiguration RemoteConfiguration { get; private set; }
    public LocalConfiguration LocalConfiguration { get; private set; }

    public static long Tick => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    public GameMap CurrentMap { get; private set; }

    private void Awake() {
        if (AudioSource == null) {
            AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSource.outputAudioMixerGroup = BGMMixerGroup;
        }

        Instance = this;

        DontDestroyOnLoad(this);
    }

    private void OnEnable() {
        SceneManager.activeSceneChanged += OnSceneChanged;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnDisable() {
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene arg0, Scene arg1) {
        MainCamera = Camera.main;
        var cameraObject = GameObject.FindGameObjectWithTag("CursorCamera");
        if (cameraObject != null) {
            CursorCamera = cameraObject.GetComponent<Camera>();
        }
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera) {
        OnPostRender();
    }

    void Start() {
        Init();
    }

    private void Init() {
        InitManagers();

        DBManager.Init();

        MaybeInitOfflineUtils();

        MapRenderer = new MapRenderer(EffectsMixerGroup, WorldLight);
        MapLoader = new MapLoader();
    }

    void FixedUpdate() {
        if (MapRenderer == null) {
            return;
        }

        if (MapRenderer.Ready) {
            MapRenderer.FixedUpdate();
        }
    }

    void Update() {
        if (MainCamera == null) {
            MainCamera = Camera.main;
        }
    }

    public void OnPostRender() {
        if (MapRenderer == null) {
            return;
        }

        if (MapRenderer.Ready) {
            MapRenderer.PostRender();
        }
    }

    public void SetWorldLight(Light worldLight) {
        MapRenderer.WorldLight = worldLight;
    }

    public void InitCamera() {
        MainCamera = Camera.main;
    }

    public void PlayBgm(string name) {
        var bgm = Addressables.LoadAssetAsync<AudioClip>(Path.Combine("bgm", name).SanitizeForAddressables()).WaitForCompletion();
        AudioSource.clip = bgm;
        AudioSource.Play();
    }

    /**
     * We do not want to pause/resume packet handling from here
     * That side effect can bring unwanted behaviour like being able to see
     * npcs vanishing and spawning
     * 
     * So we leave for the caller the responsibility to pause/resume
     */
    public async Task<GameMap> BeginMapLoading(string mapName) {
        await LoadScene("LoadingScene", LoadSceneMode.Additive);

        MapRenderer.Clear();
        if (CurrentMap != null) {
            Destroy(CurrentMap.gameObject);
        }

#if UNITY_EDITOR
        AsyncMapLoader.GameMapData gameMap = await new AsyncMapLoader().Load($"{mapName}.rsw");
        CurrentMap = await MapRenderer.OnMapComplete(gameMap);
#else
        var mapPrefab = await Addressables.LoadAssetAsync<GameObject>($"data/maps/{Path.GetFileNameWithoutExtension(mapName)}.prefab").Task;
        CurrentMap = Instantiate(mapPrefab).GetComponent<GameMap>();
#endif
        await UnloadScene("LoadingScene");

        PlayBgm(Tables.MapTable[$"{mapName}.rsw"].mp3);
        return CurrentMap;
    }

    public Task<bool> LoadScene(string sceneName, LoadSceneMode mode) {
        var t = new TaskCompletionSource<bool>();

        SceneManager.LoadSceneAsync(sceneName, mode).completed += delegate {
            t.TrySetResult(true);
        };

        return t.Task;
    }

    public Task<bool> UnloadScene(string sceneName) {
        var t = new TaskCompletionSource<bool>();

        SceneManager.UnloadSceneAsync(sceneName).completed += delegate {
            t.TrySetResult(true);
        };

        return t.Task;
    }

    public void SetConfigurations(RemoteConfiguration remoteConfiguration, LocalConfiguration localConfiguration) {
        RemoteConfiguration = remoteConfiguration;
        LocalConfiguration = localConfiguration;

#if UNITY_EDITOR
        if (LocalHost) {
            RemoteConfiguration.loginServer = "127.0.0.1";
        }
#endif
    }

    //TODO Get rid of these
    #region Statics
    private static GameManager Instance;

    #endregion

    private void InitManagers() {
        new GameObject("ThreadManager").AddComponent<ThreadManager>();
        NetworkClient = new GameObject("NetworkClient").AddComponent<NetworkClient>();
        EntityManager = new GameObject("EntityManager").AddComponent<EntityManager>();
        new GameObject("CursorRenderer").AddComponent<CursorRenderer>();
        new GameObject("GridRenderer").AddComponent<GridRenderer>();
        new GameObject("ItemManager").AddComponent<ItemManager>();
    }

    private void MaybeInitOfflineUtils() {
        if (!OfflineOnly) {
            return;
        }

        gameObject.AddComponent<OfflineUtility>();
    }
}
