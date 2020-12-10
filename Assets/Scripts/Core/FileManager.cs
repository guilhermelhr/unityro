using B83.Image.BMP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// File System
/// Manages file io
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class FileManager {
    private static Regex rext = new Regex(@".[^\.]+$");
    private static BMPLoader loader = new BMPLoader();
    private static Grf grf = null;
    private static bool batching = false;
    private static List<string> batch = new List<string>();

    public static Grf Grf {
        get { return grf; }
    }

    public class RawImage {
        public byte[] data;
    }

    public static void loadGrf(string grfPath) {
        grf = Grf.grf_callback_open(grfPath, "r", null);
    }

    public static void InitBatch() {
        batching = true;
    }

    public static void EndBatch(System.Action batchItemLoadedCallback = null) {
        batching = false;
        if(batch.Count > 0) {
            pendingThreads = batch.Count;
            doneEvent = new ManualResetEvent(false);
            for(int i = 0; i < batch.Count; i++) {
                string ext = rext.Match(batch[i]).Value.Substring(1).ToLower();
                BatchLoader loader = new BatchLoader(batch[i], ext);
                ThreadPool.QueueUserWorkItem(loader.ThreadPoolCallback, new object[] { i, batchItemLoadedCallback });
            }
            batch.Clear();
            doneEvent.WaitOne();
        }
    }

    public static object Load(string file) {
        file = file.Trim();
        file = file.Replace("\\", "/");

        if(batching) {
            if(!batch.Contains(file)) {
                batch.Add(file);
            }
            return null;
        }

        if(!string.IsNullOrEmpty(file)) {
            string ext = rext.Match(file).Value.Substring(1).ToLower();
            if(!string.IsNullOrEmpty(ext)) {
                if(FileCache.Has(file)) {
                    return FileCache.Get(file, ext);
                } else {
                    object data = DoLoad(file, ext);
                    if(data != null) {
                        if(FileCache.Add(file, ext, data)) {
                            return FileCache.Get(file, ext);
                        } else {
                            return data;
                        }
                    }
                }
            }
        }

        return null;
    }

    private static object DoLoad(string file, string ext) {
        if(ext == "grf") {
            return File.OpenRead(file);
        } else {
            using(var br = ReadSync(file)) {
                if(br == null) {
                    throw new Exception($"Could not load file: {file}");
                }

                switch(ext) {
                    // Images
                    case "jpg":
                    case "jpeg":
                    case "png":
                        return new RawImage() {
                            data = br.ToArray()
                        };
                    case "bmp":
                        return loader.LoadBMP(br);
                    case "tga":
                        return TGALoader.LoadTGA(br);

                    // Text
                    case "txt":
                    case "xml":
                    case "lua":
                        return Encoding.UTF8.GetString(br.ToArray());

                    case "spr":
                        SPR spr = SpriteLoader.Load(br);
                        spr.SwitchToRGBA();
                        spr.Compile();
                        spr.filename = file;
                        return spr;
                    case "str":
                        return EffectLoader.Load(br);
                    case "act":
                        return ActionLoader.Load(br);

                    // Binary
                    case "gat":
                        return new Altitude(br);
                    case "rsw":
                        return WorldLoader.Load(br);
                    case "gnd":
                        return GroundLoader.Load(br);
                    case "rsm":
                        return ModelLoader.Load(br);

                    // Audio
                    case "wav":
                        WAVLoader.WAVFile wav = WAVLoader.OpenWAV(br.ToArray());
                        AudioClip clip = AudioClip.Create(file, wav.samples, wav.channels, wav.sampleRate, false);
                        clip.SetData(wav.leftChannel, 0);
                        return clip;
                    case "mp3":
                    case "ogg":
                        break;
                    default:
                        throw new Exception($"Unsuported file format: {ext} for file {file}");
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Syncronously read a file with Unity's Resources
    /// </summary>
    /// <param name="path">file path</param>
    /// <returns>file or null</returns>
    public static BinaryReader UnityReadSync(string path) {
        TextAsset result = Resources.Load(path) as TextAsset;
        return new BinaryReader(result.bytes);
    }

    /// <summary>
    /// Asyncronously read a file with Unity's Resources
    /// </summary>
    /// <param name="path">file path</param>
    /// <returns><see cref="ResourceRequest"/></returns>
    public static ResourceRequest UnityReadAsync(string path) {
        ResourceRequest result = Resources.LoadAsync(path);

        return result;
    }

    /// <summary>
    /// Syncronously read a file
    /// </summary>
    /// <param name="path">file path</param>
    /// <returns>file or null</returns>
    public static BinaryReader ReadSync(string path) {
        //try grf first
        if(grf != null) {
            GrfFile file = grf.GetDescriptor(path);
            if(file != null) {
                byte[] data = grf.GetData(file);

                if(data != null) {
                    return new BinaryReader(data);
                } else {
                    Debug.Log("Could not read grf data for " + path);
                }
            } else {
                Debug.Log("File not found on GRF: " + path);
            }
        }

        //try filesystem
        if(File.Exists(Application.dataPath + "/" + path)) {
            byte[] buffer = File.ReadAllBytes(Application.dataPath + "/" + path);
            return new BinaryReader(buffer);
        } else {
            return null;
        }
    }

    /// <summary>
    /// Asyncronously read a file
    /// </summary>
    /// <param name="path">file path</param>
    /// <returns>BinaryReader containing file or null</returns>
    /// <seealso cref="BinaryReader"/>
    public static async Task<BinaryReader> ReadAsync(string path) {
        BinaryReader result = new BinaryReader();

        using(FileStream sourceStream = new FileStream(path,
            FileMode.Open, FileAccess.Read, FileShare.Read,
            bufferSize: 4096, useAsync: true)) {


            byte[] buffer = new byte[4096];
            int numRead;
            while((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0) {
                result.Write(buffer, 0, numRead);
            }
        }

        result.Seek(0, SeekOrigin.Begin);
        return result;
    }

    private static int pendingThreads;
    private static ManualResetEvent doneEvent;
    private class BatchLoader {
        private string file;
        private string ext;

        public string File { get { return file; } }

        public BatchLoader(string file, string ext) {
            this.file = file;
            this.ext = ext;
        }

        public void ThreadPoolCallback(object state) {
            try {
                object[] parameters = state as object[];
                System.Action callback = (System.Action)parameters[1];
                if(!FileCache.Has(file)) {
                    object data = DoLoad(file, ext);
                    callback?.Invoke();
                    if(data != null) {
                        FileCache.Add(file, ext, data);
                    }
                }
            } finally {
                if(Interlocked.Decrement(ref pendingThreads) == 0) {
                    doneEvent.Set();
                }
            }
        }
    }
}
