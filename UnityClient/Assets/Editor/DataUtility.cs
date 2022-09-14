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
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class DataUtility {

    private static Configuration config;

    static DataUtility() {
        config = ConfigurationLoader.Init();
        FileManager.LoadGRF(config.root, config.grf);
        Debug.Log("Done initializing grf");
    }

    public static string GENERATED_RESOURCES_PATH = Path.Combine("Assets", "_Generated", "Resources");
    public static string GENERATED_ADDRESSABLES_PATH = Path.Combine("Assets", "_Generated", "AddressablesAssets");

    [MenuItem("UnityRO/Utils/Extract/Textures")]
    static void ExtractTextures() {
        var textureDescriptors = FilterDescriptors(FileManager.GetFileDescriptors(), "data/texture").ToList();
        var shouldContinue = true;

        try {
            // This disable Unity's auto update of assets
            // Making it much faster to batch create files like we're about to do
            AssetDatabase.StartAssetEditing();

            var file = 1f;
            foreach (var descriptor in textureDescriptors) {
                try {
                    var progress = file / textureDescriptors.Count;
                    if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting texture {file} of {textureDescriptors.Count}\t\t{progress * 100}%", progress)) {
                        shouldContinue = false;
                        break;
                    }

                    var filename = Path.GetFileName(descriptor);
                    var filenameWithoutExtension = Path.GetFileNameWithoutExtension(descriptor).SanitizeForAddressables();
                    var dir = Path.GetDirectoryName(descriptor);

                    string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, dir);

                    Directory.CreateDirectory(assetPath);

                    var texture = FileManager.Load(descriptor) as Texture2D;
                    if (texture != null) {
                        texture.alphaIsTransparency = true;
                        var bytes = texture.EncodeToPNG();
                        var completePath = Path.Combine(assetPath, filenameWithoutExtension + ".png");
                        File.WriteAllBytes(completePath, bytes);
                    }
                    file++;
                } catch (Exception e) {
                    Debug.LogError(e);
                }
            }
        } finally {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }

        if (!shouldContinue) {
            return;
        }

        MakeTexturesReadable();
    }

    [MenuItem("UnityRO/Utils/Textures/Make Textures Readable")]
    private static void MakeTexturesReadable() {
        var texturesPaths = GetFilesFromDir(Path.Combine(GENERATED_RESOURCES_PATH, "data", "texture"));
        try {
            AssetDatabase.StartAssetEditing();

            for (int i = 0; i < texturesPaths.Length; i++) {
                var progress = i / texturesPaths.Length;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Processing textures {i} of {texturesPaths.Length}\t\t{progress * 100}%", progress)) {
                    break;
                }

                var path = texturesPaths[i];
                var tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                if (tImporter != null) {
                    tImporter.textureType = TextureImporterType.Default;
                    tImporter.isReadable = true;
                    AssetDatabase.ImportAsset(path);
                }
            }

        } finally {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
    }

    [MenuItem("UnityRO/Utils/Extract/Sprites")]
    static void ExtractSprites() {
        try {
            var shouldContinue = true;
            var descriptors = FilterDescriptors(FileManager.GetFileDescriptors(), "data/sprite/")
                .Select(it => it[..it.IndexOf(Path.GetExtension(it))])
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
                    var dir = sprPath.Substring(0, sprPath.IndexOf(filename));
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

    [MenuItem("UnityRO/Utils/Extract/Lua Files")]
    static void ExtractLuaFiles() {
        /**
         * These files are the ones I could find on an experiment
         * where I put a script to log the name of the file when the OG client
         * was loading. So apparently this is the order they load and these are
         * the only files being actually used
         */
        var files = new string[] {
            "PetEvolutionCln_true.lub",
            "achievement_list.lub",
            "PrivateAirplane_true.lub",
            "CheckAttendance.lub",
            "itemInfo_true.lub",
            "tipbox.lub",
            "data/luafiles514/lua files/datainfo/changedirectorylist.lub",
            "data/luafiles514/lua files/msgstring_kr.lub",
            "data/luafiles514/lua files/datainfo/npcidentity.lub",
            "data/luafiles514/lua files/datainfo/jobname_f.lub",
            "data/luafiles514/lua files/datainfo/jobname.lub",
            "data/luafiles514/lua files/datainfo/pcjobnamegender.lub",
            "data/luafiles514/lua files/datainfo/petinfo.lub",
            "data/luafiles514/lua files/datainfo/accessoryid.lub",
            "data/luafiles514/lua files/datainfo/accname_f.lub",
            "data/luafiles514/lua files/datainfo/accname.lub",
            "data/luafiles514/lua files/skillinfoz/jobinheritlist.lub",
            "data/luafiles514/lua files/skillinfoz/skillid.lub",
            "data/luafiles514/lua files/skillinfoz/skillinfolist.lub",
            "data/luafiles514/lua files/skillinfoz/skilldescript.lub",
            "data/luafiles514/lua files/skillinfoz/skillinfo_f.lub",
            "data/luafiles514/lua files/skillinfoz/skilltreeview.lub",
            "data/luafiles514/lua files/stateicon/efstids.lub",
            "data/luafiles514/lua files/stateicon/stateiconinfo.lub",
            "data/luafiles514/lua files/stateicon/stateiconinfo_f.lub",
            "data/luafiles514/lua files/stateicon/stateiconimginfo.lub",
            "LuaFiles514/OptionInfo.lub",
            "data/luafiles514/lua files/optioninfo/cmdinfo.lub",
            "data/luafiles514/lua files/optioninfo/optioninfo_f.lub",
            "data/luafiles514/lua files/datainfo/spriterobeid.lub",
            "data/luafiles514/lua files/datainfo/spriterobename_f.lub",
            "data/luafiles514/lua files/datainfo/spriterobename.lub",
            "data/luafiles514/lua files/datainfo/npclocationradius.lub",
            "data/luafiles514/lua files/datainfo/npclocationradius_f.lub",
            "data/luafiles514/lua files/skilleffectinfo/effectid.lub",
            "data/luafiles514/lua files/skilleffectinfo/actorstate.lub",
            "data/luafiles514/lua files/skilleffectinfo/skilleffectinfolist.lub",
            "data/luafiles514/lua files/skilleffectinfo/skilleffectinfo_f.lub",
            "data/luafiles514/lua files/datainfo/kaframovemapservicelist.lub",
            "data/luafiles514/lua files/datainfo/kaframovemapservicelist_f.lub",
            "data/luafiles514/lua files/navigation/navi_f_krpri.lub",
            "data/luafiles514/lua files/navigation/navi_map_krpri.lub",
            "data/luafiles514/lua files/navigation/navi_npc_krpri.lub",
            "data/luafiles514/lua files/navigation/navi_mob_krpri.lub",
            "data/luafiles514/lua files/navigation/navi_link_krpri.lub",
            "data/luafiles514/lua files/navigation/navi_linkdistance_krpri.lub",
            "data/luafiles514/lua files/navigation/navi_npcdistance_krpri.lub",
            "data/luafiles514/lua files/navigation/navi_scroll_krpri.lub",
            "data/luafiles514/lua files/navigation/navi_picknpc_krpri.lub",
            "data/luafiles514/lua files/datainfo/helpmsgstr.lub",
            "data/luafiles514/lua files/entryqueue/entryqueuelist.lub",
            "data/luafiles514/lua files/datainfo/weapontable.lub",
            "data/luafiles514/lua files/datainfo/weapontable_f.lub",
            "data/luafiles514/lua files/datainfo/jobidentity.lub",
            "data/luafiles514/lua files/datainfo/shadowtable.lub",
            "data/luafiles514/lua files/datainfo/shadowtable_f.lub",
            "data/luafiles514/lua files/worldviewdata/worldviewdata_language.lub",
            "data/luafiles514/lua files/worldviewdata/worldviewdata_list.lub",
            "data/luafiles514/lua files/worldviewdata/worldviewdata_table.lub",
            "data/luafiles514/lua files/worldviewdata/worldviewdata_f.lub",
            "data/luafiles514/lua files/worldviewdata/worldviewdata_info.lub",
            "data/luafiles514/lua files/datainfo/enumvar.lub",
            "data/luafiles514/lua files/datainfo/addrandomoptionnametable.lub",
            "data/luafiles514/lua files/datainfo/addrandomoption_f.lub",
            "data/luafiles514/lua files/dressroom/dress_f.lub",
            "data/luafiles514/lua files/dressroom/jobdresslist.lub",
            "data/luafiles514/lua files/datainfo/titletable.lub",
            "data/luafiles514/lua files/hateffectinfo/hateffectinfo.lub",
            "data/luafiles514/lua files/signboardlist.lub",
            "data/luafiles514/lua files/effecttool/forcerendereffect.lub",
            "data/luafiles514/lua files/datainfo/lapineddukddakbox.lub",
            "data/luafiles514/lua files/datainfo/LapineUpgradeBox.lub",
            "data/luafiles514/lua files/transparentItem/transparentItem.lub",
            "data/luafiles514/lua files/transparentItem/transparentItem_f.lub",
            "data/luafiles514/lua files/service_brazil/ExternalSettings_br.lub",
            "data/luafiles514/lua files/datainfo/TB_Layer_Priority.lub",
            "data/luafiles514/lua files/datainfo/tb_cashshop_banner.lub",
            "mapInfo_true.lub",
            "Towninfo.lub",
            "data/luafiles514/lua files/datainfo/questinfo_f.lub",
            "RecommendedQuestInfoList_True.lub",
        };

        try {
            AssetDatabase.StartAssetEditing();

            foreach (var lua in files) {
                try {
                    string luaFileString;
                    if (lua.StartsWith("data/lua")) {
                        luaFileString = FileManager.ReadSync(lua, System.Text.Encoding.GetEncoding(1252)).ReadToEnd();
                    } else {
                        luaFileString = new StreamReader(Path.Combine(config.SystemPath, lua), System.Text.Encoding.GetEncoding(1252)).ReadToEnd();
                    }
                    var path = Path.Combine(GENERATED_RESOURCES_PATH, "lua", Path.GetDirectoryName(lua));
                    Directory.CreateDirectory(path);

                    File.WriteAllText(Path.Combine(path, Path.GetFileName(lua) + ".txt"), luaFileString);
                } catch (Exception e) {
                    Debug.LogError($"Couldnt load file {lua} {e}");
                }
            }
        } finally {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("UnityRO/Utils/Extract/Txt Tables")]
    static void ExtractTables() {
        try {
            var descriptors = FilterDescriptors(FileManager.GetFileDescriptors(), "data")
                .Where(it => Path.GetExtension(it) == ".txt")
                .ToList();

            AssetDatabase.StartAssetEditing();

            foreach (var descriptor in descriptors) {
                try {
                    string table = FileManager.Load(descriptor) as string;
                    var path = Path.Combine(GENERATED_RESOURCES_PATH, "txt", Path.GetDirectoryName(descriptor));
                    Directory.CreateDirectory(path);

                    File.WriteAllText(Path.Combine(path, Path.GetFileName(descriptor) + ".txt"), table);
                } catch (Exception e) {
                    Debug.LogError($"Couldnt load file {descriptor} {e}");
                }
            }
        } finally {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("UnityRO/Utils/Extract/Effects")]
    async static void ExtractEffects() {
        AssetDatabase.StartAssetEditing();

        try {
            var descriptors = FilterDescriptors(FileManager.GetFileDescriptors(), "data/texture/effect")
                .Where(it => Path.GetExtension(it) == ".str")
                .ToList();

            for (int i = 0; i < descriptors.Count; i++) {
                var progress = i * 1f / descriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting effects {i} of {descriptors.Count}\t\t{progress * 100}%", progress)) {
                    break;
                }

                try {
                    var descriptor = descriptors[i];
                    var strEffect = await EffectLoader.Load(FileManager.ReadSync(descriptor), Path.GetDirectoryName(descriptor).Replace("\\", "/"));

                    if (strEffect != null) {
                        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(descriptor).SanitizeForAddressables();
                        var dir = Path.GetDirectoryName(descriptor);

                        string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, dir);
                        Directory.CreateDirectory(assetPath);

                        var completePath = Path.Combine(assetPath, filenameWithoutExtension + ".asset");
                        AssetDatabase.CreateAsset(strEffect, completePath);
                    }
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

    [MenuItem("UnityRO/Utils/Extract/BGM")]
    static void ExtractBGM() {
        AssetDatabase.StartAssetEditing();

        try {
            var descriptors = GetFilesFromDir(config.BgmPath).ToList();

            for (int i = 0; i < descriptors.Count; i++) {
                var progress = i * 1f / descriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting bgm {i} of {descriptors.Count}\t\t{progress * 100}%", progress)) {
                    break;
                }

                try {
                    var descriptor = descriptors[i];
                    var bgm = File.ReadAllBytes(descriptor);

                    if (bgm != null) {
                        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(descriptor).SanitizeForAddressables();

                        string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, "bgm");
                        Directory.CreateDirectory(assetPath);

                        var completePath = Path.Combine(assetPath, filenameWithoutExtension + ".mp3");
                        File.WriteAllBytes(completePath, bgm);
                    }
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

    [MenuItem("UnityRO/Utils/Extract/Wav")]
    static void ExtractWav() {
        var descriptors = FilterDescriptors(FileManager.GetFileDescriptors(), "data/wav")
            .Where(it => Path.GetExtension(it) == ".wav")
            .ToList();

        try {
            AssetDatabase.StartAssetEditing();
            for (int i = 0; i < descriptors.Count; i++) {
                var progress = i * 1f / descriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting wavs {i} of {descriptors.Count}\t\t{progress * 100}%", progress)) {
                    break;
                }

                var descriptor = descriptors[i];
                try {
                    var audioClip = FileManager.Load(descriptor) as AudioClip;

                    if (audioClip != null) {
                        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(descriptor).SanitizeForAddressables();
                        var dir = Path.GetDirectoryName(descriptor);
                        string assetPath = Path.Combine(GENERATED_RESOURCES_PATH, dir);
                        Directory.CreateDirectory(assetPath);

                        var completePath = Path.Combine(assetPath, filenameWithoutExtension + ".asset");
                        AssetDatabase.CreateAsset(audioClip, completePath);
                    }
                } catch (Exception e) {
                    Debug.LogError($"Failed to extract {descriptor} {e}");
                }
            }
        } finally {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("UnityRO/1. Extract Assets")]
    static void ExtractAssets() {
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Textures");
        //This has to be done here so the models can load their textures
        CreateTexturesAddressableAssets();

        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Sprites");
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Lua Files");
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Effects");
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Wav");
        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Extract/Txt Tables");

        EditorApplication.ExecuteMenuItem("UnityRO/Utils/Prepare/Models");

        // Generate map prefabs
    }

    [MenuItem("UnityRO/2. Check for missing assets")]
    static void CheckForMissingAssets() {
        AssetDatabase.Refresh();

        var descriptors = FileManager.GetFileDescriptors();

        var textureDescriptors = FilterDescriptors(descriptors, "data/texture")
            .Select(it => {
                var dir = Path.GetDirectoryName(it);
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(it);

                return Path.Combine(GENERATED_RESOURCES_PATH, dir, filenameWithoutExtension + ".png");
            }).ToList();

        var modelDescriptors = FilterDescriptors(descriptors, "data/model")
            .Where(it => Path.GetExtension(it) == ".rsm")
            .Select(it => {
                var dir = Path.GetDirectoryName(it);
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(it);

                return Path.Combine(GENERATED_RESOURCES_PATH, dir, filenameWithoutExtension + ".prefab");
            }).ToList();

        var spriteDescriptors = FilterDescriptors(descriptors, "data/sprite")
            .Select(it => {
                var dir = Path.GetDirectoryName(it);
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(it);

                return Path.Combine(GENERATED_RESOURCES_PATH, dir, filenameWithoutExtension + ".asset");
            }).ToList();

        List<string> missingTextures = new List<string>(textureDescriptors.Count);
        Parallel.ForEach(textureDescriptors, (descriptor) => {
            if (!File.Exists(descriptor)) {
                lock (missingTextures) {
                    missingTextures.Add(descriptor);
                }
            }
        });

        File.WriteAllLines("Assets/Logs/missing-textures.txt", missingTextures);
        Debug.LogError($"{missingTextures.Count} out of {textureDescriptors.Count} textures not found. Full list saved to Assets/Logs/missing-textures.txt");

        List<string> missingModels = new List<string>(modelDescriptors.Count);
        Parallel.ForEach(modelDescriptors, descriptor => {
            if (!File.Exists(descriptor)) {
                lock (missingModels) {
                    missingModels.Add(descriptor);
                }
            }
        });

        File.WriteAllLines("Assets/Logs/missing-models.txt", missingModels);
        Debug.LogError($"{missingModels.Count} out of {modelDescriptors.Count} models not found. Full list saved to Assets/Logs/missing-models.txt");

        List<string> missingSprites = new List<string>(spriteDescriptors.Count);
        Parallel.ForEach(spriteDescriptors, (descriptor) => {
            if (!File.Exists(descriptor)) {
                lock (missingSprites) {
                    missingSprites.Add(descriptor);
                }
            }
        });
        File.WriteAllLines("Assets/Logs/missing-sprites.txt", missingModels);
        Debug.LogError($"{missingSprites.Count} out of {spriteDescriptors.Count} sprites not found. Full list saved to Assets/Logs/missing-sprites.txt");
    }

    [MenuItem("UnityRO/3. Create Addressable Assets/1. All")]
    static void CreateAllAddressableAssets() {
        // textures were already assigned to addressables when you run UnityRO/1. Extract Assets
        EditorApplication.ExecuteMenuItem("UnityRO/3. Create Addressable Assets/3. Models");
        EditorApplication.ExecuteMenuItem("UnityRO/3. Create Addressable Assets/4. Sprites");
        EditorApplication.ExecuteMenuItem("UnityRO/3. Create Addressable Assets/5. Data Tables");
        EditorApplication.ExecuteMenuItem("UnityRO/3. Create Addressable Assets/6. Effects");
        EditorApplication.ExecuteMenuItem("UnityRO/3. Create Addressable Assets/7. Wav");
        EditorApplication.ExecuteMenuItem("UnityRO/3. Create Addressable Assets/8. BGM");
    }

    [MenuItem("UnityRO/3. Create Addressable Assets/2. Textures")]
    static void CreateTexturesAddressableAssets() {
        var textures = Resources.LoadAll(Path.Join("data", "texture")).ToList();
        textures.SetAddressableGroup("Textures", "Textures");
    }

    [MenuItem("UnityRO/3. Create Addressable Assets/3. Models")]
    static void CreateModelsAddressableAssets() {
        var models = Resources.LoadAll(Path.Join("data", "model")).ToList();
        models.SetAddressableGroup("Models", "Models");
    }

    [MenuItem("UnityRO/3. Create Addressable Assets/4. Sprites")]
    static void CreateSpritesAddressableAssets() {
        var sprites = Resources.LoadAll(Path.Join("data", "sprite"))
            .Where(it => it is Texture2D || it is SpriteData) // filter out the thousands of sprites we've created
            .ToList();
        sprites.SetAddressableGroup("Sprites", "Sprites");
    }

    [MenuItem("UnityRO/3. Create Addressable Assets/5. Data Tables")]
    static void CreateDataTablesAddressableAssets() {
        var files = Resources.LoadAll("lua").ToList();
        files.SetAddressableGroup("DataTables", "DataTables");

        var txtTables = Resources.LoadAll("txt").ToList();
        txtTables.SetAddressableGroup("DataTables", "DataTables");
    }

    [MenuItem("UnityRO/3. Create Addressable Assets/6. Effects")]
    static void CreateEffectsAddressableAssets() {
        var files = Resources.LoadAll(Path.Combine("data", "texture", "effect"))
            .Where(it => it is STR)
            .ToList();
        files.SetAddressableGroup("Effects", "Effects");
    }

    [MenuItem("UnityRO/3. Create Addressable Assets/7. Wav")]
    static void CreateWavAddressableAssets() {
        var files = Resources.LoadAll(Path.Combine("data", "wav"))
            .Where(it => it is AudioClip)
            .ToList();
        files.SetAddressableGroup("Wav", "Wav");
    }

    [MenuItem("UnityRO/3. Create Addressable Assets/8. BGM")]
    static void CreateBGMAddressableAssets() {
        var files = Resources.LoadAll("bgm")
            .Where(it => it is AudioClip)
            .ToList();
        files.SetAddressableGroup("BGM", "BGM");
    }

    [MenuItem("UnityRO/4. Rename Generated Resources folder")]
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