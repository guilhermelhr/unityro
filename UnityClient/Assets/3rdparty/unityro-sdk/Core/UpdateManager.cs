using System.Collections.Generic;
using UnityEngine;

namespace UnityRO.Core {
    public abstract class ManagedMonoBehaviour : MonoBehaviour {
        private void OnEnable() {
            UpdateManager.Add(this);
        }

        private void OnDisable() {
            UpdateManager.Remove(this);
        }

        public abstract void ManagedUpdate();
    }

    public static class UpdateManager {

        static HashSet<ManagedMonoBehaviour> _updateables = new();
        static UpdateManagerInnerMonoBehaviour _innerMonoBehaviour;

        static UpdateManager() {
            var gameObject = new GameObject();
            _innerMonoBehaviour = gameObject.AddComponent<UpdateManagerInnerMonoBehaviour>();

#if UNITY_EDITOR
            gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            _innerMonoBehaviour.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
#endif
        }

        public static void Add(ManagedMonoBehaviour managedMonoBehaviour) {
            _updateables.Add(managedMonoBehaviour);
        }

        public static void Remove(ManagedMonoBehaviour managedMonoBehaviour) {
            _updateables.Remove(managedMonoBehaviour);
        }

        class UpdateManagerInnerMonoBehaviour : MonoBehaviour {
            private void Update() {
                foreach (var mover in _updateables) {
                    mover.ManagedUpdate();
                }
            }
        }
    }
}