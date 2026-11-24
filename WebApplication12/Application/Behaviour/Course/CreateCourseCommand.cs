using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Course
{
    public class CreateCourseCommand : IRequest<CourseEntity>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; } = [];
        public Guid AuthorId { get; set; }
    }

    public class CreateCourseCommandHandler(
        ICourseRepository courseRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateCourseCommand, CourseEntity>
    {
        public async Task<CourseEntity> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            request.AuthorId = Guid.Parse("75a18735-0074-4678-93cb-eebd5c57fc18");
            bool userExists = await userRepository.ExistsAsync(request.AuthorId);
            if (!userExists)
                throw new Exception();

            bool nameIsTaken = await courseRepository.NameIsTakenAsync(request.Name);
            if (nameIsTaken)
                throw new Exception();

            CourseEntity course = new()
            {
                Id = Guid.NewGuid(),
                Sections = [],
                Reviews = [],
                IsArchived = false,
                Tags = request.Tags,
                Name = request.Name,
                AuthorId = request.AuthorId,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await courseRepository.CreateCourseAsync(course);
            await unitOfWork.SaveChangesAsync();

            return course;
        }
    }
}
