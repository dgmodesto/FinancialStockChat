using FinancialChat.Application.Extensions;
using FinancialChat.Integration.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialChat.IoC
{
    public static class DependencyInjectionConfiguration
    {

        public static void BuildConfiguration(this IServiceCollection services)
        {
            services.AddApplicationConfiguration();
            services.AddIntegrationConfiguration();
        }


        public static void BuildAppConfigure(this IApplicationBuilder app)
        {
            
        }
    }
}