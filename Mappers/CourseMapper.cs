using Api.Model.Courses;

namespace Api.Mappers
{
    public static class CourseMapper
    {
         public static Course MapToCourse(this CreateCourseRequest request)
        {
            return new Course(
                request.Name,
                request.StartDateTime,
                request.EndDateTime,
                request.Description
            );
        }

         public static CourseResponseDto MapToCourseResponse (this Course course)
        {
            return new CourseResponseDto(
                course.Id,
                course.Name,
                course.Description,
                course.StartDateTime,
                course.EndDateTime
            );
        }
    }
}