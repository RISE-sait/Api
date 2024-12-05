using Stripe;

namespace Api.Services
{
    public static class StripeService
    {
         public static void AddStripeServices(this IServiceCollection services, IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
        }
    }
}