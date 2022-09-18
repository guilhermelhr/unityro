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

    [Header(":: Rendering Setup")]
    public AudioMixerGroup SoundMixerGroup;
    public Light WorldLight;
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
    public Camera MainCamera { get; private set; }
    public GameMap CurrentMap { get; private set; }

    private void Awake() {
        if (MainCamera == null) {
            MainCamera = Camera.main;
        }
        if (AudioSource == null) {
            AudioSource = gameObject.AddComponent<AudioSource>();
        }

        DontDestroyOnLoad(this);

        Instance = this;
    }

    private void OnEnable() {
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnDisable() {
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera) {
        OnPostRender();
    }

    async void Start() {
        await Init();
    }

    private async Task Init() {
        InitManagers();

        await DBManager.Init();

        MaybeInitOfflineUtils();

        MapRenderer = new MapRenderer(SoundMixerGroup, WorldLight);
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

    public async void PlayBgm(string name) {
        var bgm = await Addressables.LoadAssetAsync<AudioClip>(Path.Combine("bgm", name).SanitizeForAddressables()).Task;
        AudioSource.clip = bgm;
        //AudioSource.Play();
    }

    public async Task<GameMap> BeginMapLoading(string mapName) {
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

        NetworkClient.PausePacketHandling();

        MapRenderer.Clear();
        EntityManager.ClearEntities();
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

        NetworkClient.ResumePacketHandling();
        SceneManager.UnloadSceneAsync("LoadingScene");

        PlayBgm(Tables.MapTable[$"{mapName}.rsw"].mp3);
        return CurrentMap;
    }

    public void SetConfigurations(RemoteConfiguration remoteConfiguration, LocalConfiguration localConfiguration) {
        RemoteConfiguration = remoteConfiguration;
        LocalConfiguration = localConfiguration;
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
