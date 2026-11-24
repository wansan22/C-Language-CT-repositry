using Application.Interfaces.Repositories;
using Domain.Entities;
using Mapster;
using MediatR;
using Hasher = BCrypt.Net.BCrypt;

namespace Application.Behaviour.Authorization
{
    public class RegisterCommand : IRequest<UserEntity>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class RegisterCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, UserEntity>
    {
        public async Task<UserEntity> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            UserEntity user = request.Adapt<UserEntity>();
            user.Role = "User";
            string hash = Hasher.HashPassword(request.Password);
            user.PasswordHash = hash;

            await userRepository.CreateUserAsync(user);
            await unitOfWork.SaveChangesAsync();

            return user;
        }
    }
}
