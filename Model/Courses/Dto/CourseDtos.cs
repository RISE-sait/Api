using System.ComponentModel.DataAnnotations;
using Api.Attributes;

namespace Api.Model.Courses.Dto
{
    public sealed record CreateCourseDto(
        [StringLength(50, MinimumLength = 1)] string Name,
        string? Description,
        [ValidDateOnly] DateOnly StartDateTime,
        [ValidDateOnly] DateOnly EndDateTime
    );

    public sealed record UpdateCourseDto(
        [Guid] Guid Id,
        [StringLength(50, MinimumLength = 1)] string Name,
        [ValidDateOnly] DateOnly StartDateTime,
        [ValidDateOnly] DateOnly EndDateTime,
        string? Description 
        );

    public sealed record CourseResponseDto(
        [Guid] Guid Id,
        [StringLength(50, MinimumLength = 1)] string Name,
        string? Description,
        [ValidDateOnly] DateOnly StartDateTime,
        [ValidDateOnly] DateOnly EndDateTime
    );
}