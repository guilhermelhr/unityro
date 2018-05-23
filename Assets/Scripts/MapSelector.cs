using ProceduralLevel.PowerConsole.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : AConsoleCommand
{
    private static string currentMap;
    private static string[] maps;
    private static MapHint mapHint = new MapHint();

    public static string CurrentMap {
        get { return currentMap; }
    }

    public MapSelector(ConsoleInstance console, string name, string description, bool isOption = false)
        : base(console, name, description, isOption) {
    }

    private class MapHint : ACollectionHint<string>
    {
        protected override string[] GetAllOptions() {
            return maps;
        }
    }

    public override AHint GetHintFor(HintManager manager, int parameterIndex) {
        return mapHint;
    }

    public void LoadMapList(Grf grf) {
        var mapList = new LinkedList<string>();
        
        //build map list
        foreach(string key in grf.files.Keys){
            if(key.EndsWith(".rsw", StringComparison.OrdinalIgnoreCase)) {
                mapList.AddLast(key);
            }
        }

        maps = new string[mapList.Count];
        int i = 0;
        foreach(string map in mapList) {
            maps[i++] = GetMapName(map);
        }
        Array.Sort(maps);
    }

    public Message Command(string name) {
        if(Array.IndexOf(maps, name) != -1) {
            ChangeMap(name);
            return null;
        } else {
            return new Message(EMessageType.Error, "Map not found");
        }
    }

    public string[] GetMapList() {
        return maps;
    }

    public static string GetMapName(string path) {
        string mapName = path.Substring(path.LastIndexOf('/') + 1);
        return mapName.Replace(".rsw", "");
    }

    public void ChangeMap(string mapname) {
        Core.MapRenderer.Clear();
        currentMap = mapname;
        float start = Time.realtimeSinceStartup;
        Core.MapLoader.Load(mapname + ".rsw", Core.MapRenderer.OnComplete);
        float delta = Time.realtimeSinceStartup - start;
        Debug.Log("Total load time: " + delta);
    }
}
