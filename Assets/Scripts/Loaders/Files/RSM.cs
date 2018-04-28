
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSM {
    public static string Header = "GRSM";

    public enum SHADING {
        NONE,
        FLAT,
        SMOOTH
    }

    public string filename;
    public string version;
    public string name;
    public long count;
    public long animLen;
    public SHADING shadeType;
    public Node[] nodes;
    public Node mainNode;
    public float alpha;
    public string[] textures;
    public VolumeBox[] volumeBoxes;
    public PositionKeyframe[] posKeyframes;
    public Box box;
    public List<Mat4> instances;

    public class VolumeBox {
        public Vector3 pos;
        public Vector3 size;
        public Vector3 rot;
        public long flag;
    }

    public class Box {
        public Vector3 max = Vector3.negativeInfinity, 
            min = Vector3.positiveInfinity, 
            offset = new Vector3(), 
            range = new Vector3(), 
            center = new Vector3();
    }

    public class Face {
        public Vector3Int vertidx;
        public Vector3Int tvertidx;
        public ushort texid;
        public ushort padding;
        public long twoSide;
        public long smoothGroup;
    }

    public class PositionKeyframe {
        public long frame;
        public Vector3 p;
    }

    public class RotationKeyframe {
        public long frame;
        public Vector4 q;
    }

    public class Node {
        public string FullName {
            get { return name + "[" + instanceNumber + "]"; }
        }
        public string name;
        public string parentName;
        public RSM main;
        public bool isOnly;
        public Vector3[] mat3;
        public Vector3 offset, pos, rotAxis, scale;
        public float rotAngle;
        public long[] textures;
        public Vector3[] vertices;
        public float[] tVertices; //texture vertices
        public Face[] faces;
        public RotationKeyframe[] rotKeyframes;
        public PositionKeyframe[] posKeyframes;
        public Box box;
        public Mat4 matrix;
        public Mat4 instanceMatrix;
        public int instanceNumber;

        public Hashtable Compile(Mat4 instanceMatrix) {
            this.instanceMatrix = instanceMatrix;

            Hashtable mesh = new Hashtable();

            //calculate matrix
            var matrix = Mat4.Identity;
            Mat4.Translate(matrix, matrix, new Vector3(-main.box.center[0], -main.box.max[1], -main.box.center[2]));
            Mat4.Multiply(matrix, matrix, this.matrix);

            if(!isOnly) {
                Mat4.Translate(matrix, matrix, offset);
            }
            
            Mat4.Multiply(matrix, matrix, Mat4.FromMat3(mat3, null));

            //multiply with instance matrix (position/rotation/...)
            //generate normal matrix
            var modelViewMat = new Mat4();
            Mat4.Multiply(modelViewMat, instanceMatrix, matrix);
            var normalMat = new Mat4();
            Mat4.ExtractRotation(normalMat, modelViewMat);

            //generate new vertices
            float[] verts = new float[vertices.Length * 3];
            for(int i = 0; i < verts.Length / 3; i++) {
                float x = vertices[i][0];
                float y = vertices[i][1];
                float z = vertices[i][2];

                //(vec3)vert = (mat4)modelViewMat * (vec3)vertices[i];
                verts[i * 3 + 0] = modelViewMat[0] * x + modelViewMat[4] * y + modelViewMat[8] * z + modelViewMat[12];
                verts[i * 3 + 1] = modelViewMat[1] * x + modelViewMat[5] * y + modelViewMat[9] * z + modelViewMat[13];
                verts[i * 3 + 2] = modelViewMat[2] * x + modelViewMat[6] * y + modelViewMat[10] * z + modelViewMat[14];
            }

            //generate face normals
            float[] faceNormals = new float[faces.Length * 3];
            //setup mesh slot array
            Hashtable meshSize = new Hashtable();
            for(int i = 0; i < textures.Length; i++) {
                meshSize[textures[i]] = 0;
            }

            //find mesh max face
            for(int i = 0; i < faces.Length; i++) {
                int value = (int) meshSize[textures[faces[i].texid]];
                meshSize[textures[faces[i].texid]] = value + 1;
            }

            //initialize buffer
            for(int i = 0; i < textures.Length; i++) {
                mesh[textures[i]] = new float[(int) meshSize[textures[i]] * 27 * 3];
            }

            float[][] shadeGroup = new float[32][];
            bool[] shadeGroupUsed = new bool[32];

            switch(main.shadeType) {
                case SHADING.NONE:
                    CalcNormal_NONE(faceNormals);
                    GenerateMesh_FLAT(verts, faceNormals, mesh);
                    break;
                case SHADING.FLAT:
                    CalcNormal_FLAT(faceNormals, normalMat, shadeGroupUsed);
                    GenerateMesh_FLAT(verts, faceNormals, mesh);
                    break;
                case SHADING.SMOOTH:
                    CalcNormal_FLAT(faceNormals, normalMat, shadeGroupUsed);
                    CalcNormal_SMOOTH(faceNormals, shadeGroupUsed, shadeGroup);
                    GenerateMesh_SMOOTH(verts, shadeGroup, mesh);
                    break;
            }

            return mesh;
        }

        private void CalcNormal_NONE(float[] _out) {
            for(int i = 1; i < _out.Length; i += 3) {
                _out[i] = -1;
            }
        }

        private void CalcNormal_FLAT(float[] _out, Mat4 normalMat, bool[] groupUsed) {
            for(int i = 0, j = 0; i < faces.Length; i++, j += 3) {
                Face face = faces[i];
                var temp_vec = Conversions.CalcNormal(vertices[face.vertidx[0]],
                    vertices[face.vertidx[1]],
                    vertices[face.vertidx[2]]);

	            // (vec3)out = (mat4)normalMat * (vec3)temp_vec:
	            _out[j] = normalMat[0] * temp_vec[0] + normalMat[4] * temp_vec[1] + normalMat[8]  * temp_vec[2] + normalMat[12];
	            _out[j+1] = normalMat[1] * temp_vec[0] + normalMat[5] * temp_vec[1] + normalMat[9]  * temp_vec[2] + normalMat[13];
	            _out[j+2] = normalMat[2] * temp_vec[0] + normalMat[6] * temp_vec[1] + normalMat[10] * temp_vec[2] + normalMat[14];

	            groupUsed[face.smoothGroup] = true;
            }
        }

        private void CalcNormal_SMOOTH(float[] normal, bool[] groupUsed, float[][] group) {
            var size = vertices.Length;

            for(int j = 0; j < 32; ++j) {

                // Group not used, skip it
                if(!groupUsed[j]) {
                    continue;
                }

                group[j] = new float[size * 3];
                var norm = group[j];

                for(int v = 0, l = 0; v < size; ++v, l += 3) {
                    Vector3 t = new Vector3(0, 0, 0);

                    for(int i = 0, k = 0; i < faces.Length; ++i, k += 3) {
                        var face = faces[i];
                        if(face.smoothGroup == j && (face.vertidx[0] == v || face.vertidx[1] == v || face.vertidx[2] == v)) {
                            t.x += normal[k];
                            t.y += normal[k + 1];
                            t.z += normal[k + 2];
                        }
                    }

                    t.Normalize();

                    // (vec3)norm = normalize( vec3(x,y,z) );
                    norm[l] = t.x;
                    norm[l + 1] = t.y;
                    norm[l + 2] = t.z;
                }
            }
        }

        private void GenerateMesh_FLAT(float[] vert, float[] norm, Hashtable mesh) {
            Hashtable offset = new Hashtable();

            // Setup mesh slot array
            for(int i = 0; i < textures.Length; i++) {
                offset[textures[i]] = 0;
            }

            for(int i = 0, o = 0, k = 0; i < faces.Length; i++, k += 3) {
                var face = faces[i];
                var idx = face.vertidx;
                var tidx = face.tvertidx;
                var t = textures[face.texid];
			    float[] _out  = (float[]) mesh[t];
                o = (int) offset[t];

                for(int j = 0; j < 3; j++, o += 27) {
                    var a = idx[j] * 3;
                    var b = tidx[j] * 6;
				    /* vec3 positions  */  
                    _out[o+0]  = vert[a + 0];
                    _out[o+1]  = vert[a + 1];   
                    _out[o+2] = vert[a + 2];
				    /* vec3 normals    */  
                    _out[o+3]  = norm[k + 0];   
                    _out[o+4]  = norm[k + 1];   
                    _out[o+5] = norm[k + 2];
				    /* vec2 textCoords */  
                    _out[o+6]  = tVertices[b + 4];   
                    _out[o+7]  = tVertices[b + 5];
				    /* float alpha     */  
                    _out[o+8]  = main.alpha;
                    _out[o + 9] = instanceMatrix[0];
                    _out[o + 10] = instanceMatrix[1];
                    _out[o + 11] = instanceMatrix[2];
                    _out[o + 12] = instanceMatrix[3];
                    _out[o + 13] = instanceMatrix[4];
                    _out[o + 14] = instanceMatrix[5];
                    _out[o + 15] = instanceMatrix[6];
                    _out[o + 16] = instanceMatrix[7];
                    _out[o + 17] = instanceMatrix[8];
                    _out[o + 18] = instanceMatrix[9];
                    _out[o + 19] = instanceMatrix[10];
                    _out[o + 20] = instanceMatrix[11];
                    _out[o + 21] = instanceMatrix[12];
                    _out[o + 22] = instanceMatrix[13];
                    _out[o + 23] = instanceMatrix[14];
                    _out[o + 24] = instanceMatrix[15];
                    _out[o + 25] = instanceNumber;
                    _out[0 + 26] = face.twoSide;
                }

                offset[t] = o;
		    }
	    }

        private void GenerateMesh_SMOOTH(float[] vert, float[][] shadeGroup, Hashtable mesh) {
            var offset = new Hashtable();

            // Setup mesh slot array
            for(int i = 0; i < textures.Length; ++i) {
                offset[textures[i]] = 0;
            }

            for(int i = 0, o = 0; i < faces.Length; ++i) {
                var face = faces[i];
                var norm = shadeGroup[face.smoothGroup];
                var idx = face.vertidx;
                var tidx = face.tvertidx;

                var t = textures[face.texid];
			    var _out  = (float[]) mesh[t];
                o = (int) offset[t];

                for(int j = 0; j < 3; j++, o += 27) {
                    var a = idx[j] * 3;
                    var b = tidx[j] * 6;
                    /* vec3 positions  */
                    _out[o+0]  = vert[a + 0];
                    _out[o+1]  = vert[a + 1];
                    _out[o+2] = vert[a + 2];
                    /* vec3 normals    */
                    _out[o+3]  = norm[a + 0];
                    _out[o+4]  = norm[a + 1];
                    _out[o+5] = norm[a + 2];
                    /* vec2 textCoords */
                    _out[o+6]  = tVertices[b + 4];
                    _out[o+7]  = tVertices[b + 5];
                    /* float alpha     */
                    _out[o+8]  = main.alpha;
                    _out[o + 9] = instanceMatrix[0];
                    _out[o + 10] = instanceMatrix[1];
                    _out[o + 11] = instanceMatrix[2];
                    _out[o + 12] = instanceMatrix[3];
                    _out[o + 13] = instanceMatrix[4];
                    _out[o + 14] = instanceMatrix[5];
                    _out[o + 15] = instanceMatrix[6];
                    _out[o + 16] = instanceMatrix[7];
                    _out[o + 17] = instanceMatrix[8];
                    _out[o + 18] = instanceMatrix[9];
                    _out[o + 19] = instanceMatrix[10];
                    _out[o + 20] = instanceMatrix[11];
                    _out[o + 21] = instanceMatrix[12];
                    _out[o + 22] = instanceMatrix[13];
                    _out[o + 23] = instanceMatrix[14];
                    _out[o + 24] = instanceMatrix[15];
                    _out[o + 25] = instanceNumber;
                    _out[o + 26] = face.twoSide;
                }

                offset[t] = o;
		    }
	    }
    }

    public class Model {
        public string[] textures;
        public Hashtable[] meshes;
    }

    public class CompiledModel
    {
        public string name;
        public string texture;
        public float alpha;
        public float[] mesh;
        public RSM source;
    }

    public void CreateInstance(RSW.Model model, float width, float height) {
        var matrix = Mat4.Identity;
        Mat4.Translate(matrix, matrix, new Vector3(width + model.position[0], -model.position[1], height + model.position[2]));
        Mat4.RotateZ(matrix, matrix, -model.rotation[2] * Mathf.Deg2Rad);
        Mat4.RotateX(matrix, matrix, -model.rotation[0] * Mathf.Deg2Rad);
        Mat4.RotateY(matrix, matrix, model.rotation[1] * Mathf.Deg2Rad);
        Mat4.Scale(matrix, matrix, new Vector3(model.scale[0], -model.scale[1], model.scale[2]));

        instances.Add(matrix);
    }
}
