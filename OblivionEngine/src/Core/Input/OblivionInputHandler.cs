using OblivionEngine.Core.Extras;
using OblivionEngine.Render.Objects;
using SDL2;

namespace OblivionEngine.Core.Input;

public class OblivionInputHandler
{
    private Dictionary<SDL.SDL_Keycode, Actions> _keyBindings;
    private Dictionary<int, int> _keys;
    private Sprite _player;

    public OblivionInputHandler(Sprite player)
    {
        _player = player;
        _keyBindings = new Dictionary<SDL.SDL_Keycode, Actions>
        {
            { SDL.SDL_Keycode.SDLK_LEFT, Actions.MoveLeft },
            { SDL.SDL_Keycode.SDLK_RIGHT, Actions.MoveRight },
            { SDL.SDL_Keycode.SDLK_UP, Actions.MoveUp },
            { SDL.SDL_Keycode.SDLK_DOWN, Actions.MoveDown },
            { SDL.SDL_Keycode.SDLK_x, Actions.Sprint },
            { SDL.SDL_Keycode.SDLK_p, Actions.ShowDiag }
            // Add other keybindings as needed
        };
        _keys = new Dictionary<int, int>();
    }

    public void OnKeyDown(SDL.SDL_Event key)
    {
        _keys[(int)key.key.keysym.sym] = 1;

        if (key.key.keysym.sym == SDL.SDL_Keycode.SDLK_p)
        {
            Game.Instance.ToggleDialogBox();
            Game.Instance.SendDialog("This is a test dialog box. Hopefully it doesn't break.");
        }
    }

    public void OnKeyUp(SDL.SDL_Event key)
    {
        _keys[(int)key.key.keysym.sym] = 0;
    }

    public void HandleInput()
    {
        if (Game.Instance.IsTransitioning()) return;
        
        bool isSprinting = _keys.ContainsKey((int)SDL.SDL_Keycode.SDLK_x) && _keys[(int)SDL.SDL_Keycode.SDLK_x] == 1;
        _player.SetSprint(isSprinting);

        bool anyKeyPressed = false;
        foreach (var keyBinding in _keyBindings)
        {
            if (_keys.ContainsKey((int)keyBinding.Key) && _keys[(int)keyBinding.Key] == 1)
            {
                anyKeyPressed = true;
                PerformAction(keyBinding.Value);
            }
        }

        if (!anyKeyPressed)
        {
            _player.NoKeysPressed();
        }
    }

    private void PerformAction(Actions action)
    {
        switch (action)
        {
            case Actions.MoveLeft:
                _player.StartMovement(Direction.Left);
                break;
            case Actions.MoveRight:
                _player.StartMovement(Direction.Right);
                break;
            case Actions.MoveUp:
                _player.StartMovement(Direction.Up);
                break;
            case Actions.MoveDown:
                _player.StartMovement(Direction.Down);
                break;
            case Actions.Sprint:
                // Sprinting is already handled in HandleInput
                break;
            case Actions.ShowDiag:
                
                break;
            // Handle other actions as needed
        }
    }
}