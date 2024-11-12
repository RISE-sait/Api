using Api.Database;
using Api.enums;
using Api.Interfaces;
using Api.Model.Courses;
using Microsoft.EntityFrameworkCore;

namespace Api.helpers
{
    public static class ScheduleHelper
    {
        public static Task<bool> IsFacilityScheduleOverlapping(AppDbContext context, CourseSchedule courseSchedule)
        {
            return IsOverlapping(context, courseSchedule.FacilityId, courseSchedule.Day, courseSchedule.StartDate, courseSchedule.EndDate, courseSchedule.BeginTime, courseSchedule.EndTime);
        }

        public static Task<bool> IsFacilityScheduleOverlapping(AppDbContext context, ScheduleInfo scheduleInfo)
        {
            return IsOverlapping(context, scheduleInfo.FacilityId, scheduleInfo.Day, scheduleInfo.StartDate, scheduleInfo.EndDate, scheduleInfo.BeginTime, scheduleInfo.EndTime);
        }

        private static async Task<bool> IsOverlapping(AppDbContext context, Guid facilityId, DaysInWeekEnum day, DateOnly startDate, DateOnly endDate, TimeOnly beginTime, TimeOnly endTime)
        {
            return await context.CourseSchedules
                .AnyAsync(cs => cs.FacilityId == facilityId &&
                                cs.Day == day &&
                                cs.StartDate <= endDate &&
                                cs.EndDate >= startDate &&
                                cs.BeginTime < endTime &&
                                cs.EndTime > beginTime);
        }
    }
}