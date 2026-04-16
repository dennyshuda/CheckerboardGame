using CheckerboardGame.Backend.Enums;

namespace CheckerboardGame.Backend.Interfaces;

public interface IPiece
{
    Color Color { get; }
    Role Role { get; }
}