using System;
using UnityEngine;

namespace Core.Effects.EffectParts {
    [Serializable]
    public class TwoDEffect : EffectPart {
        public long duration;
        public int duplicates;
        public float timeBetweenDuplication;
        public AudioClip wav;
        public bool attachedEntity;

        /// type = 2D
        ///
        /// - file:
        ///   Texture file name stored in data/texture/(.*.bmp|tga)
        public Texture2D file;

        /// - red:
        ///   if set to >0, overrides the red color of the texture
        [Range(0f, 1f)] public float red;

        /// - green:
        ///   if set to >0, overrides the green color of the texture
        [Range(0f, 1f)] public float green;

        /// - blue:
        ///   if set to >0, overrides the blue color of the texture
        [Range(0f, 1f)] public float blue;

        /// - alphaMaxDelta:
        ///   when using duplicates, increase alpaMax value between duplicates by this amount
        public float alphaMaxDelta;

        /// - fadeIn:
        ///   if set to true the texture will fade in at the beginning of the duration
        public bool fadeIn;

        /// - fadeOut:
        ///   if set to true the texture will fade out at the end of the duration
        public bool fadeOut;

        /// - posx, posy, posz:
        ///   sets the relative position of the texture
        ///
        /// - posxStart, posyStart, poszStart, posxEnd, posyEnd, poszEnd:
        ///   sets the relative starting and ending position of the texture
        /// 
        /// - posxRand, posyRand, poszRand:
        ///   sets a +- range for a random relative position
        /// 
        /// - posxRandDiff, posyRandDiff, poszRandDiff:
        ///   sets a +- range for a random relative starting and ending position. The start and end is 2 different random numbers in the same range.
        ///
        /// - posxStartRand, posyStartRand, poszStartRand, posxEndRand, posyEndRand, poszEndRand:
        ///   sets a +- range for a random relative starting and ending position
        public Vector3 posRelative, posStart, posEnd;

        public Vector3 posRand;
        public Vector3 posRandDiff;
        public Vector3 posStartRand, posEndRand;

        /// - posxSmooth, posySmooth, poszSmooth:
        ///   smoohtes out the movement on an axis
        public float posXSmooth, posYSmooth, posZSmooth;

        /// - size, sizeX, sizeY, sizeStart, sizeEnd, sizeStartX, sizeStartY, sizeEndX, sizeEndY:
        /// - sizeRand, sizeRandx, sizeEndY:
        /// - sizeSmooth:
        ///   works the same way as positions, but effects the size of the texture
        public float size, sizeX, sizeY, sizeStart, sizeEnd, sizeStartX, sizeStartY, sizeEndX, sizeEndY;

        public float sizeRand, sizeRandX, sizeRandY;
        public float sizeSmooth;

        /// - sizeDelta
        ///   when using duplicates, increase sizeDelta value between duplicates by this amount
        public float sizeDelta;

        /// - posxStartRandMiddle, posyStartRandMiddle, poszStartRandMiddle, posxEndRandMiddle, posyEndRandMiddle, poszEndRandMiddle:
        ///   sets the relative middle position of the random range of the starting and ending
        public float posXStartRandMiddle,
            posYStartRandMiddle,
            posZStartRandMiddle,
            posXEndRandMiddle,
            posYEndRandMiddle,
            posZEndRandMiddle;

        /// - sizeRandXMiddle, sizeRandYMiddle:
        ///   sets the middle value for the random range for the size X&Y values
        public float sizeRandXMiddle, sizeRandYMiddle;

        /// - rotate:  
        ///   if set to true makes the texture rotate on it's y axis (turn around)
        public bool rotate;

        /// - angle:
        ///   sets the starting angle of the texture in degrees (eg: 180=upside down)
        public float angle;

        /// - toAngle:
        ///   sets the final angle of the texture in degrees (eg: 180=upside down)
        public float toAngle;

        /// - zIndex:
        ///   sets the zindex of the texture (closer to the camera or farther)
        public int zIndex;
    }
}