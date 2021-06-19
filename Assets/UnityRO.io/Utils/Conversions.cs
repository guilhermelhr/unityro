using System;
using UnityEngine;

namespace ROIO.Utils
{

    /// <summary>
    /// misc conversion methods
    /// 
    /// @author Guilherme Hernandez
    /// </summary>
    public class Conversions
    {
        public static float SafeDivide(float a, float b)
        {
            if (b == 0)
            {
                return 0;
            }
            else
            {
                return a / b;
            }
        }

        public static T safeArrayAccess<T>(T[] array, long index, T failover)
        {
            if (array.Length < index || index < 0)
            {
                return failover;
            }
            else
            {
                return array[index];
            }
        }

        /// <summary>
        /// get mouse coords with (0,0) being at top left
        /// </summary>
        /// <returns>mouse position</returns>
        public static Vector2 GetMouseTopLeft()
        {
            return new Vector2(Input.mousePosition.x, -Input.mousePosition.y + Screen.height);
        }

        public static Vector3 CalcNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            return Vector3.Cross(b - a, c - a).normalized;
        }

        public static Vector3 CalcNormal(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Vector3 v = Vector3.Cross(c - b, a - b);
            v.Normalize();

            Vector3 v2 = Vector3.Cross(a - d, c - d);
            v2.Normalize();

            v += v2;
            v.Normalize();

            return v;
        }

        /// <summary>
        /// Transforms the vec4 with a mat4.
        /// Based on http://glmatrix.net/docs/vec4.js.html#line453
        /// </summary>
        /// <param name="a">the vector to transform</param>
        /// <param name="m">matrix to transform with</param>
        /// <returns>transformed</returns>
        public static Vector4 TransformMat4(Vector4 a, Matrix4x4 m)
        {
            Vector4 result = new Vector4();

            var x = a[0];
            var y = a[1];
            var z = a[2];
            var w = a[3];

            result[0] = m[0] * x + m[4] * y + m[8] * z + m[12] * w;
            result[1] = m[1] * x + m[5] * y + m[9] * z + m[13] * w;
            result[2] = m[2] * x + m[6] * y + m[10] * z + m[14] * w;
            result[3] = m[3] * x + m[7] * y + m[11] * z + m[15] * w;

            return result;
        }

        /// <summary>
        /// translates a matrix by the given Z property
        /// Based on http://glmatrix.net/
        /// </summary>
        /// <param name="mat">mat4 to translate</param>
        /// <param name="z">float z translation</param>
        /// <param name="dest">mat4 receiving the operation result. if null, result is written to mat</param>
        /// <returns></returns>
        public static Matrix4x4 TranslateZ(Matrix4x4 mat, float z, Matrix4x4? nDest)
        {
            float a00, a01, a02, a03,
                a10, a11, a12, a13,
                a20, a21, a22, a23;

            if (!nDest.HasValue || mat == nDest)
            {
                mat[12] += mat[8] * z;
                mat[13] += mat[9] * z;
                mat[14] += mat[10] * z;
                mat[15] += mat[11] * z;
                return mat;
            }

            Matrix4x4 dest = nDest.Value;

            a00 = mat[0];
            a01 = mat[1];
            a02 = mat[2];
            a03 = mat[3];
            a10 = mat[4];
            a11 = mat[5];
            a12 = mat[6];
            a13 = mat[7];
            a20 = mat[8];
            a21 = mat[9];
            a22 = mat[10];
            a23 = mat[11];

            dest[0] = a00;
            dest[1] = a01;
            dest[2] = a02;
            dest[3] = a03;
            dest[4] = a10;
            dest[5] = a11;
            dest[6] = a12;
            dest[7] = a13;
            dest[8] = a20;
            dest[9] = a21;
            dest[10] = a22;
            dest[11] = a23;

            dest[12] += a20 * z;
            dest[13] += a21 * z;
            dest[14] += a22 * z;
            dest[15] += a23 * z;
            return dest;
        }

        /**
        * Translate a mat4 by the given vector
        *
        * @param {mat4} out the receiving matrix
        * @param {mat4} a the matrix to translate
        * @param {vec3} v vector to translate by
        * @returns {mat4} out
        */
        public static Matrix4x4 Translate(Matrix4x4 _out, Matrix4x4 a, Vector3 v)
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
                _out[0] = a00;
                _out[1] = a01;
                _out[2] = a02;
                _out[3] = a03;
                _out[4] = a10;
                _out[5] = a11;
                _out[6] = a12;
                _out[7] = a13;
                _out[8] = a20;
                _out[9] = a21;
                _out[10] = a22;
                _out[11] = a23;
                _out[12] = a00 * x + a10 * y + a20 * z + a[12];
                _out[13] = a01 * x + a11 * y + a21 * z + a[13];
                _out[14] = a02 * x + a12 * y + a22 * z + a[14];
                _out[15] = a03 * x + a13 * y + a23 * z + a[15];
            }

            return _out;
        }

        /**
         * Calculates the inverse of the upper 3x3 elements of a mat4 and copies the result into a mat3
         * The resulting matrix is useful for calculating transformed normals
         *
         * Params:
         * @param {mat4} mat mat4 containing values to invert and copy
         * @param {mat3} [dest] mat3 receiving values
         *
         * @returns {mat3} dest is specified, a new mat3 otherwise, null if the matrix cannot be inverted
         */
        public static Matrix4x4? toInverseMat3(Matrix4x4 mat, Matrix4x4? dest)
        {
            // Cache the matrix values (makes for huge speed increases!)
            float a00 = mat[0], a01 = mat[1], a02 = mat[2],
                a10 = mat[4], a11 = mat[5], a12 = mat[6],
                a20 = mat[8], a21 = mat[9], a22 = mat[10],
                b01 = a22 * a11 - a12 * a21,
                b11 = -a22 * a10 + a12 * a20,
                b21 = a21 * a10 - a11 * a20,
                d = a00 * b01 + a01 * b11 + a02 * b21,
                id;

            if (d == 0f) { return null; }

            id = 1 / d;

            if (!dest.HasValue) { dest = new Matrix4x4(); }

            var destv = dest.Value;

            destv[0] = b01 * id;
            destv[1] = (-a22 * a01 + a02 * a21) * id;
            destv[2] = (a12 * a01 - a02 * a11) * id;
            destv[3] = b11 * id;
            destv[4] = (a22 * a00 - a02 * a20) * id;
            destv[5] = (-a12 * a00 + a02 * a10) * id;
            destv[6] = b21 * id;
            destv[7] = (-a21 * a00 + a01 * a20) * id;
            destv[8] = (a11 * a00 - a01 * a10) * id;

            return dest;
        }
    }
}