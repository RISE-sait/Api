using Api.Model;
using Api.Model.Courses;
using Api.Model.People;
using Api.Model.People.Customers;
using Api.Model.People.Employees;
using dotenv.net;
using Microsoft.EntityFrameworkCore;

namespace Api.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses { get; init; }
        public DbSet<CourseSchedule> CourseSchedules { get; init; }
        public DbSet<Family> Families { get; init; }
        public DbSet<Admin> Admins { get; init; }
        public DbSet<SuperAdmin> SuperAdmins { get; init; }
        public DbSet<Coach> Coaches { get; init; }
        public DbSet<Customer> Customers { get; init; }
        public DbSet<Account> Accounts { get; init; }

        public DbSet<BasicAthleteInfo> BasicAthleteInfo { get; init; }
        public DbSet<AdvancedAthleteInfo> AdvancedAthleteInfo { get; init; }

        public DbSet<Facility> Facilities { get; init; }
        public DbSet<FinancialInfo> FinancialInfo { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var shouldUseLocal = true;

            // var dbConnectionString = Environment.GetEnvironmentVariable($"DB_URL_FOR_SERVER_ON_{(shouldUseLocal ? "LOCAL" : "DOCKER")}_NETWORK");

            // Console.WriteLine(dbConnectionString);

            optionsBuilder.UseNpgsql($"Host={(shouldUseLocal ? "localhost" : "api_db")};Port=5432;Username=postgres;Password=root;Database=mydatabase");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Admin>().ToTable("Admins");

            modelBuilder.Entity<Coach>().ToTable("Coaches");

            modelBuilder.Entity<Customer>().HasMany(c => c.FinancialInfos)
                .WithMany(fi => fi.Customers)
                .UsingEntity(j => j.ToTable("CustomerFinancialInfo"));

            modelBuilder.Entity<Family>().HasMany(f => f.Members)
                    .WithOne(m => m.Family)
                    .HasForeignKey(m => m.FamilyId)
                    .IsRequired(false);

            modelBuilder.Entity<CourseSchedule>().HasKey(cs =>
               new
               {
                   cs.CourseId,
                   cs.Location,
                   cs.BeginTime,
               });

            modelBuilder.Entity<BasicAthleteInfo>().HasOne(ai => ai.Customer)
            .WithOne(c => c.BasicAthleteInfo)
            .HasForeignKey<BasicAthleteInfo>(ai => ai.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdvancedAthleteInfo>().HasOne(ai => ai.Customer)
                .WithOne(c => c.AdvancedAthleteInfo)
                .HasForeignKey<AdvancedAthleteInfo>(ai => ai.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}