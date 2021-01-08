using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class DBManager {

    public const string INTERFACE_PATH = "data/texture/\xc0\xaf\xc0\xfa\xc0\xce\xc5\xcd\xc6\xe4\xc0\xcc\xbd\xba/";

    private static Regex rcomments = new Regex(@"\n(\/\/[^\n]+)", RegexOptions.Multiline);
    private struct MapTableStruct {
        public string name;
        public string mp3;
        public object fog;
    }
    private struct ItemTableStruct {
        public string illustResourcesName;
        public string prefixNameTable;
    }
    private static Hashtable mapTable = new Hashtable();
    private static Hashtable msgStringTable = new Hashtable();
    private static Hashtable mapAlias = new Hashtable();
    private static Dictionary<int, string> bodyPathTable = BodyPathTable.BodyPath;
    private static Dictionary<int, string> monsterPathTable = MonsterTable.Table;
    private static string[] SexTable = new string[] { "\xbf\xa9", "\xb3\xb2" };

    public static Item GetItemInfo(int gID) {
        ItemDB.TryGetValue(gID, out Item item);

        return item;
    }

    public static string GetItemPath(int gID, bool isIdentified) {
        var it = GetItemInfo(gID);
        var resName = isIdentified ? it.identifiedResourceName : it.unidentifiedResourceName;
        return $"data/sprite/\xbe\xc6\xc0\xcc\xc5\xdb/{resName}";
    }

    public static string GetItemResPath(int itemID, bool isIdentified) {
        var item = GetItemInfo(itemID);
        return GetItemResPath(item, isIdentified);
    }

    public static string GetItemResPath(Item item, bool isIdentified) {
        return $"{INTERFACE_PATH}item/{(isIdentified ? item.identifiedResourceName : item.unidentifiedResourceName)}.bmp";
    }

    // TODO implement this
    public static int GetWeaponAction(Job job, int sex, int weapon) {
        var baseJob = JobHelper.GetBaseClass((ushort)job, sex);

        ItemDB.TryGetValue(weapon, out Item item);
        return 1;
    }

    public static int GetItemViewID(int itemId) {
        ItemDB.TryGetValue(itemId, out Item item);

        return item?.ClassNum ?? -1;
    }

    public static string GetBodyPath(Job job, int sex) {
        var id = (int)job;

        // PC
        if (id < 45) {
            var path = BodyPath[id] == null ? BodyPath[0] : BodyPath[id];
            var jobPath = Encoding.GetEncoding(1252).GetString(Encoding.GetEncoding(949).GetBytes(path));

            return $"data/sprite/\xc0\xce\xb0\xa3\xc1\xb7/\xb8\xf6\xc5\xeb/{SexTable[sex]}/{jobPath}_{SexTable[sex]}";
        }

        // TODO: Warp STR file
        //if (id == 45) {
        //    return null;
        //}

        // Not visible sprite
        if (id == 111 || id == 139) {
            return null;
        }

        // NPC
        if (id < 1000) {
            return "data/sprite/npc/" + (MonsterPath[id] ?? MonsterPath[46]).ToLower();
        }

        // Monsters
        if (id < 4000) {
            return "data/sprite/\xb8\xf3\xbd\xba\xc5\xcd/" + (MonsterPath[id] ?? MonsterPath[1001]).ToLower();
        }

        // PC
        if (id < 6000) {
            var path = BodyPath[id] == null ? BodyPath[0] : BodyPath[id];
            var jobPath = Encoding.GetEncoding(1252).GetString(Encoding.GetEncoding(949).GetBytes(path));
            return $"data/sprite/\xc0\xce\xb0\xa3\xc1\xb7/\xb8\xf6\xc5\xeb/{SexTable[sex]}/{jobPath}_{SexTable[sex]}";
        }

        // Homunculus
        return "data/sprite/homun/" + (MonsterPath[id] ?? MonsterPath[1002]).ToLower();

        // TODO: add support for mercenary
    }

    public static string GetHeadPath(int headId, int sex) {
        return $"data/sprite/\xc0\xce\xb0\xa3\xc1\xb7/\xb8\xd3\xb8\xae\xc5\xeb/{SexTable[sex]}/{HairIndexPath[sex][headId]}_{SexTable[sex]}";
    }

    public static string GetShieldPath(int id, int job, int sex) {
        if (id == 0) {
            return null;
        }

        // Dual weapon (based on range id)
        if (id > 500 && (id < 2100 || id > 2200)) {
            return GetWeaponPath(id, job, sex);
        }

        var baseJob = (int)JobHelper.GetBaseClass((ushort)job, sex);
        var path = BodyPath[baseJob] == null ? BodyPath[0] : BodyPath[baseJob];
        var baseClass = path.KoreanTo1252();

        // ItemID to View Id
        var ViewID = id;
        if ((ItemDB.ContainsKey(id)) && (ItemDB[id].ClassNum >= 0)) {
            ViewID = ItemDB[id].ClassNum;
        }

        ItemTable.Shields.TryGetValue(ViewID, out var shield);
        return $"data/sprite/\xb9\xe6\xc6\xd0/{baseClass}/{baseClass}_{SexTable[sex]}_{shield ?? ItemTable.Shields[1]}";
    }

    public static string GetWeaponPath(int id, int job, int sex) {
        if (id == 0) {
            return null;
        }

        var baseJob = (int)JobHelper.GetBaseClass((ushort)job, sex);
        var path = BodyPath[baseJob] == null ? BodyPath[0] : BodyPath[baseJob];
        var baseClass = path.KoreanTo1252();

        // ItemID to View Id
        var ViewID = id;
        if ((ItemDB.ContainsKey(id)) && (ItemDB[id].ClassNum >= 0)) {
            ViewID = ItemDB[id].ClassNum;
        }

        ItemTable.Weapons.TryGetValue((WeaponType)ViewID, out var weapon);

        return $"data/sprite/\xc0\xce\xb0\xa3\xc1\xb7/{baseClass}/{baseClass}_{SexTable[sex]}{weapon.KoreanTo1252() ?? $"_{ViewID}"}";
    }

    public static Hashtable MapTable => mapTable;

    public static Hashtable MsgStringTable => msgStringTable;

    public static Hashtable MapAlias => mapAlias;

    public static Dictionary<int, string> BodyPath => bodyPathTable;

    public static Dictionary<int, string> MonsterPath => monsterPathTable;

    public static Dictionary<int, Item> ItemDB => ItemTable.Items;

    public static int[][] HairIndexPath => HairIndexTable.table;

    public static void init() {
        new LuaInterface();

        try {
            foreach (object[] args in LoadTable("data/msgstringtable.txt", 1)) {
                msgStringTable[args[0]] = args[1];
            }

            foreach (object[] args in LoadTable("data/mp3nametable.txt", 2)) {
                if (!mapTable.ContainsKey(args[1])) {
                    mapTable.Add(args[1], new MapTableStruct());
                }

                MapTableStruct mts = (MapTableStruct)mapTable[args[1]];
                mts.mp3 = Convert.ToString(args[2]);
            }

            foreach (object[] args in LoadTable("data/mapnametable.txt", 2)) {
                if (!mapTable.ContainsKey(args[1])) {
                    mapTable.Add(args[1], new MapTableStruct());
                }

                MapTableStruct mts = (MapTableStruct)mapTable[args[1]];
                mts.name = Convert.ToString(args[2]);
            }

            foreach (object[] args in LoadTable("data/resnametable.txt", 2)) {
                mapAlias[args[1]] = args[2];
            }



            //TODO load these tables
            //LoadTable("data/num2cardillustnametable.txt", 2);
            //LoadTable("data/cardprefixnametable.txt", 2);
            //LoadTable("data/fogparametertable.txt", 5);
        } catch {

        }

    }

    /// <summary>
    /// load txt table
    /// </summary>
    /// <param name="filename">file to load</param>
    /// <param name="size">size of each group</param>
    public static IEnumerable<object> LoadTable(string filename, int size) {
        Debug.Log("Loading table " + filename);

        string data = FileManager.Load(filename) as string;
        //remove comments
        string content = rcomments.Replace(("\n" + data), "");
        string[] elements = content.Split('#');
        object[] args = new string[size + 1];

        for (int i = 0; i < elements.Length; i++) {
            if (i % size == 0) {
                if (i != 0) {
                    yield return args;
                }
                args[i % size] = i;
            }

            args[(i % size) + 1] = elements[i].Trim();
        }

        //DIFF ending callback
    }
}
