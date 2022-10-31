using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UiBillboard : MonoBehaviour {

    private Camera MainCamera;

    private void Awake() {
        MainCamera = Camera.main;
    }

    private void Update() {
        if (MainCamera == null) {
            MainCamera = Camera.main;
        }

        transform.LookAt(transform.position + MainCamera.transform.forward);
    }
}