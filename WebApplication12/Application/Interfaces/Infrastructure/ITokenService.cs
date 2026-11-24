    using Domain.Entities;
    using Domain.Models;

    namespace Application.Interfaces.Infrastructure
    {
        public interface ITokenService
        {
           public AccessPair GenerateTokens(UserEntity user);
           public DateTime GetRefreshTokenExpiry();
            public string Encrypt(string token);

        }
    }
