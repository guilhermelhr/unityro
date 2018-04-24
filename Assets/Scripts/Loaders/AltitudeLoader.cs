using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// Loader for Gravity .gat file (Ground Altitude)
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class AltitudeLoader {
    /// <summary>
    /// Cell known type
    /// </summary>
    public enum TYPE : byte {
        NONE = 1 << 0,
        WALKABLE = 1 << 1,
        WATER = 1 << 2,
        SNIPABLE = 1 << 3
    }

    public struct Cell {
        public Vector4 heights;
        public byte type;
    }

    /// <summary>
    /// Taken from *athena at src/map/map.c
    /// </summary>
    public static byte[] TYPE_TABLE = {
        (byte) TYPE.WALKABLE | (byte) TYPE.SNIPABLE,                     // walkable ground
		(byte) TYPE.NONE,                                                // non-walkable ground
		(byte) TYPE.WALKABLE | (byte) TYPE.SNIPABLE,                     // ???
		(byte) TYPE.WALKABLE | (byte) TYPE.SNIPABLE | (byte) TYPE.WATER, // walkable water
		(byte) TYPE.WALKABLE | (byte) TYPE.SNIPABLE,                     // ???
		(byte) TYPE.SNIPABLE,                                            // gat (snipable)
		(byte) TYPE.WALKABLE | (byte) TYPE.SNIPABLE                      // ???
    };

    /// <summary>
    /// Load a GAT file
    /// </summary>
    /// <param name="data">GAT file data</param>
    public static GAT Load(BinaryReader data) {
        string header = data.ReadBinaryString(4);
        
        //check for valid gat file
        if(!string.Equals(header, GAT.Header)) {
            throw new Exception("AltitudeLoader.Load: Header (" + header + ") is not \"GRAT\"");
        }

        //load parameters
        string version = Convert.ToString(data.ReadUByte());
        string subversion = Convert.ToString(data.ReadUByte());
        version += "." + subversion;
        uint width = data.ReadULong();
        uint height = data.ReadULong();
        Cell[] cells = new Cell[width * height];

        //load the cells
        for(int i = 0; i < width * height; i++) {
            Vector4 heights = cells[i].heights = new Vector4();
            heights[0] = data.ReadFloat() * 0.2f;         // height 1
            heights[1] = data.ReadFloat() * 0.2f;         // height 2
            heights[2] = data.ReadFloat() * 0.2f;         // height 3
            heights[3] = data.ReadFloat() * 0.2f;         // height 4
            cells[i].type = TYPE_TABLE[data.ReadULong()];    // type
        }

        //exports
        return new GAT(width, height, cells, version);
    }
}
