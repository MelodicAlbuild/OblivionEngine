using SDL2;

namespace OblivionEngine.Render.Font;

public class OblivionFontManager
{
    public static OblivionFontManager _instance;

    public IntPtr Font;

    public OblivionFontManager(string path)
    {
        _instance = this;
        
        Font = SDL_ttf.TTF_OpenFont(path, 24);
    }
}