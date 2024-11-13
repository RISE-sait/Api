using System.ComponentModel.DataAnnotations;

namespace Api.Model.Courses.Dto
{
    public struct CreateCourseDto(string name, DateOnly startDateTime, DateOnly endDateTime)
    {
        [Length(1, 50)]
        public string Name { get; set; } = name;
        public string Description { get; set; } = string.Empty;
        public DateOnly StartDateTime { get; set; } = startDateTime;
        public DateOnly EndDateTime { get; set; } = endDateTime;
    }
}