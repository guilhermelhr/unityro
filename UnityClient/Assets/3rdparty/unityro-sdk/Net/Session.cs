using System;

public class Session {

    public const int PC_ENTITY_TYPE = 3;

    public static Action<string> OnMapChanged;

    public int AccountID;
    public INetworkEntity Entity { get; private set; }
    public string CurrentMap { get; private set; }

    public Session(INetworkEntity entity, int accountID) {
        if (entity.GetEntityType() != PC_ENTITY_TYPE) {
            throw new ArgumentException("Cannot start session with non player entity");
        }

        AccountID = accountID;
        this.Entity = entity;
    }

    public void SetCurrentMap(string mapname) {
        CurrentMap = mapname;
        OnMapChanged?.Invoke(mapname);
    }
}