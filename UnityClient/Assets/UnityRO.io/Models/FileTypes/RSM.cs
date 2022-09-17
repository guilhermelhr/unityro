using ROIO.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace ROIO.Models.FileTypes
{
    public class RSM
    {
        public static string Header = "GRSM";

        public enum SHADING
        {
            NONE,
            FLAT,
            SMOOTH
        }

        public string version;
        public string name;

        public List<RSW.ModelDescriptor> instances;

        public string[] textures;
        public SHADING shadeType;
        public float alpha;

        public long animLen;

        public Node[] nodes;
        public Node mainNode;

        public Box box;
        public VolumeBox[] volumeBoxes;
        public PositionKeyframe[] posKeyframes;

        public class NodeMeshData
        {
            public List<Vector3> vertices = new List<Vector3>();
            public List<int> triangles = new List<int>();
            public List<Vector3> normals = new List<Vector3>();
            public List<Vector2> uv = new List<Vector2>();
            public bool twoSided;
            public Node node;
        }

        public class Instance
        {
            public int id;
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
        }

        public class VolumeBox
        {
            public Vector3 pos;
            public Vector3 size;
            public Vector3 rot;
            public long flag;
        }

        public class Box
        {
            public Vector3 max = Vector3.negativeInfinity,
                min = Vector3.positiveInfinity,
                offset = new Vector3(),
                range = new Vector3(),
                center = new Vector3();
        }

        public class Face
        {
            public Vector3Int vertidx;
            public Vector3Int tvertidx;
            public ushort texid;
            public ushort padding;
            public long twoSided;
            public long smoothGroup;
        }

        public class PositionKeyframe
        {
            public long frame;
            public Vector3 p;
        }

        public class RotationKeyframe
        {
            public long frame;
            public Quaternion q;
        }

        public class Node
        {
            public string FullName
            {
                get { return name + "[" + instanceNumber + "]"; }
            }
            public string name;
            public string parentName;
            public Node parent;
            public List<Node> children = new List<Node>();
            public RSM model;
            public bool isOnly;
            public Vector3[] mat3;
            public Vector3 offset, pos, rotAxis, scale;
            public float rotAngle;
            public long[] textures;
            public List<Vector3> vertices;
            public float[] tVertices; //texture vertices
            public Face[] faces;
            public SortedList<int, Quaternion> rotKeyframes = new SortedList<int, Quaternion>();
            public SortedList<int, Vector3> posKeyframes = new SortedList<int, Vector3>();

            public int instanceNumber;
            public Box box;

            public Mat4 matrix = Mat4.Identity;

            public Matrix4x4 GetPositionMatrix()
            {
                //calculate matrix
                var matrix = Mat4.Identity;
                Mat4.Translate(matrix, matrix, new Vector3(-model.box.center[0], -model.box.max[1], -model.box.center[2]));
                Mat4.Multiply(matrix, matrix, this.matrix);

                return matrix.ToMatrix4x4();
            }

            public Dictionary<long, NodeMeshData> Compile()
            {
                Dictionary<long, NodeMeshData> mesh = new Dictionary<long, NodeMeshData>();

                //calculate offset and rotation matrix
                var matrix = Mat4.Identity;

                if (parent != null || children.Count > 0)
                {
                    Mat4.Translate(matrix, matrix, offset);
                }

                Mat4.Multiply(matrix, matrix, Mat4.FromMat3(mat3, null));

                //generate new vertices
                for (int i = 0; i < vertices.Count; i++)
                {
                    float x = vertices[i].x;
                    float y = vertices[i].y;
                    float z = vertices[i].z;

                    //(vec3)vert = (mat4)modelViewMat * (vec3)vertices[i];
                    Vector3 transformed = new Vector3
                    {
                        x = matrix[0] * x + matrix[4] * y + matrix[8] * z + matrix[12],
                        y = matrix[1] * x + matrix[5] * y + matrix[9] * z + matrix[13],
                        z = matrix[2] * x + matrix[6] * y + matrix[10] * z + matrix[14]
                    };

                    vertices[i] = transformed;
                }

                //initialize buffer
                for (int i = 0; i < textures.Length; i++)
                {
                    //initialize mesh for texture i
                    mesh[textures[i]] = new NodeMeshData
                    {
                        node = this
                    };
                }

                GenerateMesh(vertices, mesh);

                return mesh;
            }

            private void GenerateMesh(List<Vector3> vert, Dictionary<long, NodeMeshData> mesh)
            {
                for (int i = 0; i < faces.Length; ++i)
                {
                    var face = faces[i];
                    var idx = face.vertidx;
                    var tidx = face.tvertidx;
                    var t = textures[face.texid];
                    var _out = mesh[t];
                    _out.twoSided = face.twoSided != 0;

                    for (int j = 0; j < 3; j++)
                    {
                        var a = idx[j];
                        var b = tidx[j] * 6;
                        /* vec3 positions  */
                        _out.vertices.Add(vert[a]);
                        /* vec2 textCoords */
                        _out.uv.Add(new Vector2(tVertices[b + 4], -tVertices[b + 5]));
                    }
                }
            }
        }

        public class CompiledModel
        {
            public RSM rsm;
            public Dictionary<long, NodeMeshData>[] nodesData;
        }

        public void CreateInstance(RSW.ModelDescriptor model)
        {
            instances.Add(model);
        }

        //utility vars
        public string filename;
    }
}