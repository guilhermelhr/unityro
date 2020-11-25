using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Core : MonoBehaviour {
    private static MapLoader mapLoader = new MapLoader();
    private static MapRenderer mapRenderer = new MapRenderer();

    private PathFindingManager pathFinding = new PathFindingManager();

    public static MapLoader MapLoader {
        get { return mapLoader; }
    }

    public static MapRenderer MapRenderer {
        get { return mapRenderer; }
    }

    public static Core Instance;

    public string mapname;
    public GameObject entity;
    public AudioMixerGroup soundsMixerGroup;
    public Light worldLight;
    public Dropdown mapDropdown;

    private Hashtable configs = new Hashtable();
    private static string CFG_NAME = "config.txt";

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start() {
        new TestPathfinding();
        loadConfigs();

        Debug.Log("Loading GRF at " + configs["grf2"] + "...");
        FileManager.loadGrf(configs["grf"] as string);
        Debug.Log("GRF loaded, filetable contains " + FileManager.Grf.files.Count + " files.");

        Debug.Log("Building map list...");
        MapSelector selector = new MapSelector(FileManager.Grf);
        selector.buildDropdown(mapDropdown);
        Debug.Log("Map list has " + selector.GetMapList().Count + " entries.");

        MapRenderer.SoundsMixerGroup = soundsMixerGroup;
        MapRenderer.WorldLight = worldLight;

        if (!string.IsNullOrEmpty(mapname)) {
            selector.ChangeMap(mapname);
        }
    }

    private void loadConfigs() {

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

    List<PathNode> path;

    void Update() {
        //mapDropdown.gameObject.SetActive(Cursor.lockState != CursorLockMode.Locked);
        if (mapRenderer.Ready) {
            mapRenderer.Render();
        }

        if (Input.GetMouseButtonDown(0)) {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            path = PathFindingManager.Instance.GetPath(50, 50, (int) worldPosition.x, (int) worldPosition.z);

            foreach (var node in path) {
                //var origin = new Vector3(node.x, 5f, node.y);
                //Debug.DrawLine(origin, new Vector3(1, 5, 1));
                //    var moveDir = (new Vector3(node.x, 0.5f, node.y) - entity.transform.position).normalized;
                //    entity.transform.position = entity.transform.position + moveDir * 2f * Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmos() {
        PathFindingManager.Instance.DebugNodes();
        if (path != null) {
            foreach (var node in path) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector3(node.x, 5f, node.y), new Vector3(node.x + 1, 5f, node.y + 1));
            }
        }
    }

    public void OnPostRender() {
        if (mapRenderer.Ready) {
            mapRenderer.PostRender();
        }
    }
}
