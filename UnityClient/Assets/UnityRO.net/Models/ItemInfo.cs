using System.Collections.Generic;
using UnityEngine;

public class ItemInfo {

    public Slot slot;
    public List<Option> options;

    public short index;
    public int ItemID;
    public int itemType;
    public int location;
    public int wearState;
    public byte refine;
    public byte enchantgrade;
    public int expireTime;
    public ushort bindOnEquip;
    public ushort wItemSpriteNumber;
    public byte randomOptionCount;
    public bool IsDamaged;
    public int flag;
    public int amount;
    public short viewID;

    public bool IsIdentified => (flag & 0x1) == 1;
    public bool IsFavorite => (flag & 0x2) == 1;

    public Item item;
    public Texture2D res;
    public Texture2D collection;
    public InventoryType tab;

    public class Slot {
        public int card1;
        public int card2;
        public int card3;
        public int card4;
    }

    public class Option {
        public short optIndex;
        public short value;
        public byte param1;
    }
}