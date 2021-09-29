using Assets.Scripts.Effects;
using ROIO;
using ROIO.Loaders;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityRO.GameCamera;

public class Core : MonoBehaviour {

    #region Inspector
    public bool Offline = false;
    public string mapname;
    public AudioMixerGroup soundsMixerGroup;
    public Light worldLight;
    public Dropdown mapDropdown;
    #endregion

    private static MapLoader mapLoader = new MapLoader();
    private static MapRenderer mapRenderer = new MapRenderer();

    private static PathFindingManager pathFinding = new PathFindingManager();
    private static NetworkClient networkClient;

    public static EntityManager EntityManager;
    public static ItemManager ItemManager;
    public static MapLoader MapLoader => mapLoader;
    public static MapRenderer MapRenderer => mapRenderer;
    public static CursorRenderer CursorRenderer;

    public static PathFindingManager PathFinding => pathFinding;
    public static NetworkClient NetworkClient => networkClient;

    public static Action OnGrfLoaded;

    public static Core Instance;
    public static Camera MainCamera;
    public static long Tick => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

    public static Configuration Configs;

    private bool roCamEnabled;

    private void Awake() {

        if (Instance == null) {
            Instance = this;
        }

        /**
         * Caching the camera as it's heavy to search for it
         */
        if (MainCamera == null) {
            MainCamera = Camera.main;
        }

        if (EntityManager == null) {
            EntityManager = gameObject.AddComponent<EntityManager>();
        }

        networkClient = GetComponent<NetworkClient>();

        DontDestroyOnLoad(this);
    }

    void Start() {
        MapRenderer.SoundsMixerGroup = soundsMixerGroup;
        MapRenderer.WorldLight = worldLight;
        roCamEnabled = true;

        Configs = ConfigurationLoader.Init();

        LoadGrf();
        DBManager.Init();

        if (CursorRenderer == null) {
            CursorRenderer = gameObject.AddComponent<CursorRenderer>();
        }

        /**
         * We start the network client only after the configs
         * have been loaded
         */
        if (!Offline) {
            NetworkClient.Start();
        } else {
            BeginMapLoading(mapname ?? "prontera");

            var entity = EntityManager.SpawnPlayer(new CharacterData() { Sex = 1, Job = 0, Name = "Player", GID = 20001, Weapon = 1, Speed = 150 });
            entity.transform.position = new Vector3(150, 0, 150);
            entity.SetAttackSpeed(135);
            Session.StartSession(new Session(entity, 0));

            //var mob = EntityManager.Spawn(new EntityData() { job = 1002, name = "Poring", GID = 20001, speed = 697, PosDir = new int[] { 0, 0, 0 }, objecttype = EntityType.MOB });
            //mob.transform.position = new Vector3(150, 0, 155);

            CharacterCamera charCam = FindObjectOfType<CharacterCamera>();
            charCam.SetTarget(entity.EntityViewer.transform);

            entity.SetReady(true);
            //mob.SetReady(true);

            //var str = FileManager.Load("data/texture/effect/magnificat.str") as STR;
            //var renderer = new GameObject().AddComponent<StrEffectRenderer>();
            //renderer.Initialize(str);
        }
    }

    private void LoadGrf() {
        FileManager.LoadGRF(Configs.root, Configs.grf);
        OnGrfLoaded?.Invoke();
    }

    void FixedUpdate() {
        if (mapRenderer.Ready) {
            mapRenderer.FixedUpdate();
        }
    }

    void Update() {
        if (mapRenderer.Ready) {
            mapRenderer.Render();
        }

        if (MainCamera == null) {
            MainCamera = Camera.main;
        }

        // is map selector enabled
        var mapSelectorEnabled = mapDropdown?.gameObject?.activeSelf ?? false;

        // ESC pressed: toggle map selector visiblity
        if (Input.GetKeyDown(KeyCode.Escape)) {

            // toggle map selector
            mapSelectorEnabled = !mapSelectorEnabled;
            mapDropdown.gameObject.SetActive(mapSelectorEnabled);

            // disable cameras when map selector is visible
            MainCamera.GetComponent<FreeflyCam>().enabled = !mapSelectorEnabled && !roCamEnabled;

            // disable entity when map selector is visible
            MainCamera.GetComponentInParent<Entity>().enabled = !mapSelectorEnabled;
        }

        // F1 pressed and not on map selector: switch between ROCamera and FreeflyCam
        else if (Input.GetKeyDown(KeyCode.F1) && !mapSelectorEnabled) {

            // switch cameras
            roCamEnabled = !roCamEnabled;
            MainCamera.GetComponent<FreeflyCam>().enabled = !roCamEnabled;

            // handle cursor
            Cursor.lockState = roCamEnabled ? CursorLockMode.None : Cursor.lockState;
            Cursor.visible = roCamEnabled;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            var entity = (Session.CurrentSession.Entity as Entity);
            CastingEffect.StartCasting(3, "data/texture/effect/ring_red.tga", entity.gameObject);
        } else if (Input.GetKeyDown(KeyCode.W)) {
            var entity = (Session.CurrentSession.Entity as Entity);
            var temp = new GameObject("Warp");
            temp.transform.position = entity.transform.position;
            MapWarpEffect.StartWarp(temp);
        }
    }

    public void OnPostRender() {
        if (mapRenderer.Ready) {
            mapRenderer.PostRender();
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
        StartCoroutine(
            MapLoader.Load(mapName + ".rsw", MapRenderer.OnComplete)
        );
    }

    public void InitManagers() {
        if (ItemManager == null) {
            ItemManager = gameObject.AddComponent<ItemManager>();
        }
    }
}
