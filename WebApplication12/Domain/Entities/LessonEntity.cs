namespace Domain.Entities
{
    public class LessonEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Content { get; set; }

        public Guid SectionId { get; set; }
        public SectionEntity Section { get; set; }
    }
}
