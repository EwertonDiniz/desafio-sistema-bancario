
using FluentValidation;

namespace ContaCorrenteAPI.Application.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.NumeroConta).NotEmpty().Matches("^\\d+$").WithMessage("Número da conta deve ser numérico.");
            RuleFor(x => x.Senha).NotEmpty().MinimumLength(6);
        }
    }
}
