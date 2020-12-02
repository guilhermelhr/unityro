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

    internal static int getWeaponAction(object weapon, object job, object sex) {
        throw new NotImplementedException();
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

    public static void init() {
        foreach(object[] args in LoadTable("data/mp3nametable.txt", 2)) {
            if(!mapTable.ContainsKey(args[1])) {
                mapTable.Add(args[1], new MapTableStruct());
            }

            MapTableStruct mts = (MapTableStruct) mapTable[args[1]];
            mts.mp3 = Convert.ToString(args[2]);
        }

        foreach(object[] args in LoadTable("data/mapnametable.txt", 2)) {
            if(!mapTable.ContainsKey(args[1])) {
                mapTable.Add(args[1], new MapTableStruct());
            }

            MapTableStruct mts = (MapTableStruct) mapTable[args[1]];
            mts.name = Convert.ToString(args[2]);
        }

        foreach(object[] args in LoadTable("data/msgstringtable.txt", 1)) {
            msgStringTable[args[0]] = args[1];
        }

        foreach(object[] args in LoadTable("data/resnametable.txt", 2)) {
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

        for(int i = 0; i < elements.Length; i++) {
            if(i % size == 0) {
                if(i != 0) {
                    yield return args;
                }
                args[i % size] = i;
            }

            args[(i % size) + 1] = elements[i].Trim();
        }

        //DIFF ending callback
    }
}
