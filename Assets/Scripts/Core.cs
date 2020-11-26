using System;
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
            path = PathFindingManager.Instance.GetPath((int) entity.transform.position.x, (int)entity.transform.position.z, (int)worldPosition.x, (int)worldPosition.z);

            StartCoroutine(Move(path));
        }
    }

    IEnumerator Move(List<PathNode> path) {
        foreach (var node in path) {
            var MoveToIE = StartCoroutine(MoveTo(node));
            yield return MoveToIE;
        }
    }

    IEnumerator MoveTo(PathNode node) {
        var destination = new Vector3(node.x, node.y, node.z);
        while (entity.transform.position != destination) {
            entity.transform.position = Vector3.MoveTowards(entity.transform.position, destination, 8 * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos() {
        //PathFindingManager.Instance.DebugNodes();
        if (path != null) {
            Gizmos.color = Color.red;
            for (int i = 0; i < path.Count; i++) {
                var node = path[i];
                var origin = new Vector3(node.x, node.y, node.z);
                var dest = origin;
                if (i + 1 < path.Count) {
                    var nextNode = path[i + 1];
                    dest = new Vector3(nextNode.x, nextNode.y, nextNode.z);
                }
                Gizmos.DrawLine(origin, dest);

            }
        }
    }

    public void OnPostRender() {
        if (mapRenderer.Ready) {
            mapRenderer.PostRender();
        }
    }
}
