using ROIO;
using ROIO.Loaders;
using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class ModelsSceneManager : MonoBehaviour {
    internal float onProgress;

    [SerializeField]
    private bool ExtractOnlyMissingModels = false;

    // Start is called before the first frame update
    async void Start() {
        var config = ConfigurationLoader.Init();
        FileManager.LoadGRF(config.root, config.grf);
        var descriptorsHashtable = FileManager.GetFileDescriptors();

        var modelDescriptors = FindDescriptors(descriptorsHashtable).Take(10).ToList();

        MapRenderer.mapParent = gameObject;
        List<RSM.CompiledModel> compiledModels = new List<RSM.CompiledModel>();

        foreach (var descriptor in modelDescriptors) {
            try {
                RSM model = FileManager.Load(descriptor) as RSM;
                if (model != null) {
                    model.filename = descriptor;
                    compiledModels.Add(ModelLoader.Compile(model));
                } else {
                    Debug.LogError($"Failed to compile {descriptor}");
                }
            } catch (Exception e) {
                Debug.LogError($"{descriptor} ${e}");
            }
        }

        Debug.Log($"Finished compiling {compiledModels.Count} of {modelDescriptors.Count} models");

        var count = 0;
        await new Models(compiledModels).BuildMeshes(delegate (float progress) {
            count++;

            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Loading models - {progress * 100}%", progress)) {
                EditorApplication.ExitPlaymode();
                EditorUtility.ClearProgressBar();
            }

            if (count == modelDescriptors.Count) {
                EditorUtility.ClearProgressBar();
                //EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Models");
            }
        });
    }

    private List<string> FindDescriptors(Hashtable descriptorsHashtable) {
        List<string> modelDescriptors;

        if (ExtractOnlyMissingModels) {
            try {
                modelDescriptors = FindMissingModelsDescriptors();
            } catch {
                modelDescriptors = FindAllDescriptors(descriptorsHashtable);
            }
        } else {
            modelDescriptors = FindAllDescriptors(descriptorsHashtable);
        }

        return modelDescriptors;
    }

    private List<string> FindAllDescriptors(Hashtable descriptorsHashtable) {
        List<string> modelDescriptors;
        var descriptors = new DictionaryEntry[descriptorsHashtable.Count];
        descriptorsHashtable.CopyTo(descriptors, 0);
        modelDescriptors = descriptors
            .Where(it => Path.GetExtension(it.Key.ToString()) == ".rsm")
            .Select(it => it.Key.ToString())
            .ToList();
        return modelDescriptors;
    }

    private List<string> FindMissingModelsDescriptors() {
        List<string> modelDescriptors;
        var lines = File.ReadAllLines("Assets/Logs/missing-models.txt");
        modelDescriptors = lines.Select(it => {
            var dir = Path.GetDirectoryName(it)["Assets\\_Generated\\Resources\\".Length..];
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(it);

            return Path.Combine(dir, filenameWithoutExtension + ".rsm");
        }).ToList();
        return modelDescriptors;
    }
}
#endif