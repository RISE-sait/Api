using Api.Model;
using Api.Model.Courses;
using Api.Model.CourseSchedules;
using Api.Model.Facilities;
using Api.Model.People.Staff;
using Microsoft.EntityFrameworkCore;

namespace Api.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses { get; init; }
        public DbSet<CourseSchedule> CourseSchedules { get; init; }
        public DbSet<Staff> Staffs { get; init; }
        public DbSet<StaffType> StaffTypes { get; init; }
        public DbSet<BasicAthleteInfo> BasicAthleteInfo { get; init; }
        public DbSet<AdvancedAthleteInfo> AdvancedAthleteInfo { get; init; }

        public DbSet<Facility> Facilities { get; init; }
        public DbSet<FacilityType> FacilityTypes { get; init; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONN_STRING");

            optionsBuilder.UseNpgsql(dbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseSchedule>().HasKey(cs =>
               new
               {
                   cs.CourseId,
                   cs.StartDate,
                   cs.FacilityId,
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

            modelBuilder.Entity<FacilityType>(entity =>
       {
           entity.HasKey(ft => ft.Id);
           entity.HasIndex(ft => ft.Name).IsUnique();
       });
        }

        public static DbContextOptions<AppDbContext> GetLocalDbContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=root;Database=mydatabase")
                .Options;
        }
    }
}