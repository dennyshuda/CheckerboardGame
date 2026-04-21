using CheckerboardGame.Backend.Dto;
using CheckerboardGame.Backend.Enums;
using CheckerboardGame.Backend.Models;

namespace CheckerboardGame.Backend.Interfaces;

public interface IGame
{
    GameStatus Status { get; set; }
    void Run(List<IPlayer> players);
    void SwitchTurn();
    IPlayer GetCurrentPlayer();
    void DoMove(Point from, Point to);
    void RemovePiece(Point point);
    IBoard GetBoard();
    int CountPieces(Color color);
    List<ValidMoveDto> GetAllValidMoves(Color color);
}
