using Core.Effects.EffectParts;
using UnityEngine;

namespace Core.Effects {
    
    public class EffectRenderer : MonoBehaviour {

        [SerializeField] private Effect Effect;
        [SerializeField] private bool autoStart = false;

        private void Start() {
            if (autoStart)
                InitEffects();
        }

        public void InitEffects() {
            if (Effect.CylinderParts.Length > 0) {
                for (int i = 0; i < Effect.CylinderParts.Length; i++) {
                    var param = Effect.CylinderParts[i];

                    var cylinderRenderer = new GameObject($"Cylinder{i}").AddComponent<CylinderEffectRenderer>();
                    cylinderRenderer.gameObject.layer = LayerMask.NameToLayer("Effects");
                    cylinderRenderer.transform.SetParent(transform, false);
                    cylinderRenderer.SetPart(param, param.delay);

                    for (int j = 1; j <= param.duplicates; j++) {
                        var cylinderJ = new GameObject($"Cylinder{i}-{j}").AddComponent<CylinderEffectRenderer>();
                        cylinderJ.gameObject.layer = LayerMask.NameToLayer("Effects");
                        cylinderJ.transform.SetParent(transform, false);
                        cylinderJ.SetPart(param, j * param.timeBetweenDuplication);
                    }
                }
            }

            if (Effect.ThreeDParts.Length > 0) {
                for (int i = 0; i < Effect.ThreeDParts.Length; i++) {
                    var param = Effect.ThreeDParts[i];

                    var threeDRenderer = new GameObject($"3D{i}").AddComponent<ThreeDEffectRenderer>();
                    threeDRenderer.gameObject.layer = LayerMask.NameToLayer("Effects");
                    //threeDRenderer.gameObject.GetOrAddComponent<Billboard>();
                    threeDRenderer.transform.SetParent(transform, false);
                    
                    var time = GameManager.Tick;
                    var instanceParam = new EffectInstanceParam {
                        position = transform.position,
                        otherPosition = transform.position + Vector3.left * 5,
                        startTick = time,
                        endTick = time + param.duration
                    };

                    var initParam = new EffectInitParam {
                        ownerAID = 0
                    };

                    threeDRenderer.Init(param, instanceParam, initParam);
                }
            }
        }
    }
}