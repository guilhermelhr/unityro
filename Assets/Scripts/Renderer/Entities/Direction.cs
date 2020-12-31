/**
 * [1][8][7] 
 * [2][0][6] 
 * [3][4][5]
 *  South = 4,
    SouthWest = 3,
    West = 2,
    NorthWest = 1,
    North = 8,
    NorthEast = 7,
    East = 6,
    SouthEast = 5,
    None = 0
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

public static class DirectionExtensions {
    public static bool IsDiagonal(this Direction dir) {
        if (dir == Direction.NorthEast || dir == Direction.NorthWest ||
            dir == Direction.SouthEast || dir == Direction.SouthWest)
            return true;
        return false;
    }
}
