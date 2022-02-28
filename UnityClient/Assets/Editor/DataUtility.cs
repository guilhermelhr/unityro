using ROIO;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
[InitializeOnLoadAttribute]
public class DataUtility {

    [MenuItem("UnityRO/Utils/Extract/Textures")]
    static void ExtractTextures() {
        var config = ConfigurationLoader.Init();
        FileManager.LoadGRF(config.root, config.grf);
        var descriptors = FileManager.GetFileDescriptors();
        try {
            // This disable Unity's auto update of assets
            // Making it much faster to batch create files like we're about to do
            AssetDatabase.StartAssetEditing();

            var file = 1f;
            foreach (DictionaryEntry entry in descriptors) {
                try {
                    var progress = file / descriptors.Count;
                    if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting data (this is going to take 40 mins+) - {progress * 100}%", progress)) {
                        break;
                    }

                    string completePath = ExtractFile((entry.Key as string).Trim());

                    if (completePath == null) {
                        file++;
                        continue;
                    }

                    if (file % 50 == 0) {
                        AssetDatabase.ImportAsset(completePath);
                    }

                    file++;
                } catch (Exception e) {
                    Debug.LogError(e);
                }
            }
        } finally {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            EditorApplication.ExitPlaymode();
        }
    }

    [MenuItem("UnityRO/Utils/Bundle/Create AssetBundle")]
    static void BundleAssets() {
        AssetBundleBuild[] bundleMap = new AssetBundleBuild[1];

        bundleMap[0].assetBundleName = "texturesBundle";
        var texturePath = Path.Combine(Application.dataPath, "Resources", "Textures");
        var textures = Directory.GetFiles(texturePath, "*.*", SearchOption.AllDirectories)
            .Where(it => Path.HasExtension(it) && !it.Contains(".meta"))
            .Select(it => it.Replace(Application.dataPath, "Assets"))
            .ToArray();
        bundleMap[0].assetNames = textures;

        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", bundleMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    // WIP
    [MenuItem("UnityRO/Utils/Prefabs/Create Maps Prefabs")]
    static void CreateMapPrefabs() {
        var gameManager = Selection.activeGameObject.GetComponent<GameManager>();
        var map = FindMap();

        if (map != null) {
            SaveMap(map, gameManager);
        }
    }

    private static string ExtractFile(string path) {
        if (!path.StartsWith("data/texture/")) {
            return null;
        }
        var filename = Path.GetFileName(path);
        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(path);
        var extension = Path.GetExtension(filename).ToLowerInvariant();
        var dir = path.Substring(0, path.IndexOf(filename)).Replace("/", "\\");

        string assetPath = Path.Combine("Assets", "Resources", "Textures", dir);

        Directory.CreateDirectory(assetPath);

        var texture = FileManager.Load(path) as Texture2D;
        texture.alphaIsTransparency = true;
        var bytes = texture.EncodeToPNG();
        var completePath = Path.Combine(assetPath, filenameWithoutExtension + ".png");
        File.WriteAllBytes(completePath, bytes);
        return completePath;
    }

    private static void SaveMap(GameObject mapObject, GameManager gameManager) {
        string mapName = Path.GetFileNameWithoutExtension(mapObject.name);
        string localPath = Path.Combine("Assets", "Resources", "Prefabs", "Data", "Maps");
        Directory.CreateDirectory(localPath);

        var originalMeshes = mapObject.transform.FindRecursive("_Originals");

        for (int i = 0; i < originalMeshes.transform.childCount; i++) {
            var mesh = originalMeshes.transform.GetChild(i);
            mesh.gameObject.SetActive(true);
            var meshPathWithoutExtension = mesh.name.Substring(0, mesh.name.IndexOf(Path.GetExtension(mesh.name)));
            var meshPath = Path.Combine("Assets", "Resources", "Meshes", "data", "models", meshPathWithoutExtension);
            Directory.CreateDirectory(meshPath);

            var filters = mesh.GetComponentsInChildren<MeshFilter>();
            var renderers = mesh.GetComponentsInChildren<MeshRenderer>();
            for (int k = 0; k < filters.Length; k++) {
                var filter = filters[k];
                var material = renderers[k].material;

                var progress = k * 1f / filters.Length;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving meshes - {progress * 100}%", progress)) {
                    break;
                }

                AssetDatabase.CreateAsset(material, Path.Combine(meshPath, $"{filter.gameObject.name}.mat"));
                AssetDatabase.CreateAsset(filter.mesh, Path.Combine(meshPath, $"{filter.gameObject.name}.asset"));
                var partPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{filter.gameObject.name}.prefab"));
                PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, partPath, InteractionMode.UserAction);
            }

            meshPath = AssetDatabase.GenerateUniqueAssetPath(meshPath + ".prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(mesh.gameObject, meshPath, InteractionMode.UserAction);
        }

        localPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(localPath, $"{mapName}.prefab"));
        PrefabUtility.SaveAsPrefabAssetAndConnect(mapObject, localPath, InteractionMode.UserAction);
        EditorUtility.ClearProgressBar();
        EditorApplication.ExitPlaymode();
    }

    private static GameObject FindMap() {
        return SceneManager.GetActiveScene().GetRootGameObjects().ToList().Find(go => go.tag == "Map");
    }

    [MenuItem("UnityRO/Utils/Prefabs/Create Maps Prefabs", true)]
    static bool ValidateCreateMapPrefabs() {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject) && Selection.activeGameObject.GetComponent<GameManager>() != null;
    }
}
#endif