using CheckerboardGame.Backend;
using CheckerboardGame.Backend.Interfaces;
using CheckerboardGame.Backend.Models;
using CheckerboardGame.UI;

namespace CheckerboardGame;

class Program
{
    static void Main(string[] args)
    {

        var grid = new Square[8, 8];
        IBoard board = new Board(grid);


        List<IPlayer> players = [
            new Player("Wowo"),
            new Player("Bahlil")
        ];

        IGame game = new Game(board, players);

        var consoleApp = new GameUi(game, players);


        try
        {
            consoleApp.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("Tekan tombol apapun untuk keluar...");
        Console.ReadKey();
    }
}