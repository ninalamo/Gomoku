using FluentValidation;
using FluentValidation.Results;
using Gomoku.Application.Common;
using Gomoku.Domain.AggregatesModel;
using Microsoft.Extensions.Logging;

namespace Gomoku.Application.Commands.UpdateNextPlayer;

public class UpdateGameCurrentPlayerCommandHandler : IRequestHandler<UpdateGameCurrentPlayerCommand, UpdateGameCurrentPlayerCommandResult>
{
    private readonly ILogger<UpdateGameCurrentPlayerCommandHandler> _logger;
    private readonly IGamesRepository _repository;

    public UpdateGameCurrentPlayerCommandHandler(ILogger<UpdateGameCurrentPlayerCommandHandler> logger, IGamesRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<UpdateGameCurrentPlayerCommandResult> Handle(UpdateGameCurrentPlayerCommand request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetAsync(request.GameId);

        var currentId = game.CurrentPlayerId;
        
        var player = game.GetNextPlayer();
        
        //do not allow two-simultaneous update from the same player
        if(currentId == player.Id)
            throw new ValidationException($"{nameof(UpdateGameCurrentPlayerCommandHandler)} validation error", new[]
            {
                new ValidationFailure(nameof(game.CurrentPlayerId), "Turn already finished.")
            });

        _repository.Update(game);

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateGameCurrentPlayerCommandResult()
        {
            Color = (int)player.Color,
            GameId = game.Id,
            Name = player.Name,
            PlayerId = player.Id
        };

    }
}