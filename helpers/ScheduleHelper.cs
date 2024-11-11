using Api.Database;
using Api.enums;
using Api.Interfaces;
using Api.Model.Courses;
using Microsoft.EntityFrameworkCore;

namespace Api.helpers
{
    public static class ScheduleHelper
    {
        public static async Task<bool> IsScheduleOverlapping(AppDbContext context, CourseSchedule courseSchedule)
        {
            return await context.CourseSchedules
                .AnyAsync(cs => cs.FacilityId == courseSchedule.FacilityId && cs.Day == courseSchedule.Day &&
                                cs.BeginTime < courseSchedule.EndTime && cs.EndTime > courseSchedule.BeginTime);
        }

        public static async Task<bool> IsBookingOverlapping(AppDbContext context, BookingInfo bookingInfo)
        {
            var day = (DaysInWeekEnum)bookingInfo.StartDateTime.DayOfWeek;
            var beginTime = TimeOnly.FromDateTime(bookingInfo.StartDateTime);
            var endTime = TimeOnly.FromDateTime(bookingInfo.EndDateTime);

            return await context.CourseSchedules
                .AnyAsync(cs => cs.FacilityId == bookingInfo.FacilityId && cs.Day == day &&
                                cs.BeginTime < endTime && cs.EndTime > beginTime);
        }
    }
}