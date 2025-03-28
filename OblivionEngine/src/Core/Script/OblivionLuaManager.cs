using Lua;
using OblivionEngine.Exceptions;
using SDL2;

namespace OblivionEngine.Core.Script;

public class OblivionLuaManager
{
    private LuaState _lua;
    
    public OblivionLuaManager()
    {
        _lua = LuaState.Create();
    }

    public async void LoadNPCs(string path)
    {
        try
        {
            _lua.Environment["NPC"] = new LuaNPC();

            LuaValue[] results = await _lua.DoFileAsync(path);

            LuaTable t = results[0].Read<LuaTable>();
            for (int i = 1; i <= t.ArrayLength; i++)
            {
                Game.Instance.CreateNPC(t[i].Read<LuaNPC>());
            }
        }
        catch (Exception e)
        {
            throw new NpcLoadException("Failed to load NPCs from Lua file.", e);
        }
    }
}