using ROIO;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DataUtility {

    [MenuItem("UnityRO/Utils/Extract Data")]
    static void ExtractData() {
        var descriptors = FileManager.GetFileDescriptors();

        var file = 1f;
        foreach(DictionaryEntry entry in descriptors) {
            try {
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", "Extracting data", file / descriptors.Count)) {
                    break;
                }

                var path = (entry.Key as string).Trim().Replace("\\", "/");
                var bytes = FileManager.ReadSync(path).ToArray();
                var filename = Path.GetFileName(path);
                var dir = path.Substring(0, path.IndexOf(filename)).Replace("/", "\\");

                Directory.CreateDirectory(Path.Combine("Assets", "Resources", "Extracted", dir));

                File.WriteAllBytes(Path.Combine("Assets", "Resources", "Extracted", dir, filename), bytes);
                file++;
            } catch(Exception e) {
                Debug.LogError(e);
            }
        }

        EditorUtility.ClearProgressBar();
        EditorApplication.ExitPlaymode();
    }

    [MenuItem("UnityRO/Utils/Prefabs/Create Maps Prefabs")]
    static void CreateMapPrefabs() {
        var gameManager = Selection.activeGameObject.GetComponent<GameManager>();
        var map = FindMap();

        if (map != null) {
            SaveMap(map);
        }
    }

    private static void SaveMap(GameObject mapObject) {
        string localPath = Path.Combine("Assets", "Resources", "Prefabs", "Data", "Maps", mapObject.name + ".prefab");
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        PrefabUtility.SaveAsPrefabAssetAndConnect(mapObject, localPath, InteractionMode.UserAction);
    }

    private static GameObject FindMap() {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().ToList().Find(go => go.tag == "Map");
    }

    [MenuItem("UnityRO/Utils/Prefabs/Create Maps Prefabs", true)]
    static bool ValidateCreateMapPrefabs() {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject) && Selection.activeGameObject.GetComponent<GameManager>() != null;
    }
}
