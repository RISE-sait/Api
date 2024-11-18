using System.ComponentModel.DataAnnotations;
using Api.enums;

namespace Api.Model.CourseSchedules.Dto
{
    public readonly record struct CreateCourseScheduleDto(
        [param: Required] Guid CourseId,
        [param: Required] Guid FacilityId,
        [param: Required] DaysInWeekEnum Day,
        [param: Required] DateOnly StartDate,
        [param: Required] DateOnly EndDate,
        [param: Required] TimeOnly BeginTime,
        [param: Required] TimeOnly EndTime,
        [param: Required] Guid CoachId
    );
}