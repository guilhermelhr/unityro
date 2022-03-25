using ROIO.Models.FileTypes;
using System;
using UnityEngine;

namespace Assets.Scripts.Renderer {

    [Serializable]
    public class GameMap : MonoBehaviour {

        [SerializeField]
        private Vector2Int _size;
        public Vector2Int Size => _size;

        [SerializeField]
        private RSW.LightInfo LightInfo;

        private Light WorldLight;

        private void Start() {
            WorldLight = gameObject.AddComponent<Light>();
            WorldLight.type = LightType.Directional;
            WorldLight.shadows = LightShadows.Soft;

            Vector3 lightRotation = new Vector3(LightInfo.longitude, LightInfo.latitude, 0);
            WorldLight.transform.rotation = Quaternion.identity;
            WorldLight.transform.Rotate(lightRotation);

            Color ambient = new Color(LightInfo.ambient[0], LightInfo.ambient[1], LightInfo.ambient[2]);
            Color diffuse = new Color(LightInfo.diffuse[0], LightInfo.diffuse[1], LightInfo.diffuse[2]);

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = ambient * LightInfo.intensity;

            WorldLight.color = diffuse;
        }

        public void SetMapSize(int width, int height) {
            _size = new Vector2Int(width, height);
        }

        public void SetMapLightInfo(RSW.LightInfo lightInfo) {
            LightInfo = lightInfo;
        }
    }
}
