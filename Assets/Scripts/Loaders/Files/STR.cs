
using System.Collections.Generic;
using UnityEngine;

public class STR
{
    public static string Header = "STRM";
    public ulong version;
    public ulong fps;
    public ulong maxKey;
    public Layer[] layers;

    public class Layer
    {
        public string[] textures;
        public Animation[] animations;
    }

    public class Animation
    {
        public int frame;
        public ulong type;
        public Vector2 position;
        public float[] uv;
        public float[] xy;
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
