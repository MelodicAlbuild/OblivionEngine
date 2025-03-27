using System.Numerics;
using OblivionEngine.Core;
using OblivionEngine.Core.Events;
using OblivionEngine.Core.Extras;
using OblivionEngine.GameSystems;
using OblivionEngine.GameSystems.Anim;
using OblivionEngine.Render.Tilemaps;
using SDL2;

namespace OblivionEngine.Render.Objects;

public class Sprite : Object
{
    internal Vector2 _position = new(0, 0);
    internal IntPtr _texture;
    internal int _width;
    internal int _height;

    public int Width => _width;
    public int Height => _height;

    public override Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public virtual void SetTilePosition(int x, int y)
    {
        _position.X = x * Game.TILE_SIZE;
        _position.Y = y * Game.TILE_SIZE;
    }

    public int GetTileX()
    {
        return (int)(_position.X / Game.TILE_SIZE);
    }

    public int GetTileY()
    {
        return (int)(_position.Y / Game.TILE_SIZE);
    }

    // private void OnDraw(object? _, OblivionRenderer renderer)
    // {
    //     SDL.SDL_Rect spriteRect = new SDL.SDL_Rect() { x = (int)_position.X, y = (int)_position.Y, h = 32, w = 32 };
    //     renderer.SetColorNoSave(255, 0, 0, 255);
    //     renderer.FillRect(spriteRect);
    //     renderer.ResetColor();
    // }
}