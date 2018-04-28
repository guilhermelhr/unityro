
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelLoader {

    public static RSM.Model Compile(RSM rsm) {
        Hashtable[] meshes = new Hashtable[rsm.nodes.Length * rsm.instances.Count];

        for(int i = 0, k = 0; i < rsm.nodes.Length; i++) {
            for(int j = 0; j < rsm.instances.Count; j++, k++) {
                rsm.nodes[i].instanceNumber = j;
                meshes[k] = rsm.nodes[i].Compile(rsm.instances[j]);
            }
        }

        return new RSM.Model() {
            meshes = meshes,
            textures = rsm.textures
        };
    }

    public static RSM Load(BinaryReader data) {
        var header = data.ReadBinaryString(4);

        if(header != RSM.Header) {
            throw new Exception("ModelLoader.Load: Header (" + header + ") is not \"GRSM\"");
        }

        RSM rsm = new RSM();

        //read infos
        string version = Convert.ToString(data.ReadUByte());
        string subversion = Convert.ToString(data.ReadUByte());
        version += "." + subversion;
        double dversion = double.Parse(version);
        
        rsm.version = version;
        rsm.animLen = data.ReadLong();
        rsm.shadeType = (RSM.SHADING) data.ReadLong();

        rsm.alpha = dversion >= 1.4 ? data.ReadUByte() / 255f : 1;
        data.Seek(16, System.IO.SeekOrigin.Current);

        //read textures
        int textureCount = data.ReadLong();
        rsm.textures = new string[textureCount];
        for(int i = 0; i < textureCount; i++) {
            rsm.textures[i] = data.ReadBinaryString(40);
        }

        //read nodes (meshes)
        rsm.name = data.ReadBinaryString(40);
        int nodeCount = data.ReadLong();
        rsm.nodes = new RSM.Node[nodeCount];

        for(int i = 0; i < nodeCount; i++) {
            var node = rsm.nodes[i] = LoadNode(rsm, data, dversion);
            if(string.Equals(node.name, rsm.name)) {
                rsm.mainNode = node;
            }
        }

        //fallback for non defined main node
        if(rsm.mainNode == null) {
            rsm.mainNode = rsm.nodes[0];
        }

        //read poskeyframes
        if(dversion < 1.5) {
            int count = data.ReadLong();
            rsm.posKeyframes = new RSM.PositionKeyframe[count];
            for(int i = 0; i < count; i++) {
                rsm.posKeyframes[i] = new RSM.PositionKeyframe() {
                    frame = data.ReadLong(),
                    p = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat())
                };
            }
        } else {
            rsm.posKeyframes = new RSM.PositionKeyframe[0];
        }

        //read volume box
        int vbCount = data.ReadLong();
        rsm.volumeBoxes = new RSM.VolumeBox[vbCount];

        for(int i = 0; i < vbCount; i++) {
            rsm.volumeBoxes[i] = new RSM.VolumeBox() {
                size = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()),
                pos = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()),
                rot = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat()),
                flag = dversion >= 1.3 ? data.ReadLong() : 0
            };
        }

        rsm.instances = new List<Mat4>();
        rsm.box = new RSM.Box();

        CalcBoundingBox(rsm);

        return rsm;
    }

    private static void CalcBoundingBox(RSM rsm) {
        var matrix = Mat4.Identity;
        var count = rsm.nodes.Length;
        var box = rsm.box;

        CalcNodeBoundingBox(rsm.mainNode, matrix);

        for(int i = 0; i < 3; i++) {
            for(int j = 0; j < count; j++) {
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
        var nodes = node.main.nodes;
        var vertices = node.vertices;
        float x, y, z;
        
        //find position
        node.matrix = _matrix.Clone();

        Mat4.Translate(node.matrix, node.matrix, node.pos);

        //dynamic or static model
        if(node.rotKeyframes.Length == 0) {
            Mat4.Rotate(node.matrix, node.matrix, node.rotAngle, node.rotAxis);
        } else {
            Mat4.RotateQuat(node.matrix, node.matrix, node.rotKeyframes[0].q);
        }

        Mat4.Scale(node.matrix, node.matrix, node.scale);

        Mat4 matrix = node.matrix.Clone();

        if(!node.isOnly) {
            Mat4.Translate(matrix, matrix, node.offset);
        }

        Mat4.Multiply(matrix, matrix, Mat4.FromMat3(node.mat3, null));

        for(int i = 0, count = vertices.Length; i < count; i++) {
            x = vertices[i][0];
            y = vertices[i][1];
            z = vertices[i][2];

            v[0] = matrix[0] * x + matrix[4] * y + matrix[8] * z + matrix[12];
            v[1] = matrix[1] * x + matrix[5] * y + matrix[9] * z + matrix[13];
            v[2] = matrix[2] * x + matrix[6] * y + matrix[10] * z + matrix[14];

            for(int j = 0; j < 3; j++) {
                box.min[j] = Math.Min(v[j], box.min[j]);
                box.max[j] = Math.Max(v[j], box.max[j]);
            }
        }

        for(int i = 0; i < 3; i++) {
            box.offset[i] = (box.max[i] + box.min[i]) / 2.0f;
            box.range[i] = (box.max[i] - box.min[i]) / 2.0f;
            box.center[i] = box.min[i] + box.range[i];
        }

        for(int i = 0, count = nodes.Length; i < count; i++) {
            if(string.Equals(nodes[i].parentName, node.name) && !string.Equals(node.name, node.parentName)) {
                CalcNodeBoundingBox(nodes[i], node.matrix);
            }
        }
    }

    private static RSM.Node LoadNode(RSM rsm, BinaryReader data, double version) {
        RSM.Node node = new RSM.Node();

        node.main = rsm;
        node.isOnly = rsm.nodes.Length == 1;

        node.name = data.ReadBinaryString(40);
        node.parentName = data.ReadBinaryString(40);

        //read textures
        int textureCount = data.ReadLong();
        node.textures = new long[textureCount];

        for(int i = 0; i < textureCount; i++) {
            node.textures[i] = data.ReadLong();
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
        int verticeCount = data.ReadLong();
        node.vertices = new Vector3[verticeCount];
        for(int i = 0; i < verticeCount; i++) {
            node.vertices[i] = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat());
        }

        //read textures vertices
        int tverticeCount = data.ReadLong();
        node.tVertices = new float[tverticeCount * 6];
        for(int i = 0; i < tverticeCount; i++) {
            if(version >= 1.2) {
                node.tVertices[(i * 6) + 0] = data.ReadUByte() / 255f;
                node.tVertices[(i * 6) + 1] = data.ReadUByte() / 255f;
                node.tVertices[(i * 6) + 2] = data.ReadUByte() / 255f;
                node.tVertices[(i * 6) + 3] = data.ReadUByte() / 255f;
            }
            node.tVertices[(i * 6) + 4] = data.ReadFloat() * 0.98f + 0.01f;
            node.tVertices[(i * 6) + 5] = data.ReadFloat() * 0.98f + 0.01f;
        }

        //read faces
        int faceCount = data.ReadLong();
        node.faces = new RSM.Face[faceCount];
        for(int i = 0; i < faceCount; i++) {
            node.faces[i] = new RSM.Face() {
                vertidx = new Vector3Int(data.ReadUShort(), data.ReadUShort(), data.ReadUShort()),
                tvertidx = new Vector3Int(data.ReadUShort(), data.ReadUShort(), data.ReadUShort()),
                texid = data.ReadUShort(),
                padding = data.ReadUShort(),
                twoSide = data.ReadLong(),
                smoothGroup = version >= 1.2 ? data.ReadLong() : 0
            };
        }        

        //read position keyframes
        if(version >= 1.5) {
            int pkfCount = data.ReadLong();
            node.posKeyframes = new RSM.PositionKeyframe[pkfCount];
            for(int i = 0; i < pkfCount; i++) {
                node.posKeyframes[i] = new RSM.PositionKeyframe() {
                    frame = data.ReadLong(),
                    p = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat())
                };
            }
        }

        //read rotation keyframes
        int rkfCount = data.ReadLong();
        node.rotKeyframes = new RSM.RotationKeyframe[rkfCount];

        for(int i = 0; i < rkfCount; i++) {
            node.rotKeyframes[i] = new RSM.RotationKeyframe() {
                frame = data.ReadLong(),
                q = new Vector4(data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat())
            };
        }

        node.box = new RSM.Box();
        node.matrix = Mat4.Identity;

        return node;
    }
}
