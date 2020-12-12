using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    [SerializeField] private Light worldLight;

    private void Awake() {
        var mapInfo = Core.NetworkClient.State.MapLoginInfo;
        if(mapInfo == null) {
            throw new Exception("Map Login info cannot be null");
        }

        Core.Instance.InitCamera();
        Core.Instance.SetWorldLight(worldLight);
        Core.Instance.BeginMapLoading(mapInfo.mapname);

        var entity = Core.EntityFactory.SpawnPlayer(Core.NetworkClient.State.SelectedCharacter);
        Core.Session = new Session(entity);
        Core.Session.Entity.transform.position = new Vector3(mapInfo.PosX, 2f, mapInfo.PosY);

        /**
        * Hack
        */
        Core.MainCamera.GetComponent<ROCamera>().SetTarget(Core.Session.Entity.EntityViewer.transform);
        Core.MainCamera.transform.SetParent(Core.Session.Entity.transform);

        Core.Session.Entity.SetReady(true);
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
