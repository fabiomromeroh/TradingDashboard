using FluentValidation;

namespace TradingDashboard.Application.Features.Accounts.Commands.SetBrokerCredentials
{
    public class SetBrokerCredentialsValidator : AbstractValidator<SetBrokerCredentialsCommand>
    {
        public SetBrokerCredentialsValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Account ID is required.");

            RuleFor(x => x.BrokerCredentials).NotEmpty().WithMessage("Broker credentials are required.");
        }
    }
}
