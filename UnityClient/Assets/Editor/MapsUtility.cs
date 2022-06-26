using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
[InitializeOnLoadAttribute]
public class MapsUtility {

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

    [MenuItem("UnityRO/Utils/Extract/Selected model")]
    static void ExtractCurrentlySelectedMesh() {
        var mesh = Selection.activeGameObject;

        ExtractMesh(mesh);
    }

    [MenuItem("UnityRO/Utils/Prefabs/Create Current Map Prefab")]
    static void CreateCurrentMapPrefab() {
        var gameManager = Selection.activeGameObject.GetComponent<GameManager>();
        var map = FindMap();

        if (map != null) {
            SaveMap(map, gameManager);
        }
    }

    public static void SaveMap(GameObject mapObject, GameManager gameManager) {
        string mapName = Path.GetFileNameWithoutExtension(mapObject.name);
        string localPath = Path.Combine(DataUtility.GENERATED_RESOURCES_PATH, "Prefabs", "Maps");
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
            UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(mapObject, localPath, InteractionMode.UserAction);
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
            var meshPath = Path.Combine(DataUtility.GENERATED_RESOURCES_PATH, "data", "model", "water", mapObject.name, $"_{i}");
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
                UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, partPath, InteractionMode.UserAction);
            }
        }
    }

    private static void ExtractGround(GameObject mapObject) {
        var groundMeshes = mapObject.transform.FindRecursive("_Ground");
        for (int i = 0; i < groundMeshes.transform.childCount; i++) {
            var mesh = groundMeshes.transform.GetChild(i);
            mesh.gameObject.SetActive(true);
            var meshPath = Path.Combine(DataUtility.GENERATED_RESOURCES_PATH, "data", "model", "ground", mapObject.name, $"_{i}");
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
                        mainTex = DuplicateTexture(mainTex);
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
                        lightmapTex = DuplicateTexture(lightmapTex);
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
                        tintmapTex = DuplicateTexture(tintmapTex);
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
                UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, prefabPath, InteractionMode.UserAction);
            }
        }
    }

    private static void ExtractOriginalModels(GameObject mapObject) {
        var originalMeshes = mapObject.transform.FindRecursive("_Originals");
        /**
         * we need to set the new prefabs to another parent so we can delete the old meshes
         */
        var newOriginalParent = new GameObject("_Original");
        newOriginalParent.transform.SetParent(originalMeshes.transform.parent);

        var children = originalMeshes.transform.GetChildren();

        var i = 0;
        foreach (var mesh in children) {
            var progress = i * 1f / originalMeshes.transform.childCount;
            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving model meshes - {progress * 100}%", progress)) {
                break;
            }

            mesh.gameObject.SetActive(true);

            try {
                ExtractMesh(mesh.gameObject, newOriginalParent.transform);
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
            var meshPath = Path.Combine(DataUtility.GENERATED_RESOURCES_PATH, "data", "model", meshPathWithoutExtension);

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

            var prefab = UnityEditor.PrefabUtility.InstantiatePrefab(originalPrefabs[meshPathWithoutExtension], cloned.transform) as GameObject;
            prefab.transform.SetPositionAndRotation(mesh.transform.position, mesh.transform.rotation);
            prefab.transform.localScale = mesh.transform.localScale;
        }

        GameObject.DestroyImmediate(clonedMeshes.gameObject);
        GameObject.DestroyImmediate(originalMeshes.gameObject);
    }

    public static void ExtractMesh(GameObject mesh, Transform overrideParent = null) {
        string meshPathWithoutExtension;
        if (Path.GetExtension(mesh.name) == "") {
            meshPathWithoutExtension = mesh.name;
        } else {
            meshPathWithoutExtension = mesh.name.Substring(0, mesh.name.IndexOf(Path.GetExtension(mesh.name)));
        }
        var meshPath = Path.Combine(DataUtility.GENERATED_RESOURCES_PATH, "data", "model", meshPathWithoutExtension);
        Directory.CreateDirectory(meshPath);

        if (File.Exists(meshPath + ".prefab")) {
            var prefabObject = AssetDatabase.LoadAssetAtPath(meshPath + ".prefab", typeof(GameObject)) as GameObject;
            var prefab = UnityEditor.PrefabUtility.InstantiatePrefab(prefabObject, mesh.transform.parent) as GameObject;
            prefab.transform.SetPositionAndRotation(mesh.transform.position, mesh.transform.rotation);
            prefab.transform.localScale = mesh.transform.localScale;
            if (overrideParent != null) {
                prefab.transform.SetParent(overrideParent);
            }
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

    internal static GameObject FindMap() {
        return SceneManager
            .GetActiveScene()
            .GetRootGameObjects()
            .ToList()
            .Find(go => go.tag == "Map" && go.activeInHierarchy);
    }

    #region Validators

    [MenuItem("UnityRO/Utils/Prefabs/Create Maps Prefabs", true)]
    static bool ValidateCreateMapPrefabs() {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject) && Selection.activeGameObject.GetComponent<GameManager>() != null;
    }

    [MenuItem("UnityRO/Utils/Extract/Selected model", true)]
    static bool ValidateExtractCurrentlySelectedMesh() {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject);
    }

    #endregion

    #region Utilities
    internal static Texture2D DuplicateTexture(Texture2D source) {
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

    #endregion
}
#endif
