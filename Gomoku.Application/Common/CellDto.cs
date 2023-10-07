namespace Gomoku.Application.Common;

public record CellDto
{
    public int Row { get; init; }
    public int Column { get; init; }
    public int Color { get; init; }
}