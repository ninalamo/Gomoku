using Gomoku.Application.Common;
using Gomoku.Domain.AggregatesModel;
using Microsoft.Extensions.Logging;

namespace Gomoku.Application.Commands.AddGame;

public class AddGameCommandHandler : IRequestHandler<AddGameCommand,AddGameCommandResult>
{
    private readonly IGamesRepository _repository;
    private readonly ILogger<AddGameCommandHandler> _logger;

    public AddGameCommandHandler(IGamesRepository repository, ILogger<AddGameCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    
    public async Task<AddGameCommandResult> Handle(AddGameCommand request, CancellationToken cancellationToken)
    {
        Game game = new(request.PlayerOne, request.PlayerTwo);

        _repository.Create(game);

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return new AddGameCommandResult()
        {
            GameId = game.Id,
            Cells = game.Cells.Select(x => new CellDto()
            {
                Row = x.Row,
                Column = x.Column,
                Color = (int)x.Color
            })
        };
    }
}