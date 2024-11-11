using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.enums;
using Api.Model.People.Employees;

namespace Api.Model.Courses
{
    public class CourseSchedule(Guid courseId, Guid facilityId, DaysInWeekEnum day, TimeOnly beginTime, TimeOnly endTime, Guid coachId)
    {
        
        [ForeignKey("Course")]
        public Guid CourseId { get; set; } = courseId;
        public Course Course { get; set; } = null!;

        public Coach Coach {get; set; } = null!;
        
        [ForeignKey("Coach")]
        public Guid CoachId { get; set; } = coachId;

        [ForeignKey("Facility")]
        public Guid FacilityId { get; set; } = facilityId;
        public Facility Facility {get; set; } = null!;
        public DaysInWeekEnum Day { get; set; } = day;

        public TimeOnly BeginTime { get; set; } = beginTime;
        public TimeOnly EndTime { get; set; } = endTime;
    }
}