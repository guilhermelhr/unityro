using Assets.Scripts.Effects;
using ROIO;
using ROIO.Loaders;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Inspector
    [Header(":: Game Setup")]
    public bool OfflineOnly = false;
    public string OfflineMapname;

    [Header(":: Rendering Setup")]
    public AudioMixerGroup SoundMixerGroup;
    public Light WorldLight;
    #endregion

    private MapLoader MapLoader;
    private MapRenderer MapRenderer;

    #region Components
    private EntityManager EntityManager;
    private PathFinder PathFinder;
    #endregion

    public static Action OnGrfLoaded;

    public Camera MainCamera { get; private set; }
    public static long Tick => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

    private Configuration Configs;

    private void Awake() {
        if (MainCamera == null) {
            MainCamera = Camera.main;
        }

        DontDestroyOnLoad(this);

        Instance = this;
    }

    void Start() {
        Configs = ConfigurationLoader.Init();

        LoadGrf();
        DBManager.Init(Configs);
    }

    private void LoadGrf() {
        FileManager.LoadGRF(Configs.root, Configs.grf);
        OnGrfLoaded?.Invoke();
        InitManagers();

        MapRenderer = new MapRenderer(this, PathFinder, SoundMixerGroup, WorldLight);
        MapLoader = new MapLoader();
    }

    void FixedUpdate() {
        if (MapRenderer.Ready) {
            MapRenderer.FixedUpdate();
        }
    }

    void Update() {
        if (MainCamera == null) {
            MainCamera = Camera.main;
        }

        if (MapRenderer.Ready) {
            MapRenderer.Render();
        }
    }

    public void OnPostRender() {
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

    public void BeginMapLoading(string mapName) {
        if (!MapRenderer.Ready && MapLoader.Progress != 0)
            return;
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        MapRenderer.Clear();
        EntityManager.ClearEntities();
        StartCoroutine(
            MapLoader.Load(mapName + ".rsw", MapRenderer.OnComplete)
        );
    }

    //TODO Get rid of these
    #region Statics
    private static GameManager Instance;
    public static float GetMapLoaderProgress() => Instance.MapLoader.Progress;
    public static void IncreaseMapLoadingProgress(float val) {
        Instance.MapLoader.Progress += val;
    }
    public static bool IsMapRendererReady() => Instance.MapRenderer.Ready;
    public static void ResetMapLoadingProgress() {
        Instance.MapLoader.Progress = 0;
    }
    #endregion

    private void InitManagers() {
        new GameObject("ThreadManager").AddComponent<ThreadManager>();
        new GameObject("NetworkClient").AddComponent<NetworkClient>();
        EntityManager = new GameObject("EntityManager").AddComponent<EntityManager>();
        PathFinder = new GameObject("PathFinder").AddComponent<PathFinder>();
        new GameObject("CursorRenderer").AddComponent<CursorRenderer>();
        new GameObject("GridRenderer").AddComponent<GridRenderer>();
        new GameObject("ItemManager").AddComponent<ItemManager>();
        new GameObject("ShaderCache").AddComponent<ShaderCache>();
    }
}
