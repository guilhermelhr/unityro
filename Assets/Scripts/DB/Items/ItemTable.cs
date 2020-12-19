using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class Item {
    public int id;
    public string unidentifiedDisplayName = "Choco Gangjeong";
    public string unidentifiedResourceName = "�ݱ���";
    public string unidentifiedDescriptionName = "";
    public string identifiedDisplayName = "Choco Gangjeong";
    public string identifiedResourceName = "�ݱ���";
    public string identifiedDescriptionName = "";
    public int slotCount = 0;
    public int ClassNum = 0;
    public bool costume = false;
}

public class ItemTable {

    public static Dictionary<int, Item> Items = new Dictionary<int, Item>();

    public static void LoadDb() {
        using (var lua = new Lua()) {

            lua.DoFile(Core.Configs["itemInfo"] as string);
            LuaTable root = lua.GetTable("tbl");

            foreach (var key in root.Keys) {
                try {
                    var it = root[key] as LuaTable;

                    var item = new Item() {
                        id = (int)key,
                        unidentifiedDisplayName = it["unidentifiedDisplayName"].ToString(),
                        unidentifiedResourceName = it["unidentifiedResourceName"].ToString(),
                        unidentifiedDescriptionName = it["unidentifiedDescriptionName"].ToString(),
                        identifiedDisplayName = it["identifiedDisplayName"].ToString(),
                        identifiedResourceName = it["identifiedResourceName"].ToString(),
                        identifiedDescriptionName = it["identifiedDescriptionName"].ToString(),
                        slotCount = (int)it["slotCount"],
                        ClassNum = (int)it["ClassNum"],
                        costume = (bool)it["costume"]
                    };

                    Items.Add(item.id, item);
                } catch {
                    Debug.LogError($"Could not load item {key}");
                }
            }
        }
    }
}
