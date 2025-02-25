using OblivionEngine.Render.Font;
using SDL2;

namespace OblivionEngine.Render;

public class OblivionRenderer
{
    private int _r, _g, _b, _a;
    private IntPtr _renderer;
    
    public OblivionRenderer(IntPtr renderer)
    {
        _r = 0;
        _b = 0;
        _g = 0;
        _a = 255;
        
        _renderer = renderer;

        SetColorNoSave(_r, _g, _b, _a);
    }

    public void DrawGrid(int tileSize, int displayHeight, int displayWidth)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 255);
        
        for (int y = 0; y < displayHeight; y += tileSize)
        {
            SDL.SDL_RenderDrawLine(_renderer, 0, y, displayWidth, y);
        }

        for (int x = 0; x < displayWidth; x += tileSize)
        {
            SDL.SDL_RenderDrawLine(_renderer, x, 0, x, displayHeight);
        }
        
        ResetColor();
    }
    
    public void ResetColor()
    {
        SDL.SDL_SetRenderDrawColor(_renderer, (byte)_r, (byte)_g, (byte)_b, (byte)_a);
    }

    public void SetColorAndSave(int r, int g, int b, int a)
    {
        _r = r;
        _g = g;
        _b = b;
        _a = a;
        SDL.SDL_SetRenderDrawColor(_renderer, (byte)_r, (byte)_g, (byte)_b, (byte)_a);
    }
    
    public void SetColorNoSave(int r, int g, int b, int a)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, (byte)r, (byte)g, (byte)b, (byte)a);
    }

    public void FillRect(SDL.SDL_Rect rect)
    {
        SDL.SDL_RenderFillRect(_renderer, ref rect);
    }

    public IntPtr GetRenderer()
    {
        return _renderer;
    }
}