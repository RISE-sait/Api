using Api.Model.Courses;
using Api.Model.Courses.Dto;

namespace Api.Mappers
{
    public static class CourseMapper
    {
         public static Course MapToCourse(this CreateCourseDto createCourseDto)
        {
            return new Course(
                createCourseDto.Name,
                createCourseDto.StartDateTime,
                createCourseDto.EndDateTime
            );
        }
    }
}