using ROIO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour {

    private const int LOADING_SCREENS_LENGTH = 10;
        
    public static void SetImage(string imageName) {
        Texture2D texture = FileManager.Load($"{DBManager.INTERFACE_PATH}{imageName}") as Texture2D;
        RawImage image = (GameObject.Find("Screen") ?? GameObject.Find("Canvas")).GetComponent<RawImage>();
        image.texture = texture;
    }

    public static void setLoading() {
        var index = Math.Floor(UnityEngine.Random.Range(0, 1.0f) * LOADING_SCREENS_LENGTH);
        var imageName = "loading" + ( index < 10 ? "0" + $"{index}" : $"{index}" ) + ".jpg";
        Texture2D texture = FileManager.Load($"{DBManager.INTERFACE_PATH}{imageName}") as Texture2D;
        RawImage image = (GameObject.Find("Screen") ?? GameObject.Find("Canvas")).GetComponent<RawImage>();
        image.texture = texture;
    }
}
