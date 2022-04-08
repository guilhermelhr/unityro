using ROIO.Models.FileTypes;
using System;
using UnityEngine;

namespace Assets.Scripts.Renderer.Sprite {

    [Serializable]
    public class SpriteData : ScriptableObject {

        [SerializeField]
        public ACT act;
        [SerializeField]
        public Rect[] rects;

        public UnityEngine.Sprite[] GetSpritesFromAtlas(Texture2D atlas) {
            UnityEngine.Sprite[] sprites = new UnityEngine.Sprite[rects.Length];
            for (int i = 0; i < rects.Length; i++) {
                Rect rect = rects[i];
                /**
                 * This anchor offset is a hack
                 */
                sprites[i] = UnityEngine.Sprite.Create(atlas, rect, Vector2.zero, SPR.PIXELS_PER_UNIT, 0, SpriteMeshType.FullRect);
            }

            return sprites;
        }
    }
}
