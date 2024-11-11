using Api.Interfaces;
using Api.Model.Courses;

namespace Api.Model.People.Employees
{
    public class Coach(string name, string email, string phoneNumber, string bankAccountNumber) : Employee(name, email, phoneNumber, bankAccountNumber), ICoach
    {
        public ICollection<CourseSchedule> CourseSchedules { get; set; } = [];
    }
}