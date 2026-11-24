using Domain.Entities;

namespace Application.Interfaces.Persistence
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshTockensEntity refreshToken);
        Task<RefreshTockensEntity?> GetByTokenAsync(string encryptedToken);
        Task UpdateAsync(RefreshTockensEntity refreshToken);
        Task<IEnumerable<RefreshTockensEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}