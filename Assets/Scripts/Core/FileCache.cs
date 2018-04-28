using B83.Image.BMP;
using System;
using System.Collections;
using UnityEngine;

public static class FileCache
{
    private static Hashtable caches = new Hashtable();
    private static readonly object cacheLock = new object();
    private static string[] disallowedExtensions = new string[] {
        "grf", "gat", "rsw", "gnd"
    };

    private static int hits = 0;
    private static int misses = 0;

    public static void ClearAll() {
        hits = misses = 0;
        foreach(Hashtable cache in caches.Values) {
            cache.Clear();
        }
        caches.Clear();
    }

    public static bool Add(string file, string extension, object data) {
        if(Array.IndexOf(disallowedExtensions, extension) != -1) {
            return false;
        }

        lock(cacheLock) {
            Hashtable cache = caches[extension] as Hashtable;
            if(cache == null) {
                caches[extension] = cache = new Hashtable();
            }

        
            if(!cache.ContainsKey(file)) {
                cache.Add(file, data);
                return true;
            }
        }

        return false;
    }

    public static object Get(string file, string extension) {
        Hashtable cache = caches[extension] as Hashtable;

        if(cache != null) {
            object data = cache[file];
            Texture2D texture = toUnityTexture(data);
            if(texture != null) {
                cache.Remove(file);
                cache.Add(file, texture);
                return texture;
            }

            return data;
        }

        return null;
    }

    public static bool Has(string file) {
        lock(cacheLock) {
            foreach(Hashtable cache in caches.Values) {
                if(cache.ContainsKey(file)) {
                    hits++;
                    return true;
                }
            }
            misses++;
        }
        return false;
    }

    public static void ClearAllWithExt(string ext) {
        Hashtable cache = caches[ext] as Hashtable;
        if(cache != null) {
            cache.Clear();
        }
    }

    private static Texture2D toUnityTexture(object texture) {
        if(texture is FileManager.RawImage) {
            Texture2D t = new Texture2D(0, 0);
            t.LoadImage(((FileManager.RawImage) texture).data);
            t.name = "maptexture";
            return t;
        } else if(texture is BMPImage) {
            Texture2D t = ((BMPImage) texture).ToTexture2D();
            t.name = "maptexture";
            return t;
        } else {
            return null;
        }
    }

    internal static void Report() {
        Debug.Log("Cache Report - Hits: " + hits + " Misses: " + misses + " Success rate: " + ((float) hits / (hits + misses)) * 100 + "%");
    }
}
