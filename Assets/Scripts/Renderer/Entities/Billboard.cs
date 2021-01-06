using System;
using UnityEngine;

public class Billboard : MonoBehaviour {

    public void LateUpdate() {
        var rotation = Core.MainCamera.transform.localRotation.y;
        transform.localRotation = new Quaternion(transform.localRotation.x, rotation, transform.localRotation.z, transform.localRotation.w);

        var y = 1 / Mathf.Cos(ROCamera.Instance.Altitude * Mathf.Deg2Rad);
        transform.localScale = new Vector3(1, y, 1);
    }
}