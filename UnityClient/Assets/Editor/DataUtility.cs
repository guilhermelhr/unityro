#if UNITY_EDITOR
using Assets.Scripts.Renderer.Sprite;
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

[InitializeOnLoadAttribute]
public class DataUtility {

    public static string GENERATED_RESOURCES_PATH = Path.Combine("Assets", "_Generated", "Resources");
    public static string GENERATED_TEXTURES_PATH = Path.Combine("_Generated", "Resources", "data", "texture");
    public static string GENERATED_SPRITES_PATH = Path.Combine("_Generated", "Resources", "data", "sprite");
    public static string GENERATED_MESHES_PATH = Path.Combine("_Generated", "Resources", "data", "model");
    public static string GENERATED_MAPS_PATH = Path.Combine("_Generated", "Resources", "Prefabs", "Maps");

    [MenuItem("UnityRO/Utils/Extract/Textures")]
    static void ExtractTextures() {
        var config = ConfigurationLoader.Init();
        FileManager.LoadGRF(config.root, config.grf);
        var textureDescriptors = FilterDescriptors(FileManager.GetFileDescriptors(), "data/texture").ToList();

        try {
            // This disable Unity's auto update of assets
            // Making it much faster to batch create files like we're about to do
            AssetDatabase.StartAssetEditing();

            var file = 1f;
            foreach (var entry in textureDescriptors) {
                try {
                    var progress = file / textureDescriptors.Count;
                    if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting texture {file} of {textureDescriptors.Count}\t\t{progress * 100}%", progress)) {
                        break;
                    }

                    string completePath = ExtractFile(entry);

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
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
    }

    [MenuItem("UnityRO/Utils/Extract/Sprites")]
    static void ExtractSprites() {
        var config = ConfigurationLoader.Init();
        FileManager.LoadGRF(config.root, config.grf);
        var descriptors = FilterDescriptors(FileManager.GetFileDescriptors(), "data/sprite").Take(10).ToList();
        var sprDescriptors = descriptors.Where(t => Path.GetExtension(t) == ".spr").ToList();
        var actDescriptors = descriptors.Where(t => Path.GetExtension(t) == ".act").ToList();
        try {
            // This disable Unity's auto update of assets
            // Making it much faster to batch create files like we're about to do
            AssetDatabase.StartAssetEditing();

            for (int i = 0; i < sprDescriptors.Count; i++) {
                var progress = i * 1f / sprDescriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting sprites {i} of {sprDescriptors.Count}\t\t{progress * 100}%", progress)) {
                    break;
                }

                var spriteLoader = new CustomSpriteLoader();
                var sprPath = sprDescriptors[i];
                var actPath = actDescriptors[i];

                var filename = Path.GetFileName(sprPath);
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(sprPath);
                var dir = sprPath.Substring(0, sprPath.IndexOf(filename)).Replace("/", "\\");

                string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, "Sprites", dir);
                Directory.CreateDirectory(assetPath);
                var spriteData = ScriptableObject.CreateInstance<SpriteData>();

                var spriteByteArray = FileManager.ReadSync(sprPath).ToArray();
                spriteLoader.Load(spriteByteArray, filename);

                var sprites = spriteLoader.Sprites;
                var act = FileManager.Load(actPath) as ACT;
                spriteData.act = act;
                spriteData.sprites = sprites.ToArray();
                AssetDatabase.CreateAsset(spriteData, Path.Combine(assetPath, $"{filenameWithoutExtension}.asset"));

                foreach (var sprite in sprites) {
                    AssetDatabase.AddObjectToAsset(sprite, spriteData);
                }
            }
        } finally {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            EditorApplication.ExitPlaymode();
        }
    }

    [MenuItem("UnityRO/Generate Assets/1. Extract Assets")]
    static void ExtractAssets() {
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Textures");
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Prepare/Models");

        //var models = Resources.LoadAll(Path.Join("data", "model"));
        //foreach (var model in models) {
        //    model.SetAddressableGroup("Models");
        //}

        //TODO
        // Extract sprites
        // Extract effects
        // Generate map prefabs
    }

    [MenuItem("UnityRO/Generate Assets/2. Create Addressable Assets")]
    static void CreateAddressableAssets() {
        var textures = Resources.LoadAll(Path.Join("data", "texture")).ToList();
        textures.SetAddressableGroup("Textures", "Textures");
        
        var models = Resources.LoadAll(Path.Join("data", "model")).ToList();
        models.SetAddressableGroup("Models", "Models");
    }

    private static string ExtractFile(string path) {
        if (!path.StartsWith("data/texture/")) {
            return null;
        }
        var filename = Path.GetFileName(path);
        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(path).SanitizeForAddressables();
        var dir = path.Substring(0, path.IndexOf(filename)).Replace("/", "\\");

        string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, dir);

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

    private static string[] GetFilesFromDir(string dir) {
        return Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
            .Where(it => Path.HasExtension(it) && !it.Contains(".meta"))
            .Select(it => it.Replace(Application.dataPath, "Assets"))
            .ToArray();
    }

    private static List<string> FilterDescriptors(Hashtable descriptors, string filter) {
        List<string> result = new List<string>();
        foreach (DictionaryEntry entry in descriptors) {
            string path = (entry.Key as string).Trim();
            if (path.StartsWith(filter)) {
                result.Add(path);
            }
        }

        return result;
    }
}
#endif