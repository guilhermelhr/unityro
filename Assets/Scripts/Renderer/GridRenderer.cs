using System;
using UnityEngine;

public class GridRenderer : MonoBehaviour {

    private Renderer Renderer;

    private void Awake() {
        Core.OnRayCastHit += this.RenderGridSelector;
        Core.OnGrfLoaded += this.LoadGridTexture;
        Renderer = GetComponent<Renderer>();
    }

    private void LoadGridTexture() {
        var gridIcon = (Texture)FileManager.Load("data/texture/grid.tga");
        Renderer.sharedMaterial.SetTexture("_mainTex", gridIcon);
    }

    private void OnDestroy() {
        Core.OnRayCastHit -= this.RenderGridSelector;
        Core.OnGrfLoaded -= this.LoadGridTexture;
    }

    private void RenderGridSelector(Vector3 targetPosition) {
        gameObject.transform.position = new Vector3(Mathf.FloorToInt(targetPosition.x), gameObject.transform.position.y, Mathf.FloorToInt(targetPosition.z));
    }
}