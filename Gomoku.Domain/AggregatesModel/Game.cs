using Gomoku.Domain.SeedWork;

namespace Gomoku.Domain.AggregatesModel;

public class Game : Entity, IAggregateRoot
{
    
    #region Constants
    public const string UNINITIALIZED = nameof(UNINITIALIZED);
    public const string FULL = nameof(FULL);
    public const string CLEAN = nameof(CLEAN);
    public const string INPROGRESS = nameof(INPROGRESS);
    public const string INVALID = nameof(INVALID);
    public const int BOARDSIZE = 15;
    public const int CELLCOUNT = BOARDSIZE * BOARDSIZE;
    #endregion
    
    public Player PlayerOne { get; private set; }
    public Player PlayerTwo { get; private set; }

    private readonly List<Cell> _cells;
    public IReadOnlyCollection<Cell> Cells => _cells.AsReadOnly();
    
    public Guid CurrentPlayerId { get; private set; }

    #region Ctor

    public Game()
    {
        _cells = new List<Cell>();
    }
    
    public Game(string player1, string player2) : this()
    {
        if (player1 == player2) throw new ArgumentException("Player names should not be the same.");

        PlayerOne = new(player1);
        PlayerTwo = new(player2);
      
    }

    #endregion
    
   /// <summary>
   /// Checks the state of the board
   /// </summary>
   /// <returns></returns>
    public string GetBoardState()
    {
        if (!_cells.Any()) return UNINITIALIZED;

        if (_cells.Count() != CELLCOUNT) return INVALID;
        
        if (_cells.Count(i => i.Color != Pebbles.Empty) == Game.CELLCOUNT) return FULL;

        if (_cells.All(i => i.Color == Pebbles.Empty)) return CLEAN;

        return INPROGRESS;

    }

    /// <summary>
    /// Player 'puts' her pebble on the 'board'
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="playerId"></param>
    /// <exception cref="ArgumentException"></exception>
    public void AddPebble(int row, int col, Guid playerId)
    {
        var currentPlayer = GetCurrentPlayer();

        var cell = GetCell(row, col);

        cell.AddPebble(currentPlayer.Color);            
    }

    /// <summary>
    /// Check if cell is empty (Pebble.Empty)
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public bool IsCellEmpty(int row, int col)
    {
        var cell = GetCell(row, col);
        return cell.IsEmpty();
    }

    /// <summary>
    /// Just checking if we can accept requests.
    /// We should not accept requests when
    /// </summary>
    /// <returns></returns>
    public bool IsGameOver() => HasWinner() || IsBoardAlreadyFull();
    
    /// <summary>
    /// Dunno, just put it here. Maybe it can be of use.
    /// </summary>
    /// <returns></returns>
    public bool IsBoardAlreadyFull() => GetBoardState() == FULL;
    
    /// <summary>
    /// If we  have a winner, get that player;
    /// Otherwise just return null
    /// </summary>
    /// <returns></returns>
    public Player? GetWinner()
    {
        if (HasWinner())
        {
            return CurrentPlayerId == PlayerOne.Id ? PlayerOne : PlayerTwo;
        }

        return null;
    }

    /// <summary>
    /// Switches player based on the currentPlayer saved.
    /// </summary>
    /// <returns></returns>
    public Player GetNextPlayer()
    {
        if (CurrentPlayerId == default) return RandomizeFirstPlayer();

        if (CurrentPlayerId ==  PlayerOne.Id)
        {
            CurrentPlayerId = PlayerTwo.Id;
            return PlayerTwo;
        }
        
        CurrentPlayerId = PlayerOne.Id;
        return PlayerOne;
    }

    public Player GetPlayerOne() => PlayerOne;
    public Player GetPlayerTwo() => PlayerTwo;

  
    
    /// <summary>
    /// Creates the board
    /// </summary>
    /// <param name="playerOne"></param>
    /// <param name="playerTwo"></param>
    public void InitializeBoard()
    {
        for (int row = 0; row < BOARDSIZE; row++)
        {
            for (int col = 0; col < BOARDSIZE; col++)
            {
                _cells.Add(new Cell(row,col));
            }
        }

    
    }
    
