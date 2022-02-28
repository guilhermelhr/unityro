using System;
using UnityEngine;

public class GameEventUI : SingletonDontDestroy<GameEventUI>
{
    public static event Action EventUpdateCurrentMiniMap;
    public static event Action EventUpdateCoordinateMiniMap;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        EventUpdateCurrentMiniMap?.Invoke();
        EventUpdateCoordinateMiniMap?.Invoke();
    }
}