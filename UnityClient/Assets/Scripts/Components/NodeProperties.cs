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
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(textureName);
        var directory = Path.GetDirectoryName(textureName);
        var path = Path.Combine("data", "texture", directory, $"{nameWithoutExtension}.png").SanitizeForAddressables();
        var texture = await Addressables.LoadAssetAsync<Texture2D>(path).Task;

        if (texture == null) {
            var filename = nameWithoutExtension.ToLowerInvariant();
            var newPath = Path.Combine("data", "texture", directory, $"{filename}.png").SanitizeForAddressables();
            texture = await Addressables.LoadAssetAsync<Texture2D>(newPath).Task;
        }

        GetComponent<MeshRenderer>().material.mainTexture = texture;
    }
}
