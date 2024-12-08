// using Api.Model.Memberships;
//
// namespace Api.Mappers
// {
//     public static class MembershipMapper
//     {
//          public static Membership MapToMembership(this CreateMembershipRequest request)
//         {
//             return new Membership(
//                 request.Name,
//                 request.StartDateTime,
//                 request.EndDateTime,
//                 request.Price,
//                 request.Description
//             );
//         }
//
//          public static MembershipResponseDto MapToMembershipResponse (this Membership membership)
//         {
//             return new MembershipResponseDto(
//                 membership.Id,
//                 membership.Name,
//                 membership.Description,
//                 membership.Price,
//                 membership.StartDateTime,
//                 membership.EndDateTime
//             );
//         }
//     }
// }