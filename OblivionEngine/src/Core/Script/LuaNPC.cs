using System.Numerics;
using Lua;

namespace OblivionEngine.Core.Script;

[LuaObject]
public partial class LuaNPC
{
    private Vector2 position;
    private int mapId;
    private int animId;
    private int facing;
    
    [LuaMember("x")]
    public float X
    {
        get => position.X;
        set => position = position with { X = value };
    }
    
    [LuaMember("y")]
    public float Y
    {
        get => position.Y;
        set => position = position with { Y = value };
    }
    
    [LuaMember("mapId")]
    public int MapId
    {
        get => mapId;
        set => mapId = value;
    }
    
    [LuaMember("animId")]
    public int AnimId
    {
        get => animId;
        set => animId = value;
    }
    
    [LuaMember("facing")]
    public int Facing
    {
        get => facing;
        set => facing = value;
    }
    
    [LuaMember("create")]
    public static LuaNPC Create(float x, float y, int mapId, int animationId, int facing)
    {
        return new LuaNPC()
        {
            mapId = mapId,
            position = new Vector2(x, y),
            animId = animationId,
            facing = facing
        };
    }
}