using ROIO.Models.FileTypes;
using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ROIO.Loaders {
    /// <summary>
    /// Loaders for .rsw file
    /// 
    /// @author Guilherme Hernandez
    /// Based on ROBrowser by Vincent Thibault (robrowser.com)
    /// </summary>
    public class WorldLoader {
        public static Files files = new Files();

        public struct Files {
            public string ini;
            public string gnd;
            public string gat;
            public string src;
        }

        public static RSW Load(MemoryStreamReader data, string file) {
            //read header
            string header = data.ReadBinaryString(4);
            string version = Convert.ToString(data.ReadByte());
            string subversion = Convert.ToString(data.ReadByte());
            version += "." + subversion;
            double dversion = double.Parse(version, CultureInfo.InvariantCulture);
            int buildNumber = dversion >= 2.2 ? data.ReadByte() : 0;
            uint misteryBytes = dversion >= 2.5 ? data.ReadUInt() : 0;

            //check for valid .rsw file
            if (!string.Equals(header, RSW.Header)) {
                throw new Exception($"WorldLoader.Load: Header ({header}) is not \"GRSW\" at file {file}");
            }

            RSW rsw = new RSW(version);

            //read sub files
            files.ini = data.ReadBinaryString(40);
            files.gnd = data.ReadBinaryString(40);
            files.gat = data.ReadBinaryString(40);

            if (dversion >= 1.4) {
                files.src = data.ReadBinaryString(40);
            }

            //read water info
            if (dversion >= 1.3) {
                rsw.water.level = data.ReadFloat() / 5;

                if (dversion >= 1.8) {
                    rsw.water.type = data.ReadInt();
                    rsw.water.waveHeight = data.ReadFloat() / 5;
                    rsw.water.waveSpeed = data.ReadFloat();
                    rsw.water.wavePitch = data.ReadFloat();

                    if (dversion >= 1.9) {
                        rsw.water.animSpeed = data.ReadInt();
                    }
                }
            }

            //read lightmap
            if (dversion >= 1.5) {
                rsw.light.longitude = data.ReadInt();
                rsw.light.latitude = data.ReadInt();
                for (int i = 0; i < 3; i++) {
                    rsw.light.diffuse[i] = data.ReadFloat();
                }
                for (int i = 0; i < 3; i++) {
                    rsw.light.ambient[i] = data.ReadFloat();
                }

                if (dversion >= 1.7) {
                    rsw.light.intensity = data.ReadFloat();
                }
            }

            // Read ground
            if (dversion >= 1.6) {
                rsw.ground.top = data.ReadInt();
                rsw.ground.bottom = data.ReadInt();
                rsw.ground.left = data.ReadInt();
                rsw.ground.right = data.ReadInt();
            }

            // Read Object
            int count = data.ReadInt();
            var models = rsw.modelDescriptors = new List<RSW.ModelDescriptor>(count);
            var lights = rsw.lights = new List<RSW.Light>(count);
            var sounds = rsw.sounds = new List<RSW.Sound>(count);
            var effects = rsw.effects = new List<RSW.Effect>(count);

            for (int i = 0; i < count; i++) {
                switch (data.ReadInt()) {
                    case 1: //load model
                        var model = new RSW.ModelDescriptor();
                        
                        if (dversion >= 2.6 && buildNumber > 161) {
                            model.misteryByte = data.ReadByte();
                        }

                        model.name = dversion >= 1.3 ? data.ReadBinaryString(40) : null;
                        model.animType = dversion >= 1.3 ? data.ReadInt() : 0;
                        model.animSpeed = dversion >= 1.3 ? data.ReadFloat() : 0f;
                        model.blockType = dversion >= 1.3 ? data.ReadInt() : 0;
                        model.filename = data.ReadBinaryString(80);
                        model.nodename = data.ReadBinaryString(80);
                        model.position = new float[3];
                        for (int j = 0; j < model.position.Length; j++) {
                            model.position[j] = data.ReadFloat() / 5;
                        }
                        model.rotation = new float[3];
                        for (int j = 0; j < model.rotation.Length; j++) {
                            model.rotation[j] = data.ReadFloat();
                        }
                        model.scale = new float[3];
                        for (int j = 0; j < model.scale.Length; j++) {
                            model.scale[j] = data.ReadFloat() / 5;
                        }
                        models.Add(model);
                        continue;
                    case 2: //load light
                        var light = new RSW.Light();
                        light.name = data.ReadBinaryString(80);
                        light.pos = new float[3];
                        for (int j = 0; j < light.pos.Length; j++) {
                            light.pos[j] = data.ReadFloat() / 5;
                        }
                        light.color = new float[3];
                        for (int j = 0; j < light.color.Length; j++) {
                            light.color[j] = data.ReadFloat();
                        }
                        light.range = data.ReadFloat();
                        lights.Add(light);
                        continue;
                    case 3: //load sound
                        var sound = new RSW.Sound();
                        sound.name = data.ReadBinaryString(80);
                        sound.file = "data/wav/" + data.ReadBinaryString(80);
                        sound.pos = new float[3];
                        for (int j = 0; j < sound.pos.Length; j++) {
                            sound.pos[j] = data.ReadFloat() / 5;
                        }
                        sound.vol = data.ReadFloat();
                        sound.width = data.ReadInt();
                        sound.height = data.ReadInt();
                        sound.range = data.ReadFloat();
                        sound.cycle = dversion >= 2.0 ? data.ReadFloat() : 0f;
                        sounds.Add(sound);
                        continue;
                    case 4: //load effect
                        var effect = new RSW.Effect();
                        effect.name = data.ReadBinaryString(80);
                        effect.pos = new float[3];
                        for (int j = 0; j < effect.pos.Length; j++) {
                            effect.pos[j] = data.ReadFloat() / 5;
                        }
                        effect.id = data.ReadInt();
                        effect.delay = data.ReadFloat() * 10;
                        effect.param = new float[4];
                        for (int j = 0; j < effect.param.Length; j++) {
                            effect.param[j] = data.ReadFloat();
                        }
                        effects.Add(effect);
                        continue;
                }
            }

            models.TrimExcess();
            sounds.TrimExcess();
            lights.TrimExcess();
            effects.TrimExcess();

            return rsw;
        }

    }
}