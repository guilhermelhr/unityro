
using System.Collections.Generic;
using UnityEngine;

public class STR
{
    public static string Header = "STRM";
    public ulong version;
    public long fps;
    public long maxKey; //frameCount
    public Layer[] layers;
    public Texture2D Atlas;
    public Rect[] AtlasRects;
    public string name;

    public class Layer
    {
        public Texture2D[] textures;
        public Animation[] animations;
        public List<int> texturesIds;
    }

    public class Animation
    {
        public int frame;
        public ulong type;
        public Vector2 position;
        public Vector2[] uv;
        public Vector2[] xy;
        public float animFrame;
        public ulong animType;
        public float delay;
        public float angle;
        public Color color;
        public ulong srcAlpha;
        public ulong destAlpha;
        public ulong mtPreset;
    }
}
