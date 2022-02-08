using UnityEngine;
using System.Collections;
using System.IO;
using ROIO;

public class NodeProperties : MonoBehaviour
{
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
        StartCoroutine(LoadTexture());
    }

    private IEnumerator LoadTexture() {
        var extension = Path.GetExtension(textureName);
        var nameWithoutExtension = textureName.Substring(0, textureName.IndexOf(extension));
        var request = Resources.LoadAsync<Texture2D>(Path.Combine("Textures", "data", "texture", nameWithoutExtension));

        while(!request.isDone) {
            yield return 0;
        }

        GetComponent<MeshRenderer>().material.mainTexture = request.asset as Texture2D;
    }
}
