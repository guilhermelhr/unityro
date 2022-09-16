using ROIO.Loaders;
using ROIO.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ROIO {
    public static class Tables {

        public static Dictionary<string, MapTableStruct> MapTable = new Dictionary<string, MapTableStruct>();
        public static Hashtable ResNameTable = new Hashtable();
        public static Hashtable MsgStringTable = new Hashtable();

        public static void Init() {
            InitMsgStringTable();
            InitMp3NameTable();
            InitMapTable();
            InitResNameTable();

            //TODO load these tables
            //LoadTable("data/num2cardillustnametable.txt", 2);
            //LoadTable("data/cardprefixnametable.txt", 2);
            //LoadTable("data/fogparametertable.txt", 5);
        }

        private async static void InitResNameTable() {
            var data = await Addressables.LoadAssetAsync<TextAsset>($"txt/data/resnametable.txt.txt").Task;
            foreach (object[] args in TableLoader.LoadTable(data.text, 2)) {
                ResNameTable[args[1]] = args[2];
            }
        }

        private async static void InitMapTable() {
            var data = await Addressables.LoadAssetAsync<TextAsset>($"txt/data/mapnametable.txt.txt").Task;
            foreach (object[] args in TableLoader.LoadTable(data.text, 2)) {
                var key = Convert.ToString(args[1]);
                if (!MapTable.ContainsKey(key)) {
                    MapTable.Add(key, new MapTableStruct());
                }

                MapTableStruct mts = MapTable[key];
                mts.name = Convert.ToString(args[2]);
                MapTable[key] = mts;
            }
        }

        private async static void InitMp3NameTable() {
            var data = await Addressables.LoadAssetAsync<TextAsset>($"txt/data/mp3nametable.txt.txt").Task;
            foreach (object[] args in TableLoader.LoadTable(data.text, 2)) {
                var key = Convert.ToString(args[1]);
                if (!MapTable.ContainsKey(key)) {
                    MapTable.Add(key, new MapTableStruct());
                }

                MapTableStruct mts = MapTable[key];
                mts.mp3 = System.IO.Path.GetFileName(args[2].ToString());
                MapTable[key] = mts;
            }
        }

        private async static void InitMsgStringTable() {
            var data = await Addressables.LoadAssetAsync<TextAsset>($"txt/data/msgstringtable.txt.txt").Task;
            foreach (object[] args in TableLoader.LoadTable(data.text, 1)) {
                MsgStringTable[args[0]] = args[1];
            }
        }
    }
}
