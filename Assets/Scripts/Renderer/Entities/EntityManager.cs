using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityManager : MonoBehaviour {

    private Dictionary<uint, Entity> entityCache = new Dictionary<uint, Entity>();

    public Entity Spawn(EntityData data) {
        switch (data.type) {
            case EntityType.PC:
                entityCache.TryGetValue(data.GID, out var pc);
                pc?.gameObject.SetActive(true);
                return pc ?? SpawnPC(data);
            case EntityType.NPC:
                entityCache.TryGetValue(data.id, out var npc);
                npc?.gameObject.SetActive(true);
                return npc ?? SpawnNPC(data);
            default:
                return null;
        }
    }

    public void HideEntity(uint GID, EntityType type) {
        entityCache.TryGetValue(GID, out var entity);
        entity?.gameObject.SetActive(false);
    }

    private Entity SpawnPC(EntityData data) {
        var player = new GameObject(data.name);
        player.layer = LayerMask.NameToLayer("Characters");
        player.transform.localScale = Vector3.one;
        var entity = player.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Characters");
        body.transform.SetParent(player.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0, 0.5f);
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
        entity.ShadowSize = 0.5f;
        // Add more options such as sex etc

        bodyViewer._ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        headViewer.Parent = bodyViewer;
        headViewer.Entity = entity;
        headViewer.SpriteOrder = 1;
        headViewer.Type = entity.Type;
        headViewer._ViewerType = ViewerType.HEAD;

        entityCache.Add(data.GID, entity);

        return entity;
    }

    private Entity SpawnNPC(EntityData data) {
        var npc = new GameObject(data.name);
        npc.layer = LayerMask.NameToLayer("Characters");
        npc.transform.localScale = new Vector3(1f, 1f, 1f);
        var entity = npc.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Characters");
        body.transform.SetParent(npc.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0, 0.5f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var bodyViewer = body.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.SetReady(true);

        bodyViewer._ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        entityCache.Add(data.id, entity);

        return entity;
    }

    public Entity SpawnPlayer(CharacterData data) {
        var player = new GameObject(data.Name);
        player.layer = LayerMask.NameToLayer("Characters");
        player.transform.localScale = Vector3.one;
        var entity = player.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Characters");
        body.transform.SetParent(player.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0, 0.5f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var head = new GameObject("Head");
        head.layer = LayerMask.NameToLayer("Characters");
        head.transform.SetParent(body.transform, false);
        head.transform.localPosition = Vector3.zero;

        var bodyViewer = body.AddComponent<EntityViewer>();
        var headViewer = head.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.ShadowSize = 0.5f;
        // Add more options such as sex etc

        bodyViewer._ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        headViewer.Parent = bodyViewer;
        headViewer.Entity = entity;
        headViewer.SpriteOrder = 1;
        headViewer.Type = entity.Type;
        headViewer._ViewerType = ViewerType.HEAD;

        return entity;
    }
}
