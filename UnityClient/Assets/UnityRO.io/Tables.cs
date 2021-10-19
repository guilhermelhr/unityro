using ROIO.Loaders;
using ROIO.Models;
using System;
using System.Collections;

namespace ROIO
{
    public static class Tables
    {

        public static Hashtable MapTable = new Hashtable();
        public static Hashtable ResNameTable = new Hashtable();
        public static Hashtable MsgStringTable = new Hashtable();

        public static void Init()
        {
            InitMsgStringTable();
            InitMp3NameTable();
            InitMapTable();
            InitResNameTable();

            //TODO load these tables
            //LoadTable("data/num2cardillustnametable.txt", 2);
            //LoadTable("data/cardprefixnametable.txt", 2);
            //LoadTable("data/fogparametertable.txt", 5);
        }

        private static void InitResNameTable()
        {
            foreach (object[] args in TableLoader.LoadTable("data/resnametable.txt", 2))
            {
                ResNameTable[args[1]] = args[2];
            }
        }

        private static void InitMapTable()
        {
            foreach (object[] args in TableLoader.LoadTable("data/mapnametable.txt", 2))
            {
                if (!MapTable.ContainsKey(args[1]))
                {
                    MapTable.Add(args[1], new MapTableStruct());
                }

                MapTableStruct mts = (MapTableStruct)MapTable[args[1]];
                mts.name = Convert.ToString(args[2]);
            }
        }

        private static void InitMp3NameTable()
        {
            foreach (object[] args in TableLoader.LoadTable("data/mp3nametable.txt", 2))
            {
                if (!MapTable.ContainsKey(args[1]))
                {
                    MapTable.Add(args[1], new MapTableStruct());
                }

                MapTableStruct mts = (MapTableStruct)MapTable[args[1]];
                mts.mp3 = Convert.ToString(args[2]);
            }
        }

        private static void InitMsgStringTable()
        {
            foreach (object[] args in TableLoader.LoadTable("data/msgstringtable.txt", 1))
            {
                MsgStringTable[args[0]] = args[1];
            }
        }
    }
}
