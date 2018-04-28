using System;
using System.Text.RegularExpressions;
using UnityEngine;
using static AltitudeLoader;

/// <summary>
/// Rendering of the map
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class MapRenderer {
    public static int MAX_VERTICES = 65532;

    private RSW world;
    private Water water;
    private Models models;

    private bool worldCompleted, altitudeCompleted, groundCompleted, modelsCompleted;

    public bool Ready {
        get { return worldCompleted && altitudeCompleted && groundCompleted && modelsCompleted;  }
    }
    
    public static GameObject mapParent;

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
            //everything needed was loaded, no need to keep data cached
            FileCache.Report();
            FileCache.ClearAll();
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

        groundCompleted = true;
    }

    private void OnModelsComplete(RSM.CompiledModel[] compiledModels) {
        models = new Models(compiledModels);
        models.BuildMeshes();
        models.Render();

        modelsCompleted = true;
    }

    public void Render() {
        if(water != null) {
            water.Render();
        }
    }

    public void Clear() {
        //destroy map
        if(mapParent != null) {
            UnityEngine.Object.DestroyImmediate(mapParent);
        }

        //destroy textures
        var ob = UnityEngine.Object.FindObjectsOfType(typeof(Texture2D));
        int dCount = 0;
        foreach(Texture2D t in ob) {
            if(string.Compare(t.name, "maptexture") == 0) {
                dCount++;
                UnityEngine.Object.Destroy(t);
            }
        }
        Debug.Log(dCount + " textures destroyed");

        world = null;
        water = null;
        models = null;

        worldCompleted = altitudeCompleted = groundCompleted = modelsCompleted = false;
    }
}
