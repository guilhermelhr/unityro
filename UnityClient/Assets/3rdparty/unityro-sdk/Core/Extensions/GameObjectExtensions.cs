using UnityEngine;

namespace UnityRO.Core.Extensions {
    public static class GameObjectExtensions {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component {
            var obj = go.GetComponent<T>();
            if (obj != null)
                return obj;
            return go.AddComponent<T>();
        }

        public static void ChangeStaticRecursive(this GameObject go, bool isStatic) {
            go.layer = isStatic ? LayerMask.NameToLayer("Object") : LayerMask.NameToLayer("DynamicObject");
            go.isStatic = isStatic;
            foreach (Transform t in go.transform) {
                ChangeStaticRecursive(t.gameObject, isStatic);
            }
        }
    }
}