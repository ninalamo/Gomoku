using FluentValidation;

namespace Gomoku.Application.Commands.UpdateNextPlayer;

public class UpdateGameCurrentPlayerCommandValidator : AbstractValidator<UpdateGameCurrentPlayerCommand>
{
    public UpdateGameCurrentPlayerCommandValidator()
    {
        RuleFor(i => i.GameId).NotEmpty();
    }
}