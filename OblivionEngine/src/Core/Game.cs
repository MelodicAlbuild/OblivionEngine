using System.Data;
using OblivionEngine.Core.Config;
using OblivionEngine.Core.Events;
using OblivionEngine.Core.Extras;
using OblivionEngine.Core.Input;
using OblivionEngine.Core.Resource;
using OblivionEngine.GameSystems;
using OblivionEngine.GameSystems.Anim;
using OblivionEngine.Render;
using OblivionEngine.Render.Dialog;
using OblivionEngine.Render.Font;
using OblivionEngine.Render.Objects;
using OblivionEngine.Render.Tilemaps;
using SDL2;
using Object = OblivionEngine.Render.Objects.Object;

namespace OblivionEngine.Core;

public class Game
{
    public static Game Instance { get; private set; }

    private int _frameS = 0;
    private bool _running = false;
    private IntPtr _window;
    private IntPtr _renderer;

    private bool _isTransitioning = false;

    private OblivionEventManager _eventManager;
    private OblivionRenderer _oblivionRenderer;
    private OblivionTilemap _oblivionTilemap;
    private OblivionFontManager _oblivionFontManager;
    private OblivionInputHandler _oblivionInputHandler;
    private OblivionResourceManager _oblivionResourceManager;
    
    private DialogBox _dialogBox;

    private Locations _locations;
    private Animations _animations;

    private Sprite _player;
    
    private int DISPLAY_WIDTH = 640;
    private int DISPLAY_HEIGHT = 360;
    private float UPDATE_RANGE = 1000.0f / 128.0f;
    
    private int _cameraX, _cameraY;

    private List<Object> _objects;

    public ILocation currentLocation;
    
    public const int TILE_SIZE = 32;
    
    public Game()
    {
        Instance = this;
        _objects = new List<Object>();
        _eventManager = new OblivionEventManager();
        _oblivionRenderer = new OblivionRenderer(IntPtr.Zero);
        _oblivionTilemap = new OblivionTilemap();
    }
    
    private Dictionary<SDL.SDL_Keycode, Direction> _keyBindings = new()
    {
        { SDL.SDL_Keycode.SDLK_LEFT, Direction.Left },
        { SDL.SDL_Keycode.SDLK_RIGHT, Direction.Right },
        { SDL.SDL_Keycode.SDLK_UP, Direction.Up },
        { SDL.SDL_Keycode.SDLK_DOWN, Direction.Down }
    };
    
