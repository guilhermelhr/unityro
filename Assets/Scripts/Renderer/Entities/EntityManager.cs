using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityManager : MonoBehaviour {

    private Dictionary<uint, Entity> entityCache = new Dictionary<uint, Entity>();

    public Entity Spawn(EntityData data) {
        switch (data.objecttype) {
            case EntityType.PC:
                entityCache.TryGetValue(data.GID, out var pc);
                pc?.gameObject.SetActive(true);
                return pc ?? SpawnPC(data);
            case EntityType.NPC:
                entityCache.TryGetValue(data.AID, out var npc);
                npc?.gameObject.SetActive(true);
                return npc ?? SpawnNPC(data);
            case EntityType.MOB:
                entityCache.TryGetValue(data.AID, out var mob);
                mob?.gameObject.SetActive(true);
                return mob ?? SpawnMOB(data);
            default:
                return null;
        }
    }

    public void RemoveEntity(uint GID) {
        entityCache.TryGetValue(GID, out Entity entity);
        if (entity != null) {
            Destroy(entity.gameObject);
            entityCache.Remove(GID);
        }
    }
    public Entity GetEntity(uint GID) {
        entityCache.TryGetValue(GID, out var entity);
        return entity;
    }

    //TODO this needs checking
    public void VanishEntity(uint AID, int type) {
        entityCache.TryGetValue(AID, out Entity entity);
        entity?.Vanish(type);
    }

    public Entity SpawnItem(ItemSpawnInfo itemSpawnInfo) {

        Item item = DBManager.GetItemInfo(itemSpawnInfo.AID);
        string itemPath = DBManager.GetItemPath(itemSpawnInfo.AID, itemSpawnInfo.IsIdentified);

        ACT act = FileManager.Load(itemPath + ".act") as ACT;
        SPR spr = FileManager.Load(itemPath + ".spr") as SPR;
        spr.SwitchToRGBA();

        var itemGO = new GameObject(item.identifiedDisplayName);
        itemGO.layer = LayerMask.NameToLayer("Items");
        itemGO.transform.localScale = Vector3.one;
        itemGO.transform.localPosition = itemSpawnInfo.Position;
        var entity = itemGO.AddComponent<Entity>();

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Items");
        body.transform.SetParent(itemGO.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0.4f, 0.5f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        if (itemSpawnInfo.animate)
            body.AddComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Animations/ItemDropAnimator")) as RuntimeAnimatorController;

        var bodyViewer = body.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.Type = EntityType.ITEM;
        entity.ShadowSize = 0.5f;

        bodyViewer.ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        entity.Init(spr, act);
        entity.AID = (uint)itemSpawnInfo.mapID;
        entityCache.Add(entity.AID, entity);
        entity.SetReady(true);

        return entity;
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
        body.transform.localPosition = new Vector3(0.5f, 0.4f, 0.5f);
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

        bodyViewer.ViewerType = ViewerType.BODY;
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
        headViewer.ViewerType = ViewerType.HEAD;

        entityCache.Add(data.AID, entity);
        entity.SetReady(true);

        return entity;
    }

    private Entity SpawnNPC(EntityData data) {
        var npc = new GameObject(data.name);
        npc.layer = LayerMask.NameToLayer("NPC");
        npc.transform.localScale = Vector3.one;
        var entity = npc.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("NPC");
        body.transform.SetParent(npc.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0.4f, 0.5f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var bodyViewer = body.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.SetReady(true);
        entity.ShadowSize = 1f;

        bodyViewer.ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        entityCache.Add(data.AID, entity);
        entity.SetReady(true);

        return entity;
    }

    private Entity SpawnMOB(EntityData data) {
        var mob = new GameObject(data.name);
        mob.layer = LayerMask.NameToLayer("Monsters");
        mob.transform.localScale = Vector3.one;
        var entity = mob.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Monsters");
        body.transform.SetParent(mob.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0.4f, 0.5f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var bodyViewer = body.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.SetReady(true);
        entity.ShadowSize = 1f;

        bodyViewer.ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        entityCache.Add(data.AID, entity);
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
        body.transform.localPosition = new Vector3(0.5f, 0.4f, 0.5f);
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

        bodyViewer.ViewerType = ViewerType.BODY;
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
        headViewer.ViewerType = ViewerType.HEAD;

        if (data.Weapon != 0) {
            var weapon = new GameObject("Weapon");
            weapon.layer = LayerMask.NameToLayer("Characters");
            weapon.transform.SetParent(body.transform, false);
            weapon.transform.localPosition = Vector3.zero;

            var weaponViewer = weapon.AddComponent<EntityViewer>();
            weaponViewer.Parent = bodyViewer;
            weaponViewer.SpriteOrder = 2;
            weaponViewer.Entity = entity;
            weaponViewer.ViewerType = ViewerType.WEAPON;

            bodyViewer.Children.Add(weaponViewer);
        }

        var controller = player.AddComponent<EntityControl>();
        controller.Entity = entity;

        return entity;
    }
}
