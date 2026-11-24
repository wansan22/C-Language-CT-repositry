using Domain.Entities;

namespace WebApi.Dto.Course
{
    public class CourseDto
    {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<string> Tags { get; set; } = [];
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public bool IsArchived { get; set; }

            public List<ReviewEntity> Reviews { get; set; } = [];
            public List<SectionEntity> Sections { get; set; } = [];

            public Guid AuthorId { get; set; }
            public UserEntity Author { get; set; }
    }

}
