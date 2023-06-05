using System;
using UnityEngine;

public class Billboard : MonoBehaviour {

    private Camera _camera;
    
    private void Awake() {
        _camera = Camera.main;
    }

    public void Update() {
        if (_camera == null) {
            _camera = Camera.main;
        }
        
        transform.localRotation = _camera.transform.rotation;
    }
}