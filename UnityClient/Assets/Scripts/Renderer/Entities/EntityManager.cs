using Assets.Scripts.Renderer.Sprite;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityRO.Core.GameEntity;
using static ZC.NOTIFY_VANISH;

public class EntityManager : MonoBehaviour {

    private GameObject EntityCanvasPrefab;
    private Dictionary<uint, Entity> entityCache = new Dictionary<uint, Entity>();

    private void Awake() {
        DontDestroyOnLoad(this);
        EntityCanvasPrefab = Resources.Load("Prefabs/UI/EntityCanvas") as GameObject;
    }

    public Entity Spawn(EntitySpawnData data) {
        switch ((EntityType)data.objecttype) {
            case EntityType.PC:
                entityCache.TryGetValue(data.AID, out var pc);
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

    public void RemoveEntity(uint AID) {
        entityCache.TryGetValue(AID, out Entity entity);
        if (entity != null) {
            Destroy(entity.gameObject);
            entityCache.Remove(AID);
        }
    }

    public Entity GetEntity(uint AID) {
        var hasFound = entityCache.TryGetValue(AID, out var entity);
        if (hasFound) {
            return entity;
        } else if (Session.CurrentSession.Entity.GetEntityGID() == AID || Session.CurrentSession.AccountID == AID) {
            return Session.CurrentSession.Entity as Entity;
        } else {
            Debug.LogError($"No Entity found for given ID: {AID}");
            return null;
        }
    }

    //TODO this needs checking
    public void VanishEntity(uint AID, VanishType type) {
        GetEntity(AID)?.Vanish(type);
        entityCache.Remove(AID);
    }

    public Entity SpawnItem(ItemSpawnInfo itemSpawnInfo) {

        Item item = DBManager.GetItem(itemSpawnInfo.AID);
        string itemPath = DBManager.GetItemPath(itemSpawnInfo.AID, itemSpawnInfo.IsIdentified);

        SpriteData spriteData = Addressables.LoadAssetAsync<SpriteData>($"{itemPath}.asset".SanitizeForAddressables()).WaitForCompletion();
        Texture2D atlas = Addressables.LoadAssetAsync<Texture2D>($"{itemPath}.png".SanitizeForAddressables()).WaitForCompletion();

        var itemGO = new GameObject(item.identifiedDisplayName);
        itemGO.layer = LayerMask.NameToLayer("Items");
        itemGO.transform.localScale = Vector3.one;
        itemGO.transform.localPosition = itemSpawnInfo.Position;
        var entity = itemGO.AddComponent<Entity>();

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Items");
        body.transform.SetParent(itemGO.transform, false);
        body.transform.localPosition = new Vector3(0f, 0.4f, 0f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        if (itemSpawnInfo.animate) {
            var animator = body.AddComponent<Animator>();
            animator.runtimeAnimatorController = Instantiate(Resources.Load("Animations/ItemDropAnimator")) as RuntimeAnimatorController;
        }

        var bodyViewer = body.AddComponent<SpriteEntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.Type = EntityType.ITEM;
        entity.ShadowSize = 0.5f;

        bodyViewer.ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.HeadDirection = 0;

        entity.Init(spriteData, atlas);
        entity.AID = (uint) itemSpawnInfo.mapID;
        entityCache.Add(entity.AID, entity);
        entity.SetReady(true);

        return entity;
    }

    private Entity SpawnPC(EntitySpawnData data) {
        var player = new GameObject(data.name);
        var canvas = Instantiate(EntityCanvasPrefab, player.transform).GetComponent<EntityCanvas>();
        var layer = LayerMask.NameToLayer("Characters");
        player.layer = layer;
        player.transform.localScale = Vector3.one;

        var entity = player.AddComponent<Entity>();
        entity.EntityViewerType = DBManager.GetEntityViewerType(data.job);
        entityCache.Add(data.AID, entity);
        entity.Init(data, layer, canvas);

        return entity;
    }

    private Entity SpawnNPC(EntitySpawnData data) {
        var npc = new GameObject(data.name);
        var canvas = Instantiate(EntityCanvasPrefab, npc.transform).GetComponent<EntityCanvas>();
        var layer = LayerMask.NameToLayer("NPC");
        npc.layer = layer;
        npc.transform.localScale = Vector3.one;
        var entity = npc.AddComponent<Entity>();
        entity.EntityViewerType = DBManager.GetEntityViewerType(data.job);

        entityCache.Add(data.AID, entity);
        entity.Init(data, layer, canvas);

        return entity;
    }

    private Entity SpawnMOB(EntitySpawnData data) {
        var mob = new GameObject(data.name);
        var canvas = Instantiate(EntityCanvasPrefab, mob.transform).GetComponent<EntityCanvas>();
        var layer = LayerMask.NameToLayer("Monsters");
        mob.layer = layer;
        mob.transform.localScale = Vector3.one;
        var entity = mob.AddComponent<Entity>();
        entity.EntityViewerType = DBManager.GetEntityViewerType(data.job);

        entityCache.Add(data.AID, entity);
        entity.Init(data, layer, canvas);

        return entity;
    }

    public Entity SpawnPlayer(CharacterData data) {
        var player = new GameObject(data.Name);
        var canvas = Instantiate(EntityCanvasPrefab, player.transform).GetComponent<EntityCanvas>();
        var layer = LayerMask.NameToLayer("Characters");
        player.layer = layer;
        player.transform.localScale = Vector3.one;
        var entity = player.AddComponent<Entity>();
        entity.EntityViewerType = DBManager.GetEntityViewerType(data.Job);
        entity.Init(data, layer, canvas);

        var controller = player.AddComponent<EntityControl>();
        controller.Entity = entity;

        return entity;
    }

    public void ClearEntities() {
        entityCache.Values.ToList().ForEach(it => GameObject.Destroy(it));
        entityCache.Clear();
    }
}
