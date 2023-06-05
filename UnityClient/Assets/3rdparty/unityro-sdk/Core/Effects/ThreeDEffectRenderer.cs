using System;
using System.Collections.Generic;
using Core.Effects.EffectParts;
using UnityEngine;
using UnityRO.Core;
using UnityRO.core.Effects;
using Random = UnityEngine.Random;

namespace Core.Effects {
    public class ThreeDEffectRenderer : ManagedMonoBehaviour {
        [SerializeField] public ThreeDEffect Effect;

        private ThreeDEffectPart _part;

        private MeshRenderer MeshRenderer; //todo use this instead of SpriteRenderer
        private SpriteRenderer SpriteRenderer;

        private Sprite MainSprite;
        private List<Sprite> SpriteList = new();

        public void Init(ThreeDEffect effect, EffectInstanceParam instanceParam, EffectInitParam initParam) {
            _part = new ThreeDEffectPart();
            var position = instanceParam.position;
            var otherPosition = instanceParam.otherPosition;
            var startTick = instanceParam.startTick;
            var endTick = instanceParam.endTick;
            var AID = initParam.ownerAID;

            _part.AID = AID;
            _part.texture = effect.file;
            _part.textureList = effect.fileList ?? new List<Texture2D>();
            _part.frameDelay = effect.frameDelay != null ? effect.frameDelay : 10f;
            _part.zIndex = effect.zIndex != null ? effect.zIndex : 0;
            _part.fadeOut = effect.fadeOut != null ? effect.fadeOut : false;
            _part.fadeIn = effect.fadeIn != null ? true : false;
            _part.useShadow = effect.shadowTexture != null ? true : false;
            _part.sprite = effect.sprite;
            _part.playSprite = effect.playSprite != null ? true : false;
            _part.spriteDelay = effect.sprDelay != null ? effect.sprDelay : 0f;

            _part.rotatePos = new Vector3(
                effect.rotatePosX > 0 ? effect.rotatePosX : 0,
                effect.rotatePosY > 0 ? effect.rotatePosY : 0,
                effect.rotatePosZ > 0 ? effect.rotatePosZ : 0);
            _part.numberOfRotations = effect.nbOfRotation > 0 ? effect.nbOfRotation : 1;

            _part.rotateLate = (effect.rotateLate > 0) ? effect.rotateLate : 0;
            _part.rotateLate +=
                (effect.rotateLateDelta > 0) ? effect.rotateLateDelta * instanceParam.duplicateID : 0;

            _part.isRotationClockwise = effect.rotationClockwise != null ? true : false;
            _part.hasSparkling = effect.sparkling != null ? true : false;
            if (effect.sparkNumber > 0) {
                _part.sparkNumber = effect.sparkNumber;
            } else {
                if (effect.sparkNumberRand != null) {
                    _part.sparkNumber = RandBetween(effect.sparkNumberRand[0], effect.sparkNumberRand[1]);
                } else {
                    _part.sparkNumber = 1;
                }
            }

            var alphaMax = effect.alphaMax != null ? Mathf.Max(Mathf.Min(effect.alphaMax, 1), 0) : 1;
            _part.alphaMax =
                Mathf.Max(
                    Mathf.Min(
                        alphaMax + (effect.alphaMaxDelta != null
                            ? effect.alphaMaxDelta * instanceParam.duplicateID
                            : 0), 1),
                    0);

            var red = 0f;
            var green = 0f;
            var blue = 0f;

            if (effect.red > 0) red = effect.red;
            else red = 1;
            if (effect.green > 0) green = effect.green;
            else green = 1;
            if (effect.blue > 0) blue = effect.blue;
            else blue = 1;
            _part.color = new Color(red, green, blue);

            _part.position = position;
            _part.positionStart = effect.posStart != null ? effect.posStart : Vector3.zero;
            _part.positionEnd = effect.posEnd != null ? effect.posEnd : Vector3.zero;

            if (effect.posRelative is { x: > 0 }) {
                _part.positionStart.x = effect.posRelative.x;
                _part.positionEnd.x = effect.posRelative.x;
            }

            if (effect.posRand is { x: > 0 }) {
                _part.positionStart.x = RandBetween(-effect.posRand.x, effect.posRand.x);
                _part.positionEnd.x = _part.positionStart.x;
            }

            if (effect.posRandDiff is { x: > 0 }) {
                _part.positionStart.x = RandBetween(-effect.posRandDiff.x, effect.posRandDiff.x);
                _part.positionEnd.x = RandBetween(-effect.posRandDiff.x, effect.posRandDiff.x);
            }

            if (effect.posStartRand is { x: > 0 }) {
                var posXStartRandMiddle = effect.posXStartRandMiddle != null ? effect.posXStartRandMiddle : 0;
                _part.positionStart.x = RandBetween(posXStartRandMiddle - effect.posStartRand.x,
                    posXStartRandMiddle + effect.posStartRand.x);
            }

            if (effect.posEndRand is { x: > 0 }) {
                var posXEndRandMiddle = effect.posXEndRandMiddle != null ? effect.posXEndRandMiddle : 0;
                _part.positionEnd.x = RandBetween(posXEndRandMiddle - effect.posEndRand.x,
                    posXEndRandMiddle + effect.posEndRand.x);
            }

            _part.posXSmooth = effect.posXSmooth != null ? true : false;

            if (effect.posRelative is { y: > 0 }) {
                _part.positionStart.y = effect.posRelative.y;
                _part.positionEnd.y = effect.posRelative.y;
            }

            if (effect.posRand is { y: > 0 }) {
                _part.positionStart.y = RandBetween(-effect.posRand.y, effect.posRand.y);
                _part.positionEnd.y = _part.positionStart.y;
            }

            if (effect.posRandDiff is { y: > 0 }) {
                _part.positionStart.y = RandBetween(-effect.posRandDiff.y, effect.posRandDiff.y);
                _part.positionEnd.y = RandBetween(-effect.posRandDiff.y, effect.posRandDiff.y);
            }

            if (effect.posStartRand is { y: > 0 }) {
                var posyStartRandMiddle = effect.posYStartRandMiddle != null ? effect.posYStartRandMiddle : 0;
                _part.positionStart.y = RandBetween(posyStartRandMiddle - effect.posStartRand.y,
                    posyStartRandMiddle + effect.posStartRand.y);
            }

            if (effect.posEndRand is { y: > 0 }) {
                var posyEndRandMiddle = effect.posYEndRandMiddle != null ? effect.posYEndRandMiddle : 0;
                _part.positionEnd.y = RandBetween(posyEndRandMiddle - effect.posEndRand.y,
                    posyEndRandMiddle + effect.posEndRand.y);
            }

            _part.posYSmooth = effect.posYSmooth != null ? true : false;

            if (effect.posRelative is { z: > 0 }) {
                _part.positionStart.z = effect.posRelative.z;
                _part.positionEnd.z = effect.posRelative.z;
            }

            if (effect.posRand is { z: > 0 }) {
                _part.positionStart.z = RandBetween(-effect.posRand.z, effect.posRand.z);
                _part.positionEnd.z = _part.positionStart.z;
            }

            if (effect.posRandDiff is { z: > 0 }) {
                _part.positionStart.z = RandBetween(-effect.posRandDiff.z, effect.posRandDiff.z);
                _part.positionEnd.z = RandBetween(-effect.posRandDiff.z, effect.posRandDiff.z);
            }

            if (effect.posStartRand is { z: > 0 }) {
                var posZStartRandMiddle = effect.posZStartRandMiddle != null ? effect.posZStartRandMiddle : 0;
                _part.positionStart.z = RandBetween(posZStartRandMiddle - effect.posStartRand.z,
                    posZStartRandMiddle + effect.posStartRand.z);
            }

            if (effect.posEndRand is { z: > 0 }) {
                var poszEndRandMiddle = effect.posZEndRandMiddle != null ? effect.posZEndRandMiddle : 0;
                _part.positionEnd.z = RandBetween(poszEndRandMiddle - effect.posEndRand.z,
                    poszEndRandMiddle + effect.posEndRand.z);
            }

            var xOffset = effect.offset is { x: > 0f } ? effect.offset.x : 0f;
            var yOffset = effect.offset is { y: > 0f } ? effect.offset.y : 0f;
            var zOffset = effect.offset is { z: > 0f } ? effect.offset.z : 0f;
            _part.offset = new Vector3(xOffset, yOffset, zOffset);

            _part.posZSmooth = effect.posZSmooth != null ? true : false;
            if (effect.fromSrc) {
                var randStart = new[] {
                    effect.posStartRand.x > 0
                        ? RandBetween(-effect.posStartRand.x, effect.posStartRand.x)
                        : 0,
                    effect.posStartRand.y > 0
                        ? RandBetween(-effect.posStartRand.y, effect.posStartRand.y)
                        : 0,
                    effect.posStartRand.z > 0
                        ? RandBetween(-effect.posStartRand.z, effect.posStartRand.z)
                        : 0
                };
                var randEnd = new[] {
                    effect.posEndRand.x > 0
                        ? RandBetween(-effect.posEndRand.x, effect.posEndRand.x)
                        : 0,
                    effect.posEndRand.y > 0
                        ? RandBetween(-effect.posEndRand.y, effect.posEndRand.y)
                        : 0,
                    effect.posEndRand.z > 0
                        ? RandBetween(-effect.posEndRand.z, effect.posEndRand.z)
                        : 0
                };

                _part.positionStart.x = 0 + xOffset + randStart[0];
                _part.positionEnd.x = (otherPosition[0] - position[0]) + xOffset + randEnd[0];
                _part.positionStart.y = 0 + yOffset + randStart[1];
                _part.positionEnd.y = (otherPosition[1] - position[1]) + yOffset + randEnd[1];
                _part.positionStart.z = 0 + zOffset + randStart[2];
                _part.positionEnd.z = (otherPosition[2] - position[2]) + zOffset + randEnd[2];
            }

            if (effect.toSrc) {
                var randStart = new[] {
                    effect.posStartRand.x > 0
                        ? RandBetween(-effect.posStartRand.x, effect.posStartRand.x)
                        : 0,
                    effect.posStartRand.y > 0
                        ? RandBetween(-effect.posStartRand.y, effect.posStartRand.y)
                        : 0,
                    effect.posStartRand.z > 0
                        ? RandBetween(-effect.posStartRand.z, effect.posStartRand.z)
                        : 0
                };
                var randEnd = new[] {
                    effect.posEndRand.x > 0
                        ? RandBetween(-effect.posEndRand.x, effect.posEndRand.x)
                        : 0,
                    effect.posEndRand.y > 0
                        ? RandBetween(-effect.posEndRand.y, effect.posEndRand.y)
                        : 0,
                    effect.posEndRand.z > 0
                        ? RandBetween(-effect.posEndRand.z, effect.posEndRand.z)
                        : 0
                };

                _part.positionStart.x = (otherPosition[0] - position[0]) + xOffset + randStart[0];
                _part.positionEnd.x = 0 + xOffset + randEnd[0];
                _part.positionStart.y = (otherPosition[1] - position[1]) + yOffset + randStart[1];
                _part.positionEnd.y = 0 + yOffset + randEnd[1];
                _part.positionStart.z = (otherPosition[2] - position[2]) + zOffset + randStart[2];
                _part.positionEnd.z = 0 + zOffset + randEnd[2];
            }

            if (effect.size > 0) {
                _part.sizeStart = new Vector2(effect.size, effect.size);
                _part.sizeEnd = new Vector2(effect.size, effect.size);
            } else {
                _part.sizeStart = Vector2.one;
                _part.sizeEnd = Vector2.one;
            }

            if (effect.sizeDelta > 0) {
                _part.sizeStart.x += effect.sizeDelta * instanceParam.duplicateID;
                _part.sizeStart.y += effect.sizeDelta * instanceParam.duplicateID;
                _part.sizeEnd.x += effect.sizeDelta * instanceParam.duplicateID;
                _part.sizeEnd.y += effect.sizeDelta * instanceParam.duplicateID;
            }

            if (effect.sizeStart > 0) {
                _part.sizeStart = new Vector2(effect.sizeStart, effect.sizeStart);
            }

            if (effect.sizeEnd > 0) {
                _part.sizeEnd = new Vector2(effect.sizeEnd, effect.sizeEnd);
            }

            if (effect.sizeX > 0) {
                _part.sizeStart.x = effect.sizeX;
                _part.sizeEnd.x = effect.sizeX;
            }

            if (effect.sizeY > 0) {
                _part.sizeStart.y = effect.sizeY;
                _part.sizeEnd.y = effect.sizeY;
            }

            if (effect.sizeStartX > 0) _part.sizeStart.x = effect.sizeStartX;
            if (effect.sizeStartY > 0) _part.sizeStart.y = effect.sizeStartY;
            if (effect.sizeEndX > 0) _part.sizeEnd.x = effect.sizeEndX;
            if (effect.sizeEndY > 0) _part.sizeEnd.y = effect.sizeEndY;
            if (effect.sizeRand > 0) {
                _part.sizeStart.x = effect.size + RandBetween(-effect.sizeRand, effect.sizeRand);
                _part.sizeStart.y = _part.sizeStart.x;
                _part.sizeEnd.x = _part.sizeStart.x;
                _part.sizeEnd.y = _part.sizeStart.x;
            }

            if (effect.sizeRandX > 0) {
                var sizeRandXMiddle = effect.sizeRandXMiddle > 0 ? effect.sizeRandXMiddle : 100;
                _part.sizeStart.x =
                    RandBetween(sizeRandXMiddle - effect.sizeRandX, sizeRandXMiddle + effect.sizeRandX);
                _part.sizeEnd.x = _part.sizeStart.x;
            }

            if (effect.sizeRandY > 0) {
                var sizeRandYMiddle = effect.sizeRandYMiddle > 0 ? effect.sizeRandYMiddle : 100;
                _part.sizeStart.y =
                    RandBetween(sizeRandYMiddle - effect.sizeRandY, sizeRandYMiddle + effect.sizeRandY);
                _part.sizeEnd.y = _part.sizeStart.y;
            }

            _part.sizeSmooth = effect.sizeSmooth != null ? true : false;
            _part.angle = effect.angle > 0 ? effect.angle : 0;
            _part.rotate = effect.rotate ? true : false;
            _part.targetAngle = effect.toAngle > 0 ? effect.toAngle : 0;

            if (_part.useShadow) {
                //var GroundEffect = require('Renderer/Effects/GroundEffect');
                //require('Renderer/EffectManager').add(new GroundEffect(this.posxStart, this.posyStart), 1000000);
            }

            _part.startTick = startTick;
            _part.endTick = endTick;
            _part.repeat = effect.repeat;
            _part.blendMode = effect.blendMode;

            if (effect.rotateToTarget) {
                _part.rotateToTarget = true;
                var endPos = _part.positionEnd - _part.positionStart;
                _part.angle += 90 - Mathf.Atan2(endPos.y, endPos.x) * (180 / Math.PI);
            }

            _part.rotateWithCamera = effect.rotateWithCamera ? true : false;

            if (_part.textureList.Count == 0) {
                MainSprite = Sprite.Create(_part.texture,
                    new Rect(0.0f, 0.0f, _part.texture.width, _part.texture.height),
                    new Vector2(0.5f, 0.5f), 32.0f);
            } else {
                foreach (var tex in _part.textureList) {
                    SpriteList.Add(Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f),
                        32.0f));
                }
            }

