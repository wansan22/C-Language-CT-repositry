using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UserRepository
        (ApplicationContext context): IUserRepository
    {
        public async Task<UserEntity> CreateUserAsync(UserEntity user)
        {
            await context.Users.AddAsync(user);
            return user;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return context.Users.AnyAsync(user => user.Id == id);
        }

        public Task<UserEntity?> GetUserByLogin(string login)
        {
            return context.Users
                          .Where(user => user.Login == login)
                          .FirstOrDefaultAsync();
        }
    }
}
