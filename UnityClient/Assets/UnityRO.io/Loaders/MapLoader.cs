using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ROIO.Loaders {
    /// <summary>
    /// Loaders for a ro map
    /// 
    /// @author Guilherme Hernandez
    /// Based on ROBrowser by Vincent Thibault (robrowser.com)
    /// </summary>
    public class MapLoader {

        public async Task Load(string mapname, Action<string, string, object> callback) {
            RSW world = await LoadWorld(mapname);
            callback.Invoke(mapname, "MAP_WORLD", world);

            GAT altitude = await LoadAltitude(mapname);
            callback.Invoke(mapname, "MAP_ALTITUDE", altitude);

            GND ground = await LoadGround(mapname, world);
            GND.Mesh compiledGround = await CompileGroundMesh(ground, world);
            callback.Invoke(mapname, "MAP_GROUND", compiledGround);

            RSM[] models = await LoadModels(mapname, world.modelDescriptors);
            RSM.CompiledModel[] compiledModels = await CompileModels(models);
            callback.Invoke(mapname, "MAP_MODELS", compiledModels);
        }

        private async Task<RSW> LoadWorld(string mapname) {
            string rswPath = "data/" + GetFilePath(mapname);
            RSW world = await Task.Run<RSW>(() => FileManager.Load(rswPath) as RSW);
            if (world == null) {
                throw new Exception("Could not load rsw for " + mapname);
            }

            return world;
        }

        private async Task<GAT> LoadAltitude(string mapname) {
            string gatPath = "data/" + GetFilePath(WorldLoader.files.gat);
            GAT altitude = await Task.Run<GAT>(() => FileManager.Load(gatPath) as GAT);
            if (altitude == null) {
                throw new Exception("Could not load gat for " + mapname);
            }

            return altitude;
        }

        private async Task<GND> LoadGround(string mapname, RSW world) {
            string gndPath = "data/" + GetFilePath(WorldLoader.files.gnd);
            GND ground = await Task.Run<GND>(() => FileManager.Load(gndPath) as GND);
            if (ground == null) {
                throw new Exception("Could not load gnd for " + mapname);
            }

            return ground;
        }

        private async Task<GND.Mesh> CompileGroundMesh(GND ground, RSW world) {
            GND.Mesh compiledGround = await Task.Run<GND.Mesh>(() => GroundLoader.Compile(ground, world.water.level, world.water.waveHeight));
            LoadGroundTexture(world, compiledGround);
            return compiledGround;
        }

        private async Task<RSM[]> LoadModels(string mapname, List<RSW.ModelDescriptor> modelDescriptors) {
            // cache models
            FileManager.InitBatch();
            for (int i = 0; i < modelDescriptors.Count; i++) {
                var model = modelDescriptors[i];
                model.filename = "data/model/" + model.filename;

                FileManager.Load(model.filename);
            }
            await Task.Run(() => FileManager.EndBatch());

            // create model instances
            // HashSet<RSM> objectsSet = new HashSet<RSM>();
            Task<RSM>[] tasks = new Task<RSM>[modelDescriptors.Count];
            
            for (int i = 0; i < modelDescriptors.Count; ++i) {
                tasks[i] = Task.Run<RSM>(() => {
                    RSM model = FileManager.Load(modelDescriptors[i].filename) as RSM;
                    if (model != null) {
                        model.CreateInstance(modelDescriptors[i]);
                    }
                    return model;
                });
            }

            RSM[] models = await Task.WhenAll(tasks);
            FileCache.ClearAllWithExt("rsm");
            // RSM[] objects = new RSM[objectsSet.Count];
            // objectsSet.CopyTo(objects);

            return models.ToList().Where(t => t != null).ToArray();
        }

        private async Task<RSM.CompiledModel[]> CompileModels(RSM[] objects) {
            Task<RSM.CompiledModel>[] tasks = new Task<RSM.CompiledModel>[objects.Length];
            for (int i = 0; i < objects.Length; i++) {
                tasks[i] = Task.Run<RSM.CompiledModel>(() => ModelLoader.Compile(objects[i]));
            }

            return await Task.WhenAll(tasks);
        }

        private IEnumerator LoadModelTexture(RSM.CompiledModel model) {
            HashSet<string> textures = new HashSet<string>();
            foreach (var nodeMesh in model.nodesData) {
                //load its textures
                foreach (var textureId in nodeMesh.Keys) {
                    var texture = "data/texture/" + model.rsm.textures[textureId];
                    //load texture
                    if (!textures.Contains(texture)) {
                        textures.Add(texture);
                        FileManager.Load(texture);
                        yield return new WaitForEndOfFrame();
                    }

                    if (textures.Count == model.rsm.textures.Length) {
                        //we found every possible texture, no need to keep looking for new ones
                        yield break;
                    }
                }
            }

            yield return null;
        }

        private void LoadModelsTextures(List<RSM.CompiledModel> compiledModels) {
            HashSet<string> textures = new HashSet<string>();

            //enqueue textures
            FileManager.InitBatch();

            //for each model
            foreach (var model in compiledModels) {
                //and each of its nodes
                foreach (var nodeMesh in model.nodesData) {
                    //load its textures
                    foreach (var textureId in nodeMesh.Keys) {
                        var texture = "data/texture/" + model.rsm.textures[textureId];
                        //load texture
                        if (!textures.Contains(texture)) {
                            textures.Add(texture);
                            FileManager.Load(texture);
                        }

                        if (textures.Count == model.rsm.textures.Length) {
                            //we found every possible texture, no need to keep looking for new ones
                            FileManager.EndBatch();

                            return;
                        }
                    }
                }
            }

            //load textures
            FileManager.EndBatch();
        }

        private void LoadGroundTexture(RSW world, GND.Mesh ground) {
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

            FileManager.EndBatch();

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