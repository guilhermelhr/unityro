using UnityEngine;

public class Billboard : MonoBehaviour {
    public void LateUpdate() {
        transform.localRotation = Core.MainCamera.transform.rotation;
    }
}
