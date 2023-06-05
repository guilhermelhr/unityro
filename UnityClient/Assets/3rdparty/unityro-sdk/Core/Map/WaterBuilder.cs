
using Assets.Scripts.Renderer.Map;
using ROIO;
using ROIO.Models.FileTypes;
using ROIO.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaterBuilder {
    private RSW.WaterInfo info;

    private Texture2D[] textures = new Texture2D[32];
    private Mesh[] meshes;
    private MeshRenderer[] renderers;
    private GameObject[] objects;
    private int currTexture = -1;

    public void InitTextures(GND.Mesh compiledMesh, RSW.WaterInfo waterInfo) {
        info = waterInfo;

        for (int i = 0; i < 32; i++) {
            var texture = FileManager.Load(waterInfo.images[i]) as Texture2D;
            textures[i] = texture;
        }
    }

    public void BuildMesh(GND.Mesh compiledMesh) {
        meshes = new Mesh[(int) Math.Ceiling(compiledMesh.waterVertCount / (float) int.MaxValue)];
        var material = new Material(Shader.Find("Custom/WaterShader"));

        for (int nMesh = 0; nMesh < meshes.Length; nMesh++) {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();

            float[] vertexData = new float[5];
            for (int i = 0, ended = 0; vertices.Count < int.MaxValue && ended == 0; i++) {
                Ground.Vertex[] vs = new Ground.Vertex[4];
                for (int j = 0; j < 4; j++) {
                    var vIndex = i * 4 + j + nMesh * int.MaxValue;

                    if (vIndex * vertexData.Length >= compiledMesh.waterMesh.Length) {
                        ended = 1;
                        break;
                    }

                    Array.ConstrainedCopy(compiledMesh.waterMesh, vIndex * vertexData.Length, vertexData, 0, vertexData.Length);
                    Ground.Vertex vertex = BuildVertex(vertexData);
                    vs[j] = vertex;
                    vertices.Add(vertex.position);
                    normals.Add(vertex.normal);
                    uv.Add(vertex.texCoord);
                }

                if (ended == 0) {
                    triangles.AddRange(new int[] {
                        i * 4 + 0, i * 4 + 2, i * 4 + 3, //left triangle  
                        i * 4 + 0, i * 4 + 1, i * 4 + 2, //right triangle                      
                    });
                }
            }

            Mesh mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uv.ToArray();

            meshes[nMesh] = mesh;
        }

        objects = new GameObject[meshes.Length];

        renderers = new MeshRenderer[meshes.Length];

        GameObject water = new GameObject("_Water");
        water.transform.parent = GameObject.FindObjectOfType<GameMap>().transform;

        for (int i = 0; i < meshes.Length; i++) {
            Mesh mesh = meshes[i];

            GameObject gameObject = new GameObject("Water[" + i + "]");
            var waterRenderer = gameObject.AddComponent<WaterRenderer>();
            waterRenderer.SetWaterInfo(info);
            gameObject.transform.parent = water.transform;
            gameObject.layer = 4;
            var mf = gameObject.AddComponent<MeshFilter>();
            mf.mesh = mesh;
            var mr = gameObject.AddComponent<MeshRenderer>();
            renderers[i] = mr;
            mr.sharedMaterial = material;
            mr.sharedMaterial.mainTexture = textures[0];

            Vector3 scale = gameObject.transform.localScale;
            scale.Set(1f, -1f, 1f);
            gameObject.transform.localScale = scale;

            objects[i] = gameObject;
        }

        material.SetFloat("Wave Height", info.waveHeight);
        material.SetFloat("Wave Pitch", info.wavePitch);
    }

    public static Ground.Vertex BuildVertex(float[] vertexData) {
        Ground.Vertex v = new Ground.Vertex();

        v.position = new Vector3(vertexData[0], vertexData[1], vertexData[2]);
        v.normal = Vector3.up;
        v.texCoord = new Vector2(vertexData[3], vertexData[4]);

        return v;
    }

    public void Render() {
        float frame = Time.time / (1 / 60f);
        int textureId = (int) Conversions.SafeDivide(frame, info.animSpeed) % 32;

        float offset = frame * info.waveSpeed;
        foreach (MeshRenderer mr in renderers) {
            mr.material.SetFloat("_WaterOffset", offset);
        }

        if (currTexture != textureId) {
            foreach (GameObject gameObject in objects) {
                var mr = gameObject.GetComponent<MeshRenderer>();
                mr.material.mainTexture = textures[textureId];
            }

            currTexture = textureId;
        }
    }
}
