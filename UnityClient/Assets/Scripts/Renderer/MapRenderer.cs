using ROIO;
using ROIO.Models.FileTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Rendering of the map
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class MapRenderer {

    public Action<GameObject> OnMapLoaded = null;

    public static int MAX_VERTICES = 65532;
    public static AudioMixerGroup SoundsMixerGroup;

    public static GameObject mapParent;
    public Light WorldLight;
    public static uint width, height;

    private Altitude altitude;
    private RSW world;
    private Water water;
    private Models models;
    private Sounds sounds = new Sounds();
    private Sky sky;

    private bool worldCompleted, altitudeCompleted, groundCompleted, modelsCompleted;

    private GameManager GameManager;
    private PathFinder PathFinder;

    public bool Ready {
        get { return worldCompleted && altitudeCompleted && groundCompleted && modelsCompleted;  }
    }

    // TODO this PathFinder here probably can be somewhere else
    public MapRenderer(GameManager gameManager, PathFinder pathFinder, AudioMixerGroup audioMixerGroup, Light worldLight) {
        SoundsMixerGroup = audioMixerGroup;
        WorldLight = worldLight;
        GameManager = gameManager;
        PathFinder = pathFinder;
    }

    /*public class Fog {
        public Fog(bool use) { this.use = use; }
        //bool use = MapPreferences.useFog; TODO
        bool use;
        bool exist = true;
        int far = 30;
        int near = 180;
        float factor = 1.0f;
        float[] color = new float[]{1, 1, 1};
    }

    public Fog fog = new Fog(false);*/

    public void OnComplete(string mapname, string id, object data) {
        if(mapParent == null) {
            mapParent = new GameObject(mapname);
            mapParent.tag = "Map";
        }

        float start = Time.realtimeSinceStartup;
        switch(id) {
            case "MAP_WORLD":
                OnWorldComplete(data as RSW);
                break;
            case "MAP_ALTITUDE":
                OnAltitudeComplete(data as GAT);
                break;
            case "MAP_GROUND":
                OnGroundComplete(data as GND.Mesh);
                break;
            case "MAP_MODELS":
                OnModelsComplete(data as RSM.CompiledModel[]);
                break;
        }
        float delta = Time.realtimeSinceStartup - start;
        Debug.Log(id + " oncomplete time: " + delta);

        if(Ready) {
            OnMapComplete(mapname);
        }
    }

    private void OnMapComplete(string mapname) {
        //everything needed was loaded, no need to keep the current cache
        //FileCache.Report();
        FileCache.ClearAll();

        //add sounds to playlist (and cache)
        foreach(var sound in world.sounds) {
            sounds.Add(sound, null);
        }
        Debug.Log(sounds.Count() + " sounds loaded");


        if(WeatherEffect.HasMap(mapname)) {
            //create sky
            sky = new Sky();
            //initialize clouds
            sky.Initialize(mapname);
        } else {
            //no weather effects, set sky color to blueish
            //Camera.main.backgroundColor = new Color(0.4f, 0.6f, 0.8f, 1.0f);
        }

        //add lights
        GameObject lightsParent = new GameObject("_lights");
        lightsParent.transform.parent = mapParent.transform;
        
        foreach(var light in world.lights) {
            var lightObj = new GameObject(light.name);
            lightObj.transform.SetParent(lightsParent.transform);
            Light lightComponent = lightObj.AddComponent<Light>();
#if UNITY_EDITOR
            lightComponent.lightmapBakeType = LightmapBakeType.Baked;
#endif

            lightComponent.color = new Color(light.color[0], light.color[1], light.color[2]);
            lightComponent.range = light.range;
            Vector3 position = new Vector3(light.pos[0] + width, -light.pos[1], light.pos[2] + height);
            lightObj.transform.position = position;
        }
    }

    /// <summary>
    /// receive parsed world
    /// </summary>
    /// <param name="world"></param>
    private void OnWorldComplete(RSW world) {
        this.world = world;

        //calculate light direction
        RSW.LightInfo lightInfo = world.light;
        lightInfo.direction = new Vector3();

        Vector3 lightRotation = new Vector3(lightInfo.longitude, lightInfo.latitude, 0);
        WorldLight.transform.rotation = Quaternion.identity;
        WorldLight.transform.Rotate(lightRotation);

        Color ambient = new Color(lightInfo.ambient[0], lightInfo.ambient[1], lightInfo.ambient[2]);
        Color diffuse = new Color(lightInfo.diffuse[0], lightInfo.diffuse[1], lightInfo.diffuse[2]);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = ambient * lightInfo.intensity;

        Debug.Log("diffuse: " + diffuse);
        WorldLight.color = diffuse;

        worldCompleted = true;
    }

    /// <summary>
    /// receive parsed gat
    /// </summary>
    /// <param name="gat"></param>
    private void OnAltitudeComplete(GAT gat) {
        altitudeCompleted = true;
        altitude = new Altitude(gat, PathFinder);
    }

    private void OnGroundComplete(GND.Mesh mesh) {
        width = mesh.width;
        height = mesh.height;

        Ground ground = new Ground();
        ground.BuildMesh(mesh);
        ground.InitTextures(mesh);
        ground.Render();

        if(mesh.waterVertCount > 0) {
            water = new Water();
            water.InitTextures(mesh, world.water);
            water.BuildMesh(mesh);
        }

        //initialize sounds
        for(int i = 0; i < world.sounds.Count; i++) {
            world.sounds[i].pos[0] += mesh.width;
            world.sounds[i].pos[1] *= -1;
            world.sounds[i].pos[2] += mesh.height;
            //world.sounds[i].pos[2] = tmp;
            world.sounds[i].range *= 0.3f;
            world.sounds[i].tick = 0;
        }

        groundCompleted = true;
    }

    private void OnModelsComplete(RSM.CompiledModel[] compiledModels) {
        models = new Models(compiledModels.ToList());
        models.BuildMeshes();

        modelsCompleted = true;
    }

    public void PostRender() {
        if(water != null) {
            water.Render();
        }
    }

    public void Render() {
        if(sky != null) {
            sky.Render();
        }
        
        models.Render();
    }

    public void FixedUpdate() {
        sounds.Update();
        if(sky != null) {
            sky.FixedUpdate();
        }
    }

    public void Clear() {
        sounds.Clear();
        if(sky != null) {
            sky.Clear();
        }

        world = null;
        water = null;
        models = null;
        sky = null;

        //destroy map
        if(mapParent != null) {
            UnityEngine.Object.Destroy(mapParent);
            mapParent = null;
        }

        //destroy textures
        var ob = UnityEngine.Object.FindObjectsOfType(typeof(Texture2D));
        int dCount = 0;
        foreach(Texture2D t in ob) {
            if(t.name.StartsWith("maptexture@")) {
                dCount++;
                UnityEngine.Object.Destroy(t);
            }
        }
        Debug.Log(dCount + " textures destroyed");

        worldCompleted = altitudeCompleted = groundCompleted = modelsCompleted = false;
    }
}
