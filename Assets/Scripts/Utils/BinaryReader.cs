using System;
using System.IO;

/// <summary>
/// Helper to load and parse binary data
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class BinaryReader : MemoryStream {
    public BinaryReader(byte[] buffer) : base(buffer) {
    }

    public BinaryReader() : base() {
    }

    /// <summary>
    /// Reads a string from a buffer
    /// </summary>
    /// <param name="buffer">buffer</param>
    /// <param name="length">num of bytes to read</param>
    /// <returns>string</returns>
    public string ReadBinaryString(int length) {
        char[] strBytes = new char[length];

        for(int i = 0; i < length; i++) {
            strBytes[i] = (char) ReadByte();
        }

        //string krEncoded = Encoding.GetEncoding(949).GetString(strBytes);
        string str = new string(strBytes);
        //.net strings are not zero-terminated
        int terminator = str.IndexOf('\0');
        if(terminator != -1) {
            str = str.Substring(0, terminator);
        }
        return str;
    }

    public string ReadBinaryString(uint length) {
        return ReadBinaryString((int) length);
    }

    public int ReadLong() {
        byte[] bint = new byte[4];
        Read(bint, 0, 4);

        return BitConverter.ToInt32(bint, 0);
    }

    public byte ReadUByte() {
        return Convert.ToByte(ReadByte());
    }

    public byte[] ReadUBytes(int count) {
        byte[] data = new byte[count];
        Read(data, 0, count);
        return data;
    }

    public uint ReadULong() {
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
}
