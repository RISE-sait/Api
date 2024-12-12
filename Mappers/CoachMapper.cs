using Api.Model.People.Staff;

namespace Api.Mappers
{
    public static class CoachMapper
    {
        public static Staff MapToCoach(this CreateStaffRequest request)
        {
            return new Staff(
                name: request.Name,
                email: request.Email,
                phoneNumber: request.PhoneNumber,
                request.StaffTypeId
            );
        }
        
         public static StaffResponseDto MapToCoachResponse(this Staff coach)
        {
            return new StaffResponseDto(
                coach.Id,
                coach.Name,
                coach.Email,
                coach.PhoneNumber!
            );
        }
    }
}
