using System;
using UnityEngine;

public class Billboard : MonoBehaviour {

    public void LateUpdate() {
        transform.localRotation = Core.MainCamera.transform.rotation;
        var euler = transform.localEulerAngles;
        euler.x = 0;
        transform.localEulerAngles = euler;

        var y = 1 / Mathf.Cos((ROCamera.Instance?.Altitude ?? 0) * Mathf.Deg2Rad);
        transform.localScale = new Vector3(1, y, 1);
    }
}