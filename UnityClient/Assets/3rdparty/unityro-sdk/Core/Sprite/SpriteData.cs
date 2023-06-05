using ROIO.Models.FileTypes;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpriteData : ScriptableObject {
    public int jobId;
    public ACT act;
    
    public Sprite[] sprites;
    
    public Rect[] rects;
    public Texture2D atlas;
    public Texture2D[] palettes;

    public Sprite[] GetSprites() {
        return GetSprites(atlas);
    }
    
    public Sprite[] GetSprites(Texture2D atlas) {
        return rects
            .Select(t =>
                new Rect(t.x * atlas.width, t.y * atlas.height, t.width * atlas.width, t.height * atlas.height)).Select(
                texrect => UnityEngine.Sprite.Create(atlas, texrect, new Vector2(0.5f, 0.5f), SPR.PIXELS_PER_UNIT, 0,
                    SpriteMeshType.FullRect)).ToArray();
    }
}