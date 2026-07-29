using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.BrokerSync
{
    public class BrokerAccountCredentialService : IBrokerAccountCredentialService
    {
        private readonly IBrokerAccountCredentialRepository brokerAccountCredentialRepository;
        private readonly IDataProtector dataProtector;

        public BrokerAccountCredentialService(IBrokerAccountCredentialRepository brokerAccountCredentialRepository, IDataProtectionProvider dataProtectionProvider)
        {
            this.brokerAccountCredentialRepository = brokerAccountCredentialRepository;
            this.dataProtector = dataProtectionProvider.CreateProtector("BrokerCredentials");
        }
        public async Task<TCredentials?> GetAsync<TCredentials>(Guid accountId, CancellationToken ct) where TCredentials : BrokerCredentials
        {
            var entity = await brokerAccountCredentialRepository.GetAsync(accountId, ct);
            if (entity == null)
            {
                return null;
            }

            var decryptedCredentials = dataProtector.Unprotect(entity.EncryptedPayload);
            return JsonSerializer.Deserialize<TCredentials>(decryptedCredentials);
        }

        public async Task CreateAsync(Guid accountId, string brokerName, object credentials, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(credentials);
            var encrypted = dataProtector.Protect(json);
            var entity = BrokerAccountCredential.Create(accountId, encrypted);
            await brokerAccountCredentialRepository.AddAsync(entity, ct);
        }

        public async Task UpdateAsync(BrokerAccountCredential brokerCredentials, object credentials, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(credentials);
            var encrypted = dataProtector.Protect(json);
            var updatedBrokerCredentials = brokerCredentials.UpdateEncryptedPayload(encrypted);
            await brokerAccountCredentialRepository.UpdateAsync(updatedBrokerCredentials, ct);
        }
    }
}
