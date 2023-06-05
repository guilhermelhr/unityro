using System.Collections.Generic;

public partial class CZ {
    public class PC_PURCHASE_ITEMLIST : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_PC_PURCHASE_ITEMLIST;
        public const int SIZE = -1;

        public List<PC_PURCHASE_ITEMLIST_sub> items = new List<PC_PURCHASE_ITEMLIST_sub>();

        public PC_PURCHASE_ITEMLIST() : base(HEADER, SIZE) { }

        public override void Send() {
            foreach(var item in items) {
                Write(item.Amount);
                Write(item.ItemId);
            }

            base.Send();
        }

        public class PC_PURCHASE_ITEMLIST_sub {
            public short Amount;
            public int ItemId;
        }
    }
}
