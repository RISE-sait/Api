using Api.Model.Courses;
using Api.Model.Courses.Dto;
using Api.Model.CourseSchedules.Dto;

namespace Api.Mappers
{
    public static class CourseScheduleMapper
    {
         public static CourseSchedule MapToCourseSchedule(this CreateCourseScheduleDto createCourseScheduleDto)
        {
            return new CourseSchedule(
                createCourseScheduleDto.CourseId,
                createCourseScheduleDto.FacilityId,
                createCourseScheduleDto.Day,
                createCourseScheduleDto.StartDate,
                createCourseScheduleDto.EndDate,
                createCourseScheduleDto.BeginTime,
                createCourseScheduleDto.EndTime,
                createCourseScheduleDto.CoachId
            );
        }
    }
}