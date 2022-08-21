using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ROIO.Models.FileTypes {

    public class STR : ScriptableObject {
        private Texture2D _Atlas;
        private Rect[] _AtlasRects;

        public static string Header = "STRM";
        public ulong version;
        public long fps;
        public long maxKey; //frameCount
        public Layer[] layers;
        public Texture2D Atlas => GetOrGenerateAtlas();
        public Rect[] AtlasRects => GetOrGenerateRects();
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

            var textures = layers.SelectMany(it => it.textures).Distinct().ToList();
            var baseName = Path.GetFileNameWithoutExtension(name);
            var atlasName = $"{baseName.Replace("\\", "_")}_atlas";

            var extraTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            textures.Add(extraTexture);

            var superTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            superTexture.name = atlasName;

            var rects = superTexture.PackTextures(textures.ToArray(), 2, 4096, false);
            PatchAtlasEdges(superTexture, rects);

            _Atlas = superTexture;
            _AtlasRects = rects;
            name = baseName;

            return _Atlas;
        }

        private Rect[] GetOrGenerateRects() {
            if (_AtlasRects != null) {
                return _AtlasRects;
            }

            GetOrGenerateAtlas();

            return _AtlasRects;
        }

        private void PatchAtlasEdges(Texture2D atlas, Rect[] rects) {
            foreach (var r in rects) {
                var xMin = Mathf.RoundToInt(Mathf.Lerp(0, atlas.width, r.x));
                var xMax = Mathf.RoundToInt(Mathf.Lerp(0, atlas.width, r.x + r.width));
                var yMin = Mathf.RoundToInt(Mathf.Lerp(0, atlas.height, r.y));
                var yMax = Mathf.RoundToInt(Mathf.Lerp(0, atlas.height, r.y + r.height));

                //bottom left
                if (xMin > 0 && yMin > 0)
                    atlas.SetPixel(xMin - 1, yMin - 1, atlas.GetPixel(xMin, yMin));

                //top left
                if (xMin > 0 && yMax < atlas.height)
                    atlas.SetPixel(xMin - 1, yMax, atlas.GetPixel(xMin, yMax - 1));

                //bottom right
                if (xMax < atlas.width && yMin > 0)
                    atlas.SetPixel(xMax, yMin - 1, atlas.GetPixel(xMax - 1, yMin));

                //top right
                if (xMax < atlas.width && yMax < atlas.height)
                    atlas.SetPixel(xMax, yMax, atlas.GetPixel(xMax - 1, yMax - 1));

                //left edge
                if (xMin > 0) {
                    var colors = atlas.GetPixels(xMin, yMin, 1, yMax - yMin);
                    atlas.SetPixels(xMin - 1, yMin, 1, yMax - yMin, colors);
                }

                //right edge
                if (xMax < atlas.width) {
                    var colors = atlas.GetPixels(xMax - 1, yMin, 1, yMax - yMin);
                    atlas.SetPixels(xMax, yMin, 1, yMax - yMin, colors);
                }

                //bottom edge
                if (yMin > 0) {
                    var colors = atlas.GetPixels(xMin, yMin, xMax - xMin, 1);
                    atlas.SetPixels(xMin, yMin - 1, xMax - xMin, 1, colors);
                }

                //top edge
                if (yMax < atlas.height) {
                    var colors = atlas.GetPixels(xMin, yMax - 1, xMax - xMin, 1);
                    atlas.SetPixels(xMin, yMax, xMax - xMin, 1, colors);
                }
            }
        }
    }
}