using Api.Model.People.Employees;
using Api.Model.People.Employees.Dto;
namespace Api.Mappers
{
    public static class CoachMapper
    {
        public static Coach MapToCoach(this CreateCoachRequest request)
        {
            return new Coach(
                name: request.Name,
                email: request.Email,
                phoneNumber: request.PhoneNumber,
                request.BankAccountNumber
            );
        }

         public static CoachResponseDto MapToCoachResponse(this Coach coach)
        {
            return new CoachResponseDto(
                coach.Id,
                coach.Name,
                coach.Email,
                coach.PhoneNumber!
            );
        }
    }
}
