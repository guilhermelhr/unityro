using UnityEngine;

namespace ROIO.Models.FileTypes
{
    public class GND
    {
        public static string Header = "GRGN";

        public string version;
        public uint width;
        public uint height;
        public float zoom;
        public string[] textures;
        public int[] textureLookupList;
        public Tile[] tiles;
        public Surface[] surfaces;
        public Lightmap lightmap;

        public class Mesh
        {
            public uint width;
            public uint height;
            public string[] textures;

            public RoImage lightmap;
            public RoImage tileColor;
            public byte[] shadowMap;

            public float[] mesh;
            public int meshVertCount;

            public float[] waterMesh;
            public int waterVertCount;
        }

        public class Tile
        {
            public Vector4 textureStart;
            public Vector4 textureEnd;
            public ushort texture;
            public ushort light;
            public byte[] color;
        }

        public class Surface
        {
            public Vector4 height;
            public long tileUp;
            public long tileFront;
            public long tileRight;
        }

        public class Lightmap
        {
            public int perCell;
            public uint count;
            public byte[][] data;
        }

        public GND(string version)
        {
            this.version = version;
        }

    }
}