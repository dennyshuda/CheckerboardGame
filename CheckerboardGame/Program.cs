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

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                // Setiap Square wajib punya Point (x, y)
                // Piece diset null dulu karena papan baru disiapkan kosong
                grid[y, x] = new Square(new Point(x, y), null);
            }
        }
        IBoard board = new Board(grid);
        
        IGame game = new Game(board);
        
        var consoleApp = new GameUi(game);
        
        
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