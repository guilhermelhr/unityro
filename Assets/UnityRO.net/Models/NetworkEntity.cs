public interface INetworkEntity
{
    EntityType GetEntityType();
    uint GetEntityGID();
    EntityBaseStatus GetBaseStatus();
    void UpdateSprites();
}
