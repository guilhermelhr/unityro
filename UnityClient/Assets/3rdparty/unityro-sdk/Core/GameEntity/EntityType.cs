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
        return type switch {
                   -3 => EntityType.EFFECT,
                   -2 => EntityType.UNKNOWN,
                   -1 => EntityType.WARP,
                   0 => EntityType.PC,
                   1 => EntityType.DISGUISED,
                   5 => EntityType.MOB,
                   6 or 12 => EntityType.NPC,
                   7 => EntityType.PET,
                   8 => EntityType.HOM,
                   9 => EntityType.MERC,
                   10 => EntityType.ELEM,
                   11 => EntityType.ITEM,
                   _ => EntityType.UNKNOWN,
               };
    }
}