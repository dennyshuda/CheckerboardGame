using CheckerboardGame.Backend.Interfaces;

namespace CheckerboardGame.Backend.Models;

public class Board : IBoard
{
    public Square[,] Squares { get; set; }
    
    public Board(Square[,] squares)
    {
        Squares = squares;
    }
    
}