            isReady = true;
            _part.startTick = GameManager.Tick;
            _part.endTick = GameManager.Tick + effect.duration;
        }

        private void Start() {
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            SpriteRenderer.drawMode = SpriteDrawMode.Sliced;
        }

        private bool isReady = false;

        public override void ManagedUpdate() {
            if (_part.startTick > GameManager.Tick || !isReady) return;

            if (GameManager.Tick > _part.endTick) {
                Destroy(gameObject);
                return;
            }

            if (_part.blendMode is > 0 and < 16) {
                //gl.blendFunc(gl.SRC_ALPHA, blendMode[this.blendMode]);
            } else {
                //gl.blendFunc(gl.SRC_ALPHA, gl.ONE_MINUS_SRC_ALPHA);
            }

            var start = GameManager.Tick - _part.startTick;
            var end = (_part.endTick - _part.startTick) * 1f;
            var steps = start / end * 100;

            if (steps > 100) steps = 100.0f;

            // todo
            // if (!this.spriteResource)...
            // if (this.spriteResource)...
            // if (this.textureList.length > 0) ...
            // SpriteRenderer.image.texture = this.texture;
            // SpriteRenderer.zIndex = this.zindex;

            var sprite = MainSprite;
            if (_part.textureList.Count > 0) {
                var frame = Mathf.FloorToInt((GameManager.Tick - _part.startTick) / _part.frameDelay) %
                    _part.textureList.Count;
                sprite = SpriteList[frame];
            }

            SpriteRenderer.sprite = sprite;

            var x = CalculatePosition(steps, _part.rotatePos.x, _part.isRotationClockwise, _part.posXSmooth,
                _part.positionStart.x, _part.positionEnd.x);
            var y = CalculatePosition(steps, _part.rotatePos.y, false, _part.posYSmooth, _part.positionStart.y,
                _part.positionEnd.y);
            var z = CalculatePosition(steps, 0f, false, _part.posZSmooth, _part.positionStart.z, _part.positionEnd.z);

            var positionDelta = new Vector3(x, y, z);
            gameObject.transform.localPosition = positionDelta;

            // this is probably the wrong var name
            // it seems that it is checking to see whether this instance of the renderer is a shadow or not
            if (_part.useShadow) {
                // SpriteRenderer.position[2] = Altitude.getCellHeight(SpriteRenderer.position[0], SpriteRenderer.position[0]);
            }

            var alpha = _part.alphaMax;
            if (_part.fadeIn && start < end / 4) {
                alpha = start * _part.alphaMax / (end / 4);
            } else if (_part.fadeOut && start > end / 2 + end / 4) {
                alpha = (end - start) * _part.alphaMax / (end / 4);
            } else if (_part.hasSparkling) {
                alpha = _part.alphaMax *
                        ((Mathf.Cos(steps * 11 * _part.sparkNumber * Mathf.PI / 180) + 1) / 2);
            }

            alpha = alpha switch {
                < 0 => 0.0f,
                > 1 => 1.0f,
                _ => alpha
            };
            _part.color.a = alpha;
            SpriteRenderer.color = _part.color;

            // sprite default size seem to be 4,4
            // while on robrowser it seem to be 100,100
            // so we transform using the desired size then
            // map back to the values it was supposed to be
            var size = CalculateSize(steps) * SpriteRenderer.size.x / 100;
            SpriteRenderer.size = size;

            if (_part.rotate) {
                var angleStep = (_part.targetAngle - (float)_part.angle) / 100f;
                var startAngle = (float)_part.angle;
                var angle = steps * angleStep + startAngle;
                gameObject.transform.Rotate(Vector3.up, angle);
                //SpriteRenderer.angle = RendererParams.rotateWithCamera ? angle + Camera.angle[1] : angle;
            } else {
                gameObject.transform.rotation = Quaternion.Euler(-Camera.main.transform.eulerAngles.x, (float)_part.angle, Camera.main.transform.eulerAngles.z);
                //gameObject.transform.LookAt(Vector3.zero);
            }

            // todo
            // if (this.shadowTexture && 0)
        }

        private Vector2 CalculateSize(float steps) {
            float sizeX;
            float sizeY;
            if (_part.sizeSmooth) {
                if (_part.sizeEnd.x != _part.sizeStart.x) {
                    var csJ = steps * 0.09f + 1;
                    var csK = Mathf.Log10(csJ);
                    var csL = _part.sizeEnd.x - _part.sizeStart.x;
                    var csM = _part.sizeStart.x;
                    var csN = csK * csL + csM;
                    sizeX = csN;
                } else sizeX = _part.sizeStart.x;

                if (_part.sizeEnd.y != _part.sizeStart.y) {
                    var ctf = steps * 0.09f + 1;
                    var ctg = Mathf.Log10(ctf);
                    var cth = _part.sizeEnd.y - _part.sizeStart.y;
                    var cti = _part.sizeStart.y;
                    var ctj = ctg * cth + cti;
                    sizeY = ctj;
                } else sizeY = _part.sizeStart.y;
            } else {
                if (_part.sizeEnd.x != _part.sizeStart.x) {
                    var csL = (_part.sizeEnd.x - _part.sizeStart.x) / 100;
                    var csM = _part.sizeStart.x;
                    var csN = steps * csL + csM;
                    sizeX = csN;
                } else sizeX = _part.sizeStart.x;

                if (_part.sizeEnd.y != _part.sizeStart.y) {
                    var cth = (_part.sizeEnd.y - _part.sizeStart.y) / 100;
                    var cti = _part.sizeStart.y;
                    var ctj = steps * cth + cti;
                    sizeY = ctj;
                } else sizeY = _part.sizeStart.y;
            }

            return new Vector2(sizeX, sizeY);
        }

        private float CalculatePosition(float steps,
            float rotateAxis,
            bool isRotationClockwise,
            bool positionAxisSmooth,
            float positionStartAxis,
            float positionEndAxis) {
            float posDelta;
            if (rotateAxis > 0f) {
                posDelta = rotateAxis * Mathf.Cos(steps * 3.5f * _part.numberOfRotations * Mathf.PI / 180 -
                                                  _part.rotateLate * Mathf.PI / 2);
                if (isRotationClockwise)
                    posDelta = -1 * posDelta;
            } else {
                if (positionAxisSmooth) {
                    if (positionStartAxis != positionEndAxis) {
                        var csJ = steps * 0.09f + 1;
                        var csK = Mathf.Log10(csJ);
                        var csL = positionEndAxis - positionStartAxis;
                        var csM = positionStartAxis;
                        var csN = csK * csL + csM;
                        posDelta = csN;
                    } else posDelta = positionStartAxis;
                } else {
                    if (positionStartAxis != positionEndAxis) {
                        var csL = (positionEndAxis - positionStartAxis) / 100;
                        var csM = positionStartAxis;
                        var csN = steps * csL + csM;
                        posDelta = csN;
                    } else posDelta = positionStartAxis;
                }
            }

            return posDelta;
        }

        private static float RandBetween(float minimum, float maximum) {
            return float.Parse(Mathf.Min(minimum + Random.Range(0, 1f) * (maximum - minimum), maximum).ToString("n3"));
        }
    }
}