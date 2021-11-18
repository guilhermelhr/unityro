using UnityEngine;
using UnityEngine.UI;

public static class RawImageExtenions {
    public static RawImage FindOnCanvas(this RawImage ri) {
        return (GameObject.Find("Screen") ?? GameObject.Find("Canvas")).GetComponent<RawImage>();
    }
}
