
using System;
using System.Collections.Generic;
using UnityEngine;
using static RSM;

public class Models {
    private CompiledModel[] models;
    private Material material = (Material) Resources.Load("ModelMaterial", typeof(Material));
    private Material material2s = (Material) Resources.Load("ModelMaterial2Sided", typeof(Material));

    public Models(CompiledModel[] models) {
        this.models = models;
    }

    private class MeshData {
        public static int FloatsPerVert = 27;
        public Vector3 position;
        public Vector3 normals;
        public Vector2 uv;
        public float alpha;
        public Mat4 instanceMatrix;
        public int instanceNumber;
        public long twoSided;

        public MeshData(float[] data) {
            position = new Vector3(data[0], data[1], data[2]);
            normals = new Vector3(data[3], data[4], data[5]);
            uv = new Vector2(data[6], -data[7]);
            alpha = data[8];
            float[] mat = new float[16];
            Array.ConstrainedCopy(data, 9, mat, 0, mat.Length);
            instanceMatrix = Mat4.FromValues(mat);
            instanceNumber = (int) data[25];
            twoSided = (long) data[26];
        }
    }

    public void BuildMeshes() {
        GameObject parent = new GameObject("_Models");
        parent.transform.parent = MapRenderer.mapParent.transform;

        foreach(CompiledModel model in models) {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();

            //I'm ignoring unity's vertex limit per mesh here because
            //models hopefully are optimized enough not to surpass 65532 vertexes

            float[] data = new float[MeshData.FloatsPerVert];
            int instanceNumber = -1;
            //Mat4 instanceMatrix = null;

            bool twoSided = false;

            for(int i = 0; i < model.mesh.Length / MeshData.FloatsPerVert; i++) {
                Array.ConstrainedCopy(model.mesh, i * data.Length, data, 0, data.Length);
                MeshData meshData = new MeshData(data);

                vertices.Add(meshData.position);
                normals.Add(meshData.normals);
                uv.Add(meshData.uv);
                //triangles.Add(i);
                
                if(i % 3 == 0) {
                    //8 = flip x
                    //0 = flip z
                    if(meshData.instanceMatrix[0] < 0 && meshData.instanceMatrix[5] < 0 && meshData.instanceMatrix[8] < 0 && meshData.instanceMatrix[10] < 0) {
                        triangles.Add(i + 2);
                        triangles.Add(i + 1);
                        triangles.Add(i + 0);
                    } else if(meshData.instanceMatrix[0] < 0 && meshData.instanceMatrix[5] < 0 && meshData.instanceMatrix[8] == 0 && meshData.instanceMatrix[10] > 0) {
                        triangles.Add(i + 0);
                        triangles.Add(i + 1);
                        triangles.Add(i + 2);
                    } else if(meshData.instanceMatrix[0] < 0 && meshData.instanceMatrix[5] < 0 && meshData.instanceMatrix[8] > 0 && meshData.instanceMatrix[10] > 0) {
                        triangles.Add(i + 0);
                        triangles.Add(i + 1);
                        triangles.Add(i + 2);
                    } else if(meshData.instanceMatrix[0] > 0 && meshData.instanceMatrix[5] < 0 && meshData.instanceMatrix[8] < 0 && meshData.instanceMatrix[10] < 0) {
                        triangles.Add(i + 0);
                        triangles.Add(i + 1);
                        triangles.Add(i + 2);
                    } else if(meshData.instanceMatrix[0] > 0 && meshData.instanceMatrix[5] < 0 && meshData.instanceMatrix[8] == 0 && meshData.instanceMatrix[10] < 0) {
                        triangles.Add(i + 0);
                        triangles.Add(i + 1);
                        triangles.Add(i + 2);
                    } else if(meshData.instanceMatrix[0] > 0 && meshData.instanceMatrix[2] > 0 && meshData.instanceMatrix[8] > 0) {
                        triangles.Add(i + 0);
                        triangles.Add(i + 1);
                        triangles.Add(i + 2);
                    } else if(meshData.instanceMatrix[0] < 0 && meshData.instanceMatrix[8] < 0) {
                        triangles.Add(i + 0);
                        triangles.Add(i + 1);
                        triangles.Add(i + 2);
                    } else { 
                        triangles.Add(i + 2);
                        triangles.Add(i + 1);
                        triangles.Add(i + 0);
                    }
                }

                twoSided |= meshData.twoSided != 0;

                if(instanceNumber == -1) {
                    instanceNumber = meshData.instanceNumber;
                    //instanceMatrix = meshData.instanceMatrix;
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uv.ToArray();
            //meshes.Add(mesh);

            //textures.Add(MapLoader.Cache[model.texture] as Texture2D);

            GameObject gameObject = new GameObject(model.source.name + "[" + instanceNumber + "]");
            gameObject.transform.parent = parent.transform;
            //gameObject.name += " " + instanceMatrix.ToString();
            
            var mf = gameObject.AddComponent<MeshFilter>();
            mf.mesh = mesh;
            var mr = gameObject.AddComponent<MeshRenderer>();
            if(twoSided) {
                mr.material = material2s;
            } else {
                mr.material = material;
            }
            mr.material.mainTexture = FileManager.Load(model.texture) as Texture2D;
        }
    }

    public void Render() {
        /*GameObject models = new GameObject("_Models");

        for(int i = 0; i < meshes.Count; i++) {
            Mesh mesh = meshes[i];

            GameObject gameObject = new GameObject();
            gameObject.transform.parent = models.transform;
            var mf = gameObject.AddComponent<MeshFilter>();
            mf.mesh = mesh;
            var mr = gameObject.AddComponent<MeshRenderer>();
            mr.material = material;
            mr.material.mainTexture = textures[i];

            Vector3 scale = gameObject.transform.localScale;
            scale.Set(1f, 1f, 1f);
            gameObject.transform.localScale = scale;
        }*/
    }    
}
