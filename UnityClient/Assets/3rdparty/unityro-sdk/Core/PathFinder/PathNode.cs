namespace Core.Path {
    public class PathNode {
        public int X { get; set; }

        public int Y { get; set; }

        public PathNode? Parent { get; set; }

        public int Cost { get; set; }

        public int Total { get; set; }

        public PathStatus Type { get; set; }

        public int Direction { get; set; }

        public enum PathStatus {
            UNEXPLORED,
            OPEN,
            CLOSED
        };
    };
}