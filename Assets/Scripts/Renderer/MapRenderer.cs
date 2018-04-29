using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Audio;
using static AltitudeLoader;

/// <summary>
/// Rendering of the map
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class MapRenderer {
    public static int MAX_VERTICES = 65532;

    public static GameObject mapParent;
    public static AudioMixerGroup SoundsMixerGroup;

    private RSW world;
    private Water water;
    private Models models;
    private Sounds sounds = new Sounds();

    private bool worldCompleted, altitudeCompleted, groundCompleted, modelsCompleted;

    public bool Ready {
        get { return worldCompleted && altitudeCompleted && groundCompleted && modelsCompleted;  }
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
        }

        float start = Time.realtimeSinceStartup;
        switch(id) {
            case "MAP_WORLD":
                OnWorldComplete(data as RSW);
                break;
            case "MAP_ALTITUDE":
                OnAltitudeComplete(data as Altitude);
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
            //everything needed was loaded, no need to keep the current cache
            FileCache.Report();
            FileCache.ClearAll();

            //add sounds to playlist (and cache)
            foreach(var sound in world.sounds) {
                sounds.Add(sound, null);
            }
        }
    }

    /// <summary>
    /// receive parsed world
    /// </summary>
    /// <param name="world"></param>
    private void OnWorldComplete(RSW world) {
        this.world = world;

        //calculate light direction
        world.light.direction = new Vector3();
        var longitude = world.light.longitude * Math.PI / 180;
        var latitude = world.light.latitude * Math.PI / 180;


        //TODO translate unity light to this
        world.light.direction[0] = (float) (-Math.Cos(longitude) * Math.Sin(latitude));
        world.light.direction[1] = (float) (-Math.Cos(latitude));
        world.light.direction[2] = (float) (-Math.Sin(longitude) * Math.Sin(latitude));

        worldCompleted = true;
    }

    /// <summary>
    /// receive parsed gat
    /// </summary>
    /// <param name="altitude"></param>
    private void OnAltitudeComplete(Altitude altitude) {

        //TODO
        //var gl = Renderer.getContext();
        //GridSelector.init(gl);

        altitudeCompleted = true;
    }

    private void OnGroundComplete(GND.Mesh mesh) {
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
        models = new Models(compiledModels);
        models.BuildMeshes();
        models.Render();

        modelsCompleted = true;
    }

    public void PostRender() {
        if(water != null) {
            water.Render();
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
