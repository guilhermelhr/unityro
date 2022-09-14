using Assets.Scripts.Renderer.Map;
using ROIO;
using ROIO.Loaders;
using ROIO.Models.FileTypes;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Rendering of the map
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class MapRenderer {

    internal static Action<float> OnProgress;

    public static int MAX_VERTICES = 65532;
    public static AudioMixerGroup SoundsMixerGroup;

    public static GameObject mapParent;
    public Light WorldLight;
    public static uint width, height;

    private RSW world;
    private WaterBuilder water;
    private Models models;
    private Sounds sounds = new Sounds();
    private Sky sky;

    private bool worldCompleted, altitudeCompleted, groundCompleted, modelsCompleted;

    public bool Ready {
        get { return worldCompleted && altitudeCompleted && groundCompleted && modelsCompleted; }
    }

    public MapRenderer(AudioMixerGroup audioMixerGroup, Light worldLight) {
        SoundsMixerGroup = audioMixerGroup;
        WorldLight = worldLight;
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

    private void InitializeSounds() {
        //add sounds to playlist (and cache)
        foreach (var sound in world.sounds) {
            sounds.Add(sound, null);
        }
    }

    private void MaybeInitSky(string mapname) {
        if (WeatherEffect.HasMap(mapname)) {
            //create sky
            var skyObject = new GameObject("_sky");
            skyObject.transform.SetParent(mapParent.transform);
            sky = skyObject.AddComponent<Sky>();
            sky.Initialize(mapname);
        } else {
            //no weather effects, set sky color to blueish
            //Camera.main.backgroundColor = new Color(0.4f, 0.6f, 0.8f, 1.0f);
        }
    }

    private void CreateLightPoints() {
        //add lights
        GameObject lightsParent = new GameObject("_lights");
        lightsParent.transform.parent = mapParent.transform;

        foreach (var light in world.lights) {
            var lightObj = new GameObject(light.name);
            var lightContainer = lightObj.AddComponent<LightContainer>();
            lightObj.transform.SetParent(lightsParent.transform);
            lightContainer.SetLightProps(light, height, width);
        }
    }

    public async Task<GameMap> OnMapComplete(AsyncMapLoader.GameMap gameMap) {
        if (mapParent == null) {
            mapParent = new GameObject(gameMap.Name);
            mapParent.tag = "Map";
            mapParent.AddComponent<GameMap>();
        }
        var GameMapManager = mapParent.GetComponent<GameMap>();
        GameMapManager.SetMapLightInfo(gameMap.World.light);
        GameMapManager.SetMapSize((int) gameMap.Ground.width, (int) gameMap.Ground.height);
        GameMapManager.SetMapAltitude(new Altitude(gameMap.Altitude));

        FileCache.ClearAll();

        OnWorldComplete(gameMap.World);
        OnGroundComplete(gameMap.CompiledGround);
        OnAltitudeComplete(gameMap.Altitude);
        await OnModelsComplete(gameMap.CompiledModels);

        InitializeSounds();
        MaybeInitSky(gameMap.Name);
        CreateLightPoints();

        return GameMapManager;
    }

    /// <summary>
    /// receive parsed world
    /// </summary>
    /// <param name="world"></param>
    private void OnWorldComplete(RSW world) {
        this.world = world;
        worldCompleted = true;
    }

    /// <summary>
    /// receive parsed gat
    /// </summary>
    /// <param name="gat"></param>
    private void OnAltitudeComplete(GAT gat) {
        altitudeCompleted = true;
    }

    private void OnGroundComplete(GND.Mesh mesh) {
        width = mesh.width;
        height = mesh.height;

        Ground ground = new Ground();
        ground.BuildMesh(mesh);
        if (ground.meshes.Length > 0) {
            ground.InitTextures(mesh);
            ground.Render();
        }

        if (mesh.waterVertCount > 0) {
            water = new WaterBuilder();
            water.InitTextures(mesh, world.water);
            water.BuildMesh(mesh);
        }

        //initialize sounds
        for (int i = 0; i < world.sounds.Count; i++) {
            world.sounds[i].pos[0] += mesh.width;
            world.sounds[i].pos[1] *= -1;
            world.sounds[i].pos[2] += mesh.height;
            //world.sounds[i].pos[2] = tmp;
            world.sounds[i].range *= 0.3f;
            world.sounds[i].tick = 0;
        }

        groundCompleted = true;
    }

    private async Task OnModelsComplete(RSM.CompiledModel[] compiledModels) {
        models = new Models(compiledModels.ToList());
        await models.BuildMeshesAsync(OnMapLoadingProgress);

        modelsCompleted = true;
    }

    private void OnMapLoadingProgress(float progress) {
        OnProgress?.Invoke(progress);
    }

    public void PostRender() {
        if (water != null) {
            //water.Render();
        }
    }

    public void FixedUpdate() {
        sounds.Update();
    }

    public void Clear() {
        sounds.Clear();

        world = null;
        water = null;
        models = null;
        sky = null;

        //destroy map
        if (mapParent != null) {
            //UnityEngine.Object.Destroy(mapParent);
            mapParent.gameObject.SetActive(false);
            mapParent = null;
        }

        //destroy textures
        var ob = UnityEngine.Object.FindObjectsOfType(typeof(Texture2D));
        int dCount = 0;
        foreach (Texture2D t in ob) {
            if (t.name.StartsWith("maptexture@")) {
                dCount++;
                UnityEngine.Object.Destroy(t);
            }
        }

        worldCompleted = altitudeCompleted = groundCompleted = modelsCompleted = false;
    }
}
