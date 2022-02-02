using B83.Image.BMP;
using Newtonsoft.Json.Linq;
using ROIO.GRF;
using ROIO.Loaders;
using ROIO.Models.FileTypes;
using ROIO.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace ROIO {
    /// <summary>
    /// File System
    /// Manages file io
    /// 
    /// @author Guilherme Hernandez
    /// Based on ROBrowser by Vincent Thibault (robrowser.com)
    /// </summary>
    public class FileManager {

        private static Regex rext = new Regex(@".[^\.]+$");
        private static BMPLoader BMPLoader = new BMPLoader();

        private static List<Grf> GrfList = new List<Grf>();

        private static bool batching = false;
        private static List<string> batch = new List<string>();

        public class RawImage {
            public byte[] data;
        }

        public static void LoadGRF(string rootPath, List<string> grfs) {
            GrfList = new List<Grf>();
            foreach (var path in grfs) {
                var grf = Grf.grf_callback_open(rootPath + path, "r", null);
                GrfList.Add(grf);
            }

            Tables.Init();
        }

        public static void InitBatch() {
            batching = true;
        }

        public static void EndBatch(System.Action batchItemLoadedCallback = null) {
            batching = false;
            if (batch.Count > 0) {
                pendingThreads = batch.Count;
                doneEvent = new ManualResetEvent(false);
                for (int i = 0; i < batch.Count; i++) {
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

            if (batching) {
                if (!batch.Contains(file)) {
                    batch.Add(file);
                }
                return null;
            }

            if (!string.IsNullOrEmpty(file)) {
                string ext = rext.Match(file).Value.Substring(1).ToLower();
                if (!string.IsNullOrEmpty(ext)) {
                    if (FileCache.Has(file)) {
                        return FileCache.Get(file, ext);
                    } else {
                        object data = DoLoad(file, ext);
                        if (data != null) {
                            if (FileCache.Add(file, ext, data)) {
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
            var extension = Path.GetExtension(file);
            var nameWithoutExtension = file.Substring(0, file.IndexOf(extension));
            var bundledTexturePath = Path.Combine("Textures", nameWithoutExtension);

            if (ext == "grf") {
                return File.OpenRead(file);
            } else {
                using (var br = ReadSync(file)) {
                    if (br == null) {
                        // try fallback to exported data folder
                        switch (ext) {
                            case "jpg":
                            case "jpeg":
                            case "png":
                            case "bmp":
                            case "tga":
                                return Resources.Load(bundledTexturePath);
                        }

                        throw new Exception($"Could not load file: {file}");
                    }

                    switch (ext) {
                        // Images
                        case "jpg":
                        case "jpeg":
                        case "png":
                            return new RawImage() {
                                data = br.ToArray()
                            };
                        case "bmp":
                            return BMPLoader.LoadBMP(br);
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
                            return EffectLoader.Load(br, Path.GetDirectoryName(file).Replace("\\", "/"));
                        case "act":
                            return ActionLoader.Load(br);

                        // Binary
                        case "gat":
                            return AltitudeLoader.Load(br);
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
                        case "json":
                            return JObject.Parse(Encoding.UTF8.GetString(br.ToArray()));
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
        public static MemoryStreamReader UnityReadSync(string path) {
            TextAsset result = Resources.Load(path) as TextAsset;
            return new MemoryStreamReader(result.bytes);
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
        public static MemoryStreamReader ReadSync(string path) {

            if (Application.isMobilePlatform || Application.platform == RuntimePlatform.WebGLPlayer) {
                var filePath = Path.Combine(Application.streamingAssetsPath, path);
                WWW reader = new WWW(filePath);
                while (!reader.isDone) { };

                if (reader.bytes.Length > 0) {
                    return new MemoryStreamReader(reader.bytes);
                } else {
                    return null;
                }
            }

            foreach (var grf in GrfList) {
                GrfFile file = grf.GetDescriptor(path);
                if (file != null) {
                    byte[] data = grf.GetData(file);

                    if (data != null) {
                        return new MemoryStreamReader(data);
                    }
                }
            }

            //try filesystem
            if (File.Exists(Application.streamingAssetsPath + "/" + path)) {
                byte[] buffer = File.ReadAllBytes(Application.streamingAssetsPath + "/" + path);
                return new MemoryStreamReader(buffer);
            } else {
                return null;
            }
        }

        public static StreamReader ReadSync(string path, Encoding encoding) {
            var memoryStreamReader = ReadSync(path);
            if (memoryStreamReader != null) {
                return new StreamReader(memoryStreamReader, encoding);
            } else
                return null;
        }

        /// <summary>
        /// Asyncronously read a file
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>BinaryReader containing file or null</returns>
        /// <seealso cref="BinaryReader"/>
        public static async Task<MemoryStreamReader> ReadAsync(string path) {
            MemoryStreamReader result = new MemoryStreamReader();

            using (FileStream sourceStream = new FileStream(path,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true)) {


                byte[] buffer = new byte[4096];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0) {
                    result.Write(buffer, 0, numRead);
                }
            }

            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        public static Hashtable GetFileDescriptors() {
            var files = new Hashtable(StringComparer.OrdinalIgnoreCase);
            var list = new List<Grf>();
            list.AddRange(GrfList);
            list.Reverse();

            list.ForEach(grf => {
                foreach (DictionaryEntry a in grf.files) {
                    files[a.Key] = a.Value;
                }
            });

            return files;
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
                    System.Action callback = (System.Action) parameters[1];
                    if (!FileCache.Has(file)) {
                        object data = DoLoad(file, ext);
                        callback?.Invoke();
                        if (data != null) {
                            FileCache.Add(file, ext, data);
                        }
                    }
                } finally {
                    if (Interlocked.Decrement(ref pendingThreads) == 0) {
                        doneEvent.Set();
                    }
                }
            }
        }
    }
}