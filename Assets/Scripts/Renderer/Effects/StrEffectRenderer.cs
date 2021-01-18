using System;
using System.Collections.Generic;
using UnityEngine;

public class StrEffectRenderer : MonoBehaviour {

    private Material material;
    private STR _effect;
    private long _startTick;

    private List<StrLayerRenderer> layers = new List<StrLayerRenderer>();

    private void Start() {
        material = (Material)Resources.Load("SpriteMaterial", typeof(Material));
    }

    public void SetAnimation(STR effect) {
        _effect = effect ?? throw new Exception("STR cannot be null");
        _startTick = Core.Tick;

        for(int i = 0; i < _effect.layers.Length; i++) {
            var layer = _effect.layers[i];

            var l = new GameObject($"Layer_{i}").AddComponent<StrLayerRenderer>();
            l.transform.SetParent(gameObject.transform);
            l.SetLayer(layer);
            layers.Add(l);
        }
    }

    private void LateUpdate() {
        if(_effect == null) return;
        Render();
    }

    private void Render() {
        long keyIndex = (long)((Core.Tick - _startTick) / 1000f * _effect.fps);
        var endedLayers = new List<StrLayerRenderer>();

        foreach(var layer in layers) {
            layer.UpdateLayer(keyIndex);

            if(layer.GetMaxFrame() == keyIndex) {
                endedLayers.Add(layer);
            }
        }

        endedLayers.ForEach(t => {
            Destroy(t.gameObject);
            layers.Remove(t);
        });

        if(keyIndex == _effect.maxKey) {
            Destroy(this.gameObject);
            //SetAnimation(_effect);
        }
    }

    struct Anim {
        public int aniframe;
        public float frame, type, anitype, srcalpha, destalpha, mtpreset;
        public float delay, angle;
        public Color color;
        public Vector2 pos;
        public float[] uv, xy;
    }

    class StrLayerRenderer : MonoBehaviour {

        private STR.Layer _layer;

        private float _lastAngle;

        private Material material;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        private void Start() {
            meshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
            meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
            material = (Material)Resources.Load("EffectMaterial", typeof(Material));
        }

        public void SetLayer(STR.Layer layer) {
            _layer = layer;
        }

        public void UpdateLayer(long keyIndex) {
            if(CalculateAnimation(_layer, keyIndex, out var animation)) {
                var texture = _layer.textures[animation.aniframe | 0];
                if(texture != null) {
                    RenderAnimation(texture, animation);
                }
            }
        }

        private void RenderAnimation(Texture2D texture, Anim animation) {
            var mesh = new Mesh();

            var points = new Vector2[] {
                new Vector3(animation.xy[0], animation.xy[4]),
                new Vector3(animation.xy[1], animation.xy[5]),
                new Vector3(animation.xy[3], animation.xy[7]),
                new Vector3(animation.xy[2], animation.xy[6])
            };

            mesh.vertices = new Vector3[] { points[0], points[1], points[2], points[3] };

            mesh.triangles = new int[] {
                0, 2, 1,
                2, 3, 1
            };

            var u = animation.uv[0];
            var v = animation.uv[1];
            var us = animation.uv[2];
            var vs = animation.uv[3];
            mesh.uv = new Vector2[] {
                new Vector2(u, v),
                new Vector2(us, v),
                new Vector2(u, vs),
                new Vector2(us, vs)
            };

            mesh.RecalculateNormals();

            if(animation.angle != _lastAngle) {
                //rotate
                var rotation = transform.localEulerAngles;
                rotation.x = (float)(-animation.angle / 180 * Math.PI);
                _lastAngle = animation.angle;
            }

            animation.pos.x -= 320;
            animation.pos.y -= 290;

            var position = transform.position;
            position.x += animation.pos.x / SPR.PIXELS_PER_UNIT;
            position.y -= animation.pos.y / SPR.PIXELS_PER_UNIT + 0.5f;

            transform.position = position;

            meshFilter.mesh = mesh;
            meshRenderer.material = material;
            meshRenderer.material.mainTexture = texture;
            meshRenderer.material.color = animation.color;
        }

