using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Core : MonoBehaviour {
    private static MapLoader mapLoader = new MapLoader();
    private static MapRenderer mapRenderer = new MapRenderer();

    private static PathFindingManager pathFinding = new PathFindingManager();

    public static MapLoader MapLoader {
        get { return mapLoader; }
    }

    public static MapRenderer MapRenderer {
        get { return mapRenderer; }
    }

    public static PathFindingManager PathFinding {
        get { return pathFinding; }
    }

    public static Action OnGrfLoaded;

    public static Core Instance;
    public static Camera MainCamera;

    public string mapname;
    public AudioMixerGroup soundsMixerGroup;
    public Light worldLight;
    public Dropdown mapDropdown;

    private Hashtable configs = new Hashtable();
    private static string CFG_NAME = "config.txt";

    private bool isMapSelectorEnabled = false;
    private bool isRoCamEnabled = false;

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
    }

    void Start() {
        MapRenderer.SoundsMixerGroup = soundsMixerGroup;
        MapRenderer.WorldLight = worldLight;

        LoadConfigs();
        LoadGrf();
        BuildMapSelector();
    }

    private void BuildMapSelector() {
        Debug.Log("Building map list...");
        MapSelector selector = new MapSelector(FileManager.Grf);
        selector.buildDropdown(mapDropdown);
        Debug.Log($"Map list has {selector.GetMapList().Count} entries.");

        if (!string.IsNullOrEmpty(mapname)) {
            selector.ChangeMap(mapname);
        }
    }

    private void LoadGrf() {
        Debug.Log($"Loading GRF at {configs["grf"]} ...");
        FileManager.loadGrf(configs["grf"] as string);
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
                configs.Add(properties[0], properties[1]);
            }
        }
    }

    void FixedUpdate() {
        if (mapRenderer.Ready) {
            mapRenderer.FixedUpdate();
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isMapSelectorEnabled = !isMapSelectorEnabled;
            mapDropdown.gameObject.SetActive(isMapSelectorEnabled);
        } else if (Input.GetKeyDown(KeyCode.F1)) {
            isRoCamEnabled = !isRoCamEnabled;
            MainCamera.GetComponent<ROCamera>().enabled = isRoCamEnabled;
            MainCamera.GetComponent<FreeflyCam>().enabled = !isRoCamEnabled;

            Cursor.lockState = isRoCamEnabled ? CursorLockMode.None : Cursor.lockState;
            Cursor.visible = isRoCamEnabled;
        }


        if (mapRenderer.Ready) {
            mapRenderer.Render();
        }
    }

    public void OnPostRender() {
        if (mapRenderer.Ready) {
            mapRenderer.PostRender();
        }
    }
}
