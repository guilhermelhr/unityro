using ROIO.Utils;
using UnityEngine;

public partial class SC {

    [PacketHandler(HEADER, "SC_NOTIFY_BAN", SIZE)]
    public class NOTIFY_BAN : InPacket {

        public const PacketHeader HEADER = PacketHeader.SC_NOTIFY_BAN;
        public const int SIZE = 3;
        public PacketHeader Header => HEADER;

        /// error code:
        ///     0 = BAN_UNFAIR
        ///     1 = server closed -> MsgStringTable[4]
        ///     2 = ID already logged in -> MsgStringTable[5]
        ///     3 = timeout/too much lag -> MsgStringTable[241]
        ///     4 = server full -> MsgStringTable[264]
        ///     5 = underaged -> MsgStringTable[305]
        ///     8 = Server sill recognizes last connection -> MsgStringTable[441]
        ///     9 = too many connections from this ip -> MsgStringTable[529]
        ///     10 = out of available time paid for -> MsgStringTable[530]
        ///     11 = BAN_PAY_SUSPEND
        ///     12 = BAN_PAY_CHANGE
        ///     13 = BAN_PAY_WRONGIP
        ///     14 = BAN_PAY_PNGAMEROOM
        ///     15 = disconnected by a GM -> if( servicetype == taiwan ) MsgStringTable[579]
        ///     16 = BAN_JAPAN_REFUSE1
        ///     17 = BAN_JAPAN_REFUSE2
        ///     18 = BAN_INFORMATION_REMAINED_ANOTHER_ACCOUNT
        ///     100 = BAN_PC_IP_UNFAIR
        ///     101 = BAN_PC_IP_COUNT_ALL
        ///     102 = BAN_PC_IP_COUNT
        ///     103 = BAN_GRAVITY_MEM_AGREE
        ///     104 = BAN_GAME_MEM_AGREE
        ///     105 = BAN_HAN_VALID
        ///     106 = BAN_PC_IP_LIMIT_ACCESS
        ///     107 = BAN_OVER_CHARACTER_LIST
        ///     108 = BAN_IP_BLOCK
        ///     109 = BAN_INVALID_PWD_CNT
        ///     110 = BAN_NOT_ALLOWED_JOBCLASS
        ///     ? = disconnected -> MsgStringTable[3]
        public byte type;

        public void Read(MemoryStreamReader br, int size) {
            type = (byte) br.ReadByte();
            Debug.LogException(new System.Exception($"Disconnected type {type}"));
        }
    }
}