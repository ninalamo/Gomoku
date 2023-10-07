using Gomoku.Application.Common;

namespace Gomoku.Application.Commands.UpdateNextPlayer;

public class UpdateGameCurrentPlayerCommand : IRequest<UpdateGameCurrentPlayerCommandResult>
{
    public Guid GameId { get; private set; }

    public UpdateGameCurrentPlayerCommand(Guid gameID)
    {
        GameId = gameID;
    }
    
}

public record UpdateGameCurrentPlayerCommandResult
{
    public Guid GameId { get; init; }
    public Guid PlayerId { get; init; }
    public string Name { get; init; }
    public int Color { get; init; }
}