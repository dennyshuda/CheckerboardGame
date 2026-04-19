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
        var squareFrom = _board.Squares[from.Y, from.X];
        var squareTo = _board.Squares[to.Y, to.X];
        var piece = squareFrom.Piece;

        if (Math.Abs(from.X - to.X) == 2)
        {
            int midX = (from.X + to.X) / 2;
            int midY = (from.Y + to.Y) / 2;
            _board.Squares[midY, midX].Piece = null;
        }

        squareTo.Piece = piece;
        squareFrom.Piece = null;


        SwitchTurn();
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

    private bool IsNormalMoveValid(Point from, Point to, Piece piece)
    {
        int dy = to.Y - from.Y;
        int dx = Math.Abs(to.X - from.X);

        int allowedDy = (piece.Color == Color.White) ? -1 : 1;

        bool isDirectionValid = (piece.Role == Role.King) ? Math.Abs(dy) == 1 : dy == allowedDy;

        return isDirectionValid && dx == 1 && _board.Squares[to.Y, to.X].Piece == null;
    }

    private bool IsCaptureMoveValid(Point from, Point to, Piece piece)
    {
        int dy = to.Y - from.Y;
        int dx = to.X - from.X;

        if (Math.Abs(dx) != 2 || Math.Abs(dy) != 2) return false;

        int midX = from.X + (dx / 2);
        int midY = from.Y + (dy / 2);
        var middlePiece = _board.Squares[midY, midX].Piece;

        bool hasEnemyInMiddle = middlePiece != null && middlePiece.Color != piece.Color;
        bool isLandingEmpty = _board.Squares[to.Y, to.X].Piece == null;

        if (piece.Role != Role.King)
        {
            int allowedJumpDy = (piece.Color == Color.White) ? -2 : 2;
            if (dy != allowedJumpDy) return false;
        }

        return hasEnemyInMiddle && isLandingEmpty;
    }

    public List<(Point, Point)> GetAllValidMoves(Color color)
    {
        var captures = GetCaptureMovesFrom(color);

        if (captures.Count > 0) return captures;

        var normalMoves = new List<(Point, Point)>();
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                var piece = _board.Squares[y, x].Piece;
                if (piece != null && piece.Color == color)
                {
                    int[] dyDir = (piece.Role == Role.King) ? new int[] { -1, 1 } : new int[] { (color == Color.White ? -1 : 1) };
                    int[] dxDir = { -1, 1 };

                    foreach (int dy in dyDir)
                    {
                        foreach (int dx in dxDir)
                        {
                            Point from = new Point(x, y);
                            Point to = new Point(x + dx, y + dy);
                            if (to.X >= 0 && to.X < 8 && to.Y >= 0 && to.Y < 8)
                            {
                                if (IsNormalMoveValid(from, to, piece))
                                    normalMoves.Add((from, to));
                            }
                        }
                    }
                }
            }
        }
        return normalMoves;
    }

    private List<(Point, Point)> GetCaptureMovesFrom(Color color)
    {
        var captures = new List<(Point, Point)>();
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                var piece = _board.Squares[y, x].Piece;
                if (piece != null && piece.Color == color)
                {
                    int[] dirs = { -2, 2 };
                    foreach (int dy in dirs)
                    {
                        foreach (int dx in dirs)
                        {
                            Point from = new Point(x, y);
                            Point to = new Point(x + dx, y + dy);
                            if (to.X >= 0 && to.X < 8 && to.Y >= 0 && to.Y < 8)
                            {
                                if (IsCaptureMoveValid(from, to, piece))
                                    captures.Add((from, to));
                            }
                        }
                    }
                }
            }
        }
        return captures;
    }

    private bool IsKing(Piece piece)
    {
        return piece.Role == Role.King;
    }

    private void PromoteToKing(Role role)
    {
        throw new NotImplementedException();
    }

    public GameStatus GetGameStatus()
    {
        return Status;
    }

    public int CountPieces(Color color)
    {
        int count = 0;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (_board.Squares[y, x].Piece?.Color == color)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void SwitchTurn()
    {
        if (_players.Count == 0) return;
        if (Status is GameStatus.Draw or GameStatus.GameOver)
        {
            throw new InvalidOperationException("Game is already over");
        }

        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

        IPlayer currentPlayer = _players[_currentPlayerIndex];
        Console.WriteLine($"\nCurrent Turn: {currentPlayer.Name}");
    }

    public IPlayer GetCurrentPlayer()
    {
        return _players[_currentPlayerIndex];
    }
}