using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
/// <summary>
/// Loaders for a ro map
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class MapLoader {

    private Action<string, string, object> callback;
    private int progress = 0;
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

    public void Load(string mapname, Action<string, string, object> callback) {
        this.callback = callback;
        Progress = 0;

        // Load RSW
        string rswPath = "data/" + GetFilePath(mapname);
        RSW world = FileManager.Load(rswPath) as RSW;
        if(world == null) {
            throw new Exception("Could not load rsw for " + mapname);
        }

        // Load GAT
        string gatPath = "data/" + GetFilePath(WorldLoader.files.gat);
        Altitude altitude = FileManager.Load(gatPath) as Altitude;
        if(altitude == null) {
            throw new Exception("Could not load gat for " + mapname);
        }
        callback.Invoke(mapname, "MAP_ALTITUDE", altitude);

        // Load GND
        string gndPath = "data/" + GetFilePath(WorldLoader.files.gnd);
        GND ground = FileManager.Load(gndPath) as GND;
        if(ground == null) {
            throw new Exception("Could not load gnd for " + mapname);
        }

        var compiledGround = GroundLoader.Compile(ground, world.water.level, world.water.waveHeight);

        // Just to approximate, guess we have 2 textures for each models
        // To get a more linear loading
        //fileCount = ground.textures.Length + world.models.Count * 3;
        // account for water
        //if(compiledGround.waterVertCount > 0) {
        //    fileCount += 32;
        //}

        LoadGroundTexture(world, compiledGround);

        callback.Invoke(mapname, "MAP_WORLD", world);
        callback.Invoke(mapname, "MAP_GROUND", compiledGround);

        var compiledModels = LoadModels(world.models, ground);

        callback.Invoke(mapname, "MAP_MODELS", compiledModels.ToArray());
    }

    private List<RSM.CompiledModel> LoadModels(List<RSW.Model> models, GND ground) {
        FileManager.InitBatch();

        //queue list of models to load
        for(int i = 0; i < models.Count; i++) {
            var model = models[i];
            var path = model.filename = "data/model/" + model.filename;

            FileManager.Load(path);
        }

        //load models
        FileManager.EndBatch();

        //create model instances
        HashSet<RSM> objectsSet = new HashSet<RSM>();
        for(int i = 0; i < models.Count; i++) {
            RSM model = (RSM) FileManager.Load(models[i].filename);
            if(model != null) {
                model.filename = models[i].filename;
                model.CreateInstance(models[i], ground.width, ground.height);
                objectsSet.Add(model);
            }
        }
        FileCache.ClearAllWithExt("rsm");
        RSM[] objects = new RSM[objectsSet.Count];
        objectsSet.CopyTo(objects);

        var compiledModels = CompileModels(objects);
        LoadModelsTextures(compiledModels);
        return compiledModels;
    }

    private void LoadModelsTextures(List<RSM.CompiledModel> objects) {
        FileManager.InitBatch();

        //enqueue texture
        for(int i = 0; i < objects.Count; i++) {
            //load texture
            var texture = objects[i].texture;
            FileManager.Load(texture);
        }

        //load textures
        FileManager.EndBatch();
    }



    private void LoadGroundTexture(RSW world, GND.Mesh ground) {
        LinkedList<string> textures = new LinkedList<string>();

        FileManager.InitBatch();

        //queue water textures
        if(ground.waterVertCount > 0) {
            var path = "data/texture/\xbf\xf6\xc5\xcd/water" + world.water.type;
            for(int i = 0; i < 32; i++) {
                string num = (i < 10) ? ("0" + i) : ("" + i);
                textures.AddLast(path + num + ".jpg");
                FileManager.Load(textures.Last.Value);
            }
        }

        //queue ground textures
        for(int i = 0; i < ground.textures.Length; i++) {
            textures.AddLast("data/texture/" + ground.textures[i]);
            FileManager.Load(textures.Last.Value);
        }

        FileManager.EndBatch();

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

    private class ModelCompiler
    {
        private RSM _obj;
        private RSM.Model compiledModel;

        public RSM.Model CompiledModel { get { return compiledModel; } }
        public RSM Source { get { return _obj; } }

        public ModelCompiler(RSM obj) {
            _obj = obj;
        }

        public void ThreadPoolCallback(object threadContext) {
            try {
                compiledModel = ModelLoader.Compile(_obj);
            } finally {
                if(Interlocked.Decrement(ref pendingCMThreads) == 0) {
                    doneCMEvent.Set();
                }
            }
        }
    }

    private static int pendingCMThreads;
    private static ManualResetEvent doneCMEvent;
    private List<RSM.CompiledModel> CompileModels(RSM[] objects) {
        List<RSM.CompiledModel> models = new List<RSM.CompiledModel>();

        pendingCMThreads = objects.Length;
        doneCMEvent = new ManualResetEvent(false);

        float start = Time.realtimeSinceStartup;
        ModelCompiler[] compilerArray = new ModelCompiler[objects.Length];
        for(int i = 0; i < objects.Length; i++) {
            ModelCompiler compiler = new ModelCompiler(objects[i]);
            compilerArray[i] = compiler;
            ThreadPool.QueueUserWorkItem(compiler.ThreadPoolCallback, i);
        }

        doneCMEvent.WaitOne();
        float delta = Time.realtimeSinceStartup - start;
        Debug.Log("Models compiling time: " + delta);

        start = Time.realtimeSinceStartup;
        for(int i = 0; i < objects.Length; i++) {
            ModelCompiler compiler = compilerArray[i];
            var _object = compiler.CompiledModel;
            var nodes = _object.meshes;

            for(int j = 0; j < nodes.Length; ++j) {
                var meshes = nodes[j];

                foreach(long index in meshes.Keys) {
                    models.Add(new RSM.CompiledModel() {
                        source = compiler.Source,
                        texture = "data/texture/" + _object.textures[index],
                        alpha = compiler.Source.alpha,
                        mesh = (float[]) meshes[index]
                    });
                }
            }
        }
        delta = Time.realtimeSinceStartup - start;
        Debug.Log("Models gathering time: " + delta);

        return models;
    }
}
