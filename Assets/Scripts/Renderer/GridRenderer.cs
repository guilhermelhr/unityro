using ROIO;
using System;
using UnityEngine;

public class GridRenderer : MonoBehaviour {

    private Texture2D gridIcon;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;
    private Material material;

    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;

    private void Awake() {

    }

    public void Start() {
        LoadGridTexture();
    }

    private void Update() {
        if (gridIcon == null) {
            gridIcon = (Texture2D)FileManager.Load("data/texture/grid.tga");
        }

        var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 150, LayerMask.GetMask("Ground"))) {
            var target = new Vector2(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
            RenderGridSelector(target);
        }
    }

    private void LoadGridTexture() {
        material = new Material(Shader.Find("Unlit/WalkableShader"));
        material.SetFloat("_Glossiness", 0f);
        material.mainTexture = gridIcon;
        material.color = Color.red;
        material.name = "Cursor material";
        material.doubleSidedGI = false;
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
        material.enableInstancing = false;

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    private void RenderGridSelector(Vector2 targetPosition) {
        var cell = Core.PathFinding.GetCell(targetPosition.x, targetPosition.y);
        GetClosestTileTopToPoint(targetPosition, out var target);

        if (!Core.PathFinding.IsWalkable(target.x, target.y)) {
            //Disable renderer
            return;
        } else {
            material.mainTexture = gridIcon;
            material.color = new Color(50 / 255f, 240 / 255f, 160 / 255f, 0.6f);
        }

        if (vertices == null) {
            mesh = new Mesh();
            vertices = new Vector3[4];
            uvs = new Vector2[4];
            triangles = new[] { 0, 1, 2, 1, 3, 2 };
        } else {
            mesh.Clear();
        }

        var offset = new Vector3(0f, 0.015f, 0f);

        vertices[0] = new Vector3(target.x, cell.Heights[0] / 5f, target.y + 1) + offset;
        vertices[1] = new Vector3(target.x + 1, cell.Heights[1] / 5f, target.y + 1) + offset;
        vertices[2] = new Vector3(target.x, cell.Heights[2] / 5f, target.y) + offset;
        vertices[3] = new Vector3(target.x + 1, cell.Heights[3] / 5f, target.y) + offset;

        uvs[0] = new Vector2(0, 1);
        uvs[1] = new Vector2(1, 1);
        uvs[2] = new Vector2(0, 0);
        uvs[3] = new Vector2(1, 0);

        //var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        meshFilter.sharedMesh = mesh;
        meshRenderer.enabled = true;
    }

    public bool GetClosestTileTopToPoint(Vector2 point, out Vector2Int tile) {
        tile = new Vector2Int();

        var x = Mathf.FloorToInt((point.x - transform.position.x));
        var y = Mathf.FloorToInt((point.y - transform.position.z));

        //Debug.Log(WalkData.Width + " " + WalkData.Height + " " + x + " " + y);

        if (x < 0 || x >= (Core.PathFinding.Altitude.getWidth()) || y < 0 || y >= (Core.PathFinding.Altitude.getHeight()))
            return false;

        tile = new Vector2Int(x, y);
        return true;
    }
}