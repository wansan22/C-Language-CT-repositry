using Application.Behaviour.Course;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dto.Course;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("/api/course")]
    public class CourseController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<CourseDto> CreateCourse([FromBody] CourseToCreateOrUpdateDto dto)
        {
            CreateCourseCommand command = new()
            {
                Name = dto.Name,
                Description = dto.Description,
                Tags = dto.Tags,
                AuthorId = Guid.Parse(User.FindFirstValue("sub")!)
            };

            CourseEntity result = await mediator.Send(command);

            return result.Adapt<CourseDto>();
        }
    }

}
