using ROIO.Models.FileTypes;
using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace ROIO.Loaders {
    public class ModelLoader {
        //returns a collection of nodes and its meshes generated from an RSM.
        public static RSM.CompiledModel Compile(RSM rsm) {
            var nodesData = new Dictionary<long, RSM.NodeMeshData>[rsm.nodes.Length];

            for (int i = 0; i < rsm.nodes.Length; ++i) {
                //mesh = union of nodes meshes
                nodesData[i] = rsm.nodes[i].Compile();
            }

            return new RSM.CompiledModel() {
                nodesData = nodesData,
                rsm = rsm,
            };
        }

        public static RSM Load(MemoryStreamReader data) {
            var header = data.ReadBinaryString(4);

            if (header != RSM.Header) {
                throw new Exception("ModelLoader.Load: Header (" + header + ") is not \"GRSM\"");
            }

            RSM rsm = new RSM();

            //read infos
            string version = Convert.ToString(data.ReadByte());
            string subversion = Convert.ToString(data.ReadByte());
            version += "." + subversion;
            double dversion = double.Parse(version, CultureInfo.InvariantCulture);
            rsm.version = version;
            rsm.animLen = data.ReadInt();
            rsm.shadeType = (RSM.SHADING) data.ReadInt();

            rsm.alpha = dversion >= 1.4 ? data.ReadByte() / 255f : 1;
            data.Seek(16, System.IO.SeekOrigin.Current);

            //read textures
            int textureCount = data.ReadInt();
            rsm.textures = new string[textureCount];
            for (int i = 0; i < textureCount; ++i) {
                rsm.textures[i] = data.ReadBinaryString(40);
            }

            //read nodes (meshes)
            rsm.name = data.ReadBinaryString(40);
            int nodeCount = data.ReadInt();
            rsm.nodes = new RSM.Node[nodeCount];

            for (int i = 0; i < nodeCount; ++i) {
                var node = rsm.nodes[i] = LoadNode(rsm, data, dversion);
                if (string.Equals(node.name, rsm.name)) {
                    rsm.mainNode = node;
                }
            }

            //fallback for non defined main node
            if (rsm.mainNode == null) {
                rsm.mainNode = rsm.nodes[0];
            }

            //read poskeyframes
            if (dversion < 1.5) {
                int count = data.ReadInt();
                rsm.posKeyframes = new RSM.PositionKeyframe[count];
                for (int i = 0; i < count; ++i) {
                    rsm.posKeyframes[i] = new RSM.PositionKeyframe() {
                        frame = data.ReadInt(),
                        p = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat())
                    };
                }
            } else {
                rsm.posKeyframes = new RSM.PositionKeyframe[0];
            }

            //read volume box
            var vbCount = data.ReadInt();
            rsm.volumeBoxes = new RSM.VolumeBox[vbCount];

            for (int i = 0; i < vbCount; ++i) {
                rsm.volumeBoxes[i] = new RSM.VolumeBox() {
                    size = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()),
                    pos = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()),
                    rot = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()),
                    flag = dversion >= 1.3 ? data.ReadInt() : 0
                };
            }

            rsm.instances = new List<RSW.ModelDescriptor>();
            rsm.box = new RSM.Box();

            CalcBoundingBox(rsm);

            return rsm;
        }

        private static void CalcBoundingBox(RSM rsm) {
            var matrix = Mat4.Identity;
            var count = rsm.nodes.Length;
            var box = rsm.box;

            CalcNodeBoundingBox(rsm.mainNode, matrix);

            for (int i = 0; i < 3; ++i) {
                for (int j = 0; j < count; ++j) {
                    box.max[i] = Math.Max(box.max[i], rsm.nodes[j].box.max[i]);
                    box.min[i] = Math.Min(box.min[i], rsm.nodes[j].box.min[i]);
                }
                box.offset[i] = (box.max[i] + box.min[i]) / 2.0f;
                box.range[i] = (box.max[i] - box.min[i]) / 2.0f;
                box.center[i] = box.min[i] + box.range[i];
            }
        }

        private static void CalcNodeBoundingBox(RSM.Node node, Mat4 _matrix) {
            var v = new Vector3();
            var box = node.box;
            var nodes = node.model.nodes;
            var vertices = node.vertices;
            float x, y, z;

            //find position
            node.matrix = _matrix.Clone();

            Mat4.Translate(node.matrix, node.matrix, node.pos);

            //dynamic or static model
            if (node.rotKeyframes.Count == 0) {
                Mat4.Rotate(node.matrix, node.matrix, node.rotAngle, node.rotAxis);
            }

            Mat4.Scale(node.matrix, node.matrix, node.scale);

            Mat4 matrix = node.matrix.Clone();

            if (!node.isOnly) {
                Mat4.Translate(matrix, matrix, node.offset);
            }

            Mat4.Multiply(matrix, matrix, Mat4.FromMat3(node.mat3, null));

            for (int i = 0, count = vertices.Count; i < count; ++i) {
                x = vertices[i][0];
                y = vertices[i][1];
                z = vertices[i][2];

                v[0] = matrix[0] * x + matrix[4] * y + matrix[8] * z + matrix[12];
                v[1] = matrix[1] * x + matrix[5] * y + matrix[9] * z + matrix[13];
                v[2] = matrix[2] * x + matrix[6] * y + matrix[10] * z + matrix[14];

                for (int j = 0; j < 3; j++) {
                    box.min[j] = Math.Min(v[j], box.min[j]);
                    box.max[j] = Math.Max(v[j], box.max[j]);
                }
            }

            for (int i = 0; i < 3; ++i) {
                box.offset[i] = (box.max[i] + box.min[i]) / 2.0f;
                box.range[i] = (box.max[i] - box.min[i]) / 2.0f;
                box.center[i] = box.min[i] + box.range[i];
            }

            for (int i = 0, count = nodes.Length; i < count; ++i) {
                if (string.Equals(nodes[i].parentName, node.name) && !string.Equals(node.name, node.parentName)) {
                    nodes[i].parent = node;
                    node.children.Add(nodes[i]);
                    CalcNodeBoundingBox(nodes[i], node.matrix);
                }
            }
        }

        private static RSM.Node LoadNode(RSM rsm, MemoryStreamReader data, double version) {
            RSM.Node node = new RSM.Node();

            node.model = rsm;
            node.isOnly = rsm.nodes.Length == 1;

            node.name = data.ReadBinaryString(40);
            node.parentName = data.ReadBinaryString(40);

            //read textures
            int textureCount = data.ReadInt();
            node.textures = new long[textureCount];

            for (int i = 0; i < textureCount; ++i) {
                node.textures[i] = data.ReadInt();
            }

            //read options
            node.mat3 = new Vector3[] {
            new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()),
            new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()),
            new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat())
        };

            node.offset = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat());
            node.pos = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat());
            node.rotAngle = data.ReadFloat();
            node.rotAxis = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat());
            node.scale = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat());

            //read vertices
            int verticeCount = data.ReadInt();
            node.vertices = new List<Vector3>();
            for (int i = 0; i < verticeCount; ++i) {
                node.vertices.Add(new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()));
            }

            //read textures vertices
            int tverticeCount = data.ReadInt();
            node.tVertices = new float[tverticeCount * 6];
            for (int i = 0; i < tverticeCount; ++i) {
                if (version >= 1.2) {
                    node.tVertices[i * 6 + 0] = data.ReadByte() / 255f;
                    node.tVertices[i * 6 + 1] = data.ReadByte() / 255f;
                    node.tVertices[i * 6 + 2] = data.ReadByte() / 255f;
                    node.tVertices[i * 6 + 3] = data.ReadByte() / 255f;
                }
                node.tVertices[i * 6 + 4] = data.ReadFloat() * 0.98f + 0.01f;
                node.tVertices[i * 6 + 5] = data.ReadFloat() * 0.98f + 0.01f;
            }

            //read faces
            int faceCount = data.ReadInt();
            node.faces = new RSM.Face[faceCount];
            for (int i = 0; i < faceCount; ++i) {
                node.faces[i] = new RSM.Face() {
                    vertidx = new Vector3Int(data.ReadUShort(), data.ReadUShort(), data.ReadUShort()),
                    tvertidx = new Vector3Int(data.ReadUShort(), data.ReadUShort(), data.ReadUShort()),
                    texid = data.ReadUShort(),
                    padding = data.ReadUShort(),
                    twoSided = data.ReadInt(),
                    smoothGroup = version >= 1.2 ? data.ReadInt() : 0
                };
            }

            //read position keyframes
            // DIFF: roBrowser and open-ragnarok use (version >= 1.5) here.
            // BrowEdit does not read position keyframes at all for any version.
            if (version > 1.5) {
                int pkfCount = data.ReadInt();
                for (int i = 0; i < pkfCount; ++i) {
                    var key = data.ReadInt();

                    if (!node.posKeyframes.ContainsKey(key)) {
                        node.posKeyframes.Add(key, new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()));
                    }
                }
            }

            //read rotation keyframes
            int rkfCount = data.ReadInt();
            for (int i = 0; i < rkfCount; ++i) {
                int time = data.ReadInt();
                Quaternion quat = new Quaternion(data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat());

                if (!node.rotKeyframes.ContainsKey(time)) {
                    //some models have multiple keyframes with the
                    //same timestamp, here we just keep the first one
                    //and throw out the rest.
                    node.rotKeyframes.Add(time, quat);
                }
            }

            node.box = new RSM.Box();

            return node;
        }
    }
}