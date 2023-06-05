using UnityEngine;

namespace ROIO.Models.FileTypes {
    public class SPR {

        public const int PIXELS_PER_UNIT = 32;
        public static string Header = "SP";

        public static int TYPE_PAL = 0;
        public static int TYPE_RGBA = 1;

        public string filename;
        public string version;
        public ushort indexedCount;
        public ushort _indexedCount;
        public int rgbaCount;
        public Frame[] frames;
        public ushort rgbaIndex;
        public ushort oldRgbaIndex;
        public byte[] palette;
        public bool compiled = false;

        public class Frame {
            public bool compiled = false;

            public int type { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public byte[] data { get; set; }

            public int originalWidth { get; set; }
            public int originalHeight { get; set; }

            public void Compile(byte[] palette) {
                if (compiled) {
                    return;
                }

                // Calculate new texture size and pos to center
                var gl_width = (int) Mathf.Pow(2, Mathf.Ceil(Mathf.Log(width) / Mathf.Log(2)));
                var gl_height = (int) Mathf.Pow(2, Mathf.Ceil(Mathf.Log(height) / Mathf.Log(2)));
                var start_x = (int) Mathf.Floor((gl_width - width) * 0.5f);
                var start_y = (int) Mathf.Floor((gl_height - height) * 0.5f);
                byte[] _out;
                if (type == TYPE_PAL) {
                    _out = new byte[gl_width * gl_height];

                    for (int y = 0; y < height; y++) {
                        for (int x = 0; x < width; x++) {
                            _out[(y + start_y) * gl_width + x + start_x] = data[y * width + x];

                            if (palette[data[y * width + x] * 4] == 255
                                && palette[data[y * width + x] * 4 + 2] == 255
                                && palette[data[y * width + x] * 4 + 1] == 0) {
                                _out[(y + start_y) * gl_width + x + start_x] = 0;
                            }
                        }
                    }
                } else {
                    _out = new byte[gl_width * gl_height * 4];

                    for (int y = 0; y < height; ++y) {
                        for (int x = 0; x < width; ++x) {
                            _out[((y + start_y) * gl_width + x + start_x) * 4 + 0] = data[((height - y - 1) * width + x) * 4 + 3];
                            _out[((y + start_y) * gl_width + x + start_x) * 4 + 1] = data[((height - y - 1) * width + x) * 4 + 2];
                            _out[((y + start_y) * gl_width + x + start_x) * 4 + 2] = data[((height - y - 1) * width + x) * 4 + 1];
                            _out[((y + start_y) * gl_width + x + start_x) * 4 + 3] = data[((height - y - 1) * width + x) * 4 + 0];
                        }
                    }
                }

                originalWidth = width;
                originalHeight = height;
                width = gl_width;
                height = gl_height;
                data = _out;
                compiled = true;
            }
        }

        public void SwitchToRGBA() {
            SwitchToRGBA(new byte[] { });
        }

        public void SwitchToRGBA(byte[] currentPalette) {
            if (currentPalette.Length > 0) {
                palette = currentPalette;
            }

            for (int i = 0; i < indexedCount; i++) {
                var frame = frames[i];

                if (frame.type == TYPE_PAL) {
                    var _out = new byte[frame.width * frame.height * 4];

                    for (int y = 0; y < frame.height; y++) {
                        for (int x = 0; x < frame.width; x++) {
                            var idx1 = frame.data[x + y * frame.width] * 4;
                            var idx2 = (x + (frame.height - y - 1) * frame.width) * 4;
                            _out[idx2 + 3] = palette[idx1 + 0];
                            _out[idx2 + 2] = palette[idx1 + 1];
                            _out[idx2 + 1] = palette[idx1 + 2];
                            _out[idx2 + 0] = idx1 != 0 ? (byte) 255 : (byte) 0;
                        }
                    }

                    frame.data = _out;
                    frame.type = TYPE_RGBA;
                }
            }

            indexedCount = 0;
            rgbaCount = frames.Length;
            rgbaIndex = 0;
        }

        public void Compile() {
            if (!compiled) {
                for (int i = 0; i < frames.Length; i++) {
                    frames[i].Compile(palette);
                }

                oldRgbaIndex = _indexedCount;
                compiled = true;
            }
        }

        public Sprite[] GetSprites() {
            if (!compiled) {
                throw new System.Exception("SPR.GetAtlas: SPR is not compiled");
            }

            Sprite[] sprites = new Sprite[frames.Length];
            for (int i = 0; i < frames.Length; i++) {
                Frame frame = frames[i];
                Texture2D texture = new Texture2D(frame.width, frame.height, TextureFormat.RGBA32, false);
                texture.LoadRawTextureData(frame.data);
                texture.filterMode = FilterMode.Bilinear;
                texture.Apply();
                /**
                 * This anchor offset is a hack
                 */
                sprites[i] = Sprite.Create(texture, new Rect(0, 0, frame.width, frame.height), new Vector2(0.5f, 0.5f), PIXELS_PER_UNIT, 0, SpriteMeshType.FullRect);
            }

            return sprites;
        }
    }
}