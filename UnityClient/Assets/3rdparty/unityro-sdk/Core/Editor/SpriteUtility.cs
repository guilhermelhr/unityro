using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter;
using ROIO;
using ROIO.Loaders;
using ROIO.Models.FileTypes;
using UnityEditor;
using UnityEngine;

public class SpriteUtility {
    private static string UTILS_DIR =
        Path.Combine(Directory.GetCurrentDirectory(), "Assets", "3rdparty", "unityro-core", "Editor", "Utils");

    private const string MALE = "³²";
    private const string FEMALE = "¿©";

    private static string GENERATED_RESOURCES_PATH =
        Path.Combine("Assets", "3rdparty", "unityro-resources", "Resources", "Sprites");

    private static string GENERATED_HEAD_PATH = Path.Combine(GENERATED_RESOURCES_PATH, "Head");
    private static string GENERATED_BODY_PATH = Path.Combine(GENERATED_RESOURCES_PATH, "Body");
    private static string GENERATED_NPC_PATH = Path.Combine(GENERATED_RESOURCES_PATH, "Npc");
    private static string GENERATED_WEAPON_PATH = Path.Combine(GENERATED_RESOURCES_PATH, "Weapon");
    private static string GENERATED_SHIELD_PATH = Path.Combine(GENERATED_RESOURCES_PATH, "Shield");
    private static string GENERATED_HEADGEAR_PATH = Path.Combine(GENERATED_RESOURCES_PATH, "Headgear");

    private static string DEFAULT_HEAD_DIR =
        Path.Combine("data", "sprite", "ÀÎ°£Á·", "¸Ó¸®Åë") + Path.DirectorySeparatorChar;

    private static string DEFAULT_BODY_DIR =
        Path.Combine("data", "sprite", "ÀÎ°£Á·", "¸öÅë") + Path.DirectorySeparatorChar;

    private static string DEFAULT_WEAPON_DIR = Path.Combine("data", "sprite", "ÀÎ°£Á·") + Path.DirectorySeparatorChar;
    private static string DEFAULT_SHIELD_DIR = Path.Combine("data", "sprite", "¹æÆÐ") + Path.DirectorySeparatorChar;

    private static string DEFAULT_HEADGEAR_DIR =
        Path.Combine("data", "sprite", "¾Ç¼¼»ç¸®") + Path.DirectorySeparatorChar;

    private static string DEFAULT_ITEM_DROP_IMAGE_DIR =
        Path.Combine("data", "sprite", "¾ÆÀÌÅÛ") + Path.DirectorySeparatorChar;

    private static string DEFAULT_ITEM_COLLECTION_DIR =
        Path.Combine("data", "texture", "À¯ÀúÀÎÅÍÆäÀÌ½º", "collection") + Path.DirectorySeparatorChar;

    private static string DEFAULT_ITEM_INVENTORY_DIR =
        Path.Combine("data", "texture", "À¯ÀúÀÎÅÍÆäÀÌ½º", "item") + Path.DirectorySeparatorChar;

    private static string DEFAULT_NPC_DIR = Path.Combine("data", "sprite", "npc") + Path.DirectorySeparatorChar;
    private static string DEFAULT_MONSTER_DIR = Path.Combine("data", "sprite", "¸ó½ºÅÍ") + Path.DirectorySeparatorChar;

    private static string DEFAULT_HEAD_PALETTE_DIR =
        Path.Combine("data", "palette", "¸Ó¸®") + Path.DirectorySeparatorChar;

    private static string DEFAULT_BODY_PALETTE_DIR =
        Path.Combine("data", "palette", "¸ö") + Path.DirectorySeparatorChar;

