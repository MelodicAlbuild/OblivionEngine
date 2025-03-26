using System.Drawing;
using System.Drawing.Text;
using SDL2;

namespace OblivionEngine.Render.Font;

public class OblivionFontManager
{
    public static OblivionFontManager _instance;

    public IntPtr Font;
    public IntPtr DiagFont;

    public int FontPtSize = 24;
    public int DiagFontPtSize = 56;

    public OblivionFontManager(string path)
    {
        _instance = this;
        
        Font = SDL_ttf.TTF_OpenFont(path, 24);
        DiagFont = SDL_ttf.TTF_OpenFont(path, 56);

        if (SDL.SDL_GetError() != "")
        {
            throw new Exception("Failed to open font: " + SDL.SDL_GetError());
        }
    }
}