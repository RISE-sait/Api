using Api.Database;
using Api.enums;
using Api.helpers;
using Api.Model.Courses;

namespace Api.Interfaces
{
    public interface ICoach
    {
        void AddCustomerToTeam(Guid customerId, Guid teamId)
        {
            throw new NotImplementedException();
        }

        void RemoveCustomerFromTeam(Guid customerId, Guid teamId)
        {
            throw new NotImplementedException();
        }

        async Task AddCourseSchedule(AppDbContext context, CourseSchedule courseSchedule)
        {
            if (await ScheduleHelper.IsFacilityScheduleOverlapping(context, courseSchedule))
            {
                throw new InvalidOperationException("The course schedule overlaps with an existing schedule.");
            }

            context.CourseSchedules.Add(courseSchedule);
            await context.SaveChangesAsync();
        }

        async Task AmendCourseSchedule(AppDbContext context, ScheduleInfo newSchedule)
        {
            if (await ScheduleHelper.IsFacilityScheduleOverlapping(context, newSchedule))
            {
                throw new InvalidOperationException("The course schedule overlaps with an existing schedule.");
            }

            var courseSchedule = await context.CourseSchedules.FindAsync(newSchedule.StartDate, newSchedule.FacilityId, newSchedule.BeginTime);

            if (courseSchedule != null)
            {
                courseSchedule.Day = newSchedule.Day;
                courseSchedule.StartDate = newSchedule.StartDate;
                courseSchedule.EndDate = newSchedule.EndDate;
                courseSchedule.BeginTime = newSchedule.BeginTime;
                courseSchedule.EndTime = newSchedule.EndTime;

                await context.SaveChangesAsync();
            }
        }
    }

    public struct ScheduleInfo
    {
        public Guid FacilityId;
        public DaysInWeekEnum Day;
        public DateOnly StartDate;
        public DateOnly EndDate;
        public TimeOnly BeginTime;
        public TimeOnly EndTime;
    }
}