using ROIO.Models.FileTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ROIO.Loaders {
    /// <summary>
    /// Loaders for a ro map
    /// 
    /// @author Guilherme Hernandez
    /// Based on ROBrowser by Vincent Thibault (robrowser.com)
    /// </summary>
    public class MapLoader {

        public async Task Load(string mapname, Action<string, string, object> callback) {
            await LoadWorld(mapname, callback);
            await LoadAltitude(mapname, callback);
        }

        private async Task LoadWorld(string mapname, Action<string, string, object> callback) {
            string rswPath = "data/" + GetFilePath(mapname);
            RSW world = await Task.Run(() => FileManager.Load(rswPath) as RSW);
            if (world == null) {
                throw new Exception("Could not load rsw for " + mapname);
            }

            callback.Invoke(mapname, "MAP_WORLD", world);

            Task ground = LoadGround(mapname, world, callback);
            Task models = LoadModels(mapname, world.modelDescriptors, callback);
            await Task.WhenAll(ground, models);
        }

        private async Task LoadAltitude(string mapname, Action<string, string, object> callback) {
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

            GND.Mesh compiledGround = await CompileGroundMesh(ground, world);
            callback.Invoke(mapname, "MAP_GROUND", compiledGround);
        }

        private async Task<GND.Mesh> CompileGroundMesh(GND ground, RSW world) {
            GND.Mesh compiledGround = await Task.Run(() => GroundLoader.Compile(ground, world.water.level, world.water.waveHeight));
            await LoadGroundTexture(world, compiledGround);
            return compiledGround;
        }

        private async Task LoadModels(string mapname, List<RSW.ModelDescriptor> modelDescriptors, Action<string, string, object> callback) {
            HashSet<RSM> objectsSet = new HashSet<RSM>();

            for (int i = 0; i < modelDescriptors.Count; i++) {
                await Task.Run(() => {
                    RSM model = FileManager.Load("data/model/" + modelDescriptors[i].filename) as RSM;
                    if (model != null) {
                        model.CreateInstance(modelDescriptors[i]);
                        objectsSet.Add(model);
                    }
                });
            }

            FileCache.ClearAllWithExt("rsm");
            RSM[] models = new RSM[objectsSet.Count];
            objectsSet.CopyTo(models);

            await CompileModels(mapname, models, callback);
        }

        private async Task CompileModels(string mapname, RSM[] objects, Action<string, string, object> callback) {
            List<Task<RSM.CompiledModel>> tasks = new List<Task<RSM.CompiledModel>>();

            foreach(var model in objects) {
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