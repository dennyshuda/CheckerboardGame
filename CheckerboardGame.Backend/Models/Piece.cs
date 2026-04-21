using CheckerboardGame.Backend.Enums;
using CheckerboardGame.Backend.Interfaces;

namespace CheckerboardGame.Backend.Models;

public class Piece : IPiece
{
    public Color Color { get; }
    public Role Role { get; set; }

    public Piece(Color color, Role role)
    {
        Color = color;
        Role = role;
    }
}