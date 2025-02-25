using SDL2;

namespace OblivionEngine;

public class Game
{
    public static Game Instance { get; private set; }

    private int _frameS = 0;
    private bool _running = false;
    private IntPtr _window;
    private IntPtr _renderer;
    
    private int DISPLAY_WIDTH = 1280;
    private int DISPLAY_HEIGHT = 720;
    private float UPDATE_RANGE = 1000.0f / 128.0f;

    private Dictionary<int, int> keys;
    
    public Game()
    {
        Instance = this;
        keys = new Dictionary<int, int>();
    }
    
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
        
        _running = true;
        Run();
    }

    private void Run()
    {
        uint past = SDL.SDL_GetTicks();
        uint now = past, pastFps = past;
        uint fps = 0, framesSkipped = 0;

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
                        OnKeyDown(e);
                        break;
                    case SDL.SDL_EventType.SDL_KEYUP:
                        OnKeyUp(e);
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
                if (framesSkipped++ > _frameS)
                {
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
    }

    private void Draw()
    {
        SDL.SDL_RenderClear(_renderer);
        
        
        
        SDL.SDL_RenderPresent(_renderer);
    }

    private void FPSChanged(uint fps)
    {
        string sFps = $"Oblivion Engine: {fps} FPS";
        SDL.SDL_SetWindowTitle(_window, sFps);
    }

    private void OnQuit()
    {
        _running = false;
    }
    
    private void OnKeyDown(SDL.SDL_Event key)
    {
        keys[(int)key.key.keysym.sym] = 1;
    }
    
    private void OnKeyUp(SDL.SDL_Event key)
    {
        keys[(int)key.key.keysym.sym] = 0;
    }
}