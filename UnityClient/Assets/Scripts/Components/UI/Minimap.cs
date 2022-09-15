using ROIO;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    [SerializeField] private RawImage PlayerIndicator;
    
    private Texture2D MapThumbTexture;
    private Texture2D PlayerIndicatorTexture;

    private RawImage MapThumb;
    private string CurrentMap;
    private int CurrentZoom = 1;

    // Start is called before the first frame update
    async void Start() {
        MapThumb = GetComponent<RawImage>();

        PlayerIndicatorTexture = await Addressables.LoadAssetAsync<Texture2D>($"{DBManager.INTERFACE_PATH}map/map_arrow.png").Task;
        Session.OnMapChanged += OnMapChanged;
    }

    private void OnDestroy() {
        Session.OnMapChanged -= OnMapChanged;
    }

    private async void OnMapChanged(string mapName) {
        CurrentMap = Path.GetFileNameWithoutExtension(mapName);
        MapThumbTexture = await Addressables.LoadAssetAsync<Texture2D>($"{DBManager.INTERFACE_PATH}map/{CurrentMap}.png").Task;

        if (MapThumbTexture == null) {
            return;
        }

        MapThumb.texture = MapThumbTexture;
        var size = CalculateNewSize(MapThumbTexture.width, MapThumbTexture.height, 128, 128);
        (transform as RectTransform).sizeDelta = size;
    }

    private void Update() {
        if (CurrentMap != null && MapThumbTexture == null) {
            OnMapChanged(CurrentMap);
        }
    }

    private Vector2 CalculateNewSize(int srcWidth, int srcHeight, int maxWidth, int maxHeight) {
        var ratio = Mathf.Min((float) maxWidth / (float) srcWidth, (float) maxHeight / (float) srcHeight);
        return new Vector2(srcWidth * ratio, srcHeight * ratio);
    }

}
