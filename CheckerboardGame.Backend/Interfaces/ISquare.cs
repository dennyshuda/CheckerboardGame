using CheckerboardGame.Backend.Models;

namespace CheckerboardGame.Backend.Interfaces;

public interface ISquare
{
    Point Point { get; set; }
    Piece? Piece { get; set; }
}