using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ROIO.Models.FileTypes {

    public class STR : ScriptableObject {
        public static string Header = "STRM";
        public ulong version;
        public long fps;
        public long maxKey; //frameCount
        public Layer[] layers;
        private Texture2D _Atlas;
        public Texture2D Atlas => GetOrGenerateAtlas();
        public Rect[] AtlasRects;
        public string name;

        [Serializable]
        public class Layer {
            public Texture2D[] textures;
            public Animation[] animations;
            public List<int> texturesIds;
        }

        [Serializable]
        public class Animation {
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

        private Texture2D GetOrGenerateAtlas() {
            if (_Atlas != null) {
                return _Atlas;
            }

            //var baseName = Path.GetFileNameWithoutExtension(name);
            //var atlasName = $"{baseName.Replace("\\", "_")}_atlas";

            //var extraTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            //textures.Add(extraTexture);

            //var superTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            //superTexture.name = atlasName;

            //var atlasRects = superTexture.PackTextures(textures.ToArray(), 2, 4096, false);

            //PatchAtlasEdges(superTexture, atlasRects);

            //str.AtlasRects = atlasRects;
            //str.name = baseName;

            return new Texture2D(2, 2, TextureFormat.RGBA32, false);
        }
    }
}