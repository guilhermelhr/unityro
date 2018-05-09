
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RSM;

public class Models {
    private List<CompiledModel> models;
    //private Hashtable animsTable;

    private Material material = (Material) Resources.Load("ModelMaterial", typeof(Material));
    private Material material2s = (Material) Resources.Load("ModelMaterial2Sided", typeof(Material));

    public Models(List<CompiledModel> models) {
        this.models = models;
    }

    public void BuildMeshes() {
        GameObject parent = new GameObject("_Models");
        parent.transform.parent = MapRenderer.mapParent.transform;

        //animsTable = new Hashtable();

        foreach(CompiledModel model in models) {
            GameObject modelObj = new GameObject(model.rsm.name);
            modelObj.transform.parent = parent.transform;

            foreach(var nodeData in model.nodesData) {
                foreach(var meshesByTexture in nodeData) {
                    long textureId = meshesByTexture.Key;
                    NodeMeshData meshData = meshesByTexture.Value;
                    Node node = meshData.node;

                    for(int i = 0; i < meshData.vertices.Count; i += 3) {
                        meshData.triangles.AddRange(new int[] {
                                i + 0 , i + 1, i + 2
                            });
                    }                   

                    //create node unity mesh
                    Mesh mesh = new Mesh();
                    mesh.vertices = meshData.vertices.ToArray();
                    mesh.triangles = meshData.triangles.ToArray();
                    //mesh.normals = meshData.normals.ToArray();
                    mesh.uv = meshData.uv.ToArray();

                    GameObject nodeObj;
                    if(model.nodesData.Length == 1 && nodeData.Count == 1) {
                        nodeObj = modelObj;
                    } else {
                        nodeObj = new GameObject(node.name);
                        nodeObj.transform.parent = modelObj.transform;
                    }

                    string textureFile = model.rsm.textures[textureId];

                    var mf = nodeObj.AddComponent<MeshFilter>();
                    mf.mesh = mesh;
                    var mr = nodeObj.AddComponent<MeshRenderer>();
                    if(meshData.twoSided) {
                        mr.material = material2s;
                        if(textureFile.EndsWith("tga")) {
                            mr.material.shader = Resources.Load("2SidedAlpha") as Shader;
                            mr.material.renderQueue += 1;
                        }
                    } else {
                        mr.material = material;
                        if(textureFile.EndsWith("tga")) {
                            mr.material.shader = Resources.Load("ModelShaderAlpha") as Shader;
                            mr.material.renderQueue += 1;
                        }
                    }
                    
                    mr.material.mainTexture = FileManager.Load("data/texture/" + textureFile) as Texture2D;

                    if(model.rsm.shadeType == SHADING.SMOOTH) {
                        NormalSolver.RecalculateNormals(mf.mesh, 60);
                    } else {
                        mf.mesh.RecalculateNormals();
                    }
                }
            }

            modelObj.SetActive(false);

            //instantiate model
            for(int i = 0; i < model.rsm.instances.Count; i++) {
                var instanceObj = UnityEngine.Object.Instantiate(modelObj);
                instanceObj.transform.parent = parent.transform;
                instanceObj.SetActive(true);
                instanceObj.name += "[" + i + "]";

                RSW.ModelDescriptor descriptor = model.rsm.instances[i];

                //avoid z fighting between models
                float xRandom = UnityEngine.Random.Range(-0.002f, 0.002f);
                float yRandom = UnityEngine.Random.Range(-0.002f, 0.002f);
                float zRandom = UnityEngine.Random.Range(-0.002f, 0.002f);

                Vector3 position = new Vector3(descriptor.position[0] + xRandom, descriptor.position[1] + yRandom, descriptor.position[2] + zRandom);
                position.x += MapRenderer.width;
                position.y *= -1;
                position.z += MapRenderer.height;
                instanceObj.transform.position = position;

                Vector3 rotation = new Vector3(-descriptor.rotation[0], descriptor.rotation[1], -descriptor.rotation[2]);
                instanceObj.transform.Rotate(rotation, Space.World);

                Vector3 scale = new Vector3(descriptor.scale[0], -descriptor.scale[1], descriptor.scale[2]);
                instanceObj.transform.localScale = scale;
            }

            //foreach(var node in model.source.nodes) {
            //    if(node.posKeyframes.Count > 0 || node.rotKeyframes.Count > 0) {
            //        animsTable.Add(gameObject, node);
            //        break;
            //    }
            //}
            
        }
    }

    public void Render() {
        //animate keyframed objects

        //int keyframe = Mathf.FloorToInt(Time.fixedTime * 1000);

        /*foreach(DictionaryEntry entry in animsTable) {
            GameObject gameObject = entry.Key as GameObject;
            Node node = entry.Value as Node;

            for(int i = node.rotKeyframes.Count - 1; i >= 0; i--) {
                var key = node.rotKeyframes.Keys[i];
                if(keyframe % node.main.animLen >= key) {
                    Quaternion rot = node.rotKeyframes[key];
                    Vector3 center = gameObject.GetComponent<MeshFilter>().mesh.bounds.center;

                    float angle;
                    Vector3 axis;
                    rot.ToAngleAxis(out angle, out axis);

                    gameObject.transform.RotateAround(center, axis, (angle / (node.main.animLen * Time.deltaTime)));
                    break;
                }
            }
        }*/
    }
}
