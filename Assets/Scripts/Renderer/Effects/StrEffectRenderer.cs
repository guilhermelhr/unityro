using ROIO.Models.FileTypes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StrEffectRenderer : MonoBehaviour {

    private STR Anim;

    private bool isInit;

    private List<GameObject> layerObjects = new List<GameObject>();
    private List<MeshRenderer> layerRenderers;
    private List<MeshFilter> layerFilters;
    private List<Mesh> layerMeshes;

    private static Vector3[] tempPositions = new Vector3[4];
    private static Vector2[] tempPositions2 = new Vector2[4];
    private static Vector2[] tempUvs = new Vector2[4];
    private static Vector2[] tempUvs2 = new Vector2[4];
    private static Vector3[] tempNormals = new Vector3[4];
    private static int[] tempTris = new int[6];
    private float[] angles;

    private Dictionary<string, Material> materials = new Dictionary<string, Material>(8);

    private float time;
    private int frame;

    private Material GetEffectMaterial(int layer, int srcBlend, int destBlend) {
        var hash = $"{Anim.name}-{layer}-{srcBlend.ToString()}-{destBlend.ToString()}";
        if(materials.TryGetValue(hash, out var val))
            return val;

        var mat = new Material(Shader.Find("Ragnarok/EffectShader"));
        mat.SetFloat("_SrcBlend", (float)BlendMode.One);
        mat.SetFloat("_DstBlend", (float)BlendMode.One);
        mat.SetFloat("_ZWrite", 0);
        mat.SetFloat("_Cull", 0);

        mat.SetTexture("_MainTex", Anim.Atlas);
        mat.SetColor("_Color", Color.white);

        return mat;
    }

    public void Initialize(STR animation) {
        Anim = animation;

        layerObjects = new List<GameObject>(Anim.layers.Length);
        layerRenderers = new List<MeshRenderer>(Anim.layers.Length);
        layerMeshes = new List<Mesh>(Anim.layers.Length);
        layerFilters = new List<MeshFilter>(Anim.layers.Length);
        angles = new float[Anim.layers.Length];

        for(var i = 0; i < Anim.layers.Length; i++) {
            var go = new GameObject("Layer " + i);
            var mr = go.AddComponent<MeshRenderer>();
            var mf = go.AddComponent<MeshFilter>();
            go.transform.SetParent(transform, false);

            var mesh = new Mesh();

            layerObjects.Add(go);
            layerRenderers.Add(mr);
            layerFilters.Add(mf);
            layerMeshes.Add(mesh);
            angles[i] = -1;
        }

        time = 0;
        frame = -1;
        isInit = true;
    }

    // Use this for initialization
    private void Awake() {
        if(Anim != null)
            Initialize(Anim);
    }

    private void UpdateMesh(MeshFilter mf, Mesh mesh, Vector2[] pos, Vector2[] uvs, float angle, int imageId) {
        if(pos.Length > 4 || uvs.Length > 4)
            Debug.LogError("WHOA! Animation " + Anim.name + " has more than 4 verticies!");

        var bounds = Anim.AtlasRects[imageId];

        for(var i = 0; i < 4; i++) {
            var p = Rotate(pos[i], -angle * Mathf.Deg2Rad);
            tempPositions[i] = new Vector3(p.x, p.y, 0) / 35f;

            var uvx = uvs[i].x; //.Remap(0, 1, bounds.xMin, bounds.xMax);
            var uvy = uvs[i].y; //.Remap(0, 1, bounds.yMin, bounds.yMax);

            tempUvs[i] = new Vector2(uvx, uvy);
            tempNormals[i] = Vector3.back;
        }

        tempUvs[2] = new Vector2(bounds.xMin, bounds.yMin);
        tempUvs[3] = new Vector2(bounds.xMax, bounds.yMin);
        tempUvs[0] = new Vector2(bounds.xMin, bounds.yMax);
        tempUvs[1] = new Vector2(bounds.xMax, bounds.yMax);

        tempTris = new int[] { 
            0, 1, 2,
            1, 3, 2
        };

        mesh.vertices = tempPositions;
        mesh.uv = tempUvs;
        mesh.normals = tempNormals;
        mesh.triangles = tempTris;

        mf.sharedMesh = mesh;
    }

    private void UpdateLayerData(GameObject go, Material mat, Vector2 pos, Color color) {
        go.transform.localPosition = new Vector3((pos.x - 320f) / 35f + 0.5f, -(pos.y - 360f) / 35f, 0);
        go.transform.localScale = new Vector3(1f, 1f, 1f);
        mat.SetColor("_Color", color);
    }

    private bool UpdateAnimationLayer(int layerNum) {
        var layer = Anim.layers[layerNum];

        var lastFrame = 0;
        var lastSource = 0;
        var startAnim = -1;
        var nextAnim = -1;

        for(var i = 0; i < layer.animations.Length; i++) {
            var a = layer.animations[i];
            if(a.frame < frame) {
                if(a.type == 0)
                    startAnim = i;
                if(a.type == 1)
                    nextAnim = i;
            }

            lastFrame = Mathf.Max(lastFrame, a.frame);
            if(a.type == 0)
                lastSource = Mathf.Max(lastSource, a.frame);
        }

        if(startAnim < 0 || (nextAnim < 0 && lastFrame < frame))
            return false;

        var from = layer.animations[startAnim];
        STR.Animation to = null;

        if(nextAnim >= 0)
            to = layer.animations[nextAnim];
        var delta = frame - from.frame;
        var blendSrc = (int)from.srcAlpha;
        var blendDest = (int)from.destAlpha;

        var mat = GetEffectMaterial(layerNum, blendSrc, blendDest);
        var go = layerObjects[layerNum];
        var mr = layerRenderers[layerNum];
        var mf = layerFilters[layerNum];
        var mesh = layerMeshes[layerNum];
        mr.material = mat;

        if(nextAnim != startAnim + 1 || to?.frame != from.frame) {
            if(to != null && lastSource <= from.frame)
                return false;

            var fixedFrame = layer.texturesIds[(int)from.animFrame];
            UpdateMesh(mf, mesh, from.xy, from.uv, from.angle, fixedFrame);
            UpdateLayerData(go, mat, from.position, from.color);
            return true;
        }

        var prog = Mathf.InverseLerp(from.frame, to.frame, frame);

        for(var i = 0; i < 4; i++) {
            tempPositions2[i] = from.xy[i] + to.xy[i] * delta;
            tempUvs2[i] = from.uv[i] + to.uv[i] * delta;
        }

        //Debug.Log("from: " + from.Position + " to: " + to.Position + " delta:" + delta);
        var pos = from.position + to.position * delta;
        var angle = from.animFrame + to.angle * delta;
        var color = from.color + to.color * delta;
        //color.a *= 0.5f;
        //Debug.Log(color);

        var frameId = 0;

        switch(to.animType) {
            case 1:
                frameId = Mathf.FloorToInt(from.animFrame + to.animFrame * delta);
                break;
            case 2:
                frameId = Mathf.FloorToInt(Mathf.Min(from.animFrame + to.delay * delta, layer.textures.Length - 1));
                break;
            case 3:
                frameId = Mathf.FloorToInt((from.animFrame + to.delay * delta) % layer.textures.Length);
                break;
            case 4:
                frameId = Mathf.FloorToInt((from.animFrame - to.delay * delta) % layer.textures.Length);
                break;
        }

        var texIndex = layer.texturesIds[frameId];

        UpdateMesh(mf, mesh, tempPositions2, tempUvs2, angle, texIndex);
        UpdateLayerData(go, mat, pos, color);

        return true;
    }

    private void UpdateAnimationFrame() {
        for(var i = 0; i < Anim.layers.Length; i++) {
            if(Anim.layers[i].animations.Length == 0)
                continue;

            var res = UpdateAnimationLayer(i);
            layerObjects[i].SetActive(res);
        }
    }

    // Update is called once per frame
    private void Update() {
        if(!isInit)
            return;

        time += Time.deltaTime;
        var newFrame = Mathf.FloorToInt(time * Anim.fps);
        if(newFrame == frame)
            return;

        //Debug.Log(frame);

        frame = newFrame;

        if(frame > Anim.maxKey) {
            Destroy(gameObject);
            return;
        }

        UpdateAnimationFrame();
    }

    void OnDestroy() {
        if(!isInit)
            return;
        foreach(var m in layerMeshes)
            Destroy(m);
    }

    public Vector2 Rotate(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}