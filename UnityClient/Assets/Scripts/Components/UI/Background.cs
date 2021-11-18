using ROIO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour {

    private const string LOGIN_BACKGROUND_IMAGE = "bgi_temp.bmp";
    private const int LOADING_SCREENS_LENGTH = 10;

    private static RawImage RawImage;
    
    public static void SetLoginBackground() {
        Texture2D texture = FileManager.Load($"{DBManager.INTERFACE_PATH}{LOGIN_BACKGROUND_IMAGE}") as Texture2D;
        RawImage.FindOnCanvas().texture = texture;
    }

    public static void SetLoading() {
        var index = Math.Floor(UnityEngine.Random.Range(0, 1.0f) * LOADING_SCREENS_LENGTH);
        var imageName = "loading" + ( index < 10 ? "0" + $"{index}" : $"{index}" ) + ".jpg";
        Texture2D texture = FileManager.Load($"{DBManager.INTERFACE_PATH}{imageName}") as Texture2D;
        RawImage.FindOnCanvas().texture = texture;
    }
}
