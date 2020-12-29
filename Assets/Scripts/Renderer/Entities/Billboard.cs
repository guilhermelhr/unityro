using System;
using UnityEngine;

public class Billboard : MonoBehaviour {

    private float angle = 30;

    public void LateUpdate() {
        transform.localRotation = new Quaternion(transform.localRotation.x, Core.MainCamera.transform.rotation.y, transform.localRotation.z, transform.localRotation.w);
        transform.localScale = new Vector3(1, 1 + Mathf.Cos(angle), 1);
    }
}