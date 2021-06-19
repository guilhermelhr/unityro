// This was made by aaro4130 on the Unity forums.  Thanks boss!
// It's been optimized and slimmed down for the purpose of loading Quake 3 TGA textures from memory streams.

using System;
using System.IO;
using UnityEngine;

namespace ROIO.Loaders
{
    public static class TGALoader
    {
        public class TGAImage
        {
            public Color32[] data;
            public short width, height;

            public TGAImage(Color32[] data, short width, short height)
            {
                this.data = data;
                this.width = width;
                this.height = height;
            }

            public Texture2D ToTexture2D()
            {
                var tex = new Texture2D(width, height);
                tex.SetPixels32(data);
                tex.Apply();
                return tex;
            }
        }

        public static TGAImage LoadTGA(MemoryStream stream)
        {
            using (BinaryReader r = new BinaryReader(stream))
            {
                // Skip some header info we don't care about.
                // Even if we did care, we have to move the stream seek point to the beginning,
                // as the previous method in the workflow left it at the end.
                r.BaseStream.Seek(12, SeekOrigin.Begin);

                short width = r.ReadInt16();
                short height = r.ReadInt16();
                int bitDepth = r.ReadByte();

                // Skip a byte of header information we don't care about.
                r.BaseStream.Seek(1, SeekOrigin.Current);

                Color32[] pulledColors = new Color32[width * height];

                if (bitDepth == 32)
                {
                    for (int i = 0; i < width * height; i++)
                    {
                        byte red = r.ReadByte();
                        byte green = r.ReadByte();
                        byte blue = r.ReadByte();
                        byte alpha = r.ReadByte();

                        pulledColors[i] = new Color32(blue, green, red, alpha);
                    }
                }
                else if (bitDepth == 24)
                {
                    for (int i = 0; i < width * height; i++)
                    {
                        byte red = r.ReadByte();
                        byte green = r.ReadByte();
                        byte blue = r.ReadByte();

                        pulledColors[i] = new Color32(blue, green, red, 1);
                    }
                }
                else
                {
                    throw new Exception("TGA texture had non 32/24 bit depth.");
                }

                return new TGAImage(pulledColors, width, height);
            }
        }
    }
}