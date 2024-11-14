using Api.Database;
using Api.Model;
using Api.Model.People.Customers;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.Tests;

public class ModelTest()
{
    [Fact]
    public async Task AddAdmin_ShouldCreateAdminInDatabase()
    {
        var options = AppDbContext.GetLocalDbContextOptions();

        var admins = InstancesGenerators.GenerateAdmins(5);

        await using (var context = new AppDbContext(options))
        {
            Helper.TruncateTables(context);

            await context.Admins.AddRangeAsync(admins); // Add the generated Admins to the context
            await context.SaveChangesAsync(); // Save changes to the database

            var adminCount = await context.Accounts.CountAsync(); // Count the number of Admins
            Assert.Equal(5, adminCount); // Ensure there are exactly 5 Admins
        }
    }

    [Fact]
    public void AddFacility_ShouldCreateFacilityInDatabase()
    {
        var options = AppDbContext.GetLocalDbContextOptions();

        var facilities = InstancesGenerators.GenerateFacilities(5);

        using (var context = new AppDbContext(options))
        {
            Helper.TruncateTables(context);

            context.Facilities.AddRange(facilities);
            context.SaveChanges();

            var facilityCount = context.Facilities.Count();
            Assert.Equal(5, facilityCount);
        }
    }

    [Fact]
    public void AddCustomer_ShouldCreateCustomerInDatabase_No_Family()
    {
        var options = AppDbContext.GetLocalDbContextOptions();

        var customers = InstancesGenerators.GenerateCustomers(5, null, null);

        using (var context = new AppDbContext(options))
        {
            Helper.TruncateTables(context);

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
        var options = AppDbContext.GetLocalDbContextOptions();

        await using (var context = new AppDbContext(options))
        {
            Helper.TruncateTables(context);

            var families = InstancesGenerators.GenerateFamilies(5);

            context.Families.AddRange(families);

            context.SaveChanges();

            var familiesCount = await context.Families.CountAsync(); // Count the number of families

            Assert.Equal(5, familiesCount); // Ensure there are exactly 5 families
        }
    }

    [Fact]
    public async Task AddCustomers_With_Family_ShouldCreate_Customers_And_Family_And_Accounts_InDatabase()
    {
        var options = AppDbContext.GetLocalDbContextOptions();

        await using (var context = new AppDbContext(options))
        {
            Helper.TruncateTables(context);

            var families = InstancesGenerators.GenerateFamilies(5);

            context.Families.AddRange(families);

            List<Customer.RolesEnum> roles = [Customer.RolesEnum.Child, Customer.RolesEnum.Parent];

            var customers = InstancesGenerators.GenerateCustomers(20, families, roles);

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
        var options = AppDbContext.GetLocalDbContextOptions();

        await using (var context = new AppDbContext(options))
        {
            Helper.TruncateTables(context);

            var coaches = InstancesGenerators.GenerateCoaches(5);

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
        var options = AppDbContext.GetLocalDbContextOptions();

        var customers = InstancesGenerators.GenerateCustomers(5, null, null);
        var basicAthleteInfos = customers.Select(c => new BasicAthleteInfo(c.Id) { Customer = c }).ToList();

        await using (var context = new AppDbContext(options))
        {
            Helper.TruncateTables(context);

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
        var options = AppDbContext.GetLocalDbContextOptions();

        var customers = InstancesGenerators.GenerateCustomers(5, null, null);

        using (var context = new AppDbContext(options))
        {
            Helper.TruncateTables(context);

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
        var options = AppDbContext.GetLocalDbContextOptions();

        var courses = InstancesGenerators.GenerateCourses(5);

        await using (var context = new AppDbContext(options))
        {
            Helper.TruncateTables(context);

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();

            var courseCount = await context.Courses.CountAsync();
            Assert.Equal(5, courseCount);
        }
    }
}