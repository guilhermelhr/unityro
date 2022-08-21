using Assets.Scripts.Renderer.Map;
using ROIO;
using ROIO.Loaders;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
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
    #endregion

    public static Action OnGrfLoaded;
    public static Action OnMapLoaded;

    public Camera MainCamera { get; private set; }
    public static long Tick => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

    private Configuration Configs;

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
        Configs = ConfigurationLoader.Init();

        await LoadGrf();
    }

    private async Task LoadGrf() {
        FileManager.LoadGRF(Configs.root, Configs.grf);
        OnGrfLoaded?.Invoke();

        await DBManager.Init(Configs);

        InitManagers();
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
        var request = Resources.LoadAsync<AudioClip>(Path.Combine("Audio", "BGM", Path.GetFileNameWithoutExtension(name)));

        while (!request.isDone) {
            await Task.Yield();
        }

        AudioSource.clip = request.asset as AudioClip;
        //AudioSource.Play();
    }

    public async Task<GameMap> BeginMapLoading(string mapName) {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        MapRenderer.Clear();
        EntityManager.ClearEntities();

        AsyncMapLoader.GameMap gameMap = await new AsyncMapLoader().Load($"{mapName}.rsw");
        GameMap map = await MapRenderer.OnMapComplete(gameMap);

        SceneManager.UnloadSceneAsync("LoadingScene");
        OnMapLoaded?.Invoke();

        PlayBgm(Tables.MapTable[$"{mapName}.rsw"].mp3);
        return map;
    }

    //TODO Get rid of these
    #region Statics
    private static GameManager Instance;

    #endregion

    private void InitManagers() {
        new GameObject("ThreadManager").AddComponent<ThreadManager>();
        new GameObject("NetworkClient").AddComponent<NetworkClient>();
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
