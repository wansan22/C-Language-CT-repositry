using Application.Interfaces.Infrastructure;
using Domain.Entities;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly int _refreshTokenDays;
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenMinutes;
        public TokenService(IConfiguration configuration)
        {
            _secret = configuration.GetSection("JwtSettings").GetSection("Secret").Value;
            _issuer = configuration.GetSection("JwtSettings").GetSection("Issuer").Value;
            _audience = configuration.GetSection("JwtSettings").GetSection("Audience").Value;
            _accessTokenMinutes = int.Parse(configuration.GetSection("JwtSettings").GetSection("AccessTokenMinutes").Value);
            _refreshTokenDays = int.Parse(configuration.GetSection("JwtSettings").GetSection("RefreshTokenDays").Value!);
        }
        public AccessPair GenerateTokens(UserEntity user)
        {
            string accessToken = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken();
            return new AccessPair() { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        private string GenerateAccessToken(UserEntity user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secret));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_accessTokenMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
        public DateTime GetRefreshTokenExpiry()
    {
        return DateTime.UtcNow.AddDays(_refreshTokenDays);
    }
   public string Encrypt(string token)
    {
    byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(_secret));
    byte[] iv = new byte[16];

    using var aes = Aes.Create();
    aes.Key = key;
    aes.IV = iv;

    using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
    byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
    byte[] encryptedBytes = encryptor.TransformFinalBlock(tokenBytes, 0, tokenBytes.Length);

    return Convert.ToBase64String(encryptedBytes);
    }
}

}