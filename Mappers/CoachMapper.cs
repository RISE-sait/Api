using Api.Model.People.Employees;
using Api.Model.People.Employees.Dto;
namespace Api.Mappers
{
    public static class CoachMapper
    {
        public static Coach MapToCoach(this CreateCoachDto createCoachDto)
        {
            return new Coach(
                name: createCoachDto.Name,
                email: createCoachDto.Email,
                phoneNumber: createCoachDto.PhoneNumber
            );
        }
    }
}
