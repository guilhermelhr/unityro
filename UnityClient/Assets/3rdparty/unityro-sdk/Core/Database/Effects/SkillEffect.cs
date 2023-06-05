using Core.Effects.EffectParts;
using UnityEngine;

namespace Core.Effects {
    [CreateAssetMenu(menuName = "Database Entry/Skill Effect")]
    public class SkillEffect : ScriptableObject {
        public Effect Effect;
        public Effect CasterEffect;
        public Effect GroundEffect;
        public Effect HitEffect;
        public Effect BeforeHitEffect;
        public Effect BeginCastEffect;
        public Effect SuccessEffect;
        public Effect CasterSuccessEffect;

        public bool hideCastBar;
        public bool hideCastAura;
    }
}