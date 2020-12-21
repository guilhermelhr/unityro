using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemInfo {


    public Slot slot;
    public List<Option> options;
    internal short index;
    internal short viewID;
    internal int itemType;
    internal int location;
    internal int wearState;
    internal int refine;
    internal int expireTime;
    internal short bindOnEquip;
    internal short look;
    internal int randomOptionCount;
    internal int flag;
    internal int ammount;

    public class Slot {
        public ushort card1;
        public ushort card2;
        public ushort card3;
        public ushort card4;
    }

    public class Option {
        public short optIndex;
        public short value;
        public int param1;
    }
}