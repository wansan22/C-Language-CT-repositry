namespace Domain.Entities
{
    public class ReviewEntity
    {
        public Guid Id { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Text { get; set; }

        public Guid AuthorId { get; set; }
        public UserEntity Author { get; set; }

        public Guid CourseId { get; set; }
        public CourseEntity Course { get; set; }
    }
}
