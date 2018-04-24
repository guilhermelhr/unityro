
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
/// <summary>
/// Loaders for a ro map
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class MapLoader {
    private Queue<string> files = new Queue<string>();
    private static Hashtable cache = new Hashtable();

    private Action<string, object> callback;
    private int progress = 0;
    private int fileCount = 0;
    private int offset = 0;
    public Action<int> onProgress = null;

    public int Progress {
        get {
            return progress;
        }

        set {
            var progress = Math.Min(100, value);
            if(progress != this.progress && onProgress != null) {
                onProgress.Invoke(progress);
            }
            this.progress = progress;
        }
    }

    public static Hashtable Cache {
        get {
            return cache;
        }
    }

    public void Load(string mapname, Action<string, object> callback) {
        this.callback = callback;
        Progress = 0;
        offset = 0;
        fileCount = 0;
        files.Clear();
        cache.Clear();

        GC.Collect();

        // Load RSW
        string rswPath = "data/" + GetFilePath(mapname);
        RSW world = FileManager.Load(rswPath) as RSW;
        if(world == null) {
            throw new Exception("Could not load rsw for " + mapname);
        }
        Progress = 1;

        // Load GAT
        string gatPath = "data/" + GetFilePath(WorldLoader.files.gat);
        Altitude altitude = FileManager.Load(gatPath) as Altitude;
        if(altitude == null) {
            throw new Exception("Could not load gat for " + mapname);
        }
        callback.Invoke("MAP_ALTITUDE", altitude);
        Progress = 2;

        // Load GND
        string gndPath = "data/" + GetFilePath(WorldLoader.files.gnd);
        GND ground = FileManager.Load(gndPath) as GND;
        if(ground == null) {
            throw new Exception("Could not load gnd for " + mapname);
        }
        Progress = 3;

        var compiledGround = GroundLoader.Compile(ground, world.water.level, world.water.waveHeight);

        // Just to approximate, guess we have 2 textures for each models
        // To get a more linear loading
        fileCount = ground.textures.Length + world.models.Count * 3;
        // account for water
        if(compiledGround.waterVertCount > 0) {
            fileCount += 32;
        }

        LoadGroundTexture(world, compiledGround);

        callback.Invoke("MAP_WORLD", world);
        callback.Invoke("MAP_GROUND", compiledGround);

        LoadModels(world.models, ground);
    }

    private void LoadModels(List<RSW.Model> models, GND ground) {
        //queue list of models to load
        for(int i = 0; i < models.Count; i++) {
            var model = models[i];
            var path = model.filename = "data/model/" + model.filename;

            if(!files.Contains(path)) {
                files.Enqueue(path);
            }
        }

        //load models
        Start();

        //create model instances
        HashSet<RSM> objectsSet = new HashSet<RSM>();
        for(int i = 0; i < models.Count; i++) {
            RSM model = (RSM) cache[models[i].filename];
            if(model != null) {
                model.filename = models[i].filename;
                model.CreateInstance(models[i], ground.width, ground.height);
                objectsSet.Add(model);
            }
        }
        RSM[] objects = new RSM[objectsSet.Count];
        objectsSet.CopyTo(objects);
        CompileModels(objects);
    }

    private void CompileModels(RSM[] objects) {
        List<RSM.CompiledModel> models = new List<RSM.CompiledModel>();

        for(int i = 0; i < objects.Length; i++) {
            var _object = ModelLoader.Compile(objects[i]);
            var nodes = _object.meshes;

            for(int j = 0; j < nodes.Length; ++j) {
                var meshes = nodes[j];

                foreach(long index in meshes.Keys) {
                    models.Add(new RSM.CompiledModel() {
                        source = objects[i],
                        texture = "data/texture/" + _object.textures[index],
                        alpha = objects[i].alpha,
                        mesh = (float[]) meshes[index]
                    });
                }
            }


            progress = progress + (100 - progress) / objects.Length * (i + 1) / 2;
        }

        // load textures
        LoadModelsTextures(models);
    }

    private void LoadModelsTextures(List<RSM.CompiledModel> objects) {
        //enqueue texture
        for(int i = 0; i < objects.Count; i++) {
            //load texture
            var texture = objects[i].texture;
            files.Enqueue(texture);
        }

        //load textures
        Start();

        callback.Invoke("MAP_MODELS", objects.ToArray());
    }



    private void LoadGroundTexture(RSW world, GND.Mesh ground) {
        LinkedList<string> textures = new LinkedList<string>();

        //queue water textures
        if(ground.waterVertCount > 0) {
            var path = "data/texture/\xbf\xf6\xc5\xcd/water" + world.water.type;
            for(int i = 0; i < 32; i++) {
                string num = (i < 10) ? ("0" + i) : ("" + i);
                textures.AddLast(path + num + ".jpg");
                files.Enqueue(textures.Last.Value);
            }
        }

        //queue ground textures
        for(int i = 0; i < ground.textures.Length; i++) {
            textures.AddLast("data/texture/" + ground.textures[i]);
            files.Enqueue(textures.Last.Value);
        }

        Start();

        //splice water textures from ground textures
        List<string> waterTextures = new List<string>();
        if(ground.waterVertCount > 0) {
            for(int i = 0; i < 32; i++) {
                waterTextures.Add(textures.First.Value);
                textures.RemoveFirst();
            }
        }

        waterTextures.CopyTo(world.water.images);
        ground.textures = new string[textures.Count];
        textures.CopyTo(ground.textures, 0);
    }

    private string GetFilePath(string path) {
        if(DBManager.MapAlias.ContainsKey(path)){
            return Convert.ToString(DBManager.MapAlias[path]);
        }

        return path;
    }

    /// <summary>
    /// start to load files
    /// </summary>
    private void Start() {
        while(files.Count > 0) {
            Next();
            Progress = 3 + 97 / fileCount * (++offset);
        }
    }

    /// <summary>
    /// load next file
    /// </summary>
    private void Next() {
        string filename = files.Dequeue();

        if(string.IsNullOrEmpty(filename) || cache.Contains(filename)) {
            return;
        }

        object file = FileManager.Load(filename);
        cache.Add(filename, file);
    }
}
