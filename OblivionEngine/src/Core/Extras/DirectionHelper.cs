namespace OblivionEngine.Core.Extras;

public enum Direction
{
    Left,
    Right,
    Up,
    Down,
    None
}

public class DirectionHelper
{
    public static Direction ComputeFromBooleans(bool left, bool right, bool up, bool down)
    {
        if (left && right && up && down)
        {
            return Direction.None;
        }
        
        if (left && !right)
        {
            return Direction.Left;
        }
        if (right && !left)
        {
            return Direction.Right;
        }
        if (up && !down)
        {
            return Direction.Up;
        }
        if (down && !up)
        {
            return Direction.Down;
        }

        return Direction.None;
    }
}