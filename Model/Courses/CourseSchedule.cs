using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.enums;
using Api.Model.People.Employees;

namespace Api.Model.Courses
{
    public class CourseSchedule(Guid courseId, string location, DaysInWeekEnum day, TimeOnly beginTime, TimeOnly endTime, Guid coachId)
    {
        
        [ForeignKey("Course")]
        public Guid CourseId { get; set; } = courseId;
        public Course Course { get; set; } = null!;

        public Coach Coach {get; set; } = null!;
        
        [ForeignKey("Coach")]
        public Guid CoachId { get; set; } = coachId;

        [Length(1, 50)]
        public string Location { get; set; } = location;
        public DaysInWeekEnum Day { get; set; } = day;

        public TimeOnly BeginTime { get; set; } = beginTime;
        public TimeOnly EndTime { get; set; } = endTime;
    }
}