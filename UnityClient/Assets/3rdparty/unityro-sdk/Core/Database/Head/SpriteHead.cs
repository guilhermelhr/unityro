using UnityEngine;

namespace UnityRO.Core.Database {
    [CreateAssetMenu(menuName = "Heimdallr/Database Entry/Sprite Head")]
    public class SpriteHead : ScriptableObject {
        public int Id;
        public SpriteData Female;
        public SpriteData Male;
    }
}