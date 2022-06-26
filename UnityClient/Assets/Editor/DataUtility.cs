using Assets.Scripts.Renderer.Sprite;
using ROIO;
using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
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

    [MenuItem("UnityRO/Generate Unity assets")]
    static void GenerateAddressablesResources() {
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Textures");
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Prepare/Models");
        
        //TODO
        // Extract sprites
        // Extract effects
        // Generate map prefabs
    }

    private static string ExtractFile(string path) {
        if (!path.StartsWith("data/texture/")) {
            return null;
        }
        var filename = Path.GetFileName(path);
        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(path);
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