namespace UnityRO.Core.GameEntity {
    public abstract class CoreSpriteGameEntity : CoreGameEntity {
        public abstract Direction Direction { get; set; }
        public abstract int HeadDirection { get; }
    }
}