using ROIO.Models.FileTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Renderer.Sprite {

    [Serializable]
    public class SpriteData : ScriptableObject {

        [SerializeField] public ACT act;
        [SerializeField] public Rect[] rects;

        public UnityEngine.Sprite[] GetSprites(Texture2D Atlas) {
            var _sprites = new List<UnityEngine.Sprite>();

            for (var i = 0; i < rects.Length; i++) {
                var texrect = new Rect(rects[i].x * Atlas.width, rects[i].y * Atlas.height, rects[i].width * Atlas.width, rects[i].height * Atlas.height);

                var sprite = UnityEngine.Sprite.Create(Atlas, texrect, new Vector2(0.5f, 0.5f), SPR.PIXELS_PER_UNIT, 0, SpriteMeshType.FullRect);

                _sprites.Add(sprite);
            }

            return _sprites.ToArray();
        }
    }
}
