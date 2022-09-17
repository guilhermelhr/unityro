using System;
using UnityEngine;

namespace ROIO.Utils
{

    //based on gl-matrix http://glmatrix.net
    public class Mat4
    {
        public static float EPSILON = 0.000001f;

        private static float[] identity = new float[] {
        1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1
    };

        private float[] data;

        public Mat4()
        {
            data = new float[16];
            Array.Copy(identity, data, data.Length);
        }

        public float this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        public Mat4 Clone()
        {
            float[] cloneData = new float[data.Length];
            Array.Copy(data, cloneData, data.Length);
            Mat4 clone = new Mat4()
            {
                data = cloneData
            };
            return clone;
        }

        public static Mat4 Transpose(Mat4 _out, Mat4 a)
        {
            if (_out == a)
            {
                float a01 = a[1], a02 = a[2], a03 = a[3];
                float a12 = a[6], a13 = a[7];
                float a23 = a[11];

                _out[1] = a[4];
                _out[2] = a[8];
                _out[3] = a[12];
                _out[4] = a01;
                _out[6] = a[9];
                _out[7] = a[13];
                _out[8] = a02;
                _out[9] = a12;
                _out[11] = a[14];
                _out[12] = a03;
                _out[13] = a13;
                _out[14] = a23;
            }
            else
            {
                _out[0] = a[0];
                _out[1] = a[4];
                _out[2] = a[8];
                _out[3] = a[12];
                _out[4] = a[1];
                _out[5] = a[5];
                _out[6] = a[9];
                _out[7] = a[13];
                _out[8] = a[2];
                _out[9] = a[6];
                _out[10] = a[10];
                _out[11] = a[14];
                _out[12] = a[3];
                _out[13] = a[7];
                _out[14] = a[11];
                _out[15] = a[15];
            }

            return _out;
        }

        public static Mat4 Invert(Mat4 _out, Mat4 a)
        {
            float a00 = a[0], a01 = a[1], a02 = a[2], a03 = a[3];
            float a10 = a[4], a11 = a[5], a12 = a[6], a13 = a[7];
            float a20 = a[8], a21 = a[9], a22 = a[10], a23 = a[11];
            float a30 = a[12], a31 = a[13], a32 = a[14], a33 = a[15];

            float b00 = a00 * a11 - a01 * a10;
            float b01 = a00 * a12 - a02 * a10;
            float b02 = a00 * a13 - a03 * a10;
            float b03 = a01 * a12 - a02 * a11;
            float b04 = a01 * a13 - a03 * a11;
            float b05 = a02 * a13 - a03 * a12;
            float b06 = a20 * a31 - a21 * a30;
            float b07 = a20 * a32 - a22 * a30;
            float b08 = a20 * a33 - a23 * a30;
            float b09 = a21 * a32 - a22 * a31;
            float b10 = a21 * a33 - a23 * a31;
            float b11 = a22 * a33 - a23 * a32;

            float det = b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;

            if (det == 0)
            {
                return null;
            }
            det = 1.0f / det;

            _out[0] = (a11 * b11 - a12 * b10 + a13 * b09) * det;
            _out[1] = (a02 * b10 - a01 * b11 - a03 * b09) * det;
            _out[2] = (a31 * b05 - a32 * b04 + a33 * b03) * det;
            _out[3] = (a22 * b04 - a21 * b05 - a23 * b03) * det;
            _out[4] = (a12 * b08 - a10 * b11 - a13 * b07) * det;
            _out[5] = (a00 * b11 - a02 * b08 + a03 * b07) * det;
            _out[6] = (a32 * b02 - a30 * b05 - a33 * b01) * det;
            _out[7] = (a20 * b05 - a22 * b02 + a23 * b01) * det;
            _out[8] = (a10 * b10 - a11 * b08 + a13 * b06) * det;
            _out[9] = (a01 * b08 - a00 * b10 - a03 * b06) * det;
            _out[10] = (a30 * b04 - a31 * b02 + a33 * b00) * det;
            _out[11] = (a21 * b02 - a20 * b04 - a23 * b00) * det;
            _out[12] = (a11 * b07 - a10 * b09 - a12 * b06) * det;
            _out[13] = (a00 * b09 - a01 * b07 + a02 * b06) * det;
            _out[14] = (a31 * b01 - a30 * b03 - a32 * b00) * det;
            _out[15] = (a20 * b03 - a21 * b01 + a22 * b00) * det;

            return _out;
        }

