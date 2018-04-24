
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapSelector
{
    private LinkedList<string> mapList;
    private readonly MapRenderer mapRenderer;
    private Dropdown dropdown;

    public MapSelector(Grf grf, MapRenderer mapRenderer) {
        this.mapRenderer = mapRenderer;
        mapList = new LinkedList<string>();
        
        //build map list
        foreach(string key in grf.files.Keys){
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
            mapRenderer.SetMap(mapname + ".rsw");
        }
    }
}
