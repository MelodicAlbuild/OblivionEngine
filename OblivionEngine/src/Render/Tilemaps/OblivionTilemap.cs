using DotTiled;
using DotTiled.Serialization;
using OblivionEngine.Core.Events;
using SDL2;

namespace OblivionEngine.Render.Tilemaps;

public class Tilemap
{
    private Loader _loader;
    private Map _map;

    public Tilemap()
    {
        _loader = Loader.Default();
        OblivionEventManager.Instance.OnDraw += OnDraw;
    }
    
    public void LoadMap(string path)
    {
        _map = _loader.LoadMap(path);
    }

    private void OnDraw(object? sender, OblivionRenderer renderer)
    {
        renderer.SetColorNoSave(10, 255, 255, 0);
        renderer.Clear();

        int l = 0;

        foreach (BaseLayer layer in _map.Layers)
        {
            if (layer is TileLayer)
            {
                TileLayer tl = (TileLayer)layer;
                for (int y = 0; y < tl.Height; y++)
                {
                    for (int x = 0; x < tl.Width; x++)
                    {
                        SDL.SDL_Log($"Tile in layer {l} at {x}, {y} is {tl.Data.Value.GlobalTileIDs.Value[y * _map.Width + x]}");
                    }
                }
            } else if (layer is ImageLayer)
            {
                
            } else if (layer is ObjectLayer)
            {
                
            }

            l++;
        }
    }
}