        public static Mat4 Translate(Mat4 _out, Mat4 a, Vector3 v)
        {
            float x = v[0], y = v[1], z = v[2];
            float a00, a01, a02, a03;
            float a10, a11, a12, a13;
            float a20, a21, a22, a23;

            if (a == _out)
            {
                _out[12] = a[0] * x + a[4] * y + a[8] * z + a[12];
                _out[13] = a[1] * x + a[5] * y + a[9] * z + a[13];
                _out[14] = a[2] * x + a[6] * y + a[10] * z + a[14];
                _out[15] = a[3] * x + a[7] * y + a[11] * z + a[15];
            }
            else
            {
                a00 = a[0]; a01 = a[1]; a02 = a[2]; a03 = a[3];
                a10 = a[4]; a11 = a[5]; a12 = a[6]; a13 = a[7];
                a20 = a[8]; a21 = a[9]; a22 = a[10]; a23 = a[11];

                _out[0] = a00; _out[1] = a01; _out[2] = a02; _out[3] = a03;
                _out[4] = a10; _out[5] = a11; _out[6] = a12; _out[7] = a13;
                _out[8] = a20; _out[9] = a21; _out[10] = a22; _out[11] = a23;

                _out[12] = a00 * x + a10 * y + a20 * z + a[12];
                _out[13] = a01 * x + a11 * y + a21 * z + a[13];
                _out[14] = a02 * x + a12 * y + a22 * z + a[14];
                _out[15] = a03 * x + a13 * y + a23 * z + a[15];
            }

            return _out;
        }

        public static Mat4 Rotate(Mat4 _out, Mat4 a, float rad, Vector3 axis)
        {
            float x = axis[0], y = axis[1], z = axis[2];
            float len = Mathf.Sqrt(x * x + y * y + z * z);
            float s, c, t;
            float a00, a01, a02, a03;
            float a10, a11, a12, a13;
            float a20, a21, a22, a23;
            float b00, b01, b02;
            float b10, b11, b12;
            float b20, b21, b22;

            if (len < EPSILON)
            {
                return null;
            }

            len = 1 / len;
            x *= len;
            y *= len;
            z *= len;

            s = Mathf.Sin(rad);
            c = Mathf.Cos(rad);
            t = 1 - c;

            a00 = a[0];
            a01 = a[1];
            a02 = a[2];
            a03 = a[3];
            a10 = a[4];
            a11 = a[5];
            a12 = a[6];
            a13 = a[7];
            a20 = a[8];
            a21 = a[9];
            a22 = a[10];
            a23 = a[11];

            b00 = x * x * t + c;
            b01 = y * x * t + z * s;
            b02 = z * x * t - y * s;
            b10 = x * y * t - z * s;
            b11 = y * y * t + c;
            b12 = z * y * t + x * s;
            b20 = x * z * t + y * s;
            b21 = y * z * t - x * s;
            b22 = z * z * t + c;

            _out[0] = a00 * b00 + a10 * b01 + a20 * b02;
            _out[1] = a01 * b00 + a11 * b01 + a21 * b02;
            _out[2] = a02 * b00 + a12 * b01 + a22 * b02;
            _out[3] = a03 * b00 + a13 * b01 + a23 * b02;
            _out[4] = a00 * b10 + a10 * b11 + a20 * b12;
            _out[5] = a01 * b10 + a11 * b11 + a21 * b12;
            _out[6] = a02 * b10 + a12 * b11 + a22 * b12;
            _out[7] = a03 * b10 + a13 * b11 + a23 * b12;
            _out[8] = a00 * b20 + a10 * b21 + a20 * b22;
            _out[9] = a01 * b20 + a11 * b21 + a21 * b22;
            _out[10] = a02 * b20 + a12 * b21 + a22 * b22;
            _out[11] = a03 * b20 + a13 * b21 + a23 * b22;

            if (a != _out)
            {
                _out[12] = a[12];
                _out[13] = a[13];
                _out[14] = a[14];
                _out[15] = a[15];
            }

            return _out;
        }

        public static Mat4 RotateX(Mat4 _out, Mat4 a, float rad)
        {
            float s = Mathf.Sin(rad),
                c = Mathf.Cos(rad),
                a10 = a[4],
                a11 = a[5],
                a12 = a[6],
                a13 = a[7],
                a20 = a[8],
                a21 = a[9],
                a22 = a[10],
                a23 = a[11];

            if (a != _out)
            { // If the source and destination differ, copy the unchanged rows
                _out[0] = a[0];
                _out[1] = a[1];
                _out[2] = a[2];
                _out[3] = a[3];
                _out[12] = a[12];
                _out[13] = a[13];
                _out[14] = a[14];
                _out[15] = a[15];
            }

            // Perform axis-specific matrix multiplication
            _out[4] = a10 * c + a20 * s;
            _out[5] = a11 * c + a21 * s;
            _out[6] = a12 * c + a22 * s;
            _out[7] = a13 * c + a23 * s;
            _out[8] = a20 * c - a10 * s;
            _out[9] = a21 * c - a11 * s;
            _out[10] = a22 * c - a12 * s;
            _out[11] = a23 * c - a13 * s;
            return _out;
        }

