using Application.Interfaces.Infrastructure;
using Application.Interfaces.Persistence;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Hasher = BCrypt.Net.BCrypt;

namespace Application.Behaviour.Authorization
{
    public class LoginCommand : IRequest<AccessPair?>
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? UserAgent { get; set; }
    }

    public class LoginCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<LoginCommand, AccessPair?>
    {
        public async Task<AccessPair?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            UserEntity? user = await userRepository.GetUserByLogin(request.Login);

            if (user == null || !Hasher.Verify(request.Password, user.PasswordHash))
                return null;

            AccessPair pair = tokenService.GenerateTokens(user);

            var refreshTokenEntity = new RefreshTockensEntity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                EncryptedToken = tokenService.Encrypt(pair.RefreshToken),
                ExpiresTime = tokenService.GetRefreshTokenExpiry(),
                UserAgent = request.UserAgent,
                IsUsed = false,
                IsExpired = false
            };

            await refreshTokenRepository.AddAsync(refreshTokenEntity);

            return pair;
        }
    }
}