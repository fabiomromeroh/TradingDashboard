using FluentValidation;

namespace TradingDashboard.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Account name is required.")
            .MaximumLength(150).WithMessage("Account name cannot exceed 150 characters.");

        RuleFor(x => x.ImportSourceType)
            .NotEmpty().WithMessage("Type is required.");

        RuleFor(x => x.BrokerId)
            .NotEmpty().WithMessage("Broker ID is required.");
    }
}
