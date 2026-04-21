using CheckerboardGame.Backend;
using CheckerboardGame.Backend.Dto;
using CheckerboardGame.Backend.Enums;
using CheckerboardGame.Backend.Interfaces;

namespace CheckerboardGame.UI;

public class GameUi
{
    private readonly IGame _game;
    private readonly List<IPlayer> _players;

    public GameUi(IGame game, List<IPlayer> players)
    {
        _game = game ?? throw new ArgumentNullException(nameof(game));
        _players = players ?? throw new ArgumentNullException(nameof(players));
        _game.PlayerSwitched += HandlePlayerSwitched;
    }

    private void HandlePlayerSwitched(object? sender, PlayerSwitchedEventArgs e)
    {
        Console.WriteLine("\n---------------------------------");
        Console.Write("🔔 NOTIFIKASI: Giliran ");

        Console.Write($"{e.Name.ToUpper()} - {(_players[0].Name == e.Name ? "PUTIH" : "HITAM")}");
        Console.ResetColor();

        Console.WriteLine(" sekarang!");
        Console.WriteLine("---------------------------------");
    }

    public void DisplayBoard()
    {
        IBoard boardData = _game.GetBoard();

        Console.WriteLine("   0  1  2  3  4  5  6  7");

        for (var y = 0; y < 8; y++)
        {
            Console.Write(y + " ");

            for (var x = 0; x < 8; x++)
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

    public void DisplayValidMoves(List<ValidMoveDto> moves)
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
            Console.WriteLine($"[{i + 1}] ({m.FromPoint.Y},{m.FromPoint.X}) -> ({m.ToPoint.Y},{m.ToPoint.X})");
        }
    }

    public void PlayTurn()
    {
        Console.WriteLine(_game.GetCurrentPlayer().Name);

        var validMoves = _game.GetAllValidMoves(_players[0].Name == _game.GetCurrentPlayer().Name ? Color.White : Color.Black);

        if (validMoves.Count == 0)
        {
            _game.Status = GameStatus.GameOver;
            Console.WriteLine("Game Over! Tidak ada langkah tersisa.");
            return;
        }

        DisplayBoard();

        DisplayValidMoves(validMoves);

        Console.Write("Pilih nomor langkah: ");
        var choice = int.Parse(Console.ReadLine() ?? string.Empty) - 1;

        var selectedMove = validMoves[choice];

        _game.DoMove(selectedMove.FromPoint, selectedMove.ToPoint);
    }

    public void Run()
    {
        Console.Clear();
        while (_game.Status != GameStatus.GameOver)
        {
            Color? winner = _game.GetWinner();
            if (winner != null)
            {
                Console.WriteLine($"\n===============================");
                Console.WriteLine($"   PERMAINAN SELESAI!");
                Console.WriteLine($"   PEMENANGNYA ADALAH: {winner}");
                Console.WriteLine($"===============================");
                break;
            }

            PlayTurn();
        }
    }
}