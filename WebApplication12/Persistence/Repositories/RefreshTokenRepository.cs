using Application.Interfaces.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationContext _context;

        public RefreshTokenRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshTockensEntity refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshTockensEntity?> GetByTokenAsync(string encryptedToken)
        {
            return await _context.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.EncryptedToken == encryptedToken);
        }
    public async Task<IEnumerable<RefreshTockensEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
{
    return await _context.RefreshTokens 
        .Where(x => x.UserId == userId)
        .ToListAsync(cancellationToken);
}
        public async Task UpdateAsync(RefreshTockensEntity refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}