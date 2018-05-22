
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RSM;

public class Models {
    private List<CompiledModel> models;

    private Material material = (Material) Resources.Load("ModelMaterial", typeof(Material));
    private Material material2s = (Material) Resources.Load("ModelMaterial2Sided", typeof(Material));

    public Models(List<CompiledModel> models) {
        this.models = models;
    }

    public class AnimProperties {
        public SortedList<int, Quaternion> rotKeyframes;
        public SortedList<int, Vector3> posKeyframes;
        public long animLen;
        public Quaternion baseRotation;
    }

    public void BuildMeshes() {
        GameObject parent = new GameObject("_Models");
        parent.transform.parent = MapRenderer.mapParent.transform;
        Dictionary<int, AnimProperties> anims = new Dictionary<int, AnimProperties>();
        int nodeId = 0;

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

                    GameObject nodeObj = new GameObject(node.name);
                    nodeObj.transform.parent = modelObj.transform;

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

                    var matrix = node.GetPositionMatrix();
                    nodeObj.transform.position = matrix.ExtractPosition();
                    var rotation = matrix.ExtractRotation();
                    nodeObj.transform.rotation = rotation;
                    nodeObj.transform.localScale = matrix.ExtractScale();

                    if(node.posKeyframes.Count > 0 || node.rotKeyframes.Count > 0) {
                        nodeObj.AddComponent<NodeAnimation>().nodeId = nodeId;
                        anims.Add(nodeId, new AnimProperties() {
                            posKeyframes = node.posKeyframes,
                            rotKeyframes = node.rotKeyframes,
                            animLen = model.rsm.animLen,
                            baseRotation = rotation
                        });
                    }
                    
                    nodeId++;
                }
            }

            modelObj.SetActive(false);

            //instantiate model
            for(int i = 0; i < model.rsm.instances.Count; i++) {
                GameObject instanceObj;
                if(i == model.rsm.instances.Count - 1) {
                    //last instance
                    instanceObj = modelObj;
                } else {
                    instanceObj = UnityEngine.Object.Instantiate(modelObj);
                }
                
                instanceObj.transform.parent = parent.transform;
                instanceObj.name += "[" + i + "]";

                RSW.ModelDescriptor descriptor = model.rsm.instances[i];

                instanceObj.transform.Rotate(Vector3.forward, -descriptor.rotation[2]);
                instanceObj.transform.Rotate(Vector3.right, -descriptor.rotation[0]);
                instanceObj.transform.Rotate(Vector3.up, descriptor.rotation[1]);

                Vector3 scale = new Vector3(descriptor.scale[0], -descriptor.scale[1], descriptor.scale[2]);
                instanceObj.transform.localScale = scale;

                //avoid z fighting between models
                float xRandom = UnityEngine.Random.Range(-0.002f, 0.002f);
                float yRandom = UnityEngine.Random.Range(-0.002f, 0.002f);
                float zRandom = UnityEngine.Random.Range(-0.002f, 0.002f);

                Vector3 position = new Vector3(descriptor.position[0] + xRandom, descriptor.position[1] + yRandom, descriptor.position[2] + zRandom);
                position.x += MapRenderer.width;
                position.y *= -1;
                position.z += MapRenderer.height;
                instanceObj.transform.position = position;

                //setup animations
                var animComponents = instanceObj.GetComponentsInChildren<NodeAnimation>();
                foreach(var animComponent in animComponents) {
                    var properties = anims[animComponent.nodeId];
                    animComponent.Initialize(properties);
                }

                instanceObj.SetActive(true);
            }
            
        }

        anims.Clear();
    }

    public void Render() {
        
    }
}
