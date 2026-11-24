using System.Linq.Expressions;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.Persistence;
using Domain.Entities;
using Domain.Models;
using MediatR;

namespace Application.Behaviour.Authorization
{
    public class RefreshCommand : IRequest<AccessPair?>
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string? UserAgent { get; set; }
    }

    public class RefreshCommandHandler(
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<RefreshCommand, AccessPair?>
    {
        public async Task<AccessPair?> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            string encryptedToken = tokenService.Encrypt(request.RefreshToken);

            RefreshTockensEntity? existingToken = await refreshTokenRepository.GetByTokenAsync(encryptedToken);

            if (existingToken == null
                || existingToken.User == null
                || existingToken.IsUsed
                || existingToken.IsExpired
                || existingToken.ExpiresTime <= DateTime.UtcNow)
            {
                return null;
            }

            
             if (!string.Equals(existingToken.UserAgent, request.UserAgent, StringComparison.Ordinal))
            {
            existingToken.IsExpired = true;
            await refreshTokenRepository.UpdateAsync(existingToken);
            throw new UnauthorizedAccessException("UserAgent mismatch on refresh token.");
            }

        
            

        
            existingToken.IsExpired = true;
            existingToken.IsUsed = true;
            await refreshTokenRepository.UpdateAsync(existingToken);

        
            AccessPair newPair = tokenService.GenerateTokens(existingToken.User);

            var newRefreshTokenEntity = new RefreshTockensEntity
            {
                Id = Guid.NewGuid(),
                UserId = existingToken.UserId,
                EncryptedToken = tokenService.Encrypt(newPair.RefreshToken),
                ExpiresTime = tokenService.GetRefreshTokenExpiry(),
                UserAgent = request.UserAgent,
                IsUsed = false,
                IsExpired = false
            };

            await refreshTokenRepository.AddAsync(newRefreshTokenEntity);

            return newPair;
        }
    }
}