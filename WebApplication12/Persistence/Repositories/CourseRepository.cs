using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    internal class CourseRepository(
        ApplicationContext context) : ICourseRepository
    {
        public async Task<CourseEntity> CreateCourseAsync(CourseEntity course)
        {
            await context.Courses.AddAsync(course);

            return course;
        }

        public Task<bool> NameIsTakenAsync(string name)
        {
            return context.Courses.AnyAsync(course => course.Name == name);
        }
    }
}
