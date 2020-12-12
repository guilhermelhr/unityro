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
        entity.Animation = new Animation() {
            action = entity.ActionTable.IDLE,
            delay = 100
        };
        // Add more options such as sex etc

        bodyViewer._ViewerType = EntityViewer.ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.Children.Add(headViewer);
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        headViewer.Parent = bodyViewer;
        headViewer.Entity = entity;
        headViewer.SpriteOrder = 1;
        headViewer.Type = entity.Type;
        headViewer._ViewerType = EntityViewer.ViewerType.HEAD;

        entity.ShadowSize = 0.5f;

        return entity;
    }
}
