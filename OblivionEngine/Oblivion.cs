using OblivionEngine.Core;
using OblivionEngine.Exceptions;
using OblivionEngine.Init;

namespace OblivionEngine;

class Oblivion
{
    public static async Task Main(string[] args)
    {
        while (!SysInit.Instance.Initialized)
        {
            await Task.Delay(25);
        }
        
        Game game = new Game();
        game.Start(args);
    }
}