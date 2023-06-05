#if UNITY_EDITOR
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataUtility {
    public static string[] GetFilesFromDir(string dir) {
        return Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
            .Where(it => Path.HasExtension(it) && !it.Contains(".meta"))
            .Select(it => it.Replace(Application.dataPath, "Assets"))
            .ToArray();
    }

    public static List<string> FilterDescriptors(Hashtable descriptors, params string[] filters) {
        return (from object path in descriptors.Keys from filter in filters where (path as string).StartsWith(filter.Replace(Path.DirectorySeparatorChar, '/')) select path as string).ToList();
    }
}
#endif