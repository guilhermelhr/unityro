using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Effects {
    [Serializable]
    public class ThreeDEffectPart {
        public int AID;

        public Texture2D texture; //textureName
        public List<Texture2D> textureList; //textureNameList

        public float frameDelay;

        public int zIndex;

        public bool fadeOut;
        public bool fadeIn;
        public bool useShadow; //shadowTexture

        public SpriteData sprite;
        public bool playSprite;
        public float spriteDelay;

        public Vector3 rotatePos;
        public int numberOfRotations; //nbOfRotation
        public float rotateLate;
        public bool isRotationClockwise; //rotationClockwise

        public bool hasSparkling; //sparkling
        public float sparkNumber;

        public float alphaMax;

        public Vector2 sizeStart;
        public Vector2 sizeEnd;

        public Vector3 offset;

        public long startTick;
        public long endTick;

        public bool repeat;

        public int blendMode;

        public bool rotateToTarget;

        public double angle;

        public bool rotateWithCamera;

        public float targetAngle; //toAngle

        public bool rotate;
        public bool sizeSmooth;

        public Color color; //red, green, blue

        public Vector3 position;
        public Vector3 positionStart; //posStart
        public Vector3 positionEnd; //posEnd;

        public bool posXSmooth;
        public bool posYSmooth;
        public bool posZSmooth;
    }
}