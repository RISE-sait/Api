using Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests
{
    public class Helper
    {
        public static void TruncateTables(AppDbContext context)
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
    }
}