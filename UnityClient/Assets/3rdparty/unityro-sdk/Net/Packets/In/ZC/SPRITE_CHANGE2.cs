using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "SPRITE_CHANGE2", SIZE)]
    public class SPRITE_CHANGE2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SPRITE_CHANGE2;
        public const int SIZE = 15;
        public PacketHeader Header => HEADER;

        public uint GID;
        public LookType type;
        public short value;
        public short value2;

        public void Read(MemoryStreamReader br, int size) {
            GID = br.ReadUInt();
            type = (LookType) br.ReadByte();
            value = br.ReadShort();
            value2 = br.ReadShort();
        }

        public enum LookType : int {
            LOOK_BASE,
            LOOK_HAIR,
            LOOK_WEAPON,
            LOOK_HEAD_BOTTOM,
            LOOK_HEAD_TOP,
            LOOK_HEAD_MID,
            LOOK_HAIR_COLOR,
            LOOK_CLOTHES_COLOR,
            LOOK_SHIELD,
            LOOK_SHOES,
            LOOK_BODY,          //Purpose Unknown. Doesen't appear to do anything.
            LOOK_RESET_COSTUMES,//Makes all headgear sprites on player vanish when activated.
            LOOK_ROBE,
            // LOOK_FLOOR,	// TODO : fix me!! offcial use this ?
            LOOK_BODY2
        };
    }
}
