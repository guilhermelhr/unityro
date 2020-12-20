using System;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;

public class Item {
    public int id;
    public string unidentifiedDisplayName = "";
    public string unidentifiedResourceName = "";
    public string unidentifiedDescriptionName = "";
    public string identifiedDisplayName = "";
    public string identifiedResourceName = "";
    public string identifiedDescriptionName = "";
    public int slotCount = 0;
    public int ClassNum = 0;
    public bool costume = false;
}

public enum EquipmentLocation : int {
    HEAD_BOTTOM = 1 << 0,
    WEAPON = 1 << 1,
    GARMENT = 1 << 2,
    ACCESSORY1 = 1 << 3,
    ARMOR = 1 << 4,
    SHIELD = 1 << 5,
    SHOES = 1 << 6,
    ACCESSORY2 = 1 << 7,
    HEAD_TOP = 1 << 8,
    HEAD_MID = 1 << 9,
    AMMO = 1 << 15
}

public enum ItemType : int {
    HEALING = 0,
    USABLE = 2,
    ETC = 3,
    WEAPON = 4,
    EQUIP = 5,
    CARD = 6,
    PETEGG = 7,
    PETEQUIP = 8,
    AMMO = 10,
    USABLE_SKILL = 11,
    USABLE_UNK = 18
}

public enum WeaponType : int {
    NONE = 0,
    SHORTSWORD = 1,
    SWORD = 2,
    TWOHANDSWORD = 3,
    SPEAR = 4,
    TWOHANDSPEAR = 5,
    AXE = 6,
    TWOHANDAXE = 7,
    MACE = 8,
    TWOHANDMACE = 9,
    ROD = 10,
    BOW = 11,
    KNUKLE = 12,
    INSTRUMENT = 13,
    WHIP = 14,
    BOOK = 15,
    KATAR = 16,
    GUN_HANDGUN = 17,
    GUN_RIFLE = 18,
    GUN_GATLING = 19,
    GUN_SHOTGUN = 20,
    GUN_GRANADE = 21,
    SYURIKEN = 22,
    TWOHANDROD = 23,
    LAST = 24,
    SHORTSWORD_SHORTSWORD = 25,
    SWORD_SWORD = 26,
    AXE_AXE = 27,
    SHORTSWORD_SWORD = 28,
    SHORTSWORD_AXE = 29,
    SWORD_AXE = 30,
    MAX = 31
}

public class ItemTable {

    public static Dictionary<int, Item> Items = new Dictionary<int, Item>();

    public static Dictionary<int, string> Shields = new Dictionary<int, string>() {
        { 1, "\xb0\xa1\xb5\xe5" },
        { 2, "\xb9\xf6\xc5\xac\xb7\xaf" },
        { 3, "\xbd\xaf\xb5\xe5" },
        { 4, "\xb9\xcc\xb7\xaf\xbd\xaf\xb5\xe5" }
    };

    public static Dictionary<WeaponType, string> Weapons = new Dictionary<WeaponType, string> {
        { WeaponType.NONE                  , "" },
        { WeaponType.SHORTSWORD            , "\xb4\xdc\xb0\xcb"},
        { WeaponType.SWORD                 , "\xb0\xcb"},
        { WeaponType.TWOHANDSWORD          , "\xb0\xcb"},
        { WeaponType.SPEAR                 , "\xc3\xa2"},
        { WeaponType.TWOHANDSPEAR          , "\xc3\xa2"},
        { WeaponType.AXE                   , "\xb5\xb5\xb3\xa2"},
        { WeaponType.TWOHANDAXE            , "\xb5\xb5\xb3\xa2"},
        { WeaponType.MACE                  , "\xc5\xac\xb7\xb4"},
        { WeaponType.TWOHANDMACE           , "\xc5\xac\xb7\xb4"},
        { WeaponType.ROD                   , "\xb7\xd4\xb5\xe5"},
        { WeaponType.BOW                   , "\xc8\xb0"},
        { WeaponType.KNUKLE                , "\xb3\xca\xc5\xac"},
        { WeaponType.INSTRUMENT            , "\xbe\xc7\xb1\xe2"},
        { WeaponType.WHIP                  , "\xc3\xa4\xc2\xef"},
        { WeaponType.BOOK                  , "\xc3\xa5"},
        { WeaponType.KATAR                 , "\xc4\xab\xc5\xb8\xb8\xa3\x5f\xc4\xab\xc5\xb8\xb8\xa3"},
        { WeaponType.GUN_HANDGUN           , "\xb1\xc7\xc3\xd1"},
        { WeaponType.GUN_RIFLE             , "\xb1\xe2\xb0\xfc\xc3\xd1"},
        { WeaponType.GUN_GATLING           , "\xb1\xe2\xb0\xfc\xc3\xd1"},
        { WeaponType.GUN_SHOTGUN           , "\xb1\xe2\xb0\xfc\xc3\xd1"},
        { WeaponType.GUN_GRANADE           , "\xb1\xe2\xb0\xfc\xc3\xd1"},
        { WeaponType.SYURIKEN              , "\xbc\xf6\xb8\xae\xb0\xcb"},
        { WeaponType.TWOHANDROD            , "\xb7\xd4\xb5\xe5"},
        { WeaponType.SHORTSWORD_SHORTSWORD , "\xb4\xdc\xb0\xcb\x5f\xb4\xdc\xb0\xcb"},
        { WeaponType.SWORD_SWORD           , "\xb0\xcb\x5f\xb0\xcb"},
        { WeaponType.AXE_AXE               , "\xb5\xb5\xb3\xa2\x5f\xb5\xb5\xb3\xa2"},
        { WeaponType.SHORTSWORD_SWORD      , "\xb4\xdc\xb0\xcb\x5f\xb0\xcb"},
        { WeaponType.SHORTSWORD_AXE        , "\xb4\xdc\xb0\xcb\x5f\xb5\xb5\xb3\xa2"},
        { WeaponType.SWORD_AXE             , "\xb0\xcb\x5f\xb5\xb5\xb3\xa2"},
    };

