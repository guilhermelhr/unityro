using Core.Effects;
using UnityEngine;

namespace UnityRO.Core.Database {
    [CreateAssetMenu(menuName = "Database Entry/Skill")]
    public class Skill : ScriptableObject {
        public int SkillId;
        public SkillEffect Effect;

        public int BaseDamage;
    }
}