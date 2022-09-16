using ROIO;
using ROIO.Models.FileTypes;
using ROIO.Utils;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Renderer.Map {

    [Serializable]
    public class WaterRenderer : MonoBehaviour {

        [SerializeField]
        private RSW.WaterInfo WaterInfo;

        private Material material;
        private int currentTextureId;
        private Texture2D[] textures;

        private async void Start() {
            material = gameObject.GetComponent<MeshRenderer>().material;

            material.SetFloat("Wave Height", WaterInfo.waveHeight);
            material.SetFloat("Wave Pitch", WaterInfo.wavePitch);

            textures = new Texture2D[32];
            for (int i = 0; i < 32; i++) {
                var texture = await Addressables.LoadAssetAsync<Texture2D>(Path.ChangeExtension(WaterInfo.images[i], ".png").SanitizeForAddressables()).Task;
                textures[i] = texture;
            }
        }

        private void FixedUpdate() {
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
