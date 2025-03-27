using System.Numerics;
using OblivionEngine.Core;
using OblivionEngine.Core.Events;
using OblivionEngine.Core.Extras;
using OblivionEngine.GameSystems;
using OblivionEngine.GameSystems.Anim;
using OblivionEngine.Render.Tilemaps;
using SDL2;

namespace OblivionEngine.Render.Objects;

public class Player : Sprite
{
    private bool _isMoving = false;
    private Direction _d;
    private Direction _aD;
    private Dictionary<Direction, List<Vector4>> _animations;
    private int maxFrames = 4;
    private const float SPEED = 0.175f;
    private const int ANIMATION_SPEED = 150;
    private float currentSpeed;
    private int currentAnimationSpeed;
    private Vector2 _targetPosition;
    private Vector2 _startPosition;
    private float _moveProgress;
    private bool _isLerping;
    private const float LERP_SPEED = 10f;
    private int _currentFrame = 0;
    private uint startingTicks = 0;
    
    public Player(OblivionRenderer renderer, Animation animation)
    {
        _animations = new Dictionary<Direction, List<Vector4>>();

        currentSpeed = SPEED;
        currentAnimationSpeed = ANIMATION_SPEED;

        OblivionEventManager.Instance.OnUpdate += OnUpdate;

        OblivionEventManager.Instance.OnLateDraw += OnLateDraw;

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

    public override void SetTilePosition(int x, int y)
    {
        base.SetTilePosition(x, y);
        _isLerping = false;
        _isMoving = false;
    }

    private void OnLateDraw(object? _, OblivionRenderer renderer)
    {
        DrawAnimated(renderer);
    }
    
    private Vector2 Lerp(Vector2 start, Vector2 end, float t)
    {
        t = Math.Clamp(t, 0, 1);
        return new Vector2(
            start.X + (end.X - start.X) * t,
            start.Y + (end.Y - start.Y) * t
        );
    }

    public void StartMovement(Direction dir)
    {
        if (Game.Instance.IsTransitioning()) return;
        if (_isLerping) return;

        // Set the animation direction if not equal to None
        if (dir != Direction.None)
        {
            _aD = dir;
        }

        Vector2 currentTile = new Vector2(
            MathF.Round(_position.X / Game.TILE_SIZE),
            MathF.Round(_position.Y / Game.TILE_SIZE)
        );

        Vector2 targetTile = dir switch
        {
            Direction.Left  => new Vector2(currentTile.X - 1, currentTile.Y),
            Direction.Right => new Vector2(currentTile.X + 1, currentTile.Y),
            Direction.Up    => new Vector2(currentTile.X, currentTile.Y - 1),
            Direction.Down  => new Vector2(currentTile.X, currentTile.Y + 1),
            _               => currentTile
        };

        Vector2 targetWorldPos = new Vector2(
            targetTile.X * Game.TILE_SIZE,
            targetTile.Y * Game.TILE_SIZE
        );

        if (!IsCollisionAt(targetWorldPos))
        {
            _startPosition = _position;
            _targetPosition = targetWorldPos;
            _moveProgress = 0;
            _isLerping = true;
            _isMoving = true;
        }
    }
    
    private void UpdateAnimation()
    {
        if (startingTicks == 0)
        {
            startingTicks = SDL.SDL_GetTicks();
        }

        if (_isMoving)
        {
            if (SDL.SDL_GetTicks() - startingTicks > currentAnimationSpeed)
            {
                _currentFrame = (_currentFrame + 1) % maxFrames;
                startingTicks = SDL.SDL_GetTicks();
            }
        }
        else
        {
            _currentFrame = 0;
        }
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
    
    private bool IsCollisionAt(Vector2 futurePos)
    {
        Game game = Game.Instance;
        OblivionTilemap tmap = game.GetTilemap();
        List<Vector2> collidableTiles = tmap.GetCollidableTiles();
        List<Vector2> interactableTiles = tmap.GetInteractableTiles();

        Vector2 tilePos = new Vector2(
            MathF.Floor(futurePos.X / Game.TILE_SIZE),
            MathF.Floor(futurePos.Y / Game.TILE_SIZE)
        );

        if (game.currentLocation is Location loc)
        {
            if (loc.buildings.Any(b => b.connections.Contains(tilePos)))
            {
                game.EnterBuilding(loc.buildings.First(b => b.connections.Contains(tilePos)));
                return true;
            }
        }
        else if (game.currentLocation is Building building)
        {
            if (building.internalConnections.Contains(tilePos))
            {
                game.ExitBuilding(building);
                return true;
            }
        }

        return collidableTiles.Contains(tilePos) || interactableTiles.Contains(tilePos);
    }

    private void UpdateMovement()
    {
        if (_isLerping)
        {
            float delta = currentSpeed * LERP_SPEED * (1.0f / 60.0f);
            _moveProgress += delta;

            if (_moveProgress >= 1.0f)
            {
                _position = _targetPosition;
                _isLerping = false;
                _moveProgress = 0;
            }
            else
            {
                _position = Lerp(_startPosition, _targetPosition, _moveProgress);
            }
        }
    }

    public void SetSprint(bool sprint)
    {
        if (sprint)
        {
            currentSpeed = SPEED * 1.5f;
            currentAnimationSpeed = (int)Math.Floor(ANIMATION_SPEED / 1.25f);
        }
        else
        {
            currentSpeed = SPEED;
            currentAnimationSpeed = ANIMATION_SPEED;
        }
    }

    private void OnUpdate(object? sender, EventArgs? e)
    {
        UpdateMovement();
        UpdateAnimation();
    }
    
    public void SetFreeze()
    {
        _isMoving = false;
        _isLerping = false;
        _targetPosition = Vector2.Zero;
        _startPosition = Vector2.Zero;
        _moveProgress = 0;
    }
    
    public void NoKeysPressed()
    {
        if (!_isLerping)
        {
            _isMoving = false;
            _currentFrame = 0;
        }
    }
    
    public void SetAnimDirection(Direction dir)
    {
        _aD = dir;
    }
}