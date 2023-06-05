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

    public static AudioMixerGroup SoundsMixerGroup;

    public Light WorldLight;

    private RSW world;
    private WaterBuilder water;
    private Models models;
    private Sounds sounds = new Sounds();
    private Sky sky;

    private bool worldCompleted, altitudeCompleted, groundCompleted, modelsCompleted;

    public bool Ready {
        get { return worldCompleted && altitudeCompleted && groundCompleted && modelsCompleted; }
    }

    public MapRenderer() { }

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

    public async Task<GameMap> RenderMap(AsyncMapLoader.GameMapData gameMapData, string mapName) {
        var gameMap = new GameObject(mapName).AddComponent<GameMap>();
        gameMap.tag = "Map";
        gameMap.SetMapLightInfo(gameMapData.World.light);
        gameMap.SetMapSize((int)gameMapData.Ground.width, (int)gameMapData.Ground.height);
        gameMap.SetMapAltitude(new Altitude(gameMapData.Altitude));

        var ground = new Ground(gameMapData.CompiledGround, gameMapData.World.water);

        for (int i = 0; i < gameMapData.World.sounds.Count; i++) {
            gameMapData.World.sounds[i].pos[0] += gameMap.Size.x;
            gameMapData.World.sounds[i].pos[1] *= -1;
            gameMapData.World.sounds[i].pos[2] += gameMap.Size.y;
            //world.sounds[i].pos[2] = tmp;
            gameMapData.World.sounds[i].range *= 0.3f;
            gameMapData.World.sounds[i].tick = 0;
        }

        await new Models(gameMapData.CompiledModels.ToList()).BuildMeshesAsync(null, true, gameMap.Size);

        InitializeSounds(gameMapData.World);
        MaybeInitSky(gameMap.transform, mapName);
        CreateLightPoints(gameMap.transform, gameMapData.World, gameMap.Size);

        return gameMap;
    }

    private void InitializeSounds(RSW world) {
        //add sounds to playlist (and cache)
        foreach (var sound in world.sounds) {
            sounds.Add(sound, null);
        }
    }

    private void MaybeInitSky(Transform parent, string mapname) {
        if (WeatherEffect.HasMap(mapname)) {
            //create sky
            var skyObject = new GameObject("_sky");
            skyObject.transform.SetParent(parent, false);
            sky = skyObject.AddComponent<Sky>();
            sky.Initialize(mapname);
        } else {
            //no weather effects, set sky color to blueish
            //Camera.main.backgroundColor = new Color(0.4f, 0.6f, 0.8f, 1.0f);
        }
    }

    private void CreateLightPoints(Transform parent, RSW world, Vector2Int mapSize) {
        //add lights
        GameObject lightsParent = new GameObject("_lights");
        lightsParent.transform.SetParent(parent, false);

        foreach (var light in world.lights) {
            var lightObj = new GameObject(light.name).AddComponent<Light>();
            Transform transform;
            (transform = lightObj.transform).SetParent(lightsParent.transform, false);
            lightObj.color = new Color(light.color[0], light.color[1], light.color[2]);
            lightObj.range = light.range / 5;
            lightObj.intensity = 5f;
            lightObj.shadows = LightShadows.Soft;
            var position = new Vector3(light.pos[0] + mapSize.x, -light.pos[1], light.pos[2] + mapSize.y);
            transform.position = position;
        }
    }

    public async Task<GameMap> OnMapComplete(AsyncMapLoader.GameMapData gameMap) {
        GameObject mapParent = GameObject.FindObjectOfType<GameMap>().gameObject;
        if (mapParent == null) {
            mapParent = new GameObject(gameMap.Name);
            mapParent.tag = "Map";
            mapParent.AddComponent<GameMap>();
        }

        var GameMapManager = mapParent.GetComponent<GameMap>();
        GameMapManager.SetMapLightInfo(gameMap.World.light);
        GameMapManager.SetMapSize((int)gameMap.Ground.width, (int)gameMap.Ground.height);
        GameMapManager.SetMapAltitude(new Altitude(gameMap.Altitude));

        FileCache.ClearAll();

        OnWorldComplete(gameMap.World);
        OnGroundComplete(gameMap.CompiledGround);
        OnAltitudeComplete(gameMap.Altitude);
        await OnModelsComplete(gameMap.CompiledModels, GameMapManager.Size);

        InitializeSounds(gameMap.World);
        MaybeInitSky(mapParent.transform, gameMap.Name);
        CreateLightPoints(mapParent.transform, gameMap.World, GameMapManager.Size);

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

    private async Task OnModelsComplete(RSM.CompiledModel[] compiledModels, Vector2Int mapSize) {
        models = new Models(compiledModels.ToList());
        await models.BuildMeshesAsync(OnMapLoadingProgress, false, mapSize);

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
        //if (mapParent != null) {
        //    //UnityEngine.Object.Destroy(mapParent);
        //    mapParent.gameObject.SetActive(false);
        //    mapParent = null;
        //}

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