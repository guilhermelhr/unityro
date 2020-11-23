
using System;
using System.Globalization;

public class SpriteLoader
{
    public static SPR Load(BinaryReader data) {
        var header = data.ReadBinaryString(2);
        if(!header.Equals(SPR.Header)) {
            throw new Exception("SpriteLoader.Load: Header (" + header + ") is not \"SP\"");
        }

        SPR spr = new SPR();

        string subversion = Convert.ToString(data.ReadUByte());
        string version = Convert.ToString(data.ReadUByte());
        version += "." + subversion;

        spr.version = version;
        spr.indexedCount = data.ReadUShort();
        spr._indexedCount = spr.indexedCount;

        var dversion = Double.Parse(version, CultureInfo.InvariantCulture);
        if(dversion > 1.1) {
            spr.rgbaCount = data.ReadUShort();
        }

        spr.frames = new SPR.Frame[spr.indexedCount + spr.rgbaCount];
        spr.rgbaIndex = spr.indexedCount;

        if(dversion < 2.1) {
            ReadIndexedImage(spr, data);
        } else {
            ReadIndexedImageRLE(spr, data);
        }

        ReadRgbaImage(spr, data);

        if(dversion > 1.0) {
            long position = data.Position;
            data.Seek(-1024, System.IO.SeekOrigin.End);
            spr.palette = data.ReadUBytes(1024);
            data.Seek(position, System.IO.SeekOrigin.Begin);
        }

        return spr;
    }

    private static void ReadIndexedImage(SPR spr, BinaryReader data) {
        for(int i = 0; i < spr.indexedCount; i++) {
            var width = data.ReadUShort();
            var height = data.ReadUShort();
            spr.frames[i] = new SPR.Frame() {
                type = SPR.TYPE_PAL,
                width = width,
                height = height,
                data = data.ReadUBytes(width * height)
            };
        }
    }

    private static void ReadIndexedImageRLE(SPR spr, BinaryReader data) {
        for(int i = 0; i < spr.indexedCount; i++) {
            var width = data.ReadUShort();
            var height = data.ReadUShort();
            var _data = new byte[width * height];
            var end = data.ReadUShort() + data.Position;

            var index = 0;
            while(data.Position < end) {
                var c = _data[index++] = data.ReadUByte();
                if(c == 0) {
                    var count = data.ReadUByte();

                    if(count == 0) {
                        _data[index++] = 0;
                    } else {
                        for(int j = 1; j < count; j++) {
                            _data[index++] = c;
                        }
                    }
                }
            }

            spr.frames[i] = new SPR.Frame() {
                type = SPR.TYPE_PAL,
                width = width,
                height = height,
                data = _data
            };
        }
    }

    private static void ReadRgbaImage(SPR spr, BinaryReader data) {
        for(int i = 0; i < spr.rgbaCount; i++) {
            var width = data.ReadShort();
            var height = data.ReadShort();

            spr.frames[i + spr.rgbaIndex] = new SPR.Frame() {
                type = SPR.TYPE_RGBA,
                width = width,
                height = height,
                data = data.ReadUBytes(width * height * 4)
            };
        }
    }    
}
