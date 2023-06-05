using UnityEngine;

namespace UnityRO.Core.GameEntity {
    public abstract class CoreGameEntity : ManagedMonoBehaviour, INetworkEntity {
        public abstract GameEntityBaseStatus Status { get; }

        public abstract void ChangeMotion(MotionRequest request, MotionRequest? nextRequest = null);
        public abstract void ChangeDirection(Direction direction);
        public abstract void LookTo(Vector3 position);

        public abstract void Init(GameEntityBaseStatus gameEntityBaseStatus);
        public abstract void Spawn(GameEntityBaseStatus gameEntityBaseStatus, int[] PosDir, bool forceNorthDirection);

        public abstract bool HasAuthority();
        public int GetEntityType() => (int)Status.EntityType;
        public int GetEntityAID() => Status.AID;
        public string GetEntityName() => Status.Name;

        public abstract int GetEntityGID();

        /// <summary>
        /// Use this when you have an offset to add to the current position to walk to
        /// </summary>
        /// <param name="destination"></param>
        public abstract void RequestOffsetMovement(Vector2 destination);

        /// <summary>
        /// Use this when you have an absolute coordinate to walk to
        /// </summary>
        /// <param name="destination"></param>
        public abstract void RequestMovement(Vector2 destination);

        public abstract void Vanish(VanishType vanishType);
        public abstract void SetAction(EntityActionRequest actionRequest, bool isSource);
        public abstract void SetAttackSpeed(ushort actionRequestSourceSpeed);
    }
}