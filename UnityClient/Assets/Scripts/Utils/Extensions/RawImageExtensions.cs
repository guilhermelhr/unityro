using ROIO;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public static class RawImageExtenions {
    
    private const string LOGIN_BACKGROUND_IMAGE = "bgi_temp.bmp";
    private const int LOADING_SCREENS_LENGTH = 10;
    
    public async static Task SetLoginBackground(this RawImage image) {
        Texture2D texture = await Addressables.LoadAssetAsync<Texture2D>($"{DBManager.INTERFACE_PATH}{LOGIN_BACKGROUND_IMAGE}").Task;
        image.texture = texture;
    }
    
    public async static Task SetLoading(this RawImage image) {
        double index = Math.Floor(UnityEngine.Random.Range(0, 1.0f) * LOADING_SCREENS_LENGTH);
        string imageName = "loading" + ( index < 10 ? "0" + $"{index}" : $"{index}" ) + ".jpg";
        Texture2D texture = await Addressables.LoadAssetAsync<Texture2D>($"{DBManager.INTERFACE_PATH}{imageName}").Task;
        image.texture = texture;
    }
}
