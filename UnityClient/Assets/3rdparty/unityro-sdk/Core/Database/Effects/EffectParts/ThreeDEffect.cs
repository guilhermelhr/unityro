using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Effects.EffectParts {
    [Serializable]
    public class ThreeDEffect : EffectPart {
        public long duration;
        public int duplicates;
        public float timeBetweenDuplication;
        public AudioClip wav;
        public bool attachedEntity;

        /// type = 3D
        ///
        /// - file:
        ///   Texture file name stored in data/texture/(.*.bmp|tga)
        public Texture2D file;

        /// - fileList:
        ///   An array of texture file name stored in data/texture/(.*.bmp|tga)
        public List<Texture2D> fileList;

        /// - frameDelay:
        ///   For how many ticks (time) one texture is shown when using fileList. Used for "hand animated" effects that are created from multiple textures.
        public float frameDelay;

        /// - spriteName:
        ///   Sprite file name stored in data/sprite/AIANA®/(.*).spr
        public SpriteData sprite;

        /// - playSprite:
        ///   if set to true plays the sprite animation
        public bool playSprite;

        /// - sprDelay:
        ///   frame delay for a sprite animation
        public float sprDelay;

        /// - red:
        ///   if set to >0, overrides the red color of the texture
        [Range(0f, 1f)] public float red;

        /// - green:
        ///   if set to >0, overrides the green color of the texture
        [Range(0f, 1f)] public float green;

        /// - blue:
        ///   if set to >0, overrides the blue color of the texture
        [Range(0f, 1f)] public float blue;

        /// - alphaMax:
        ///   sets the opacity of the texture (0.0 ~ 1.0)
        [Range(0f, 1f)] public float alphaMax;

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
        public bool sizeSmooth;

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

        /// - rotatePosX, rotatePosY, rotatePosZ:
        ///   offsets the rotation axis
        public float rotatePosX, rotatePosY, rotatePosZ;

        /// - nbOfRotation:
        ///   sets the number of rotations
        public int nbOfRotation;

        /// - rotateLate:
        ///   sets a delay before the rotation
        public float rotateLate;

        /// - rotateLateDelta:
        ///   when using duplicates, increase rotateLate value between duplicates by this amount
        public float rotateLateDelta;

        /// - rotationClockwise:
        ///   if set to true then rotates clockwise
        public bool rotationClockwise;

        /// - angle:
        ///   sets the starting angle of the texture in degrees (eg: 180=upside down)
        public float angle;

        /// - toAngle:
        ///   sets the final angle of the texture in degrees (eg: 180=upside down)
        public float toAngle;

        /// - rotateToTarget:
        ///   if set to true the upper side of the texture is rotated to face the target
        public bool rotateToTarget;

        /// - rotateWithCamera:
        ///   if set to true the texture rotates if the camera is rotated makig it's sides always face the same coordinate in the 3D environment
        public bool rotateWithCamera;

        /// - zIndex:
        ///   sets the zindex of the texture (closer to the camera or farther)
        public int zIndex;

        /// - shadowTexture:
        ///   if set to true then enables the shadow
        public bool shadowTexture;

        /// - blendMode:
        ///   sets the webgl blendFunc target mode of the cylinder
        ///	 - 1: ZERO
        ///	 - 2: ONE (transparent light effect)
        ///	 - 3: SRC_COLOR
        ///	 - 4: ONE_MINUS_SRC_COLOR
        ///	 - 5: DST_COLOR
        ///	 - 6: ONE_MINUS_DST_COLOR
        ///	 - 7: SRC_ALPHA
        ///	 - 8: ONE_MINUS_SRC_ALPHA (default)
        ///	 - 9: DST_ALPHA
        ///	 - 10: ONE_MINUS_DST_ALPHA
        ///	 - 11: CONSTANT_COLOR
        ///	 - 12: ONE_MINUS_CONSTANT_COLOR
        ///	 - 13: CONSTANT_ALPHA
        ///	 - 14: ONE_MINUS_CONSTANT_ALPHA
        ///	 - 15: SRC_ALPHA_SATURATE
        ///   the webgl blendFunc source mode is always SRC_ALPHA
        public int blendMode;

        /// - sparkling
        ///   if set to true then makes the effect blink/sparkle
        public bool sparkling;

        /// - sparkNumber
        ///   sets how often the effect blinks/sparkles
        public int sparkNumber;

        /// - sparkNumber
        ///   sets how often the effect blinks/sparkles
        public Vector2 sparkNumberRand;

        /// - repeat
        ///   if set to true the effect will play repeatedly until removed
        public bool repeat;

        public GameObject EffectObject;

        public bool fromSrc, toSrc;
        public Vector3 offset;
    }
}