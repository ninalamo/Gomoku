
using Gomoku.Domain.SeedWork;

namespace Gomoku.Domain.AggregatesModel;

public class Player : Entity
{

    public Player(string name)
    {
        Name = name;
        Color = Pebbles.Empty;
    }

    public string Name { get; private set; }
    public Pebbles Color { get; private set; }
 
    public void SetColor(Pebbles pebbles) => Color = pebbles;
}