        public static Mat4 RotateY(Mat4 _out, Mat4 a, float rad)
        {
            float s = Mathf.Sin(rad),
                c = Mathf.Cos(rad),
                a00 = a[0],
                a01 = a[1],
                a02 = a[2],
                a03 = a[3],
                a20 = a[8],
                a21 = a[9],
                a22 = a[10],
                a23 = a[11];

            if (a != _out)
            { // If the source and destination differ, copy the unchanged rows
                _out[4] = a[4];
                _out[5] = a[5];
                _out[6] = a[6];
                _out[7] = a[7];
                _out[12] = a[12];
                _out[13] = a[13];
                _out[14] = a[14];
                _out[15] = a[15];
            }

            // Perform axis-specific matrix multiplication
            _out[0] = a00 * c - a20 * s;
            _out[1] = a01 * c - a21 * s;
            _out[2] = a02 * c - a22 * s;
            _out[3] = a03 * c - a23 * s;
            _out[8] = a00 * s + a20 * c;
            _out[9] = a01 * s + a21 * c;
            _out[10] = a02 * s + a22 * c;
            _out[11] = a03 * s + a23 * c;

            return _out;
        }

        public static Mat4 RotateZ(Mat4 _out, Mat4 a, float rad)
        {
            float s = Mathf.Sin(rad),
                c = Mathf.Cos(rad),
                a00 = a[0],
                a01 = a[1],
                a02 = a[2],
                a03 = a[3],
                a10 = a[4],
                a11 = a[5],
                a12 = a[6],
                a13 = a[7];

            if (a != _out)
            { // If the source and destination differ, copy the unchanged last row
                _out[8] = a[8];
                _out[9] = a[9];
                _out[10] = a[10];
                _out[11] = a[11];
                _out[12] = a[12];
                _out[13] = a[13];
                _out[14] = a[14];
                _out[15] = a[15];
            }

            // Perform axis-specific matrix multiplication
            _out[0] = a00 * c + a10 * s;
            _out[1] = a01 * c + a11 * s;
            _out[2] = a02 * c + a12 * s;
            _out[3] = a03 * c + a13 * s;
            _out[4] = a10 * c - a00 * s;
            _out[5] = a11 * c - a01 * s;
            _out[6] = a12 * c - a02 * s;
            _out[7] = a13 * c - a03 * s;

            return _out;
        }

        public static Mat4 Multiply(Mat4 _out, Mat4 a, Mat4 b)
        {
            float a00 = a[0], a01 = a[1], a02 = a[2], a03 = a[3];
            float a10 = a[4], a11 = a[5], a12 = a[6], a13 = a[7];
            float a20 = a[8], a21 = a[9], a22 = a[10], a23 = a[11];
            float a30 = a[12], a31 = a[13], a32 = a[14], a33 = a[15];

            // Cache only the current line of the second matrix
            float b0 = b[0], b1 = b[1], b2 = b[2], b3 = b[3];
            _out[0] = b0 * a00 + b1 * a10 + b2 * a20 + b3 * a30;
            _out[1] = b0 * a01 + b1 * a11 + b2 * a21 + b3 * a31;
            _out[2] = b0 * a02 + b1 * a12 + b2 * a22 + b3 * a32;
            _out[3] = b0 * a03 + b1 * a13 + b2 * a23 + b3 * a33;

            b0 = b[4]; b1 = b[5]; b2 = b[6]; b3 = b[7];
            _out[4] = b0 * a00 + b1 * a10 + b2 * a20 + b3 * a30;
            _out[5] = b0 * a01 + b1 * a11 + b2 * a21 + b3 * a31;
            _out[6] = b0 * a02 + b1 * a12 + b2 * a22 + b3 * a32;
            _out[7] = b0 * a03 + b1 * a13 + b2 * a23 + b3 * a33;

            b0 = b[8]; b1 = b[9]; b2 = b[10]; b3 = b[11];
            _out[8] = b0 * a00 + b1 * a10 + b2 * a20 + b3 * a30;
            _out[9] = b0 * a01 + b1 * a11 + b2 * a21 + b3 * a31;
            _out[10] = b0 * a02 + b1 * a12 + b2 * a22 + b3 * a32;
            _out[11] = b0 * a03 + b1 * a13 + b2 * a23 + b3 * a33;

            b0 = b[12]; b1 = b[13]; b2 = b[14]; b3 = b[15];
            _out[12] = b0 * a00 + b1 * a10 + b2 * a20 + b3 * a30;
            _out[13] = b0 * a01 + b1 * a11 + b2 * a21 + b3 * a31;
            _out[14] = b0 * a02 + b1 * a12 + b2 * a22 + b3 * a32;
            _out[15] = b0 * a03 + b1 * a13 + b2 * a23 + b3 * a33;
            return _out;
        }

