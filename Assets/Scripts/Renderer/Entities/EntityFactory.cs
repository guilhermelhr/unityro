using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityFactory : MonoBehaviour {

    public Entity SpawnPlayer() {

        var go = new GameObject("Player");
        go.layer = LayerMask.NameToLayer("Characters");
        go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        var control = go.AddComponent<Entity>();

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Characters");
        body.transform.SetParent(go.transform, false);
        body.transform.localPosition = Vector3.zero;
        body.AddComponent<SortingGroup>();
        var bodySprite = body.AddComponent<SPRRenderer>();

        var path = DBManager.GetBodyPath(0, 0);
        SPR bodySpr = FileManager.Load(path + ".spr") as SPR;
        bodySprite.setSPR(bodySpr, 0, 0);

        var head = new GameObject("Head");
        head.layer = LayerMask.NameToLayer("Characters");
        head.transform.SetParent(body.transform, false);
        head.transform.localPosition = Vector3.zero;
        var headSprite = head.AddComponent<SPRRenderer>();

        path = DBManager.GetHeadPath(0, 0);
        SPR headSpr = FileManager.Load(path + ".spr") as SPR;
        headSprite.setSPR(headSpr, 0, 0);

        Core.MainCamera.GetComponent<ROCamera>().SetTarget(control.transform);
        //Core.MainCamera.transform.SetParent(control.transform);
        go.AddComponent<Billboard>();

        return control;
    }
}
