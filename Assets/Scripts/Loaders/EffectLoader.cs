using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EffectLoader
{
    public static STR Load(BinaryReader data) {
        var header = data.ReadBinaryString(4);

        if(!header.Equals(STR.Header)) {
            throw new Exception("EffectLoader.Load: Header (" + header + ") is not \"STRM\"");
        }

        var version = data.ReadULong();
        if(version != 0x94) {
            throw new Exception("EffectLoader.Load: Unsupported STR version (v" + version +")");
        }

        STR str = new STR();
        str.version = version;
        str.fps = data.ReadULong();
        str.maxKey = data.ReadULong();
        var layerCount = data.ReadULong();
        data.Seek(16, System.IO.SeekOrigin.Current);


        //read layers
        str.layers = new STR.Layer[layerCount];
        for(uint i = 0; i < layerCount; i++) {
            STR.Layer layer = str.layers[i] = new STR.Layer();

            //read texture filenames
            var textureCount = data.ReadLong();
            layer.textures = new Texture2D[textureCount];
            for(int j = 0; j < textureCount; j++) {
                layer.textures[j] =  FileManager.Load("data/texture/effect/" + data.ReadBinaryString(128)) as Texture2D;
            }

            //read animations
            var animCount = data.ReadLong();
            layer.animations = new STR.Animation[animCount];
            for(int j = 0; j < animCount; j++) {
                layer.animations[j] = new STR.Animation() {
                    frame = data.ReadLong(),
                    type = data.ReadULong(),
                    position = new Vector2(data.ReadFloat(), data.ReadFloat()),
                    uv = new float[] {
                        data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat(),
                        data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat()
                    },
                    xy = new float[] {
                        data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat(),
                        data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat()
                    },
                    animFrame = data.ReadFloat(),
                    animType = data.ReadULong(),
                    delay = data.ReadFloat(),
                    angle = data.ReadFloat() / (1024/360),
                    color = new Color(
                        data.ReadFloat() / 255, data.ReadFloat() / 255, 
                        data.ReadFloat() / 255, data.ReadFloat() / 255),
                    srcAlpha = data.ReadULong(),
                    destAlpha = data.ReadULong(),
                    mtPreset = data.ReadULong()
                };
            }
        }

        return str;
    }
}
