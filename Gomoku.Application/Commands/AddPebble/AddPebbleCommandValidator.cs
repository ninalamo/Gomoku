using FluentValidation;
using Gomoku.Domain.AggregatesModel;

namespace Gomoku.Application.Commands.AddPebble;

public class AddPebbleCommandValidator : AbstractValidator<AddPebbleCommand>
{
    public AddPebbleCommandValidator()
    {
        RuleFor(i => i.Row >= 0 && i.Row < Game.BOARDSIZE);
        RuleFor(i => i.Column >= 0 && i.Column < Game.BOARDSIZE);
        RuleFor(i => i.GameId).NotEmpty();
        RuleFor(i => i.PlayerId).NotEmpty();
    }
}