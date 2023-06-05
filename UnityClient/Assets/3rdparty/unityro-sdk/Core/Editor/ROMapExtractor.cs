#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ROIO;
using ROIO.Loaders;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityRO.Core.Extensions;

[InitializeOnLoad]
public class ROMapExtractor : EditorWindow {
    [SerializeField] private string grfRootPath = "C:/foo";
    [SerializeField] private List<string> grfPaths = new List<string>();
    [SerializeField] private string mapName = "prontera";

    private ReorderableList GrfReordableList;

    private GameMap CurrentGameMap;

    [MenuItem("Window/ROMapExtractor")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(ROMapExtractor));
    }

    async void LoadMap() {
        AsyncMapLoader.GameMapData gameMapData = await new AsyncMapLoader().Load($"{mapName}.rsw");
        CurrentGameMap = await new MapRenderer().RenderMap(gameMapData, mapName);
    }

    public static string GetBasePath() {
        return "Assets/3rdparty/unityro-resources/Resources/Maps/";
    }

    public static void SaveMap(GameObject mapObject) {
        string mapName = Path.GetFileNameWithoutExtension(mapObject.name);
        string localPath = Path.Combine(GetBasePath());
        Directory.CreateDirectory(localPath);
        var texturePaths = new List<string>();

        try {
            AssetDatabase.StartAssetEditing();
            texturePaths = ExtractOriginalModels(mapObject, Path.Combine(GetBasePath(), mapName, "models"));
        } finally {
            AssetDatabase.StopAssetEditing();
        }

        AssetDatabase.Refresh();

        foreach (var texture in texturePaths) {
            TextureImporter importer = AssetImporter.GetAtPath(texture) as TextureImporter;
            importer.alphaIsTransparency = true;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.filterMode = FilterMode.Bilinear;
            importer.mipmapEnabled = true;
            importer.mipMapBias = 0.5f;
            var textureSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(textureSettings);

            importer.SetTextureSettings(textureSettings);
            importer.SaveAndReimport();
        }

        try {
            AssetDatabase.StartAssetEditing();
            ExtractClonedModels(mapObject, Path.Combine(GetBasePath(), mapName, "models"));
            AssetDatabase.StopAssetEditing();

            ExtractGround(mapObject, mapName);
            ExtractWater(mapObject, mapName);

            AssetDatabase.Refresh();

            var meshesPathes = DataUtility
                .GetFilesFromDir(Path.Combine(GetBasePath(), mapName, "models"))
                .Where(it => Path.GetExtension(it) == ".mat")
                .Select(it => Path.ChangeExtension(it, ""))
                .ToList();
            foreach (var mesh in meshesPathes) {
                var material = AssetDatabase.LoadAssetAtPath<Material>(mesh + "mat");
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(mesh + "png");

                material.SetTexture("_MainTex", texture);
            }

            var models = mapObject.transform.Find("_Models");
            var originals = models.transform.Find("_Originals");
            var cloned = models.transform.Find("_Cloned");

            foreach (var child in originals.transform.GetChildren()) {
                foreach (var grandChildren in child.transform.GetChildren()) {
                    if (grandChildren.transform.GetComponentInChildren(typeof(NodeAnimation)) == null) {
                        child.gameObject.isStatic = true;
                    }
                }
            }

            foreach (var child in cloned.transform.GetChildren()) {
                foreach (var grandChildren in child.transform.GetChildren()) {
                    if (grandChildren.transform.GetComponentInChildren(typeof(NodeAnimation)) == null) {
                        child.gameObject.isStatic = true;
                    }
                }
            }

            AssetDatabase.Refresh();

            localPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(localPath, $"{mapName}.prefab"));
            PrefabUtility.SaveAsPrefabAssetAndConnect(mapObject, localPath, InteractionMode.AutomatedAction);

            var defaultScene = EditorSceneManager.GetActiveScene();
            var mapScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            mapScene.name = mapName;
            SceneManager.MoveGameObjectToScene(mapObject, mapScene);
            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveScene(mapScene, $"Assets/3rdparty/unityro-resources/Scenes/{mapName}.unity");
            EditorSceneManager.CloseScene(defaultScene, true);

            StaticOcclusionCulling.Compute();

            EditorSceneManager.SaveOpenScenes();

            var original = EditorBuildSettings.scenes;
            var newSettings = new EditorBuildSettingsScene[original.Length + 1];
            System.Array.Copy(original, newSettings, original.Length);
            var sceneToAdd = new EditorBuildSettingsScene(mapScene.path, true);
            newSettings[newSettings.Length - 1] = sceneToAdd;
            EditorBuildSettings.scenes = newSettings;
            
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");

            //ImportAssetAndApplyAddressableGroup(localPath, typeof(GameObject));
        } finally {
            EditorUtility.ClearProgressBar();
        }
    }

    private static void ExtractWater(GameObject mapObject, string mapName) {
        var waterMeshes = mapObject.transform.FindRecursive("_Water");
        if (waterMeshes == null) {
            return;
        }

        for (int i = 0; i < waterMeshes.transform.childCount; i++) {
            var mesh = waterMeshes.transform.GetChild(i);
            mesh.gameObject.SetActive(true);
            var meshPath = Path.Combine(GetBasePath(), mapName, "water", $"_{i}");
            Directory.CreateDirectory(meshPath);

            var progress = i * 1f / waterMeshes.transform.childCount;
            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving water meshes - {progress * 100}%",
                    progress)) {
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
                    //tex.SetAddressableGroup("Maps", "Maps");
                }

                var materialPath = Path.Combine(meshPath, $"{filter.gameObject.name.SanitizeForAddressables()}.mat");
                AssetDatabase.CreateAsset(material, materialPath);
                AssetDatabase.ImportAsset(materialPath);

                var filterPath = Path.Combine(meshPath, $"{filter.gameObject.name.SanitizeForAddressables()}.asset");
                AssetDatabase.CreateAsset(filter.mesh, filterPath);
                AssetDatabase.ImportAsset(filterPath);

                var partPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath,
                    $"{filter.gameObject.name.SanitizeForAddressables()}.prefab"));
                UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, partPath,
                    InteractionMode.AutomatedAction);
                AssetDatabase.ImportAsset(partPath);
            }
        }
    }

    private static void ExtractGround(GameObject mapObject, string mapName) {
        var ground = mapObject.transform.FindRecursive("_Ground");

        ExtractGroundTextures(mapName, ground, out var lightmapTexture, out var tintmapTexture, out var mainTexture, out var material);

        try {
            AssetDatabase.StartAssetEditing();

            for (var i = 0; i < ground.transform.childCount; i++) {
                var mesh = ground.transform.GetChild(i);
                mesh.gameObject.SetActive(true);
                var meshPath = Path.Combine(GetBasePath(), mapName, "ground", mesh.gameObject.name);
                Directory.CreateDirectory(meshPath);

                var progress = i * 1f / ground.transform.childCount;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving ground meshes - {progress * 100}%",
                        progress)) {
                    break;
                }

                var filter = mesh.GetComponent<MeshFilter>();
                mesh.GetComponent<MeshRenderer>().material = material;

                var partPath =
                    AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{filter.gameObject.name.SanitizeForAddressables()}.asset"));
                AssetDatabase.CreateAsset(filter.mesh, partPath);

                var prefabPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath,
                    $"{filter.gameObject.name.SanitizeForAddressables()}.prefab"));
                UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(filter.gameObject, prefabPath,
                    InteractionMode.AutomatedAction);

                AssetDatabase.ImportAsset(partPath);
                AssetDatabase.ImportAsset(prefabPath);
            }
        } finally {
            AssetDatabase.StopAssetEditing();
        }
    }

    private static void ExtractGroundTextures(string mapName,
        Transform groundMeshes,
        out Texture2D lightmapTexture,
        out Texture2D tintmapTexture,
        out Texture2D mainTex,
        out Material material
    ) { // Extract first textures only
        var firstMesh = groundMeshes.transform;
        firstMesh.gameObject.SetActive(true);
        var firstMeshMeshPath = Path.Combine(GetBasePath(), mapName, "ground");
        Directory.CreateDirectory(firstMeshMeshPath);

        material = firstMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material;

        mainTex = material.GetTexture("_MainTex") as Texture2D;
        lightmapTexture = material.GetTexture("_Lightmap") as Texture2D;
        tintmapTexture = material.GetTexture("_Tintmap") as Texture2D;

        if (mainTex != null) {
            if (!mainTex.isReadable) {
                mainTex = DuplicateTexture(mainTex);
            }

            var path = Path.Combine(firstMeshMeshPath, "texture.png");
            var bytes = mainTex.EncodeToPNG();
            File.WriteAllBytes(path, bytes);

            AssetDatabase.ImportAsset(path);
            mainTex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
            material.SetTexture("_MainTex", mainTex);
        }

        if (lightmapTexture != null) {
            if (!lightmapTexture.isReadable) {
                lightmapTexture = DuplicateTexture(lightmapTexture);
            }

            var path = Path.Combine(firstMeshMeshPath, "lightmap.png");
            var bytes = lightmapTexture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);

            AssetDatabase.ImportAsset(path);
            lightmapTexture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
            material.SetTexture("_Lightmap", lightmapTexture);
        }

        if (tintmapTexture != null) {
            if (!tintmapTexture.isReadable) {
                tintmapTexture = DuplicateTexture(tintmapTexture);
            }

            var path = Path.Combine(firstMeshMeshPath, "tintmap.png");
            var bytes = tintmapTexture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);

            AssetDatabase.ImportAsset(path);
            tintmapTexture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
            material.SetTexture("_Tintmap", tintmapTexture);
        }

        var materialPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(firstMeshMeshPath, $"{mapName}.mat"));
        AssetDatabase.CreateAsset(material, materialPath);
    }

    private static List<string> ExtractOriginalModels(GameObject mapObject, string overridePath = null) {
        var originalMeshes = mapObject.transform.FindRecursive("_Originals");
        /**
         * we need to set the new prefabs to another parent so we can delete the old meshes
         */
        var newOriginalParent = new GameObject("_Original");
        newOriginalParent.transform.SetParent(originalMeshes.transform.parent);

        var children = originalMeshes.transform.GetChildren();
        var texturePaths = new List<string>();

        var i = 0;
        foreach (var mesh in children) {
            var progress = i * 1f / originalMeshes.transform.childCount;
            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving model meshes - {progress * 100}%",
                    progress)) {
                break;
            }

            mesh.gameObject.SetActive(true);

            try {
                var textures = ExtractMesh(mesh.gameObject, newOriginalParent.transform, overridePath);
                texturePaths.AddRange(textures);
            } catch (Exception e) {
                Debug.LogError(e);
                Debug.LogError($"Error extracting model {mesh.gameObject.name}");
            } finally {
                i++;
            }
        }

        EditorUtility.ClearProgressBar();
        return texturePaths;
    }

    public static List<string> ExtractMesh(GameObject mesh, Transform overrideParent = null, string overridePath = null) {
        var nodeTexturesPath = new List<string>();
        string meshPathWithoutExtension;
        if (Path.GetExtension(mesh.name).Length == 0) {
            meshPathWithoutExtension = mesh.name;
        } else {
            meshPathWithoutExtension = mesh.name.Substring(0, mesh.name.IndexOf(Path.GetExtension(mesh.name)));
        }

        string meshPath;
        if (mesh.name.Contains("data/model")) {
            meshPath = Path.Combine(GetBasePath(), meshPathWithoutExtension);
        } else {
            meshPath = Path.Combine(GetBasePath(), "data", "model", meshPathWithoutExtension);
        }

        if (overridePath != null) {
            meshPath = Path.Combine(overridePath, meshPathWithoutExtension);
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
                    if (!node.TryGetComponent<NodeAnimation>(out var anim)) {
                        //GameObjectUtility.SetStaticEditorFlags(node.gameObject, StaticEditorFlags.BatchingStatic);
                        node.gameObject.isStatic = true;
                    }

                    var filter = node.GetComponent<MeshFilter>();
                    var material = node.GetComponent<MeshRenderer>().material;

                    var nodeName = node.mainName.Length == 0 ? "node" : node.mainName;
                    var partPath =
                        AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath,
                            $"{nodeName}_{node.nodeId}.asset"));
                    var materialPath =
                        AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshPath, $"{nodeName}_{node.nodeId}.mat"));

                    var texture = FileManager.Load($"data/texture/{node.textureName}") as Texture2D;
                    var texturePath = Path.Combine(meshPath, $"{nodeName}_{node.nodeId}.png");
                    nodeTexturesPath.Add(texturePath);
                    File.WriteAllBytes(texturePath, texture.EncodeToPNG());

                    AssetDatabase.CreateAsset(filter.mesh, partPath);
                    AssetDatabase.CreateAsset(material, materialPath);
                }

                meshPath = AssetDatabase.GenerateUniqueAssetPath(meshPath + ".prefab");
                PrefabUtility.SaveAsPrefabAssetAndConnect(mesh, meshPath, InteractionMode.AutomatedAction);
            } catch (Exception) {
                Debug.LogError($"Failed extracting model {mesh.name}");
            }
        }

        return nodeTexturesPath;
    }

    private static void ExtractClonedModels(GameObject mapObject, string overridePath = null) {
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

            var meshPath = "";
            if (overridePath != null) {
                meshPath = Path.Combine(overridePath, meshPathWithoutExtension);
            } else {
                meshPath = Path.Combine(GetBasePath(), "data", "model", meshPathWithoutExtension);
            }

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
            var meshPathWithoutExtension =
                mesh.name.Substring(0, originalMeshName.IndexOf(Path.GetExtension(originalMeshName)));

            var prefab =
                UnityEditor.PrefabUtility.InstantiatePrefab(originalPrefabs[meshPathWithoutExtension], cloned.transform)
                    as GameObject;
            prefab.transform.SetPositionAndRotation(mesh.transform.position, mesh.transform.rotation);
            prefab.transform.localScale = mesh.transform.localScale;
        }

        GameObject.DestroyImmediate(clonedMeshes.gameObject);
        //GameObject.DestroyImmediate(originalMeshes.gameObject);
    }

    private void OnEnable() {
        var data = EditorPrefs.GetString("ROMapExtractorWindow", JsonUtility.ToJson(this, false));
        // Then we apply them to this window
        JsonUtility.FromJsonOverwrite(data, this);

        if (grfPaths.Count > 0 && grfRootPath.Length > 0) {
            LoadGRF();
        }

        GrfReordableList = new ReorderableList(grfPaths, typeof(string));
        GrfReordableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "GRF List");
        GrfReordableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            rect.y += 2f;
            rect.height = EditorGUIUtility.singleLineHeight;

            GrfReordableList.list[index] = EditorGUI.TextField(rect, (string)GrfReordableList.list[index]);
        };
    }

    private void OnDisable() {
        var data = JsonUtility.ToJson(this, false);
        // And we save it
        EditorPrefs.SetString("ROMapExtractorWindow", data);
    }

    private void OnGUI() {
        GUILayout.Space(8);
        GUILayout.Label("GRF Settings", EditorStyles.boldLabel);
        grfRootPath = EditorGUILayout.TextField("GRF Root Path", grfRootPath);
        GUILayout.Space(8);
        GrfReordableList.DoLayoutList();

        if (GUILayout.Button("Load GRF")) {
            LoadGRF();
        }

        GUILayout.Space(16);
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        mapName = EditorGUILayout.TextField("Map name", mapName);
        GUILayout.Space(8);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Map")) {
            LoadMap();
        }

        if (GUILayout.Button("Save Map") && CurrentGameMap != null) {
            SaveMap(CurrentGameMap.gameObject);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void LoadGRF() {
        if (grfPaths != null && grfPaths.Count > 0) {
            FileManager.LoadGRF(grfRootPath, grfPaths.Where(it => it.Length > 0).ToList());
        }
    }

    private void OnInspectorUpdate() {
        Repaint();
    }

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
}
#endif