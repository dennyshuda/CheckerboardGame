using CheckerboardGame.Backend.Interfaces;

namespace CheckerboardGame.Backend.Models;

public class Board : IBoard
{
    public Square[,] Squares { get; }
    
    public Board(Square[,] squares)
    {
        Squares = squares;
    }

}