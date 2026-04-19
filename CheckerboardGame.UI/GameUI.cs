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
        _players =
        [
            new Player("Wowo"),
            new Player("Bahlil")
        ];
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

    public void DisplayValidMoves(List<(Point From, Point To)> moves)
    {
        if (moves.Count == 0)
        {
            Console.WriteLine("Tidak ada langkah yang tersedia!");
            return;
        }

        Console.WriteLine("\n--- Langkah yang Tersedia ---");

        for (int i = 0; i < moves.Count; i++)
        {
            var m = moves[i];
            Console.WriteLine($"[{i + 1}] ({m.From.Y},{m.From.X}) -> ({m.To.Y},{m.To.X})");
        }
    }

    public void PlayTurn()
    {
        var validMoves = _game.GetAllValidMoves(Color.White);

        if (validMoves.Count == 0)
        {
            Console.WriteLine("Game Over! Tidak ada langkah tersisa.");
            return;
        }

        DisplayValidMoves(validMoves);

        Console.Write("Pilih nomor langkah: ");
        int choice = int.Parse(Console.ReadLine()) - 1;

        var selectedMove = validMoves[choice];

        _game.DoMove(selectedMove.Item1, selectedMove.Item2);
    }

    public void Run()
    {
        Console.Clear();
        _game.Run(_players);
        Console.WriteLine(_game.CountPieces(Color.Black));
        Console.WriteLine($"--- Giliran ---");
        var currentPlayer = _game.GetCurrentPlayer();
        DisplayBoard();
        _game.RemovePiece(new Point(0, 1));
        _game.RemovePiece(new Point(0, 1));
        Console.WriteLine(_game.CountPieces(Color.Black));
        Console.WriteLine($"Bidak di (2,5) telah dihapus.");
        Console.WriteLine($"Sekarang giliran: {currentPlayer.Name}");
        _game.SwitchTurn();
        var currentPlayer2 = _game.GetCurrentPlayer();
        Console.WriteLine($"Sekarang giliran: {currentPlayer2.Name}");
        PlayTurn();
        DisplayBoard();



        _game.GetBoard();
    }
}