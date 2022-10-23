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

        public static STR Load(MemoryStreamReader data, string path) {
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
                    var texture = Addressables.LoadAssetAsync<Texture2D>(path + "/" + Path.ChangeExtension(tex, ".png")).WaitForCompletion();
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

            return str;
        }
    }
}