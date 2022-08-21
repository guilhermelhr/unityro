using ROIO.Models.FileTypes;
using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ROIO.Loaders {
    public class EffectLoader {
        private static Dictionary<string, int> textureIdLookup = new Dictionary<string, int>();
        private static List<string> textureNames = new List<string>();
        private static List<Texture2D> textures = new List<Texture2D>();

        public async static Task<STR> Load(MemoryStreamReader data, string path) {
            var header = data.ReadBinaryString(4);

            if (!header.Equals(STR.Header)) {
                throw new Exception("EffectLoader.Load: Header (" + header + ") is not \"STRM\"");
            }

            var version = data.ReadUInt();
            if (version != 0x94) {
                throw new Exception("EffectLoader.Load: Unsupported STR version (v" + version + ")");
            }

            STR str = ScriptableObject.CreateInstance<STR>();
            str.version = version;
            str.fps = data.ReadUInt();
            str.maxKey = data.ReadUInt();
            var layerCount = data.ReadUInt();
            data.Seek(16, System.IO.SeekOrigin.Current);

            //read layers
            str.layers = new STR.Layer[layerCount];
            for (uint i = 0; i < layerCount; i++) {
                STR.Layer layer = str.layers[i] = new STR.Layer();

                //read texture filenames
                var textureCount = data.ReadInt();
                layer.textures = new Texture2D[textureCount];
                layer.texturesIds = new List<int>();
                for (int j = 0; j < textureCount; j++) {
                    var tex = data.ReadBinaryString(128);
                    var texture = await Addressables.LoadAssetAsync<Texture2D>(path + "/" + tex).Task;
                    layer.textures[j] = texture;

                    if (!textureNames.Contains(tex)) {
                        layer.texturesIds.Add(textureNames.Count);
                        textureIdLookup.Add(tex, textureNames.Count);
                        textureNames.Add(tex);
                        textures.Add(texture);
                    } else {
                        layer.texturesIds.Add(textureIdLookup[tex]);
                    }
                }

                //read animations
                var animCount = data.ReadInt();
                layer.animations = new STR.Animation[animCount];
                for (int j = 0; j < animCount; j++) {

                    var entry = new STR.Animation() {
                        frame = data.ReadInt(),
                        type = data.ReadUInt(),
                        position = new Vector2(data.ReadFloat(), data.ReadFloat())
                    };

                    var uv = new float[] {
                        data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat(),
                        data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat()
                    };
                    var xy = new float[] {
                        data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat(),
                        data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat()
                    };

                    entry.uv = new Vector2[4];
                    entry.uv[0] = new Vector2(0, 0);
                    entry.uv[1] = new Vector2(1, 0);
                    entry.uv[2] = new Vector2(0, 1);
                    entry.uv[3] = new Vector2(1, 1);

                    entry.xy = new Vector2[4];
                    entry.xy[0] = new Vector2(xy[0], -xy[4]);
                    entry.xy[1] = new Vector2(xy[1], -xy[5]);
                    entry.xy[2] = new Vector2(xy[3], -xy[7]);
                    entry.xy[3] = new Vector2(xy[2], -xy[6]);

                    entry.animFrame = data.ReadFloat();
                    entry.animType = data.ReadUInt();
                    entry.delay = data.ReadFloat();
                    entry.angle = data.ReadFloat() / (1024f / 360f);
                    entry.color = new Color(data.ReadFloat() / 255, data.ReadFloat() / 255, data.ReadFloat() / 255, data.ReadFloat() / 255);
                    entry.srcAlpha = data.ReadUInt();
                    entry.destAlpha = data.ReadUInt();
                    entry.mtPreset = data.ReadUInt();

                    layer.animations[j] = entry;
                }
            }

            return MakeAtlas(str, path);
        }

        private static STR MakeAtlas(STR str, string path) {
            var baseName = Path.GetFileNameWithoutExtension(path);
            var atlasName = $"{baseName.Replace("\\", "_")}_atlas";

            var extraTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            textures.Add(extraTexture);

            var superTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            superTexture.name = atlasName;

            var atlasRects = superTexture.PackTextures(textures.ToArray(), 2, 4096, false);

            PatchAtlasEdges(superTexture, atlasRects);

            str.AtlasRects = atlasRects;
            str.name = path;

            textureIdLookup.Clear();
            textures.Clear();
            textureNames.Clear();

            return str;
        }

        public static void PatchAtlasEdges(Texture2D atlas, Rect[] rects) {
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