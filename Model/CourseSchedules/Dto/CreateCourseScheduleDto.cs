using System.ComponentModel.DataAnnotations;
using Api.enums;

namespace Api.Model.CourseSchedules.Dto
{
    public struct CreateCourseScheduleDto
    {
        [Required]
        public Guid CourseId { get; set; }

        [Required]
        public Guid FacilityId { get; set; }

        [Required]
        public DaysInWeekEnum Day { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        public TimeOnly BeginTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public Guid CoachId { get; set; }
    }
}