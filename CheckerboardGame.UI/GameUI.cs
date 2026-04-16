using CheckerboardGame.Backend.Interfaces;
using CheckerboardGame.Backend.Models;

namespace CheckerboardGame.UI;

public class GameUi
{
    private readonly IGame _game;
    private readonly List<IPlayer> _players;

   
    public GameUi(IGame game)
    {
        _game = game ?? throw new ArgumentNullException(nameof(game));
        Console.CursorVisible = false;
        _players = new List<IPlayer>
        {
            new Player("Wowo"), // Pemain 1 (Putih)
            new Player("Bahlil")    // Pemain 2 (Hitam)
        };
        
    }

    public void Run() 
    {
        _game.Run(_players);
    }
}