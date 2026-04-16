using CheckerboardGame.Backend.Interfaces;

namespace CheckerboardGame.Backend.Models;

public class Square : ISquare
{
    public Point Point { get; set; }
    public Piece? Piece { get; set; }

    public Square(Point point, Piece? piece)
    {
        Point = point;
        Piece = piece;
    }
}