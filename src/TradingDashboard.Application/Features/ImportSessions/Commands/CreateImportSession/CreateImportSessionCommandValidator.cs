using FluentValidation;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.CreateImportSession;

public class CreateImportSessionCommandValidator : AbstractValidator<CreateImportSessionCommand>
{
    public CreateImportSessionCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.")
            .MaximumLength(500).WithMessage("File name cannot exceed 500 characters.");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required.");
    }
}
