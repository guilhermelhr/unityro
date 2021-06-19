﻿using System;

public class Session
{

    public static Session CurrentSession { get; private set; }

    public int AccountID;
    public NetworkEntity Entity { get; private set; }
    public string CurrentMap { get; private set; }

    public Session(NetworkEntity entity, int accountID)
    {
        if (entity.GetEntityType() != EntityType.PC)
        {
            throw new ArgumentException("Cannot start session with non player entity");
        }

        AccountID = accountID;
        this.Entity = entity;
    }

    public void SetCurrentMap(string mapname)
    {
        CurrentMap = mapname;
    }

    public static void StartSession(Session session)
    {
        CurrentSession = session;
    }
}