using System.ComponentModel.DataAnnotations.Schema;
using Api.enums;
using Api.Model.Courses;
using Api.Model.People.Staff;

namespace Api.Model.CourseSchedules
{
    public class CourseSchedule(Guid courseId, Guid facilityId, DaysInWeekEnum day, DateOnly startDate, DateOnly endDate, TimeOnly beginTime, TimeOnly endTime, Guid coachId)
    {
        [ForeignKey("Course")]
        public Guid CourseId { get; set; } = courseId;
        public Course Course { get; set; } = null!;

        public Staff Coach { get; set; } = null!;

        [ForeignKey("Coach")]
        public Guid CoachId { get; set; } = coachId;

        [ForeignKey("Facility")]
        public Guid FacilityId { get; set; } = facilityId;
        // public Facility Facility { get; set; } = null!;
        public DaysInWeekEnum Day { get; set; } = day;
        public DateOnly StartDate { get; set; } = startDate;
        public DateOnly EndDate { get; set; } = endDate;
        public TimeOnly BeginTime { get; set; } = beginTime;
        public TimeOnly EndTime { get; set; } = endTime;
    }

    public readonly record struct ScheduleDateTimes(
    Guid FacilityId,
    DaysInWeekEnum Day,
    DateOnly StartDate, 
    DateOnly EndDate,
    TimeOnly BeginTime,
    TimeOnly EndTime
);
}