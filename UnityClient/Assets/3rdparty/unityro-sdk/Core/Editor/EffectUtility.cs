using ROIO;
using ROIO.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ROIO.Models.FileTypes;
using UnityEditor;
using UnityEngine;
using UnityRO.Core.Extensions;

public class EffectUtility {
    private static string GENERATED_RESOURCES_PATH = Path.Combine("Assets", "Resources", "Effects");
    private static string DEFAULT_EFFECT_DIR = Path.Combine("data", "texture", "effect") + Path.DirectorySeparatorChar;

    [MenuItem("UnityRO/Utils/Extract/Effects/STR")]
    static void ExtractSTREffects() {
        FileManager.LoadGRF("D:\\Projetos\\ragnarok\\test\\", new List<string> { "kro_data.grf" });
        //FileManager.LoadGRF("../../ragnarok/", new List<string> { "data.grf" });

        try {
            var descriptors = DataUtility
                .FilterDescriptors(FileManager.GetFileDescriptors(), "data/texture/effect")
                .Where(it => Path.GetExtension(it) == ".str")
                .ToList();

            for (var i = 0; i < descriptors.Count; i++) {
                var progress = i * 1f / descriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO",
                        $"Extracting effects {i} of {descriptors.Count}\t\t{progress * 100}%",
                        progress)) {
                    break;
                }

                try {
                    ExtractStr(descriptors[i]);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        } finally {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("UnityRO/Utils/Extract/Effects/SPR")]
    static void ExtractSPREffects() {
        FileManager.LoadGRF("D:\\Projetos\\ragnarok\\test\\", new List<string> { "kro_data.grf" });
        //FileManager.LoadGRF("../../ragnarok/", new List<string> { "data.grf" });

        try {
            var descriptors = DataUtility
                .FilterDescriptors(FileManager.GetFileDescriptors(), "data/sprite/ÀÌÆÑÆ®")
                .Where(it => Path.GetExtension(it) is ".str" or ".act" or ".spr")
                .ToList();

            for (var i = 0; i < descriptors.Count; i++) {
                var progress = i * 1f / descriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO",
                        $"Extracting effects {i} of {descriptors.Count}\t\t{progress * 100}%",
                        progress)) {
                    break;
                }

                try {
                    var descriptor = descriptors[i];
                    switch (Path.GetExtension(descriptor)) {
                        case ".spr":
                            ExtractSpr(descriptor);
                            break;
                        case ".str":
                            ExtractStr(descriptors[i]);
                            break;
                    }
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        } finally {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("UnityRO/Utils/Extract/Effects/Everything else")]
    static void ExtractTextureEffects() {
        FileManager.LoadGRF("D:\\Projetos\\ragnarok\\test\\", new List<string> { "kro_data.grf" });
        //FileManager.LoadGRF("../../ragnarok/", new List<string> { "data.grf" });

        try {
            AssetDatabase.StartAssetEditing();
            var descriptors = DataUtility
                .FilterDescriptors(FileManager.GetFileDescriptors(), "data/texture/effect")
                .Where(it =>
                    Path.GetDirectoryName(it) == Path.Combine("data", "texture", "effect") &&
                    Path.GetExtension(it) != ".str")
                .ToList();

            for (var i = 0; i < descriptors.Count; i++) {
                var progress = i * 1f / descriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO",
                        $"Extracting effects {i} of {descriptors.Count}\t\t{progress * 100}%",
                        progress)) {
                    break;
                }

                try {
                    ExtractTexture(descriptors[i]);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        } finally {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    private static void ExtractTexture(string descriptor) {
        var texture = FileManager.Load(descriptor) as Texture2D;
        if (texture == null) throw new Exception();

        var filenameWithoutExtension =
            Path.GetFileNameWithoutExtension(descriptor).SanitizeForAddressables();
        var dir = Path.GetDirectoryName(descriptor.Replace('/', Path.DirectorySeparatorChar)
            .Replace(DEFAULT_EFFECT_DIR, ""));

        var assetPath = Path.Combine(GENERATED_RESOURCES_PATH, "Textures", dir);
        Directory.CreateDirectory(assetPath);

        var bytes = texture.EncodeToPNG();
        File.WriteAllBytes($"{assetPath}/{filenameWithoutExtension}.png", bytes);
        AssetDatabase.ImportAsset($"{assetPath}/{filenameWithoutExtension}.png");
    }

    private static void ExtractStr(string descriptor) {
        var strEffect = EffectLoader.Load(FileManager.ReadSync(descriptor),
            Path.GetDirectoryName(descriptor).Replace("\\", "/"),
            path => FileManager.Load(path) as Texture2D);

        if (strEffect == null) return;

        var filenameWithoutExtension =
            Path.GetFileNameWithoutExtension(descriptor).SanitizeForAddressables();
        strEffect.name = filenameWithoutExtension;
        var dir = Path.GetDirectoryName(descriptor.Replace('/', Path.DirectorySeparatorChar)
            .Replace(DEFAULT_EFFECT_DIR, ""));

        var assetPath = Path.Combine(GENERATED_RESOURCES_PATH, "STR", dir);
        Directory.CreateDirectory(assetPath);

        var atlas = strEffect.Atlas;
        var bytes = atlas.EncodeToPNG();
        File.WriteAllBytes($"{assetPath}/{filenameWithoutExtension}.png", bytes);
        AssetDatabase.ImportAsset($"{assetPath}/{filenameWithoutExtension}.png");
        var diskAtlas =
            AssetDatabase.LoadAssetAtPath<Texture2D>($"{assetPath}/{filenameWithoutExtension}.png");
        strEffect._Atlas = diskAtlas;

        strEffect.name = filenameWithoutExtension;

        var completePath = Path.Combine(assetPath, filenameWithoutExtension + ".asset");
        AssetDatabase.CreateAsset(strEffect, completePath);
    }

    private static void ExtractSpr(string descriptor) {
        var baseFileName = descriptor.Replace(".spr", "");

        var sprPath = baseFileName + ".spr";
        var actPath = baseFileName + ".act";

        var sprBytes = FileManager.ReadSync(sprPath).ToArray();
        var act = FileManager.Load(actPath) as ACT;

        var spriteLoader = new CustomSpriteLoader();
        var filename = Path.GetFileNameWithoutExtension(descriptor);

        var dir = Path.GetDirectoryName(descriptor.Replace('/', Path.DirectorySeparatorChar)
            .Replace("data\\sprite\\ÀÌÆÑÆ®\\", ""));
        var assetPath = Path.Combine(GENERATED_RESOURCES_PATH, "SPR", dir);
        var spriteDataPath = Path.Combine(assetPath, filename);
        Directory.CreateDirectory(assetPath);

        var spriteData = ScriptableObject.CreateInstance<SpriteData>();
        spriteData.act = act;

        spriteLoader.Load(sprBytes, filename, false);

        var atlas = spriteLoader.Atlas;
        var bytes = atlas.EncodeToPNG();
        var atlasPath = spriteDataPath + ".png";
        File.WriteAllBytes(atlasPath, bytes);

        AssetDatabase.ImportAsset(spriteDataPath + ".png");

        // transform texture into multiple sprites texture
        var importer = AssetImporter.GetAtPath(spriteDataPath + ".png") as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        var textureSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(textureSettings);
        textureSettings.spriteMeshType = SpriteMeshType.FullRect;
        textureSettings.spritePixelsPerUnit = SPR.PIXELS_PER_UNIT;

        var sheetMetaData = spriteLoader.Sprites.Select(it => new SpriteMetaData {
            rect = it.rect,
            name = it.name
        }).ToArray();

        importer.spritesheet = sheetMetaData;
        importer.SetTextureSettings(textureSettings);
        importer.SaveAndReimport();

        AssetDatabase.ImportAsset(spriteDataPath + ".png");

        spriteData.sprites = AssetDatabase.LoadAllAssetsAtPath(spriteDataPath + ".png").OfType<Sprite>().ToArray();

        var fullAssetPath = spriteDataPath + ".asset";
        AssetDatabase.CreateAsset(spriteData, fullAssetPath);
    }
}