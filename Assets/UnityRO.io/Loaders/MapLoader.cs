using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ROIO.Loaders
{
    /// <summary>
    /// Loaders for a ro map
    /// 
    /// @author Guilherme Hernandez
    /// Based on ROBrowser by Vincent Thibault (robrowser.com)
    /// </summary>
    public class MapLoader
    {
        public const int BATCH_SIZE = 20;
        private float progress = 0;
        public Action<int> OnProgress = null;

        public float Progress
        {
            get
            {
                return progress;
            }

            set
            {
                var progress = Math.Min(100, value);
                if ((int)progress != (int)this.progress && OnProgress != null)
                {
                    OnProgress.Invoke((int)progress);
                }
                this.progress = progress;
            }
        }

        public IEnumerator Load(string mapname, Action<string, string, object> callback)
        {
            Progress = 0;

            RSW world = LoadWorld(mapname, callback);
            GAT altitude = LoadAltitude(mapname, callback);
            GND ground = LoadGround(mapname, world, callback);

            yield return LoadModels(mapname, world.modelDescriptors, callback);
        }

        private RSW LoadWorld(string mapname, Action<string, string, object> callback)
        {
            // Load RSW
            string rswPath = "data/" + GetFilePath(mapname);
            RSW world = FileManager.Load(rswPath) as RSW;
            if (world == null)
            {
                throw new Exception("Could not load rsw for " + mapname);
            }

            Progress += 1;
            callback.Invoke(mapname, "MAP_WORLD", world);

            return world;
        }

        private GAT LoadAltitude(string mapname, Action<string, string, object> callback)
        {
            string gatPath = "data/" + GetFilePath(WorldLoader.files.gat);
            GAT altitude = FileManager.Load(gatPath) as GAT;
            if (altitude == null)
            {
                throw new Exception("Could not load gat for " + mapname);
            }

            Progress += 1;
            callback.Invoke(mapname, "MAP_ALTITUDE", altitude);

            return altitude;
        }

        private GND LoadGround(string mapname, RSW world, Action<string, string, object> callback)
        {
            string gndPath = "data/" + GetFilePath(WorldLoader.files.gnd);
            GND ground = FileManager.Load(gndPath) as GND;
            if (ground == null)
            {
                throw new Exception("Could not load gnd for " + mapname);
            }

            GND.Mesh compiledGround = GroundLoader.Compile(ground, world.water.level, world.water.waveHeight);
            LoadGroundTexture(world, compiledGround);

            Progress += 1;
            callback.Invoke(mapname, "MAP_GROUND", compiledGround);

            return ground;
        }

        private IEnumerator LoadModels(string mapname, List<RSW.ModelDescriptor> modelDescriptors, Action<string, string, object> callback)
        {
            /**
             * Divide by 3 because of each loop in which we increase progress
             */
            float remainingProgress = 100 - Progress;
            float modelProgress = remainingProgress / modelDescriptors.Count / 3;

            for (int i = 0; i < modelDescriptors.Count; i++)
            {
                var model = modelDescriptors[i];
                model.filename = "data/model/" + model.filename;

                FileManager.Load(model.filename);
                Progress += modelProgress;

                if (i % BATCH_SIZE == 0)
                    yield return new WaitForEndOfFrame();
            }

            //create model instances
            HashSet<RSM> objectsSet = new HashSet<RSM>();
            for (int i = 0; i < modelDescriptors.Count; ++i)
            {
                RSM model = (RSM)FileManager.Load(modelDescriptors[i].filename);
                if (model != null)
                {
                    model.CreateInstance(modelDescriptors[i]);
                    objectsSet.Add(model);
                }
                Progress += modelProgress;

                if (i % BATCH_SIZE == 0)
                    yield return new WaitForEndOfFrame();
            }
            FileCache.ClearAllWithExt("rsm");
            RSM[] objects = new RSM[objectsSet.Count];
            objectsSet.CopyTo(objects);

            yield return CompileModels(objects, modelProgress, (compiledModels) =>
            {
                callback.Invoke(mapname, "MAP_MODELS", compiledModels);
            });
        }

        private IEnumerator CompileModels(RSM[] objects, float progressStep, Action<List<RSM.CompiledModel>> OnComplete)
        {
            /**
             * Calculate progress again because rsm objects
             * are way less than model descriptors and divide by 2
             * beucase we still have the render loading
             */
            float remainingProgress = 100 - Progress;
            float modelProgress = remainingProgress / objects.Length / 2;

            List<RSM.CompiledModel> models = new List<RSM.CompiledModel>();
            for (int i = 0; i < objects.Length; i++)
            {
                var compiledModel = ModelLoader.Compile(objects[i]);
                models.Add(compiledModel);

                Progress += modelProgress;
                if (i % 5 == 0)
                    yield return new WaitForEndOfFrame();
            }

            OnComplete(models);

            yield return models;
        }

        private IEnumerator LoadModelTexture(RSM.CompiledModel model)
        {
            HashSet<string> textures = new HashSet<string>();
            foreach (var nodeMesh in model.nodesData)
            {
                //load its textures
                foreach (var textureId in nodeMesh.Keys)
                {
                    var texture = "data/texture/" + model.rsm.textures[textureId];
                    //load texture
                    if (!textures.Contains(texture))
                    {
                        textures.Add(texture);
                        FileManager.Load(texture);
                        yield return new WaitForEndOfFrame();
                    }

                    if (textures.Count == model.rsm.textures.Length)
                    {
                        //we found every possible texture, no need to keep looking for new ones
                        yield break;
                    }
                }
            }

            yield return null;
        }

        private void LoadModelsTextures(List<RSM.CompiledModel> compiledModels)
        {
            HashSet<string> textures = new HashSet<string>();

            //enqueue textures
            FileManager.InitBatch();

            //for each model
            foreach (var model in compiledModels)
            {
                //and each of its nodes
                foreach (var nodeMesh in model.nodesData)
                {
                    //load its textures
                    foreach (var textureId in nodeMesh.Keys)
                    {
                        var texture = "data/texture/" + model.rsm.textures[textureId];
                        //load texture
                        if (!textures.Contains(texture))
                        {
                            textures.Add(texture);
                            FileManager.Load(texture);
                        }

                        if (textures.Count == model.rsm.textures.Length)
                        {
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

        private void LoadGroundTexture(RSW world, GND.Mesh ground)
        {
            LinkedList<string> textures = new LinkedList<string>();

            FileManager.InitBatch();

            //queue water textures
            if (ground.waterVertCount > 0)
            {
                var path = "data/texture/\xbf\xf6\xc5\xcd/water" + world.water.type;
                for (int i = 0; i < 32; i++)
                {
                    string num = i < 10 ? "0" + i : "" + i;
                    textures.AddLast(path + num + ".jpg");
                    FileManager.Load(textures.Last.Value);
                }
            }

            //queue ground textures
            for (int i = 0; i < ground.textures.Length; i++)
            {
                textures.AddLast("data/texture/" + ground.textures[i]);
                FileManager.Load(textures.Last.Value);
            }

            FileManager.EndBatch();

            //splice water textures from ground textures
            List<string> waterTextures = new List<string>();
            if (ground.waterVertCount > 0)
            {
                for (int i = 0; i < 32; i++)
                {
                    waterTextures.Add(textures.First.Value);
                    textures.RemoveFirst();
                }
            }

            waterTextures.CopyTo(world.water.images);
            ground.textures = new string[textures.Count];
            textures.CopyTo(ground.textures, 0);
        }

        private string GetFilePath(string path)
        {
            if (Tables.ResNameTable.ContainsKey(path))
            {
                return Convert.ToString(Tables.ResNameTable[path]);
            }

            return path;
        }
    }
}