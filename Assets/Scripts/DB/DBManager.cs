using System;
using System.Collections;
using System.Collections.Generic;
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
    private static Dictionary<Job, string> bodyPathTable = BodyPathTable.init();
    private static Dictionary<int, string> SexTable = new Dictionary<int, string>() {
        { 0, "\xbf\xa9" },
        { 1, "\xb3\xb2" },
    };

    internal static int getWeaponAction(object weapon, Job job, object sex) {
        throw new NotImplementedException();
    }

    internal static string GetBodyPath(Job job, int sex) {
        var id = (int)job;

        // PC
        if (id < 45) {
            var jobPath = BodyPath[job] == null ? BodyPath[0] : BodyPath[job];
            return $"data/sprite/\xc0\xce\xb0\xa3\xc1\xb7/\xb8\xf6\xc5\xeb/{SexTable[sex]}/{jobPath}_{SexTable[sex]}";
        }

        // TODO: Warp STR file
        if (id == 45) {
            return null;
        }

        // Not visible sprite
        if (id == 111 || id == 139) {
            return null;
        }

        // NPC
        //if (id < 1000) {
        //    return 'data/sprite/npc/' + (MonsterTable[id] || MonsterTable[46]).toLowerCase();
        //}

        // Monsters
        //if (id < 4000) {
        //    return 'data/sprite/\xb8\xf3\xbd\xba\xc5\xcd/' + (MonsterTable[id] || MonsterTable[1001]).toLowerCase();
        //}

        // PC
        if (id < 6000) {
            var jobPath = BodyPath[job] == null ? BodyPath[0] : BodyPath[job];
            return $"data/sprite/\xc0\xce\xb0\xa3\xc1\xb7/\xb8\xf6\xc5\xeb/{SexTable[sex]}/{jobPath}_{SexTable[sex]}";
        }

        // Homunculus
        //return 'data/sprite/homun/' + (MonsterTable[id] || MonsterTable[1002]).toLowerCase();

        // TODO: add support for mercenary
        return null;
    }

    public static Hashtable MapTable {
        get {
            return mapTable;
        }
    }

    public static Hashtable MsgStringTable {
        get {
            return msgStringTable;
        }
    }

    public static Hashtable MapAlias {
        get {
            return mapAlias;
        }
    }

    public static Dictionary<Job, String> BodyPath => bodyPathTable;

    public static void init() {
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

        foreach (object[] args in LoadTable("data/msgstringtable.txt", 1)) {
            msgStringTable[args[0]] = args[1];
        }

        foreach (object[] args in LoadTable("data/resnametable.txt", 2)) {
            mapAlias[args[1]] = args[2];
        }


        //TODO load these tables
        //LoadTable("data/num2cardillustnametable.txt", 2);
        //LoadTable("data/cardprefixnametable.txt", 2);
        //LoadTable("data/fogparametertable.txt", 5);
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
