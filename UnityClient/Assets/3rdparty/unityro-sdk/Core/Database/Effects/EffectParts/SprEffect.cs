using System;
using UnityEngine;

namespace Core.Effects.EffectParts {
    [Serializable]
    public class SprEffect : EffectPart {
        public long duration;
        public int duplicates;
        public float timeBetweenDuplication;
        public AudioClip wav;
        public bool attachedEntity;
        
        // file store in data/sprite/AIANA®/(.*).spr
        public SpriteData file;

        public bool head;
        public bool stopAtEnd;
        public bool direction;
    }
}