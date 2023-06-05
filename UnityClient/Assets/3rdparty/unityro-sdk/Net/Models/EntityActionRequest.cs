public class EntityActionRequest {
    public uint AID;
    public uint targetAID;
    public uint startTime;
    public ushort sourceSpeed;
    public ushort targetSpeed;
    public int damage;
    public short count;
    public ActionRequestType action;
    public int leftDamage;

    public bool IsAttackAction() {
        return action is ActionRequestType.ATTACK
            or ActionRequestType.ATTACK_LUCKY
            or ActionRequestType.ATTACK_REPEAT
            or ActionRequestType.ATTACK_CRITICAL
            or ActionRequestType.ATTACK_MULTIPLE
            or ActionRequestType.ATTACK_NOMOTION
            or ActionRequestType.ATTACK_MULTIPLE_NOMOTION;
    }
}

public enum ActionRequestType : byte {
    ATTACK, ITEMPICKUP, SIT, STAND, ATTACK_NOMOTION, SPLASH,
    SKILL, ATTACK_REPEAT, ATTACK_MULTIPLE, ATTACK_MULTIPLE_NOMOTION,
    ATTACK_CRITICAL, ATTACK_LUCKY,
    TOUCHSKILL
}