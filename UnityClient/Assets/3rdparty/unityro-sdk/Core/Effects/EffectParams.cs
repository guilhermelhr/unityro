using Core.Effects.EffectParts;
using UnityEngine;
using UnityRO.Core.GameEntity;

namespace Core.Effects {
    public struct EffectInitParam {
        public Effect effect;
        public int ownerAID;
        public Vector3 position;
        public long startTick;
        public bool persistent;
        public int repeatEnd;
        public int otherAID; // target/source AID
        public Vector3 otherPosition;

        public CoreGameEntity owner;
        public CoreGameEntity other;
    }

    public struct EffectInstanceParam {
        public Effect effect;
        public int duplicateID;

        public Vector3 position;
        public Vector3 otherPosition;

        public long startTick;
        public long endTick;
    }
}