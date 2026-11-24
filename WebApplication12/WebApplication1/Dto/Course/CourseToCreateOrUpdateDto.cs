namespace WebApi.Dto.Course
{
    public class CourseToCreateOrUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; } = [];
    }

}
