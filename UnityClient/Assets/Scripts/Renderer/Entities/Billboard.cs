using UnityEngine;
using UnityRO.GameCamera;

public class Billboard : MonoBehaviour {

    private GameManager GameManager;

    private void Awake() {
        GameManager = FindObjectOfType<GameManager>();
    }

    public void LateUpdate() {
        transform.localRotation = Camera.main.transform.rotation;
    }
}