using CheckerboardGame.Backend.Enums;
using CheckerboardGame.Backend.Models;

namespace CheckerboardGame.Backend.Interfaces;

public interface IGame
{
    void Run(List<IPlayer> players);
    void SwitchTurn();
    void DoMove(Point from, Point to);
    void RemovePiece(Point point);
    IBoard GetBoard();
}
