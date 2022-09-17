using ROIO.Models.FileTypes;
using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ROIO.Loaders {

    public class AsyncMapLoader {

        public struct GameMapData {
            public string Name;
            public RSW World;
            public GND Ground;
            public GAT Altitude;

            public RSM[] Models;
            public RSM.CompiledModel[] CompiledModels;
            public GND.Mesh CompiledGround;
        }

        public async Task<GameMapData> Load(string mapname) {
            RSW world = await LoadWorld(mapname);
            GND ground = await LoadGround(mapname);
            GAT altitude = await LoadAltitude(mapname);
            
            GND.Mesh compiledGround = await CompileGroundMesh(ground, world);
            RSM[] models = LoadModels(world.modelDescriptors);
            RSM.CompiledModel[] compiledModels = await CompileModels(models);

            return new GameMapData {
                Name = mapname,
                World = world,
                Ground = ground,
                Altitude = altitude,
                Models = models,
                CompiledModels = compiledModels,
                CompiledGround = compiledGround
            };
        }

        private async Task<RSW> LoadWorld(string mapname) {
            string rswPath = "data/" + GetFilePath(mapname);
            RSW world = await Task.Run(() => FileManager.Load(rswPath) as RSW);
            if (world == null) {
                throw new Exception("Could not load rsw for " + mapname);
            }

            return world;
        }

        private async Task<GAT> LoadAltitude(string mapname) {
            string gatPath = "data/" + GetFilePath(WorldLoader.files.gat);
            GAT altitude = await Task.Run(() => FileManager.Load(gatPath) as GAT);
            if (altitude == null) {
                throw new Exception("Could not load gat for " + mapname);
            }

            return altitude;
        }

        private async Task<GND> LoadGround(string mapname) {
            string gndPath = "data/" + GetFilePath(WorldLoader.files.gnd);
            GND ground = await Task.Run(() => FileManager.Load(gndPath) as GND);
            if (ground == null) {
                throw new Exception("Could not load gnd for " + mapname);
            }

            return ground;
        }

        private async Task<GND.Mesh> CompileGroundMesh(GND ground, RSW world) {
            GND.Mesh compiledGround = await Task.Run(() => GroundLoader.Compile(ground, world.water.level, world.water.waveHeight));
            
            //TODO this might not be necessary anymore
            await CacheGroundTextures(world, compiledGround);

            return compiledGround;
        }

        private RSM[] LoadModels(List<RSW.ModelDescriptor> modelDescriptors) {
            HashSet<RSM> objectsSet = new HashSet<RSM>();

            foreach (var descriptor in modelDescriptors) {
                RSM model = FileManager.Load("data/model/" + descriptor.filename) as RSM;
                if (model != null) {
                    model.CreateInstance(descriptor);
                    model.filename = descriptor.filename;
                    lock (objectsSet) {
                        objectsSet.Add(model);
                    }
                }
            }

            FileCache.ClearAllWithExt("rsm");
            RSM[] models = new RSM[objectsSet.Count];
            objectsSet.CopyTo(models);

            return models;
        }

        private async Task<RSM.CompiledModel[]> CompileModels(RSM[] objects) {
            List<Task<RSM.CompiledModel>> tasks = new List<Task<RSM.CompiledModel>>();

            foreach (var model in objects) {
                var t = Task.Run(() => ModelLoader.Compile(model));
                tasks.Add(t);
            }

            return await Task.WhenAll(tasks);
        }

        private async Task CacheGroundTextures(RSW world, GND.Mesh ground) {
            LinkedList<string> textures = new LinkedList<string>();

            FileManager.InitBatch();

            //queue water textures
            if (ground.waterVertCount > 0) {
                var path = "data/texture/\xbf\xf6\xc5\xcd/water" + world.water.type;
                for (int i = 0; i < 32; i++) {
                    string num = i < 10 ? "0" + i : "" + i;
                    textures.AddLast(path + num + ".jpg");
                    FileManager.Load(textures.Last.Value);
                }
            }

            //queue ground textures
            for (int i = 0; i < ground.textures.Length; i++) {
                textures.AddLast("data/texture/" + ground.textures[i]);
                FileManager.Load(textures.Last.Value);
            }

            await Task.Run(() => FileManager.EndBatch());

            //splice water textures from ground textures
            List<string> waterTextures = new List<string>();
            if (ground.waterVertCount > 0) {
                for (int i = 0; i < 32; i++) {
                    waterTextures.Add(textures.First.Value);
                    textures.RemoveFirst();
                }
            }

            waterTextures.CopyTo(world.water.images);
            ground.textures = new string[textures.Count];
            textures.CopyTo(ground.textures, 0);
        }

        private string GetFilePath(string path) {
            if (Tables.ResNameTable.ContainsKey(path)) {
                return Convert.ToString(Tables.ResNameTable[path]);
            }

            return path;
        }
    }


    /// <summary>
    /// Loaders for a ro map
    /// 
    /// @author Guilherme Hernandez
    /// Based on ROBrowser by Vincent Thibault (robrowser.com)
    /// </summary>
    public class MapLoader {

        public async Task Load(string mapname, Action<string, string, object> callback) {
            await LoadWorld(mapname, callback);
        }

        private async Task LoadWorld(string mapname, Action<string, string, object> callback) {
            string rswPath = "data/" + GetFilePath(mapname);
            RSW world = await Task.Run(() => FileManager.Load(rswPath) as RSW);
            if (world == null) {
                throw new Exception("Could not load rsw for " + mapname);
            }
            callback.Invoke(mapname, "MAP_WORLD", world);

            LoadAltitude(mapname, callback);
            await LoadGround(mapname, world, callback);

        }

        private async void LoadAltitude(string mapname, Action<string, string, object> callback) {
            string gatPath = "data/" + GetFilePath(WorldLoader.files.gat);
            GAT altitude = await Task.Run(() => FileManager.Load(gatPath) as GAT);
            if (altitude == null) {
                throw new Exception("Could not load gat for " + mapname);
            }

            callback.Invoke(mapname, "MAP_ALTITUDE", altitude);
        }

        private async Task LoadGround(string mapname, RSW world, Action<string, string, object> callback) {
            string gndPath = "data/" + GetFilePath(WorldLoader.files.gnd);
            GND ground = await Task.Run(() => FileManager.Load(gndPath) as GND);
            if (ground == null) {
                throw new Exception("Could not load gnd for " + mapname);
            }
            callback.Invoke(mapname, "MAP_GROUND_SIZE", new UnityEngine.Vector2(ground.width, ground.height));

            LoadModels(mapname, world.modelDescriptors, callback);
            GND.Mesh compiledGround = await CompileGroundMesh(mapname, ground, world, callback);
            callback.Invoke(mapname, "MAP_GROUND", compiledGround);
        }

        private async Task<GND.Mesh> CompileGroundMesh(string mapname, GND ground, RSW world, Action<string, string, object> callback) {
            GND.Mesh compiledGround = await Task.Run(() => GroundLoader.Compile(ground, world.water.level, world.water.waveHeight));
            await LoadGroundTexture(world, compiledGround);

            return compiledGround;
        }

        private void LoadModels(string mapname, List<RSW.ModelDescriptor> modelDescriptors, Action<string, string, object> callback) {
            HashSet<RSM> objectsSet = new HashSet<RSM>();

            for (int i = 0; i < modelDescriptors.Count; i++) {
                RSM model = FileManager.Load("data/model/" + modelDescriptors[i].filename) as RSM;
                if (model != null) {
                    model.CreateInstance(modelDescriptors[i]);
                    model.filename = modelDescriptors[i].filename;
                    objectsSet.Add(model);
                }
            }

            FileCache.ClearAllWithExt("rsm");
            RSM[] models = new RSM[objectsSet.Count];
            objectsSet.CopyTo(models);

            CompileModels(mapname, models, callback);
        }

        private async void CompileModels(string mapname, RSM[] objects, Action<string, string, object> callback) {
            List<Task<RSM.CompiledModel>> tasks = new List<Task<RSM.CompiledModel>>();

            foreach (var model in objects) {
                var t = Task.Run(() => ModelLoader.Compile(model));
                tasks.Add(t);
            }

            RSM.CompiledModel[] compiledModels = await Task.WhenAll(tasks);
            callback.Invoke(mapname, "MAP_MODELS", compiledModels);
        }

        private async Task LoadGroundTexture(RSW world, GND.Mesh ground) {
            LinkedList<string> textures = new LinkedList<string>();

            FileManager.InitBatch();

            //queue water textures
            if (ground.waterVertCount > 0) {
                var path = "data/texture/\xbf\xf6\xc5\xcd/water" + world.water.type;
                for (int i = 0; i < 32; i++) {
                    string num = i < 10 ? "0" + i : "" + i;
                    textures.AddLast(path + num + ".jpg");
                    FileManager.Load(textures.Last.Value);
                }
            }

            //queue ground textures
            for (int i = 0; i < ground.textures.Length; i++) {
                textures.AddLast("data/texture/" + ground.textures[i]);
                FileManager.Load(textures.Last.Value);
            }

            await Task.Run(() => FileManager.EndBatch());

            //splice water textures from ground textures
            List<string> waterTextures = new List<string>();
            if (ground.waterVertCount > 0) {
                for (int i = 0; i < 32; i++) {
                    waterTextures.Add(textures.First.Value);
                    textures.RemoveFirst();
                }
            }

            waterTextures.CopyTo(world.water.images);
            ground.textures = new string[textures.Count];
            textures.CopyTo(ground.textures, 0);
        }

        private string GetFilePath(string path) {
            if (Tables.ResNameTable.ContainsKey(path)) {
                return Convert.ToString(Tables.ResNameTable[path]);
            }

            return path;
        }
    }
}