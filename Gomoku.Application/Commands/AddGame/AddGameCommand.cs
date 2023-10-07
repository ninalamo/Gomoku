using Gomoku.Application.Common;

namespace Gomoku.Application.Commands.AddGame;

public class AddGameCommand : IRequest<AddGameCommandResult>
{
    public AddGameCommand(string playerOne, string playerTwo)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
    }
    
    public string PlayerOne { get; private set; }
    public string PlayerTwo { get; private set; }
}