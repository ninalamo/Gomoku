using Gomoku.Application.Common;

namespace Gomoku.Application.Commands.AddGame;

public record AddGameCommandResult
{
    public Guid GameId { get; init; }
    public IEnumerable<CellDto> Cells { get; init; }
}