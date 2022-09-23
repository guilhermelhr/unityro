using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public static class RawImageExtenions {

    private const string LOGIN_BACKGROUND_IMAGE = "bgi_temp.png";
    private const int LOADING_SCREENS_LENGTH = 10;

    public static void SetLoginBackground(this RawImage image) {
        Texture2D texture = Addressables.LoadAssetAsync<Texture2D>($"{DBManager.INTERFACE_PATH}{LOGIN_BACKGROUND_IMAGE}").WaitForCompletion();

        if (!image.IsDestroyed()) {
            image.texture = texture;
        }
    }

    public static void SetLoading(this RawImage image) {
        double index = Math.Floor(UnityEngine.Random.Range(0, 1.0f) * LOADING_SCREENS_LENGTH);
        string imageName = "loading" + (index < 10 ? "0" + $"{index}" : $"{index}") + ".png";
        Texture2D texture = Addressables.LoadAssetAsync<Texture2D>($"{DBManager.INTERFACE_PATH}{imageName}").WaitForCompletion();
        
        if (!image.IsDestroyed()) {
            image.texture = texture;
        }
    }
}
