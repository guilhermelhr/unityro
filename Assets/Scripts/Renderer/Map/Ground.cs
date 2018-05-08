
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Ground {
    private Mesh[] meshes;
    private Texture2D atlas;
    private Texture2D lightmap;
    private Texture2D tintmap;
    private Material material = (Material) Resources.Load("GroundMaterial", typeof(Material));

    public struct Vertex {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 texCoord;
        public Vector2 lightCoord;
        public Vector2 tileCoord;
    }

    public void Render() {
        GameObject ground = new GameObject("_Ground");
        ground.transform.parent = MapRenderer.mapParent.transform;

        for(int i = 0; i < meshes.Length; i++){
            Mesh mesh = meshes[i];
            GameObject gameObject = new GameObject("Ground[" + i + "]");
            gameObject.transform.parent = ground.transform;
            var mf = gameObject.AddComponent<MeshFilter>();
            mf.mesh = mesh;
            var mr = gameObject.AddComponent<MeshRenderer>();
            mr.material = material;
            mr.material.mainTexture = atlas;
            mr.material.SetTexture("_Tintmap", tintmap);
            mr.material.SetTexture("_Lightmap", lightmap);
            
            Vector3 scale = gameObject.transform.localScale;
            scale.Set(1f, -1f, 1f);
            gameObject.transform.localScale = scale;

            //smooth out mesh
            NormalSolver.RecalculateNormals(mf.mesh, 60);

            //avoid z fighting between ground and models
            gameObject.transform.Translate(0, -0.002f, 0);
        }
    }

    public void InitTextures(GND.Mesh compiledMesh) {
        var textures = compiledMesh.textures;
        var count = textures.Length;
        var _width = Math.Round(Math.Sqrt(count));
        int width = (int) Math.Pow(2, Math.Ceiling(Math.Log(_width * 258) / Math.Log(2)));
        int height = (int) Math.Pow(2, Math.Ceiling(Math.Log(Math.Ceiling(Math.Sqrt(count)) * 258) / Math.Log(2)));

        RenderTexture renderTexture = RenderTexture.GetTemporary(width, height);
        
        RenderTexture.active = renderTexture;

        //TODO fix ground texture seams when using clear bg color (is it possible?)
        //^ bleeding textures helped but it's not perfect
        GL.Clear(false, true, Color.clear);

        material.SetPass(0);
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, width, 0, height);

        for(int i = 0; i < count; i++) {
            var texture = FileManager.Load(textures[i]) as Texture2D;
            var x = (int) (i % _width) * 258;
            var y = (int) Math.Floor(i / _width) * 258;

            Graphics.DrawTexture(new Rect(x, y, 258, 258), texture);
            Graphics.DrawTexture(new Rect(x + 1, y + 1, 256, 256), texture);
        }

        GL.PopMatrix();
        GL.End();

        atlas = new Texture2D(width, height, TextureFormat.RGBAFloat, true);
        atlas.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        float start = Time.realtimeSinceStartup;
        BleedTexture(atlas);

        float end = Time.realtimeSinceStartup;
        Debug.Log("Gnd texture bleeding time: " + (end - start));

        atlas.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);

        lightmap = compiledMesh.lightmap;
        tintmap = compiledMesh.tileColor;
    }

    public struct BleedJob : IJob
    {
        [ReadOnly]
        public bool reversed;
        public NativeArray<Color> pixels;

        public void Execute() {            
            Color lastNonClear = Color.clear;
            
            for(int i = 0; i < pixels.Length; i++) {
                Color pixel = pixels[i];
                if(pixel != Color.clear) {
                    lastNonClear = pixel;
                } else if(lastNonClear != Color.clear) {
                    pixels[i] = lastNonClear;
                }
            }
        }
    }

    private void BleedTexture(Texture2D atlas) {
        var handles = new List<JobHandle>();
        var newPixels = new NativeArray<Color>[atlas.height];

        for(int y = 0; y < atlas.height; y++) {
            Color[] row = atlas.GetPixels(0, y, atlas.width, 1);
            var array = newPixels[y] = new NativeArray<Color>(row, Allocator.Temp);

            var jobData = new BleedJob();
            jobData.pixels = array;

            handles.Add(jobData.Schedule());
        }

        foreach(var handle in handles) {
            handle.Complete();
        }

        handles.Clear();

        for(int y = 0; y < atlas.height; y++) {
            atlas.SetPixels(0, y, atlas.width, 1, newPixels[y].ToArray());
            newPixels[y].Dispose();
        }

        for(int x = 0; x < atlas.width; x++) {
            Color[] row = atlas.GetPixels(x, 0, 1, atlas.height);
            var array = newPixels[x] = new NativeArray<Color>(row, Allocator.Temp);

            var jobData = new BleedJob();
            jobData.pixels = array;

            handles.Add(jobData.Schedule());
        }

        foreach(var handle in handles) {
            handle.Complete();
        }

        for(int x = 0; x < atlas.width; x++) {
            atlas.SetPixels(x, 0, 1, atlas.height, newPixels[x].ToArray());
            newPixels[x].Dispose();
        }
    }

    public void BuildMesh(GND.Mesh compiledMesh) {
        meshes = new Mesh[(int) Math.Ceiling(compiledMesh.meshVertCount / (float) MapRenderer.MAX_VERTICES)];

        for(int nMesh = 0; nMesh < meshes.Length; nMesh++) {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector2> tintUv = new List<Vector2>();
            List<Vector2> lightmapUv = new List<Vector2>();

            float[] vertexData = new float[12];
            int v = 0, h = 0;
            for(int i = 0, ended = 0; vertices.Count < MapRenderer.MAX_VERTICES && ended == 0; i++) {
                Vertex[] vs = new Vertex[4];
                for(int j = 0; j < 4; j++) {
                    var vIndex = i * 4 + j + nMesh * MapRenderer.MAX_VERTICES;

                    if(vIndex * vertexData.Length >= compiledMesh.mesh.Length) {
                        ended = 1;
                        break;
                    }

                    Array.ConstrainedCopy(compiledMesh.mesh, vIndex * vertexData.Length, vertexData, 0, vertexData.Length);
                    Vertex vertex = BuildVertex(vertexData);
                    vs[j] = vertex;
                    vertices.Add(vertex.position);
                    normals.Add(vertex.normal);
                    uv.Add(vertex.texCoord);
                    lightmapUv.Add(vertex.lightCoord);
                    tintUv.Add(vertex.tileCoord);
                }

                if(ended == 0) {
                    if(vs[0].normal.z == 1) {
                        v++;
                        triangles.AddRange(new int[] {
                            i * 4 + 0, i * 4 + 1, i * 4 + 2, //left triangle                              
                            i * 4 + 2, i * 4 + 3, i * 4 + 0, //right triangle      
                        });
                    } else if(vs[0].normal.x == 1) {
                        v++;
                        triangles.AddRange(new int[] {
                            i * 4 + 0, i * 4 + 2, i * 4 + 1, //left triangle  
                            i * 4 + 2, i * 4 + 3, i * 4 + 1, //right triangle       
                        });
                    } else {
                        h++;
                        triangles.AddRange(new int[] {
                            i * 4 + 0, i * 4 + 2, i * 4 + 3, //left triangle  
                            i * 4 + 0, i * 4 + 1, i * 4 + 2, //right triangle                      
                        });
                    }
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uv.ToArray();
            mesh.uv2 = lightmapUv.ToArray();
            mesh.uv3 = tintUv.ToArray();

            meshes[nMesh] = mesh;
        }
    }

    private Vertex BuildVertex(float[] vertexData) {
        Vertex v = new Vertex();

        v.position = new Vector3(vertexData[0], vertexData[1], vertexData[2]);
        v.normal = new Vector3(vertexData[3], vertexData[4], vertexData[5]);
        v.texCoord = new Vector2(vertexData[6], vertexData[7]);
        v.lightCoord = new Vector2(vertexData[8], vertexData[9]);
        v.tileCoord = new Vector2(vertexData[10], vertexData[11]);

        return v;
    }
}
