using Gomoku.Domain.SeedWork;

namespace Gomoku.Domain.AggregatesModel;

public interface IGamesRepository : IRepository<Game>
{
    Game Create(Game game);
    Game Update(Game game);
    Task<Game> GetAsync(Guid id);
}