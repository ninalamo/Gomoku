namespace Gomoku.Domain.SeedWork;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    public bool IsTransient() => Id == Guid.Empty;
}