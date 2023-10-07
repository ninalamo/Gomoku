using FluentValidation;
using FluentValidation.Results;
using Gomoku.Application.Commands.AddGame;
using Gomoku.Application.Commands.UpdateNextPlayer;
using Gomoku.Application.Common;
using Gomoku.Domain.AggregatesModel;
using Microsoft.Extensions.Logging;

namespace Gomoku.Application.Commands.AddPebble;

public class AddPebbleCommandHandler : IRequestHandler<AddPebbleCommand,CommonResult>
{
    private readonly IGamesRepository _repository;
    private readonly ILogger<AddPebbleCommandHandler> _logger;
    private readonly IMediator _mediator;

    public AddPebbleCommandHandler(IGamesRepository repository, ILogger<AddPebbleCommandHandler> logger, IMediator mediator)
    {
        _repository = repository;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<CommonResult> Handle(AddPebbleCommand request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetAsync(request.GameId);
        var failures = new List<ValidationFailure>();
        
        if (game.CurrentPlayerId != request.PlayerId) 
            failures.Add(new ValidationFailure(nameof(request.PlayerId), $"PlayerId do not match {nameof(game.CurrentPlayerId)}."));
        
        //check cell 
        if (!game.IsCellEmpty(request.Row, request.Column))
        {
            failures.Add(new ValidationFailure(nameof(request.Row), $"Cell is not empty."));
            failures.Add(new ValidationFailure(nameof(request.Column), $"Cell is not empty."));
        }
        
        if(failures.Any()) throw new ValidationException($"{nameof(AddPebbleCommandHandler)} validation error", failures);

        game.AddPebble(request.Row, request.Column, request.PlayerId);

        _repository.Update(game);
        
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        if (game.IsGameOver())
        {
            return CommonResult.Create(new
            {
                Winner = game.GetWinner(),
                Board = game.Cells,
                PlayerTurn = "",
                GameId = game.Id,
                game.Players
            });
        }

        var nextPlayer = await _mediator.Send(new UpdateGameCurrentPlayerCommand(game.Id));//game.GetNextPlayer();
      
        return CommonResult.Create(new
        {
            Winner = game.GetWinner(),
            Board = game.Cells,
            PlayerTurn = nextPlayer,
            GameId = game.Id,
            game.Players
        });

    }
}