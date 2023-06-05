using ROIO.Models.FileTypes;
using ROIO.Utils;
using System;
using System.IO;
using UnityEngine;
using UnityRO.Core;

namespace Assets.Scripts.Renderer.Map {

    [Serializable]
    public class WaterRenderer : ManagedMonoBehaviour {

        [SerializeField]
        private RSW.WaterInfo WaterInfo;

        private Material material;
        private int currentTextureId;
        private Texture2D[] textures;

        private void Start() {
            material = gameObject.GetComponent<MeshRenderer>().material;

            material.SetFloat("Wave Height", WaterInfo.waveHeight);
            material.SetFloat("Wave Pitch", WaterInfo.wavePitch);

            textures = new Texture2D[32];
            for (var i = 0; i < 32; i++) {
                var texture = Resources.Load<Texture2D>($"Textures/Water/{Path.GetFileNameWithoutExtension(WaterInfo.images[i])}");
                textures[i] = texture;
            }
        }

        public override void ManagedUpdate() {
            float frame = Time.time / (1 / 60f);
            int textureId = (int) Conversions.SafeDivide(frame, WaterInfo.animSpeed) % 32;

            float offset = frame * WaterInfo.waveSpeed;
            material.SetFloat("_WaterOffset", offset);

            if (currentTextureId != textureId) {
                material.mainTexture = textures[textureId];
                currentTextureId = textureId;
            }
        }

        public void SetWaterInfo(RSW.WaterInfo waterInfo) {
            WaterInfo = waterInfo;
        }

    }
}
