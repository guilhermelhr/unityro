using ROIO.Models.FileTypes;
using System;
using UnityEngine;

[Serializable]
public class LightContainer : MonoBehaviour {

    [SerializeField]
    private Light Light;

    public void SetLightProps(RSW.Light light, uint height, uint width) {
        // hack alert
        // a renderer is needed to trigger the functions below
        // we need those functions because unity won't render many light points
        gameObject.AddComponent<SpriteRenderer>();

        Light = gameObject.AddComponent<Light>();
        Light.color = new Color(light.color[0], light.color[1], light.color[2]);
        Light.range = light.range / 2;
        Light.intensity = light.range / 2;
        Vector3 position = new Vector3(light.pos[0] + width, -light.pos[1], light.pos[2] + height);
        gameObject.transform.position = position;

        Light.enabled = false;
    }

    private void OnBecameVisible() {
        Light.enabled = true;
    }

    private void OnBecameInvisible() {
        Light.enabled = false;
    }

}
