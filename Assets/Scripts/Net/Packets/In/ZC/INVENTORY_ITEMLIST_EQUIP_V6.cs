using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_INVENTORY_ITEMLIST_EQUIP_V6")]
    public class INVENTORY_ITEMLIST_EQUIP_V6 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_ITEMLIST_EQUIP_V6;

        public PacketHeader GetHeader() => HEADER;

        public bool Read(BinaryReader br) {
            //TODO what is this packet?
            return true;
        }
    }
}
