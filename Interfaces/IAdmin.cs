using Api.Database;
using Api.enums;
using Api.helpers;
using Api.Model.Courses;

namespace Api.Interfaces
{
    public interface IAdmin
    {
        void ManageFacilities() {

        }

        void ManageCourses() {
            
        }

        void ManageCustomers() {
            
        }

        async Task AddCourseSchedule(AppDbContext context, CourseSchedule courseSchedule)
        {
            if (await ScheduleHelper.IsScheduleOverlapping(context, courseSchedule))
            {
                throw new InvalidOperationException("The course schedule overlaps with an existing schedule.");
            }

            context.CourseSchedules.Add(courseSchedule);
            await context.SaveChangesAsync();
        }

        void AmendCourseSchedule(Guid courseId, ScheduleDayTime[] newSchedule)
        {
            throw new NotImplementedException();
        }
    }

    
    public struct ScheduleDayTime
    {
        public DaysInWeekEnum Day;
        public TimeOnly StartTime;
        public TimeOnly EndTime;
    }
}