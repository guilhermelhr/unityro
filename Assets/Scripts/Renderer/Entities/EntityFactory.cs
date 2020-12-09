using UnityEngine;
using UnityEngine.Rendering;

public class EntityFactory : MonoBehaviour {

    public Entity SpawnPlayer(CharacterData data) {

        var player = new GameObject("Player");
        player.layer = LayerMask.NameToLayer("Characters");
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        var entity = player.AddComponent<Entity>();

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Characters");
        body.transform.SetParent(player.transform, false);
        body.transform.localPosition = Vector3.zero;
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var head = new GameObject("Head");
        head.layer = LayerMask.NameToLayer("Characters");
        head.transform.SetParent(body.transform, false);
        head.transform.localPosition = Vector3.zero;

        var bodyViewer = body.AddComponent<EntityViewer>();
        var headViewer = head.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.Type = EntityType.PC;
        entity.ActionTable = new SpriteAction.PC() as SpriteAction;
        entity.Data = data;
        // Add more options such as sex etc

        bodyViewer.ViewerType = EntityViewer.Type.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.Children.Add(headViewer);
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.State = EntityState.IDLE;

        headViewer.Parent = bodyViewer;
        headViewer.SpriteOrder = 1;
        headViewer.ViewerType = EntityViewer.Type.HEAD;

        entity.ShadowSize = 0.5f;

        entity.Configure();

        /**
         * Hack
         */
        Core.MainCamera.GetComponent<ROCamera>().SetTarget(bodyViewer.transform);
        Core.MainCamera.transform.SetParent(entity.transform);

        return entity;
    }
}
