using Gomoku.Domain.SeedWork;

namespace Gomoku.Domain.AggregatesModel;

public class Cell : Entity
{
    public Cell(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public int Row { get; private set; }
    public int Column { get; private set; }

    public Pebbles Color { get; private set; } = Pebbles.Empty;

    public bool IsEmpty() => Color == Pebbles.Empty;

    public void AddPebble(Pebbles color)
    {
        Color = color;
    }
}