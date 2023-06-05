using Core.Effects;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectRenderer))]
internal class EffectRendererEditor : Editor {
    public override void OnInspectorGUI() {
        var component = (EffectRenderer) target;
        base.OnInspectorGUI();

        if(GUILayout.Button("Replay Effect")) {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            component.InitEffects();
        }
    }
}
