using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CustomButton))]
public class CustomButtonEditor : UnityEditor.UI.ButtonEditor {

    public override void OnInspectorGUI() {

        var component = (CustomButton)target;
        base.OnInspectorGUI();

        component.backgroundImage = EditorGUILayout.TextField("Background Image", component.backgroundImage);
        component.hoverImage = EditorGUILayout.TextField("Hover Image", component.hoverImage);
        component.pressedImage = EditorGUILayout.TextField("Pressed Image", component.pressedImage);
    }
}
#endif