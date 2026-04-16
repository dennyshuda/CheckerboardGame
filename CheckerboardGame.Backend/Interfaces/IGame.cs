using CheckerboardGame.Backend.Enums;

namespace CheckerboardGame.Backend.Interfaces;

public interface IGame
{
    GameStatus Status { get; }
    IBoard Board { get; }
    void Run(List<IPlayer> players);
    void SwitchTurn();
}
