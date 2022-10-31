using UnityEngine;

public class Billboard : MonoBehaviour {

    private GameManager GameManager;

    private void Awake() {
        GameManager = FindObjectOfType<GameManager>();
    }

    public void Update() {
        transform.localRotation = Camera.main.transform.rotation;
    }
}