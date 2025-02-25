using OblivionEngine.Core;

namespace OblivionEngine;

class Oblivion
{
    public static void Main(string[] args)
    {
        Game game = new Game();
        game.Start(args);
    }
}