using Api.Model.CourseSchedules;

namespace Api.Mappers
{
    public static class CourseScheduleMapper
    {
        public static CourseSchedule MapToCourseSchedule(this CreateCourseScheduleRequest request)
        {
            return new CourseSchedule(
                request.CourseId,
                request.FacilityId,
                request.Day,
                request.StartDate,
                request.EndDate,
                request.BeginTime,
                request.EndTime,
                request.CoachId
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

        public static CourseScheduleResponseDto MapToCourseScheduleResponse(this CourseSchedule courseSchedule)
        {
            return new CourseScheduleResponseDto(
                courseSchedule.CourseId,
                courseSchedule.CoachId,
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