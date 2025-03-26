using System.Runtime.InteropServices;
using System.Text;
using OblivionEngine.Core;
using OblivionEngine.Core.Config;
using OblivionEngine.Core.Events;
using OblivionEngine.Core.Extras;
using OblivionEngine.Render.Font;
using SDL2;

namespace OblivionEngine.Render.Dialog;

public class DialogBox
{
    private IntPtr dialogBoxImage;
    private bool _isShown;
    
    private List<StringBuilder> dialogBoxText;
    private static object _diagLock = new object();
    
    public DialogBox(OblivionRenderer renderer)
    {
        dialogBoxImage = SDL_image.IMG_LoadTexture(renderer.GetRenderer(), "resources/images/DialogBox.png");
        _isShown = false;
        
        dialogBoxText = new List<StringBuilder>();
        dialogBoxText.Add(new StringBuilder(""));
        
        OblivionEventManager.Instance.OnUIDraw += OnUIDraw;
    }
    
    public void ToggleDialogBox()
    {
        _isShown = !_isShown;

        if (!_isShown)
        {
            ClearText();
        }
    }
    
    private void OnUIDraw(object? sender, OblivionRenderer renderer)
    {
        if (!_isShown) return;
        
        SDL.SDL_Rect srect = new SDL.SDL_Rect()
        {
            x = 0,
            y = 0,
            w = 1490,
            h = 460
        };
        
        SDL.SDL_Rect drect = new SDL.SDL_Rect()
        {
            x = 80,
            y = 206,
            w = 480,
            h = 144
        };
        
        SDL.SDL_RenderCopy(renderer.GetRenderer(), dialogBoxImage, ref srect, ref drect);

        int lastH = 0;
        for (int i = 0; i < dialogBoxText.Count; i++)
        {
            IntPtr textSurface = SDL_ttf.TTF_RenderText_Solid(OblivionFontManager._instance.DiagFont, dialogBoxText[i].ToString(), new SDL.SDL_Color() { r = 0, g = 0, b = 0, a = 255 });
            IntPtr textTexture = SDL.SDL_CreateTextureFromSurface(renderer.GetRenderer(), textSurface);
            SDL.SDL_FreeSurface(textSurface);
            
            int texW = 0;
            int texH = 0;
            
            SDL.SDL_QueryTexture(textTexture, out _, out _, out texW, out texH);

            if (texW > 440)
            {
                texW = 440;
            }
            
            SDL.SDL_Rect tDrect = new SDL.SDL_Rect()
            {
                x = 100,
                y = 220 + lastH,
                w = texW,
                h = texH
            };
            
            lastH += texH;
            
            SDL.SDL_RenderCopy(renderer.GetRenderer(), textTexture, IntPtr.Zero, ref tDrect);
        }
    }

    public void DisplayMessage(string message, TextSpeed speed = TextSpeed.Normal)
    {
        if (!_isShown) return;
        
        Thread typewriter = new Thread(() => Typewriter(message, 60 * (int)speed));
        typewriter.Start();
    }

    private void Typewriter(string message, int delay)
    {
        for (int i = 0; i < message.Length; i++)
        {
            int textWidth = 0;
            SDL_ttf.TTF_SizeText(OblivionFontManager._instance.DiagFont, dialogBoxText.Last().ToString(), out textWidth, out _);
            if (textWidth >= 420)
            {
                dialogBoxText.Add(new StringBuilder(""));
            }
            
            if (message[i] == '\n' && dialogBoxText.Last().ToString() != "")
            {
                dialogBoxText.Add(new StringBuilder(""));
                continue;
            }
            dialogBoxText.Last().Append(message[i]);
            Thread.Sleep(delay);
        }
    }

    public void ClearText()
    {
        dialogBoxText.Clear();
        dialogBoxText.Add(new StringBuilder(""));
    }
}