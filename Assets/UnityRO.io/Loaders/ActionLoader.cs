using ROIO.Models.FileTypes;
using ROIO.Utils;
using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace ROIO.Loaders
{
    public class ActionLoader
    {
        public static ACT Load(MemoryStreamReader data)
        {
            string header = data.ReadBinaryString(2);
            if (!header.Equals(ACT.Header))
            {
                throw new Exception("ActionLoader.Load: Header \"" + header + "\" is not \"AC\"");
            }

            string subversion = Convert.ToString(data.ReadByte());
            string version = Convert.ToString(data.ReadByte());
            version += "." + subversion;

            double dversion = double.Parse(version, CultureInfo.InvariantCulture);

            ACT act = new ACT();
            act.version = version;

            ReadActions(act, data);

            if (dversion >= 2.1)
            {
                //sounds
                var count = data.ReadInt();
                act.sounds = new string[count];

                for (int i = 0; i < count; i++)
                {
                    act.sounds[i] = data.ReadBinaryString(40);
                }

                //delay
                if (dversion >= 2.2)
                {
                    for (int i = 0; i < act.actions.Length; i++)
                    {
                        act.actions[i].delay = data.ReadFloat() * 25;
                    }
                }
            }

            return act;
        }

        private static void ReadActions(ACT act, MemoryStreamReader data)
        {
            var count = data.ReadUShort();
            data.Seek(10, System.IO.SeekOrigin.Current);

            act.actions = new ACT.Action[count];
            for (int i = 0; i < count; i++)
            {
                act.actions[i] = new ACT.Action()
                {
                    frames = ReadMotions(act, data),
                    delay = 150f
                };
            }
        }

        private static ACT.Frame[] ReadMotions(ACT act, MemoryStreamReader data)
        {
            var count = data.ReadUInt();
            var motions = new ACT.Frame[count];

            for (int i = 0; i < count; i++)
            {
                data.Seek(32, System.IO.SeekOrigin.Current);
                motions[i] = ReadLayers(act, data);
            }

            return motions;
        }

        private static ACT.Frame ReadLayers(ACT act, MemoryStreamReader data)
        {
            var count = data.ReadUInt();
            var layers = new ACT.Layer[count];
            var version = double.Parse(act.version, CultureInfo.InvariantCulture);

            for (int i = 0; i < count; i++)
            {
                var layer = layers[i] = new ACT.Layer()
                {
                    pos = new Vector2Int(data.ReadInt(), data.ReadInt()),
                    index = data.ReadInt(),
                    isMirror = data.ReadInt() != 0,
                    scale = Vector2.one,
                    color = Color.white
                };

                // RoRebuild checks if only if it's greater
                if (version > 2.0)
                {
                    layer.color[0] = data.ReadByte() / 255f; //r
                    layer.color[1] = data.ReadByte() / 255f; //g
                    layer.color[2] = data.ReadByte() / 255f; //b
                    layer.color[3] = data.ReadByte() / 255f; //a

                    layer.scale[0] = data.ReadFloat();
                    layer.scale[1] = version <= 2.3 ? layer.scale[0] : data.ReadFloat();

                    layer.angle = data.ReadInt();
                    layer.sprType = data.ReadInt();

                    if (version >= 2.5)
                    {
                        layer.width = data.ReadInt();
                        layer.height = data.ReadInt();
                    }
                }
            }

            var soundId = version >= 2.0 ? data.ReadInt() : -1;
            Vector2Int[] pos = null;

            if (version >= 2.3)
            {
                pos = new Vector2Int[data.ReadInt()];
                for (int i = 0; i < pos.Length; i++)
                {
                    data.Seek(4, System.IO.SeekOrigin.Current);
                    pos[i] = new Vector2Int(data.ReadInt(), data.ReadInt());
                    data.Seek(4, System.IO.SeekOrigin.Current);
                }
            }

            return new ACT.Frame()
            {
                layers = layers.Where(t => t.index >= 0).ToArray(),
                soundId = soundId,
                pos = pos
            };
        }
    }
}