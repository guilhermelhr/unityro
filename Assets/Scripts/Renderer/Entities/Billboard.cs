using UnityEngine;
using UnityRO.GameCamera;

public class Billboard : MonoBehaviour {

    public void LateUpdate() {
        UpdateRotation();
        MaybeUpdateScale();
    }

    private void MaybeUpdateScale() {
        if (CharacterCamera.ROCamera != null) {
            float pitch = CharacterCamera.ROCamera.Pitch;
            float y = 1 / Mathf.Cos(pitch);
            transform.localScale = new Vector3(1, y, 1);
        }
    }

    private void UpdateRotation() {
        transform.localRotation = Core.MainCamera.transform.rotation;
        Vector3 euler = transform.localEulerAngles;
        euler.x = 0;
        transform.localEulerAngles = euler;
    }
}