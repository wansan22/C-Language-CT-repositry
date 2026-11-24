using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task<UserEntity> CreateUserAsync(UserEntity user);
        Task<UserEntity?> GetUserByLogin(string login);
    }
}
