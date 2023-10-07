using Gomoku.Application.Common;

namespace Gomoku.Application.Commands.AddPebble;

public class AddPebbleCommand : IRequest<CommonResult>
{
    public int Row { get; private set; }
    public int Column { get; private set; }
    public Guid GameId { get; private set; }
    public Guid PlayerId { get; private set; }
}