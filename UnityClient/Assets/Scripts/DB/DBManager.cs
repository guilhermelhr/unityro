using Newtonsoft.Json.Linq;
using ROIO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DBManager {

    public const string INTERFACE_PATH = "data/texture/À¯ÀúÀÎÅÍÆäÀÌ½º/";

    private static string[] MESH_EXTENSIONS = new string[] { ".gr2", ".prefab" };
    private static string[] SexTable = new string[] { "¿©", "³²" };
    private static JObject WeaponActions;
    private static JObject WeaponJobTable;
    private static JObject ClassTable;

    public static void Init() {
        new LuaInterface();
        Tables.Init();

        var WeaponActionsText = Addressables.LoadAssetAsync<TextAsset>("WeaponActions").WaitForCompletion();
        var WeaponJobTableText = Addressables.LoadAssetAsync<TextAsset>("WeaponJobTable").WaitForCompletion();
        var ClassTableText = Addressables.LoadAssetAsync<TextAsset>("ClassTable").WaitForCompletion();

        WeaponActions = JObject.Parse(Encoding.UTF8.GetString(WeaponActionsText.bytes));
        WeaponJobTable = JObject.Parse(Encoding.UTF8.GetString(WeaponJobTableText.bytes));
        ClassTable = JObject.Parse(Encoding.UTF8.GetString(ClassTableText.bytes));
    }

    public static Item GetItem(int gID) {
        ItemDB.TryGetValue(gID, out Item item);

        return item;
    }

    public static string GetItemPath(int gID, bool isIdentified) {
        var it = GetItem(gID);
        var resName = isIdentified ? it.identifiedResourceName : it.unidentifiedResourceName;
        return $"data/sprite/¾ÆÀÌÅÛ/{resName}";
    }

    public static string GetItemResPath(int itemID, bool isIdentified) {
        var item = GetItem(itemID);
        return GetItemResPath(item, isIdentified);
    }

    public static string GetItemResPath(Item item, bool isIdentified) {
        return $"{INTERFACE_PATH}item/{(isIdentified ? item.identifiedResourceName : item.unidentifiedResourceName)}.png";
    }

    public static string GetItemCollectionPath(Item item, bool isIdentified) {
        return $"{INTERFACE_PATH}collection/{(isIdentified ? item.identifiedResourceName : item.unidentifiedResourceName)}.png";
    }

    public static int GetWeaponAction(Job job, int sex, int weapon, int shield) {
        var jobValue = $"{(ushort) job}";
        var weaponViewId = $"{GetItemViewID(weapon)}";

        try {
            var jobActionValue = WeaponActions[jobValue];
            if (jobActionValue.Type == JTokenType.Array) {
                return jobActionValue[$"{sex}"][weaponViewId].ToObject<int>();
            } else {
                return jobActionValue[weaponViewId].ToObject<int>();
            }
        } catch {
            return 1;
        }
    }

    public static int GetWeaponType(int itemID) {
        var type = WeaponType.WEAPONTYPE_NONE;
        if (itemID == 0) {
            type = WeaponType.WEAPONTYPE_NONE;
        } else if (itemID >= 1100 && itemID <= 1199) {
            type = WeaponType.WEAPONTYPE_SWORD;
        } else if (itemID >= 1901 && itemID < 1999) {
            type = WeaponType.WEAPONTYPE_INSTRUMENT;
        } else if (itemID >= 1201 && itemID <= 1299) {
            type = WeaponType.WEAPONTYPE_SHORTSWORD;
        } else if (itemID >= 1350 && itemID <= 1399) {
            type = WeaponType.WEAPONTYPE_AXE;
        } else if (itemID >= 1301 && itemID <= 1349) {
            type = WeaponType.WEAPONTYPE_TWOHANDAXE;
        } else if (itemID >= 1450 && itemID <= 1499) {
            type = WeaponType.WEAPONTYPE_SPEAR;
        } else if (itemID >= 1401 && itemID <= 1449) {
            type = WeaponType.WEAPONTYPE_TWOHANDSPEAR;
        } else if (itemID >= 1501 && itemID <= 1599) {
            type = WeaponType.WEAPONTYPE_MACE;
        } else if (itemID >= 1601 && itemID <= 1699) {
            type = WeaponType.WEAPONTYPE_ROD;
        } else if (itemID >= 1701 && itemID <= 1749) {
            type = WeaponType.WEAPONTYPE_BOW;
        } else if (itemID >= 1801 && itemID <= 1899) {
            type = WeaponType.WEAPONTYPE_CATARRH;
        } else if (itemID >= 2001 && itemID <= 2099) {
            type = WeaponType.WEAPONTYPE_TWOHANDSWORD;
        } else if (itemID >= 1800 && itemID < 1900) {
            type = WeaponType.WEAPONTYPE_KNUKLE;
        } else if (itemID >= 1900 && itemID < 1950) {
            type = WeaponType.WEAPONTYPE_INSTRUMENT;
        } else if (itemID >= 1950 && itemID < 2000) {
            type = WeaponType.WEAPONTYPE_WHIP;
        } else if (itemID >= 2000 && itemID < 2100) {
            type = WeaponType.WPCLASS_TWOHANDROD;
        } else if (itemID >= 13150 && itemID < 13200) {
            type = WeaponType.WPCLASS_GUN_RIFLE;
        } else if (itemID >= 13000 && itemID < 13100) {
            type = WeaponType.WEAPONTYPE_SHORTSWORD;
        } else if (itemID >= 13100 && itemID < 13150) {
            type = WeaponType.WPCLASS_GUN_HANDGUN;
        } else if (itemID >= 13300 && itemID < 13400) {
            type = WeaponType.WPCLASS_SYURIKEN;
        }

        return (int) type;
    }

    public static int GetItemViewID(int itemId) {
        // Already a ViewID
        if (itemId < (int) WeaponType.MAX) {
            return itemId;
        }
        ItemDB.TryGetValue(itemId, out Item item);

        return item?.ClassNum ?? 0;
    }

    public static string GetBodyPath(int job, int sex) {
        var isPC = ClassTable.TryGetValue(job.ToString(), out var jobPath);
        var isMonster = JobIdentityPath.TryGetValue(job, out string monsterPath);
        var sexPath = SexTable[sex];

        // PC
        if (job < 45) {
            return $"data/sprite/ÀÎ°£Á·/¸öÅë/{sexPath}/{jobPath}_{sexPath}";
        }

        if (job == 4218) {
            return $"data/sprite/µµ¶÷Á·/¸öÅë/{sexPath}/summoner_{sexPath}";
        }

        // Not visible sprite
        if (job == 111 || job == 139) {
            return null;
        }

        // NPC
        if (job < 1000) {
            return "data/sprite/npc/" + (monsterPath ?? JobIdentityPath[46]).ToLower();
        }

        // Monsters
        if (job < 4000) {
            return "data/sprite/¸ó½ºÅÍ/" + (monsterPath ?? JobIdentityPath[1001]).ToLower();
        }

        // PC
        if (job < 6000) {
            return $"data/sprite/ÀÎ°£Á·/¸öÅë/{sexPath}/{jobPath}_{sexPath}";
        }

        // Homunculus
        return "data/sprite/homun/" + (monsterPath ?? JobIdentityPath[1001]).ToLower();

        // TODO: add support for mercenary
    }

    public static string GetHeadPath(int job, int headId, int sex) {
        if (job == 4218) {
            return $"data/sprite/µµ¶÷Á·/¸Ó¸®Åë/{SexTable[sex]}/{headId}_{SexTable[sex]}";
        }

        return $"data/sprite/ÀÎ°£Á·/¸Ó¸®Åë/{SexTable[sex]}/{headId}_{SexTable[sex]}";
    }

    public static string GetHeadPalPath(int id, int palId, int sex) {
        return $"data/palette/¸Ó¸®/¸Ó¸®{id}_{SexTable[sex]}_{palId}";
    }

    public static string GetBodyPalPath(int job, int palId, int sex) {
        var isPC = ClassTable.TryGetValue(job.ToString(), out var jobPath);
        return $"data/palette/¸ö/{jobPath}_{SexTable[sex]}_{palId}";
    }

    public static string GetShieldPath(int id, int job, int sex) {
        if (id == 0) {
            return null;
        }

        var baseClass = GetBaseClass(job);
        var ViewID = GetItemViewID(id);

        ItemTable.Shields.TryGetValue(ViewID, out var shield);
        return $"data/sprite/¹æÆÐ/{baseClass}/{baseClass}_{SexTable[sex]}_{shield ?? ItemTable.Shields[1]}";
    }

    public static string GetWeaponPath(int id, int job, int sex) {
        if (id == 0) {
            return null;
        }

        var baseClass = GetBaseClass(job);
        var ViewID = GetItemViewID(id);

        ItemTable.Weapons.TryGetValue((WeaponType) ViewID, out var weapon);
        return $"data/sprite/ÀÎ°£Á·/{baseClass}/{baseClass}_{SexTable[sex]}{weapon.KoreanTo1252() ?? $"_{ViewID}"}";
    }

    public static string GetHatPath(int id, int sex) {
        if (id == 0) {
            return null;
        }

        var hatPath = ItemTable.GetAccessoryResName(id);

        return $"data/sprite/¾Ç¼¼»ç¸®/{SexTable[sex]}/{SexTable[sex]}{hatPath}";
    }

    public static Dictionary<int, string> JobIdentityPath => JobItentityTable.Table;

    public static Dictionary<int, Item> ItemDB => ItemTable.Items;

    public static int[][] HairIndexPath => HairIndexTable.table;

    private static string GetBaseClass(int job) {
        return (WeaponJobTable[$"{job}"] ?? WeaponJobTable["0"]).ToObject<string>();
    }

    public static GameEntityViewerType GetEntityViewerType(short job) {
        var fallback = GameEntityViewerType.SPRITE;
        var found = JobIdentityPath.TryGetValue(job, out string name);

        if (found) {
            fallback = IsPathMesh(name) ? GameEntityViewerType.MESH : GameEntityViewerType.SPRITE;
        }

        return fallback;
    }

    public static bool IsPathMesh(string path) {
        return MESH_EXTENSIONS.Contains(Path.GetExtension(path));
    }
}
