using Api.Model.Courses;
using Api.Model.Facilities;
using Api.Model.People.Customers;
using Api.Model.People.Employees;
using Bogus;

namespace Api.Tests
{
    public static class InstancesGenerators
    {
        public static List<Customer> GenerateCustomers(int count, List<Family>? familiesForRandPick, List<Customer.RolesEnum>? rolesForRandPick)
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

        public static List<Family> GenerateFamilies(int amt)
        {
            List<Family> families = [];

            for (var i = 0; i < amt; i++)
            {
                families.Add(new Family());
            }

            return families;
        }

        public static List<Coach> GenerateCoaches(int amt)
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

        public static List<Course> GenerateCourses(int count)
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

        public static List<Admin> GenerateAdmins(int count)
        {
            return new Faker<Admin>().CustomInstantiator(f =>
                new Admin(
                    f.Person.FullName,
                    f.Internet.Email(),
                    f.Phone.PhoneNumber("##########"),
                        f.Finance.Account()
                )).Generate(count);
        }

        public static List<FacilityType> GenerateFacilityTypes(int count)
        {
            return new Faker<FacilityType>().CustomInstantiator(f =>
                new FacilityType(
                    f.Address.City()
                )).Generate(count);
        }

        public static List<Facility> GenerateFacilities(int count)
        {
            var facilitiesTypes = GenerateFacilityTypes(count);

            return new Faker<Facility>()
                .CustomInstantiator(f =>
                    new Facility(
                        f.Address.City(),
                        f.Company.CompanyName(),
                        facilitiesTypes[new Random().Next(facilitiesTypes.Count)].Id
                    ))
                .Generate(count);
        }
    }
}