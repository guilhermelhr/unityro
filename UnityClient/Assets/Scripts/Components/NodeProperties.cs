using System;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class NodeProperties : MonoBehaviour {
    //hierarchy
    public int nodeId;
    public string parentName;
    public string mainName;
    public string textureName;

    internal bool isChild {
        get { return !string.IsNullOrEmpty(parentName) && !parentName.Equals(mainName); }
    }

    public void SetTextureName(string textureName) {
        this.textureName = textureName;
    }

    private void Start() {
        LoadTexture();
    }

    private async void LoadTexture() {
        var extension = Path.GetExtension(textureName);
        var nameWithoutExtension = textureName.Substring(0, textureName.IndexOf(extension)).SanitizeForAddressables();

        try {
            var texture = await Addressables.LoadAssetAsync<Texture2D>($"data/texture/{nameWithoutExtension}.png");
            if (texture == null) {
                var filename = Path.GetFileNameWithoutExtension(textureName).ToLowerInvariant();
                var oldPath = Path.GetDirectoryName(textureName);
                var newPath = $"data/texture/{oldPath}/{filename}.png";
                texture = await Addressables.LoadAssetAsync<Texture2D>(newPath);
            }

            GetComponent<MeshRenderer>().material.mainTexture = texture;
        } catch (Exception ex) {
            Debug.LogError(ex);
        }
    }
}
