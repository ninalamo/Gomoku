using Gomoku.Domain.AggregatesModel;
using Gomoku.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Gomoku.Infrastructure.Repositories;

public class GamesRepository : IGamesRepository
{
    private readonly GameContext _dbContext;
    public IUnitOfWork UnitOfWork => _dbContext;

    public GamesRepository(GameContext dbContext)
    {
        _dbContext = dbContext?? throw new Exception($"{nameof(_dbContext)} cannot be null.");
    }

    public Game Create(Game game) => _dbContext.Games.Add(game).Entity;

    public Game Update(Game game)=> _dbContext.Games.Update(game).Entity;

    public async Task<Game> GetAsync(Guid id)
    {
        return await _dbContext.Games
            .Include(i => i.Cells)
            .Include(i => i.Players)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
}