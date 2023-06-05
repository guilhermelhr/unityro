using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static bool IsOffline = false;

    #region Time

    public static long Tick => (serverTick * 1000) + (currentTick - previousLocalTick);

    private static long serverTick;
    private static long previousLocalTick;
    private static long currentTick => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

    public void SetServerTick(long tick) {
        previousLocalTick = currentTick;
        serverTick = tick;
    }

    #endregion
}