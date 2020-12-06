using System;
using UnityEngine;

public class GridRenderer : MonoBehaviour {

    private Renderer Renderer;

    private void Awake() {
        Core.OnGrfLoaded += this.LoadGridTexture;
        Renderer = GetComponent<Renderer>();
    }

    private void OnDestroy() {
        Core.OnGrfLoaded -= this.LoadGridTexture;
    }

    private void Update() {
        RaycastHit hit;
        var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            var target = new Vector3(Mathf.Floor(hit.point.x), hit.point.y, Mathf.Floor(hit.point.z));
            RenderGridSelector(target);
        }
    }

    private void LoadGridTexture() {
        // TODO: Figure out why this isn't working
        var gridIcon = (Texture)FileManager.Load("data/texture/grid.tga");
        Renderer.sharedMaterial.SetTexture("_mainTex", gridIcon);
    }

    private void RenderGridSelector(Vector3 targetPosition) {
        gameObject.transform.position = new Vector3(Mathf.FloorToInt(targetPosition.x), gameObject.transform.position.y, Mathf.FloorToInt(targetPosition.z));
    }
}