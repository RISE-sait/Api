using Api.Database;
using Api.enums;
using Api.Interfaces;
using Api.Model;
using Api.Model.Courses;
using Api.Model.People.Customers;
using Api.Model.People.Employees;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.Tests;

public class ModelTest()
{
    private static DbContextOptions<AppDbContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=root;Database=mydatabase")
            .Options;
    }

    private static void TruncateTables(AppDbContext context)
    {
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"Accounts\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"Admins\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"AdvancedAthleteInfo\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"BasicAthleteInfo\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"Coaches\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"CourseSchedules\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"Courses\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"CustomerFinancialInfo\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"Customers\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"Facilities\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"Families\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"FinancialInfo\" CASCADE");
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE public.\"SuperAdmins\" CASCADE");
    }

    private static List<Customer> GenerateCustomers(int count, List<Family>? familiesForRandPick, List<Customer.RolesEnum>? rolesForRandPick)
    {

        if (familiesForRandPick != null && rolesForRandPick != null)
        {
            return new Faker<Customer>()
            .CustomInstantiator(f =>
                new Customer(
                    f.Person.FullName,
                    f.Internet.Email(),
                    familiesForRandPick[new Random().Next(familiesForRandPick.Count)].Id,
                    rolesForRandPick[new Random().Next(rolesForRandPick.Count)],
                    f.Phone.PhoneNumber("##########")
                )).Generate(count);
        }

        return new Faker<Customer>()
            .CustomInstantiator(f =>
                new Customer(
                    f.Person.FullName,
                    f.Internet.Email(),
                    f.Phone.PhoneNumber("##########")
                )).Generate(count);
    }

    private static List<Family> GenerateFamilies(int amt)
    {
        List<Family> families = [];

        for (var i = 0; i < amt; i++)
        {
            families.Add(new Family());
        }

        return families;
    }

    private static List<Coach> GenerateCoaches(int amt)
    {
        return new Faker<Coach>()
            .CustomInstantiator(f =>
                new Coach(
                    f.Person.FullName,
                    f.Internet.Email(),
                    f.Phone.PhoneNumber("##########"),
                    f.Finance.Account()
                )).Generate(amt);
    }

    private static List<Course> GenerateCourses(int count)
    {
        return new Faker<Course>()
            .CustomInstantiator(f =>
                new Course(
                    f.Commerce.ProductName(),
                    DateOnly.FromDateTime(f.Date.Future()),
                    DateOnly.FromDateTime(f.Date.Past())
                ))
            .Generate(count);
    }

    private static List<Admin> GenerateAdmins(int count)
    {
        return new Faker<Admin>().CustomInstantiator(f =>
            new Admin(
                f.Person.FullName,
                f.Internet.Email(),
                f.Phone.PhoneNumber("##########"),
                    f.Finance.Account()
            )).Generate(count);
    }

    private static List<Facility> GenerateFacilities(int count)
    {
        return new Faker<Facility>()
            .CustomInstantiator(f =>
                new Facility(
                    f.Address.City(),
                    f.Company.CompanyName()
                ))
            .Generate(count);
    }

    [Fact]
    public async Task AddAdmin_ShouldCreateAdminInDatabase()
    {
        var options = GetDbContextOptions();

        var admins = GenerateAdmins(5);

        await using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            await context.Admins.AddRangeAsync(admins); // Add the generated Admins to the context
            await context.SaveChangesAsync(); // Save changes to the database

            var adminCount = await context.Accounts.CountAsync(); // Count the number of Admins
            Assert.Equal(5, adminCount); // Ensure there are exactly 5 Admins
        }
    }

    [Fact]
    public async Task AddFinancialInfo_ShouldCreateFinancialInfoInDatabase()
    {
        var options = GetDbContextOptions();

        var financialInfos = new Faker<FinancialInfo>()
            .CustomInstantiator(f =>
                new FinancialInfo(f.Finance.Account()))
            .Generate(5);

        await using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            await context.FinancialInfo.AddRangeAsync(financialInfos);
            await context.SaveChangesAsync();

            var financialInfoCount = await context.FinancialInfo.CountAsync();
            Assert.Equal(5, financialInfoCount);
        }
    }

    [Fact]
    public void AddFacility_ShouldCreateFacilityInDatabase()
    {
        var options = GetDbContextOptions();

        var facilities = GenerateFacilities(5);

        using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            context.Facilities.AddRange(facilities);
            context.SaveChanges();

            var facilityCount = context.Facilities.Count();
            Assert.Equal(5, facilityCount);
        }
    }

    [Fact]
    public void AddCustomer_ShouldCreateCustomerInDatabase_No_Family()
    {
        var options = GetDbContextOptions();

        var customers = GenerateCustomers(5, null, null);

        using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            context.Customers.AddRange(customers);
            context.SaveChanges(); // Save changes to the database

            var customerCount = context.Customers.Count(); // Count the number of customers
            var accountsCount = context.Accounts.Count(); // Count the number of customers

            Assert.Equal(5, customerCount); // Ensure there are exactly 5 Customers
            Assert.Equal(5, accountsCount); // Ensure there are exactly 5 Customers
        }
    }

    [Fact]
    public async Task AddFamily_ShouldCreateFamilyInDatabase()
    {
        var options = GetDbContextOptions();

        await using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            var families = GenerateFamilies(5);

            context.Families.AddRange(families);

            context.SaveChanges();

            var familiesCount = await context.Families.CountAsync(); // Count the number of families

            Assert.Equal(5, familiesCount); // Ensure there are exactly 5 families
        }
    }

    [Fact]
    public async Task AddCustomers_With_Family_ShouldCreate_Customers_And_Family_And_Accounts_InDatabase()
    {
        var options = GetDbContextOptions();

        await using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            var families = GenerateFamilies(5);

            context.Families.AddRange(families);

            List<Customer.RolesEnum> roles = [Customer.RolesEnum.Child, Customer.RolesEnum.Parent];

            var customers = GenerateCustomers(20, families, roles);

            context.Customers.AddRange(customers);

            context.SaveChanges();

            var accountsCount = await context.Customers.CountAsync(); // Count the number of customers

            Assert.Equal(20, customers.Count); // Ensure there are exactly 5 Customers
            Assert.Equal(20, accountsCount); // Ensure there are exactly 5 Customers
            Assert.Equal(5, families.Count);
        }
    }

    [Fact]
    public async Task AddCoaches_ShouldCreateCoachesInDatabase()
    {
        var options = GetDbContextOptions();

        await using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            var coaches = GenerateCoaches(5);

            await context.Coaches.AddRangeAsync(coaches);

            await context.SaveChangesAsync();

            var coachesCount = await context.Coaches.CountAsync(); // Count the number of coaches
            var accountsCount = await context.Accounts.CountAsync(); // Count the number of coaches

            Assert.Equal(5, coachesCount); // Ensure there are exactly 5 coaches
            Assert.Equal(5, accountsCount); // Ensure there are exactly 5 accounts
        }
    }

    [Fact]
    public async Task AddBasicAthleteInfo_ShouldCreateBasicAthleteInfoInDatabase()
    {
        var options = GetDbContextOptions();

        var customers = GenerateCustomers(5, null, null);
        var basicAthleteInfos = customers.Select(c => new BasicAthleteInfo(c.Id) { Customer = c }).ToList();

        await using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            await context.Customers.AddRangeAsync(customers);
            await context.BasicAthleteInfo.AddRangeAsync(basicAthleteInfos);
            await context.SaveChangesAsync();

            var basicAthleteInfoCount = await context.BasicAthleteInfo.CountAsync();
            Assert.Equal(5, basicAthleteInfoCount);
        }
    }

    [Fact]
    public void AddAdvancedAthleteInfo_ShouldCreateAdvancedAthleteInfoInDatabase()
    {
        var options = GetDbContextOptions();

        var customers = GenerateCustomers(5, null, null);

        using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            var advancedAthleteInfos = customers.Select(c => new AdvancedAthleteInfo(c.Id)).ToList();

            context.AdvancedAthleteInfo.AddRange(advancedAthleteInfos);
            context.SaveChanges();

            var advancedAthleteInfoCount = context.AdvancedAthleteInfo.Count();
            Assert.Equal(5, advancedAthleteInfoCount);
        }
    }

    [Fact]
    public async Task AddCourse_ShouldCreateCourseInDatabase()
    {
        var options = GetDbContextOptions();

        var courses = GenerateCourses(5);

        await using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();

            var courseCount = await context.Courses.CountAsync();
            Assert.Equal(5, courseCount);
        }
    }

    [Fact]
    public async Task AddCourseSchedule_ShouldWorkCorrectly()
    {
        var options = GetDbContextOptions();

        var coaches = GenerateCoaches(5);
        var courses = GenerateCourses(5);
        var facilities = GenerateFacilities(5);

        await using (var context = new AppDbContext(options))
        {
            TruncateTables(context);

            context.Coaches.AddRange(coaches);
            context.Courses.AddRange(courses);
            context.Facilities.AddRange(facilities);

            await context.SaveChangesAsync();

            ICoach coach = coaches[0];

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

            await coach.AddCourseSchedule(context, courseSchedule1);
            await coach.AddCourseSchedule(context, courseSchedule2);

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

            await Assert.ThrowsAsync<InvalidOperationException>(() => coach.AddCourseSchedule(context, overlappingCourseSchedule));

        }
    }
}