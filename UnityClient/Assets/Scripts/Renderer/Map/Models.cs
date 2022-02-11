
using ROIO;
using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models {
    private List<RSM.CompiledModel> models;

    private Material material = (Material) Resources.Load("Materials/Models/ModelMaterial", typeof(Material));
    private Material material2s = (Material) Resources.Load("Materials/Models/ModelMaterial2Sided", typeof(Material));

    public Models(List<RSM.CompiledModel> models) {
        this.models = models;
    }

    public class AnimProperties {
        //animation
        internal SortedList<int, Quaternion> rotKeyframes;
        internal SortedList<int, Vector3> posKeyframes;
        internal long animLen;
        internal Quaternion baseRotation;
        internal bool isChild;
    }

    public IEnumerator BuildMeshes(Action<float> OnProgress) {
        GameObject modelsParent = new GameObject("_Models");
        GameObject originals = new GameObject("_Originals");
        GameObject copies = new GameObject("_Copies");
        modelsParent.transform.SetParent(MapRenderer.mapParent.transform);
        originals.transform.SetParent(modelsParent.transform);
        copies.transform.SetParent(modelsParent.transform);

        Dictionary<int, AnimProperties> anims = new Dictionary<int, AnimProperties>();
        int nodeId = 0;

        for (var index = 0; index < models.Count; index++) {
            OnProgress.Invoke(index / (float) models.Count);

            RSM.CompiledModel model = models[index];
            GameObject modelObj = new GameObject(model.rsm.filename);
            modelObj.transform.SetParent(originals.transform);

            foreach (var nodeData in model.nodesData) {
                foreach (var meshesByTexture in nodeData) {
                    long textureId = meshesByTexture.Key;
                    RSM.NodeMeshData meshData = meshesByTexture.Value;
                    RSM.Node node = meshData.node;

                    if (meshesByTexture.Value.vertices.Count == 0) {
                        continue;
                    }

                    for (int i = 0; i < meshData.vertices.Count; i += 3) {
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

                    GameObject nodeObj = new GameObject($"{node.name}_{nodeId}");
                    nodeObj.transform.parent = modelObj.transform;

                    string textureFile = model.rsm.textures[textureId];

                    var mf = nodeObj.AddComponent<MeshFilter>();
                    mf.mesh = mesh;
                    var mr = nodeObj.AddComponent<MeshRenderer>();
                    if (meshData.twoSided) {
                        mr.material = material2s;
                    } else {
                        mr.material = material;
                    }

                    var properties = nodeObj.AddComponent<NodeProperties>();
                    properties.SetTextureName(textureFile);

                    if (model.rsm.shadeType == RSM.SHADING.SMOOTH) {
                        NormalSolver.RecalculateNormals(mf.mesh, 60);
                    } else {
                        mf.mesh.RecalculateNormals();
                    }

                    var matrix = node.GetPositionMatrix();
                    nodeObj.transform.position = matrix.ExtractPosition();
                    var rotation = matrix.ExtractRotation();
                    nodeObj.transform.rotation = rotation;
                    nodeObj.transform.localScale = matrix.ExtractScale();

                    properties.nodeId = nodeId;
                    properties.mainName = model.rsm.mainNode.name;
                    properties.parentName = node.parentName;

                    if (node.posKeyframes.Count > 0 || node.rotKeyframes.Count > 0) {
                        nodeObj.AddComponent<NodeAnimation>().nodeId = nodeId;
                        anims.Add(nodeId, new AnimProperties() {
                            posKeyframes = node.posKeyframes,
                            rotKeyframes = node.rotKeyframes,
                            animLen = model.rsm.animLen,
                            baseRotation = rotation,
                            isChild = properties.isChild
                        });
                    }

                    nodeId++;
                }
            }

            modelObj.SetActive(false);

            //instantiate model
            //todo create prefabs 
            for (int i = 0; i < model.rsm.instances.Count; i++) {
                GameObject instanceObj;
                if (i == model.rsm.instances.Count - 1) {
                    //last instance
                    instanceObj = modelObj;
                } else {
                    instanceObj = UnityEngine.Object.Instantiate(modelObj);
                    instanceObj.transform.SetParent(copies.transform);
                    instanceObj.name += "[" + i + "]";
                }

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

                //setup hierarchy
                var propertiesComponents = instanceObj.GetComponentsInChildren<NodeProperties>();
                foreach (var properties in propertiesComponents) {
                    if (properties.isChild) {
                        var nodeParent = instanceObj.transform.FindRecursive(properties.parentName);
                        properties.transform.parent = nodeParent;
                    }
                }

                //setup animations
                var animComponents = instanceObj.GetComponentsInChildren<NodeAnimation>();
                foreach (var animComponent in animComponents) {
                    var properties = anims[animComponent.nodeId];
                    animComponent.Initialize(properties);
                }

                instanceObj.SetActive(true);
            }

            yield return null;
        }

        yield return null;
        anims.Clear();
    }

    public void Render() {

    }
}
