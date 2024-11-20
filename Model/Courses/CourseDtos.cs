using System.ComponentModel.DataAnnotations;
using Api.Attributes;

namespace Api.Model.Courses
{
    public sealed record CreateCourseRequest(
        [StringLength(50, MinimumLength = 1)] string Name,
        string? Description,
        [ValidDateOnly] DateOnly StartDateTime,
        [ValidDateOnly] DateOnly EndDateTime
    );

    public sealed record UpdateCourseRequest(
        Guid Id,
        [StringLength(50, MinimumLength = 1)] string Name,
        [ValidDateOnly] DateOnly StartDateTime,
        [ValidDateOnly] DateOnly EndDateTime,
        string? Description 
        );

    public readonly record struct CourseResponseDto(
        Guid Id,
        string Name,
        string? Description,
        DateOnly StartDateTime,
        DateOnly EndDateTime
    );
}