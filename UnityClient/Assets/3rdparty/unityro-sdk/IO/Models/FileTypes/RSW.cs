using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROIO.Models.FileTypes
{
    public class RSW
    {
        public static string Header = "GRSW";

        public string version;
        public List<Sound> sounds;
        public List<Light> lights;
        public List<Effect> effects;
        public List<ModelDescriptor> modelDescriptors;
        public GroundInfo ground = new GroundInfo(-500, 500, -500, 500);
        public WaterInfo water = new WaterInfo(0.0f, 0, 0.2f, 2f, 50f, 3, new string[32]);
        public LightInfo light = new LightInfo(45, 45, new float[] { 1f, 1f, 1f }, new float[] { 0.3f, 0.3f, 0.3f }, 1f, new Vector3());

        public RSW(string version)
        {
            this.version = version;
        }

        public class GroundInfo
        {
            public int top;
            public int bottom;
            public int left;
            public int right;

            public GroundInfo(int top, int bottom, int left, int right)
            {
                this.top = top;
                this.bottom = bottom;
                this.left = left;
                this.right = right;
            }
        }

        [Serializable]
        public class WaterInfo
        {
            [SerializeField] public int textureSize = 256;
            [SerializeField] public float level;
            [SerializeField] public int type;
            [SerializeField] public float waveHeight;
            [SerializeField] public float waveSpeed;
            [SerializeField] public float wavePitch;
            [SerializeField] public int animSpeed;
            [SerializeField] public string[] images;

            public WaterInfo(float level, int type, float waveHeight, float waveSpeed, float wavePitch, int animSpeed, string[] images)
            {
                this.level = level;
                this.type = type;
                this.waveHeight = waveHeight;
                this.waveSpeed = waveSpeed;
                this.wavePitch = wavePitch;
                this.animSpeed = animSpeed;
                this.images = images;
            }
        }

        [Serializable]
        public class LightInfo
        {
            [SerializeField] public int longitude;
            [SerializeField] public int latitude;
            [SerializeField] public float[] diffuse;
            [SerializeField] public float[] ambient;
            [SerializeField] public float intensity;
            [SerializeField] public Vector3 direction;

            public LightInfo(int longitude, int latitude, float[] diffuse, float[] ambient, float intensity, Vector3 direction)
            {
                this.longitude = longitude;
                this.latitude = latitude;
                this.diffuse = diffuse;
                this.ambient = ambient;
                this.intensity = intensity;
                this.direction = direction;
            }
        }

        public class ModelDescriptor
        {
            public string name;
            public int misteryByte;
            public int animType;
            public float animSpeed;
            public int blockType;
            public string filename;
            public string nodename;
            public float[] position;
            public float[] rotation;
            public float[] scale;
        }

        [Serializable]
        public class Light
        {
            [SerializeField] public string name;
            [SerializeField] public float[] pos;
            [SerializeField] public float[] color;
            [SerializeField] public float range;
        }

        [Serializable]
        public class Sound
        {
            [SerializeField] public string name;
            [SerializeField] public string file;
            [SerializeField] public float[] pos;
            [SerializeField] public float vol;
            [SerializeField] public int width;
            [SerializeField] public int height;
            [SerializeField] public float range;
            [SerializeField] public float cycle;
            [SerializeField] public float tick;
        }

        [Serializable]
        public class Effect
        {
            [SerializeField] public string name;
            [SerializeField] public float[] pos;
            [SerializeField] public int id;
            [SerializeField] public float delay;
            [SerializeField] public float[] param;
        }
    }
}