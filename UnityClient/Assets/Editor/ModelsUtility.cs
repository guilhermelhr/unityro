#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoadAttribute]
public class ModelsUtility {

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
                try {
                    ExtractMesh(mesh.gameObject);
                } catch (Exception ex) {

                }
            }
        } finally {
            EditorUtility.ClearProgressBar();
            EditorApplication.ExitPlaymode();
            AssetDatabase.StopAssetEditing();
        }
    }

    public static void ExtractMesh(GameObject mesh, Transform overrideParent = null) {
        string meshPathWithoutExtension;
        if (Path.GetExtension(mesh.name).Length == 0) {
            meshPathWithoutExtension = mesh.name;
        } else {
            meshPathWithoutExtension = mesh.name.Substring(0, mesh.name.IndexOf(Path.GetExtension(mesh.name)));
        }

        string meshPath;
        if (mesh.name.Contains("data/model")) {
            meshPath = Path.Combine(DataUtility.GENERATED_RESOURCES_PATH, meshPathWithoutExtension);
        } else {
            meshPath = Path.Combine(DataUtility.GENERATED_RESOURCES_PATH, "data", "model", meshPathWithoutExtension);
        }

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
            try {
                var nodes = mesh.GetComponentsInChildren<NodeProperties>();
                foreach (var node in nodes) {
                    var filter = node.GetComponent<MeshFilter>();
                    var material = node.GetComponent<MeshRenderer>().material;

                    var nodeName = node.mainName.Length == 0 ? "node" : node.mainName;
                    var partPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{nodeName}_{node.nodeId}.asset"));
                    var materialPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{nodeName}_{node.nodeId}.mat"));
                    AssetDatabase.CreateAsset(filter.mesh, partPath);
                    AssetDatabase.CreateAsset(material, materialPath);
                }

                meshPath = AssetDatabase.GenerateUniqueAssetPath(meshPath + ".prefab");
                PrefabUtility.SaveAsPrefabAssetAndConnect(mesh.gameObject, meshPath, InteractionMode.UserAction);
            } catch(Exception ex) {
                Debug.LogError($"Failed extracting model {mesh.name}");
            }
        }
    }
}
#endif
