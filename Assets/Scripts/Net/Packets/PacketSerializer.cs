using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class PacketSerializer {

    public struct PacketInfo {
        public int Size;
        public Type Type;
    }

    public MemoryStream Memory { get; set; }
    public int BytesToSkip { get; set; }
    private Dictionary<ushort, OnPacketReceived> PacketHooks { get; set; }

    public static Dictionary<ushort, PacketInfo> PacketSize;

    static PacketSerializer() {
        PacketSize = new Dictionary<ushort, PacketInfo>();

        foreach(var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetInterface("InPacket") != null)) {
            object[] attributes = type.GetCustomAttributes(typeof(PacketHandlerAttribute), true); // get the attributes of the packet.
            if(attributes.Length == 0) return;
            PacketHandlerAttribute ma = (PacketHandlerAttribute)attributes[0];
            PacketSize.Add(ma.MethodId, new PacketInfo { Size = ma.Size, Type = type });
        }
    }

    public PacketSerializer() {
        Memory = new MemoryStream();
        PacketHooks = new Dictionary<ushort, OnPacketReceived>();
    }

    public void Reset() {
        Memory = new MemoryStream();
    }

    public void EnqueueBytes(byte[] data, int size) {
        Debug.LogWarning($"Received {size} bytes");
        int pos = (int)Memory.Position;
        Memory.Position = Memory.Length;
        Memory.Write(data, 0, size);
        Memory.Position = pos;

        ReadPacket();
    }

    private void ReadPacket() {
        if(BytesToSkip > 0) {
            int skipped = Math.Min(BytesToSkip, (int)Memory.Length);
            Memory.Position += skipped;
            BytesToSkip -= skipped;
        }

        while(Memory.Length - Memory.Position > 2) {
            var tmp = new byte[2];
            Memory.Read(tmp, 0, 2);
            ushort cmd = BitConverter.ToUInt16(tmp, 0);
            Debug.Log("Cmd received " + (PacketHeader) cmd);

            if(!PacketSize.ContainsKey(cmd)) {
                Memory.Position -= 2;
                break;
            } else {
                int size = PacketSize[cmd].Size;
                bool isFixed = true;

                if(size <= 0) {
                    isFixed = false;
                    if(Memory.Length - Memory.Position > 2) {
                        Memory.Read(tmp, 0, 2);
                        size = BitConverter.ToUInt16(tmp, 0);
                    } else {
                        Memory.Position -= 4;

                        break;
                    }
                }

                byte[] data = new byte[size];
                Memory.Read(data, 0, size - (isFixed ? 2 : 4));

                ConstructorInfo ci = PacketSize[cmd].Type.GetConstructor(new Type[] { });
                InPacket packet = (InPacket)ci.Invoke(null);

                if(!packet.Read(data)) {
                    break;
                }

                ThreadManager.ExecuteOnMainThread(() => {
                    PacketHooks[cmd]?.DynamicInvoke(cmd, size, packet);
                    PacketReceived?.Invoke(cmd, size, packet);
                });
            }
        }

        if(Memory.Length - Memory.Position > 0) {
            MemoryStream ms = new MemoryStream();
            ms.Write(Memory.GetBuffer(), (int)Memory.Position, (int)Memory.Length - (int)Memory.Position);
            Memory.Dispose();

            Memory = ms;
        }
    }

    public void Hook(ushort cmd, OnPacketReceived onPackedReceived) {
        PacketHooks[cmd] = onPackedReceived;
    }

    public event Action<ushort, int, InPacket> PacketReceived;
    public delegate void OnPacketReceived(ushort cmd, int size, InPacket packet);
}