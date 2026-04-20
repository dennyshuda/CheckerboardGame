using CheckerboardGame.Backend.Models;

namespace CheckerboardGame.Backend.Dto;

public record ValidMoveDto
{
    public Point FromPoint { get; set; }
    public Point ToPoint { get; set; }
}