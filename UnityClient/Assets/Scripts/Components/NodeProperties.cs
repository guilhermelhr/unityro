using UnityEngine;
using System.Collections;
using System.IO;
using ROIO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using Assets.Scripts.Core;
using System;

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

            GetComponent<MeshRenderer>().material.mainTexture = texture;
        } catch(Exception ex) {
            Debug.LogError(ex);
        }
    }
}
