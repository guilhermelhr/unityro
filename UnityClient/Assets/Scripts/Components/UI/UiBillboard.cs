using UnityEditor;
using UnityEngine;

public class UiBillboard : MonoBehaviour {

    private Camera MainCamera;

    private void Awake() {
        MainCamera = Camera.main;
    }


    private void Update() {
        if (MainCamera == null) {
            MainCamera = Camera.main;
        }

        transform.rotation = Camera.main.transform.rotation;
    }
}