        public static Mat4 RotateQuat(Mat4 _out, Mat4 mat, Vector4 w)
        {
            float a, b, c, d;
            a = w[0];
            b = w[1];
            c = w[2];
            d = w[3];

            float norm = Mathf.Sqrt(a * a + b * b + c * c + d * d);
            a /= norm;
            b /= norm;
            c /= norm;
            d /= norm;

            var tmp = new float[] {
            1.0f - 2.0f * (b * b + c * c), 2.0f * (a * b + c * d), 2.0f * (a * c - b * d), 0.0f,
            2.0f * (a * b - c * d), 1.0f - 2.0f * (a * a + c * c), 2.0f * (c * b + a * d), 0.0f,
            2.0f * (a * c + b * d), 2.0f * (b * c - a * d), 1.0f - 2.0f * (a * a + b * b), 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f
        };

            return Multiply(_out, mat, FromValues(tmp));
        }

        public static Mat4 Scale(Mat4 _out, Mat4 a, Vector3 v)
        {
            float x = v[0], y = v[1], z = v[2];

            _out[0] = a[0] * x;
            _out[1] = a[1] * x;
            _out[2] = a[2] * x;
            _out[3] = a[3] * x;
            _out[4] = a[4] * y;
            _out[5] = a[5] * y;
            _out[6] = a[6] * y;
            _out[7] = a[7] * y;
            _out[8] = a[8] * z;
            _out[9] = a[9] * z;
            _out[10] = a[10] * z;
            _out[11] = a[11] * z;
            _out[12] = a[12];
            _out[13] = a[13];
            _out[14] = a[14];
            _out[15] = a[15];

            return _out;
        }

        public static Mat4 FromMat3(Vector3[] mat, Mat4 dest)
        {
            if (dest == null)
            {
                dest = new Mat4();
            }

            dest[15] = 1;
            dest[14] = 0;
            dest[13] = 0;
            dest[12] = 0;

            dest[11] = 0;
            dest[10] = mat[2][2];
            dest[9] = mat[2][1];
            dest[8] = mat[2][0];

            dest[7] = 0;
            dest[6] = mat[1][2];
            dest[5] = mat[1][1];
            dest[4] = mat[1][0];

            dest[3] = 0;
            dest[2] = mat[0][2];
            dest[1] = mat[0][1];
            dest[0] = mat[0][0];

            return dest;
        }

        public static Mat4 ExtractRotation(Mat4 _out, Mat4 mat)
        {

            var scale_x = 1.0f / new Vector3(mat[0], mat[1], mat[2]).magnitude;
            var scale_y = 1.0f / new Vector3(mat[4], mat[5], mat[6]).magnitude;
            var scale_z = 1.0f / new Vector3(mat[8], mat[9], mat[10]).magnitude;

            _out[0] = mat[0] * scale_x;
            _out[1] = mat[1] * scale_x;
            _out[2] = mat[2] * scale_x;
            _out[4] = mat[4] * scale_y;
            _out[5] = mat[5] * scale_y;
            _out[6] = mat[6] * scale_y;
            _out[8] = mat[8] * scale_z;
            _out[9] = mat[9] * scale_z;
            _out[10] = mat[10] * scale_z;

            return _out;
        }

        public void Copy(Mat4 dest)
        {
            Array.Copy(data, dest.data, data.Length);
        }

        public Matrix4x4 ToMatrix4x4()
        {
            var m = new Matrix4x4();
            for (int i = 0; i < data.Length; i++)
            {
                m[i] = data[i];
            }
            return m;
        }

        public static Mat4 FromValues(float[] values)
        {
            float[] copy = new float[values.Length];
            Array.Copy(values, copy, values.Length);
            return new Mat4()
            {
                data = copy
            };
        }

        public static Mat4 Identity
        {
            get { return FromValues(identity); }
        }

        public override string ToString()
        {
            return "|" + data[0] + " " + data[4] + " " + data[8] + " " + data[12] + "|" + "\n"
                 + "|" + data[1] + " " + data[5] + " " + data[9] + " " + data[13] + "|" + "\n"
                 + "|" + data[2] + " " + data[6] + " " + data[10] + " " + data[14] + "|" + "\n"
                 + "|" + data[3] + " " + data[7] + " " + data[11] + " " + data[15] + "|" + "\n";
        }
    }
}