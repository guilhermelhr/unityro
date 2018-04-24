using B83.Image.BMP;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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

    public static void setGrf(Grf grf) {
        FileManager.grf = grf;
    }

    public static object Load(string file) {
        if(!string.IsNullOrEmpty(file)) {
            file = file.Trim();
            file = file.Replace("\\", "/");
            string ext = rext.Match(file).Value.Substring(1).ToLower();
            string fileWOExt = file.Replace("." + ext, "");
            
            if(!string.IsNullOrEmpty(ext)) {
                switch(ext) {
                    case "grf":
                        return File.OpenRead(file);
                    // Regular images files
                    case "jpg":
                    case "jpeg":
                    case "png":
                        var image = new Texture2D(0, 0);
                        image.LoadImage(ReadSync(file).ToArray());
                        return image;
                    case "bmp":
                        var bmp = loader.LoadBMP(ReadSync(file));
                        return bmp.ToTexture2D();
                    // Texts
                    case "txt":
                    case "xml":
                    case "lua":
                        BinaryReader binaryReader = ReadSync(file);
                        if(binaryReader != null) {
                            return Encoding.UTF8.GetString(binaryReader.ToArray());
                        } else {
                            return null;
                        }
                    // Binary
                    case "gat":
                        return new Altitude(ReadSync(file));
                    case "rsw":
                        return WorldLoader.Load(ReadSync(file));
                    case "gnd":
                        return GroundLoader.Load(ReadSync(file));
                    case "rsm":
                        return ModelLoader.Load(ReadSync(file));
                    // Audio
                    case "wav":
                    case "mp3":
                    case "ogg":

                    case "act":
                        //return new Action(ReadSync(file)).compile();
                    case "str":
                        //return new Str(ReadSync(file));
                        Debug.LogWarning("Can't read " + file + "\nLoader for " + ext + " is not implemented");
                        break;
                    case "tga":
                    default:
                        Debug.LogWarning("Default reader loading " + file);
                        return ReadSync(file);
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
                }
            }
        }

        Debug.Log("File not found on GRF: " + path);

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
}