    public void Start(string[] args)
    {
        SDL.SDL_WindowFlags flags = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN;
        if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) != 0)
        {
            return;
        }
        
        if(SDL.SDL_CreateWindowAndRenderer(DISPLAY_WIDTH, DISPLAY_HEIGHT, flags, out _window, out _renderer) != 0)
        {
            return;
        }

        _oblivionRenderer = new OblivionRenderer(_renderer);
        _locations = new Locations();

        _animations = new Animations();

        SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG);
        SDL_ttf.TTF_Init();
        
        _oblivionResourceManager = new OblivionResourceManager();
        _oblivionFontManager = new OblivionFontManager("resources/fonts/Pixel.ttf");

        _dialogBox = new DialogBox(_oblivionRenderer);
        
        _oblivionTilemap.LoadMap(_locations.allLocations.First(o => o.Key == 1).Value.mapPath);
        currentLocation = _locations.allLocations.First(o => o.Key == 1).Value;
        
        _running = true;
        Run();
    }

    private void CreateSprites()
    {
        Sprite player = new Sprite(true, _oblivionRenderer, true, _animations.animations[0], "");
        player.SetTilePosition(27, 9);
        _objects.Add(player);
        _player = player;
        
        Sprite npc = new Sprite(false, _oblivionRenderer, true, _animations.animations[1], "");
        npc.SetTilePosition(29, 9);
        npc.SetAnimDirection(Direction.Down);
        _objects.Add(npc);
    }
    
    private int alpha = 0;
    private int timerId = -1;

    private void Run()
    {
        uint past = SDL.SDL_GetTicks();
        uint now = past, pastFps = past;
        uint fps = 0, framesSkipped = 0;

        CreateSprites();

        _oblivionInputHandler = new OblivionInputHandler(_player);
        
        SDL.SDL_Event e;
        while (_running)
        {
            uint timeElapsed = 0;
            if (SDL.SDL_PollEvent(out e) != 0)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        OnQuit();
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        _oblivionInputHandler.OnKeyDown(e);
                        break;
                    case SDL.SDL_EventType.SDL_KEYUP:
                        _oblivionInputHandler.OnKeyUp(e);
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                    case SDL.SDL_EventType.SDL_MOUSEMOTION:
                        break;
                }
            }

            timeElapsed = (now = SDL.SDL_GetTicks()) - past;
            if (timeElapsed >= UPDATE_RANGE)
            {
                past = now;
                Update();
                if (framesSkipped++ > _frameS)
                {
                    Draw();
                    fps++;
                    framesSkipped = 0;
                }
            }

            if (now - pastFps >= 1000)
            {
                pastFps = now;
                FPSChanged(fps);
                fps = 0;
            }
            
            SDL.SDL_Delay(1);
        }
        
        Quit();
    }

    private void Draw()
    {
        if (_isTransitioning) return;
        
        SDL.SDL_RenderClear(_renderer);
        
        OblivionEventManager.Instance.InvokeDraw(this, _oblivionRenderer);
            
        OblivionEventManager.Instance.InvokeLateDraw(this, _oblivionRenderer);
        
        OblivionEventManager.Instance.InvokeUIDraw(this, _oblivionRenderer);
        
        //_oblivionRenderer.DrawGrid(TILE_SIZE, DISPLAY_HEIGHT, DISPLAY_WIDTH);
        
        SDL.SDL_RenderPresent(_renderer);
    }

    private void Update()
    {
        _oblivionInputHandler.HandleInput();
        OblivionEventManager.Instance.InvokeUpdate(this);

        _cameraX = (int)Math.Floor(_player.Position.X) - (DISPLAY_WIDTH / 2);
        _cameraY = (int)Math.Floor(_player.Position.Y) - (DISPLAY_HEIGHT / 2);
    }
    
    public int GetCameraX()
    {
        return _cameraX;
    }
    
    public int GetCameraY()
    {
        return _cameraY;
    }

    private void FPSChanged(uint fps)
    {
        string sFps = $"Oblivion Engine: {fps} FPS";
        SDL.SDL_SetWindowTitle(_window, sFps);
    }
    
    public void ToggleDialogBox()
    {
        _dialogBox.ToggleDialogBox();
    }
    
    public void SendDialog(string dialog)
    {
        _dialogBox.DisplayMessage(dialog);
    }

    public void ChangeLocation(Location loc)
    {
        
    }

    public void EnterBuilding(Building building)
    {
        _isTransitioning = true;
        _player.SetFreeze();
        _oblivionTilemap.LoadMap(building.mapPath);
        _player.SetTilePosition((int)(building.spawn.X), (int)(building.spawn.Y));
        _player.NoKeysPressed();
        currentLocation = building;
        _isTransitioning = false;
    }

    public void ExitBuilding(Building building)
    {
        _isTransitioning = true;
        _player.SetFreeze();
        _oblivionTilemap.LoadMap(building.parent.mapPath);
        _player.SetTilePosition((int)(building.exitPos.X), (int)(building.exitPos.Y));
        _player.NoKeysPressed();
        currentLocation = building.parent;
        _isTransitioning = false;
    }
    
    public bool IsTransitioning()
    {
        return _isTransitioning;
    }

    private void OnQuit()
    {
        _running = false;
    }

    private void Quit()
    {
        if (_renderer != IntPtr.Zero)
        {
            SDL.SDL_DestroyRenderer(_renderer);
            _renderer = IntPtr.Zero;
        }
        
        if (_window != IntPtr.Zero)
        {
            SDL.SDL_DestroyWindow(_window);
            _window = IntPtr.Zero;
        }
        
        SDL.SDL_Quit();
    }

    public IntPtr GetWindow()
    {
        return _window;
    }
    
    public OblivionTilemap GetTilemap()
    {
        return _oblivionTilemap;
    }

    public Sprite GetPlayer()
    {
        return _player;
    }
    
    public Animations GetAnimations()
    {
        return _animations;
    }
    
    public Locations GetLocations()
    {
        return _locations;
    }
}