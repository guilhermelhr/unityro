using ROIO.Models.FileTypes;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ROIO.Loaders {
    public struct SpriteFrameData {
        public bool IsIndexed;
        public int Width;
        public int Height;
        public byte[] Data;
    }

    public class CustomSpriteLoader {
        private MemoryStream ms;
        private BinaryReader br;

        private int version;
        private int indexCount;
        private int rgbaCount;

        public List<SpriteFrameData> spriteFrames;
        private byte[] paletteData;

        public List<Texture2D> Textures = new List<Texture2D>();
        public List<Sprite> Sprites = new List<Sprite>();
        public List<Vector2Int> SpriteSizes = new List<Vector2Int>();

        public Texture2D Atlas;
        public Texture2D Palette;
        public Rect[] SpriteRects;

        public int IndexCount => indexCount;

        private void ReadIndexedImage() {
            for (var i = 0; i < indexCount; i++) {
                var width = br.ReadUInt16();
                var height = br.ReadUInt16();
                var data = br.ReadBytes(width * height);

                var frame = new SpriteFrameData() {
                    IsIndexed = true,
                    Width = width,
                    Height = height,
                    Data = data
                };

                spriteFrames.Add(frame);
            }
        }

        private void ReadRleIndexedImage() {
            for (var i = 0; i < indexCount; i++) {
                var width = br.ReadUInt16();
                var height = br.ReadUInt16();
                var size = width * height;
                var data = new byte[size];
                var index = 0;
                var end = br.ReadUInt16() + ms.Position;

                while (ms.Position < end) {
                    var c = br.ReadByte();
                    data[index++] = c;

                    if (c == 0) {
                        var count = br.ReadByte();
                        if (count == 0) {
                            data[index++] = count;
                        } else {
                            for (var j = 1; j < count; j++) {
                                data[index++] = c;
                            }
                        }
                    }
                }

                var frame = new SpriteFrameData() {
                    IsIndexed = true,
                    Width = width,
                    Height = height,
                    Data = data
                };

                spriteFrames.Add(frame);
            }
        }

        private void ReadRgbaImage() {
            for (var i = 0; i < rgbaCount; i++) {
                var width = br.ReadInt16();
                var height = br.ReadInt16();

                var frame = new SpriteFrameData() {
                    IsIndexed = false,
                    Width = width,
                    Height = height,
                    Data = br.ReadBytes(width * height * 4)
                };

                spriteFrames.Add(frame);
            }
        }

        private Texture2D RgbaToTexture(SpriteFrameData frame) {
            var image = new Texture2D(frame.Width, frame.Height, TextureFormat.ARGB32, false);
            image.wrapMode = TextureWrapMode.Clamp;
#if UNITY_EDITOR
            image.alphaIsTransparency = true;
#endif

            var colors = new Color[frame.Width * frame.Height];

            for (var y = 0; y < frame.Height; y++) {
                for (var x = 0; x < frame.Width; x++) {
                    var pos = (x + (frame.Height - y - 1) * frame.Width) * 4;

                    var r = frame.Data[pos + 3];
                    var g = frame.Data[pos + 2];
                    var b = frame.Data[pos + 1];
                    var a = frame.Data[pos + 0];

                    var color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);

                    colors[x + (frame.Height - y - 1) * frame.Width] = color;
                }
            }

            ExtendSpriteTextureData(colors, frame);

            image.SetPixels(colors);

            return image;
        }

        private Texture2D IndexedToTexture(SpriteFrameData frame, bool stripPalette) {
            var format = stripPalette ? TextureFormat.R8 : TextureFormat.ARGB32;
            Texture2D image;
            if (stripPalette) {
                image = new Texture2D(frame.Width, frame.Height, format, false, true) {
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Point
                };
                image.LoadRawTextureData(frame.Data);
                image.Apply();
                
                var flipped = new Texture2D(frame.Width, frame.Height, format, false, true) {
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Point,
                    anisoLevel = 3
                };

                var width = frame.Width;
                var height = frame.Height;

                for (var x = 0; x < width; x++) {
                    for (var y = 0; y < height; y++) {
                        flipped.SetPixel(x, height - y - 1, image.GetPixel(x, y));
                    }
                }

                flipped.Apply();

                return flipped;
            }

            image = new Texture2D(frame.Width, frame.Height, format, false) {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };
#if UNITY_EDITOR
            image.alphaIsTransparency = true;
#endif
            var colors = new Color[frame.Width * frame.Height];

            for (var y = 0; y < frame.Height; y++) {
                for (var x = 0; x < frame.Width; x++) {
                    var index1 = frame.Data[x + y * frame.Width] * 4;

                    var r = paletteData[index1 + 0];
                    var g = paletteData[index1 + 1];
                    var b = paletteData[index1 + 2];
                    var a = index1 > 0 ? 255 : 0;

                    var color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);

                    colors[x + (frame.Height - y - 1) * frame.Width] = color;
                }
            }

            ExtendSpriteTextureData(colors, frame);

            image.SetPixels(colors);
                
            return image;
        }

        private void ExtendSpriteTextureData(Color[] colors, SpriteFrameData frame) {
            //we're going to extend the sprite color into the transparent area around the sprite
            //this is to make bilinear filtering work good with the sprite

            for (var y = 0; y < frame.Height; y++) {
                for (var x = 0; x < frame.Width; x++) {
                    var count = 0;
                    var r = 0f;
                    var g = 0f;
                    var b = 0f;

                    var color = colors[x + y * frame.Width];
                    if (!Mathf.Approximately(color.a, 0))
                        continue;

                    for (var y2 = -1; y2 <= 1; y2++) {
                        for (var x2 = -1; x2 <= 1; x2++) {
                            if (y + y2 < 0 || y + y2 >= frame.Height)
                                continue;
                            if (x + x2 < 0 || x + x2 >= frame.Width)
                                continue;
                            var color2 = colors[x + x2 + (y + y2) * frame.Width];

                            if (Mathf.Approximately(color2.a, 0))
                                continue;

                            count++;

                            r += color2.r;
                            g += color2.g;
                            b += color2.b;
                        }
                    }

                    if (count > 0) {
                        colors[x + y * frame.Width] = new Color(r / count, g / count, b / count, 0);
                    }
                }
            }
        }

        private void ReadPalette() {
            paletteData = br.ReadBytes(1024);
        }

        public void Load(byte[] spriteData, string filename, bool stripPalette = true) {
            var basename = Path.GetFileNameWithoutExtension(filename);

            ms = new MemoryStream(spriteData);
            br = new BinaryReader(ms);

            var header = new string(br.ReadChars(2));
            if (header != "SP")
                throw new Exception("Not sprite");

            var minorVersion = br.ReadByte();
            var majorVersion = br.ReadByte();
            version = majorVersion * 10 + minorVersion;

            indexCount = br.ReadUInt16();
            rgbaCount = 0;

            if (version > 11)
                rgbaCount = br.ReadUInt16();


            var frameCount = indexCount + rgbaCount;
            var rgbaIndex = indexCount;

            spriteFrames = new List<SpriteFrameData>(frameCount);

            if (version < 21)
                ReadIndexedImage();
            else
                ReadRleIndexedImage();

            ReadRgbaImage();

            if (version > 10)
                ReadPalette();

            Palette = new Texture2D(256, 1, TextureFormat.RGBA32, false, true) {
#if UNITY_EDITOR
                alphaIsTransparency = false,
#endif
                filterMode = FilterMode.Point
            };
            Palette.LoadRawTextureData(paletteData);
            Palette.Apply();

            for (var i = 0; i < spriteFrames.Count; i++) {
                var image = spriteFrames[i].IsIndexed ? IndexedToTexture(spriteFrames[i], stripPalette) : RgbaToTexture(spriteFrames[i]);
                image.name = $"{basename}_{i:D4}";
                image.hideFlags = HideFlags.HideInHierarchy;

                Textures.Add(image);
            }

            var superTexture = new Texture2D(2, 2, TextureFormat.R8, false) {
                name = $"{basename}_atlas"
            };
            var rects = superTexture.PackTextures(Textures.ToArray(), 2, 2048, false);
            SpriteRects = rects;
            superTexture.filterMode = FilterMode.Point;
            superTexture.anisoLevel = 3;

            Atlas = superTexture;

            for (var i = 0; i < rects.Length; i++) {
                var rect = new Rect(rects[i].x * superTexture.width, rects[i].y * superTexture.height,
                    rects[i].width * superTexture.width, rects[i].height * superTexture.height);

                SpriteSizes.Add(new Vector2Int(Textures[i].width, Textures[i].height));
                var sprite = Sprite.Create(superTexture, rect, new Vector2(0.5f, 0.5f), SPR.PIXELS_PER_UNIT, 0,
                    SpriteMeshType.FullRect);

                sprite.name = $"sprite_{basename}_{i:D4}";

                Sprites.Add(sprite);
            }

            br.Dispose();
            ms.Dispose();
        }
    }
}