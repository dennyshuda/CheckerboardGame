using CheckerboardGame.Backend.Enums;
using CheckerboardGame.Backend.Interfaces;
using CheckerboardGame.Backend.Models;

namespace CheckerboardGame.Backend;


public class Game : IGame
{
    private readonly IBoard _board;
    private List<IPlayer> _players;
    private int _currentPlayerIndex;

    public Game(IBoard board)
    {
        _board = board;
        _currentPlayerIndex = 0;
        
        InitializeBoard();
        InitializePiece();
    }

    private void InitializeBoard()
    {
        Console.WriteLine("Board Initialization");
    }

    private void InitializePiece()
    {
        Console.WriteLine("Piece Initialization");
        _board.Squares[0, 0].Piece = new Piece(Color.White, Role.Troop);
    
    }

    public void Run(List<IPlayer> players)
    {
        if (players.Count != 2)
        {
            throw new ArgumentException("Checkers need 2 players!");
        }

        IPlayer white = players[0];
        IPlayer black = players[1];
        
        Console.WriteLine($"Game Ready: {white.Name} (White) vs {black.Name} (Black)");
    }
}