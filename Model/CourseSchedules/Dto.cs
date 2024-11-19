using Api.Attributes;
using Api.enums;

namespace Api.Model.CourseSchedules
{
    public readonly record struct CourseScheduleResponseDto(
           Guid CourseId,
           Guid CoachId,
           Guid FacilityId,
           DaysInWeekEnum Day,
           [ValidDateOnly] DateOnly StartDate,
           [ValidDateOnly] DateOnly EndDate,
           TimeOnly BeginTime,
           TimeOnly EndTime
        );

    public sealed record CreateCourseScheduleDto(
       Guid CourseId,
       Guid FacilityId,
       DaysInWeekEnum Day,
       DateOnly StartDate,
       [ValidDateOnly] DateOnly EndDate,
       TimeOnly BeginTime,
       TimeOnly EndTime,
       Guid CoachId
   );
}