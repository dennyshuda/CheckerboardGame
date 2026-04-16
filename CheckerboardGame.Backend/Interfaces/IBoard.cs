using CheckerboardGame.Backend.Models;

namespace CheckerboardGame.Backend.Interfaces;

public interface IBoard
{
    Square[,] Squares { get; }
}