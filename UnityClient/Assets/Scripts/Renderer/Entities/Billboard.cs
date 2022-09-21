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

    private void MaybeUpdateScale() {
        if (CharacterCamera.ROCamera != null) {
            float pitch = CharacterCamera.ROCamera.Pitch;
            float y = 1 / Mathf.Cos(pitch);
            transform.localScale = new Vector3(1, y, 1);
        }
    }

    private void UpdateRotation() {
        transform.localRotation = GameManager.MainCamera.transform.rotation;
        Vector3 euler = transform.localEulerAngles;
        euler.x = 0;
        transform.localEulerAngles = euler;
    }
}