    public static Dictionary<WeaponType, string> WeaponSound = new Dictionary<WeaponType, string>() {
        { WeaponType.NONE                  , "_hit_mace.wav" },
        { WeaponType.SHORTSWORD            , "_hit_sword.wav" },
        { WeaponType.SWORD                 , "_hit_sword.wav" },
        { WeaponType.TWOHANDSWORD          , "_hit_sword.wav" },
        { WeaponType.SPEAR                 , "_hit_spear.wav" },
        { WeaponType.TWOHANDSPEAR          , "_hit_spear.wav" },
        { WeaponType.AXE                   , "_hit_axe.wav" },
        { WeaponType.TWOHANDAXE            , "_hit_axe.wav" },
        { WeaponType.MACE                  , "_hit_mace.wav" },
        { WeaponType.TWOHANDMACE           , "_hit_mace.wav" },
        { WeaponType.ROD                   , "_hit_rod.wav" },
        { WeaponType.BOW                   , "_hit_arrow.wav" },
        { WeaponType.KNUKLE                , "_hit_mace.wav" },
        { WeaponType.INSTRUMENT            , "_hit_mace.wav" },
        { WeaponType.WHIP                  , "_hit_mace.wav" },
        { WeaponType.BOOK                  , "_hit_mace.wav" },
        { WeaponType.KATAR                 , "_hit_mace.wav" },
        { WeaponType.GUN_HANDGUN           , "_hit_±ÇÃÑ.wav" },
        { WeaponType.GUN_RIFLE             , "_hit_¶óÀÌÇÃ.wav" },
        { WeaponType.GUN_GATLING           , "_hit_mace.wav" },
        { WeaponType.GUN_SHOTGUN           , "_hit_mace.wav" },
        { WeaponType.GUN_GRANADE           , "_hit_mace.wav" },
        { WeaponType.SYURIKEN              , "_hit_mace.wav" },
        { WeaponType.TWOHANDROD            , "_hit_rod.wav" },
        { WeaponType.SHORTSWORD_SHORTSWORD , "_hit_mace.wav" },
        { WeaponType.SWORD_SWORD           , "_hit_mace.wav" },
        { WeaponType.AXE_AXE               , "_hit_mace.wav" },
        { WeaponType.SHORTSWORD_SWORD      , "_hit_mace.wav" },
        { WeaponType.SHORTSWORD_AXE        , "_hit_mace.wav" },
        { WeaponType.SWORD_AXE             , "_hit_mace.wav" },
    };

    public static void LoadItemDb() {
        Script script = new Script();
        script.Options.ScriptLoader = new FileSystemScriptLoader();
        script.DoFile(Core.Configs["itemInfo"] as string);
        Table table = (Table)script.Globals["tbl"];

        foreach (var key in table.Keys) {
            try {
                var it = table[key] as Table;

                List<string> unidentifiedDescriptionName = new List<string>();
                foreach (var desc in ((Table)it["unidentifiedDescriptionName"]).Values) {
                    unidentifiedDescriptionName.Add(desc.ToString());
                }

                List<string> identifiedDescriptionName = new List<string>();
                foreach (var desc in ((Table)it["identifiedDescriptionName"]).Values) {
                    identifiedDescriptionName.Add(desc.ToString());
                }

                var item = new Item() {
                    id = int.Parse(key.ToString()),
                    unidentifiedDisplayName = it["unidentifiedDisplayName"].ToString(),
                    unidentifiedResourceName = it["unidentifiedResourceName"].ToString(),
                    unidentifiedDescriptionName = string.Join("\n", unidentifiedDescriptionName),
                    identifiedDisplayName = it["identifiedDisplayName"].ToString(),
                    identifiedResourceName = it["identifiedResourceName"].ToString(),
                    identifiedDescriptionName = string.Join("\n", identifiedDescriptionName),
                    slotCount = int.Parse(it["slotCount"].ToString()),
                    ClassNum = int.Parse(it["ClassNum"].ToString()),
                    costume = bool.Parse(it["costume"].ToString())
                };

                Items.Add(item.id, item);
            } catch (Exception e) {
                Debug.LogError($"Could not load item {key} - {e}");
            }
        }
    }
}