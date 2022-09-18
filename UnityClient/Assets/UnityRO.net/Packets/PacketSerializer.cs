﻿using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

/**
 * Note:
 * When working with packets, sometimes the server will send multiple
 * packets all at once. In those cases, we'll receive them all together.
 * That's why we iterate in a while once we receive bytes to read
 */
public class PacketSerializer {

    public struct PacketInfo {
        public int Size;
        public Type Type;
    }

    public MemoryStream Memory { get; set; }
    public int BytesToSkip { get; set; }

    public static Dictionary<ushort, PacketInfo> RegisteredPackets;

    private IPacketHandler PacketHandler;

    static PacketSerializer() {
        RegisteredPackets = new Dictionary<ushort, PacketInfo>();

        foreach(var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetInterface("InPacket") != null)) {
            object[] attributes = type.GetCustomAttributes(typeof(PacketHandlerAttribute), true); // get the attributes of the packet.
            if(attributes.Length == 0) return;
            PacketHandlerAttribute ma = (PacketHandlerAttribute)attributes[0];
            RegisteredPackets.Add(ma.MethodId, new PacketInfo { Size = ma.Size, Type = type });
        }
    }

    public PacketSerializer(IPacketHandler packetHandler) {
        PacketHandler = packetHandler;
        Memory = new MemoryStream();
    }

    public void Reset() {
        Memory = new MemoryStream();
    }

    public void EnqueueBytes(byte[] data, int size) {
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
            // Commands are always the first two bytes
            // Followed by either the packet data in case the packet
            // has its size fixed, or the packet length
            var tmp = new byte[2];
            Memory.Read(tmp, 0, 2);
            ushort cmd = BitConverter.ToUInt16(tmp, 0);

            if(!RegisteredPackets.ContainsKey(cmd)) {
                // We gotta break because we don't know the size of the packet
                Debug.LogWarning($"Received Unknown Command: {string.Format("0x{0:x4}", cmd)}\nProbably: {(PacketHeader)cmd}");
                Memory.Position -= 2;
                break;
            } else {
                int size = RegisteredPackets[cmd].Size;
                bool isFixed = true;

                if(size <= 0) {
                    isFixed = false;

                    // Do we have more than two bytes left?
                    if(Memory.Length - Memory.Position > 2) {
                        Memory.Read(tmp, 0, 2);
                        size = BitConverter.ToUInt16(tmp, 0);
                    } else {
                        Memory.Position -= 4;
                        break;
                    }
                }

                // Read skipping command and length
                byte[] data = new byte[size];
                Memory.Read(data, 0, size - (isFixed ? 2 : 4));

                ConstructorInfo ci = RegisteredPackets[cmd].Type.GetConstructor(new Type[] { });
                InPacket packet = (InPacket)ci.Invoke(null);
                using var br = new MemoryStreamReader(data);
                packet.Read(br, size - (isFixed ? 2 : 4));

                ThreadManager.ExecuteOnMainThread(() => {
                    PacketHandler.OnPacketReceived(packet);
                });

                PacketReceived?.Invoke(cmd, size, packet);
            }
        }

        if(Memory.Length - Memory.Position > 0) {
            MemoryStream ms = new MemoryStream();
            ms.Write(Memory.GetBuffer(), (int)Memory.Position, (int)Memory.Length - (int)Memory.Position);
            Memory.Dispose();

            Memory = ms;
        }
    }

    public event Action<ushort, int, InPacket> PacketReceived;
    public delegate void OnPacketReceived(ushort cmd, int size, InPacket packet);
}