    /// <summary>
    /// Assign who goes first. Basically assigning who gets to be 'black'
    /// </summary>
    /// <returns></returns>
    private Player RandomizeFirstPlayer()
    {
        var random = new Random().Next(1, 2);
        if (random == 1)
        {
            PlayerOne.SetColor(Pebbles.Black);
            PlayerTwo.SetColor(Pebbles.White);
            CurrentPlayerId = PlayerOne.Id;
            
            return PlayerOne;
        }

        PlayerOne.SetColor(Pebbles.White);
        PlayerTwo.SetColor(Pebbles.Black);
        CurrentPlayerId = PlayerTwo.Id;
        
        return PlayerTwo;
    }
    
    /// <summary>
    /// Go find if such a cell block exists
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private Cell GetCell(int row, int col)
    {
        var cell = _cells.Where(i => i.Row == row && i.Column == col && !i.IsTransient());
        return cell.FirstOrDefault();
    }
    
    /// <summary>
    /// Check for winner
    /// </summary>
    /// <returns></returns>
    private bool HasWinner() =>  CheckHorizontalWin() || CheckVerticalWin() || CheckDiagonalWin();
    
    private Player GetCurrentPlayer() => CurrentPlayerId == PlayerOne.Id ? PlayerOne : PlayerTwo;
    
    /// <summary>
    /// 5 consecutive pebble in a horizontal pattern
    /// </summary>
    /// <returns></returns>
    private bool CheckHorizontalWin()
    {
        var currentPlayer = GetCurrentPlayer();
        for (int row = 0; row < BOARDSIZE; row++)
        {
            for (int col = 0; col <= BOARDSIZE - 5; col++)
            {
                
                if (GetCell(row,col).Color == currentPlayer.Color &&
                    GetCell(row, col + 1).Color  == currentPlayer.Color &&
                    GetCell(row, col + 2).Color  == currentPlayer.Color &&
                    GetCell(row, col + 3).Color  == currentPlayer.Color &&
                    GetCell(row, col + 4).Color  == currentPlayer.Color &&
                    currentPlayer.Color != Pebbles.Empty)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 5 consecutive pebbles in a vertical pattern
    /// </summary>
    /// <returns></returns>
    private bool CheckVerticalWin()
    {
        var currentPlayer = GetCurrentPlayer();
        for (int row = 0; row <= BOARDSIZE - 5; row++)
        {
            for (int col = 0; col < BOARDSIZE; col++)
            {
                if (GetCell(row,col).Color == currentPlayer.Color &&
                    GetCell(row + 1, col).Color  == currentPlayer.Color &&
                    GetCell(row+ 2, col).Color  == currentPlayer.Color &&
                    GetCell(row+ 3, col).Color  == currentPlayer.Color &&
                    GetCell(row+ 4, col).Color  == currentPlayer.Color &&
                    currentPlayer.Color != Pebbles.Empty)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// five diagonal pebbles
    /// </summary>
    /// <returns></returns>
    private bool CheckDiagonalWin()
    {
        var currentPlayer = GetCurrentPlayer();
        for (int row = 0; row <= BOARDSIZE - 5; row++)
        {
            for (int col = 0; col <= BOARDSIZE - 5; col++)
            {
                // Check diagonal from top-left to bottom-right
                if (GetCell(row,col).Color == currentPlayer.Color &&
                    GetCell(row + 1, col + 1).Color  == currentPlayer.Color &&
                    GetCell(row + 2, col + 2).Color == currentPlayer.Color &&
                    GetCell(row + 3, col + 3).Color  == currentPlayer.Color &&
                    GetCell(row + 4, col + 4).Color  == currentPlayer.Color &&
                    currentPlayer.Color != Pebbles.Empty)
                {
                    return true;
                }
             

                // Check diagonal from top-right to bottom-left
                if (GetCell(row,col + 4).Color == currentPlayer.Color &&
                    GetCell(row + 1, col + 3).Color  == currentPlayer.Color &&
                    GetCell(row + 2, col + 2).Color  == currentPlayer.Color &&
                    GetCell(row + 3, col + 1).Color  == currentPlayer.Color &&
                    GetCell(row + 4, col).Color  == currentPlayer.Color &&
                    currentPlayer.Color != Pebbles.Empty)
                {
                    return true;
                }
            }
        }
        return false;
    }
}