    [MenuItem("UnityRO/Utils/Extract/Sprites/Body")]
    static void ExtractBodySprites() {
        //FileManager.LoadGRF("D:\\Projetos\\ragnarok\\test\\", new List<string> { "kro_data.grf" });
        var Environment = InitUtilLua();

        try {
            var bodySpriteDescriptors = DataUtility
                                        .FilterDescriptors(FileManager.GetFileDescriptors(),
                                                           DEFAULT_BODY_DIR)
                                        .Where(it => Path.GetExtension(it) == ".spr")
                                        //.Where(it => Path.GetFileNameWithoutExtension(it) == $"ÃÊº¸ÀÚ_{FEMALE}")
                                        .ToList();

            for (var i = 0; i < bodySpriteDescriptors.Count; i++) {
                var progress = i * 1f / bodySpriteDescriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO",
                                                               $"Extracting effects {i} of {bodySpriteDescriptors.Count}\t\t{progress * 100}%",
                                                               progress)) {
                    break;
                }

                try {
                    ExtractPCSprite(bodySpriteDescriptors[i], Environment, GENERATED_BODY_PATH, DEFAULT_BODY_DIR,
                                    DEFAULT_BODY_PALETTE_DIR);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        } catch (Exception e) {
            EditorUtility.ClearProgressBar();
        } finally {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("UnityRO/Utils/Extract/Sprites/Head")]
    static void ExtractHeadSprites() {
        //FileManager.LoadGRF("D:\\Projetos\\ragnarok\\test\\", new List<string> { "kro_data.grf" });
        var Environment = InitUtilLua();

        try {
            var descriptors = DataUtility
                              .FilterDescriptors(FileManager.GetFileDescriptors(), DEFAULT_HEAD_DIR)
                              .Where(it => Path.GetExtension(it) == ".spr")
                              //.Where(it => Path.GetFileNameWithoutExtension(it) == $"1_{FEMALE}")
                              .ToList();

            for (var i = 0; i < descriptors.Count; i++) {
                var progress = i * 1f / descriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO",
                                                               $"Extracting effects {i} of {descriptors.Count}\t\t{progress * 100}%",
                                                               progress)) {
                    break;
                }

                try {
                    ExtractPCSprite(descriptors[i], Environment,
                                    GENERATED_HEAD_PATH,
                                    DEFAULT_HEAD_DIR,
                                    DEFAULT_HEAD_PALETTE_DIR);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        } catch (Exception e) {
            EditorUtility.ClearProgressBar();
        } finally {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("UnityRO/Utils/Extract/Sprites/Headgear")]
    static void ExtractHeadgearSprites() {
        ////FileManager.LoadGRF("D:\\Projetos\\ragnarok\\test\\", new List<string> { "kro_data.grf" });
        //FileManager.LoadGRF("/Users/dms11/Documents/Personal/ragnarok/", new List<string> { "data.grf" });

        try {
            var descriptors = DataUtility
                              .FilterDescriptors(FileManager.GetFileDescriptors(), DEFAULT_HEADGEAR_DIR)
                              .Where(it => Path.GetExtension(it) == ".spr")
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

                    var filename = Path.GetFileNameWithoutExtension(descriptor);
                    var spritePath = descriptor;

                    var genderlessFilename = filename
                                             .Replace(FEMALE + "_", "")
                                             .Replace(MALE + "_", "");

                    var inventoryPath = Path.Combine(DEFAULT_ITEM_INVENTORY_DIR, genderlessFilename)
                                            .Replace(Path.DirectorySeparatorChar, '/');
                    var collectionPath = Path.Combine(DEFAULT_ITEM_COLLECTION_DIR, genderlessFilename)
                                             .Replace(Path.DirectorySeparatorChar, '/');

                    var destinationDir = Path.Combine(GENERATED_HEADGEAR_PATH, genderlessFilename);

                    if (!Directory.Exists(destinationDir)) {
                        Directory.CreateDirectory(destinationDir);
                    }

                    try {
                        ExtractHeadgearSprite(spritePath, destinationDir, genderlessFilename + "_view");
                        ExtractHeadgearTexture(collectionPath, destinationDir, genderlessFilename + "_collection");
                        ExtractHeadgearTexture(inventoryPath, destinationDir, genderlessFilename + "_inventory");
                        ProcessInventoryImage(destinationDir, genderlessFilename + "_inventory");
                    } catch (Exception e) {
                        Debug.LogException(e);
                    }
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        } catch (Exception e) {
            EditorUtility.ClearProgressBar();
        } finally {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    private static void ProcessInventoryImage(string destinationDir, string genderlessFilename) {
        var importer =
            AssetImporter.GetAtPath(Path.Combine(destinationDir,genderlessFilename + ".png")) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.wrapMode = TextureWrapMode.Clamp;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        var textureSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(textureSettings);
        importer.SetTextureSettings(textureSettings);
        importer.SaveAndReimport();
    }

    private static void ExtractHeadgearTexture(string path, string assetPath, string genderlessFilename) {
        var texture = FileManager.Load(path + ".bmp") as Texture2D;
        if (texture == null) {
            Debug.LogError($"Couldnt load texture from {path}");
            return;
        }

        var bytes = texture.EncodeToPNG();
        var texturePath = Path.Combine(assetPath, genderlessFilename + ".png");
        File.WriteAllBytes(texturePath, bytes);
        AssetDatabase.ImportAsset(texturePath);
    }

    [MenuItem("UnityRO/Utils/Extract/Sprites/NPC and Monster")]
    static void ExtractNPCAndMonstersSprites() {
        //FileManager.LoadGRF("D:\\Projetos\\ragnarok\\test\\", new List<string> { "kro_data.grf" });
        var Environment = InitUtilLua();

        try {
            var monsterDescriptors = DataUtility
                                     .FilterDescriptors(FileManager.GetFileDescriptors(), DEFAULT_MONSTER_DIR)
                                     .Where(it => Path.GetExtension(it) == ".spr")
                                     .ToList();

            for (var i = 0; i < monsterDescriptors.Count; i++) {
                var progress = i * 1f / monsterDescriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO",
                                                               $"Extracting effects {i} of {monsterDescriptors.Count}\t\t{progress * 100}%",
                                                               progress)) {
                    break;
                }

                try {
                    ExtractNPCSprite(monsterDescriptors[i], Environment, GENERATED_NPC_PATH);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }

            var npcDescriptors = DataUtility
                                 .FilterDescriptors(FileManager.GetFileDescriptors(), DEFAULT_NPC_DIR)
                                 .Where(it => Path.GetExtension(it) == ".spr")
                                 .ToList();

            for (var i = 0; i < npcDescriptors.Count; i++) {
                var progress = i * 1f / npcDescriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO",
                                                               $"Extracting effects {i} of {npcDescriptors.Count}\t\t{progress * 100}%",
                                                               progress)) {
                    break;
                }

                try {
                    ExtractNPCSprite(npcDescriptors[i], Environment, GENERATED_NPC_PATH);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        } catch (Exception e) {
            EditorUtility.ClearProgressBar();
        } finally {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    private static void ExtractHeadgearSprite(string descriptor, string destinationDir, string filename) {
        var baseFileDir = descriptor.Replace(".spr", "");

        var spriteDataPath = Path.Combine(destinationDir, filename);
        var fullAssetPath = spriteDataPath + ".asset";
        if (File.Exists(fullAssetPath)) {
            return;
        }

        var sprPath = baseFileDir + ".spr";
        var actPath = baseFileDir + ".act";

        var sprBytes = FileManager.ReadSync(sprPath).ToArray();
        var act = FileManager.Load(actPath) as ACT;

        var spriteLoader = new CustomSpriteLoader();
        spriteLoader.Load(sprBytes, filename, false);

        var atlas = spriteLoader.Atlas;
        var bytes = atlas.EncodeToPNG();
        var atlasPath = spriteDataPath + ".png";
        File.WriteAllBytes(atlasPath, bytes);
        AssetDatabase.ImportAsset(atlasPath);
        ProcessAtlas(atlasPath, false);

        var spriteData = ScriptableObject.CreateInstance<SpriteData>();
        spriteData.act = act;
        spriteData.atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(atlasPath);
        spriteData.rects = spriteLoader.SpriteRects;

        AssetDatabase.CreateAsset(spriteData, fullAssetPath);
    }

    private static void ExtractNPCSprite(
        string descriptor,
        Script luaEnvironment,
        string destinationDir
    ) {
        var filename = Path.GetFileNameWithoutExtension(descriptor);
        var baseFileDir = descriptor.Replace(".spr", "");

        #region Lookup for readable name

        var npcIdsTable = luaEnvironment.Globals["jobtbl"] as Table;
        var npcNameTable = luaEnvironment.Globals["JobNameTable"] as Table;

        var bodyPathKey = filename;
        var jobId = -1;

        foreach (var pair in npcNameTable.Pairs) {
            if (!pair.Value.CastToString().Equals(filename, StringComparison.OrdinalIgnoreCase)) continue;

            jobId = int.Parse(pair.Key.ToString());
            bodyPathKey = pair.Value.CastToString();
            break;
        }

        #endregion

        var spriteDataPath = Path.Combine(destinationDir, bodyPathKey);
        Directory.CreateDirectory(destinationDir);
        var fullAssetPath = spriteDataPath + ".asset";
        if (File.Exists(fullAssetPath)) {
            return;
        }

        var sprPath = baseFileDir + ".spr";
        var actPath = baseFileDir + ".act";

        var sprBytes = FileManager.ReadSync(sprPath).ToArray();
        var act = FileManager.Load(actPath) as ACT;

        var spriteLoader = new CustomSpriteLoader();
        spriteLoader.Load(sprBytes, filename, false);

        var atlas = spriteLoader.Atlas;
        var bytes = atlas.EncodeToPNG();
        var atlasPath = spriteDataPath + ".png";
        File.WriteAllBytes(atlasPath, bytes);
        AssetDatabase.ImportAsset(atlasPath);
        ProcessAtlas(atlasPath, false);

        var spriteData = ScriptableObject.CreateInstance<SpriteData>();
        spriteData.act = act;
        spriteData.jobId = jobId;
        spriteData.atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(atlasPath);
        spriteData.rects = spriteLoader.SpriteRects;

        AssetDatabase.CreateAsset(spriteData, fullAssetPath);
    }

    private static void ExtractPCSprite(
        string descriptor,
        Script luaEnvironment,
        string destinationDir,
        string sourceDir,
        string paletteDir
    ) {
        var filename = Path.GetFileNameWithoutExtension(descriptor)
                           .Split("_")[0];
        var baseFileDir = descriptor.Replace(".spr", "");
        var pathsTable = luaEnvironment.Globals["PCPaths"] as Table;
        var idsTable = luaEnvironment.Globals["PCIds"] as Table;
        var mountIdsTable = luaEnvironment.Globals["PCMounts"] as Table;

        string bodyPathKey = filename;
        int jobId = -1;
        foreach (var pair in pathsTable.Pairs) {
            if (pair.Value.CastToString() != filename) continue;

            foreach (var idPair in idsTable.Pairs) {
                if (idPair.Value.CastToString() != pair.Key.CastToString()) continue;

                bodyPathKey = idPair.Key.CastToString();
                break;
            }

            foreach (var idPair in mountIdsTable.Pairs) {
                if (idPair.Value.CastToString() != pair.Key.CastToString()) continue;

                bodyPathKey = idPair.Key.CastToString();
                break;
            }

            jobId = int.Parse(pair.Key.CastToString());
            break;
        }

        var sprPath = baseFileDir + ".spr";
        var actPath = baseFileDir + ".act";

        var sprBytes = FileManager.ReadSync(sprPath).ToArray();
        var act = FileManager.Load(actPath) as ACT;

        var spriteLoader = new CustomSpriteLoader();

        var dir = Path
            .GetDirectoryName(descriptor.Replace('/', Path.DirectorySeparatorChar)
                                        .Replace(sourceDir, ""));

        var isCostume = dir.Contains("costume_");
        var isFemale = dir.Contains(FEMALE);

        var assetPath = Path.Combine(destinationDir, isCostume ? "Costume" : "", bodyPathKey);
        var spriteDataPath = Path.Combine(assetPath, bodyPathKey + (isFemale ? "_f" : "_m"));

        Directory.CreateDirectory(assetPath);

        var spriteData = ScriptableObject.CreateInstance<SpriteData>();
        spriteData.act = act;
        spriteData.jobId = jobId;

        spriteLoader.Load(sprBytes, filename, true);

        var atlas = spriteLoader.Atlas;
        var bytes = atlas.EncodeToPNG();
        var atlasPath = spriteDataPath + ".png";
        File.WriteAllBytes(atlasPath, bytes);
        AssetDatabase.ImportAsset(atlasPath);

        ProcessAtlas(atlasPath);
        spriteData.atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(atlasPath);

        var paletteList = new List<Texture2D>();
        var palette = spriteLoader.Palette;
        var paletteBytes = palette.EncodeToPNG();
        var palettePath = spriteDataPath + "_pal_0.png";
        File.WriteAllBytes(palettePath, paletteBytes);
        AssetDatabase.ImportAsset(palettePath);

        ProcessPalette(palettePath);
        var basePalette = AssetDatabase.LoadAssetAtPath<Texture2D>(palettePath);
        paletteList.Add(basePalette);

        spriteData.rects = spriteLoader.SpriteRects;
        var paletteFilter = Path.Combine(paletteDir, filename + $"_{(isFemale ? FEMALE : MALE)}")
            ;
        var paletteDescriptors = DataUtility
                                 .FilterDescriptors(FileManager.GetFileDescriptors(), paletteFilter)
                                 .Where(it => Path.GetExtension(it) == ".pal")
                                 .ToList();

        foreach (var paletteDescriptor in paletteDescriptors) {
            var memoryReader = FileManager.ReadSync(paletteDescriptor);
            var paletteTexture = new Texture2D(256, 1, TextureFormat.RGBA32, false, true);
            paletteTexture.alphaIsTransparency = false;
            paletteTexture.filterMode = FilterMode.Point;
            paletteTexture.LoadRawTextureData(memoryReader.ToArray());
            paletteTexture.Apply();
            var pNumber = int.Parse(paletteDescriptor.Split("_").Last().Split(".").First()) + 1;

            var pBytes = paletteTexture.EncodeToPNG();
            var pPath = spriteDataPath + $"_pal_{pNumber}.png";
            File.WriteAllBytes(pPath, pBytes);
            AssetDatabase.ImportAsset(pPath);
            ProcessPalette(pPath);
            var diskPalette = AssetDatabase.LoadAssetAtPath<Texture2D>(pPath);
            paletteList.Add(diskPalette);
        }

        spriteData.palettes = paletteList.OrderBy(it => it.name).ToArray();

        var fullAssetPath = spriteDataPath + ".asset";
        AssetDatabase.CreateAsset(spriteData, fullAssetPath);
    }

    private static void ProcessPalette(string palettePath) {
        TextureImporter importer = AssetImporter.GetAtPath(palettePath) as TextureImporter;
        importer.textureType = TextureImporterType.Default;
        importer.sRGBTexture = false;
        importer.alphaIsTransparency = false;
        importer.wrapMode = TextureWrapMode.Clamp;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        var textureSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(textureSettings);
        importer.SetTextureSettings(textureSettings);
        importer.SaveAndReimport();
    }

    private static void ProcessAtlas(string atlasPath, bool isSingleChannel = true) {
        TextureImporter importer = AssetImporter.GetAtPath(atlasPath) as TextureImporter;
        importer.textureType = isSingleChannel ? TextureImporterType.SingleChannel : TextureImporterType.Default;
        importer.sRGBTexture = !isSingleChannel;
        importer.alphaIsTransparency = !isSingleChannel;
        importer.wrapMode = TextureWrapMode.Clamp;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        var textureSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(textureSettings);

        if (isSingleChannel) {
            textureSettings.singleChannelComponent = TextureImporterSingleChannelComponent.Red;
            textureSettings.textureFormat = TextureImporterFormat.R8;
        }

        importer.SetTextureSettings(textureSettings);
        importer.SaveAndReimport();
    }

    private static Script InitUtilLua() {
        var luaEnv = new Script();

        var pcIds = File.ReadAllText(Path.Combine(UTILS_DIR, "PCIds.lub"), Encoding.GetEncoding("windows-1252"));
        var pcPaths = File.ReadAllText(Path.Combine(UTILS_DIR, "PCPaths.lub"), Encoding.GetEncoding("windows-1252"));
        var pcNames = File.ReadAllText(Path.Combine(UTILS_DIR, "PCNames.lub"), Encoding.GetEncoding("windows-1252"));
        var pcPals = File.ReadAllText(Path.Combine(UTILS_DIR, "PCPals.lub"), Encoding.GetEncoding("windows-1252"));
        var shieldTable = File.ReadAllText(Path.Combine(UTILS_DIR, "ShieldTable.lub"),
                                           Encoding.GetEncoding("windows-1252"));
        var pcHands = File.ReadAllText(Path.Combine(UTILS_DIR, "PCHands.lub"), Encoding.GetEncoding("windows-1252"));
        var npcIdentity = File.ReadAllText(Path.Combine(UTILS_DIR, "npcidentity.lub"),
                                           Encoding.GetEncoding("windows-1252"));
        var jobName = File.ReadAllText(Path.Combine(UTILS_DIR, "jobname.lub"), Encoding.GetEncoding("windows-1252"));

        luaEnv.DoString(pcIds);
        luaEnv.DoString(pcPaths);
        luaEnv.DoString(pcNames);
        luaEnv.DoString(pcPals);
        luaEnv.DoString(pcHands);
        luaEnv.DoString(shieldTable);

        luaEnv.DoString(npcIdentity);
        luaEnv.DoString(jobName);

        return luaEnv;
    }
}