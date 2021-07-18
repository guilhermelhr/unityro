using ROIO.GRF;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector {
    private LinkedList<string> mapList;
    private Dropdown dropdown;
    private static string currentMap;

    public static string CurrentMap {
        get { return currentMap; }
    }

    public MapSelector(Grf grf) {
        mapList = new LinkedList<string>();

        //build map list
        foreach(string key in grf.files.Keys) {
            if(key.EndsWith(".rsw", StringComparison.OrdinalIgnoreCase)) {
                mapList.AddLast(key);
            }
        }
    }

    public LinkedList<string> GetMapList() {
        return mapList;
    }

    public static string GetMapName(string path) {
        string mapName = path.Substring(path.LastIndexOf('/') + 1);
        return mapName.Replace(".rsw", "");
    }

    internal void buildDropdown(Dropdown dropdown) {
        if(dropdown == null) return;
        this.dropdown = dropdown;

        dropdown.ClearOptions();
        dropdown.options.Add(new Dropdown.OptionData("Select Map"));
        dropdown.captionText.text = "Select Map";

        string[] maps = new string[mapList.Count];
        int i = 0;
        foreach(string map in mapList) {
            maps[i++] = GetMapName(map);
        }
        Array.Sort(maps);

        foreach(string map in maps) {
            Dropdown.OptionData option = new Dropdown.OptionData(map);
            dropdown.options.Add(option);
        }

        dropdown.onValueChanged.AddListener(delegate {
            OnMapSelected();
        });

        dropdown.Select();
    }

    public void OnMapSelected() {
        string mapname = dropdown.captionText.text;

        if(!mapname.Equals("Select Map")) {
            ChangeMap(mapname);
        }
    }

    public void ChangeMap(string mapname) {
        Core.MapRenderer.Clear();
        currentMap = mapname;
        //float start = Time.realtimeSinceStartup;
        Core.Instance.StartCoroutine(
            Core.MapLoader.Load(mapname + ".rsw", Core.MapRenderer.OnComplete)
        );
        //float delta = Time.realtimeSinceStartup - start;
        //Debug.Log("Total load time: " + delta);
    }
}
