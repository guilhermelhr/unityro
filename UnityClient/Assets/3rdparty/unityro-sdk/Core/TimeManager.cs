using System;

namespace _3rdparty.unityro_core {
    public class TimeManager {
        
        public long Tick => serverTick + (currentTick - previousLocalTick) * 1000;

        private long serverTick;
        private long previousLocalTick;
        private long currentTick => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        
        public void SetServerTick(long tick) {
            previousLocalTick = currentTick;
            serverTick = tick;
        }
    }
}