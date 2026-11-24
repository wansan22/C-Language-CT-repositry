namespace Domain.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsArchived { get; set; }
        
        public string Role { get; set; } 
        public List<CourseEntity> SubscribedAt { get; set; } = [];
        public List<ReviewEntity> Reviews { get; set; } = [];
    }
}   
