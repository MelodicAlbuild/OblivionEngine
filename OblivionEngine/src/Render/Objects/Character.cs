using System.Numerics;
using OblivionEngine.Core;
using OblivionEngine.Core.Events;
using OblivionEngine.Core.Extras;
using OblivionEngine.GameSystems.Anim;
using SDL2;

namespace OblivionEngine.Render.Objects;

public class Character : Sprite
{
    private Direction _aD;
    private Dictionary<Direction, List<Vector4>> _animations;
    private int maxFrames = 4;
    private const int ANIMATION_SPEED = 150;
    private int currentAnimationSpeed;
    private int _currentFrame = 0;
    private uint startingTicks = 0;
    
    public Character(OblivionRenderer renderer, Animation animation)
    {
        _animations = new Dictionary<Direction, List<Vector4>>();

        currentAnimationSpeed = ANIMATION_SPEED;

        OblivionEventManager.Instance.OnUpdate += OnUpdate;

        OblivionEventManager.Instance.OnDraw += OnDraw;

        _width = animation.width;
        _height = animation.height;

        _texture = SDL_image.IMG_LoadTexture(renderer.GetRenderer(), animation.texturePath);

        maxFrames = animation.maxFrames;

        for (int i = 0; i < animation.rows; i++)
        {
            List<Vector4> anim = new();
            for (int j = 0; j < animation.columns; j++)
            {
                anim.Add(new Vector4(j * animation.width, i * animation.height, animation.width, animation.height));
            }

            switch (animation.rowDefs[i])
            {
                case "S":
                    _animations.Add(Direction.Down, anim);
                    break;
                case "W":
                    _animations.Add(Direction.Left, anim);
                    break;
                case "E":
                    _animations.Add(Direction.Right, anim);
                    break;
                case "N":
                    _animations.Add(Direction.Up, anim);
                    break;
            }
        }
    }
    
    private void OnUpdate(object? sender, EventArgs? e)
    {
        if (!Game.Instance.GetTilemap().ContainsCollidableTile(new Vector2(GetTileX(), GetTileY())))
        {
            Game.Instance.GetTilemap().AddCollidableTile(new Vector2(GetTileX(), GetTileY()));
        }
    }
    
    private void OnDraw(object? _, OblivionRenderer renderer)
    {
        DrawAnimated(renderer);
    }
    
    private void DrawAnimated(OblivionRenderer renderer)
    {
        Game g = Game.Instance;

        if (_animations.Count == 0) return;

        SDL.SDL_Rect srect = new SDL.SDL_Rect()
        {
            x = (int)_animations[_aD][_currentFrame].X,
            y = (int)_animations[_aD][_currentFrame].Y,
            h = (int)_animations[_aD][_currentFrame].W,
            w = (int)_animations[_aD][_currentFrame].Z
        };

        SDL.SDL_Rect drect = new SDL.SDL_Rect()
        {
            x = (int)Math.Floor(_position.X) - g.GetCameraX(),
            y = (int)Math.Floor(_position.Y) - 16 - g.GetCameraY(),
            h = (int)_animations[_aD][_currentFrame].W,
            w = (int)_animations[_aD][_currentFrame].Z
        };

        SDL.SDL_RenderCopy(renderer.GetRenderer(), _texture, ref srect, ref drect);
    }
    
    public void SetAnimDirection(Direction dir)
    {
        _aD = dir;
    }
}