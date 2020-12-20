public enum DamageType : byte {
    HEAL = 1 << 0,
    MISS = 1 << 1,
    DAMAGE = 1 << 2,
    ENEMY = 1 << 3,
    COMBO = 1 << 4,
    COMBO_FINAL = 1 << 5,
    SP = 1 << 6
}