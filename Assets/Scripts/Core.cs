using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public static Session Session;
    public static CursorRenderer CursorRenderer;

    public static PathFindingManager PathFinding => pathFinding;
    public static NetworkClient NetworkClient => networkClient;

    public static Action OnGrfLoaded;

    public static Core Instance;
    public static Camera MainCamera;
    public static long Tick => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

    public static Hashtable Configs = new Hashtable();
    private static string CFG_NAME = "config.txt";

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
        roCamEnabled = MainCamera.GetComponent<ROCamera>()?.enabled ?? false;

        LoadConfigs();

        LoadGrf();
        BuildMapSelector();
        DBManager.init();

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
            var entity = EntityManager.SpawnPlayer(new CharacterData() { Sex = 1, Job = 0, Name = "Player", GID = 20001, Weapon = 1 });
            entity.transform.position = new Vector3(150, 0, 150);
            Core.Session = new Session(entity, 0);

            Core.MainCamera.GetComponent<ROCamera>().SetTarget(Core.Session.Entity.EntityViewer.transform);
            Core.MainCamera.transform.SetParent(Core.Session.Entity.transform);

            Core.Session.Entity.SetReady(true);

            //var npc = EntityManager.Spawn(new EntityData() { job = 909, type = EntityType.NPC, PosDir = new int[] { 0, 0, 0 }, name = "NPC" });
            //npc.transform.position = new Vector3(160, 0, 150);
        }
    }

    private void BuildMapSelector() {
        Debug.Log("Building map list...");
        MapSelector selector = new MapSelector(FileManager.Grf);
        selector.buildDropdown(mapDropdown);
        Debug.Log($"Map list has {selector.GetMapList().Count} entries.");

        // do we have a map to load on startup
        var preLoadMap = !string.IsNullOrEmpty(mapname);

        // there is a map to load on startup: load it
        if (preLoadMap) {
            selector.ChangeMap(mapname);
        }

        // if a map is pre loaded, do not display map selector on startup
        mapDropdown?.gameObject?.SetActive(!preLoadMap);
    }

    private void LoadGrf() {
        Debug.Log($"Loading GRF at {Configs["grf"]} ...");
        FileManager.loadGrf(Configs["grf"] as string);
        Debug.Log($"GRF loaded, filetable contains {FileManager.Grf.files.Count} files.");
        OnGrfLoaded?.Invoke();
    }

    private void LoadConfigs() {

        string cfgTxt = null;
        if (Application.isMobilePlatform) {
            cfgTxt = "grf=" + Application.streamingAssetsPath + "/data.grf";
        } else {
            cfgTxt = FileManager.Load("config.txt") as string;

            if (cfgTxt == null) {
                FileStream stream = File.Open(Application.dataPath + "/" + CFG_NAME, FileMode.Create);

                string defaultCfg = "grf=" + Application.dataPath + "/data.grf";
                stream.Write(Encoding.UTF8.GetBytes(defaultCfg), 0, defaultCfg.Length);
                stream.Close();
                cfgTxt = defaultCfg;
            }
        }

        foreach (string s in cfgTxt.Split('\n')) {
            string[] properties = s.Split('=');
            if (properties.Length == 2) {
                Configs.Add(properties[0], properties[1]);
            }
        }
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
            MainCamera.GetComponent<ROCamera>().enabled = !mapSelectorEnabled && roCamEnabled;
            MainCamera.GetComponent<FreeflyCam>().enabled = !mapSelectorEnabled && !roCamEnabled;

            // disable entity when map selector is visible
            MainCamera.GetComponentInParent<Entity>().enabled = !mapSelectorEnabled;
        }

        // F1 pressed and not on map selector: switch between ROCamera and FreeflyCam
        else if (Input.GetKeyDown(KeyCode.F1) && !mapSelectorEnabled) {

            // switch cameras
            roCamEnabled = !roCamEnabled;
            MainCamera.GetComponent<ROCamera>().enabled = roCamEnabled;
            MainCamera.GetComponent<FreeflyCam>().enabled = !roCamEnabled;

            // handle cursor
            Cursor.lockState = roCamEnabled ? CursorLockMode.None : Cursor.lockState;
            Cursor.visible = roCamEnabled;

            // switched to ROCamera: updated it so we go from wherever
            // freefly is to where ROCam should be
            if (roCamEnabled) {
                MainCamera.GetComponent<ROCamera>().Start();
            }
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
        if (!MapRenderer.Ready && MapLoader.Progress != 0) return;
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
