using FluentValidation;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.Trades.Commands.CreateTrade;

public class CreateTradeCommandValidator : AbstractValidator<CreateTradeCommand>
{
    public CreateTradeCommandValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty()
            .WithMessage("Symbol is required")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Symbol must be alphanumeric uppercase")
            .MaximumLength(20)
            .WithMessage("Symbol cannot exceed 20 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.EntryPrice)
            .GreaterThan(0)
            .WithMessage("Entry Price must be greater than 0");

        // Model binding already validates enum format,
        // so we only need to ensure it's one of the valid enum values
        RuleFor(x => x.Direction)
            .IsInEnum()
            .WithMessage("Direction must be either 'Long' or 'Short'");
    }
}
