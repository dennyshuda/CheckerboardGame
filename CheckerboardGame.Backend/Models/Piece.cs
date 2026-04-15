namespace CheckerboardGame.Backend.Models;

public struct Piece
{
    public int X;
    public int Y;

    public Piece(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Piece other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}