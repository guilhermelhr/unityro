using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    [SerializeField] private Entity Entity;
    [SerializeField] private Light worldLight;

    private void Awake() {
        var mapInfo = Core.NetworkClient.State.MapLoginInfo;
        if(mapInfo == null) {
            throw new Exception("Map Login info cannot be null");
        }

        Entity.transform.position = new Vector3(mapInfo.PosX, 1, mapInfo.PosY);

        Core.Instance.InitCamera();
        Core.Instance.SetWorldLight(worldLight);
        Core.Instance.BeginMapLoading(mapInfo.mapname);
    }


    // Start is called before the first frame update
    void Start() {
        new CZ.NOTIFY_ACTORINIT().Send();
    }

    // Update is called once per frame
    void Update() {
        Core.NetworkClient.Ping();
    }
}
