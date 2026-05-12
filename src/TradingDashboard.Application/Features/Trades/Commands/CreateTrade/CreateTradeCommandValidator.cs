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
            .MaximumLength(5)
            .WithMessage("Symbol cannot exceed 5 characters");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is required")
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.EntryPrice)
            .NotEmpty()
            .WithMessage("Entry Price is required")
            .GreaterThan(0)
            .WithMessage("Entry Price must be greater than 0");

        // Model binding already validates enum format,
        // so we only need to ensure it's one of the valid enum values
        RuleFor(x => x.Direction)
            .IsInEnum()
            .WithMessage("Direction must be either 'Long' or 'Short'");
    }
}
