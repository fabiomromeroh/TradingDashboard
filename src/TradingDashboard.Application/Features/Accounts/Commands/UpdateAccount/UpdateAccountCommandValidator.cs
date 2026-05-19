using FluentValidation;

namespace TradingDashboard.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Account ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Account name is required.")
            .MaximumLength(150).WithMessage("Account name cannot exceed 150 characters.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter code (e.g. USD).")
            .Matches(@"^[A-Z]{3}$").WithMessage("Currency must be uppercase letters only.");

        RuleFor(x => x.InitialBalance)
            .GreaterThanOrEqualTo(0).WithMessage("Initial balance cannot be negative.");
    }
}
