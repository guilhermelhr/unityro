using System.Collections.Generic;
using System.Linq;
using ROIO.Models.FileTypes;
using UnityEngine;

namespace Core.Effects {
    public class EffectTester : MonoBehaviour {
        private List<STR> Effects;

        private StrEffectRenderer EffectRenderer;

        private int CurrentEffect = 0;

        private void Start() {
            Effects = Resources.LoadAll<STR>("Effects/STR").ToList();

            EffectRenderer = gameObject.AddComponent<StrEffectRenderer>();

            EffectRenderer.Initialize(Effects[0]);

            EffectRenderer.OnEnd += delegate {
                var effect = Effects[CurrentEffect++];
                Debug.Log($"Testing effect {effect.name}");
                EffectRenderer.Initialize(effect);
            };
        }
    }
}