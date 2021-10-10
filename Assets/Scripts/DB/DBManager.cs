﻿using Newtonsoft.Json.Linq;
using ROIO;
using System.Collections.Generic;
using System.Text;

public class DBManager {

    public const string INTERFACE_PATH = "data/texture/À¯ÀúÀÎÅÍÆäÀÌ½º/";

    private static Dictionary<int, string> monsterPathTable = MonsterTable.Table;
    private static string[] SexTable = new string[] { "¿©", "³²" };
    private static JObject WeaponActions;
    private static JObject WeaponJobTable;
    private static JObject ClassTable;

    public static void Init() {
        new LuaInterface();
        WeaponActions = FileManager.Load("Resources/DataTables/WeaponActions.json") as JObject;
        WeaponJobTable = FileManager.Load("Resources/DataTables/WeaponJobTable.json") as JObject;
        ClassTable = FileManager.Load("Resources/DataTables/ClassTable.json") as JObject;
    }

    public static Item GetItemInfo(int gID) {
        ItemDB.TryGetValue(gID, out Item item);

        return item;
    }

    public static string GetItemPath(int gID, bool isIdentified) {
        var it = GetItemInfo(gID);
        var resName = isIdentified ? it.identifiedResourceName : it.unidentifiedResourceName;
        return $"data/sprite/¾ÆÀÌÅÛ/{resName}";
    }

    public static string GetItemResPath(int itemID, bool isIdentified) {
        var item = GetItemInfo(itemID);
        return GetItemResPath(item, isIdentified);
    }

    public static string GetItemResPath(Item item, bool isIdentified) {
        return $"{INTERFACE_PATH}item/{(isIdentified ? item.identifiedResourceName : item.unidentifiedResourceName)}.bmp";
    }

    public static string GetItemCollectionPath(Item item, bool isIdentified) {
        return $"{INTERFACE_PATH}collection/{(isIdentified ? item.identifiedResourceName : item.unidentifiedResourceName)}.bmp";
    }

    public static int GetWeaponAction(Job job, int sex, int weapon) {
        var jobValue = $"{(ushort) job}";
        var weaponViewId = $"{GetItemViewID(weapon)}";

        try {
            var jobActionValue = WeaponActions[jobValue];
            if (jobActionValue.Type == JTokenType.Array) {
                return jobActionValue[$"{sex}"][weaponViewId].ToObject<int>();
            } else {
                return jobActionValue[weaponViewId].ToObject<int>();
            }
        } catch {
            return 1;
        }
    }

    public static int GetItemViewID(int itemId) {
        // Already a ViewID
        if (itemId < (int) WeaponType.MAX) {
            return itemId;
        }
        ItemDB.TryGetValue(itemId, out Item item);

        return item?.ClassNum ?? 0;
    }

    public static string GetBodyPath(int job, int sex) {

        var isPC = ClassTable.TryGetValue(job.ToString(), out var jobPath);
        var isMonster = MonsterPath.TryGetValue(job, out string monsterPath);
        var sexPath = SexTable[sex];


        // PC
        if (job < 45) {
            return $"data/sprite/ÀÎ°£Á·/¸öÅë/{sexPath}/{jobPath}_{sexPath}";
        }

        // TODO: Warp STR file
        //if (id == 45) {
        //    return null;
        //}

        // Not visible sprite
        if (job == 111 || job == 139) {
            return null;
        }

        // NPC
        if (job < 1000) {
            return "data/sprite/npc/" + (monsterPath ?? MonsterPath[46]).ToLower();
        }

        // Monsters
        if (job < 4000) {
            return "data/sprite/¸ó½ºÅÍ/" + (monsterPath ?? MonsterPath[1001]).ToLower();
        }

        // PC
        if (job < 6000) {
            return $"data/sprite/ÀÎ°£Á·/¸öÅë/{sexPath}/{jobPath}_{sexPath}";
        }


        // Homunculus
        return "data/sprite/homun/" + (monsterPath ?? MonsterPath[1001]).ToLower();

        // TODO: add support for mercenary
    }

    public static string GetHeadPath(int headId, int sex) {
        return $"data/sprite/ÀÎ°£Á·/¸Ó¸®Åë/{SexTable[sex]}/{HairIndexPath[sex][headId]}_{SexTable[sex]}";
    }

    public static string GetShieldPath(int id, int job, int sex) {
        if (id == 0) {
            return null;
        }

        var baseClass = GetBaseClass(job);
        var ViewID = GetItemViewID(id);

        ItemTable.Shields.TryGetValue(ViewID, out var shield);
        return $"data/sprite/¹æÆÐ/{baseClass}/{baseClass}_{SexTable[sex]}_{shield ?? ItemTable.Shields[1]}";
    }

    public static string GetWeaponPath(int id, int job, int sex) {
        if (id == 0) {
            return null;
        }

        var baseClass = GetBaseClass(job);
        var ViewID = GetItemViewID(id);

        ItemTable.Weapons.TryGetValue((WeaponType) ViewID, out var weapon);
        return $"data/sprite/ÀÎ°£Á·/{baseClass}/{baseClass}_{SexTable[sex]}{weapon.KoreanTo1252() ?? $"_{ViewID}"}";
    }

    public static string GetHatPath(int id, int sex) {
        if (id == 0) {
            return null;
        }

        var hatPath = ItemTable.GetAccessoryResName(id);

        return $"data/sprite/¾Ç¼¼»ç¸®/{SexTable[sex]}/{SexTable[sex]}{hatPath}";
    }

    public static Dictionary<int, string> MonsterPath => monsterPathTable;

    public static Dictionary<int, Item> ItemDB => ItemTable.Items;

    public static int[][] HairIndexPath => HairIndexTable.table;

    private static string GetBaseClass(int job) {
        return (WeaponJobTable[$"{job}"] ?? WeaponJobTable["0"]).ToObject<string>();
    }
}
