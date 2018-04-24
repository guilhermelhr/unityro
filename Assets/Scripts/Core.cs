using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour {
    private const string defaultcfg = "map=prontera\n";
    private MapRenderer mapRenderer = new MapRenderer();
    public string mapName;
    public Dropdown mapDropdown;    
    private Hashtable configs = new Hashtable();
    private static string CFG_NAME = "config.txt";

    void Start() {
        string cfgTxt = FileManager.Load("config.txt") as string;
        if(cfgTxt == null) {
            FileStream stream = File.Open(Application.dataPath + "/" + CFG_NAME, FileMode.Create);
            stream.Write(Encoding.UTF8.GetBytes(defaultcfg), 0, defaultcfg.Length);
            stream.Close();
            cfgTxt = defaultcfg;
        }

        foreach(string s in cfgTxt.Split('\n')) {
            string[] properties = s.Split('=');
            if(properties.Length == 2) {
                configs.Add(properties[0], properties[1]);
            }
        }

        if(string.IsNullOrEmpty(mapName)) {
            mapName = configs["map"] as string;
        }

        string grfPath;
        if(configs.Contains("grf")) {
            grfPath = configs["grf"] as string;
        } else {
            grfPath = Application.dataPath + "/data.grf";
        }

        Grf grf = Grf.grf_callback_open(grfPath, "r", null);
        FileManager.setGrf(grf);

        MapSelector selector = new MapSelector(grf, mapRenderer);
        selector.buildDropdown(mapDropdown);
	}

    void Update() {
        mapDropdown.gameObject.SetActive(Cursor.lockState != CursorLockMode.Locked);
    }

    public void OnPostRender() {
        mapRenderer.Render();
    }

}
