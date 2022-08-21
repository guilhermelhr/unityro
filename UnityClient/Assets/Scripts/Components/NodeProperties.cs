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

        var texture = await Addressables.LoadAssetAsync<Texture2D>($"data/texture/{nameWithoutExtension}.png").Task;
        if (texture == null) {
            var filename = Path.GetFileNameWithoutExtension(textureName).ToLowerInvariant();
            var oldPath = Path.GetDirectoryName(textureName);
            var newPath = Path.Combine("data", "texture", oldPath, $"{filename}.png").SanitizeForAddressables();
            texture = await Addressables.LoadAssetAsync<Texture2D>(newPath).Task;
        }

        GetComponent<MeshRenderer>().material.mainTexture = texture;
    }
}
