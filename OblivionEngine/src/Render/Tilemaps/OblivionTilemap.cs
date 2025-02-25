using System.Numerics;
using DotTiled;
using DotTiled.Serialization;
using OblivionEngine.Core;
using OblivionEngine.Core.Events;
using SDL2;

namespace OblivionEngine.Render.Tilemaps;

public class OblivionTilemap
{
    private Loader _loader;
    private Map _map;
    private SortedDictionary<uint, Tileset> _tilesets;
    private SortedDictionary<uint, IntPtr> _textures;

    private List<Vector2> _collidableTiles;
    private List<Vector2> _interactableTiles;
    
    private int LEVEL_WIDTH = 0;
    private int LEVEL_HEIGHT = 0;
    
    public int GetLevelWidth()
    {
        return LEVEL_WIDTH;
    }
    
    public int GetLevelHeight()
    {
        return LEVEL_HEIGHT;
    }

    public OblivionTilemap()
    {
        _loader = Loader.Default();
        _tilesets = new SortedDictionary<uint, Tileset>();
        _textures = new SortedDictionary<uint, IntPtr>();
        _collidableTiles = new List<Vector2>();
        _interactableTiles = new List<Vector2>();
        OblivionEventManager.Instance.OnDraw += OnDraw;
    }
    
    public bool ContainsCollidableTile(Vector2 position)
    {
        return _collidableTiles.Contains(position);
    }
    
    public void AddCollidableTile(Vector2 position)
    {
        _collidableTiles.Add(position);
    }
    
    public void LoadMap(string path)
    {
        SDL.SDL_Log("Loading new Map: " + path);
        
        _map = _loader.LoadMap(path);
        
        LEVEL_WIDTH = (int)_map.Width * Game.TILE_SIZE;
        LEVEL_HEIGHT = (int)_map.Height * Game.TILE_SIZE;
        
        _tilesets.Clear();
        _textures.Clear();
        _collidableTiles.Clear();
        _interactableTiles.Clear();
        foreach (Tileset tileset in _map.Tilesets)
        {
            _tilesets.Add(tileset.FirstGID, tileset);
        }
    }
    
    public List<Vector2> GetCollidableTiles()
    {
        return _collidableTiles;
    }
    
    public List<Vector2> GetInteractableTiles()
    {
        return _interactableTiles;
    }

    private void OnDraw(object? sender, OblivionRenderer renderer)
    {
        foreach (BaseLayer layer in _map.Layers)
        {
            if (layer is TileLayer)
            {
                TileLayer tl = (TileLayer)layer;
                if(!tl.Visible) continue;
                
                for (int y = 0; y < tl.Height; y++)
                {
                    for (int x = 0; x < tl.Width; x++)
                    {
                        if (tl.Data.Value.GlobalTileIDs.Value[y * _map.Width + x] != 0)
                        {
                            if (layer.GetProperty<BoolProperty>("doesCollide").Value)
                            {
                                _collidableTiles.Add(new Vector2(x, y));
                            }
                            
                            if (layer.GetProperty<BoolProperty>("doesInteract").Value)
                            {
                                _interactableTiles.Add(new Vector2(x, y));
                            }
                            
                            Tileset ts = FindClosestEntry(_tilesets,
                                tl.Data.Value.GlobalTileIDs.Value[y * _map.Width + x]);

                            IntPtr tex;

                            if (!_textures.ContainsKey(ts.FirstGID))
                            {
                                SDL.SDL_Log("resources/" + ts.Image.Value.Source.Value.Replace("../", ""));
                                _textures.Add(ts.FirstGID, SDL_image.IMG_LoadTexture(renderer.GetRenderer(), "resources/" + ts.Image.Value.Source.Value.Replace("../", "")));
                            }
                            
                            tex = _textures[ts.FirstGID];

                            SDL.SDL_Rect srect = new SDL.SDL_Rect()
                            {
                                x = (int)(((tl.Data.Value.GlobalTileIDs.Value[y * _map.Width + x] - ts.FirstGID) % (int)ts.Columns) * 32), y =
                                    (int)(((tl.Data.Value.GlobalTileIDs.Value[y * _map.Width + x] - ts.FirstGID) / (int)ts.Columns) * 32),
                                h = 32, w = 32
                            };
                            SDL.SDL_Rect drect = new SDL.SDL_Rect() {x = (x * 32) - Game.Instance.GetCameraX(), y = (y * 32) - Game.Instance.GetCameraY(), h = 32, w = 32};
                            SDL.SDL_RenderCopy(renderer.GetRenderer(), tex, ref srect,  ref drect);
                        }
                    }
                }
            } else if (layer is ImageLayer)
            {
                
            } else if (layer is ObjectLayer)
            {
                
            }
        }
    }
    
    private Tileset FindClosestEntry(SortedDictionary<uint, Tileset> data, uint key)
    {
        // If exact key exists, return its value
        if (data.ContainsKey(key))
        {
            return data[key];
        }

        // Find the largest key that is less than or equal to the given key
        uint closestKey = data.Keys.Where(k => k <= key).DefaultIfEmpty(data.First().Key).Max();

        return data[closestKey];
    }
}