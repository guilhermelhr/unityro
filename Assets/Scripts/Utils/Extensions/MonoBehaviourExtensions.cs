using System;
using UnityEngine;

public static class MonoBehaviourExtensions {

    public static void ToggleActive(this MonoBehaviour mono) {
        mono.gameObject.ToggleActive();
    }
}
