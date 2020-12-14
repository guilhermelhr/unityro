using System.IO;
using System.Net;

struct PACKET_AC_ACCEPT_LOGIN {     //ÕÊ»§µÇÂ¼·þÎñÆ÷ÉÏµÄÈ¨ÏÞ 
    short PacketType;
    short PacketLength;      //	variable length packet
    int AuthCode;           //ÑéÖ¤Âë 
    long AID;
    long userLevel;            //¸ù¾ÝÓÃ»§µÄ·þÎñË®Æ½£¬ÒÔÈ·¶¨ÊÇ·ñÁ¬½Ó¡£ 
                                //	DWORD	lastLoginIP;		
    char[] lastLoginIP; ////  ×îºó£¬Á¬½ÓµÄIP 
    char[] lastLoginTime; //  ×îºó£¬Ê±¼äÈÕÖ¾¡£ 
    byte Sex;
    //SERVER_ADDRÐÅÏ¢£¨ PacketLength £¬´óÐ¡£¨ PACKET_AC_ACCEPT_LOGIN £© /´óÐ¡£¨ SERVER_ADDR £© gaemankeumÀ´ 
};

public partial class AC {
    [PacketHandler(HEADER, "AC_ACCEPT_LOGIN")]
    public class ACCEPT_LOGIN : InPacket {

        public const PacketHeader HEADER = PacketHeader.AC_ACCEPT_LOGIN3;
        public int LoginID1 { get; set; }
        public int AccountID { get; set; }
        public int LoginID2 { get; set; }
        public byte Sex { get; set; }
        public CharServerInfo[] Servers { get; set; }

        public PacketHeader GetHeader() => HEADER;

        public bool Read(BinaryReader br) {

            LoginID1 = br.ReadLong();
            AccountID = br.ReadLong();
            LoginID2 = br.ReadLong();
            br.Seek(30, SeekOrigin.Current);
            Sex = br.ReadUByte();
            br.Seek(17, SeekOrigin.Current);

            /**
             * This seems very wrong to me
             * Which always return 5
             * Is there another way of knowing the server count?
             * header(64) + size (160) * server_num (taken from rAthena)
             * Note: Here we've already skipped 4 bytes from reading the cmd and size
             */

            long serverCount = (br.Length - br.Position) / 32;
            Servers = new CharServerInfo[serverCount];
            for(int i = 0; i < serverCount; i++) {
                CharServerInfo csi = new CharServerInfo();
                csi.IP = new IPAddress(br.ReadULong());
                csi.Port = br.ReadUShort();
                csi.Name = br.ReadBinaryString(20);
                csi.UserCount = br.ReadUShort();
                csi.State = br.ReadShort();
                csi.Property = br.ReadUShort();

                Servers[i] = csi;
            }

            return true;
        }
    }
}