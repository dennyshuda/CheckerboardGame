using CheckerboardGame.Backend.Models;

namespace CheckerboardGame.Backend;


public class Game
{
    private List<IPlayer> _players;

    public Game(List<IPlayer> players)
    {
        _players = new List<IPlayer>(players);
    }

    public void GetPlayer()
    {
        Console.WriteLine(_players);
    }
}