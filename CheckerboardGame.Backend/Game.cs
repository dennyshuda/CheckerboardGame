using CheckerboardGame.Backend.Enums;
using CheckerboardGame.Backend.Interfaces;
using CheckerboardGame.Backend.Models;

namespace CheckerboardGame.Backend;

public class Game : IGame
{
    private IBoard _board;

    private List<IPlayer> _players;
    private int _currentPlayerIndex;
    private IPlayer? _winner;
    private GameStatus Status { get; }

    public Game(IBoard board) 
    {
        _board = board ?? throw new ArgumentNullException(nameof(board));
        _currentPlayerIndex = 0;
        Status = GameStatus.Ongoing;
        
        
        InitializeBoard();
        InitializePiece();
    }

    private void InitializeBoard()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                _board.Squares[y, x] = new Square(new Point(x, y), null);
            }
        }
        Console.WriteLine("Backend: 64 Kotak (Squares) telah dibuat.");
    }
    
    private void InitializePiece()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if ((x + y) % 2 != 0)
                {
                    if (y < 3)
                    {
                        _board.Squares[y, x].Piece = new Piece(Color.Black, Role.Troop);
                    }
                    else if (y > 4)
                    {
                        _board.Squares[y, x].Piece = new Piece(Color.White, Role.Troop);
                    }
                }
            }
        }
        Console.WriteLine("Backend: Bidak telah disusun di posisi awal.");
    }
    
    public IBoard GetBoard()
    {
        return _board;
    }
    
    public void Run(List<IPlayer> players)
    {
        if (players.Count != 2)
        {
            throw new ArgumentException("Checkers need 2 players!");
        }

        _players = players;
        
        Console.WriteLine($"Game Ready: {_players[0].Name} (White) vs {_players[1].Name} (Black)");
    }

    public void DoMove(Point from, Point to)
    {
        throw new NotImplementedException();
    }

    public void RemovePiece(Point point)
    {
        if (point.X < 0 || point.X > 7 || point.Y < 0 || point.Y > 7)
        {
            Console.WriteLine("Backend Error: Koordinat di luar papan!");
            return;
        }
        var square = _board.Squares[point.Y, point.X];
        
        if (square.Piece != null)
        {
            square.Piece = null;
            Console.WriteLine($"Backend: Bidak di ({point.Y},{point.X}) telah dihapus.");
        }
        else
        {
            Console.WriteLine($"Backend: Kotak ({point.Y},{point.X}) memang sudah kosong.");
        }
    }

    public void GetAllValidMove(Color color)
    {
        throw new NotImplementedException();
    }

    public GameStatus GetGameStatus()
    {
        return Status;
    }

    public void SwitchTurn()
    {
        if (Status is GameStatus.Draw or GameStatus.GameOver )
        {
            throw new InvalidOperationException("Game is already over");
        }
        
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
    
        IPlayer currentPlayer = _players[_currentPlayerIndex];
        Console.WriteLine($"\nCurrent Turn: {currentPlayer.Name}");
    }
    
}