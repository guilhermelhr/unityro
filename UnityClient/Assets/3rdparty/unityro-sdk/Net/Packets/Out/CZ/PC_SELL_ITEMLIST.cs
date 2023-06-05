using System.Collections.Generic;

public partial class CZ {
    public class PC_SELL_ITEMLIST : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_PC_SELL_ITEMLIST;
        public const int SIZE = -1;

        public List<PC_SELL_ITEMLIST_sub> items = new List<PC_SELL_ITEMLIST_sub>();

        public PC_SELL_ITEMLIST() : base(HEADER, SIZE) { }

        public override void Send() {
            foreach(var item in items) {
                Write(item.InventoryIndex);
                Write(item.Amount);
            }

            base.Send();
        }

        public class PC_SELL_ITEMLIST_sub {
            public short InventoryIndex;
            public short Amount;
        }
    }
}
