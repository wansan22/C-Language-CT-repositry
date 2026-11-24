namespace Domain.Entities
{
    public class SectionEntity
    {
        public  Guid Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public bool IsArchived { get; set; }

        public Guid CourseId { get; set; }
        public CourseEntity Course { get; set; }

        public List<LessonEntity> Lessons { get; set; }
    }
}
