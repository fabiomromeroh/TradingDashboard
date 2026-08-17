using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task DeleteAsync(User user, CancellationToken cancellationToken);
    Task UpdateUserConfiguration(UserConfiguration userConfiguration, CancellationToken cancellationToken);
    Task<UserConfiguration?> GetUserConfigurationAsync(Guid userId, CancellationToken cancellationToken);
    Task<UserConfiguration> CreateUserConfigurationAsync(UserConfiguration userConfiguration, CancellationToken cancellationToken);
}
