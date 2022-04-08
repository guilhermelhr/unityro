using Assets.Scripts.Renderer.Sprite;
using ROIO;
using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

    [MenuItem("UnityRO/Utils/Extract/Sprites")]
    static void ExtractSprites() {
        var config = ConfigurationLoader.Init();
        FileManager.LoadGRF(config.root, config.grf);
        var descriptors = FilterDescriptors(FileManager.GetFileDescriptors(), "data/sprite").ToList();
        var sprDescriptors = descriptors.Where(t => Path.GetExtension(t) == ".spr").ToList();
        var actDescriptors = descriptors.Where(t => Path.GetExtension(t) == ".act").ToList();
        try {
            // This disable Unity's auto update of assets
            // Making it much faster to batch create files like we're about to do
            AssetDatabase.StartAssetEditing();

            for (int i = 0; i < sprDescriptors.Count; i++) {
                var progress = i * 1f / sprDescriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting sprites - {progress * 100}%", progress)) {
                    break;
                }

                var sprPath = sprDescriptors[i];
                var actPath = actDescriptors[i];

                var filename = Path.GetFileName(sprPath);
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(sprPath);
                var dir = sprPath.Substring(0, sprPath.IndexOf(filename)).Replace("/", "\\");

                string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, "Sprites", dir);

                Directory.CreateDirectory(assetPath);

                var spr = FileManager.Load(sprPath) as SPR;
                spr.SwitchToRGBA();
                spr.Compile();

                var sprites = spr.GetSprites();
                var textures = sprites.Select(it => it.texture).ToArray();
                if (spr == null) {
                    continue;
                }

                var atlas = new Texture2D(2, 2);
                atlas.name = $"{filenameWithoutExtension}_atlas";
                var rects = atlas.PackTextures(textures, 2, 2048, false);
                atlas.filterMode = FilterMode.Point;
                File.WriteAllBytes(Path.Combine(assetPath, $"{filenameWithoutExtension}.png"), atlas.EncodeToPNG());

                var act = FileManager.Load(actPath) as ACT;
                var spriteData = ScriptableObject.CreateInstance<SpriteData>();

                spriteData.act = act;
                spriteData.rects = rects;

                AssetDatabase.CreateAsset(spriteData, Path.Combine(assetPath, $"{filenameWithoutExtension}.asset"));
            }
        } finally {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            EditorApplication.ExitPlaymode();
        }
    }

    static List<string> FilterDescriptors(Hashtable descriptors, string filter) {
        List<string> result = new List<string>();
        foreach (DictionaryEntry entry in descriptors) {
            string path = (entry.Key as string).Trim();
            if (path.StartsWith(filter)) {
                result.Add(path);
            }
        }

        return result;
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

    [MenuItem("UnityRO/Utils/Prefabs/Create Maps Prefabs")]
    static async void CreateMapPrefabs() {
        var gameManager = Selection.activeGameObject.GetComponent<GameManager>();
        var offlineUtility = gameManager.GetComponent<OfflineUtility>();

        foreach (var mapName in offlineUtility.MapNames) {
            try {
                offlineUtility.MapName = mapName;
                await offlineUtility.LoadMap();
                await Task.Delay(2000);
                var map = FindMap();

                if (map != null) {
                    SaveMap(map, gameManager);
                }
            } catch {
                Debug.LogError($"Error saving {mapName}");
            }

        }
    }

    [MenuItem("UnityRO/Utils/Prefabs/Create Current Map Prefab")]
    static void CreateCurrentMapPrefab() {
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

    [MenuItem("UnityRO/Utils/Extract/Selected model")]
    static void ExtractCurrentlySelectedMesh() {
        var mesh = Selection.activeGameObject;

        ExtractMesh(mesh);
    }

    private static void ExtractMesh(GameObject mesh) {
        string meshPathWithoutExtension;
        if (Path.GetExtension(mesh.name) == "") {
            meshPathWithoutExtension = mesh.name;
        } else {
            meshPathWithoutExtension = mesh.name.Substring(0, mesh.name.IndexOf(Path.GetExtension(mesh.name)));
        }
        var meshPath = Path.Combine(GENERATED_RESOURCES_PATH, "Meshes", "data", "model", meshPathWithoutExtension);
        Directory.CreateDirectory(meshPath);

        if (File.Exists(meshPath + ".prefab")) {
            var prefabObject = AssetDatabase.LoadAssetAtPath(meshPath + ".prefab", typeof(GameObject)) as GameObject;
            var prefab = PrefabUtility.InstantiatePrefab(prefabObject, mesh.transform.parent) as GameObject;
            prefab.transform.SetPositionAndRotation(mesh.transform.position, mesh.transform.rotation);
            prefab.transform.localScale = mesh.transform.localScale;
        } else {
            var nodes = mesh.GetComponentsInChildren<NodeProperties>();
            foreach (var node in nodes) {
                var filter = node.GetComponent<MeshFilter>();
                var material = node.GetComponent<MeshRenderer>().material;

                var nodeName = node.mainName.Length == 0 ? "node" : node.mainName;
                var partPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{nodeName}_{node.nodeId}.asset"));
                AssetDatabase.CreateAsset(filter.mesh, partPath);
                AssetDatabase.AddObjectToAsset(material, partPath);
            }

            meshPath = AssetDatabase.GenerateUniqueAssetPath(meshPath + ".prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(mesh.gameObject, meshPath, InteractionMode.UserAction);
        }
    }

    private static string ExtractFile(string path) {
        if (!path.StartsWith("data/texture/")) {
            return null;
        }
        var filename = Path.GetFileName(path);
        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(path);
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

    public static void SaveMap(GameObject mapObject, GameManager gameManager) {
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
            AssetDatabase.StartAssetEditing();
            ExtractClonedModels(mapObject);
            AssetDatabase.StopAssetEditing();

            ExtractGround(mapObject);
            ExtractWater(mapObject);

            localPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(localPath, $"{mapName}.prefab"));
            PrefabUtility.SaveAsPrefabAssetAndConnect(mapObject, localPath, InteractionMode.UserAction);
        } finally {
            EditorUtility.ClearProgressBar();
        }
    }

    private static void ExtractWater(GameObject mapObject) {
        var waterMeshes = mapObject.transform.FindRecursive("_Water");
        if (waterMeshes == null) {
            return;
        }
        for (int i = 0; i < waterMeshes.transform.childCount; i++) {
            var mesh = waterMeshes.transform.GetChild(i);
            mesh.gameObject.SetActive(true);
            var meshPath = Path.Combine(GENERATED_RESOURCES_PATH, "Meshes", "Water", mapObject.name, $"_{i}");
            Directory.CreateDirectory(meshPath);

            var progress = i * 1f / waterMeshes.transform.childCount;
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
                    if (!mainTex.isReadable) {
                        mainTex = duplicateTexture(mainTex);
                    }
                    var path = Path.Combine(meshPath, "texture.png");
                    var bytes = mainTex.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);

                    AssetDatabase.ImportAsset(path);
                    var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                    material.SetTexture("_MainTex", tex);
                }
                if (lightmapTex != null) {
                    if (!lightmapTex.isReadable) {
                        lightmapTex = duplicateTexture(lightmapTex);
                    }
                    var path = Path.Combine(meshPath, "lightmap.png");
                    var bytes = lightmapTex.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);

                    AssetDatabase.ImportAsset(path);
                    var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                    material.SetTexture("_Lightmap", tex);
                }
                if (tintmapTex != null) {
                    if (!tintmapTex.isReadable) {
                        tintmapTex = duplicateTexture(tintmapTex);
                    }
                    var path = Path.Combine(meshPath, "tintmap.png");
                    var bytes = tintmapTex.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);

                    AssetDatabase.ImportAsset(path);
                    var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                    material.SetTexture("_Tintmap", tex);
                }

                var partPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{filter.gameObject.name}_{k}.asset"));
                AssetDatabase.CreateAsset(filter.mesh, partPath);
                AssetDatabase.AddObjectToAsset(material, partPath);

                var prefabPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{filter.gameObject.name}.prefab"));
                PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, prefabPath, InteractionMode.UserAction);
            }
        }
    }

    private static void ExtractOriginalModels(GameObject mapObject) {
        var originalMeshes = mapObject.transform.FindRecursive("_Originals");
        var children = originalMeshes.transform.GetChildren();

        var i = 0;
        foreach (var mesh in children) {
            var progress = i * 1f / originalMeshes.transform.childCount;
            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving model meshes - {progress * 100}%", progress)) {
                break;
            }

            mesh.gameObject.SetActive(true);

            try {
                ExtractMesh(mesh.gameObject);
            } catch (Exception e) {
                Debug.LogError(e);
                Debug.LogError($"Error extracting model {mesh.gameObject.name}");
            } finally {
                i++;
            }
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
            string meshPathWithoutExtension;
            if (Path.GetExtension(mesh.name) == "") {
                meshPathWithoutExtension = mesh.name;
            } else {
                meshPathWithoutExtension = mesh.name.Substring(0, mesh.name.IndexOf(Path.GetExtension(mesh.name)));
            }
            var meshPath = Path.Combine(GENERATED_RESOURCES_PATH, "Meshes", "data", "model", meshPathWithoutExtension);

            var prefab = AssetDatabase.LoadAssetAtPath(meshPath + ".prefab", typeof(GameObject)) as GameObject;
            if (!originalPrefabs.ContainsKey(meshPathWithoutExtension)) {
                originalPrefabs.Add(meshPathWithoutExtension, prefab);
            }
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
        return SceneManager.GetActiveScene().GetRootGameObjects().ToList().Find(go => go.tag == "Map" && go.activeInHierarchy);
    }

    [MenuItem("UnityRO/Utils/Prefabs/Create Maps Prefabs", true)]
    static bool ValidateCreateMapPrefabs() {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject) && Selection.activeGameObject.GetComponent<GameManager>() != null;
    }

    [MenuItem("UnityRO/Utils/Extract/Selected model", true)]
    static bool ValidateExtractCurrentlySelectedMesh() {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject);
    }

    private static Texture2D duplicateTexture(Texture2D source) {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
#endif