using Gomoku.Application.Common;

namespace Gomoku.Application.Commands.AddPebble;

public class AddPebbleCommand : IRequest<CommonResult>
{
    public AddPebbleCommand(int row, int column, Guid gameId, Guid playerId)
    {
        Row = row;
        Column = column;
        GameId = gameId;
        PlayerId = playerId;
    }
    public int Row { get; private set; }
    public int Column { get; private set; }
    public Guid GameId { get; private set; }
    public Guid PlayerId { get; private set; }
}