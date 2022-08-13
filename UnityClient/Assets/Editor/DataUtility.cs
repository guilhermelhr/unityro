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
    public static string GENERATED_ADDRESSABLES_PATH = Path.Combine("Assets", "_Generated", "AddressablesAssets");

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
            foreach (var path in textureDescriptors) {
                try {
                    var progress = file / textureDescriptors.Count;
                    if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting texture {file} of {textureDescriptors.Count}\t\t{progress * 100}%", progress)) {
                        break;
                    }

                    var filename = Path.GetFileName(path);
                    var filenameWithoutExtension = Path.GetFileNameWithoutExtension(path).SanitizeForAddressables();
                    var dir = path.Substring(0, path.IndexOf(filename)).Replace("/", "\\");

                    string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, dir);

                    Directory.CreateDirectory(assetPath);

                    var texture = FileManager.Load(path) as Texture2D;
                    texture.alphaIsTransparency = true;
                    var bytes = texture.EncodeToPNG();
                    var completePath = Path.Combine(assetPath, filenameWithoutExtension + ".png");
                    File.WriteAllBytes(completePath, bytes);

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
        try {
            var shouldContinue = true;
            var config = ConfigurationLoader.Init();
            FileManager.LoadGRF(config.root, config.grf);
            var descriptors = FilterDescriptors(FileManager.GetFileDescriptors(), "data/sprite/")
                .Select(it => it[..it.IndexOf(Path.GetExtension(it))].Replace("/", "\\"))
                .Where(it => it.Length > 0)
                .Distinct()
                .ToList();

            /**
             * First we bulk create all the assets and the atlas textures
             * Then we tell Unity to refresh its asset database
             * After that we go over each sprite and convert its atlas texture to a proper Sprite format 
             * and slice every subsprite on the atlas
             */

            #region Extract atlases and acts
            AssetDatabase.StartAssetEditing();
            var spriteDataPaths = new List<string>();
            var spritesList = new List<List<Sprite>>();
            for (int i = 0; i < descriptors.Count; i++) {
                var progress = i * 1f / descriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting sprites {i} of {descriptors.Count}\t\t{progress * 100}%", progress)) {
                    shouldContinue = false;
                    break;
                }

                try {
                    var descriptor = descriptors[i];
                    var sprPath = descriptor + ".spr";
                    var memoryReader = FileManager.ReadSync(descriptor + ".spr");

                    if (memoryReader == null) {
                        Debug.LogError($"Failed to extract {descriptor}");
                        continue;
                    }

                    var spr = memoryReader.ToArray();
                    var act = FileManager.Load(descriptor + ".act") as ACT;
                    var sprLoader = new CustomSpriteLoader();

                    var filename = Path.GetFileName(sprPath);
                    var filenameWithoutExtension = Path.GetFileNameWithoutExtension(sprPath);
                    var dir = sprPath.Substring(0, sprPath.IndexOf(filename)).Replace("/", "\\");
                    string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, dir);

                    Directory.CreateDirectory(assetPath);

                    var spriteData = ScriptableObject.CreateInstance<SpriteData>();
                    sprLoader.Load(spr, filename);
                    spriteData.act = act;
                    spritesList.Add(sprLoader.Sprites);

                    var atlas = sprLoader.Atlas;
                    atlas.alphaIsTransparency = true;
                    var bytes = atlas.EncodeToPNG();

                    var spritePath = Path.Combine(assetPath, filenameWithoutExtension);

                    var atlasPath = spritePath + ".png";
                    File.WriteAllBytes(atlasPath, bytes);

                    var fullAssetPath = spritePath + ".asset";
                    AssetDatabase.CreateAsset(spriteData, fullAssetPath);
                    spriteDataPaths.Add(spritePath);
                } catch (Exception ex) {
                    Debug.LogError($"Failed extracting sprites {ex}");
                }
            }

            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
            #endregion

            if (!shouldContinue) {
                return;
            }

            #region Post process atlases
            AssetDatabase.StartAssetEditing();

            var dataList = new List<SpriteData>();
            for (var i = 0; i < spriteDataPaths.Count; i++) {
                var progress = i * 1f / spriteDataPaths.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Post processing sprites {i} of {spriteDataPaths.Count}\t\t{progress * 100}%", progress)) {
                    break;
                }

                try {
                    var spritePath = spriteDataPaths[i];
                    var sprites = spritesList[i];

                    var spriteData = AssetDatabase.LoadAssetAtPath(spritePath + ".asset", typeof(SpriteData)) as SpriteData;
                    dataList.Add(spriteData);

                    if (spriteData != null) {
                        TextureImporter importer = AssetImporter.GetAtPath(spritePath + ".png") as TextureImporter;
                        importer.textureType = TextureImporterType.Sprite;
                        importer.spriteImportMode = SpriteImportMode.Multiple;
                        var textureSettings = new TextureImporterSettings();
                        importer.ReadTextureSettings(textureSettings);
                        textureSettings.spriteMeshType = SpriteMeshType.FullRect;

                        var sheetMetaData = sprites.Select(it => {
                            return new SpriteMetaData {
                                rect = it.rect,
                                name = it.name
                            };
                        }).ToArray();

                        importer.spritesheet = sheetMetaData;
                        importer.SetTextureSettings(textureSettings);
                        importer.SaveAndReimport();
                        //AssetDatabase.WriteImportSettingsIfDirty(spritePath + ".png"); //Doesnt seem to do anything
                    } else {
                        Debug.LogError($"Failed to load atlas of {spritePath}");
                    }
                } catch (Exception ex) {
                    Debug.LogError($"Failed post processing sprite {ex}");
                }
            }

            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
            #endregion

            #region Assign sprites to SpriteData
            AssetDatabase.StartAssetEditing();

            for (int i = 0; i < spriteDataPaths.Count; i++) {
                try {
                    var spritePath = spriteDataPaths[i];
                    var spriteData = dataList[i];
                    var savedSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath + ".png")
                           .Where(it => it is Sprite)
                           .Select(it => it as Sprite)
                           .ToArray();
                    if (savedSprites.Length > 0) {
                        spriteData.sprites = savedSprites;
                    } else {
                        Debug.LogError($"Failed to load sprites of {spritePath}");
                    }
                    EditorUtility.SetDirty(spriteData);
                } catch (Exception ex) {
                    Debug.LogError($"Failed wrapping up sprite {ex}");
                }
            }

            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
            #endregion

            AssetDatabase.SaveAssets();
        } catch (Exception ex) {
            Debug.LogError(ex);
        } finally {
            EditorUtility.ClearProgressBar();
        }
    }

    [MenuItem("UnityRO/Generate Assets/1. Extract Assets")]
    static void ExtractAssets() {
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Textures");
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Sprites");
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Prepare/Models");

        // Extract effects
        // Generate map prefabs
    }

    [MenuItem("UnityRO/Generate Assets/2. Create Addressable Assets")]
    static void CreateAddressableAssets() {
        var textures = Resources.LoadAll(Path.Join("data", "texture")).ToList();
        textures.SetAddressableGroup("Textures", "Textures");

        var models = Resources.LoadAll(Path.Join("data", "model")).ToList();
        models.SetAddressableGroup("Models", "Models");

        var sprites = Resources.LoadAll(Path.Join("data", "sprite"))
            .Where(it => it is Texture2D || it is SpriteData) // filter out the thousands of sprites we've created
            .ToList();
        sprites.SetAddressableGroup("Sprites", "Sprites");
    }

    [MenuItem("UnityRO/Generate Assets/3. Rename Generated Resources folder")]
    static void RanameGeneratedResourcesFolder() {
        /**
         * This exists because Unity will pack anything under ../Resources/..
         * So we must rename the folder to any other name other than Resources
         * (That's what the addressables system does when you drag and drop a file to it)
         */
        Directory.Move(GENERATED_RESOURCES_PATH, GENERATED_ADDRESSABLES_PATH);
        AssetDatabase.Refresh();
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