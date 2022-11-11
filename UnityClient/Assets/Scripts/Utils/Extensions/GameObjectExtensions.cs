using UnityEngine;

public static class GameObjectExtensions {

    public static T GetOrAddComponent<T>(this GameObject go) where T : Component {
        var obj = go.GetComponent<T>();
        if (obj != null)
            return obj;
        return go.AddComponent<T>();
    }

    public static void ToggleActive(this GameObject go) {
        go.SetActive(!go.activeInHierarchy);
    }
}
