using UnityEngine;
using UnityRO.GameCamera;

public class Billboard : MonoBehaviour {

    public void LateUpdate() {
        transform.localRotation = Core.MainCamera.transform.rotation;
        Vector3 euler = transform.localEulerAngles;
        euler.x = 0;
        transform.localEulerAngles = euler;
        float pitch = CharacterCamera.ROCamera?.Pitch ?? 1f;
        float y = 1 / Mathf.Cos(pitch);
        transform.localScale = new Vector3(1, y, 1);
    }
}