        private bool CalculateAnimation(STR.Layer layer, long keyIndex, out Anim result) {
            var animations = layer.animations;
            var lastFrame = 0;
            var lastSource = 0;
            var fromId = -1;
            var toId = -1;
            STR.Animation from, to;

            result = new Anim() {
                frame = 0,
                type = 0,
                aniframe = -1,
                anitype = 0,
                srcalpha = 1,
                destalpha = 1,
                mtpreset = 0,
                delay = 0f,
                angle = 0f,
                color = new Color(),
                pos = Vector2.zero,
                uv = new float[8],
                xy = new float[8]
            };

            for(int i = 0; i < layer.animations.Length; i++) {
                if(animations[i].frame <= keyIndex) {
                    if(animations[i].type == 0) fromId = i;
                    if(animations[i].type == 1) toId = i;
                }
                lastFrame = Math.Max(lastFrame, animations[i].frame);

                if(animations[i].type == 0) {
                    lastSource = Math.Max(lastSource, animations[i].frame);
                }
            }

            // Nothing to render
            if(fromId < 0 || (toId < 0 && lastFrame < keyIndex)) {
                return false;
            }

            if(toId < 0) {
                if(fromId < layer.animations.Length - 1) {
                    toId = fromId++;
                } else {
                    return false;
                }
            }

            from = animations[fromId];
            to = animations[toId];
            var delta = keyIndex - from.frame;
            result.srcalpha = (int)from.srcAlpha;
            result.destalpha = (int)from.destAlpha;

            // Static frame (or frame that can't be updated)
            if(toId != fromId + 1 || to.frame != from.frame) {
                // No other source
                if(to != null && lastSource <= from.frame) {
                    return false;
                }

                result.angle = from.angle;
                result.aniframe = (int)from.animFrame;

                result.color = from.color;
                result.pos = from.position;
                result.uv = from.uv;
                result.xy = from.xy;
                result.color.a = result.destalpha / 100;

                return true;
            }

            // Morph animation
            result.color[0] = from.color[0] + to.color[0] * delta;
            result.color[1] = from.color[1] + to.color[1] * delta;
            result.color[2] = from.color[2] + to.color[2] * delta;
            result.color[3] = from.color[3] + to.color[3] * delta;

            result.uv[0] = from.uv[0] + to.uv[0] * delta;
            result.uv[1] = from.uv[1] + to.uv[1] * delta;
            result.uv[2] = from.uv[2] + to.uv[2] * delta;
            result.uv[3] = from.uv[3] + to.uv[3] * delta;
            result.uv[4] = from.uv[4] + to.uv[4] * delta;
            result.uv[5] = from.uv[5] + to.uv[5] * delta;
            result.uv[6] = from.uv[6] + to.uv[6] * delta;
            result.uv[7] = from.uv[7] + to.uv[7] * delta;

            result.xy[0] = from.xy[0] + to.xy[0] * delta;
            result.xy[1] = from.xy[1] + to.xy[1] * delta;
            result.xy[2] = from.xy[2] + to.xy[2] * delta;
            result.xy[3] = from.xy[3] + to.xy[3] * delta;
            result.xy[4] = from.xy[4] + to.xy[4] * delta;
            result.xy[5] = from.xy[5] + to.xy[5] * delta;
            result.xy[6] = from.xy[6] + to.xy[6] * delta;
            result.xy[7] = from.xy[7] + to.xy[7] * delta;

            result.angle = from.angle + to.angle * delta;
            result.pos.x = from.position.x + to.position.x * delta;
            result.pos.y = from.position.y + to.position.y * delta;

            switch(to.animType) {
                default:
                    result.aniframe = 0;
                    break;

                case 1: //normal
                    result.aniframe = (int)(from.animFrame + to.animFrame * delta);
                    break;

                case 2: //stop at end
                    result.aniframe = (int)Math.Min(from.animFrame + to.delay * delta, layer.textures.Length - 1);
                    break;

                case 3: //repeat
                    result.aniframe = (int)((from.animFrame + to.delay * delta) % layer.textures.Length);
                    break;

                case 4: //play reverse infinity
                    result.aniframe = (int)((from.animFrame - to.delay * delta) % layer.textures.Length);
                    break;
            }

            return true;
        }

        public int GetMaxFrame() => _layer.animations.Length > 0 ? _layer.animations[_layer.animations.Length - 1].frame : 0;
    }
}