using System.Text.Json.Serialization;
using TradingDashboard.Application.Abstractions.Services.BrokerSync.Ibkr;

namespace TradingDashboard.Application.Abstractions.Services.BrokerSync
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "BrokerName")]
    [JsonDerivedType(typeof(IbkrFlexCredentials), "Interactive Brokers")]
    public abstract record BrokerCredentials;

}
