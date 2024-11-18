using Api.Model.CourseSchedules;

namespace Api.Model.People.Employees
{
    public class Coach(string name, string email, string phoneNumber) : Employee(name, email, phoneNumber)
    {
        public ICollection<CourseSchedule> CourseSchedules { get; set; } = [];
    }
}