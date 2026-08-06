using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Accounts.Commands.SetBrokerCredentials
{
    public record SetBrokerCredentialsCommand : IRequest<Result>
    {
        public Guid AccountId { get; set; }
        public JsonElement BrokerCredentials { get; set; }
    }
}
