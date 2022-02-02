public enum EntityType {
    EFFECT,
    UNKNOWN,
    WARP,
    PC,
    DISGUISED,
    MOB,
    NPC,
    PET,
    HOM,
    MERC,
    ELEM,
    ITEM,
}

public static class EntityTypeExtension {
    public static EntityType GetEntityType(this int type) {
        switch (type) {
            case -3:
                return EntityType.EFFECT;
            case -2:
                return EntityType.UNKNOWN;
            case -1:
                return EntityType.WARP;
            case 0:
                return EntityType.PC;
            case 1:
                return EntityType.DISGUISED;
            case 5:
                return EntityType.MOB;
            case 6:
            case 12:
                return EntityType.NPC;
            case 7:
                return EntityType.PET;
            case 8:
                return EntityType.HOM;
            case 9:
                return EntityType.MERC;
            case 10:
                return EntityType.ELEM;
            case 11:
                return EntityType.ITEM;
            default:
                return EntityType.UNKNOWN;
       }
    }
}