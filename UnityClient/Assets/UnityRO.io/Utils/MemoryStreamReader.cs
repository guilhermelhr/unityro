using System;
using System.IO;

namespace ROIO.Utils {

    /// <summary>
    /// Helper to load and parse binary data
    /// 
    /// @author Guilherme Hernandez
    /// Based on ROBrowser by Vincent Thibault (robrowser.com)
    /// </summary>
    public class MemoryStreamReader : MemoryStream {
        public MemoryStreamReader(byte[] buffer) : base(buffer) {
        }

        public MemoryStreamReader() : base() {
        }

        public string ReadBinaryString(long length) {
            return ReadBinaryString((int) length);
        }

        /// <summary>
        /// Reads a string from a buffer
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="length">num of bytes to read</param>
        /// <returns>string</returns>
        public string ReadBinaryString(int length) {
            char[] strBytes = new char[length];

            for (int i = 0; i < length; i++) {
                strBytes[i] = (char) base.ReadByte();
            }

            //string krEncoded = Encoding.GetEncoding(949).GetString(strBytes);
            string str = new string(strBytes);
            //.net strings are not zero-terminated
            int terminator = str.IndexOf('\0');
            if (terminator != -1) {
                str = str.Substring(0, terminator);
            }
            return str;
        }

        public string ReadBinaryString(uint length) {
            return ReadBinaryString((int) length);
        }

        public int ReadInt() {
            byte[] bint = new byte[4];
            Read(bint, 0, 4);

            return BitConverter.ToInt32(bint, 0);
        }

        public byte[] ReadBytes(int count) {
            byte[] data = new byte[count];
            Read(data, 0, count);
            return data;
        }

        public uint ReadUInt() {
            byte[] bulong = new byte[4];
            Read(bulong, 0, 4);

            return BitConverter.ToUInt32(bulong, 0);
        }

        public float ReadFloat() {
            byte[] bfloat = new byte[4];
            Read(bfloat, 0, 4);

            return BitConverter.ToSingle(bfloat, 0);
        }

        public double ReadDouble() {
            byte[] bdouble = new byte[8];
            Read(bdouble, 0, 8);

            return BitConverter.ToDouble(bdouble, 0);
        }

        public long ReadLong() {
            byte[] blong = new byte[8];
            Read(blong, 0, 8);

            return BitConverter.ToInt64(blong, 0);
        }

        public ulong ReadULong() {
            byte[] bulong = new byte[8];
            Read(bulong, 0, 8);

            return BitConverter.ToUInt64(bulong, 0);
        }

        public ushort ReadUShort() {
            byte[] bushort = new byte[2];
            Read(bushort, 0, 2);

            return BitConverter.ToUInt16(bushort, 0);
        }

        public short ReadShort() {
            byte[] bshort = new byte[2];
            Read(bshort, 0, 2);

            return BitConverter.ToInt16(bshort, 0);
        }

        /**
         * Taken from rAthena RBUFPOS
         */
        public int[] ReadPos() {
            var posX = base.ReadByte();
            var posY = base.ReadByte();
            var dir = base.ReadByte();

            var x = (posX & 0xff) << 2 | posY >> 6;
            var y = (posY & 0x3f) << 4 | dir >> 4;
            var d = dir & 0x0f;

            return new int[3] { x, y, d };
        }

        /**
         * "Random bullshit, go!!"
         * Taken from roBrowser BinaryReader
         */
        public int[] ReadPos2() {
            var a = base.ReadByte();
            var b = base.ReadByte();
            var c = base.ReadByte();
            var d = base.ReadByte();
            var e = base.ReadByte();

            var x1 = (a & 0xFF) << 2 | (b & 0xC0) >> 6;
            var y1 = (b & 0x3F) << 4 | (c & 0xF0) >> 4;
            var x2 = (d & 0xFC) >> 2 | (c & 0x0F) << 6;
            var y2 = (d & 0x03) << 8 | e & 0xFF;

            return new int[4] { x1, y1, x2, y2 };
        }
    }
}