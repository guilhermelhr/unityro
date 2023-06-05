using ROIO.Models.FileTypes;
using System;
using Core.Path;
using UnityEngine;
using UnityRO.Core.Extensions;


[Serializable]
public class GameMap : MonoBehaviour {
    [SerializeField] private Vector2Int _size;
    public Vector2Int Size => _size;
    
    [SerializeField] [HideInInspector] private Light DirectionalLight;

    [SerializeField] [HideInInspector] private RSW.LightInfo LightInfo;
    [HideInInspector] [SerializeField] private Altitude Altitude;
    
    private PathFinder PathFinder;

    private void Awake() {
        InitWorldLight();
        InitPathFinder();
    }

    private void InitPathFinder() {
        PathFinder = gameObject.GetOrAddComponent<PathFinder>();
        PathFinder.SetMap(Altitude);
    }

    private void InitWorldLight() {
        if (DirectionalLight != null)
            return;

        var worldLightGameObject = new GameObject("Light");
        worldLightGameObject.transform.SetParent(gameObject.transform);
        DirectionalLight = worldLightGameObject.GetOrAddComponent<Light>();
        SetupWorldLight();
    }

    private void SetupWorldLight() {
        if (LightInfo == null) {
            return;
        }

        DirectionalLight.type = LightType.Directional;
        DirectionalLight.shadows = LightShadows.Soft;
        DirectionalLight.shadowStrength = 0.6f;
        DirectionalLight.intensity = 1;

        var rotation = Quaternion.Euler(90 - LightInfo.longitude, LightInfo.latitude, 0);
        DirectionalLight.transform.rotation = rotation;

        if (LightInfo.ambient.Length > 0) {
            var ambient = new Color(LightInfo.ambient[0], LightInfo.ambient[1], LightInfo.ambient[2]);
            RenderSettings.ambientLight = ambient * LightInfo.intensity;
            RenderSettings.ambientIntensity = LightInfo.intensity;
        }

        if (LightInfo.diffuse.Length > 0) {
            var diffuse = new Color(LightInfo.diffuse[0], LightInfo.diffuse[1], LightInfo.diffuse[2]);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            DirectionalLight.color = diffuse;
        }
    }

    public void SetMapSize(int width, int height) {
        _size = new Vector2Int(width, height);
    }

    public void SetMapLightInfo(RSW.LightInfo lightInfo) {
        LightInfo = lightInfo;

        if (DirectionalLight == null) {
            InitWorldLight();
        }

        SetupWorldLight();
    }

    public void SetMapAltitude(Altitude altitude) {
        Altitude = altitude;
        PathFinder?.SetMap(Altitude);
    }

    public PathFinder GetPathFinder() {
        if (PathFinder == null) {
            InitPathFinder();
        }

        return PathFinder;
    }
}