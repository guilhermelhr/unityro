using System;

public static class EncryptionHelper {

    private const ulong ENCRYPTION_KEY_1 = 0x16E8C2E0;
    private const ulong ENCRYPTION_KEY_2 = 0x288DE195;
    private const ulong ENCRYPTION_KEY_3 = 0x348BD4AF;

    public static bool Enabled = false;

    public static ushort Decrypt(byte[] tmp) {
        ushort cmd = BitConverter.ToUInt16(tmp, 0);

        return 0;
    }

    public static ushort Encrypt(ushort cmd) {
        if (Enabled) {
            var key = (((((ENCRYPTION_KEY_1 * ENCRYPTION_KEY_2) + ENCRYPTION_KEY_3) & 0xFFFFFFFF) * ENCRYPTION_KEY_2) + ENCRYPTION_KEY_3) & 0xFFFFFFFF;
            cmd |= (ushort)(key << 16 & 0x7FFF);
        }
        return cmd;
    }
}
