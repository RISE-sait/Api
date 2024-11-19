using Api.Model.CourseSchedules;

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

        public static ScheduleDateTimes MapToCourseScheduleDateTimes(this CourseSchedule courseSchedule)
        {
            return new ScheduleDateTimes(
                courseSchedule.FacilityId,
                courseSchedule.Day,
                courseSchedule.StartDate,
                courseSchedule.EndDate,
                courseSchedule.BeginTime,
                courseSchedule.EndTime
            );
        }
    }
}