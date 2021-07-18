/**
 * [1][8][7] 
 * [2][0][6] 
 * [3][4][5]
 */
public enum Direction : byte {
    South,
    SouthWest,
    West,
    NorthWest,
    North,
    NorthEast,
    East,
    SouthEast,
    None
}

public enum NpcDirection : byte {
    South = 4,
    SouthWest = 3,
    West = 2,
    NorthWest = 1,
    North = 8,
    NorthEast = 7,
    East = 6,
    SouthEast = 5,
    None = 0
}

public static class DirectionExtensions {
    public static bool IsDiagonal(this Direction dir) {
        if (dir == Direction.NorthEast || dir == Direction.NorthWest ||
            dir == Direction.SouthEast || dir == Direction.SouthWest)
            return true;
        return false;
    }

    public static Direction ToDirection(this NpcDirection dir) {
        switch(dir) {
            case NpcDirection.South:
                return Direction.South;
            case NpcDirection.SouthWest:
                return Direction.SouthWest;
            case NpcDirection.West:
                return Direction.West;
            case NpcDirection.NorthWest:
                return Direction.NorthWest;
            case NpcDirection.North:
                return Direction.North;
            case NpcDirection.NorthEast:
                return Direction.NorthEast;
            case NpcDirection.East:
                return Direction.East;
            case NpcDirection.SouthEast:
                return Direction.SouthEast;
            case NpcDirection.None:
            default:
                return Direction.South;
        }
    }
}
