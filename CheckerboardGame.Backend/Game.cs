using CheckerboardGame.Backend.Dto;
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
    public GameStatus Status { get; set; }

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
    }

    private void InitializePiece()
    {
        for (var y = 0; y < 8; y++)
        {
            for (var x = 0; x < 8; x++)
            {
                if ((x + y) % 2 != 0)
                {
                    _board.Squares[y, x].Piece = y switch
                    {
                        < 3 => new Piece(Color.Black, Role.Troop),
                        > 4 => new Piece(Color.White, Role.Troop),
                        _ => _board.Squares[y, x].Piece
                    };
                }
            }
        }
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
            var midCol = (from.X + to.X) / 2;
            var midRow = (from.Y + to.Y) / 2;
            RemovePiece(new Point(midRow, midCol));
        }

        squareTo.Piece = piece;
        squareFrom.Piece = null;

        if (piece != null) CheckPromotion(piece, to);

        SwitchTurn();
    }

    public void RemovePiece(Point point)
    {
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
        var diffRow = to.Y - from.Y;
        var diffCol = Math.Abs(to.X - from.X);

        var allowedDirectionRow = (piece.Color == Color.White) ? -1 : 1;

        var isDirectionValid = (IsKing(piece)) ? Math.Abs(diffRow) == 1 : diffRow == allowedDirectionRow;
        
        var isPieceExist = _board.Squares[to.Y, to.X].Piece == null;

        return isDirectionValid && diffCol == 1 && isPieceExist;
    }

    private bool IsCaptureMoveValid(Point from, Point to, Piece piece)
    {
        var diffRow = to.Y - from.Y;
        var diffCol = to.X - from.X;

        if (Math.Abs(diffCol) != 2 || Math.Abs(diffRow) != 2) return false;

        var midCol = from.X + (diffCol / 2);
        var midRow = from.Y + (diffRow / 2);
        var middlePiece = _board.Squares[midRow, midCol].Piece;

        var hasEnemyInMiddle = middlePiece != null && middlePiece.Color != piece.Color;
        var isLandingEmpty = _board.Squares[to.Y, to.X].Piece == null;

        if (!IsKing(piece))
        {
            var allowedJumpDy = (piece.Color == Color.White) ? -2 : 2;
            if (diffRow != allowedJumpDy) return false;
        }

        return hasEnemyInMiddle && isLandingEmpty;
    }

    public List<ValidMoveDto> GetAllValidMoves(Color color)
    {
        var captures = GetCaptureMovesFrom(color);
        if (captures.Count > 0) return captures;
        
        var normalMoves = new List<ValidMoveDto>();
        for (var y = 0; y < 8; y++)
        {
            for (var x = 0; x < 8; x++)
            {
                var piece = _board.Squares[y, x].Piece;
                if (piece == null || piece.Color != color) continue;
                var rowDirection = (IsKing(piece)) ? [-1, 1] : new[] { (color == Color.White ? -1 : 1) };
                    
                foreach (var row in rowDirection)
                {
                    var from = new Point(x, y);
                    
                    var toLeft = new Point(x - 1, y + row);
                    if (IsInsideBoard(toLeft) && IsNormalMoveValid(from, toLeft, piece))
                    {
                        normalMoves.Add(new ValidMoveDto { FromPoint = from, ToPoint = toLeft });
                    }
                    
                    var toRight = new Point(x + 1, y + row);
                    if (IsInsideBoard(toRight) && IsNormalMoveValid(from, toRight, piece))
                    {
                        normalMoves.Add(new ValidMoveDto { FromPoint = from, ToPoint = toRight });
                    }
                }
            }
        }
        
        return normalMoves;
    }
    
    
    private bool IsInsideBoard(Point point) => point.X is >= 0 and < 8 && point.Y is >= 0 and < 8;

    private List<ValidMoveDto> GetCaptureMovesFrom(Color color)
    {
        var captures = new List<ValidMoveDto>();
        for (var y = 0; y < 8; y++)
        {
            for (var x = 0; x < 8; x++)
            {
                var piece = _board.Squares[y, x].Piece;
                if (piece == null || piece.Color != color) continue;
                int[] dirs = [-2, 2];
                foreach (var dy in dirs)
                {
                    foreach (var dx in dirs)
                    {
                        var from = new Point(x, y);
                        var to = new Point(x + dx, y + dy);
                        if (!IsInsideBoard(to)) continue;
                        if (IsCaptureMoveValid(from, to, piece))
                            captures.Add(new ValidMoveDto { FromPoint = from, ToPoint = to });
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

    private void CheckPromotion(Piece piece, Point to)
    {
        if (piece?.Role != Role.Troop) return;
        if ((piece.Color != Color.White || to.Y != 0) &&
            (piece.Color != Color.Black || to.Y != 7)) return;
        piece.Role = Role.King;
        Console.WriteLine("PROMOSI! Bidak menjadi KING!");
    }

    public GameStatus GetGameStatus()
    {
        return Status;
    }

    public int CountPieces(Color color)
    {
        var count = 0;
        for (var y = 0; y < 8; y++)
        {
            for (var x = 0; x < 8; x++)
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
    }

    public IPlayer GetCurrentPlayer()
    {
        return _players[_currentPlayerIndex];
    }
}