using System.Collections.Generic;
using UnityEngine;

namespace UnityRO.Core.Database {

    [CreateAssetMenu(menuName = "Database/Sprite Head")]
    public class SpriteHeadDatabase : ScriptableObject {
        public List<SpriteHead> Values;
    }
}