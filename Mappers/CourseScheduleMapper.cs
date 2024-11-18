using Api.Model.Courses;
using Api.Model.CourseSchedules;
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

        public static ScheduleInfo MapToScheduleInfo(this CourseSchedule courseSchedule)
        {
            return new ScheduleInfo() {
                BeginTime = courseSchedule.BeginTime,
                Day = courseSchedule.Day,
                EndDate = courseSchedule.EndDate,
                EndTime = courseSchedule.EndTime,
                FacilityId = courseSchedule.FacilityId,
                StartDate = courseSchedule.StartDate
            };
        }
    }
}