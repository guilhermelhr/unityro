using ROIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
[InitializeOnLoadAttribute]
public class DataUtility {

    private static string GENERATED_RESOURCES_PATH = Path.Combine("Assets", "_Generated", "Resources");

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
                    if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting textures - {progress * 100}%", progress)) {
                        break;
                    }

                    string completePath = ExtractFile((entry.Key as string).Trim());

                    if (completePath == null) {
                        file++;
                        continue;
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
        var texturePath = Path.Combine(Application.dataPath, GENERATED_RESOURCES_PATH, "Textures");
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

    [MenuItem("UnityRO/Utils/Prepare/Models")]
    static void PrepareModels() {
        EditorSceneManager.OpenScene("Assets/Scenes/UtilityScenes/ModelsScene.unity");
        EditorApplication.EnterPlaymode();
    }

    [MenuItem("UnityRO/Utils/Extract/Models")]
    static void ExtractModels() {
        var mapObject = FindMap().GetComponent<ModelsSceneManager>();
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
                var meshPath = Path.Combine(GENERATED_RESOURCES_PATH, "Meshes", meshPathWithoutExtension);
                Directory.CreateDirectory(meshPath);

                var filters = mesh.GetComponentsInChildren<MeshFilter>();
                var renderers = mesh.GetComponentsInChildren<MeshRenderer>();
                for (int k = 0; k < filters.Length; k++) {
                    var filter = filters[k];
                    var material = renderers[k].material;

                    AssetDatabase.CreateAsset(material, Path.Combine(meshPath, $"{meshFileName}_{filter.gameObject.name}_{i}.mat"));
                    AssetDatabase.CreateAsset(filter.mesh, Path.Combine(meshPath, $"{meshFileName}_{filter.gameObject.name}_{i}.asset"));
                    var partPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{filter.gameObject.name}.prefab"));
                    PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, partPath, InteractionMode.UserAction);
                }

                meshPath = AssetDatabase.GenerateUniqueAssetPath(meshPath + ".prefab");
                PrefabUtility.SaveAsPrefabAssetAndConnect(mesh.gameObject, meshPath, InteractionMode.UserAction);
            }

        } finally {
            EditorUtility.ClearProgressBar();
            EditorApplication.ExitPlaymode();
            AssetDatabase.StopAssetEditing();
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

        string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, "Textures", dir);

        Directory.CreateDirectory(assetPath);

        var texture = FileManager.Load(path) as Texture2D;
        if (texture == null) {
            return null;
        }
        texture.alphaIsTransparency = true;
        var bytes = texture.EncodeToPNG();
        var completePath = Path.Combine(assetPath, filenameWithoutExtension + ".png");
        File.WriteAllBytes(completePath, bytes);
        return completePath;
    }

    private static void SaveMap(GameObject mapObject, GameManager gameManager) {
        string mapName = Path.GetFileNameWithoutExtension(mapObject.name);
        string localPath = Path.Combine(GENERATED_RESOURCES_PATH, "Prefabs", "Maps");
        Directory.CreateDirectory(localPath);

        try {
            AssetDatabase.StartAssetEditing();
            ExtractOriginalModels(mapObject);
        } finally {
            AssetDatabase.StopAssetEditing();
        }

        try {
            ExtractGround(mapObject);
            ExtractWater(mapObject);
            ExtractClonedModels(mapObject);

            localPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(localPath, $"{mapName}.prefab"));
            PrefabUtility.SaveAsPrefabAssetAndConnect(mapObject, localPath, InteractionMode.UserAction);
        } finally {
            EditorUtility.ClearProgressBar();
            EditorApplication.ExitPlaymode();
        }
    }

    private static void ExtractWater(GameObject mapObject) {
        var groundMeshes = mapObject.transform.FindRecursive("_Water");
        for (int i = 0; i < groundMeshes.transform.childCount; i++) {
            var mesh = groundMeshes.transform.GetChild(i);
            mesh.gameObject.SetActive(true);
            var meshPath = Path.Combine(GENERATED_RESOURCES_PATH, "Meshes", "Water", mapObject.name, $"_{i}");
            Directory.CreateDirectory(meshPath);

            var progress = i * 1f / groundMeshes.transform.childCount;
            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving water meshes - {progress * 100}%", progress)) {
                break;
            }

            var filters = mesh.GetComponentsInChildren<MeshFilter>();
            var renderers = mesh.GetComponentsInChildren<MeshRenderer>();
            for (int k = 0; k < filters.Length; k++) {
                var filter = filters[k];
                var material = renderers[k].material;
                var mainTex = material.GetTexture("_MainTex") as Texture2D;

                if (mainTex != null) {
                    var path = Path.Combine(meshPath, "texture.png");
                    var bytes = mainTex.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);

                    AssetDatabase.ImportAsset(path);
                    var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                    material.SetTexture("_MainTex", tex);
                }

                AssetDatabase.CreateAsset(material, Path.Combine(meshPath, $"{filter.gameObject.name}.mat"));
                AssetDatabase.CreateAsset(filter.mesh, Path.Combine(meshPath, $"{filter.gameObject.name}.asset"));
                var partPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{filter.gameObject.name}.prefab"));
                PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, partPath, InteractionMode.UserAction);
            }
        }
    }

    private static void ExtractGround(GameObject mapObject) {
        var groundMeshes = mapObject.transform.FindRecursive("_Ground");
        for (int i = 0; i < groundMeshes.transform.childCount; i++) {
            var mesh = groundMeshes.transform.GetChild(i);
            mesh.gameObject.SetActive(true);
            var meshPath = Path.Combine(GENERATED_RESOURCES_PATH, "Meshes", "Ground", mapObject.name, $"_{i}");
            Directory.CreateDirectory(meshPath);

            var progress = i * 1f / groundMeshes.transform.childCount;
            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving ground meshes - {progress * 100}%", progress)) {
                break;
            }

            var filters = mesh.GetComponentsInChildren<MeshFilter>();
            var renderers = mesh.GetComponentsInChildren<MeshRenderer>();
            for (int k = 0; k < filters.Length; k++) {
                var filter = filters[k];
                var material = renderers[k].material;

                var mainTex = material.GetTexture("_MainTex") as Texture2D;
                var lightmapTex = material.GetTexture("_Lightmap") as Texture2D;
                var tintmapTex = material.GetTexture("_Tintmap") as Texture2D;

                if (mainTex != null) {
                    var path = Path.Combine(meshPath, "texture.png");
                    var bytes = mainTex.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);

                    AssetDatabase.ImportAsset(path);
                    var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                    material.SetTexture("_MainTex", tex);
                }
                if (lightmapTex != null) {
                    var path = Path.Combine(meshPath, "lightmap.png");
                    var bytes = lightmapTex.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);

                    AssetDatabase.ImportAsset(path);
                    var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                    material.SetTexture("_Lightmap", tex);
                }
                if (tintmapTex != null) {
                    var path = Path.Combine(meshPath, "tintmap.png");
                    var bytes = tintmapTex.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);

                    AssetDatabase.ImportAsset(path);
                    var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                    material.SetTexture("_Tintmap", tex);
                }

                AssetDatabase.CreateAsset(material, Path.Combine(meshPath, $"{filter.gameObject.name}.mat"));
                AssetDatabase.CreateAsset(filter.mesh, Path.Combine(meshPath, $"{filter.gameObject.name}.asset"));

                var partPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{filter.gameObject.name}.prefab"));
                PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, partPath, InteractionMode.UserAction);
            }
        }
    }

    private static void ExtractOriginalModels(GameObject mapObject) {
        var originalMeshes = mapObject.transform.FindRecursive("_Originals");

        for (int i = 0; i < originalMeshes.transform.childCount; i++) {
            var mesh = originalMeshes.transform.GetChild(i);
            mesh.gameObject.SetActive(true);
            var meshPathWithoutExtension = mesh.name.Substring(0, mesh.name.IndexOf(Path.GetExtension(mesh.name)));
            var meshPath = Path.Combine(GENERATED_RESOURCES_PATH, "Meshes", "data", "models", meshPathWithoutExtension);
            Directory.CreateDirectory(meshPath);

            var progress = i * 1f / originalMeshes.transform.childCount;
            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving model meshes - {progress * 100}%", progress)) {
                break;
            }

            var filters = mesh.GetComponentsInChildren<MeshFilter>();
            var renderers = mesh.GetComponentsInChildren<MeshRenderer>();
            for (int k = 0; k < filters.Length; k++) {
                var filter = filters[k];
                var material = renderers[k].material;

                AssetDatabase.CreateAsset(material, Path.Combine(meshPath, $"{filter.gameObject.name}.mat"));
                AssetDatabase.CreateAsset(filter.mesh, Path.Combine(meshPath, $"{filter.gameObject.name}.asset"));
                var partPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{filter.gameObject.name}.prefab"));
                PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, partPath, InteractionMode.UserAction);
            }

            meshPath = AssetDatabase.GenerateUniqueAssetPath(meshPath + ".prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(mesh.gameObject, meshPath, InteractionMode.UserAction);
        }
    }

    private static void ExtractClonedModels(GameObject mapObject) {
        var models = mapObject.transform.FindRecursive("_Models");
        var clonedMeshes = mapObject.transform.FindRecursive("_Copies");
        var originalMeshes = mapObject.transform.FindRecursive("_Originals");
        var originalPrefabs = new Dictionary<string, GameObject>();

        // Query for the original prefabs
        for (int i = 0; i < originalMeshes.transform.childCount; i++) {
            var mesh = originalMeshes.transform.GetChild(i);
            var meshPathWithoutExtension = mesh.name.Substring(0, mesh.name.IndexOf(Path.GetExtension(mesh.name)));
            var meshPath = Path.Combine(GENERATED_RESOURCES_PATH, "Meshes", "data", "models", meshPathWithoutExtension);

            var prefab = AssetDatabase.LoadAssetAtPath(meshPath + ".prefab", typeof(GameObject)) as GameObject;
            originalPrefabs.Add(meshPathWithoutExtension, prefab);
        }

        var cloned = new GameObject("_Cloned");
        cloned.transform.SetParent(models.transform);

        for (int i = 0; i < clonedMeshes.transform.childCount; i++) {
            var mesh = clonedMeshes.transform.GetChild(i);
            var originalMeshName = mesh.name.Substring(0, mesh.name.IndexOf("(Clone)"));
            var meshPathWithoutExtension = mesh.name.Substring(0, originalMeshName.IndexOf(Path.GetExtension(originalMeshName)));

            var prefab = PrefabUtility.InstantiatePrefab(originalPrefabs[meshPathWithoutExtension], cloned.transform) as GameObject;
            prefab.transform.SetPositionAndRotation(mesh.transform.position, mesh.transform.rotation);
            prefab.transform.localScale = mesh.transform.localScale;
        }

        GameObject.DestroyImmediate(clonedMeshes.gameObject);
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