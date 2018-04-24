

using System.Collections.Generic;
using UnityEngine;

public class RSW {
    public static string Header = "GRSW";

    public string version;
    public List<Sound> sounds;
    public List<Light> lights;
    public List<Effect> effects;
    public List<Model> models;
    public GroundInfo ground = new GroundInfo(-500, 500, -500, 500);
    public WaterInfo water = new WaterInfo(0.0f, 0, 0.2f, 2f, 50f, 3, new string[32]);
    public LightInfo light = new LightInfo(45, 45, new float[]{1f, 1f, 1f}, new float[]{0.3f, 0.3f, 0.3f}, 1f, new Vector3());

    public RSW(string version) {
        this.version = version;
    }

    public class GroundInfo {
        public int top;
        public int bottom;
        public int left;
        public int right;

        public GroundInfo(int top, int bottom, int left, int right) {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }
    }

    public class WaterInfo {
        public int textureSize = 256;
        public float level;
        public int type;
        public float waveHeight;
        public float waveSpeed;
        public float wavePitch;
        public int animSpeed;
        public string[] images;

        public WaterInfo(float level, int type, float waveHeight, float waveSpeed, float wavePitch, int animSpeed, string[] images) {
            this.level = level;
            this.type = type;
            this.waveHeight = waveHeight;
            this.waveSpeed = waveSpeed;
            this.wavePitch = wavePitch;
            this.animSpeed = animSpeed;
            this.images = images;
        }
    }

    public class LightInfo {
        public int longitude;
        public int latitude;
        public float[] diffuse;
        public float[] ambient;
        public float opacity;
        public Vector3 direction;

        public LightInfo(int longitude, int latitude, float[] diffuse, float[] ambient, float opacity, Vector3 direction) {
            this.longitude = longitude;
            this.latitude = latitude;
            this.diffuse = diffuse;
            this.ambient = ambient;
            this.opacity = opacity;
            this.direction = direction;
        }
    }

    public class Model {
        public string name;
        public int animType;
        public float animSpeed;
        public int blockType;
        public string filename;
        public string nodename;
        public float[] position;
        public float[] rotation;
        public float[] scale;
    }

    public class Light {
        public string name;
        public float[] pos;
        public int[] color;
        public float range;
    }

    public class Sound {
        public string name;
        public string file;
        public float[] pos;
        public float vol;
        public int width;
        public int height;
        public float range;
        public float cycle;
    }

    public class Effect {
        public string name;
        public float[] pos;
        public int id;
        public float delay;
        public float[] param;
    }
}
