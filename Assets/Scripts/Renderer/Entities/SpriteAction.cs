public class SpriteAction {
    public virtual int IDLE { get; } = 0;
    public virtual int ATTACK { get; } = -2;
    public virtual int WALK { get; } = -1;
    public virtual int SIT { get; } = -1;
    public virtual int PICKUP { get; } = -1;
    public virtual int READYFIGHT { get; } = -1;
    public virtual int FREEZE { get; } = -1;
    public virtual int HURT { get; } = -1;
    public virtual int DIE { get; } = -1;
    public virtual int FREEZE2 { get; } = -1;
    public virtual int ATTACK1 { get; } = -1;
    public virtual int ATTACK2 { get; } = -1;
    public virtual int ATTACK3 { get; } = -1;
    public virtual int SKILL { get; } = -1;
    public virtual int ACTION { get; } = -1;
    public virtual int SPECIAL { get; } = -1;
    public virtual int PERF1 { get; } = -1;
    public virtual int PERF2 { get; } = -1;
    public virtual int PERF3 { get; } = -1;

    public class PC : SpriteAction {
        public override int IDLE { get; } = 0;
        public override int WALK { get; } = 1;
        public override int SIT { get; } = 2;
        public override int PICKUP { get; } = 3;
        public override int READYFIGHT { get; } = 4;
        public override int ATTACK1 { get; } = 5;
        public override int HURT { get; } = 6;
        public override int FREEZE { get; } = 7;
        public override int DIE { get; } = 8;
        public override int FREEZE2 { get; } = 9;
        public override int ATTACK2 { get; } = 10;
        public override int ATTACK3 { get; } = 11;
        public override int SKILL { get; } = 12;
    }

    public class MOB : SpriteAction {
        public override int IDLE { get; } = 0;
        public override int WALK { get; } = 1;
        public override int ATTACK { get; } = 2;
        public override int HURT { get; } = 3;
        public override int DIE { get; } = 4;
    }

    public class PET : SpriteAction {
        public override int IDLE { get; } = 0;
        public override int WALK { get; } = 1;
        public override int ATTACK { get; } = 2;
        public override int HURT { get; } = 3;
        public override int DIE { get; } = 4;
        public override int SPECIAL { get; } = 5;
        public override int PERF1 { get; } = 6;
        public override int PERF2 { get; } = 7;
        public override int PERF3 { get; } = 8;
    }

    public class NPC : SpriteAction {
        public override int IDLE { get; } = 0;
        public override int WALK { get; } = 1;
    }

    public class HOM : SpriteAction {
        public override int IDLE { get; } = 0;
        public override int WALK { get; } = 1;
        public override int ATTACK { get; } = 2;
        public override int HURT { get; } = 3;
        public override int DIE { get; } = 4;
        public override int ATTACK2 { get; } = 5;
        public override int ATTACK3 { get; } = 6;
        public override int ACTION { get; } = 7;
    }
}