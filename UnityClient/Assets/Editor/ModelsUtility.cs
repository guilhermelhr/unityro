using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoadAttribute]
public class ModelsUtility {

    private static string GENERATED_RESOURCES_PATH = Path.Combine("Assets", "_Generated", "Resources");
    [MenuItem("UnityRO/Utils/Prepare/Models")]
    static void PrepareModels() {
        EditorSceneManager.OpenScene("Assets/Scenes/UtilityScenes/ModelsScene.unity");
        EditorApplication.EnterPlaymode();
    }

    [MenuItem("UnityRO/Utils/Extract/Models")]
    static void ExtractModels() {
        var mapObject = MapsUtility.FindMap().GetComponent<ModelsSceneManager>();
        var originalMeshes = mapObject.transform.FindRecursive("_Originals");
        var count = originalMeshes.transform.childCount;

        try {
            AssetDatabase.StartAssetEditing();

            for (int i = 0; i < count; i++) {
                var progress = i * 1f / count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting models - {progress * 100}%", progress)) {
                    break;
                }

                var mesh = originalMeshes.transform.GetChild(i);

                var meshFileName = Path.GetFileNameWithoutExtension(mesh.name);
                var meshPathWithoutExtension = mesh.name.Substring(0, mesh.name.IndexOf(Path.GetExtension(mesh.name)));
                var meshPath = Path.Combine(GENERATED_RESOURCES_PATH, meshPathWithoutExtension);
                Directory.CreateDirectory(meshPath);

                var filters = mesh.GetComponentsInChildren<MeshFilter>();
                var renderers = mesh.GetComponentsInChildren<MeshRenderer>();
                for (int k = 0; k < filters.Length; k++) {
                    var filter = filters[k];
                    var material = renderers[k].material;

                    var filterName = filter.gameObject.name.Replace("/", ".").Replace("\\", ".");

                    AssetDatabase.CreateAsset(material, Path.Combine(meshPath, $"{meshFileName}_{filterName}_{i}_{k}.mat"));
                    AssetDatabase.CreateAsset(filter.mesh, Path.Combine(meshPath, $"{meshFileName}_{filterName}_{i}_{k}.asset"));
                }

                meshPath = AssetDatabase.GenerateUniqueAssetPath(meshPath + ".prefab");
                PrefabUtility.SaveAsPrefabAssetAndConnect(mesh.gameObject, meshPath, InteractionMode.UserAction);

                Debug.Log($"Created model {i + 1} of {count} at {meshPath}");
            }

        } finally {
            EditorUtility.ClearProgressBar();
            EditorApplication.ExitPlaymode();
            AssetDatabase.StopAssetEditing();
        }
    }
}
#endif
