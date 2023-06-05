using System.IO;

namespace ROIO.Utils.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void WriteCString(this BinaryWriter bw, string str, int size)
        {
            for (int i = 0; i < size; i++)
            {
                if (i < str.Length)
                    bw.Write((byte)str[i]);
                else
                    bw.Write((byte)0);
            }
        }

        /**
         * Taken from rAthena WBUFPOS
         */
        public static void WritePos(this BinaryWriter bw, short x, short y, byte dir)
        {
            bw.Write((byte)(x >> 2));
            bw.Write((byte)(x << 6 | y >> 4 & 0x3f));
            bw.Write((byte)(y << 4 | dir & 0xf));
        }
    }
}