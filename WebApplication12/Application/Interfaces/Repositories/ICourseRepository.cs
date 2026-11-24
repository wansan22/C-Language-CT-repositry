using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<CourseEntity> CreateCourseAsync(CourseEntity course);
        Task<bool> NameIsTakenAsync(string name);
    }
}
