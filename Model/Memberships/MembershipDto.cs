using System.ComponentModel.DataAnnotations;
using Api.Attributes;

namespace Api.Model.Memberships
{
    public sealed record CreateMembershipRequest(
        [StringLength(50, MinimumLength = 1)] string Name,
        string? Description,
        [ValidDateOnly] DateOnly StartDateTime,
        [ValidDateOnly] DateOnly EndDateTime
    );

    public sealed record UpdateMembershipRequest(
        Guid Id,
        [StringLength(50, MinimumLength = 1)] string Name,
        [ValidDateOnly] DateOnly StartDateTime,
        [ValidDateOnly] DateOnly EndDateTime,
        string? Description
        );

    public readonly record struct MembershipResponseDto(
        Guid Id,
        string Name,
        string? Description,
        DateOnly StartDateTime,
        DateOnly EndDateTime
    );
}