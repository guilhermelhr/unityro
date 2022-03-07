using System;
using UnityEngine;

public class GameEventUI : SingletonMonoBehavior<GameEventUI>
{
    public static Action EventUpdateCurrentMiniMap;
    public static Action EventUpdateCoordinateMiniMap;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        EventUpdateCurrentMiniMap?.Invoke();
    }

    private void Update()
    {
        EventUpdateCoordinateMiniMap?.Invoke();
    }
}