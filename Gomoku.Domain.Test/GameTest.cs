using System.Diagnostics;
using System.Text.Json;
using Gomoku.Domain.AggregatesModel;

namespace Gomoku.Domain.Test;

public class GameTest
{
    [Fact]
    public void BoardShouldBeCreated()
    {
        var game = new Game("Yoshi","Mario");
        
        game.InitializeBoard();

        var boardState = game.GetBoardState();

        var firstPlayer = game.CurrentPlayerId;
        var secondPlayer = game.GetNextPlayer();

        Assert.Equal(Domain.AggregatesModel.Game.CLEAN, boardState);
        Assert.Equivalent(game.Id, Guid.Empty);
        Assert.NotNull(game.Cells);
        Assert.NotEmpty(game.Cells);
        Assert.Equal(Game.CELLCOUNT, game.Cells.Count);
        Assert.True(game.Cells.All(i => i.IsEmpty()));
        
        Assert.NotNull(game.GetPlayerOne() );
        Assert.NotNull(game.GetPlayerTwo() );
        Assert.True(firstPlayer == game.GetPlayerOne().Id || firstPlayer == game.GetPlayerTwo().Id);
       
        Assert.NotEqual(game.GetPlayerOne, game.GetPlayerTwo);
        Assert.False(game.IsGameOver());
        Assert.False(game.IsBoardAlreadyFull());
    }
    
    [Fact]
    public void BoardCellsStatusCanBeChecked()
    {
        var game = new Game("Yoshi","Mario");
        
        game.InitializeBoard();
        var boardState = game.GetBoardState();
        
        Assert.Equal(Domain.AggregatesModel.Game.CLEAN, boardState);
        Assert.True(game.Cells.All(i => i.IsEmpty()));
        
    }

    [Fact]
    public void ShouldThrowExceptionOnPlayerNameValidation()
    {
        var name = "Nin";
        Assert.Throws<ArgumentException>(() => new Game(name,name));
    }

    [Fact]
    public void CanPlacePiece()
    {
        var game = new Game("Nin","Shaun");
        game.InitializeBoard();
        //we don't care if it ain't real ID or some random Guid...
        game.AddPebble(0, 0, Guid.NewGuid());
        var gameOver = game.IsGameOver();

        Assert.Equal(game.GetPlayerOne().Color, game.Cells.First(i => i is { Row: 0, Column: 0 }).Color);
        Assert.False(gameOver);
        
        //this is CLEAN because we are not generate uid yet...
        Assert.Equivalent(Game.CLEAN, game.GetBoardState());
    }
}