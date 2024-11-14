using Api.Database;
using Api.enums;
using Api.Model.Courses;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.Tests
{
    public class ScheduleOverlap
    {

        [Fact]
        public async Task AddCourseScheduleOverlap_Show_Throw_Error()
        {
            var options = AppDbContext.GetLocalDbContextOptions();

            var coaches = InstancesGenerators.GenerateCoaches(5);
            var courses = InstancesGenerators.GenerateCourses(5);
            var facilities = InstancesGenerators.GenerateFacilities(5);

            await using (var context = new AppDbContext(options))
            {
                Helper.TruncateTables(context);

                context.Coaches.AddRange(coaches);
                context.Courses.AddRange(courses);
                context.Facilities.AddRange(facilities);

                await context.SaveChangesAsync();
                
                // Add non-overlapping course schedules
                var courseSchedule1 = new CourseSchedule(
                    courses[0].Id,
                    facilities[0].Id,
                    DaysInWeekEnum.M,
                    DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                    new TimeOnly(9, 0),
                    new TimeOnly(9, 59),
                    coaches[0].Id
                );

                var courseSchedule2 = new CourseSchedule(
                    courses[1].Id,
                    facilities[0].Id,
                    DaysInWeekEnum.M,
                    DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                    new TimeOnly(10, 0),
                    new TimeOnly(10, 59),
                    coaches[1].Id
                );

                // await coach.AddCourseSchedule(context, courseSchedule1);
                // await coach.AddCourseSchedule(context, courseSchedule2);

                await context.SaveChangesAsync();

                // Attempt to add an overlapping course schedule
                var overlappingCourseSchedule = new CourseSchedule(
                    courses[2].Id,
                    facilities[0].Id,
                    DaysInWeekEnum.M,
                    DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                    new TimeOnly(10, 40),
                    new TimeOnly(12, 30),
                    coaches[2].Id
                );

                // await Assert.ThrowsAsync<InvalidOperationException>(() => coach.AddCourseSchedule(context, overlappingCourseSchedule));

            }
        }

        [Fact]
        public async Task AddCourseScheduleOverlap_Show_Not_Throw_Error()
        {
            var options = AppDbContext.GetLocalDbContextOptions();

            var coaches = InstancesGenerators.GenerateCoaches(5);
            var courses = InstancesGenerators.GenerateCourses(5);
            var facilities = InstancesGenerators.GenerateFacilities(5);

            await using (var context = new AppDbContext(options))
            {
                Helper.TruncateTables(context);

                context.Coaches.AddRange(coaches);
                context.Courses.AddRange(courses);
                context.Facilities.AddRange(facilities);

                await context.SaveChangesAsync();

                // ICoach coach = coaches[0];

                // Add non-overlapping course schedules
                var courseSchedule1 = new CourseSchedule(
                    courses[0].Id,
                    facilities[0].Id,
                    DaysInWeekEnum.M,
                    DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                    new TimeOnly(9, 0),
                    new TimeOnly(9, 59),
                    coaches[0].Id
                );

                var courseSchedule2 = new CourseSchedule(
                    courses[1].Id,
                    facilities[0].Id,
                    DaysInWeekEnum.M,
                    DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                    new TimeOnly(10, 0),
                    new TimeOnly(10, 59),
                    coaches[1].Id
                );

                // await coach.AddCourseSchedule(context, courseSchedule1);
                // await coach.AddCourseSchedule(context, courseSchedule2);

                await context.SaveChangesAsync();

                // Attempt to add a non-overlapping course schedule
                var newCourseSchedule = new CourseSchedule(
                    courses[2].Id,
                    facilities[0].Id,
                    DaysInWeekEnum.M,
                    DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                    new TimeOnly(11, 0),
                    new TimeOnly(12, 30),
                    coaches[2].Id
                );

                // await coach.AddCourseSchedule(context, newCourseSchedule);

                await context.SaveChangesAsync();

                var schedulesCount = await context.CourseSchedules.CountAsync();
                Assert.Equal(3, schedulesCount);
            }
        }
    }
}