using FluentValidation;

namespace Gomoku.Application.Commands.AddGame;

public class AddGameCommandValidator : AbstractValidator<AddGameCommand>
{
    public AddGameCommandValidator()
    {
        RuleFor(i => i.PlayerOne).NotEmpty().NotEqual(i => i.PlayerTwo);
        RuleFor(i => i.PlayerTwo).NotEmpty().NotEqual(i => i.PlayerOne);
    }   
}