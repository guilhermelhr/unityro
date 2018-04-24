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

    private Regex mapfix1 = new Regex(@"^(\d{3})(\d@)");
    private Regex mapfix2 = new Regex(@"^\d{3}#");
    private Regex rsw = new Regex(@"\.gat$", RegexOptions.IgnoreCase);

    public string currentMap;
    public static GameObject mapParent;
    private RSW world;
    private Water water;
    private Models modelsRenderer;
    public bool loading = false;
    private MapLoader mapLoader;

    public class Fog {
        public Fog(bool use) { this.use = use; }
        //bool use = MapPreferences.useFog; TODO
        bool use;
        bool exist = true;
        int far = 30;
        int near = 180;
        float factor = 1.0f;
        float[] color = new float[]{1, 1, 1};
    }

    public Fog fog = new Fog(false);

    public void SetMap(string mapname) {
        //(TODO from robrowser) stop the map loading and start to load the new map
        if(loading) return;
                
        mapname = mapfix1.Replace(mapname, @"$2");
        mapname = mapfix2.Replace(mapname, "");

        // Clean objects TODO
        // SoundManager.stop();
        // Renderer.stop();
        // UIManager.removeComponents();
        // Cursor.setType(Cursor.ACTION.DEFAULT);

        // Don't reload a map when it's just a local teleportation
        if(!string.Equals(currentMap, mapname)) {
            loading = true;

            if(mapParent != null) {
                GameObject.Destroy(mapParent);
                water = null;
                world = null;
                modelsRenderer = null;
            }

            mapParent = new GameObject(mapname);

            //BGM.stop();
            currentMap = mapname;

            //parse the filename
            string filename = rsw.Replace(mapname, ".rsw");

            //TODO load screen
            mapLoader = new MapLoader();
            mapLoader.Load(filename, OnComplete);
            return;
        }
    }

    public void OnComplete(string id, object data) {
        switch(id) {
            case "MAP_WORLD":
                OnWorldComplete(data as RSW);
                break;
            case "MAP_ALTITUDE":
                OnAltitudeComplete(data as Altitude);
                break;
            case "MAP_GROUND":
                OnGroundComplete((GND.Mesh) data);
                break;
            case "MAP_MODELS":
                OnModelsComplete(data as RSM.CompiledModel[]);
                loading = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    private void OnModelsComplete(RSM.CompiledModel[] models) {
        modelsRenderer = new Models(models);
        modelsRenderer.BuildMeshes();
        modelsRenderer.Render();
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

        world.light.direction[0] = (float) (-Math.Cos(longitude) * Math.Sin(latitude));
        world.light.direction[1] = (float) (-Math.Cos(latitude));
        world.light.direction[2] = (float) (-Math.Sin(longitude) * Math.Sin(latitude));
    }

    /// <summary>
    /// receive parsed gat
    /// </summary>
    /// <param name="altitude"></param>
    private void OnAltitudeComplete(Altitude altitude) {

        //TODO
        //var gl = Renderer.getContext();
        //GridSelector.init(gl);
    }

    public void Render() {
        if(water != null && !loading) { 
            water.Render();
        }
    }
}
