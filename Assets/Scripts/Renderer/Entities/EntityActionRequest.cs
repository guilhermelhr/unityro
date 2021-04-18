public class EntityActionRequest {
    public uint GID;
    public uint targetGID;
    public uint startTime;
    public ushort sourceSpeed;
    public ushort targetSpeed;
    public int damage;
    public short count;
    public ActionRequestType action;
    public int leftDamage;
}

public enum ActionRequestType : byte {
    ATTACK, ITEMPICKUP, SIT, STAND, ATTACK_NOMOTION, SPLASH,
    SKILL, ATTACK_REPEAT, ATTACK_MULTIPLE, ATTACK_MULTIPLE_NOMOTION,
    ATTACK_CRITICAL, ATTACK_LUCKY,
    TOUCHSKILL
}