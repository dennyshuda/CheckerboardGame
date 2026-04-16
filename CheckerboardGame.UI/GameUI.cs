using CheckerboardGame.Backend.Enums;
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
        _players = new List<IPlayer>
        {
            new Player("Wowo"),
            new Player("Bahlil")  
        };
    }
    
    public void DisplayBoard()
    
    {
        IBoard boardData = _game.GetBoard();
        Console.WriteLine("   0  1  2  3  4  5  6  7");

        for (int y = 0; y < 8; y++)
        {
            Console.Write(y + " ");

            for (int x = 0; x < 8; x++)
            {
                var piece = boardData.Squares[y, x].Piece;
                
                if (piece != null)
                {
                    Console.ForegroundColor = (piece.Color == Color.White) ? ConsoleColor.White : ConsoleColor.Red;
                    Console.Write(" O "); 
                }
                else
                {
                    Console.Write("   "); 
                }

                Console.BackgroundColor = (x + y) % 2 == 0 ? ConsoleColor.Black : ConsoleColor.White;
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    public void Run() 
    {
        Console.Clear();
        Console.WriteLine($"--- Giliran ---");
        DisplayBoard();
        
        _game.GetBoard();
        _game.Run(_players);
    }
}