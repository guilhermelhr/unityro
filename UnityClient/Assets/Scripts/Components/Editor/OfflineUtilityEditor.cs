#if UNITY_EDITOR
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(OfflineUtility))]
[InitializeOnLoad]
public class OfflineUtilityEditor : Editor {

    public override async void OnInspectorGUI() {
        var component = (OfflineUtility) target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Load map")) {
            component.LoadMap();
        }

        if (GUILayout.Button("Create next map prefab")) {
            component.SelectNextMap();
            await component.LoadMap();
            await Task.Delay(2000);
            var map = SceneManager.GetActiveScene().GetRootGameObjects().ToList().Find(go => go.tag == "Map" && go.activeInHierarchy);
            MapsUtility.SaveMap(map, null);
        }
    }
}
#endif