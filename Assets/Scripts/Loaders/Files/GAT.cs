
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// .gat file representation
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class GAT {
    public static string Header = "GRAT";

    public long width;
    public long height;
    public Cell[] cells;
    public string version;
    public List<PathNode> nodes;

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

    public GAT(uint width, uint height, Cell[] cells, string version) {
        this.width = width;
        this.height = height;
        this.cells = cells;
        this.version = version;

        GenerateNodes();
    }

    public override string ToString() {
        return "GAT v" + version + "(" + width + "x" + height + ")";
    }

    public PathNode GetPathNode(int x, int y) => nodes[x + (y * (int)width)];

    private void GenerateNodes() {
        nodes = new List<PathNode>();
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var isWalkable = cells[x + (y * width)].type == ((byte)TYPE.WALKABLE | (byte)TYPE.SNIPABLE);
                var newNode = new PathNode() {
                    x = x,
                    y = y,
                    walkable = isWalkable
                };

                nodes.Add(newNode);
            }
        }
    }
}
