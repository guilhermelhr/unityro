using UnityEngine;

namespace UnityRO.Core.Database {
    
    [CreateAssetMenu(menuName = "Database Entry/Sprite Job")]
    public class SpriteJob : Job {
        public SpriteData Male;
        public SpriteData Female;
    }
}