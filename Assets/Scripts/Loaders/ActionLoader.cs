
using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class ActionLoader
{
    public static ACT Load(BinaryReader data) {
        string header = data.ReadBinaryString(2);
        if(!header.Equals(ACT.Header)) {
            throw new Exception("ActionLoader.Load: Header \"" + header + "\" is not \"AC\"");
        }

        string subversion = Convert.ToString(data.ReadUByte());
        string version = Convert.ToString(data.ReadUByte());
        version += "." + subversion;

        double dversion = double.Parse(version, CultureInfo.InvariantCulture);

        ACT act = new ACT();
        act.version = version;

        ReadActions(act, data);
        
        if(dversion >= 2.1) {
            //sounds
            var count = data.ReadLong();
            act.sounds = new string[count];

            for(int i = 0; i < count; i++) {
                act.sounds[i] = data.ReadBinaryString(40);
            }

            //delay
            if(dversion >= 2.2) {
                for(int i = 0; i < act.actions.Length; i++) {
                    act.actions[i].delay = data.ReadFloat() * 25;
                }
            }
        }

        return act;
    }

    private static void ReadActions(ACT act, BinaryReader data) {
        var count = data.ReadUShort();
        data.Seek(10, System.IO.SeekOrigin.Current);

        act.actions = new ACT.Action[count];
        for(int i = 0; i < count; i++) {
            act.actions[i] = new ACT.Action() {
                motions = ReadMotions(act, data),
                delay = 150f
            };
        }
    }

    private static ACT.Motion[] ReadMotions(ACT act, BinaryReader data) {
        var count = data.ReadULong();
        var motions = new ACT.Motion[count];

        for(int i = 0; i < count; i++) {
            data.Seek(32, System.IO.SeekOrigin.Current);
            motions[i] = ReadLayers(act, data);
        }

        return motions;
    }

    private static ACT.Motion ReadLayers(ACT act, BinaryReader data) {
        var count = data.ReadULong();
        var layers = new ACT.Layer[count];
        var version = double.Parse(act.version, CultureInfo.InvariantCulture);

        for(int i = 0; i < count; i++) {
            var layer = layers[i] = new ACT.Layer() {
                pos = new Vector2Int(data.ReadLong(), data.ReadLong()),
                index = data.ReadLong(),
                isMirror = data.ReadLong() != 0,
                scale = Vector2.one,
                color = Color.white
            };

            // RoRebuild checks if only if it's greater
            if(version > 2.0) {
                layer.color[0] = data.ReadUByte() / 255f; //r
                layer.color[1] = data.ReadUByte() / 255f; //g
                layer.color[2] = data.ReadUByte() / 255f; //b
                layer.color[3] = data.ReadUByte() / 255f; //a

                layer.scale[0] = data.ReadFloat();
                layer.scale[1] = version <= 2.3 ? layer.scale[0] : data.ReadFloat();
                
                layer.angle = data.ReadLong();
                layer.sprType = data.ReadLong();

                if(version >= 2.5) {
                    layer.width = data.ReadLong();
                    layer.height = data.ReadLong();
                }
            }
        }

        var soundId = version >= 2.0 ? data.ReadLong() : -1;
        Vector2Int[] pos = null;

        if(version >= 2.3) {
            pos = new Vector2Int[data.ReadLong()];
            for(int i = 0; i < pos.Length; i++) {
                data.Seek(4, System.IO.SeekOrigin.Current);
                pos[i] = new Vector2Int(data.ReadLong(), data.ReadLong());
                data.Seek(4, System.IO.SeekOrigin.Current);
            }
        }

        return new ACT.Motion() {
            layers = layers.Where(t => t.index >= 0).ToArray(),
            soundId = soundId,
            pos = pos
        };
    }
}
