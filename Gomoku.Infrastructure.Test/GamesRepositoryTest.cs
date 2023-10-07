using Gomoku.Domain.AggregatesModel;
using Gomoku.Infrastructure.Repositories;

namespace Gomoku.Infrastructure.Test;


public class GamesRepositoryTest
{
    
    private readonly GameContext _dbContext;
    private readonly IGamesRepository _repository;

    public GamesRepositoryTest()
    {
        _dbContext = CreateInMemoryDbContext();
        _repository = new GamesRepository(_dbContext);
    }

    
    [Fact]
    public async Task CreateAsync_AddsGameToDatabase()
    {
        // Arrange
        var game = new Game("P1", "P2");

        // Act
        var createdGame = _repository.Create(game);

        // Assert
        Assert.NotNull(createdGame);
        await _dbContext.SaveChangesAsync();
        var retrievedGame = await _dbContext.Games.FindAsync(game.Id);
        Assert.NotNull(retrievedGame);
        Assert.Equal(createdGame, retrievedGame);
    }
    
    [Fact]
    public async Task UpdateAsync_UpdatesGameInDatabase()
    {
        // Arrange
        var game = new Game("P1", "P2");

        // Act
        var createdGame = _repository.Create(game);
        await _repository.UnitOfWork.SaveChangesAsync(CancellationToken.None);
        

        var player = createdGame.GetNextPlayer();

        createdGame.AddPebble(0, 0, player.Id);

        var nextPlayer = createdGame.GetNextPlayer();

        // Act
        var updatedGame = _repository.Update(createdGame);
        await _repository.UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(updatedGame);
        Assert.NotEqual(player.Id, nextPlayer.Id);
        Assert.False(updatedGame.IsGameOver());
        Assert.False(updatedGame.IsBoardAlreadyFull());
        Assert.Equal(updatedGame.Cells.Count(i => i.Color == Pebbles.Black) , 1);

        //we did not 'place' a piece in behalf of the 'white' player
        Assert.Equal(updatedGame.Cells.Count(i => i.Color == Pebbles.White) , 0);
    }

    [Fact]
    public async Task CanFetch()
    {
        var game = new Game("P1", "P2");
        _repository.Create(game);
        await _repository.UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var retrievedGame = await _repository.GetAsync(game.Id);

        Assert.Equal(game.Id, retrievedGame.Id);
    }

    [Fact]
    public async Task CanHaveAWinner()
    {
        var game = new Game("P1", "P2");
        var createdGame = _repository.Create(game);
        await _repository.UnitOfWork.SaveChangesAsync(CancellationToken.None);

        int winningCol = 0;
        while (!createdGame.IsGameOver())
        {
            var player = createdGame.GetNextPlayer();
            if (player.Color == Pebbles.Black)
            {
                createdGame.AddPebble(0, winningCol, player.Id);
                winningCol++;
            }
            else
                createdGame.AddPebble(new Random().Next(1,Game.BOARDSIZE-1), new Random().Next(1,Game.BOARDSIZE-1),player.Id);

            _repository.Update(createdGame);
            await _repository.UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }

        Assert.True(createdGame.IsGameOver());
        Assert.True(createdGame.Cells.Count(i => i.Color == Pebbles.Black) == 5);
        Assert.Equal(createdGame.GetWinner().Id,createdGame.GetPlayerOne().Id );
        Assert.True(createdGame.Cells.Count(i => i.Color == Pebbles.Empty) != Game.CELLCOUNT);
    }
    
    private GameContext CreateInMemoryDbContext()
    {
        var dbContextOptions = new DbContextOptionsBuilder<GameContext>()
            .UseInMemoryDatabase(databaseName: "in-memory")
            .Options;
        
        var context = new GameContext(dbContextOptions);

        return context;
    }
}