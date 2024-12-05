using Api.Database;
using Api.enums;
using Api.Model.CourseSchedules;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.Tests
{
    public class ScheduleOverlap
    {
        private readonly ServiceProvider _serviceProvider;
        public ScheduleOverlap()
        {
            // Setup DI
            var services = new ServiceCollection();

            // Add your configuration sources here
            var configurationSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Register the DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configurationSetting["Environment:ConnectionStrings:DefaultConnection"]));

            // Add other necessary services
            services.AddSingleton<IConfiguration>(configurationSetting);

            // Build the ServiceProvider
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task AddCourseScheduleOverlap_Show_Throw_Error()
        {
            // Resolve dependencies using DI
            var dbContext = _serviceProvider.GetRequiredService<AppDbContext>();

            var coaches = InstancesGenerators.GenerateCoaches(5);
            var courses = InstancesGenerators.GenerateCourses(5);
            var facilities = InstancesGenerators.GenerateFacilities(5);

            // Use resolved dependencies
            await using (dbContext)
            {
                Helper.TruncateTables(dbContext);

                dbContext.Staffs.AddRange(coaches);
                dbContext.Courses.AddRange(courses);
                dbContext.Facilities.AddRange(facilities);

                await dbContext.SaveChangesAsync();

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

                await dbContext.SaveChangesAsync();

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

                // await Assert.ThrowsAsync<InvalidOperationException>(() => coach.AddCourseSchedule(dbContext, overlappingCourseSchedule));

            }
        }

        [Fact]
        public async Task AddCourseScheduleOverlap_Show_Not_Throw_Error()
        {
            // Resolve dependencies using DI
            var dbContext = _serviceProvider.GetRequiredService<AppDbContext>();

            var coaches = InstancesGenerators.GenerateCoaches(5);
            var courses = InstancesGenerators.GenerateCourses(5);
            var facilities = InstancesGenerators.GenerateFacilities(5);

            await using (dbContext)
            {
                Helper.TruncateTables(dbContext);

                dbContext.Staffs.AddRange(coaches);
                dbContext.Courses.AddRange(courses);
                dbContext.Facilities.AddRange(facilities);

                await dbContext.SaveChangesAsync();

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

                await dbContext.SaveChangesAsync();

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

                await dbContext.SaveChangesAsync();

                var schedulesCount = await dbContext.CourseSchedules.CountAsync();
                Assert.Equal(3, schedulesCount);
            }
        }
    }
}