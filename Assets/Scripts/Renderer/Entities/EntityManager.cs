﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityManager : MonoBehaviour {

    private Dictionary<uint, Entity> entityCache = new Dictionary<uint, Entity>();

    public Entity Spawn(EntityData data) {
        switch (data.type) {
            case EntityType.PC:
                entityCache.TryGetValue(data.id, out var pc);
                pc?.gameObject.SetActive(true);
                return pc ?? SpawnPC(data);
            case EntityType.NPC:
                entityCache.TryGetValue(data.id, out var npc);
                npc?.gameObject.SetActive(true);
                return npc ?? SpawnNPC(data);
            case EntityType.MOB:
                entityCache.TryGetValue(data.id, out var mob);
                mob?.gameObject.SetActive(true);
                return mob ?? SpawnMOB(data);
            default:
                return null;
        }
    }

    public Entity GetEntity(uint GID) {
        entityCache.TryGetValue(GID, out var entity);
        return entity;
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
        body.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
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
        entity.ShadowSize = 1f;
        // Add more options such as sex etc

        bodyViewer._ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;
        bodyViewer.Children.Add(headViewer);

        headViewer.Parent = bodyViewer;
        headViewer.Entity = entity;
        headViewer.SpriteOrder = 1;
        headViewer.Type = entity.Type;
        headViewer._ViewerType = ViewerType.HEAD;

        entityCache.Add(data.id, entity);
        entity.GID = data.id;
        entity.SetReady(true);

        return entity;
    }

    private Entity SpawnNPC(EntityData data) {
        var npc = new GameObject(data.name);
        npc.layer = LayerMask.NameToLayer("NPC");
        npc.transform.localScale = new Vector3(1f, 1f, 1f);
        var entity = npc.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("NPC");
        body.transform.SetParent(npc.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var bodyViewer = body.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.SetReady(true);
        entity.ShadowSize = 1f;

        bodyViewer._ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        entityCache.Add(data.id, entity);
        entity.GID = data.id;
        entity.SetReady(true);

        return entity;
    }

    private Entity SpawnMOB(EntityData data) {
        var mob = new GameObject(data.name);
        mob.layer = LayerMask.NameToLayer("Monsters");
        mob.transform.localScale = new Vector3(1f, 1f, 1f);
        var entity = mob.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Monsters");
        body.transform.SetParent(mob.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var bodyViewer = body.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.SetReady(true);
        entity.ShadowSize = 1f;

        bodyViewer._ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        entityCache.Add(data.id, entity);
        entity.GID = data.id;
        entity.SetReady(true);

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
        body.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var head = new GameObject("Head");
        head.layer = LayerMask.NameToLayer("Characters");
        head.transform.SetParent(body.transform, false);
        head.transform.localPosition = Vector3.zero;

        var bodyViewer = body.AddComponent<EntityViewer>();
        var headViewer = head.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.ShadowSize = 1f;
        entity.GID = (uint)data.GID;
        // Add more options such as sex etc

        bodyViewer._ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;
        bodyViewer.Children.Add(headViewer);

        headViewer.Parent = bodyViewer;
        headViewer.Entity = entity;
        headViewer.SpriteOrder = 1;
        headViewer.Type = entity.Type;
        headViewer._ViewerType = ViewerType.HEAD;

        if (data.Weapon != 0) {

        }

        var controller = player.AddComponent<EntityControl>();
        controller.Entity = entity;

        return entity;
    }
}
