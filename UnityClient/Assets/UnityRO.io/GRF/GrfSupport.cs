
using System;

namespace ROIO.GRF
{
    public class GrfSupport
    {
        public static uint getCStringLength(byte[] buffer, uint offset)
        {
            for (uint i = offset; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                {
                    return i - offset;
                }
            }

            return 0;
        }

        /** Grabs a C string (null terminated) from a buffer
         *  
         *  @return string
         */
        public static string getCString(byte[] buffer, uint offset)
        {
            uint length = getCStringLength(buffer, offset);

            char[] str = new char[length];
            for (int i = 0; i < length; i++)
            {
                str[i] = System.Convert.ToChar(buffer[i + offset]);
            }

            return new string(str);
        }

        /** Endian support function.
         *
         * Grabs a uint32_t from a 4byte (or more) character array.
         *
         * @warning If the character array is less than 4 bytes long, this function
         *		will access memory outside of the array
         *
         * @param p A uint8_t (char) array holding the bytes
         * @return A uint32_t in Little Endian order
         */
        public static uint LittleEndian32(byte[] p, int offset)
        {
            if (offset + 4 > p.Length)
            {
                throw new Exception("OUT_OF_BOUNDS");
            }

            return (uint)((p[3 + offset] * 256 + p[2 + offset]) * 256 + p[1 + offset]) * 256 + p[offset];
        }

        /** Endian support function.
         *
         * Transforms a host uint32_t into a little-endian uint32_t
         *
         * @param hi A host uint32_t value
         * @return A uint32_t in Little Endian order
         */
        unsafe public static uint ToLittleEndian32(uint hi)
        {
            uint lei;
            byte* p = (byte*)&lei;
            p[0] = (byte)(hi & 0xFF);
            p[1] = (byte)((hi & 0xFF00) >> 8);
            p[2] = (byte)((hi & 0xFF0000) >> 16);
            p[3] = (byte)((hi & 0xFF000000) >> 24);

            return lei;
        }

        /** Function to hash a filename.
         *
         * @note This function hashes the exact same way that GRAVITY's GRF openers
         *		do. Enjoy ;-)
         *
         * @param name Filename to be hashed
         * @return The value of the hashed filename
         */
        public static uint GRF_NameHash(string name)
        { //IT HAS COLLISIONS
            int i;
            uint tmp;

            i = name.Length;
            tmp = 0x1505;
            for (i = name.Length; i > 0; i--, name.Substring(1))
            {
                tmp = tmp * 0x21 + name[0];
            }

            return tmp;
        }
    }
}
