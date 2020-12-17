using UnityEngine;
using System.Collections;
using System;

public class EntityControl : MonoBehaviour {

    public Entity Entity;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            ProcessClickAction();
        }
    }

    private void ProcessClickAction() {
        var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 150, LayerMask.GetMask("NPC", "Monsters"))) {
            hit.collider.gameObject.TryGetComponent<EntityViewer>(out var target);

            if (target != null) {
                switch (target.Entity.Type) {
                    case EntityType.NPC:
                        new CZ.CONTACTNPC() {
                            NAID = target.Entity.GID,
                            Type = 1
                        }.Send();
                        break;
                    case EntityType.ITEM:
                        break;
                    case EntityType.MOB:
                        break;
                    case EntityType.WARP:
                        break;
                }
            }
        }
    }
}
