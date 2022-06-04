using FinancialChat.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialChat.Data.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataConfiguration(this IServiceCollection services)
        {

            //services.AddScoped<IRevenueKindsRepository, RevenueKindsRepository>();

            services
                .AddEntityFrameworkContexts()
                .AddRepositories();

        }

        public static void UseDataConfiguration(this IApplicationBuilder app)
        {

        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //services.AddScoped<IRevenueKindsRepository, RevenueKindsRepository>();
            return services;
        }
        private static IServiceCollection AddEntityFrameworkContexts(this IServiceCollection services)
        {


            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            //});


            //var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;

            //if (isDevelopment)
            //{
            //    services.AddDbContext<ApplicationDbContext>(options =>
            //    {
            //        options.UseInMemoryDatabase(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            //    });
            //}
            //else
            //{
            //    //services.AddDbContext<ApplicationDbContext>(options =>
            //    //{
            //    //    options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            //    //});
            //}


            return services;
        }
    }
}
