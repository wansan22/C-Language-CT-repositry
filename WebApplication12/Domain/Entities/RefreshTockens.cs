using System.Runtime.CompilerServices;

namespace Domain.Entities
{
    public class RefreshTockensEntity
    {
        public Guid Id {get; set;}
        public string? EncryptedToken {get; set;}
        public string? UserAgent {get; set;}
        public DateTime ExpiresTime {get; set;}
        public bool IsUsed {get; set;}
        public bool IsExpired {get; set;}
        public Guid UserId {get; set;}
        public UserEntity? User {get; set;} = null;
    }
}