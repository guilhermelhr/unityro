using System;
using UnityEngine;

namespace Core.Effects.EffectParts {
    [CreateAssetMenu(menuName = "Database Entry/Effect")]
    [Serializable]
    public class Effect : ScriptableObject {
        public int EffectId;

        public CylinderEffectPart[] CylinderParts;
        public SprEffect[] SPRParts;
        public StrEffect[] STRParts;
        public ThreeDEffect[] ThreeDParts;
        public TwoDEffect[] TwoDParts;
    }
}