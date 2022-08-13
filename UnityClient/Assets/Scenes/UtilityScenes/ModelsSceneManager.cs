using ROIO;
using ROIO.Loaders;
using ROIO.Models.FileTypes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class ModelsSceneManager : MonoBehaviour {
    internal float onProgress;

    // Start is called before the first frame update
    async void Start() {
        var config = ConfigurationLoader.Init();
        FileManager.LoadGRF(config.root, config.grf);
        var descriptorsHashtable = FileManager.GetFileDescriptors();
        var descriptors = new DictionaryEntry[descriptorsHashtable.Count];
        descriptorsHashtable.CopyTo(descriptors, 0);
        var modelDescriptors = descriptors.Where(it => Path.GetExtension(it.Key.ToString()) == ".rsm").ToList();

        MapRenderer.mapParent = gameObject;
        List<RSM.CompiledModel> compiledModels = new List<RSM.CompiledModel>();

        foreach (var descriptor in modelDescriptors) {
            RSM model = FileManager.Load(descriptor.Key.ToString()) as RSM;
            if (model != null) {
                model.filename = descriptor.Key.ToString();
                compiledModels.Add(ModelLoader.Compile(model));
            }
        }

        var count = 0;
        await new Models(compiledModels).BuildMeshes(delegate (float progress) {
            count++;

            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Loading models - {progress * 100}%", progress)) {
                EditorApplication.ExitPlaymode();
                EditorUtility.ClearProgressBar();
            }

            if (count == modelDescriptors.Count) {
                EditorUtility.ClearProgressBar();
                EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Models");
            }
        });
    }
}
#endif